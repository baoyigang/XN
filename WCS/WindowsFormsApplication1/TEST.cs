using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class TEST : Form
    {
        public TEST()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        private void TEST_Load(object sender, EventArgs e)
        {
            DataTable dt = bll.FillDataTable("CMD.SelectAisle", new DataParameter("{0}", string.Format("WareHouseCode='{0}'", "S")));
            int AisleNoCount = dt.Rows.Count;
            DataTable dtDevice;

            for (int i = 1; i < AisleNoCount + 1; i++)
            {
                dtDevice = bll.FillDataTable("Cmd.SelectAisleDeviceChart", new DataParameter("{0}", string.Format("WareHouseCode='{0}' and AisleNo='{1}'", "S", "0" + i.ToString())));
                for (int j = 1; j < dtDevice.Rows.Count + 1; j++)
                {
                    chart1.Series.Add(new Series(dtDevice.Rows[j - 1]["DeviceNo2"].ToString()));
                }
                chart1.Series.Add(new Series(i.ToString() + "号巷道"));
            }
            chart1.Series.Add(new Series("任务数"));
        }
    }
}
