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
    public partial class Rfrm : Form
    {
        public Rfrm()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        bool result = true;
        string stardate;
        string enddate;
        int tasktype;
        List<string> txData2 = new List<string>();
        List<int> txHour2 = new List<int>();
        List<int> tyData2 = new List<int>();

        private void Rfrm_Load(object sender, EventArgs e)
        {

            DataTable dt = bll.FillDataTable("CMD.SelectAisle",new DataParameter("{0}",string.Format("WareHouseCode='{0}'","S")));
            int AisleNoCount = dt.Rows.Count;
            DataTable dtDevice;

            for (int i = 1; i < AisleNoCount + 1; i++)
            {
                dtDevice = bll.FillDataTable("Cmd.SelectAisleDeviceChart", new DataParameter("{0}", string.Format("WareHouseCode='{0}' and AisleNo='{1}'", "S", "0" + i.ToString())));
                for (int j = 1; j < dtDevice.Rows.Count + 1; j++)
                {
                    chart1.Series.Add(new Series(dtDevice.Rows[j-1]["DeviceNo2"].ToString()));
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].Label = "#VAL";
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].IsVisibleInLegend = false;
                    chart1.Series[dtDevice.Rows[j - 1]["DeviceNo2"].ToString()].ChartType = SeriesChartType.Column;
                }
                chart1.Series.Add(new Series(i.ToString() + "号巷道"));
                chart1.Series[i.ToString() + "号巷道"].Label = "#VAL";
                chart1.Series[i.ToString() + "号巷道"].IsVisibleInLegend = false;
                chart1.Series[i.ToString() + "号巷道"].ChartType = SeriesChartType.Column;
                chart1.Series[i.ToString() + "号巷道"].CustomProperties = "DrawingStyle=Wedge";
            }
            chart1.Series.Add(new Series("任务数"));
            chart1.Series["任务数"].Label = "#VAL";
            chart1.Series["任务数"].ChartType = SeriesChartType.Column;
            chart1.Series["任务数"].CustomProperties = "DrawingStyle=Cylinder";

            dateTimePicker1.Value=dateTimePicker2.Value.Add(new TimeSpan(-31,0,0,0));
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetChart(0,stardate,enddate);
            this.MouseWheel +=new MouseEventHandler(Rfrm_MouseWheel);
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
        }
        //获取日期每个巷道任务数
        private void GetAisleChart(int Aisle,int TaskType)
        {
            try
            {

                txData2.Clear();
                tyData2.Clear();
                txHour2.Clear();
                //foreach (var series in chart1.Series)
                //{
                //    series.Points.Clear();
                //}
                DataParameter[] param;
                if (TaskType==1)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='12' and AisleNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + Aisle.ToString())) };
                }
                else if (TaskType==2)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='12' and AisleNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + Aisle.ToString())) };
                }
                else
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("AisleNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + Aisle.ToString())) };
                }
                DataTable dt = bll.FillDataTable("WCS.SelectTaskFinish", param);
                TimeSpan t1 = dateTimePicker2.Value - dateTimePicker1.Value;
                int days = t1.Days;
                for (int i = 0; i < days + 1; i++)
                {
                    string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string d = ymd.Substring(5);
                    txData2.Add(d);

                    int a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select 1).Count();
                    tyData2.Add(a);
                }
                chart1.Series[(Aisle - 1) * 3 + 2].Points.DataBindXY(txData2, tyData2);
                result = true;
                //for (int i = 0; i < 24; i++)
                //{
                //    txData2.Add(i + 1);
                //    int a = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select 1).Count();
                //    tyData2.Add(a);
                //}
                //foreach (var series in chart1.Series)
                //{
                //    series.Points.DataBindXY(txData2, tyData2);
                //    series.Points.InsertXY(0, 0, 0);

                //}
                //chart1.Series[0].Points.InsertXY(24, 24.99, 0);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //获取日期每辆小车任务数
        private void GetCarChart(int CarAisleNo, int TaskType)
        {

            for (int j = 1; j < 3; j++)
            {
                txData2.Clear();
                tyData2.Clear();
                txHour2.Clear();

                DataParameter[] param;
                if (TaskType == 1)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='12' and AisleNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + CarAisleNo.ToString())) };
                }
                else if (TaskType == 2)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("TaskType='12' and AisleNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + CarAisleNo.ToString())) };
                }
                else
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("DeviceNo='{2}' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate, "0" + CarAisleNo.ToString() + "0" + j.ToString())) };
                }
                DataTable dt = bll.FillDataTable("WCS.SelectTaskFinish", param);
                TimeSpan t1 = dateTimePicker2.Value - dateTimePicker1.Value;
                int days = t1.Days;
                for (int i = 0; i < days + 1; i++)
                {
                    string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string d = ymd.Substring(5);
                    txData2.Add(d);

                    int a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select 1).Count();
                    tyData2.Add(a);
                }
                chart1.Series[(CarAisleNo - 1) * 3 + j - 1].Points.DataBindXY(txData2, tyData2);
            }
            result = true;
        }
        //获取日期任务数
        private void GetChart(int TaskType,string stardate,string enddate)
        {
            try
            {

                txData2.Clear();
                tyData2.Clear();
                txHour2.Clear();
                chart1.ChartAreas[0].AxisX.IsMarginVisible = true;
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                DataParameter[] param;
                if (TaskType == 1)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=11 and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
                }
                else if (TaskType == 2)
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE='12' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
                }
                else
                {
                    param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
                }
                DataTable dt = bll.FillDataTable("WCS.SelectTaskFinish", param);
                TimeSpan t1= dateTimePicker2.Value -dateTimePicker1.Value;
                int days = t1.Days;
                for (int i = 0; i < days + 1; i++)
                {
                    string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string d = ymd.Substring(5);
                    txData2.Add(d);

                    int a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select 1).Count();
                    tyData2.Add(a);
                }
                chart1.Series[21].Points.DataBindXY(txData2, tyData2);
                result = true;
                //for (int i = 0; i < 24; i++)
                //{
                //    txData2.Add(i + 1);
                //    int a = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select 1).Count();
                //    tyData2.Add(a);
                //}
                //foreach (var series in chart1.Series)
                //{
                //    series.Points.DataBindXY(txData2, tyData2);
                //    series.Points.InsertXY(0, 0, 0);
                   
                //}
                //chart1.Series[0].Points.InsertXY(24, 24.99, 0);

            }
            catch (Exception ex)
            {

                throw;
            }
        }




        //private void btnIn_Click(object sender, EventArgs e)
        //{
            
        //    GetChart(1,stardate,enddate);
        //    tasktype = 1;
        //    for (int i = 1; i < 8; i++)
        //    {
        //        chart1.Series[(i - 1) * 3 + 2].IsVisibleInLegend = false;
        //        chart1.Series[(i - 1) * 3].IsVisibleInLegend = false;
        //        chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = false;
        //    }
        //    if (checkBox1.Checked)
        //    {
        //        for (int i = 1; i < 8; i++)
        //        {
        //            chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = true;
        //            GetAisleChart(i, 1);
        //        }
        //    }
        //    if (checkBox2.Checked)
        //    {
        //        for (int i = 1; i < 8; i++)
        //        {
        //            chart1.Series[(i - 1) * 3].IsVisibleInLegend = true;
        //            chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = true;
        //            GetCarChart(i, 1);
        //        }
        //    }
        //}

        //private void btnOut_Click(object sender, EventArgs e)
        //{
        //    GetChart(2,stardate,enddate);
        //    tasktype = 2;
        //    for (int i = 1; i < 8; i++)
        //    {
        //        chart1.Series[(i - 1) * 3 + 2].IsVisibleInLegend = false;
        //        chart1.Series[(i - 1) * 3].IsVisibleInLegend = false;
        //        chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = false;
        //    }
        //    if (checkBox1.Checked)
        //    {
        //        for (int i = 1; i < 8; i++)
        //        {
        //            chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = true;
        //            GetAisleChart(i, 2);
        //        }
        //    }
        //    if (checkBox2.Checked)
        //    {
        //        for (int i = 1; i < 8; i++)
        //        {
        //            chart1.Series[(i - 1) * 3].IsVisibleInLegend = true;
        //            chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = true;
        //            GetCarChart(i, 2);
        //        }
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            lbldate.Visible = false;
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            TimeSpan t1 = dateTimePicker2.Value - dateTimePicker1.Value;
            int days = t1.Days;
            if (days > 31)
            {
                MessageBox.Show("请选择一个月以内的日期范围");
                return;
            }
            if (checkInstock.Checked && checkOutStock.Checked)
            {
                GetChart(0, stardate, enddate);
                tasktype = 0;
            }
            else if (checkInstock.Checked)
            {
                GetChart(1, stardate, enddate);
                tasktype = 1;
            }
            else if (checkOutStock.Checked)
            {
                GetChart(2, stardate, enddate);
                tasktype = 2;
            }
            else
            {
                MessageBox.Show("请至少在出入库中勾选一项");
            }
            for (int i = 1; i < 8; i++)
            {
                chart1.Series[(i - 1) * 3 + 2].IsVisibleInLegend = false;
                chart1.Series[(i - 1) * 3].IsVisibleInLegend = false;
                chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = false;
            }
            if (checkBox1.Checked)
            {
                for (int i = 1; i < 8; i++)
                {
                    chart1.Series[(i - 1) * 3 + 2].IsVisibleInLegend = true;
                    GetAisleChart(i, 0);
                }
            }
            if (checkBox2.Checked)
            {
                for (int i = 1; i < 8; i++)
                {
                    chart1.Series[(i - 1) * 3].IsVisibleInLegend = true;
                    chart1.Series[(i - 1) * 3 + 1].IsVisibleInLegend = true;
                    GetCarChart(i, 0);
                }
            }
        }



        //点击日期任务数进入获取当日24小时任务数
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult Result = new HitTestResult();
            Result = chart1.HitTest(e.X, e.Y);

            if (result)
            {
                if (Result.ChartElementType == ChartElementType.DataPoint)
                {
                    int t = Result.PointIndex;
                    string aDay = dateTimePicker1.Value.AddDays(t).ToString("yyyy/MM/dd");
                    lbldate.Text = aDay;
                    lbldate.Visible = true;
                    DataParameter[] param;
                    if (tasktype == 1)
                    {
                        param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=11 and CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    }
                    else if (tasktype==2)
                    {
                        param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=12 and CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    }
                    else
                    {
                        param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    }
                    GetDetailChart(param);
                }
            }
        }
        //获取24小时每小时任务数
        private void GetDetailChart(DataParameter[] param)
        {
            try
            {

                txData2.Clear();
                tyData2.Clear();
                txHour2.Clear();
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.ChartAreas[0].Axes[0].LabelStyle.Format = "#时";
                chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
                DataTable dt = bll.FillDataTable("WCS.SelectTaskFinish", param);
                for (int i = 0; i < 24; i++)
                {
           
                    var c = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select Order );
                    int a = c.Count();

                    chart1.Series[21].Points.InsertXY(i, i + 1, a);
                    if (checkBox1.Checked)
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            chart1.Series[(j - 1) * 3 + 1].IsVisibleInLegend = true;

                            int e = c.Count(t => t[0].ToString() == "0" + j.ToString());

                            chart1.Series[(j - 1) * 3 + 2].Points.InsertXY(i, i + 1, e);
                        }
                    }
                    if (checkBox2.Checked)
                    {
                        if (checkBox2.Checked)
                        {
                            for (int j = 1; j < 8; j++)
                            {
                                chart1.Series[(j - 1) * 3].IsVisibleInLegend = true;
                                chart1.Series[(j - 1) * 3 + 1].IsVisibleInLegend = true;
                                for (int m = 1; m < 3; m++)
                                {
                                    int carDevice = c.Count(t => t[2].ToString() == "0" + j.ToString() + "0" + m.ToString());
                                    chart1.Series[(j - 1) * 3 + m - 1].Points.InsertXY(i, i + 1, carDevice);
                                }
                                
                            }
                        }
                    }
                 
                }

                chart1.Series[21].Points.InsertXY(0, 0, 0);
                chart1.Series[21].Points.InsertXY(24, 24.99, 0);
                result = false;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        //放大缩小
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 3)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.ChartAreas[0].AxisX.Maximum;
            }
            else
                chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
        }

        private void Rfrm_MouseWheel(object sender, MouseEventArgs e) 
        {
            if (e.Delta>0)
            {
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 3 || chart1.ChartAreas[0].AxisX.ScaleView.Size == 1)
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 1;
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
                }  
                
            }
            else
            {
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 1)
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 3;
                }
                else
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.ChartAreas[0].AxisX.Maximum;
            }
        }

    }
}
