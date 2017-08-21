using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;
namespace App.Dispatching.Process
{
    public class ElevatorProcess : AbstractProcess
    {        
        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();

        private Timer tmWorkTimer = new Timer();
        //private string WarehouseCode = "";
        private bool blRun = false;
        private DataTable dtDeviceAlarm;
        Report report = new Report();

        public override void Initialize(Context context)
        {
            try
            {
                dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag in(2,3)") });                

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);

                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("ElevatorProcess提升机初始化出错，原因：" + ex.Message);
            }
        }
        #region StateChanged
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            switch (stateItem.ItemName)
            {
                case "TaskFinished01":
                case "TaskFinished02":
                    object[] obj = ObjectUtil.GetObjects(stateItem.State);
                    
                    if (obj == null)
                        return;
                    string TaskNo = ConvertStringChar.BytesToString(obj);
                    {                
                        //存储过程处理
                        if (TaskNo.Length > 0)
                        {
                            byte[] b = new byte[30];
                            ConvertStringChar.stringToByte("", 30).CopyTo(b, 0);
                            WriteToService(stateItem.Name, stateItem.ItemName, b);

                            Logger.Info(stateItem.ItemName + "完成标志,任务号:" + TaskNo);
                            DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                            bll.ExecNonQueryTran("WCS.Sp_TaskProcess", param);
                        }
                        //上报总控WCS，下架完成

                    }
                    break;
                case "CarAlarm01":
                case "CarAlarm02":
                    object obj1 = ObjectUtil.GetObject(stateItem.State);
                    if (obj1 == null)
                        return;
                    if (obj1.ToString() != "0")
                    {
                        string strError = "";
                        DataRow[] drs = dtDeviceAlarm.Select(string.Format("Flag=2 and AlarmCode={0}", obj1.ToString()));
                        if (drs.Length > 0)
                            strError = drs[0]["AlarmCode"].ToString();
                        else
                            strError = "穿梭车未知错误！";
                        Logger.Error(strError);
                    }
                    break;
                case "ElevatorAlarm":
                    object obj2 = ObjectUtil.GetObject(stateItem.State);
                    if (obj2 == null)
                        return;
                    if (obj2.ToString() != "0")
                    {
                        string strError = "";
                        DataRow[] drs = dtDeviceAlarm.Select(string.Format("Flag=3 and AlarmCode={0}", obj2.ToString()));
                        if (drs.Length > 0)
                            strError = drs[0]["AlarmCode"].ToString();
                        else
                            strError = "提升机未知错误！";
                        Logger.Error(strError);
                    }
                    break;
                case "Run":
                    blRun = (int)stateItem.State == 1;
                    if (blRun)
                    {
                        tmWorkTimer.Start();
                        Logger.Info("提升机联机");
                    }
                    else
                    {
                        tmWorkTimer.Stop();
                        Logger.Info("提升机脱机");
                    }
                    break;
                default:
                    break;
            }


