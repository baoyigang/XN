using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Util;

namespace ServiceHost
{
    /// <summary>
    /// Summary description for transSRMTask
    /// </summary>
    public class transSRMTask : IHttpHandler
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        public void ProcessRequest(HttpContext context)
        {
            string postString = string.Empty;
            if(HttpContext.Current.Request.HttpMethod.ToUpper()=="POST")
            {
                using(Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = System.Text.Encoding.UTF8.GetString(postBytes);
                    string str = ImportWCSTask(postString);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(str);
                    context.Response.End();
                }
            }
        }
        public string ImportWCSTask(string taskData)
        {
            Log.WriteToLog("1", "transSRMTask-Rec", taskData);
            string json = "";
            string id = "";
            string state = "";
            string State = "";
            try
            {
                DataTable dt = Util.JsonHelper.Json2Dtb(taskData);
                if (dt.Rows.Count > 0)
                    id = dt.Rows[0]["id"].ToString();
                else
                    id = "";

                bll.ExecNonQuery("WCS.DeleteWcsTemp");
                bll.BatchInsertTable(dt, "WCS_TaskTemp");
                bll.ExecNonQueryTran("WCS.Sp_ImportWmsTask");
                
                json = "{\"id\":\"" + id + "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}";
            }
            catch (Exception ex)
            {
                json = "{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"" + ex.Message + "\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}";
            }
            Log.WriteToLog("1", "transSRMTask-Rtn", json);
            return json;

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}