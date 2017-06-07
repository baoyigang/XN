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
        private Point InitialP5;
        private Point InitialP6;
        private Point InitialP7;


        float colDis = 21f;
        float rowDis = 54.4f;

        private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();

        BLL.BLLBase bll = new BLL.BLLBase();
        Dictionary<int, string> dicFork = new Dictionary<int, string>();
        Dictionary<int, string> dicStatus = new Dictionary<int, string>();
        Dictionary<int, string> dicWorkMode = new Dictionary<int, string>();

        DataTable dtDeviceAlarm;

        public frmMonitorB()
        {
            InitializeComponent();
        }

        private void frmMonitorB_Load(object sender, EventArgs e)
        {

            Cranes.OnCrane += new CraneEventHandler(Monitor_OnCrane);
            
            InitialP5 = btnSRM5.Location;
            InitialP6 = btnSRM6.Location;
            InitialP7 = btnSRM7.Location;
 
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
                    if (Servers[i].Name.IndexOf("PLC") >= 0)
                    {
                        opcServer.Groups.DefaultGroup.OnDataChanged += new OPCGroup.DataChangedEventHandler(Crane_OnDataChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            tmWorkTimer.Interval = 30000;
            tmWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmWorker);
            tmWorkTimer.Start();
        }
        private void tmWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();

                foreach(KeyValuePair<string,Crane> kvp in dicCrane)
                {
                    //反馈给总控WCS设备状态
                    string id = Guid.NewGuid().ToString();
                    string deviceNo = "01" + kvp.Value.CraneNo;
                    string mode = kvp.Value.Mode.ToString();
                    string status = kvp.Value.Status.ToString();
                    string taskNo = kvp.Value.TaskNo;
                    string fork = kvp.Value.ForkStatus.ToString();
                    string load = kvp.Value.Load.ToString();
                    string aisleNo = kvp.Value.Row.ToString();
                    string column = kvp.Value.Column.ToString();
                    string layer = kvp.Value.Layer.ToString();
                    string alarmCode = kvp.Value.AlarmCode.ToString();
                    string sender1 = "admin";

                    string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
                    Logger.Info("上报设备状态");
                    string message = Program.send("transWCSDevice", Json);
                    App.Dispatching.Process.RtnMessage rtnMessage = JsonHelper.JSONToObject<App.Dispatching.Process.RtnMessage>(message);
                    Logger.Info("上报设备状态,收到反馈：" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("frmMonitorB在上报设备状态时发生错误：" + ex.Message);
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        #region 堆垛机监控
        void Crane_OnDataChanged(object sender, DataChangedEventArgs e)
        {
            
            if (e.State == null)
                return;
            string CraneNo = e.ServerName.Substring(5, 2);
            GetCrane(CraneNo);
            if (e.ItemName.IndexOf("Status") >= 0)
            {
                dicCrane[CraneNo].Status = int.Parse(e.States[4].ToString());
                dicCrane[CraneNo].Row = int.Parse(e.States[5].ToString());
            }
            else if (e.ItemName.IndexOf("WorkMode") >= 0)
            {
                dicCrane[CraneNo].Mode = int.Parse(e.States[1].ToString());
                dicCrane[CraneNo].AlarmCode = int.Parse(e.States[2].ToString());
                dicCrane[CraneNo].Load = int.Parse(e.States[3].ToString());
                dicCrane[CraneNo].Column = int.Parse(e.States[4].ToString());
                dicCrane[CraneNo].Layer = int.Parse(e.States[5].ToString());
                dicCrane[CraneNo].ForkStatus = int.Parse(e.States[6].ToString());
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

                txt = GetTextBox("txtStatus", crane.CraneNo);
                if (txt != null && dicStatus.ContainsKey(crane.Status))
                    txt.Text = dicStatus[crane.Status];

                txt = GetTextBox("txtWorkMode", crane.CraneNo);
                if (txt != null && dicWorkMode.ContainsKey(crane.Mode))
                    txt.Text = dicWorkMode[crane.Mode];

                txt = GetTextBox("txtRow", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Row.ToString();

                txt = GetTextBox("txtColumn", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Column.ToString();

                //堆垛机位置
                if (crane.CraneNo == "05")
                {
                    Point P = InitialP5;
                    if (crane.Row == 6)
                        P = InitialP6;
                    else if (crane.Row == 7)
                        P = InitialP7;

                    P.X = P.X + (int)((45 - crane.Column) * colDis);
                    this.btnSRM5.Location = P;
                }
                if (crane.CraneNo == "06")
                {
                    Point P = InitialP6;
                    if (crane.Row == 5)
                        P = InitialP5;
                    else if (crane.Row == 7)
                        P = InitialP7;

                    P.X = P.X + (int)((45 - crane.Column) * colDis);
                    this.btnSRM6.Location = P;
                }
                if (crane.CraneNo == "07")
                {
                    Point P = InitialP7;
                    if (crane.Row == 5)
                        P = InitialP5;
                    else if (crane.Row == 6)
                        P = InitialP6;

                    P.X = P.X + (int)((45-crane.Column) * colDis);
                    this.btnSRM7.Location = P;                    
                }

                txt = GetTextBox("txtLayer", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Layer.ToString();

                txt = GetTextBox("txtForkStatus", crane.CraneNo);
                if (txt != null && dicFork.ContainsKey(crane.ForkStatus))
                    txt.Text = dicFork[crane.ForkStatus];
                txt = GetTextBox("txtAlarmCode", crane.CraneNo);
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
                    //DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", crane.TaskNo), new DataParameter("@CraneErrCode", crane.AlarmCode.ToString()), new DataParameter("@CraneErrDesc", strAlarmDesc) };
                    //bll.ExecNonQueryTran("WCS.Sp_UpdateTaskError", param);
                    //Logger.Error(crane.CraneNo.ToString() + "堆垛机执行时出现错误,代码:" + crane.AlarmCode.ToString() + ",描述:" + strAlarmDesc);
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
            dicFork.Add(0, "货叉在原位");
            dicFork.Add(1, "货叉在左侧");
            dicFork.Add(2, "货叉在右侧");

            dicStatus.Add(0, "未知");
            dicStatus.Add(1, "空闲");
            //dicStatus.Add(2, "检查任务数据");
            //dicStatus.Add(3, "定位到取货位");
            //dicStatus.Add(4, "取货中");
            //dicStatus.Add(7, "取货完成");
            //dicStatus.Add(8, "等待调度柜");
            //dicStatus.Add(9, "取货完成");
            //dicStatus.Add(10, "取货完成");
            //dicStatus.Add(13, "取货完成");
            //dicStatus.Add(14, "取货完成");
            //dicStatus.Add(15, "取货完成");
            //dicStatus.Add(20, "检查源位置");

            dicWorkMode.Add(0, "关机");
            dicWorkMode.Add(1, "自动");
            dicWorkMode.Add(2, "手动");
            dicWorkMode.Add(3, "半自动");
            dicWorkMode.Add(4, "维修");
            
            dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "1=1") });
        }

        TextBox GetTextBox(string name, string CraneNo)
        {
            Control[] ctl = this.Controls.Find(name + int.Parse(CraneNo), true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }   
    }
}