            return;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!blRun)
                {
                    tmWorkTimer.Stop();
                    return;
                }
                tmWorkTimer.Stop();

                DataTable dtAisle = bll.FillDataTable("CMD.SelectAisleElevator", new DataParameter[] { new DataParameter("{0}", string.Format("WarehoseCode='{0}'", Program.WarehouseCode)) });

                for (int i = 0; i < dtAisle.Rows.Count; i++)
                {
                    string serviceName = dtAisle.Rows[i]["ServiceName2"].ToString();
                    //读取标识位，如果为0才可继续
                    object[] objFlag = ObjectUtil.GetObjects(WriteToService(serviceName, "WriteFinished"));
                    if (int.Parse(objFlag.ToString()) == 1)
                        continue;
                    string AisleNo = dtAisle.Rows[i]["AisleNo"].ToString();
                    string filter = string.Format("WarehoseCode='{0}' and AisleNo='{1}'", Program.WarehouseCode, AisleNo);
                    DataTable dtTask = GetTask(AisleNo);
                    DataTable dtCar = bll.FillDataTable("CMD.SelectAisleCar", new DataParameter[] { new DataParameter("{0}", filter) });

                    bool IsSent = false;
                    object task = dtTask.Compute("count(*)", "TaskType='11' and State in('1','2')");
                    int taskCount = int.Parse(task.ToString());
                    if (taskCount >= 2)
                    {
                        //先找入库任务
                        IsSent = FindInTask(dtCar, dtTask);
                    }

                    //对于入库任务来说，因为一个巷道同时只有一个任务，所以应该优先以任务目标层找对应层空闲的小车

                    for (int j = 0; j < dtCar.Rows.Count; j++)
                    {
                        //读取小车状态
                        string carNo = dtCar.Rows[j]["DeviceNo2"].ToString().Substring(2,2);
                        object[] obj = ObjectUtil.GetObjects(WriteToService(serviceName, "CarStatus" + carNo));
                        int Layer = int.Parse(obj[3].ToString());
                        int Column = int.Parse(obj[2].ToString());

                        //如果小车空闲
                        //先找小车当前层出库任务，判断入库任务是否已经超过2个，如果超过先入，再找其他出库任务，再找入库任务
                        if (Check_Car_Status_IsOk(carNo, serviceName))
                        {
                            IsSent = false;
                            
                            //先找出库任务
                            IsSent = FindOutTask(dtCar, dtTask, carNo, obj);
                            if (IsSent)
                                continue;
                            //再找入库任务
                            IsSent = FindInTask(dtCar, dtTask);
                            if (IsSent)
                                continue;
                            //如果车空闲，直接退到当前层的1列
                            if (!IsSent && Column <= 0)
                            {
                                //小车空闲，需要避开
                                //找出其他车所在层以及目标层
                                //如果其他车都空闲，则不需要避开
                                //if(GetOtherCarNoTask(dt,carNo))
                                //    return;

                                int ToLayer = GetNoTaskLayer(serviceName,dtCar, carNo, Layer);
                                if (ToLayer > 0)
                                {
                                    DataRow dr = dtTask.NewRow();
                                    dr["TaskNo"] = "001";
                                    dr["TaskType"] = "10";
                                    dr["FromAddress"] = "S000000000";
                                    dr["ToStation"] = "S001002" + (1000 + ToLayer).ToString().Substring(1, 3);
                                    Send2PLC(serviceName, dr,carNo);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        //获取小车让车可去的空闲的层
        private int GetNoTaskLayer(string serviceName,DataTable dtCar, string carNo, int carLayer)
        {
            int NoTaskLayer = carLayer;
            if (!IsCurrentLayerOK(serviceName, dtCar, carNo, carLayer))
                return NoTaskLayer;

            for (int k = 1; k < 11; k++)
            {
                NoTaskLayer = k;
                if (IsCurrentLayerOK(serviceName, dtCar, carNo, k))
                    continue;
                else
                    break;                
            }
            return NoTaskLayer;
        }
        private bool IsCurrentLayerOK(string serviceName, DataTable dtCar, string carNo, int carLayer)
        {
            bool isExist = false;
            //优先判断小车当前层是否可行
            for (int i = 0; i < dtCar.Rows.Count; i++)
            {
                string CarNo = dtCar.Rows[i]["DeviceNo2"].ToString().Substring(2, 2);
                {
                    if (CarNo != carNo)
                    {
                        //读取小车状态
                        object[] obj = ObjectUtil.GetObjects(WriteToService(serviceName, "CarStatus" + CarNo));

                        int Layer = int.Parse(obj[3].ToString());
                        int FromLayer = int.Parse(obj[6].ToString());
                        int FromColumn = int.Parse(obj[5].ToString());
                        int ToLayer = int.Parse(obj[9].ToString());
                        int Column = int.Parse(obj[2].ToString());
                        int ToColumn = int.Parse(obj[8].ToString());

                        if (FromLayer == carLayer || ToLayer == carLayer || Layer == carLayer)
                            isExist = true;

                        if (isExist)
                            break;
                    }
                }
            }
            return isExist;
        }
        private bool FindInTask(DataTable dtCar, DataTable dtTask)
        {
            int ToLayer = 0;
            bool IsSendTask = false;
            string filter = string.Format("TaskType in ('11','16','14') and State='2'");
            DataRow[] drTasks = dtTask.Select(filter, "TaskLevel DESC,RequestDate,StartDate");
            if (drTasks.Length > 0)
                ToLayer = int.Parse(drTasks[0]["ToLayer"].ToString());                
            
            //根据入库任务的目标层，优先给目标层的小车下任务
            for (int i = 0; i < dtCar.Rows.Count; i++)
            {
                //读取小车状态
                string serviceName = dtCar.Rows[i]["ServiceName"].ToString();
                string carNo = dtCar.Rows[i]["DeviceNo2"].ToString().Substring(2, 2);
                object[] obj = ObjectUtil.GetObjects(WriteToService(serviceName, "CarStatus" + carNo));
                int Layer = int.Parse(obj[3].ToString());
                int Column = int.Parse(obj[2].ToString());

                //如果小车空闲  
                if (Layer == ToLayer)
                {
                    if (Check_Car_Status_IsOk(carNo, serviceName))
                    {
                        IsSendTask = SendTask(dtCar, carNo, drTasks, obj);
                    }
                    break;
                }
            }
            if (!IsSendTask)
            {
                for (int i = 0; i < dtCar.Rows.Count; i++)
                {
                    //读取小车状态
                    string serviceName = dtCar.Rows[i]["ServiceName"].ToString();
                    string carNo = dtCar.Rows[i]["DeviceNo2"].ToString().Substring(2, 2);
                    object[] obj = ObjectUtil.GetObjects(WriteToService(serviceName, "CarStatus" + carNo));
                    int Layer = int.Parse(obj[3].ToString());
                    int Column = int.Parse(obj[2].ToString());

                    //如果小车空闲  

                    if (Check_Car_Status_IsOk(carNo, serviceName))
                    {
                        IsSendTask = SendTask(dtCar, carNo, drTasks, obj);
                    }
                    break;
                }
            }
            return IsSendTask;
        }
        private bool FindInTask(DataTable dtCar, DataTable dtTask, string carNo, object[] obj)
        {
            int carLayer = int.Parse(obj[3].ToString());
            bool IsSendTask = false;
            string filter = string.Format("TaskType in ('11','16','14') and State='2' and ToLayer={0}", carLayer);
            DataRow[] drTasks = dtTask.Select(filter, "TaskLevel DESC,RequestDate,StartDate");
            if (drTasks.Length > 0)
            {
                IsSendTask = SendTask(dtCar, carNo, drTasks, obj);
            }
            if (!IsSendTask)
            {
                //再找不在这层的入库任务
                filter = string.Format("TaskType in ('11','16','14') and State='2'");
                drTasks = dtTask.Select(filter, "TaskLevel DESC,RequestDate,StartDate");

                IsSendTask = SendTask(dtCar, carNo, drTasks, obj);

            }
            return IsSendTask;
        }
        private bool FindOutTask(DataTable dtCar, DataTable dtTask, string carNo, object[] obj)
        {
            int carLayer = int.Parse(obj[3].ToString());
            bool IsSendTask = false;
            string filter = string.Format("TaskType in ('12','13','14',15) and State='0' and FromLayer={0}", carLayer);
            DataRow[] drTasks = dtTask.Select(filter, "TaskLevel DESC,RequestDate,StartDate");
            if (drTasks.Length > 0)
            {
                IsSendTask = SendTask(dtCar, carNo, drTasks, obj);
            }
            if (!IsSendTask)
            {
                //再找不在这层的出库任务
                filter = string.Format("TaskType in ('12','13','14',15) and State='0'");
                drTasks = dtTask.Select(filter, "TaskLevel DESC,RequestDate,StartDate");

                IsSendTask = SendTask(dtCar, carNo, drTasks, obj);                
            }
            return IsSendTask;
        }
        private bool SendTask(DataTable dtCar, string carNo, DataRow[] drTasks, object[] obj)
        {
            string serviceName = dtCar.Rows[0]["ServiceName"].ToString();
            bool IsSend = false;
            for (int i = 0; i < drTasks.Length; i++)
            {
                DataRow drTask = drTasks[i];
                //轮询其他小车
                if (CheckOtherCarStatus(dtCar, carNo, drTask, obj))
                {
                    //给小车下达任务
                    Send2PLC(serviceName, drTask, carNo);
                    IsSend = true;
                    break;
                }
            }
            return IsSend;
        }

        /// <summary>
        /// 判断能否给小车下任务
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="carNo"></param>
        /// <param name="carToLayer"></param>
        /// <returns></returns>
        private bool CheckOtherCarStatus(DataTable dtCar, string carNo, DataRow drTask, object[] carobj)
        {
            int carLayer = int.Parse(carobj[3].ToString());
            int carColumn = int.Parse(carobj[2].ToString());
            string carTaskType = drTask["TaskType"].ToString();
            int carFromLayer = int.Parse(drTask["FromLayer"].ToString());
            int carToLayer = int.Parse(drTask["ToLayer"].ToString());

            bool carOK = true;
            for (int i = 0; i < dtCar.Rows.Count; i++)
            {
                string DeviceNo = dtCar.Rows[i]["DeviceNo"].ToString();
                string serviceName = dtCar.Rows[i]["ServiceName"].ToString();
                if (DeviceNo != carNo)
                {
                    //读取小车状态
                    object[] obj = ObjectUtil.GetObjects(WriteToService(serviceName, "CarStatus" + DeviceNo.Substring(2,2)));
                    object[] obj1 = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished"));
                    object[] obj2 = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress"));
                    int TaskFlag = int.Parse(obj1[0].ToString());
                    int OpMode = int.Parse(obj[0].ToString());

                    int Column = int.Parse(obj[2].ToString());
                    int Layer = int.Parse(obj[3].ToString());
                    int FromColumn = int.Parse(obj[5].ToString());
                    int FromLayer = int.Parse(obj[6].ToString());
                    int ToLayer = int.Parse(obj[9].ToString());
                    int ToColumn = int.Parse(obj[5].ToString());
                    int TaskType = int.Parse(obj[12].ToString());

                    //防止读到PLC目标层不准，读到标识还没移走，那么读上位机下发的目标层
                    if (TaskFlag == 1)
                        ToLayer = int.Parse(obj2[8].ToString());
                    //Logger.Debug("小车" + dicCars[i].CarNo + "目标层" + ToLayer + "当前层:" + Layer + ",任务类型:" + TaskType);

                    //入库类型
                    if (carTaskType == "11")
                    {
                        //1入库站台不能有车
                        if (Layer == 1 && Column == 0)
                        {
                            carOK = false;
                            break;
                        }
                        //2其他车任务起始层也在这层，但车不在这层
                        if (carFromLayer == FromLayer && Layer > 1 && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }
                        //3其他车任务目标层在起始层 出库
                        if (carFromLayer == ToLayer && carColumn > 0 && TaskType == 11)
                        {
                            carOK = false;
                            break;
                        }
                        //4其他车任务目标层在起始层
                        if (carFromLayer == ToLayer && carColumn > 0 && Layer > 1 && TaskType == 9)
                        {
                            carOK = false;
                            break;
                        }

                        //4目标层有车
                        if (carToLayer == Layer)
                        {
                            carOK = false;
                            break;
                        }
                        //6目标层有任务起始层
                        if (carToLayer == FromLayer)
                        {
                            carOK = false;
                            break;
                        }
                    }
                    if (carTaskType == "12")
                    {
                        //1站台不能有车
                        if (Layer == 1 && Column == 0)
                        {
                            carOK = false;
                            break;
                        }
                        //起始层有车
                        if (carFromLayer == Layer)
                        {
                            carOK = false;
                            break;
                        }
                        //其他车任务起始层也在这层 
                        if (carFromLayer == FromLayer && carFromLayer != carLayer && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }
                        //其他车任务目标层在起始层 
                        if (carFromLayer == ToLayer && ToColumn > 0 && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }

                        //目标层有车
                        if (carToLayer == Layer && Column == 0)
                        {
                            carOK = false;
                            break;
                        }

                        //目标层有任务起始层
                        //if (FromLayer == 1 && FromColumn == 0)
                        //{
                        //    carOK = false;
                        //    break;
                        //}
                    }
                    if (carTaskType == "13")
                    {
                        //1站台不能有车
                        if (Layer == 1 && Column == 0)
                        {
                            carOK = false;
                            break;
                        }
                        //起始层有车
                        if (carFromLayer == Layer)
                        {
                            carOK = false;
                            break;
                        }
                        //其他车任务起始层也在这层 
                        if (carFromLayer == FromLayer && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }
                        //其他车任务目标层在起始层 
                        if (carFromLayer == ToLayer && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }

                        //目标层有车
                        if (carToLayer == Layer)
                        {
                            carOK = false;
                            break;
                        }

                        //目标层有任务起始层
                        if (carToLayer == FromLayer && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }
                        //目标层有任务目标层
                        if (carToLayer == ToLayer && TaskType > 0)
                        {
                            carOK = false;
                            break;
                        }
                    }
                }
            }
            return carOK;
        }
       
        private DataTable GetTask(string AisleNo)
        {
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("(WCS_Task.State in('0','1','2') and WCS_Task.WarehouseCode = '{0}' and WCS_Task.AisleNo='{1}'", Program.WarehouseCode, AisleNo)) };
            DataTable dt = bll.FillDataTable("WCS.SelectTask", param);            
            return dt;
        }

        /// <summary>
        /// 检查小车入库状态
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <returns></returns>
        private bool Check_Car_Status_IsOk(string carNo,string serviceName)
        {
            try
            {
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "CarStatus" + carNo));
                object[] obj1 = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished"));
                int TaskFlag = int.Parse(obj1[0].ToString());
                int CarMode = int.Parse(obj[0].ToString());
                int TaskType = int.Parse(obj[12].ToString());
                //int CraneAlarmCode = int.Parse(obj[0].ToString());

                if (CarMode == 1 && TaskType == 0 && TaskFlag == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.Error("检查小车" + carNo + "状态时出现错误:" + ex.Message);
                return false;
            }
        }
        private void Send2PLC(string serviceName, DataRow dr, string carNo)
        {
            string TaskNo = dr["TaskNo"].ToString();
            string BillID = dr["BillID"].ToString();
            string TaskType = dr["TaskType"].ToString();
            string state = dr["State"].ToString();
            int taskType = 10;
            string NextState = "3";
            if (state == "0")
            {
                if (TaskType == "13")
                {
                    taskType = 9;
                    NextState = "4";
                }
                else if (TaskType == "10")
                {
                    taskType = 12;
                    NextState = "4";
                }
                else
                {
                    taskType = 11;
                    NextState = "4";
                }
            }

            string FromStationAdd = dr["FromAddress"].ToString();
            string ToStationAdd = dr["ToAddress"].ToString();

            int[] cellAddr = new int[12];

            cellAddr[3] = byte.Parse(FromStationAdd.Substring(1, 3));
            cellAddr[4] = byte.Parse(FromStationAdd.Substring(4, 3));
            cellAddr[5] = byte.Parse(FromStationAdd.Substring(7, 3));
            cellAddr[6] = byte.Parse(ToStationAdd.Substring(1, 3));
            cellAddr[7] = byte.Parse(ToStationAdd.Substring(4, 3));
            cellAddr[8] = byte.Parse(ToStationAdd.Substring(7, 3));

            cellAddr[9] = taskType;
            cellAddr[10] = int.Parse(carNo);

            sbyte[] taskNo = new sbyte[30];
            Util.ConvertStringChar.stringToBytes(TaskNo, 30).CopyTo(taskNo, 0);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);

            if (WriteToService(serviceName, "WriteFinished", 1))
            {
                report.Send2MJWcs(base.Context, 1, TaskNo);
                bll.ExecNonQuery("WCS.UpdateTaskTimeByTaskNo", new DataParameter[] { new DataParameter("@State", NextState), new DataParameter("@CarNo", carNo), new DataParameter("@TaskNo", TaskNo) });
                bll.ExecNonQuery("WCS.UpdateBillStateByBillID", new DataParameter[] { new DataParameter("@State", 3), new DataParameter("@BillID", BillID) });
            }
            Logger.Info("任务:" + dr["TaskNo"].ToString() + "已下发给" + carNo + "穿梭车;起始地址:" + FromStationAdd + ",目标地址:" + ToStationAdd);
        }        
    }
}