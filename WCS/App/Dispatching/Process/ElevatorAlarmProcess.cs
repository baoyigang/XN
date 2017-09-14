using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;
using System.Timers;

namespace App.Dispatching.Process
{
    public class ElevatorAlarmProcess : AbstractProcess
    {
        // 记录提升机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        private DataTable dtDeviceAlarm;
        Report report = new Report();
        public override void Initialize(Context context)
        {
            try
            {
                dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=2") });

                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("ElevatorAlarmProcess提升机初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {                
                switch (stateItem.ItemName)
                {
                    case "CarAlarm01":
                    case "CarAlarm02":
                    
                        object obj = ObjectUtil.GetObject(stateItem.State);
                        if (obj == null)
                            return;

                        string carNo = stateItem.ItemName.Substring(8, 2);
                        string DeviceNo = stateItem.Name.Substring(5, 2) + carNo;
                        string AlarmCode = obj.ToString();
                        string AlarmDesc = "";

                        if (AlarmCode != "0")
                        {

                            DataRow[] drs = dtDeviceAlarm.Select(string.Format("AlarmCode={0}", AlarmCode));
                            if (drs.Length > 0)
                                AlarmDesc = drs[0]["AlarmDesc"].ToString();
                            else
                                AlarmDesc = "穿梭车未知错误！";
                            //更新任务报警
                            string TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(stateItem.Name, "CarTask" + carNo)));
                            if (TaskNo.Length > 0)
                            {
                                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@AlarmCode", obj), new DataParameter("@AlarmDesc", AlarmDesc) };
                                bll.ExecNonQueryTran("WCS.UpdateTaskDeviceAlarm", param);

                                report.Send2MJWcs(base.Context, 2, TaskNo);
                                //Send2MJWcs(2, TaskNo);
                            }

                            Logger.Error("设备编号" + DeviceNo + "发生报警，代号：" + obj.ToString() + ";描述：" + AlarmDesc);
                        }

                        DataParameter[] paramb = new DataParameter[] { new DataParameter("@AlarmCode", obj), new DataParameter("@DeviceNo", DeviceNo) };
                        bll.ExecNonQueryTran("WCS.UpdateDeviceAlarm", paramb);
                        //上报设备状态
                        report.SendDeviceStatus2(base.Context, stateItem.Name, carNo,AlarmDesc);
                        //SendDeviceStatus(stateItem.Name, AlarmDesc);
                        break;
                    case "ElevatorAlarm":
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ElevatorAlarmProcess StateChanged方法出错，原因：" + ex.Message);
            }
        }
    }
}