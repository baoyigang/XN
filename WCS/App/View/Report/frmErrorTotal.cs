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

namespace App.View.Report
{
    public partial class frmErrorTotal : Form
    {
        public frmErrorTotal()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        List<string> XDate = new List<string>();
        List<int> YTime = new List<int>();
        List<string> XDevice = new List<string>();
        List<string> XAlarm = new List<string>();
        string stardate;
        string enddate;
        public int AisleNoCount { get; set; }
        public int AisleStart { get; set; }
        public string DeviceType { get; set; }
        bool result = false;

        private void frmErrorTotal_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker2.Value.AddMonths(-1);


            if (Program.WarehouseCode=="S")
            {
                DeviceType = "02";
            }
            else
            {
                DeviceType = "01";
            }
            BindData();
            DataTable dt = bll.FillDataTable("CMD.SelectDevice", new DataParameter("{0}", string.Format("WarehouseCode='{0}'and DeviceType='{1}'", Program.WarehouseCode, DeviceType)));
            foreach (DataRow item in dt.Rows)
            { 
                
                  checkedListBox1.Items.Add(item["DeviceNo2"]);
            }

            DataTable dt1 = bll.FillDataTable("CMD.SelectAisle", new DataParameter("{0}", string.Format("WareHouseCode='{0}'", Program.WarehouseCode)));
            AisleStart = int.Parse(dt1.Rows[0]["AisleNo"].ToString());
            AisleNoCount = dt1.Rows.Count;
            DataTable dtDevice;
            for (int i = AisleStart; i < AisleNoCount + AisleStart; i++)
            {
                dtDevice = bll.FillDataTable("Cmd.SelectAisleDeviceChart", new DataParameter("{0}", string.Format("WareHouseCode='{0}' and AisleNo='{1}' and Priority=1", Program.WarehouseCode, "0" + i.ToString())));
                for (int j = 1; j < dtDevice.Rows.Count + 1; j++)
                {
                    chart1.Series.Add(new Series(dtDevice.Rows[j - 1]["DeviceNo2"].ToString()));
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].IsVisibleInLegend = false;
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].ChartType = SeriesChartType.Column;
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].Label = "#VAL";
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top;
                }
            }
            chart1.Series.Add(new Series("故障频次"));
            chart1.Series["故障频次"].Label = "#VAL";
            chart1.Series["故障频次"].Color = Color.Blue;
            chart1.Series["故障频次"].ChartType = SeriesChartType.Column;

            chart2.Series.Add(new Series("故障频次(圆)"));
            chart2.Series["故障频次(圆)"].ChartType = SeriesChartType.Pie;
            chart2.Series["故障频次(圆)"].Label = "#PERCENT{P} 编号:#VALX";
            chart2.Series["故障频次(圆)"].Font = new Font("微软雅黑", 12);
            chart2.Series["故障频次(圆)"].XValueType = ChartValueType.String;
            chart2.Series["故障频次(圆)"].LegendText = "#VALX";
            chart2.Series["故障频次(圆)"].CustomProperties = "PieStartAngle=270";
            chart2.MouseClick+=new MouseEventHandler(chart2_MouseClick);

            chart2.Series.Add(new Series("单设备故障分布"));
            chart2.Series["单设备故障分布"].ChartType = SeriesChartType.Pie;
            chart2.Series["单设备故障分布"].Label = "#PERCENT{P} 故障代码:#VALX";
            chart2.Series["单设备故障分布"].Font = new Font("微软雅黑", 12);
            chart2.Series["单设备故障分布"].XValueType = ChartValueType.String;
            chart2.Series["单设备故障分布"].LegendText = "#VALX";
            chart2.Series["单设备故障分布"].CustomProperties = "PieStartAngle=270";


            this.MouseWheel += new MouseEventHandler(Rfrm_MouseWheel);
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetChart("0", 0, stardate, enddate);
        }
        private void BindData()
        {
            bsAlarm.DataSource = GetMonitorData();

            cmbAlarm.DisplayMember = "AlarmCD";
            cmbAlarm.ValueMember = "AlarmCode";
        }
        private DataTable GetMonitorData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", string.Format("DeviceType='{0}'", DeviceType)) });
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
        //设置故障选择不可编辑
        private void cmbAlarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void GetChart(string Device,int Alarm, string stardate, string enddate)
        {
            try
            {
                XDate.Clear();
                YTime.Clear();
                DataParameter[] param;
                if (Alarm == 0)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("D.DeviceType='{3}' and C.WarehouseCode='{2}' and CONVERT(varchar(12) , R.EndDate, 111 ) between '{0}' and '{1}'", stardate, enddate, Program.WarehouseCode, DeviceType)) };
                }
                else
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("D.DeviceType='{4}' and R.AlarmCode={3} and C.WarehouseCode='{2}'  and  CONVERT(varchar(12) , R.EndDate, 111 ) between '{0}' and '{1}'", stardate, enddate, Program.WarehouseCode, Alarm, DeviceType)) };
                }
                DataTable dt = bll.FillDataTable("WCS.SelectAlarmRecord", param);
                TimeSpan t1 = dateTimePicker2.Value - dateTimePicker1.Value;
                int days = t1.Days;
                if (Device=="0")
                {
                    for (int i = 0; i < days + 1; i++)
                    {
                        string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                        int count = (from DataRow AlarmCount in dt.Rows select AlarmCount).Where(s => ((DateTime)s["EndDate"]).ToString("yyyy-MM-dd") == ymd).Count();
                        if (count != 0)
                        {

                            string d = ymd.Substring(5);
                            XDate.Add(d);

                            YTime.Add(count);
                        }
                    }
                    chart1.Series["故障频次"].Points.DataBindXY(XDate, YTime);
                }
                else
                {
                    var a = chart1.Series["故障频次"].Points.Select(s => s.AxisLabel).ToList();
                    //XDate.AddRange(a);
                    for (int i = 0; i < a.Count; i++)
                    {
                        YTime.Add(0);
                    }
                    for (int i = 0; i < days + 1; i++)
                    {
                        DataRow[] drs = dt.Select(string.Format("DeviceNo='{0}'", Device));
                        string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                        int count = (from DataRow AlarmCount in drs select AlarmCount).Where(s => ((DateTime)s["EndDate"]).ToString("yyyy-MM-dd") == ymd).Count();
                        if (count!=0)
                        {
                            string d = ymd.Substring(5);
                           
             
                            for (int j = 0; j < a.Count; j++)
                            {
                                if (a[j] == d)
                                {
                                    if (YTime[j] != 0)
                                    {
                                        YTime[j] = YTime[j] + count;
                                    }
                                    else
                                    {
                                        YTime[j] = count;
                                    }
                                    //chart1.Series[Device].Points.InsertXY(j, d, count);
                                    XDate.Add(d);
                                }
                                else
                                {
                                    //if (chart1.Series[Device].Points[j].YValues[0] == 0)
                                    //{

                                    //}
                                    XDate.Add("");

                                }
                            }
                            
                            //XDate.Clear();
                            //YTime.Clear();
                        }
                    }
                    chart1.Series[Device].Points.DataBindXY(XDate, YTime);
                }
            }
            catch(Exception ex) 
            {
                Log.Error(ex.ToString());
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            result = false;
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            GetChart("0",int.Parse(cmbAlarm.SelectedValue.ToString()), stardate, enddate);
            foreach (var item in checkedListBox1.Items)
            {
                chart1.Series[item.ToString()].IsVisibleInLegend = false;
            }
            foreach (var item in checkedListBox1.CheckedItems)
            {
                chart1.Series[item.ToString()].IsVisibleInLegend = true;
                GetChart(item.ToString(),int.Parse(cmbAlarm.SelectedValue.ToString()), stardate, enddate);
            }
            chart1.Visible = true;
            chart2.Visible = false;
            lblAlarmTimes.Visible = false;
            lblAlarmTimeNum.Visible = false;
            lblDate.Visible = false;
            lblDateNum.Visible = false;
            lblDeviceNo.Visible = false;
            lblDeviceNoNum.Visible = false;
        }

        private void Rfrm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 3 || chart1.ChartAreas[0].AxisX.ScaleView.Size == 2)
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 2;
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
                    chart1.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.First);
                }

            }
            else
            {
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 2)
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
                }
                else
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize;
            }
        }

        private void btnPie_Click(object sender, EventArgs e)
        {
            result = true;
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            GetPieChart("0",int.Parse(cmbAlarm.SelectedValue.ToString()), stardate, enddate);
            chart1.Visible = false;
            chart2.Visible = true;
            lblDateNum.Text = stardate + "~" + enddate;
            lblAlarmTimes.Visible = true;
            lblAlarmTimeNum.Visible = true;
            lblDate.Visible = true;
            lblDateNum.Visible = true;
            lblDeviceNo.Visible = false;
            lblDeviceNoNum.Visible = false;
        }
        private void GetPieChart(string Device, int Alarm, string stardate, string enddate)
        {
            XDevice.Clear();
            YTime.Clear();
            DataParameter[] param;
            if (Alarm == 0)
            {
                param = new DataParameter[] { new DataParameter("{0}", string.Format("D.DeviceType='{3}' and C.WarehouseCode='{2}' and CONVERT(varchar(12) , R.EndDate, 111 ) between '{0}' and '{1}'", stardate, enddate, Program.WarehouseCode, DeviceType)) };
            }
            else
            {
                param = new DataParameter[] { new DataParameter("{0}", string.Format("D.DeviceType='{4}' and R.AlarmCode={3} and C.WarehouseCode='{2}'  and  CONVERT(varchar(12) , R.EndDate, 111 ) between '{0}' and '{1}'", stardate, enddate, Program.WarehouseCode, Alarm, DeviceType)) };
            }
            DataTable dt = bll.FillDataTable("WCS.SelectAlarmRecord", param);
            if (Device == "0")
            {
                DataTable dtDevice;
                dtDevice = bll.FillDataTable("Cmd.SelectAisleDeviceChart", new DataParameter("{0}", string.Format("WareHouseCode='{0}'  and Priority=1", Program.WarehouseCode)));
                for (int i = 0; i < dtDevice.Rows.Count; i++)
                {
                    int ycount = dt.Select(string.Format("DeviceNo='{0}'", dtDevice.Rows[i]["DeviceNo2"].ToString())).Count();
                    if (ycount != 0)
                    {
                        XDevice.Add(dtDevice.Rows[i]["DeviceNo2"].ToString());

                        YTime.Add(ycount);
                    }
                   
                }
                chart2.Series["故障频次(圆)"].Points.DataBindXY(XDevice, YTime);
            }
            lblAlarmTimeNum.Text = dt.Rows.Count.ToString();
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        { 
            HitTestResult Result = new HitTestResult();
            Result = chart2.HitTest(e.X, e.Y);
            XDevice.Clear();
            XAlarm.Clear();
            YTime.Clear();
            if (result)
            {
                if (Result.ChartElementType == ChartElementType.DataPoint)
                {
                    int pointIndex = Result.PointIndex;
                    string xName = chart2.Series[0].Points[pointIndex].AxisLabel;
                    DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("R.DeviceNo='{4}' and  D.DeviceType='{3}' and C.WarehouseCode='{2}' and CONVERT(varchar(12) , R.EndDate, 111 ) between '{0}' and '{1}'", stardate, enddate, Program.WarehouseCode, DeviceType,xName)) };
                    DataTable dt = bll.FillDataTable("WCS.SelectAlarmRecord", param);
                    DataView dtView = dt.DefaultView;
                    DataTable dtnew = dtView.ToTable(true, "AlarmCode", "AlarmDesc");
                    foreach (DataRow item in dtnew.Rows)
                    {
                        DataRow[] drs = dt.Select(string.Format("AlarmCode='{0}'", item[0]));
                        XAlarm.Add(item[0].ToString() + "   " + item[1].ToString());
                        YTime.Add(drs.Length);
                    }
                    lblDeviceNoNum.Text = xName;
                    lblAlarmTimeNum.Text = dt.Rows.Count.ToString();
                    lblDeviceNo.Visible = true;
                    lblDeviceNoNum.Visible = true;
                    chart2.Series[0].Points.Clear();
                    chart2.Series[1].Points.DataBindXY(XAlarm, YTime);
                    result = false;
                }
            }
        }
    }
}
