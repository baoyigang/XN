using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using System.Data;
using Util;
using System.Runtime.Serialization.Json;
using System.Net;

namespace ServiceHost
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“WCSDataService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class WCSDataService : IWCSDataService
    {
        public string transWCSExecuteTask(string taskNo)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            string Json = "";
            string rtnMessage = "";
            try
            {
                Log.WriteToLog("1", "transWCSExecuteTask--Rec", taskNo);
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", taskNo));
                if (Jdt.Rows.Count>0)
                    Json = Util.JsonHelper.Dtb2Json(Jdt, "yyyy-MM-dd HH:mm:ss.fff");

                rtnMessage = send("transWCSExecuteTask",Json);
            }
            catch (Exception ex)
            {
                rtnMessage = "[{\"id\":\"123678\",\"returnCode\"=\"111\",\"message\"=\"失败\",\"finishDate\":\"" + DateTime.Now.ToString() + "\"}]";
            }
            Log.WriteToLog("1", "transWCSExecuteTask--Rtn", rtnMessage);
            return rtnMessage;
        }

        public string transWCSTaskStatus(string taskNo)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            string Json = "";
            string rtnMessage = "";
            try
            {
                Log.WriteToLog("1", "transWCSTaskStatus--Rec", taskNo);
                DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", taskNo));
                if (Jdt.Rows.Count > 0)
                    Json = Util.JsonHelper.Dtb2Json(Jdt, "yyyy-MM-dd HH:mm:ss.fff");

                rtnMessage = send("transWCSTaskStatus", Json);
            }
            catch (Exception ex)
            {
                rtnMessage = "[{\"id\":\"123678\",\"returnCode\"=\"111\",\"message\"=\"" + ex.Message + "\",\"finishDate\":\"" + DateTime.Now.ToString() + "\",\"filed1\":\"" + ex.Message + "\"}]";
            }
            Log.WriteToLog("1", "transWCSTaskStatus--Rtn", rtnMessage);
            return rtnMessage;
        }

        public string send(string method, string data)
        {
            string url = "" + method;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Headers.Add("Authorization: Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImF1dGgiOiJST0xFX0FETUlOLFJPTEVfVVNFUiIsImV4cCI6MTQ5MDI1MjQwNH0.1WP9IMZuZzeHOo3Y9WEBtSnYgvSmi1nqBDUVdC4wNM-WsbyzE3IN5QdT4Ffbf6As_iSIQ5KHv27hEs6CULsshw");
            request.Method = "POST";

            //request.
            request.ContentType = "application/json";

            //request.Accept = "text/html, application/xhtml+xml, */*";
            //request.ContentType = "application/x-www-form-urlencoded";

            //string data = "{\n\"header\": {\n\"token\": \"30xxx6aaxxx93ac8cxx8668xx39xxxx\",\n\"username\": \"jdads\",\n\"password\": \"liuqiangdong2010\",\n\"action\": \"\"\n},\n\"body\": {}\n}";

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            string rtnString = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                rtnString = reader.ReadToEnd();
            }
            return rtnString;
        }
    }
}
