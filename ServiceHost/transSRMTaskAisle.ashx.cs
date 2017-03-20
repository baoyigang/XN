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
    public class transSRMTaskAisle : IHttpHandler
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
                    string str = RequestWCSTaskAisle(postString);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(str);
                    context.Response.End();
                }
            }
            
            
        }
        public string RequestWCSTaskAisle(string taskData)
        {
            Log.WriteToLog("1", "transSRMTaskAisle-Rec", taskData);
            string json = "";
            string id = "";
            string taskNo = "";
            string Aisle = "";
            string WarehouseCode = "";
            try
            {               
                DataTable dt = Util.JsonHelper.Json2Dtb(taskData);

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

                DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest", new DataParameter("{0}",  string.Format("WarehouseCode='{0}'", WarehouseCode)));
                //DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest");
                Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();

                json = "{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}";
            }
            catch (Exception ex)
            {
                json = "{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}";
            }
            Log.WriteToLog("1", "transSRMTaskAisle-Rtn", json);
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