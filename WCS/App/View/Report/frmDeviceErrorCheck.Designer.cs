namespace App.View.Report
{
    partial class frmDeviceErrorCheck
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeviceErrorCheck));
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.pnlTool = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Query = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Close = new System.Windows.Forms.ToolStripButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.DeviceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AisleNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AlarmCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AlarmDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WarehouseCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            this.pnlTool.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlContent);
            this.pnlMain.Controls.Add(this.pnlTool);
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1052, 572);
            this.pnlMain.TabIndex = 6;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 56);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1052, 460);
            this.pnlContent.TabIndex = 1;
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceNo,
            this.AisleNo,
            this.AlarmCode,
            this.AlarmDesc,
            this.AreaCode,
            this.WarehouseCode,
            this.StartDate,
            this.EndDate,
            this.duration});
            this.dgvMain.DataSource = this.bsMain;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMain.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvMain.RowHeadersWidth = 40;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(1052, 460);
            this.dgvMain.TabIndex = 5;
            // 
            // pnlTool
            // 
            this.pnlTool.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlTool.Controls.Add(this.toolStrip1);
            this.pnlTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTool.Location = new System.Drawing.Point(0, 0);
            this.pnlTool.Name = "pnlTool";
            this.pnlTool.Size = new System.Drawing.Size(1052, 56);
            this.pnlTool.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton_Query,
            this.toolStripButton_Close});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1052, 56);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton1.Text = "刷新";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton_Query
            // 
            this.toolStripButton_Query.AutoSize = false;
            this.toolStripButton_Query.Image = global::App.Properties.Resources.zoom;
            this.toolStripButton_Query.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Query.Name = "toolStripButton_Query";
            this.toolStripButton_Query.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_Query.Text = "查询";
            this.toolStripButton_Query.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Query.Click += new System.EventHandler(this.toolStripButton_Query_Click);
            // 
            // toolStripButton_Close
            // 
            this.toolStripButton_Close.AutoSize = false;
            this.toolStripButton_Close.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Close.Image")));
            this.toolStripButton_Close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Close.Name = "toolStripButton_Close";
            this.toolStripButton_Close.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_Close.Text = "关闭";
            this.toolStripButton_Close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 516);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1052, 56);
            this.pnlBottom.TabIndex = 2;
            this.pnlBottom.Visible = false;
            // 
            // DeviceNo
            // 
            this.DeviceNo.DataPropertyName = "DeviceNo";
            this.DeviceNo.HeaderText = "设备编号";
            this.DeviceNo.Name = "DeviceNo";
            this.DeviceNo.ReadOnly = true;
            // 
            // AisleNo
            // 
            this.AisleNo.DataPropertyName = "AisleNo";
            this.AisleNo.HeaderText = "巷道编号";
            this.AisleNo.Name = "AisleNo";
            this.AisleNo.ReadOnly = true;
            // 
            // AlarmCode
            // 
            this.AlarmCode.DataPropertyName = "AlarmCode";
            this.AlarmCode.HeaderText = "故障编码";
            this.AlarmCode.Name = "AlarmCode";
            this.AlarmCode.ReadOnly = true;
            // 
            // AlarmDesc
            // 
            this.AlarmDesc.DataPropertyName = "AlarmDesc";
            this.AlarmDesc.HeaderText = "故障描述";
            this.AlarmDesc.Name = "AlarmDesc";
            this.AlarmDesc.ReadOnly = true;
            // 
            // AreaCode
            // 
            this.AreaCode.DataPropertyName = "AreaCode";
            this.AreaCode.HeaderText = "区域编码";
            this.AreaCode.Name = "AreaCode";
            this.AreaCode.ReadOnly = true;
            // 
            // WarehouseCode
            // 
            this.WarehouseCode.DataPropertyName = "WarehouseCode";
            this.WarehouseCode.HeaderText = "仓库编码";
            this.WarehouseCode.Name = "WarehouseCode";
            this.WarehouseCode.ReadOnly = true;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.HeaderText = "起始时间";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            this.StartDate.Width = 150;
            // 
            // EndDate
            // 
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.HeaderText = "结束时间";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            this.EndDate.Width = 150;
            // 
            // duration
            // 
            this.duration.DataPropertyName = "duration";
            this.duration.HeaderText = "持续时间";
            this.duration.Name = "duration";
            this.duration.ReadOnly = true;
            // 
            // frmDeviceErrorCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 572);
            this.Controls.Add(this.pnlMain);
            this.Name = "frmDeviceErrorCheck";
            this.Text = "frmDeviceErrorCheck";
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            this.pnlTool.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.Panel pnlTool;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Query;
        private System.Windows.Forms.ToolStripButton toolStripButton_Close;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.BindingSource bsMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AisleNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AlarmCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn AlarmDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn WarehouseCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn duration;

    }
}