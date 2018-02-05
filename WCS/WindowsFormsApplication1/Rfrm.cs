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
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetChart(0,stardate,enddate);
            for (int i = 1; i < 8; i++)
            {
                GetAisleChart(i, 0);
            }
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
                if (days > 31)
                {
                    return;
                }
                for (int i = 0; i < days + 1; i++)
                {
                    string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string d = ymd.Substring(5);
                    txData2.Add(d);

                    int a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select 1).Count();
                    tyData2.Add(a);
                }
                chart1.Series[Aisle].Points.DataBindXY(txData2, tyData2);
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
                if (days>31)
                {
                    return;
                }
                for (int i = 0; i < days + 1; i++)
                {
                    string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string d = ymd.Substring(5);
                    txData2.Add(d);

                    int a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select 1).Count();
                    tyData2.Add(a);
                }
                chart1.Series[0].Points.DataBindXY(txData2, tyData2);
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




        private void btnIn_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=11 and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(1,stardate,enddate);
            tasktype = 1;
            for (int i = 1; i < 8; i++)
            {
                GetAisleChart(i, 1);
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetChart(2,stardate,enddate);
            tasktype = 2;
            for (int i = 1; i < 8; i++)
            {
                GetAisleChart(i, 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetChart(0,stardate,enddate);
            //DataParameter[] param1 = new DataParameter[] { new DataParameter("{0}", string.Format("AisleNo='01' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            tasktype = 0;
            for (int i = 1; i < 8; i++)
            {
                GetAisleChart(i,0);
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
           
                    var c = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select Order[0] );
                    int a = c.Count();
       
                    chart1.Series[0].Points.InsertXY(i,i+1, a);
                    for (int j = 1; j < 8; j++)
                    {
                      
                        int e = c.Count(t => t.ToString() == "0" + j.ToString());

                        chart1.Series[j].Points.InsertXY(i, i + 1, e);
                    }
                }
        
                chart1.Series[0].Points.InsertXY(0, 0, 0);
     
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
            if (chart1.ChartAreas[0].AxisX.ScaleView.Size == 5)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Size = chart1.ChartAreas[0].AxisX.Maximum;
            }
            else
                chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
        }

    }
}
