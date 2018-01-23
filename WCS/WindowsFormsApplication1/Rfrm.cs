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
        int tasktype = 0;
        string stardate;
        string enddate;
        List<string> txData2 = new List<string>();
        List<int> txHour2 = new List<int>();
        List<int> tyData2 = new List<int>();
        private void Rfrm_Load(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(param);
        }


        private void GetChart(DataParameter[] param)
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
            GetChart(param);
            tasktype = 1;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE='12' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(param);
            tasktype = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(param);
            tasktype = 0;
        }

        private void chart1_GetToolTipText(object sender,ToolTipEventArgs e)
        {
            //if (e.HitTestResult.ChartElementType==ChartElementType.DataPoint)
            //{
            //    int t = e.HitTestResult.PointIndex;
            //    DataPoint dp = e.HitTestResult.Series.Points[t];
            //    double x = dp.XValue;
            //}
        
        }


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
        private void GetDetailChart(DataParameter[] param)
        {
            try
            {

                txData2.Clear();
                tyData2.Clear();
                txHour2.Clear();
                DataTable dt = bll.FillDataTable("WCS.SelectTaskFinish", param);
                for (int i = 0; i < 24; i++)
                {
                    txHour2.Add(i + 1);
                    int a = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select 1).Count();
                    tyData2.Add(a);
                }
                foreach (var series in chart1.Series)
                {
                    series.Points.DataBindXY(txHour2, tyData2);
                    series.Points.InsertXY(0, 0, 0);

                }
                chart1.Series[0].Points.InsertXY(24, 24.99, 0);
                result = false;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
