using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Util;
using System.ServiceModel.Activation;
using System.IO;
using System.Runtime.Serialization.Json;


namespace ServiceHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SRMDataService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class SRMDataService : ISRMDataService
    {        
        public string transWCSTask(List<Task> list)
        {
            string Json = List2Json(list);
            Log.WriteToLog("1", "transSRMTask--Rec", Json);
            string rtnMessage = "";
            string id = "";
            try
            {
                DataTable dt = Util.JsonHelper.Json2Dtb(Json);
                if (dt.Rows.Count > 0)
                    id = dt.Rows[0]["id"].ToString();
                else
                    id = "";

                BLL.BLLBase bll = new BLL.BLLBase();

                bll.ExecNonQuery("WCS.DeleteWcsTemp");
                bll.BatchInsertTable(dt, "WCS_TaskTemp");
               // bll.ExecNonQueryTran("WCS.Sp_ImportWmsTask");

                rtnMessage = "[{\"id\":\"" + id + "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
            }
            catch (Exception ex)
            {
                rtnMessage = "[{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"" + ex.Message + "\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            Log.WriteToLog("1", "transSRMTask-Rtn", rtnMessage);
            return rtnMessage;
        }

        //总控WCS入库巷道请求
        public string transWCSTaskAisle(List<TaskAisle> list)
        {
            string Json = List2Json(list);
            Log.WriteToLog("1", "transSRMTaskAisle-Rec", Json);

            string rtnMessage = "";
            string id = "";
            string taskNo = "";
            string Aisle = "";
            string WarehouseCode = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = Util.JsonHelper.Json2Dtb(Json);

                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["id"].ToString();
                    taskNo = dt.Rows[0]["taskNo"].ToString();
                    WarehouseCode = dt.Rows[0]["warehouseCode"].ToString();
                }
                else
                {
                    id = "";
                    taskNo = "";
                    WarehouseCode = "";
                }
                bll.BatchInsertTable(dt, "WCS_AisleTemp");


                DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest", new DataParameter("{0}", string.Format("WarehouseCode='{0}'", WarehouseCode)));
                //DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest");
                if(dtSelectAisle.Rows.Count>0)
                    Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();

                rtnMessage = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
            }
            catch (Exception ex)
            {
                rtnMessage = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            Log.WriteToLog("1", "transSRMTaskAisle-Rtn", rtnMessage);
            return rtnMessage;
        }
        private string List2Json<T>(List<T> list)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(list.GetType());
            string Json = "";
            //序列化
            using (MemoryStream stream = new MemoryStream())
            {
                ser.WriteObject(stream, list);
                Json = Encoding.UTF8.GetString(stream.ToArray());
            }
            return Json;
        }
    }
}
