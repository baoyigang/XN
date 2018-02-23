namespace App.View.Report
{
    partial class frmDeviceError
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpTaskDate2 = new System.Windows.Forms.DateTimePicker();
            this.dtpTaskDate1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAisle = new System.Windows.Forms.Label();
            this.cmbAlarm = new System.Windows.Forms.ComboBox();
            this.bsAlarm = new System.Windows.Forms.BindingSource(this.components);
            this.lblAlarm = new System.Windows.Forms.Label();
            this.cmbAisle = new System.Windows.Forms.ComboBox();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.btnCk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bsAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.label3.Location = new System.Drawing.Point(320, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 27);
            this.label3.TabIndex = 57;
            this.label3.Text = "~";
            // 
            // dtpTaskDate2
            // 
            this.dtpTaskDate2.Checked = false;
            this.dtpTaskDate2.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.dtpTaskDate2.Location = new System.Drawing.Point(353, 49);
            this.dtpTaskDate2.Name = "dtpTaskDate2";
            this.dtpTaskDate2.Size = new System.Drawing.Size(201, 34);
            this.dtpTaskDate2.TabIndex = 56;
            // 
            // dtpTaskDate1
            // 
            this.dtpTaskDate1.Checked = false;
            this.dtpTaskDate1.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.dtpTaskDate1.Location = new System.Drawing.Point(131, 49);
            this.dtpTaskDate1.Name = "dtpTaskDate1";
            this.dtpTaskDate1.Size = new System.Drawing.Size(183, 34);
            this.dtpTaskDate1.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.label2.Location = new System.Drawing.Point(39, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 31);
            this.label2.TabIndex = 54;
            this.label2.Text = "时间：";
            // 
            // lblAisle
            // 
            this.lblAisle.AutoSize = true;
            this.lblAisle.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAisle.Location = new System.Drawing.Point(15, 171);
            this.lblAisle.Name = "lblAisle";
            this.lblAisle.Size = new System.Drawing.Size(123, 31);
            this.lblAisle.TabIndex = 61;
            this.lblAisle.Text = "巷道设备 :";
            // 
            // cmbAlarm
            // 
            this.cmbAlarm.DataSource = this.bsAlarm;
            this.cmbAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlarm.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.cmbAlarm.FormattingEnabled = true;
            this.cmbAlarm.Location = new System.Drawing.Point(161, 107);
            this.cmbAlarm.Name = "cmbAlarm";
            this.cmbAlarm.Size = new System.Drawing.Size(254, 31);
            this.cmbAlarm.TabIndex = 62;
            // 
            // lblAlarm
            // 
            this.lblAlarm.AutoSize = true;
            this.lblAlarm.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.lblAlarm.Location = new System.Drawing.Point(15, 107);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(123, 31);
            this.lblAlarm.TabIndex = 63;
            this.lblAlarm.Text = "错误代码 :";
            // 
            // cmbAisle
            // 
            this.cmbAisle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAisle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAisle.FormattingEnabled = true;
            this.cmbAisle.Location = new System.Drawing.Point(161, 176);
            this.cmbAisle.Name = "cmbAisle";
            this.cmbAisle.Size = new System.Drawing.Size(121, 29);
            this.cmbAisle.TabIndex = 64;
            this.cmbAisle.SelectedIndexChanged += new System.EventHandler(this.cmbAisle_SelectedIndexChanged);
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(310, 176);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(124, 29);
            this.cmbDevice.TabIndex = 65;
            // 
            // btnCk
            // 
            this.btnCk.Location = new System.Drawing.Point(223, 288);
            this.btnCk.Name = "btnCk";
            this.btnCk.Size = new System.Drawing.Size(164, 40);
            this.btnCk.TabIndex = 66;
            this.btnCk.Text = "查询";
            this.btnCk.UseVisualStyleBackColor = true;
            this.btnCk.Click += new System.EventHandler(this.btnCk_Click);
            // 
            // frmDeviceError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 500);
            this.Controls.Add(this.btnCk);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.cmbAisle);
            this.Controls.Add(this.lblAlarm);
            this.Controls.Add(this.cmbAlarm);
            this.Controls.Add(this.lblAisle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpTaskDate2);
            this.Controls.Add(this.dtpTaskDate1);
            this.Controls.Add(this.label2);
            this.Name = "frmDeviceError";
            this.Text = "frmDeviceError";
            this.Load += new System.EventHandler(this.frmDeviceError_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsAlarm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpTaskDate2;
        private System.Windows.Forms.DateTimePicker dtpTaskDate1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAisle;
        private System.Windows.Forms.ComboBox cmbAlarm;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.BindingSource bsAlarm;
        private System.Windows.Forms.ComboBox cmbAisle;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.Button btnCk;
    }
}