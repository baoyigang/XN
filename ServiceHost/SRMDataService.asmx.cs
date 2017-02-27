using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using Util;
using System.Configuration;
using System.Data.SqlClient;

namespace ServiceHost
{
    /// <summary>
    /// SRMDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SRMDataService : System.Web.Services.WebService
    {
      
        //总控WCS提供给立库WCS任务数据
        [WebMethod]
        public string transSRMTask(string wcsProductObject)
        {
            WriteToLog("1", "transSRMTask", wcsProductObject);
            string json = "";
            string id = "";
            try
            {
                DataTable dt = Util.JsonHelper.Json2Dtb(wcsProductObject);
                if (dt.Rows.Count > 0)
                    id = dt.Rows[0]["id"].ToString();
                else
                    id = "";


                BLL.BLLBase bll = new BLL.BLLBase();

                bll.ExecNonQuery("WCS.DeleteWcsTemp");
                bll.BatchInsertTable(dt, "WCS_TaskTemp");
                bll.ExecNonQueryTran("WCS.Sp_ImportWmsTask");

                json = "[{\"id\":\"" + id + "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
            catch (Exception ex)
            {
                json = "[{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"" + ex.Message + "\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
            return json;

        }

        //总控WCS入库巷道请求
        [WebMethod]
        public string transSRMTaskAisle(string wcsProductObject)
        {
            WriteToLog("1","transSRMTaskAisle", wcsProductObject);
            string json = "";
            string id = "";
            string taskNo = "";
            string Aisle = "";
            string WarehouseCode = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = Util.JsonHelper.Json2Dtb(wcsProductObject);
               
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["id"].ToString();
                    taskNo = dt.Rows[0]["taskNo"].ToString();
                    WarehouseCode = dt.Rows[0]["WarehouseCode"].ToString();
                }
                else
                {
                    id = "";
                    taskNo = "";
                    WarehouseCode = "";
                }
                bll.BatchInsertTable(dt, "WCS_AisleTemp");


                DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest", new DataParameter("{0}", WarehouseCode));
                Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();

                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
            }
            catch (Exception ex)
            {
                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            return json;
         }


        public void WriteToLog(string Flag, string Method, string Msg)
        {
            string Folder = "WMS";
            if(Flag=="2")

                Folder = "WCS";
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\" + Folder;

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            path = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.File.AppendAllText(path, string.Format("{0} , {1} :  {2}", DateTime.Now, Method, Msg + "\r\n"));
        }
    }
}
