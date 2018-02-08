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
    public partial class FrmEff : Form
    {
        public FrmEff()
        {
            InitializeComponent();
        }
        bool result = true;
        BLL.BLLBase bll = new BLL.BLLBase();
        string stardate;
        string enddate;
        List<string> Xdate = new List<string>();
        List<int> YMin = new List<int>();
        private void FrmEff_Load(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            GetEff();
        }

        private void GetEff() 
        {
            DataParameter[] param;
            param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            DataTable dt = bll.FillDataTable("WCS.SelectTaskEff", param);
            TimeSpan t1 = dateTimePicker2.Value - dateTimePicker1.Value;
            int days = t1.Days;
            for (int i = 0; i < days + 1; i++)
            {
                string ymd = dateTimePicker1.Value.AddDays(i).ToString("yyyy-MM-dd");
                string d = ymd.Substring(5);
                Xdate.Add(d);
                int per;
                var a = (from DataRow Order in dt.Rows where ((DateTime)Order["FinishDate"]).ToString("yyyy-MM-dd") == ymd select new { finishdate = Order[0],tasktype = Order[1],Time=Order[2] });
                int c = a.Sum(x => int.Parse(x.Time.ToString()));
                if (a.Count()>0)
                {
                    per = c / a.Count();
                }
                else
                {
                    per = 0;
                }
                YMin.Add(per);
                
            }
            chart1.Series[0].Points.DataBindXY(Xdate, YMin);
            result = true;
        }

        private void GetDetailEff(DataParameter[] param) 
        {
            try{
            Xdate.Clear();
            YMin.Clear();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.ChartAreas[0].Axes[0].LabelStyle.Format = "#时";
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            DataTable dt = bll.FillDataTable("WCS.SelectTaskEff", param);
            for (int i = 0; i < 24; i++)
            {

                var c = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select Order);
                int a = c.Count();

                chart1.Series[0].Points.InsertXY(i, i + 1, a);


            }

            chart1.Series[0].Points.InsertXY(0, 0, 0);

            result = false;
            }
            catch (Exception ex)
            {

                throw;
            }
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
                    lbldate.Text = aDay;
                    lbldate.Visible = true;
                    DataParameter[] param;
                    //if (tasktype == 1)
                    //{
                    //    param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=11 and CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    //}i
                    //else if (tasktype == 2)
                    //{
                    //    param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE=12 and CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    //}
                    //else
                    //{
                        param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 )='{0}'", aDay)) };
                    //}
                    GetDetailEff(param);
                }
            }
        }

    }
}
