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
        //private DataTable dtDeviceAlarm;
        Report report = new Report();

        public override void Initialize(Context context)
        {
            try
            {
                //dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=1") });

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
                                //Thread th = new Thread(new ThreadStart(ThreadMethod)); //也可简写为new Thread(ThreadMethod);                
                                //th.Start(); //启动线程 

                                report.Send2MJWcs(base.Context, 3, TaskNo);
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
        void ThreadMethod()
        {
            
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

                DataParameter[] parameter = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.WarehouseCode = '{0}' and ((WCS_TASK.TaskType in('11') and WCS_TASK.State in('0','1','2')) or (WCS_TASK.TaskType in('12','13') and WCS_TASK.State='0'))", Program.WarehouseCode)) };
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
                    string filter = string.Format("AisleNo='{0}' and TaskType='11' and State in('0','1','2')", dtAisle.Rows[i]["AisleNo"].ToString());
                    DataRow[] drs = dt.Select(filter, "State desc");
                    if (drs.Length > InTaskCount)
                    {
                        //if (drs[0]["State"].ToString() == "2")
                        //{
                            DataRow dr = drs[0];
                            Send2PLC(dr);
                            continue;
                        //}
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
                                int stationNo = int.Parse(drs[0]["StationNo"].ToString().Substring(4,1));
                                object[] obj = ObjectUtil.GetObjects(WriteToService(ServiceName, "OtherStatus"));
                                //判断出库站台无货
                                if (Program.RequireAPReady == 1)
                                {
                                    if (obj[stationNo + 2].ToString() == "1")
                                    {
                                        DataRow dr = drs[0];
                                        Send2PLC(dr);
                                        continue;
                                    }
                                    else
                                        Logger.Info("出库站台AP" + stationNo.ToString() + "没有Ready");
                                }
                                else
                                {
                                    DataRow dr = drs[0];
                                    Send2PLC(dr);
                                    continue;
                                }
                            }
                        }
                    }
                    filter = string.Format("AisleNo='{0}' and TaskType='11' and State in('0','1','2')", dtAisle.Rows[i]["AisleNo"].ToString());
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

            string NextState = "4";
            if (TaskType == "11" && state == "0")
                NextState = "3";

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

                report.Send2MJWcs(base.Context, 1, TaskNo);
            }
            Logger.Info("任务:" + TaskNo + "已下发给" + DeviceNo + "设备;起始地址:" + FromStationAdd + ",目标地址:" + ToStationAdd);
        }        
    }
}