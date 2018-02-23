using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Report
{
    public partial class frmDeviceErrorDetail : BaseForm
    {
        public frmDeviceErrorDetail()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        string parentFilter = "C.WarehouseCode=''";
        private void toolStripButton_Query_Click(object sender, EventArgs e)
        {
            frmDeviceError f = new frmDeviceError();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BindData(f.filter);
            }
        }

        private void BindData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectTask", new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.WarehouseCode = '{0}' and WCS_TASK.State in('0','1','2','3','7') and convert(varchar(10),WCS_TASK.TaskDate,120)=convert(varchar(10),getdate(),120) and WCS_TASK.TaskType='11'", Program.WarehouseCode)) });
            bsMain.DataSource = dt;
        }
        private void BindData(string filter)
        {
            parentFilter = filter;
            DataTable dt = bll.FillDataTable("WCS.SelectAlarmRecord", new DataParameter("{0}", filter));
            bsMain.DataSource = dt;
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BindData(parentFilter);
        }
    }
}
