using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            SRMDataService.SRMDataService srm = new SRMDataService.SRMDataService();
            string Json = "c";
            string json = "";
            //string Ajson = "[{\"id\":\"xcafd3ssaf\",\"taskNo\":\"1702160001\",\"areaCode\":\"A01\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"admin\"" + "}]";
            //string a = srm.transSRMTaskAisle(Ajson);
            string c = srm.transSRMTask(Json);
            MessageBox.Show(c);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WCSDataService.WCSDataService wcs = new WCSDataService.WCSDataService();
            wcs.transWCSExecuteTask("1612140001");
            //string m = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
        }

       
    }
}
