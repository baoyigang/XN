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

                json = "[{\"id\":\"" + id+ "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
            catch (Exception ex)
            {
                json = "[{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"失败\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
                return json;

        }





        //总控WCS入库巷道请求
        [WebMethod]
        public string transSRMTaskAisle(string wcsProductObject)
        {
            string json = "";
            string id = "";
            string taskNo = "";
            string Aisle = "";
            string AreaCode = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = Util.JsonHelper.Json2Dtb(wcsProductObject);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["id"].ToString();
                    taskNo = dt.Rows[0]["taskNo"].ToString();
                    AreaCode = dt.Rows[0]["areaCode"].ToString();
                }
                else
                {
                    id = "";
                    taskNo = "";
                    AreaCode = "";
                }
                    bll.BatchInsertTable(dt, "WCS_AisleTemp");


                DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest", new DataParameter("{0}", AreaCode));
                Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();

                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
            catch(Exception ex)
            {
                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"}]";
            }
            return json;
         }
    }
}
