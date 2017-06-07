using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;

namespace App.Dispatching.Process
{
    public class CraneProcess : AbstractProcess
    {
        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        //private string WarehouseCode = "";
        private int InTaskCount = 2;
        private Timer tmWorkTimer = new Timer();
        private bool blRun = false;
        private DataTable dtDeviceAlarm;

        public override void Initialize(Context context)
        {
            try
            {
                dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=1") });

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);

                MCP.Config.Configuration conf = new MCP.Config.Configuration();
                conf.Load("Config.xml");
                //WarehouseCode = conf.Attributes["WarehouseCode"];
                InTaskCount = int.Parse(conf.Attributes["InTaskCount"]);
                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess堆垛机初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {                
                string TaskNo = "";
                switch (stateItem.ItemName)
                {
                    case "TaskFinish":
                        object obj = ObjectUtil.GetObject(stateItem.State);
                        if (obj == null)
                            return;
                        string TaskFinish = obj.ToString();
                        if (TaskFinish.Equals("True") || TaskFinish.Equals("1"))
                        {
                            TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, "ReadTaskNo")));
                            DataParameter[] para = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskNo='{0}'", TaskNo)) };
                            DataTable dt = bll.FillDataTable("WCS.SelectTask", para);

                            string TaskType = "";
                            if (dt.Rows.Count > 0)
                                TaskType = dt.Rows[0]["TaskType"].ToString();

                            //存储过程处理
                            if (TaskNo != "")
                            {
                                Logger.Info(stateItem.ItemName + "完成标志,任务号:" + TaskNo);
                                //更新任务状态

                                List<string> comds = new List<string>();
                                List<DataParameter[]> paras = new List<DataParameter[]>();

                                comds.Add("WCS.Sp_TaskProcess");

                                para = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                                paras.Add(para);

                                bll.ExecTran(comds.ToArray(), paras);

                                //清除堆垛机任务号
                                sbyte[] taskNo = new sbyte[20];
                                for (int i = 0; i < 20; i++)
                                    taskNo[i] = 32;
                                //Util.ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);
                                WriteToService(stateItem.Name, "TaskNo", taskNo);

                                Send2MJWcs(3, TaskNo);
                            }
                        }
                        break;
                    case "ACK":
                        obj = ObjectUtil.GetObject(stateItem.State);
                        if (obj == null)
                            return;
                        string ack = obj.ToString();
                        if (ack.Equals("True") || ack.Equals("1"))
                            WriteToService(stateItem.Name, "STB", 0);
                        break;
                    case "AlarmCode":
                        obj = ObjectUtil.GetObject(stateItem.State);
                        if (obj == null)
                            return;

                        string DeviceNo = stateItem.Name.Substring(3, 4);
                        string AlarmCode = obj.ToString();
                        string AlarmDesc = "";

