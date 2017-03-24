using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Base
{
    public partial class frmDeviceEdit : Form
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        string ProductCode = "";
        DataRow dr;

        public frmDeviceEdit()
        {
            InitializeComponent();
        }
        public frmDeviceEdit(DataRow dr)
        {
            InitializeComponent();
            this.dr = dr;
            this.ProductCode = dr["ProductCode"].ToString();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtProductCode.Text.Trim().Length <= 0)
            {
                MessageBox.Show("产品编号不能为空,请输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtProductCode.Focus();
                return;
            }
            if (this.txtProductName.Text.Trim().Length <= 0)
            {
                MessageBox.Show("产品名称不能为空,请输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtProductName.Focus();
                return;
            }
            if (this.txtSpec.Text.Trim().Length <= 0)
            {
                MessageBox.Show("规格不能为空,请输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtSpec.Focus();
                return;
            }

            if (ProductCode.Length <= 0) //新增
            {
                int Count = bll.GetRowCount("CMD_Product", string.Format("ProductCode='{0}'", this.txtProductCode.Text.Trim()));
                if (Count > 0)
                {
                    MessageBox.Show("该编号已经存在,请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtProductCode.Focus();
                    return;
                }

                bll.ExecNonQuery("Cmd.InsertProduct", new DataParameter[] { 
                            new DataParameter("@ProductCode", this.txtProductCode.Text.Trim()),
                            new DataParameter("@ProductName", this.txtProductName.Text.Trim()),
                            new DataParameter("@Spec", this.txtSpec.Text.Trim()),
                            new DataParameter("@Weight", this.txtWeight.Text.Trim()),
                            new DataParameter("@IsProduce", this.ckbIsProduce.Checked?"1":"0"),                           
                            new DataParameter("@ValidPeriod", this.txtValidPeriod.Text.Trim()),
                            new DataParameter("@WarehouseCode", "001"),
                            new DataParameter("@AreaCode", "001"),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@ProductNo", this.cmbProductNo.Text.Trim()),
                            new DataParameter("@Creator", "admin"),
                            new DataParameter("@Updater", "admin")
                          
                });
            }
            else //修改
            {
                bll.ExecNonQuery("Cmd.UpdateProduct", new DataParameter[] {  
                            new DataParameter("@ProductCode", this.txtProductCode.Text.Trim()),
                            new DataParameter("@ProductName", this.txtProductName.Text.Trim()),
                            new DataParameter("@Spec", this.txtSpec.Text.Trim()),
                            new DataParameter("@Weight", this.txtWeight.Text.Trim()),
                            new DataParameter("@IsProduce", this.ckbIsProduce.Checked?"1":"0"),                           
                            new DataParameter("@ValidPeriod", this.txtValidPeriod.Text.Trim()),
                            new DataParameter("@Memo", this.txtMemo.Text.Trim()),
                            new DataParameter("@ProductNo", this.cmbProductNo.Text.Trim()),
                            new DataParameter("@Updater", "admin")
                 });
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void frmProductEdit_Load(object sender, EventArgs e)
        {
            DataTable dt = bll.FillDataTable("CMD.SelectRelationNo");
            this.cmbProductNo.DataSource = dt.DefaultView;
            this.cmbProductNo.ValueMember = "ProductNo";
            this.cmbProductNo.DisplayMember = "ProductNo";
            if (this.ProductCode.Length > 0)
            {
                this.txtProductCode.Text = dr["ProductCode"].ToString();
                this.txtProductName.Text = dr["ProductName"].ToString();
                this.cmbProductNo.Text = dr["ProductNo"].ToString();
                this.txtSpec.Text = dr["Spec"].ToString();
                this.txtValidPeriod.Text = dr["ValidPeriod"].ToString();
                this.txtWeight.Text = dr["Weight"].ToString();
                this.txtMemo.Text = dr["Memo"].ToString();
                if (dr["IsProduce"].ToString() == "1")
                    this.ckbIsProduce.Checked = true;
                else
                    this.ckbIsProduce.Checked = false;
                this.txtProductCode.ReadOnly = true;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
