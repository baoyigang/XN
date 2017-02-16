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

            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcs", new DataParameter("{0}", TaskNo));
            string Id = bll.FillDataTable("Wcs.SelectTaskTemp", new DataParameter("{0}", TaskNo)).Rows[0]["ID"].ToString();
            string Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + Jdt.Rows[0]["PalletBarcode"].ToString() + "\",\"startDate\":\"" + Jdt.Rows[0]["StartDate"].ToString() + "\",\"sendDate\":\"" + Jdt.Rows[0]["TaskDate"].ToString() + "\",\"deviceNo\":\"" + Jdt.Rows[0]["DeviceNo"].ToString() + "\",\"sender\":\"" + Jdt.Rows[0]["Tasker"].ToString() + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            #region 主控WCS服务
            DataTable dt = Util.JsonHelper.Json2Dtb(Json);
            string json = "";
            if (dt.Rows.Count == 0)
            {
                return json = "[{\"id\":\"123678\",\"returnCode\"=\"111\",\"message\"=\"失败\",\"finishDate\":\"2017-02-05 10:37:50.985\"}]";
            }
            json = "[{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"returnCode\"=\"000\",\"message\"=\"成功\",\"finishDate\":\"" + DateTime.Now.ToString() + "\"}]";
            return json;
            #endregion
        }


        [WebMethod]
        public string transWCSTaskStatus(string TaskNo)
        {

            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcs", new DataParameter("{0}", TaskNo));
            string Id = bll.FillDataTable("Wcs.SelectTaskTemp", new DataParameter("{0}", TaskNo)).Rows[0]["ID"].ToString();
            string Json = "[{\"id\":\"" + Id + "\",\"taskNo\":\"" + TaskNo + "\",\"palletBarcode\":\"" + Jdt.Rows[0]["PalletBarcode"].ToString() + "\",\"status\":\"" + Jdt.Rows[0]["State"].ToString() + "\",\"errorCode\":\"" + "!1!1!1!1!1!1!1!" + "\",\"sendDate\":\"" + Jdt.Rows[0]["TaskDate"].ToString() + "\",\"sender\":\"" + Jdt.Rows[0]["Tasker"].ToString() + "\",\"deviceNo\":\"" + Jdt.Rows[0]["DeviceNo"].ToString() + "\",\"finishDate\":\"" + Jdt.Rows[0]["FinishDate"].ToString() + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            #region 主控WCS服务
            DataTable dt = Util.JsonHelper.Json2Dtb(Json);
            string json = "";
            if (dt.Rows.Count == 0)
            {
                return json = "[{\"id\":\"123678\",\"returnCode\"=\"111\",\"message\"=\"失败\",\"finishDate\":\"2017-02-05 10:37:50.985\"}]";
            }
            json = "[{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"returnCode\"=\"000\",\"message\"=\"成功\",\"finishDate\":\"" + DateTime.Now.ToString() + "\"}]";
            return json;
            #endregion
        }
    }
}
