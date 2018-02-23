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
    public partial class frmDeviceError : Form
    {
        public frmDeviceError()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        public string filter = "1=1";
        string DeviceType = "";
        bool checkselcet = true;
        private void frmDeviceError_Load(object sender, EventArgs e)
        {
            dtpTaskDate1.Value = dtpTaskDate2.Value.Add(new TimeSpan(-31, 0, 0, 0));
            BindData();
            cmbAlarm.Items.Insert(0,"all");
            cmbAlarm.SelectedIndex = 0;
        }

        private void BindData()
        {
            bsAlarm.DataSource = GetMonitorData();
            
            cmbAlarm.DisplayMember = "AlarmCD";

            DataTable dtAisle = bll.FillDataTable("CMD.SelectAisle", new DataParameter("{0}", string.Format("WareHouseCode='{0}'", Program.WarehouseCode)));

            cmbAisle.DataSource = dtAisle;
            DataRow drAisle = dtAisle.NewRow();
            drAisle["AisleNo"] = "全选";
            dtAisle.Rows.InsertAt(drAisle, 0);
            cmbAisle.DisplayMember = "AisleNo";
            cmbAisle.SelectedIndex = 0;
        }
        private DataTable GetMonitorData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=2") });
            dt = UniteDataTableColumns(dt, "AlarmCD", "AlarmCode", "AlarmDesc");
            DataRow dr = dt.NewRow();
            dr["Flag"] = 2;
            dr["DeviceType"] = "2";
            dr["AlarmCode"] = 0;
            dr["AlarmDesc"] = "全选";
            dr["AlarmCD"] = "全选";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public static DataTable UniteDataTableColumns(DataTable dt, String newColumnName, string ColumnName1, string ColumnName2)
        {
            //汇总的表达式
            string expression = "";
            expression = String.Format("{0}+'('+{1}+')'", ColumnName1, ColumnName2);
            //增加汇总列
            System.Type myDataType = System.Type.GetType("System.String");
            DataColumn dcCol = new DataColumn(newColumnName, myDataType, expression, MappingType.Attribute);
            //增加合并列
            dt.Columns.Add(dcCol);
            return dt;
        }

        private void cmbAisle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int aisleNo = cmbAisle.SelectedIndex;
            DataTable dtDevice = bll.FillDataTable("CMD.SelectAisleDeviceChart", new DataParameter("{0}", string.Format("AisleNo='{0}' and WarehouseCode= '{1}'", "0" + aisleNo.ToString(), Program.WarehouseCode)));
            cmbDevice.DataSource = dtDevice;
            cmbDevice.DisplayMember = "DeviceNo2";
        }

        private void btnCk_Click(object sender, EventArgs e)
        {
            string WarehouseCode = Program.WarehouseCode;
            if (WarehouseCode=="S")
            {
                DeviceType = "2";
            }
            else
            {
                DeviceType = "01";
            }
            if (cmbAlarm.SelectedIndex==0)
            {
                if (cmbAisle.SelectedIndex==0)
                {
                    filter = string.Format("C.WarehouseCode='{0}' and D.DeviceType='{1}'", WarehouseCode, DeviceType);
                }
                else
                {
                    filter = string.Format("C.WarehouseCode='{0}' and D.DeviceType='{1}' and C.AisleNo='0{2}' and R.DeviceNo = '{3}'", WarehouseCode, DeviceType, cmbAisle.SelectedIndex.ToString(),cmbDevice.Text); 
                }
            }
            else
            {
                if (cmbAisle.SelectedIndex==0)
                {
                    filter = string.Format("C.WarehouseCode='{0}' and D.DeviceType='{1}' and R.AlarmCode='{2}'", WarehouseCode, DeviceType, cmbAlarm.SelectedIndex.ToString()); 
                }
                else
                {
                    filter = string.Format("C.WarehouseCode='{0}' and D.DeviceType='{1}' and C.AisleNo='0{2}' and R.DeviceNo = '{3}' and  R.AlarmCode='{4}'", WarehouseCode, DeviceType, cmbAisle.SelectedIndex.ToString(), cmbDevice.Text, cmbAlarm.SelectedIndex.ToString());
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

    }
}