                        if (AlarmCode != "0")
                        {
                            DataRow[] drs = dtDeviceAlarm.Select(string.Format("AlarmCode={0}", AlarmCode));
                            if (drs.Length > 0)
                                AlarmDesc = drs[0]["AlarmDesc"].ToString();
                            else
                                AlarmDesc = "堆垛机未知错误！";
                            //更新任务报警
                            TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, "ReadTaskNo")));
                            if (TaskNo.Length > 0)
                            {
                                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@AlarmCode", obj), new DataParameter("@AlarmDesc", AlarmDesc) };
                                bll.ExecNonQueryTran("WCS.UpdateTaskDeviceAlarm", param);

                                Send2MJWcs(2, TaskNo);
                            }

                            Logger.Error("设备编号" + DeviceNo + "发生报警，代号：" + obj.ToString() + ";描述：" + AlarmDesc);
                        }

                        DataParameter[] paramb = new DataParameter[] { new DataParameter("@AlarmCode", obj), new DataParameter("@DeviceNo", DeviceNo) };
                        bll.ExecNonQueryTran("WCS.UpdateDeviceAlarm", paramb);
                        //上报设备状态
                        
                        SendDeviceStatus(stateItem.Name, AlarmDesc);
                        break;
                    case "Run":
                        blRun = (int)stateItem.State == 1;
                        if (blRun)
                        {
                            tmWorkTimer.Start();
                            Logger.Info("堆垛机联机");
                        }
                        else
                        {
                            tmWorkTimer.Stop();
                            Logger.Info("堆垛机脱机");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess StateChanged方法出错，原因：" + ex.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();

                DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.WarehouseCode = '{0}' and ((WCS_TASK.TaskType in('11') and WCS_TASK.State='2') or (WCS_TASK.TaskType in('12','13') and WCS_TASK.State='0'))", Program.WarehouseCode)) };
                DataTable dt = bll.FillDataTable("WCS.SelectTask", parameter);
                DataTable dtAisle = bll.FillDataTable("CMD.SelectAisleDevice", new DataParameter[] { new DataParameter("{0}", string.Format("S1.WarehouseCode = '{0}'", Program.WarehouseCode)) });
                for (int i = 0; i < dtAisle.Rows.Count; i++)
                {
                    //查找可用设备
                    string DeviceNo = dtAisle.Rows[i]["DeviceNo"].ToString();
                    string ServiceName = dtAisle.Rows[i]["ServiceName"].ToString();

                    //没有可用设备
                    if (DeviceNo.Trim().Length <= 0)
                        continue;
                    //设备状态不符合
                    if (!Check_Device_Status_IsOk(ServiceName))
                        continue;

                    //查找入库任务>2的先执行
                    string filter = string.Format("AisleNo='{0}' and TaskType='11' and State in('1','2')", dtAisle.Rows[i]["AisleNo"].ToString());
                    DataRow[] drs = dt.Select(filter, "State");
                    if (drs.Length > InTaskCount)
                    {
                        if (drs[0]["State"].ToString() == "2")
                        {
                            DataRow dr = drs[0];
                            Send2PLC(dr);
                            continue;
                        }
                        //找到任务再找可以执行的设备                        
                    }
                    else //再按优先等级排序
                    {
                        filter = string.Format("AisleNo='{0}' and TaskType in('12','13') and State in('0')", dtAisle.Rows[i]["AisleNo"].ToString());
                        drs = dt.Select(filter, "TaskLevel,TaskNo");
                        if (drs.Length > 0)
                        {
                            if (drs[0]["TaskType"].ToString() == "13")
                            {
                                DataRow dr = drs[0];
                                Send2PLC(dr);
                                continue;
                            }
                            else
                            {
                                int stationNo = int.Parse(drs[0]["StationNo"].ToString().Substring(2,1));
                                object[] obj = ObjectUtil.GetObjects(WriteToService(ServiceName, "OtherStatus"));
                                //判断出库站台无货
                                if (obj[2 + stationNo].ToString() == "1")
                                {
                                    DataRow dr = drs[0];
                                    Send2PLC(dr);
                                    continue;
                                }
                            }
                        }
                    }
                    filter = string.Format("AisleNo='{0}' and TaskType='11' and State in('2')", dtAisle.Rows[i]["AisleNo"].ToString());
                    drs = dt.Select(filter, "State");
                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        Send2PLC(dr);
                        continue;
                    }
                }
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        /// <summary>
        /// 检查堆垛机入库状态
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <returns></returns>
        private bool Check_Device_Status_IsOk(string ServiceName)
        {
            try
            {
                string plcTaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(ServiceName, "ReadTaskNo")));

                string workMode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(ServiceName, "WorkMode")).ToString();
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ServiceName, "OtherStatus"));
                int State = int.Parse(obj[1].ToString());
                int AlarmCode = int.Parse(obj[0].ToString());

                if (workMode == "1" && AlarmCode == 0 && State == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex.Message);
                return false;
            }            
        }
        private void Send2PLC(DataRow dr)
        {
            string DeviceNo = dr["AisleDeviceNo"].ToString();
            string serviceName = dr["ServiceName"].ToString();
            string TaskNo = dr["TaskNo"].ToString();
            string TaskType = dr["TaskType"].ToString();
            string state = dr["State"].ToString();

            string NextState = "3";
            if (state == "0")
            {
                NextState = "4";
            }

            string FromStationAdd = dr["FromAddress"].ToString();
            string ToStationAdd = dr["ToAddress"].ToString();

            int[] cellAddr = new int[12];

            cellAddr[0] = byte.Parse(FromStationAdd.Substring(4, 3));
            cellAddr[1] = byte.Parse(FromStationAdd.Substring(7, 3));
            cellAddr[2] = byte.Parse(FromStationAdd.Substring(1, 3));
            cellAddr[3] = byte.Parse(ToStationAdd.Substring(4, 3));
            cellAddr[4] = byte.Parse(ToStationAdd.Substring(7, 3));
            cellAddr[5] = byte.Parse(ToStationAdd.Substring(1, 3));

            cellAddr[6] = 1;

            sbyte[] taskNo = new sbyte[20];
            Util.ConvertStringChar.stringToBytes(TaskNo, 20).CopyTo(taskNo, 0);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            
            if (WriteToService(serviceName, "STB", 1))
            {
                bll.ExecNonQuery("WCS.UpdateTaskTimeByTaskNo", new DataParameter[] { new DataParameter("@State", NextState), new DataParameter("@DeviceNo", DeviceNo), new DataParameter("@TaskNo", TaskNo) });

                Send2MJWcs(1, TaskNo);
            }
            Logger.Info("任务:" + TaskNo + "已下发给" + DeviceNo + "设备;起始地址:" + FromStationAdd + ",目标地址:" + ToStationAdd);
        }
        private void Send2MJWcs(int Flag, string TaskNo)
        {
            DataTable dt;
            RtnMessage rtnMessage;
            DataParameter[] param;
            if (Flag == 1)
            {
                //上报任务开始
                dt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", TaskNo));
                string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                Logger.Info("任务开始上报");
                string message = Program.send("transWCSExecuteTask", Json);
                rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);

                param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                Logger.Info("任务开始执行上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
            }
            else if (Flag == 2)
            {
                //上报任务故障
                dt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", TaskNo));
                string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                Logger.Info("任务故障上报");
                string message = Program.send("transWCSTaskStatus", Json);
                rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
                //更新任务,备用字段field1是重新分配的货位
                param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@field1", rtnMessage.field1) };
                bll.ExecNonQueryTran("WCS.UpdateTaskNewCellCode", param);
                //更新返回代码
                param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);
               
                Logger.Info("任务故障上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
            }
            else if (Flag == 3)
            {
                //上报任务完成
                dt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", TaskNo));
                string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                Logger.Info("任务完成上报");
                string message = Program.send("transWCSTaskStatus", Json);
                rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);

                param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                Logger.Info("任务完成执行上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
            }

        }
        private void SendDeviceStatus(string ServiceName, string AlarmDesc)
        {
            string id = Guid.NewGuid().ToString();
            string deviceNo = ServiceName.Substring(3, 4);

            string mode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(ServiceName, "WorkMode")).ToString();
            object[] Status = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ServiceName, "Status"));
            object[] OtherStatus = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(ServiceName, "OtherStatus"));
            string status = OtherStatus[1].ToString();
            string aisleNo = OtherStatus[2].ToString();
            string taskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(ServiceName, "ReadTaskNo")));
            string fork = Status[3].ToString();
            string load = Status[0].ToString();
            string column = Status[1].ToString();
            string layer = Status[2].ToString();
            string alarmCode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(ServiceName, "AlarmCode")).ToString();
            string field1 = AlarmDesc;
            string sender1 = "admin";

            string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"" + field1 + "\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            Logger.Info("上报设备状态");
            string message = Program.send("transWCSDevice", Json);
            RtnMessage rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
            Logger.Info("上报设备状态,收到反馈：" + rtnMessage.returnCode + ":" + rtnMessage.message);
        }
    }
}