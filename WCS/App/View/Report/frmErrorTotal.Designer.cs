﻿namespace App.View.Report
{
    partial class frmErrorTotal
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.cmbAlarm = new System.Windows.Forms.ComboBox();
            this.bsAlarm = new System.Windows.Forms.BindingSource(this.components);
            this.btnCheck = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnPie = new System.Windows.Forms.Button();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblAlarmTimes = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblAlarmTimeNum = new System.Windows.Forms.Label();
            this.lblDateNum = new System.Windows.Forms.Label();
            this.lblDeviceNo = new System.Windows.Forms.Label();
            this.lblDeviceNoNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAlarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1513, 787);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(809, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "~";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(650, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "日期";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.Checked = false;
            this.dateTimePicker2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker2.Location = new System.Drawing.Point(832, 11);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(128, 23);
            this.dateTimePicker2.TabIndex = 9;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Location = new System.Drawing.Point(688, 11);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(115, 23);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.Value = new System.DateTime(2018, 2, 6, 11, 51, 26, 0);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.White;
            this.lblTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTime.Location = new System.Drawing.Point(1390, 731);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(71, 20);
            this.lblTime.TabIndex = 19;
            this.lblTime.Text = "日期/时间";
            // 
            // lblAlarm
            // 
            this.lblAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlarm.AutoSize = true;
            this.lblAlarm.BackColor = System.Drawing.Color.White;
            this.lblAlarm.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblAlarm.Location = new System.Drawing.Point(967, 14);
            this.lblAlarm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(63, 17);
            this.lblAlarm.TabIndex = 65;
            this.lblAlarm.Text = "故障代码 :";
            // 
            // cmbAlarm
            // 
            this.cmbAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAlarm.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAlarm.DataSource = this.bsAlarm;
            this.cmbAlarm.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbAlarm.FormattingEnabled = true;
            this.cmbAlarm.Location = new System.Drawing.Point(1038, 9);
            this.cmbAlarm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAlarm.Name = "cmbAlarm";
            this.cmbAlarm.Size = new System.Drawing.Size(172, 25);
            this.cmbAlarm.TabIndex = 64;
            this.cmbAlarm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbAlarm_KeyPress);
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheck.Location = new System.Drawing.Point(1235, 9);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 66;
            this.btnCheck.Text = "查询";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label3.Location = new System.Drawing.Point(650, 53);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 70;
            this.label3.Text = "设备选择 :";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.ColumnWidth = 50;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(711, 52);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(701, 16);
            this.checkedListBox1.TabIndex = 71;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnPie
            // 
            this.btnPie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPie.Location = new System.Drawing.Point(1326, 14);
            this.btnPie.Name = "btnPie";
            this.btnPie.Size = new System.Drawing.Size(75, 23);
            this.btnPie.TabIndex = 72;
            this.btnPie.Text = "按设备查询";
            this.btnPie.UseVisualStyleBackColor = true;
            this.btnPie.Click += new System.EventHandler(this.btnPie_Click);
            // 
            // chart2
            // 
            chartArea2.Area3DStyle.Enable3D = true;
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(0, 0);
            this.chart2.Name = "chart2";
            this.chart2.Size = new System.Drawing.Size(1513, 787);
            this.chart2.TabIndex = 73;
            this.chart2.Text = "chart2";
            this.chart2.Visible = false;
            // 
            // lblAlarmTimes
            // 
            this.lblAlarmTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlarmTimes.AutoSize = true;
            this.lblAlarmTimes.BackColor = System.Drawing.Color.White;
            this.lblAlarmTimes.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAlarmTimes.Location = new System.Drawing.Point(1172, 241);
            this.lblAlarmTimes.Name = "lblAlarmTimes";
            this.lblAlarmTimes.Size = new System.Drawing.Size(106, 22);
            this.lblAlarmTimes.TabIndex = 74;
            this.lblAlarmTimes.Text = "总故障次数：";
            this.lblAlarmTimes.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.White;
            this.lblDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.Location = new System.Drawing.Point(1172, 290);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(90, 22);
            this.lblDate.TabIndex = 75;
            this.lblDate.Text = "日期区间：";
            this.lblDate.Visible = false;
            // 
            // lblAlarmTimeNum
            // 
            this.lblAlarmTimeNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlarmTimeNum.AutoSize = true;
            this.lblAlarmTimeNum.BackColor = System.Drawing.Color.White;
            this.lblAlarmTimeNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAlarmTimeNum.Location = new System.Drawing.Point(1284, 241);
            this.lblAlarmTimeNum.Name = "lblAlarmTimeNum";
            this.lblAlarmTimeNum.Size = new System.Drawing.Size(59, 22);
            this.lblAlarmTimeNum.TabIndex = 76;
            this.lblAlarmTimeNum.Text = "label4";
            this.lblAlarmTimeNum.Visible = false;
            // 
            // lblDateNum
            // 
            this.lblDateNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateNum.AutoSize = true;
            this.lblDateNum.BackColor = System.Drawing.Color.White;
            this.lblDateNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDateNum.Location = new System.Drawing.Point(1268, 290);
            this.lblDateNum.Name = "lblDateNum";
            this.lblDateNum.Size = new System.Drawing.Size(59, 22);
            this.lblDateNum.TabIndex = 77;
            this.lblDateNum.Text = "label5";
            this.lblDateNum.Visible = false;
            // 
            // lblDeviceNo
            // 
            this.lblDeviceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceNo.AutoSize = true;
            this.lblDeviceNo.BackColor = System.Drawing.Color.White;
            this.lblDeviceNo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDeviceNo.Location = new System.Drawing.Point(1174, 200);
            this.lblDeviceNo.Name = "lblDeviceNo";
            this.lblDeviceNo.Size = new System.Drawing.Size(90, 22);
            this.lblDeviceNo.TabIndex = 78;
            this.lblDeviceNo.Text = "设备编号：";
            this.lblDeviceNo.Visible = false;
            // 
            // lblDeviceNoNum
            // 
            this.lblDeviceNoNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceNoNum.AutoSize = true;
            this.lblDeviceNoNum.BackColor = System.Drawing.Color.White;
            this.lblDeviceNoNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDeviceNoNum.Location = new System.Drawing.Point(1270, 200);
            this.lblDeviceNoNum.Name = "lblDeviceNoNum";
            this.lblDeviceNoNum.Size = new System.Drawing.Size(59, 22);
            this.lblDeviceNoNum.TabIndex = 79;
            this.lblDeviceNoNum.Text = "label5";
            this.lblDeviceNoNum.Visible = false;
            // 
            // frmErrorTotal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1513, 787);
            this.Controls.Add(this.lblDeviceNoNum);
            this.Controls.Add(this.lblDeviceNo);
            this.Controls.Add(this.lblDateNum);
            this.Controls.Add(this.lblAlarmTimeNum);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblAlarmTimes);
            this.Controls.Add(this.btnPie);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.lblAlarm);
            this.Controls.Add(this.cmbAlarm);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.chart2);
            this.Name = "frmErrorTotal";
            this.Text = "frmErrorTotal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmErrorTotal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAlarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.ComboBox cmbAlarm;
        private System.Windows.Forms.BindingSource bsAlarm;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPie;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblAlarmTimes;
        private System.Windows.Forms.Label lblDateNum;
        private System.Windows.Forms.Label lblAlarmTimeNum;
        private System.Windows.Forms.Label lblDeviceNoNum;
        private System.Windows.Forms.Label lblDeviceNo;
    }
}