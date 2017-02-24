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

        private void button1_Click(object sender, EventArgs e)
        {
            SRMDataService.SRMDataService srm = new SRMDataService.SRMDataService();
            string Json = "[{\"id\":\"xcafd3ssaf\",\"taskNo\":\"1702160001\",\"taskType\":\"12\",\"taskLevel\":1,\"taskFlag\":\"2\",\"palletBarcode\":\"B001\",\"areaCode\":\"B01\",\"fromAddress\":\"B005001001\",\"toAddress\":\"B005006003\",\"status\":\"0\",\"sendDate\":\""+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")+"\",\"sender\":\"admin\"" + "}]";
            string json = "";
            //string Ajson = "[{\"id\":\"xcafd3ssaf\",\"taskNo\":\"1702160001\",\"areaCode\":\"A01\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"admin\"" + "}]";
            //string a = srm.transSRMTaskAisle(Ajson);
            string c = srm.transSRMTask(Json);
            MessageBox.Show(c);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WCSDataService.WCSDataService wcs = new WCSDataService.WCSDataService();
            wcs.transWCSExecuteTask("170216");
        }
    }
}
