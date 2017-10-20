using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Util;
using MCP;

namespace App.Dispatching.Process
{
    public class Report
    {
        BLL.BLLBase bll = new BLL.BLLBase();

        public void Send2MJWcs(Context context, int Flag, string TaskNo)
        {
            DataTable dt;
            RtnMessage rtnMessage;
            DataParameter[] param;
            if (Flag == 1)
            {
                //上报任务开始
                dt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", TaskNo));
                if (dt.Rows.Count > 0)
                {
                    string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                    Logger.Info("任务" + TaskNo + "开始上报");
                    string message = Program.send("transWCSExecuteTask", Json);
                    rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);

                    param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                    Logger.Info("任务" + TaskNo + "开始上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            else if (Flag == 2)
            {
                //上报任务故障
                if(Program.WarehouseCode=="S")
                    dt = bll.FillDataTable("Wcs.SelectTaskWcsAlarm2", new DataParameter("{0}", TaskNo));
                else
                    dt = bll.FillDataTable("Wcs.SelectTaskWcsAlarm", new DataParameter("{0}", TaskNo));
                if (dt.Rows.Count > 0)
                {
                    string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                    Logger.Info("任务" + TaskNo + "报警上报");
                    string message = Program.send("transWCSTaskStatus", Json);
                    rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
                    //更新任务,备用字段field1是重新分配的货位
                    param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@field1", rtnMessage.field1) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskNewCellCode", param);
                    //更新返回代码
                    param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                    Logger.Info("任务" + TaskNo + "报警上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            else if (Flag == 3)
            {
                //上报任务完成
                dt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", TaskNo));
                if (dt.Rows.Count > 0)
                {
                    string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                    Logger.Info("任务:" + TaskNo + "完成上报");
                    string message = Program.send("transWCSTaskStatus", Json);
                    rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);

                    param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                    Logger.Info("任务:" + TaskNo + "完成上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            else if (Flag == 4)
            {
                //手动申请上报任务故障
                if (Program.WarehouseCode == "S")
                    dt = bll.FillDataTable("Wcs.SelectTaskWcsAlarm2", new DataParameter("{0}", TaskNo));
                else
                    dt = bll.FillDataTable("Wcs.SelectTaskWcsAlarm", new DataParameter("{0}", TaskNo));
                if (dt.Rows.Count > 0)
                {
                    string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                    Logger.Info("任务" + TaskNo + "报警上报");
                    string message = Program.send("transWCSTaskStatus", Json);
                    rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
                    //更新任务,备用字段field1是重新分配的货位
                    param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@field1", rtnMessage.field1) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskNewCellCode2", param);
                    //更新返回代码
                    param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                    Logger.Info("任务" + TaskNo + "报警上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            else if (Flag == 5)
            {
                //任务终止
                dt = bll.FillDataTable("Wcs.SelectTaskWcsTerminated", new DataParameter("{0}", TaskNo));
                if (dt.Rows.Count > 0)
                {
                    string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
                    Logger.Info("任务" + TaskNo + "终止上报");
                    string message = Program.send("transWCSTaskStatus", Json);
                    rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
                    //更新返回代码
                    param = new DataParameter[] { new DataParameter("@Flag", Flag), new DataParameter("@TaskNo", TaskNo), new DataParameter("@ReturnCode", rtnMessage.returnCode) };
                    bll.ExecNonQueryTran("WCS.UpdateTaskReturnCode", param);

                    Logger.Info("任务" + TaskNo + "终止上报，收到反馈:" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
        }
        public void SendDeviceStatus(Context context, string ServiceName, string AlarmDesc)
        {
            string id = Guid.NewGuid().ToString();
            string deviceNo = ServiceName.Substring(3, 4);

            string mode = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(ServiceName, "WorkMode")).ToString();
            object[] Status = ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(ServiceName, "Status"));
            object[] OtherStatus = ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(ServiceName, "OtherStatus"));
            string status = OtherStatus[1].ToString();
            string aisleNo = OtherStatus[2].ToString();
            string taskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(ServiceName, "ReadTaskNo")));
            string fork = Status[3].ToString();
            string load = Status[0].ToString();
            string column = Status[1].ToString();
            string layer = Status[2].ToString();
            string alarmCode = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(ServiceName, "AlarmCode")).ToString();
            string field2 = AlarmDesc;
            string sender1 = "ROBO_WCS";

            string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"\",\"field2\":\"" + field2 + "\",\"field3\":\"\"" + "}]";
            Logger.Info("开始上报设备编号[" + deviceNo + "]的状态");
            string message = Program.send("transWCSDevice", Json);
            RtnMessage rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
            Logger.Info("上报设备编号[" + deviceNo + "]状态,收到反馈：" + rtnMessage.returnCode + ":" + rtnMessage.message);
        }
        public void SendDeviceStatus2(Context context, string ServiceName, string carNo, string AlarmDesc)
        {
            string id = Guid.NewGuid().ToString();

            string aisleNo = ServiceName.Substring(5, 2);
            string deviceNo = aisleNo + carNo;
            object[] Status = ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(ServiceName, "CarStatus" + carNo));
            string mode = Status[0].ToString();
            string status = Status[12].ToString();
            string taskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(ServiceName, "CarTask" + carNo)));
            string fork = Status[11].ToString();
            string load = Status[10].ToString();
            string column = Status[2].ToString();
            string layer = Status[3].ToString();
            string alarmCode = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(ServiceName, "CarAlarm" + carNo)).ToString();
            string field2 = AlarmDesc;
            string sender1 = "ROBO_WCS";

            string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + Program.WarehouseCode + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"\",\"field2\":\"" + field2 + "\",\"field3\":\"\"" + "}]";
            Logger.Info("开始上报设备编号[" + deviceNo + "]的状态");
            string message = Program.send("transWCSDevice", Json);
            RtnMessage rtnMessage = JsonHelper.JSONToObject<RtnMessage>(message);
            Logger.Info("上报设备编号[" + deviceNo + "]状态,收到反馈：" + rtnMessage.returnCode + ":" + rtnMessage.message);

        }
    }
}
