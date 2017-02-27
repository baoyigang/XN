using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Util;

namespace ServiceHost
{
    /// <summary>
    /// WCSDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WCSDataService : System.Web.Services.WebService
    {

        [WebMethod]
        public string transWCSExecuteTask(string TaskNo)
        {
            string Json = "";
            string palletBarcode = "";
            string startDate = "";
            string TaskDate = "";
            string deviceNo = "";
            string Tasker = "";
            string Id = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", TaskNo));

                Json = Util.JsonHelper.Dtb2Json(Jdt);
                Json = Json.Substring(0, Json.Length - 2) + ",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            }
            catch(Exception ex)
            {
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"startDate\":\"" + startDate + "\",\"sendDate\":\"" + TaskDate + "\",\"deviceNo\":\"" + deviceNo + "\",\"sender\":\"" + Tasker + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            }
            #region 主控WCS服务
            string json = "";
            return json;
            #endregion
        }


        [WebMethod]
        public string transWCSTaskStatus(string TaskNo)
        {

            string Json = "";
            string Id = "";
            string palletBarcode = "";
            string status = "";
            string TaskDate = "";
            string Tasker = "";
            string deviceNo = "";
            string finishDate = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", TaskNo));

                Json = Util.JsonHelper.Dtb2Json(Jdt);
                Json = Json.Substring(0, Json.Length - 2) + ",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            }
            catch (Exception ex)
            {
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"status\":\"" + status + "\",\"errorCode\":\"" + "!1!1!1!1!1!1!1!" + "\",\"sendDate\":\"" + TaskDate + "\",\"sender\":\"" + Tasker + "\",\"deviceNo\":\"" + deviceNo + "\",\"finishDate\":\"" + finishDate + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            }
                #region 主控WCS服务
            string json = "";         
            return json;
            #endregion
        }

        [WebMethod]
        public string transWCSDevice(string Json)
        {
            
            try
            {
                

            }
            catch (Exception ex)
            {

                throw;
            }
            #region 主控WCS服务
            string json = "";
            return json;
            #endregion
        }
    }
}
