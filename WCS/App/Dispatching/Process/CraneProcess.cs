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
        private class rCrnStatus
        {
            public string TaskNo { get; set; }
            public int Status { get; set; }
            public int Action { get; set; }
            public int ErrCode { get; set; }
            public int TaskStatus { get; set; }
            public int io_flag { get; set; }

            public rCrnStatus()
            {
                TaskNo = "";
                Status = 0;
                Action = 0;
                ErrCode = 0;
                TaskStatus = 0;
                io_flag = 0;
            }
        }

        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        private Dictionary<int, rCrnStatus> dCrnStatus = new Dictionary<int, rCrnStatus>();
        private Timer tmWorkTimer = new Timer();
        private bool blRun = false;
        private DataTable dtDeviceAlarm;


        public override void Initialize(Context context)
        {
            try
            {
                dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=1") });

                //获取堆垛机信息
                DataTable dt = bll.FillDataTable("CMD.SelectDevice", new DataParameter[] { new DataParameter("{0}", "Flag=1") });
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    if (!dCrnStatus.ContainsKey(i))
                    {
                        rCrnStatus crnsta = new rCrnStatus();
                        dCrnStatus.Add(i, crnsta);

                        dCrnStatus[i].TaskNo = "";
                        dCrnStatus[i].Status = int.Parse(dt.Rows[i-1]["State"].ToString());
                        dCrnStatus[i].TaskStatus = 0;
                        dCrnStatus[i].ErrCode = 0;
                        dCrnStatus[i].Action = 0;
                    }
                }

                tmWorkTimer.Interval = 1000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);
                

                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess堆垛机初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            switch (stateItem.ItemName)
            {
                case "CraneTaskFinished":
                    object obj = ObjectUtil.GetObject(stateItem.State);
                    string TaskFinish = obj.ToString();
                    if (TaskFinish.Equals("True") || TaskFinish.Equals("1"))
                    {
                        string TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, "CraneTaskNo")));
                        //清除堆垛机任务号
                        sbyte[] taskNo = new sbyte[10];
                        Util.ConvertStringChar.stringToBytes("", 10).CopyTo(taskNo, 0);
                        WriteToService(stateItem.Name, "TaskNo", taskNo);

                        DataParameter[] para = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_Task.TaskNo='{0}'", TaskNo)) };
                        DataTable dt = bll.FillDataTable("WCS.SelectTask", para);

                        string TaskType = "";
                        string strState = "";
                        if (dt.Rows.Count > 0)
                        {
                            TaskType = dt.Rows[0]["TaskType"].ToString();
                            strState = dt.Rows[0]["State"].ToString();

                        }
                        //存储过程处理
                        if (TaskNo != "")
                        {
                            Logger.Info(stateItem.ItemName + "完成标志,任务号:" + TaskNo);
                            //更新任务状态

                            List<string> comds = new List<string>();
                            List<DataParameter[]> paras = new List<DataParameter[]>();
                            if (TaskType == "12" || TaskType == "15" || TaskType == "14")  //输送线处理程序
                            {
                                comds.Add("WCS.Sp_TaskProcess"); //更新为出库任务完成
                                para = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                                paras.Add(para);


                                comds.Add("WCS.UpdateTaskStateByTaskNo"); //更新到达出库站台
                                para = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@State", 6) };
                                paras.Add(para);
                            }

                            comds.Add("WCS.Sp_TaskProcess");

                            para = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                            paras.Add(para);


                            bll.ExecTran(comds.ToArray(), paras);
                            WCSDataService.WCSDataService wcsData = new WCSDataService.WCSDataService();
                            wcsData.transWCSTaskStatus(TaskNo);
                        }

                        string strValue = "";
                        string[] str = new string[3];
                        if (TaskType == "12" || (TaskType == "14" && strState == "4"))//显示拣货信息.
                        {
                            str[0] = "1";
                            if (TaskType == "14")
                                str[0] = "2";

                            while ((strValue = FormDialog.ShowDialog(str, dt)) != "")
                            {
                                if (TaskType == "14")
                                {
                                    bll.ExecNonQuery("WCS.UpdateTaskStateByTaskNo", new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@State", 2) });
                                }
                                break;
                            }
                        }
                    }
                    break;
                case "CraneAlarmCode":
                    object obj1 = ObjectUtil.GetObject(stateItem.State);
                    if (obj1 == null)
                        return;
                    if (obj1.ToString() != "0")
                    {
                        string strError = "";
                        DataRow[] drs = dtDeviceAlarm.Select(string.Format("AlarmCode={0}", obj1.ToString()));
                        if (drs.Length > 0)
                            strError = drs[0]["AlarmDesc"].ToString();
                        else
                            strError = "堆垛机未知错误！";
                        Logger.Error(strError);
                    }
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
           
            
            return;
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

                DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", "WarehoseCode in('A','B') and ((TaskType in('11') and State='2') or (TaskType in('12','13') and State='0'))") };
                DataTable dt = bll.FillDataTable("WCS.SelectTask", parameter);
                DataTable dtAisle = bll.FillDataTable("CMD.SelectAisle", new DataParameter[] { new DataParameter("{0}", "WarehoseCode in('A','B')") });
                for (int i = 0; i <dtAisle.Rows.Count; i++)
                {
                    //查找可用设备

                    //查找任务
                    string filter = string.Format("WCS_Task.AisleNo='{0}'", dtAisle.Rows[i]["AisleNo"].ToString());
                    DataRow[] drs = dt.Select(filter);
                    if(drs.Length>0)
                    {
                        string DeviceNo = drs[0]["AisleDeviceNo"].ToString();
                        string ServiceName = drs[0]["ServiceName"].ToString();
                        //判断设备状态是否满足
                        if (!Check_Device_Status_IsOk(ServiceName))
                            return;

                        //string StationLoad = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "StationLoad" + stationNo)).ToString();
                        //判断出库站台无货
                        //if (StationLoad.Equals("True") || StationLoad.Equals("1"))
                        //{
                        //    Logger.Info("站台状态不符合堆垛机出库");
                        //    return;
                        //}
                        //找到任务再找可以执行的设备

                        if (DeviceNo.Trim().Length > 0)
                        {
                            DataRow dr = drs[0];
                            Send2PLC(dr);
                        }
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
        private bool Check_Device_Status_IsOk(string DeviceServiceName)
        {
            try
            {
                string plcTaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(DeviceServiceName, "CraneTaskNo")));

                string craneMode = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(DeviceServiceName, "CraneMode")).ToString();
                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(DeviceServiceName, "CraneAlarmCode"));
                int CraneState = int.Parse(obj[1].ToString());
                int CraneAlarmCode = int.Parse(obj[0].ToString());

                if (plcTaskNo == "" && craneMode == "1" && CraneAlarmCode == 0 && CraneState == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }            
        }
        private void Send2PLC(DataRow dr)
        {
            string DeviceNo = dr["AisleDeviceNo"].ToString();
            string serviceName = dr["ServiceName"].ToString();
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
                else
                {
                    taskType = 11;
                    NextState = "4";
                }
            }

            string fromStation = dr["FromStation"].ToString();
            string toStation = dr["ToStation"].ToString();

            int[] cellAddr = new int[10];

            cellAddr[0] = 0;
            cellAddr[1] = 0;
            cellAddr[2] = 0;

            cellAddr[3] = byte.Parse(fromStation.Substring(0, 3));
            cellAddr[4] = byte.Parse(fromStation.Substring(3, 3));
            cellAddr[5] = byte.Parse(fromStation.Substring(6, 3));
            cellAddr[6] = byte.Parse(toStation.Substring(0, 3));
            cellAddr[7] = byte.Parse(toStation.Substring(3, 3));
            cellAddr[8] = byte.Parse(toStation.Substring(6, 3));
            cellAddr[9] = taskType;

            int taskNo = int.Parse(TaskNo);

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            Context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            if (WriteToService(serviceName, "WriteFinished", 1))
            {
                bll.ExecNonQuery("WCS.UpdateTaskTimeByTaskNo", new DataParameter[] { new DataParameter("@State", NextState), new DataParameter("@DeviceNo", DeviceNo), new DataParameter("@TaskNo", TaskNo) });
            }
            Logger.Info("任务:" + dr["TaskNo"].ToString() + "已下发给" + DeviceNo + ";起始地址:" + fromStation + ",目标地址:" + toStation);
        }        
    }
}