using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using DataGridViewAutoFilter;
using MCP;
using OPC;
using MCP.Service.Siemens.Config;
namespace App.View
{
    public partial class frmMonitorB : BaseForm
    {
        private Point InitialP1;
        private Point InitialP2;
        private Point InitialP3;
        private Point InitialP4;


        float colDis = 20.75f;
        float rowDis = 54.4f;

        // private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();
        private System.Timers.Timer tmCrane1 = new System.Timers.Timer();
        BLL.BLLBase bll = new BLL.BLLBase();
        Dictionary<int, string> dicCraneFork = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneStatus = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneAction = new Dictionary<int, string>();

        DataTable dtDeviceAlarm;

        public frmMonitorB()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //Point P2 = picCrane2.Location;
            //P2.X = P2.X - 90;

            //this.picCrane2.Location = P2;
        }

        private void frmMonitorB_Load(object sender, EventArgs e)
        {

            Cranes.OnCrane += new CraneEventHandler(Monitor_OnCrane);
            
            //picCrane1.Parent = pictureBox1;
            //InitialP1 = picCrane1.Location;
            
            //picCrane1.BackColor = Color.Transparent;

            //InitialP2 = picCar01.Location;
            //picCar01.Parent = pictureBox1;
            //picCar01.BackColor = Color.Transparent;

            //InitialP4 = picCrane2.Location;
            //picCrane2.Parent = pictureBox1;
            //picCrane2.BackColor = Color.Transparent;
            AddDicKeyValue();
            try
            {
                ServerInfo[] Servers = new MonitorConfig("MonitorB.xml").Servers;
                for (int i = 0; i < Servers.Length; i++)
                {
                    OPCServer opcServer = new OPCServer(Servers[i].Name);
                    opcServer.Connect(Servers[i].ProgID, Servers[i].ServerName);// opcServer.Connect(config.ConnectionString);

                    OPCGroup group = opcServer.AddGroup(Servers[i].GroupName, Servers[i].UpdateRate);
                    foreach (ItemInfo item in Servers[i].Items)
                    {
                        group.AddItem(item.ItemName, item.OpcItemName, item.ClientHandler, item.IsActive);
                    }
                    if (Servers[i].Name.IndexOf("CraneServer")>=0)
                    {
                        opcServer.Groups.DefaultGroup.OnDataChanged += new OPCGroup.DataChangedEventHandler(Crane_OnDataChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            //tmCrane1.Interval = 3000;
            //tmCrane1.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker1);
            //tmCrane1.Start();
        }
        //反馈给总控WCS设备状态
        //string m = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
        #region 堆垛机监控
        void Crane_OnDataChanged(object sender, DataChangedEventArgs e)
        {
            
            if (e.State == null)
                return;
            string CraneNo = e.ServerName.Substring(11, 2);
            GetCrane(CraneNo);
            if (e.ItemName.IndexOf("Status") >= 0)
                dicCrane[CraneNo].Status = int.Parse(e.State.ToString());
            else if (e.ItemName.IndexOf("Mode") >= 0)
            {
                dicCrane[CraneNo].Mode = int.Parse(e.States[0].ToString());
                dicCrane[CraneNo].AlarmCode = int.Parse(e.States[1].ToString());
                //dicCrane[CraneNo].fu = int.Parse(e.States[1].ToString());
                dicCrane[CraneNo].Column = int.Parse(e.States[2].ToString());
                dicCrane[CraneNo].Layer = int.Parse(e.States[3].ToString());
                dicCrane[CraneNo].ForkStatus = int.Parse(e.States[4].ToString());
            }
            else if (e.ItemName.IndexOf("TaskNo") >= 0)
                dicCrane[CraneNo].TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(e.States));
            Cranes.CraneInfo(dicCrane[CraneNo]);
        }

        void Monitor_OnCrane(CraneEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CraneEventHandler(Monitor_OnCrane), args);
            }
            else
            {
                Crane crane = args.crane;
                TextBox txt = GetTextBox("txtTaskNo", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.TaskNo;

                //txt = GetTextBox("txtState", crane.CraneNo);
                //if (txt != null && dicCraneStatus.ContainsKey(crane.TaskType))
                //    txt.Text = dicCraneStatus[crane.TaskType];

                //txt = GetTextBox("txtCraneAction", crane.CraneNo);
                //if (txt != null && dicCraneAction.ContainsKey(crane.Action))
                //    txt.Text = dicCraneAction[crane.Action];

                txt = GetTextBox("txtRow", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Row.ToString();

                txt = GetTextBox("txtColumn", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Column.ToString();

                //堆垛机位置
                if (crane.CraneNo == "05")
                {
                    //this.picCrane2.Visible = true;
                    //Point P1 = InitialP1;
                    //if (crane.Column < 46)
                    //    P1.X = P1.X + (int)((crane.Column - 1) * colDis);
                    //else
                    //    P1.X = picCrane1.Location.X + 15;

                    //P1.Y = P1.Y + (int)(rowDis * (crane.Row - 1));
                    //this.picCrane2.Location = P1;

                    //Point P2 = InitialP2;
                    //P2.Y = P2.Y + (int)(rowDis * (crane.Row - 1));
                    //this.picCrane1.Location = P2;
                }

                txt = GetTextBox("txtHeight", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Layer.ToString();

                txt = GetTextBox("txtForkStatus", crane.CraneNo);
                if (txt != null && dicCraneFork.ContainsKey(crane.ForkStatus))
                    txt.Text = dicCraneFork[crane.ForkStatus];
                txt = GetTextBox("txtErrorNo", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.AlarmCode.ToString();

                string strAlarmDesc = "";
                txt = GetTextBox("txtAlarmDesc", crane.CraneNo);
                if (txt != null)
                {
                    if (crane.AlarmCode != 0)
                    {
                        DataRow[] drs = dtDeviceAlarm.Select(string.Format("Flag=1 and AlarmCode={0}", crane.AlarmCode));
                        if (drs.Length > 0)
                            strAlarmDesc = drs[0]["AlarmDesc"].ToString();
                        else
                            strAlarmDesc = "设备未知错误！";
                    }
                    else
                    {
                        strAlarmDesc = "";
                    }
                    txt.Text = strAlarmDesc;
                }

                //更新错误代码、错误描述
                //更新任务状态为执行中
                //bll.ExecNonQuery("WCS.UpdateTaskError", new DataParameter[] { new DataParameter("@CraneErrCode", crane.ErrCode.ToString()), new DataParameter("@CraneErrDesc", dicCraneError[crane.ErrCode]), new DataParameter("@TaskNo", crane.TaskNo) });
                if (crane.AlarmCode > 0)
                {
                    DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", crane.TaskNo), new DataParameter("@CraneErrCode", crane.AlarmCode.ToString()), new DataParameter("@CraneErrDesc", strAlarmDesc) };
                    bll.ExecNonQueryTran("WCS.Sp_UpdateTaskError", param);
                    Logger.Error(crane.CraneNo.ToString() + "堆垛机执行时出现错误,代码:" + crane.AlarmCode.ToString() + ",描述:" + strAlarmDesc);
                }
            }
        }
        private Dictionary<string, Crane> dicCrane = new Dictionary<string, Crane>();
        private Crane GetCrane(string craneno)
        {
            Crane crane = null;
            if (dicCrane.ContainsKey(craneno))
            {
                crane = dicCrane[craneno];
            }
            else
            {
                crane = new Crane();
                crane.CraneNo = craneno;
                dicCrane.Add(craneno, crane);
            }
            return crane;
        }
        #endregion

        private void AddDicKeyValue()
        {
            dicCraneFork.Add(0, "非原点");
            dicCraneFork.Add(1, "原点");

            dicCraneStatus.Add(0, "空闲");
            dicCraneStatus.Add(1, "等待");
            dicCraneStatus.Add(2, "定位");
            dicCraneStatus.Add(3, "取货");
            dicCraneStatus.Add(4, "放货");
            dicCraneStatus.Add(98, "维修");

            dicCraneAction.Add(0, "非自动");
            dicCraneAction.Add(1, "自动");            
            
            dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "1=1") });
        }

        
        Button GetButton( string CraneNo)
        {
            Control[] ctl = this.Controls.Find("btn" + CraneNo, true);
            if (ctl.Length > 0)
                return (Button)ctl[0];
            else
                return null;
        }

        TextBox GetTextBox(string name, string CraneNo)
        {
            Control[] ctl = this.Controls.Find(name + int.Parse(CraneNo), true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.btnBack5.Text == "启动")
            {
                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 1);
                this.btnBack5.Text = "停止";
            }
            else
            {
                Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 0);
                this.btnBack5.Text = "启动";
            }
        }

        private void btnBack1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要召回1号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                PutCommand("1", 0);
                Logger.Info("1号堆垛机下发召回命令");
            }
        }
        private void PutCommand(string craneNo, byte TaskType)
        {
            string serviceName = "CranePLC" + craneNo;
            int[] cellAddr = new int[9];
            cellAddr[TaskType] = 1;

            //cellAddr[3] = int.Parse(this.cbFromColumn.Text);
            //cellAddr[4] = int.Parse(this.cbFromHeight.Text);
            //cellAddr[5] = int.Parse(this.cbFromRow.Text.Substring(3, 3));
            //cellAddr[6] = int.Parse(this.cbToColumn.Text);
            //cellAddr[7] = int.Parse(this.cbToHeight.Text);
            //cellAddr[8] = int.Parse(this.cbToRow.Text.Substring(3, 3));

            Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 0);
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要急停1号堆垛机?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                PutCommand("1", 2);
                Logger.Info("1号堆垛机下发急停命令");
            }
        }

        private void btnConveyor_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string Number = btn.Name.Substring(btn.Name.Length - 2, 2);
                string Barcode = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService("TranLine", "ConveyorInfo" + Number)));
                this.toolTip1.SetToolTip(btn, Barcode);
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }        
    }
}