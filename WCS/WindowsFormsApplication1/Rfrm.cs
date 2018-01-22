using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace WindowsFormsApplication1
{
    public partial class Rfrm : Form
    {
        public Rfrm()
        {
            InitializeComponent();
        }
        BLL.BLLBase bll = new BLL.BLLBase();
        string stardate;
        string enddate;
        List<int> txData2 = new List<int>();
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
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                DataTable dt = bll.FillDataTable("WCS.SelectTask", param);
                for (int i = 0; i < 24; i++)
                {
                    txData2.Add(i + 1);
                    int a = (from DataRow Order in dt.Rows where (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) >= i && (int.Parse(((DateTime)Order["FinishDate"]).ToString("HH"))) < (i + 1) select 1).Count();
                    tyData2.Add(a);
                }
                foreach (var series in chart1.Series)
                {
                    series.Points.DataBindXY(txData2, tyData2);
                    series.Points.InsertXY(0, 0, 0);
                   
                }
                chart1.Series[0].Points.InsertXY(24, 24.99, 0);

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
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.TASKTYPE='12' and CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(param);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stardate = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            enddate = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            DataParameter[] param = new DataParameter[] { new DataParameter("{0}", string.Format("CONVERT(varchar(12) , FinishDate, 111 ) between '{0}' and '{1}'", stardate, enddate)) };
            GetChart(param);
        }
    }
}
