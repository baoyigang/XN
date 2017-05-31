using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;

namespace App
{
    static class Program
    {
        public static Main mainForm;
        public static DataTable dtUserPermission;
        public static string WarehouseCode;
        private static string WcsUrl;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Main();
            
            bool ExisFlag = false;
            System.Diagnostics.Process currentProccess = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] currentProccessArray = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in currentProccessArray)
            {
                if (p.ProcessName == currentProccess.ProcessName && p.Id != currentProccess.Id)
                {
                    ExisFlag = true;
                    break;
                }
            }

            if (ExisFlag)
            {
                MessageBox.Show("仓储调度监控系统！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Account.frmLogin myLogin = new Account.frmLogin();

                if (myLogin.ShowDialog() == DialogResult.OK)
                {
                    MCP.Config.Configuration conf = new MCP.Config.Configuration();
                    conf.Load("Config.xml");
                    WarehouseCode = conf.Attributes["WarehouseCode"];
                    WcsUrl = conf.Attributes["WcsUrl"];
                    Application.Run(mainForm);
                }
            }
        }
        public static string send(string method, string data)
        {
            string url = WcsUrl + method;

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
