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
            string deviceNo = "";
            string Tasker = "";
            string Id = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", TaskNo));

                Json = Util.JsonHelper.Dtb2Json(Jdt,"yyyy-MM-dd HH:mm:ss.fff");
                //Json = Json.Substring(0, Json.Length - 2) + "\",\"field1\":\"null\"}]";
            }
            catch(Exception ex)
            {
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"startDate\":\"" + startDate + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"deviceNo\":\"" + deviceNo + "\",\"sender\":\"" + Tasker + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            WriteToLog("2", "transSRMTask-Rep", Json);
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
            string Tasker = "";
            string deviceNo = "";
            string finishDate = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", TaskNo));

                Json = Util.JsonHelper.Dtb2Json(Jdt, "yyyy-MM-dd HH:mm:ss.fff");
                //Json = Json.Substring(0, Json.Length - 2) + "\",\"field1\":\"null\"}]";
            }
            catch (Exception ex)
            {
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"status\":\"" + status + "\",\"errorCode\":\"" + "!1!1!1!1!1!1!1!" + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + Tasker + "\",\"deviceNo\":\"" + deviceNo + "\",\"finishDate\":\"" + finishDate + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            WriteToLog("2", "transSRMTask-Rep", Json);
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
                WriteToLog("2", "transSRMTask-Rep", Json);

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


        public void WriteToLog(string Flag, string Method, string Msg)
        {
            string Folder = "WMS";
            if (Flag == "2")

                Folder = "WCS";
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\" + Folder;

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            path = path + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.File.AppendAllText(path, string.Format("{0} , {1} :  {2}", DateTime.Now, Method, Msg + "\r\n"));
        }
    }
}
