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
                string filter = string.Format("TaskNo='{0}'",TaskNo);
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTask", new DataParameter[] { new DataParameter("{0}", filter )});
                
                if (Jdt.Rows.Count>0)
                {
                    palletBarcode = Jdt.Rows[0]["PalletBarcode"].ToString();
                    startDate = Jdt.Rows[0]["StartDate"].ToString();
                    TaskDate = Jdt.Rows[0]["TaskDate"].ToString();
                    deviceNo = Jdt.Rows[0]["DeviceNo"].ToString();
                    Tasker = Jdt.Rows[0]["Tasker"].ToString();
                }
                Id = bll.FillDataTable("Wcs.SelectTaskTemp", new DataParameter("{0}", TaskNo)).Rows[0]["ID"].ToString();
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"startDate\":\"" + startDate + "\",\"sendDate\":\"" + TaskDate + "\",\"deviceNo\":\"" + deviceNo + "\",\"sender\":\"" + Tasker + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
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
            string palletBarcode = "";
            string status = "";
            string TaskDate = "";
            string deviceNo = "";
            string Tasker = "";
            string finishDate = "";
            string Id = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcs", new DataParameter("{0}", TaskNo));
                if (Jdt.Rows.Count>0)
                {
                    palletBarcode = Jdt.Rows[0]["PalletBarcode"].ToString();
                    status = Jdt.Rows[0]["State"].ToString();
                    TaskDate = Jdt.Rows[0]["TaskDate"].ToString();
                    deviceNo = Jdt.Rows[0]["DeviceNo"].ToString();
                    Tasker = Jdt.Rows[0]["Tasker"].ToString();
                    finishDate = Jdt.Rows[0]["FinishDate"].ToString();
                }
                Id = bll.FillDataTable("Wcs.SelectTaskTemp", new DataParameter("{0}", TaskNo)).Rows[0]["ID"].ToString();
                Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + palletBarcode + "\",\"status\":\"" + status + "\",\"errorCode\":\"" + "!1!1!1!1!1!1!1!" + "\",\"sendDate\":\"" + TaskDate + "\",\"sender\":\"" + Tasker + "\",\"deviceNo\":\"" + deviceNo + "\",\"finishDate\":\"" + finishDate + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";

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
    }
}
