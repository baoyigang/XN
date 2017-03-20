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
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServiceHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SRMDataService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    //[AspNetCompatibilityrequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SRMDataService : ISRMDataService
    {
        //public string transWCSTask(string taskData)
        //{
        //    Log.WriteToLog("1", "transSRMTask--Rec", taskData);
        //    string json = "";
        //    string id = "";
        //    try
        //    {
        //        DataTable dt = Util.JsonHelper.Json2Dtb(taskData);
        //        if (dt.Rows.Count > 0)
        //            id = dt.Rows[0]["id"].ToString();
        //        else
        //            id = "";


        //        BLL.BLLBase bll = new BLL.BLLBase();

        //        bll.ExecNonQuery("WCS.DeleteWcsTemp");
        //        bll.BatchInsertTable(dt, "WCS_TaskTemp");
        //        bll.ExecNonQueryTran("WCS.Sp_ImportWmsTask");

        //        json = "[{\"id\":\"" + id + "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
        //    }
        //    catch (Exception ex)
        //    {
        //        json = "[{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"" + ex.Message + "\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
        //    }
        //    Log.WriteToLog("1", "transSRMTask-Rtn", json);
        //    return json;

        //}
        //public string transWCSTask(Stream stream)
        //{
        //    StreamReader sr = new StreamReader(stream);

        //    //Byte[] postBytes = new Byte[stream.Length];
        //    //stream.Read(postBytes, 0, (Int32)stream.Length);
        //    string json = sr.ReadToEnd();
        //    sr.Dispose();
        //    //System.Collections.Specialized.NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(s);

        //    //string appKey = nvc["appKey"];
        //    //string sign = nvc["sign"];
        //    //string name = nvc["username"];

        //    //var result = new ErrorModel
        //    //{
        //    //    IsError = true,
        //    //    ErrorCode = -2,
        //    //    ErrorMsg = "操作信息",
        //    //};

        //    Log.WriteToLog("1", "transSRMTask--Rec", json);
        //    //    string json = "";
        //    string id = "";
        //    try
        //    {
        //        DataTable dt = Util.JsonHelper.Json2Dtb(json);
        //        if (dt.Rows.Count > 0)
        //            id = dt.Rows[0]["id"].ToString();
        //        else
        //            id = "";


        //        BLL.BLLBase bll = new BLL.BLLBase();

        //        bll.ExecNonQuery("WCS.DeleteWcsTemp");
        //        bll.BatchInsertTable(dt, "WCS_TaskTemp");
        //        //bll.ExecNonQueryTran("WCS.Sp_ImportWmsTask");

        //        json = "[{\"id\":\"" + id + "\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
        //    }
        //    catch (Exception ex)
        //    {
        //        json = "[{\"id\":\"" + id + "\",\"returnCode\":\"001\"" + ",\"message\":\"" + ex.Message + "\"" + ",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
        //    }
        //    Log.WriteToLog("1", "transSRMTask-Rtn", json);
        //    return json;
        //}
        public string transWCSTask(Stream stream)
        {
            if (stream == null)
                return "12344";
            Byte[] postBytes = new Byte[stream.Length];
            stream.Read(postBytes, 0, (Int32)stream.Length);
            string postString = System.Text.Encoding.UTF8.GetString(postBytes);

            return postString;
            //string str = ImportWCSTask(postString);

            //return "[{\"id\":\"001\",\"returnCode\":\"000\"" + ",\"message\":\"成功\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";

            //var json = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    json.WriteObject(stream, obj);
            //    string szJson = Encoding.UTF8.GetString(stream.ToArray());
            //    return szJson;
            //}
            //return obj.ToString();
             //byte[] postBytes = Encoding.UTF8.GetBytes(
            //return obj.ToString();
            
            //return bt.Length.ToString() + System.Text.Encoding.UTF8.GetString(bt);
            //return JsonConvert.SerializeObject(obj);
            //ArrayList list = (ArrayList)obj;
            //return "123";
            //Stream stream = (Stream)obj;
            //StreamReader sr = new StreamReader(stream);

            //return sr.ReadToEnd();
            //return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //return JavaScriptConvert.SerializeObject(obj);
            //return obj.ToString();
            //JavaScriptSerializer j = new JavaScriptSerializer();
            //return j.Serialize(obj);
            //return Util.JsonHelper.ObjectToJSON(obj);            

        }
        ///<summary> 
        /// 序列化 
        /// </summary> 
        /// <param name="data">要序列化的对象</param> 
        /// <returns>返回存放序列化后的数据缓冲区</returns> 
        public static byte[] Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            return rems.GetBuffer();
        } 

        //总控WCS入库巷道请求
        public string transWCSTaskAisle(string taskData)
        {
            Log.WriteToLog("1", "transSRMTaskAisle-Rec", taskData);
            string json = "";
            string id = "";
            string taskNo = "";
            string Aisle = "";
            string WarehouseCode = "";
            try
            {
                BLL.BLLBase bll = new BLL.BLLBase();
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


                //DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest", new DataParameter("{0}", WarehouseCode));
                DataTable dtSelectAisle = bll.FillDataTable("Cmd.AisleRequest");
                Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();

                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"null\"}]";
            }
            catch (Exception ex)
            {
                json = "[{\"id\":\"" + id + "\",\"taskNo\":\"" + taskNo + "\",\"aisleNo\":\"" + Aisle + "\",\"finishDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"field1\":\"" + ex.Message + "\"}]";
            }
            Log.WriteToLog("1", "transSRMTaskAisle-Rtn", json);
            return json;
        }
    }
}
