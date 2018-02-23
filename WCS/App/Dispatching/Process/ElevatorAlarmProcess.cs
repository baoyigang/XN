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
            lock (this)
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
                            //更新故障表
                            DataTable dtDevice = bll.FillDataTable("CMD.SelectDevice", new DataParameter("{0}", string.Format("DeviceNo2='{0}'", DeviceNo)));
                            string WarehouseCode = dtDevice.Rows[0]["WarehouseCode"].ToString();
                            string AreaCode = dtDevice.Rows[0]["AreaCode"].ToString();
                            if (AlarmCode != "0")
                            {
                                bll.ExecNonQuery("WCS.InsertDeviceAlarmRecord", new DataParameter[]{new DataParameter("@WareHouseCode",WarehouseCode),new 
                                DataParameter("@AreaCode",AreaCode), new DataParameter("@DeviceNo",DeviceNo),new DataParameter("@AlarmCode",AlarmCode)});

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
                            else
                            {
                                bll.ExecNonQuery("WCS.UpdateDeviceAlarmRecord", new DataParameter[]{new DataParameter("@AreaCode",AreaCode),new DataParameter("@WarehouseCode",WarehouseCode),new 
                                DataParameter("@DeviceNo",DeviceNo)});
                            }
                            DataParameter[] paramb = new DataParameter[] { new DataParameter("@AlarmCode", obj), new DataParameter("@DeviceNo", DeviceNo) };
                            bll.ExecNonQueryTran("WCS.UpdateDeviceAlarm", paramb);
                            //上报设备状态
                            report.SendDeviceStatus2(base.Context, stateItem.Name, carNo, AlarmDesc);
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
}