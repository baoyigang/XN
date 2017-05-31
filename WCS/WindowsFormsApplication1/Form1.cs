using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Util;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        string id="";
        private void button1_Click(object sender, EventArgs e)
        {
            //SRMDataService.SRMDataService srm = new SRMDataService.SRMDataService();
            //string Json = "c";
            //string json = "";
            ////string Ajson = "[{\"id\":\"xcafd3ssaf\",\"taskNo\":\"1702160001\",\"areaCode\":\"A01\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"admin\"" + "}]";
            ////string a = srm.transSRMTaskAisle(Ajson);
            //string c = srm.transSRMTask(Json);
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dt = bll.FillDataTable("Wcs.SelectTaskWcsStart", new DataParameter("{0}", "1701120037"));

            string Json = Util.JsonHelper.Dtb2Json(dt, "yyyy-MM-dd HH:mm:ss.fff");
            string rtnMessage = send("transWCSExecuteTask",Json);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //WCSDataService.WCSDataService wcs = new WCSDataService.WCSDataService();
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable Jdt = bll.FillDataTable("Wcs.SelectTaskWcsFinish", new DataParameter("{0}", "1701120037"));

            string Json = Util.JsonHelper.Dtb2Json(Jdt, "yyyy-MM-dd HH:mm:ss.fff");
            string rtnMessage = send("transWCSTaskStatus",Json);
            //string m = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string id = Guid.NewGuid().ToString();
            string deviceNo = "0101";
            string mode = "1";
            string status = "1";
            string taskNo = "20170316001";
            string fork = "1";
            string load = "1";
            string aisleNo = "01";
            string column = "1";
            string layer = "1";
            string alarmCode = "0";
            string sender1 = "admin";

            string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
            string rtnMessage = send("transWCSDevice",Json);
        }
        public string send(string method,string data)
        {
            string url = "http://localhost/servicehost/SRMDataService.svc/transWCSTask";
            url = "http://192.168.0.235:3000/AW/ROBO2XN/" + method;
            url = "http://192.200.105.198:8080/api/RB2MJ/" + method;
            url = "http://localhost/ServiceHost/SRMDataService.svc/" + method;
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

        private void button4_Click(object sender, EventArgs e)
        {

            string Json = "[{\"field1\": \"null\",\"field2\": \"null\",\"field3\":\"null\",\"fromAddress\": \"101010\",\"id\": \"0001\",\"palletBarcode\": \"00001\",\"sendDate\":\"2017-03-15 02:21:20.411\",\"sender\":\"admin\",\"status\":\"RECEIVED\",\"taskLevel\":\"0\",\"taskFlag\":\"NORMAL\",\"taskNo\":\"string\",\"taskType\":\"T_11\",\"toAddress\":\"202010\",\"warehouseCode\":\"XN_B\"}]";
            //Json = "param=[{\"field1\": \"null\",\"field2\": \"null\",\"field3\":\"null\",\"fromAddress\": \"101010\",\"id\": \"0001\",\"palletBarcode\": \"00001\",\"sendDate\":\"2017-03-15 02:21:20.411\",\"sender\":\"admin\",\"status\":\"RECEIVED\",\"taskLevel\":\"0\",\"taskFlag\":\"NORMAL\",\"taskNo\":\"string\",\"taskType\":\"T_11\",\"toAddress\":\"202010\",\"warehouseCode\":\"XN_A\"}]";
            //var client = new RestClient();  
            //client.EndPoint = @"http://localhost/ROBOService/SRMDataService.svc/transSRMTask"; ;  
            //client.ContentType = "application/json";  
            //client.Method = HttpVerb.POST;  
            //client.PostData = "";  
            //var json = client.MakeRequest();
            //string f = "[{\"field1\": \"null\"}]";
            //f = "";
            //string rtnMessage = send("transSRMTask", f);
            //Json = "123";
            string param = "appKey=44hbf622op&username=13011001233&sign=123456";
            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            byte[] postBytes = Encoding.UTF8.GetBytes(Json);


            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://localhost/serviceHost/transSRMTaskAisle.ashx");
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();                        
                    }
                }
            }
            catch (WebException ex)
            {
                
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string Json = "[{\"field1\": \"null\",\"field2\": \"null\",\"field3\":\"null\",\"fromAddress\": \"A001001001\",\"id\": \"0001\",\"palletBarcode\": \"00001\",\"sendDate\":\"2017-03-15 02:21:20.411\",\"sender\":\"admin\",\"status\":\"RECEIVED\",\"taskLevel\":\"0\",\"taskFlag\":\"NORMAL\",\"taskNo\":\"string15\",\"taskType\":\"T_11\",\"toAddress\":\"A001001002\",\"warehouseCode\":\"XN_A\"}]";

            //Json = "{\"key\":\"1\"}";
            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            byte[] postBytes = Encoding.UTF8.GetBytes(Json);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://127.0.0.1/ServiceHost/transSRMTask.ashx");
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       
    }
}
