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
    public partial class frmMonitor : BaseForm
    {
        private Point InitialP1;
        private Point InitialP2;
        private Point InitialP3;
        private Point InitialP4;
        private Point InitialP5;
        private Point InitialP6;
        private Point InitialP7;

        float colDis = 12.68f;
        float rowDis = 56f;

        private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();
        private System.Timers.Timer tmCar1 = new System.Timers.Timer();
        BLL.BLLBase bll = new BLL.BLLBase();
        Dictionary<int, string> dicFork = new Dictionary<int, string>();
        Dictionary<int, string> dicStatus = new Dictionary<int, string>();
        Dictionary<int, string> dicWorkMode = new Dictionary<int, string>();
        Dictionary<int, int> dicLocationX = new Dictionary<int, int>();

        DataTable dtDeviceAlarm;

        public frmMonitor()
        {
            InitializeComponent();
        }

        private void frmMonitor_Load(object sender, EventArgs e)
        {
            DataTable dt = Program.dtUserPermission;
            //监控任务--取消堆垛机任务
            string filter = "SubModuleCode='MNU_W00A_00A' and OperatorCode='1'";
            DataRow[] drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.btnCar0101.Visible = false;
                this.btnCar0102.Visible = false;
                this.btnCar0201.Visible = false;
                this.btnCar0202.Visible = false;
                this.btnCar0301.Visible = false;
                this.btnCar0302.Visible = false;
                this.btnCar0401.Visible = false;
                this.btnCar0402.Visible = false;
                this.btnCar0501.Visible = false;
                this.btnCar0502.Visible = false;
                this.btnCar0601.Visible = false;
                this.btnCar0602.Visible = false;
                this.btnCar0701.Visible = false;
                this.btnCar0702.Visible = false;
            }
            else
            {
                this.btnCar0101.Visible = true;
                this.btnCar0102.Visible = true;
                this.btnCar0201.Visible = true;
                this.btnCar0202.Visible = true;
                this.btnCar0301.Visible = true;
                this.btnCar0302.Visible = true;
                this.btnCar0401.Visible = true;
                this.btnCar0402.Visible = true;
                this.btnCar0501.Visible = true;
                this.btnCar0502.Visible = true;
                this.btnCar0601.Visible = true;
                this.btnCar0602.Visible = true;
                this.btnCar0701.Visible = true;
                this.btnCar0702.Visible = true;
            }
            Cars.OnCar += new CarEventHandler(Monitor_OnCar);

            InitialP1 = btnCar0101.Location;
            InitialP2 = btnCar0201.Location;
            InitialP3 = btnCar0301.Location;
            InitialP4 = btnCar0401.Location;
            InitialP5 = btnCar0501.Location;
            InitialP6 = btnCar0601.Location;
            InitialP7 = btnCar0701.Location;
            AddDicKeyValue();
            try
            {
                ServerInfo[] Servers = new MonitorConfig("Monitor.xml").Servers;
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
                        opcServer.Groups.DefaultGroup.OnDataChanged += new OPCGroup.DataChangedEventHandler(Car_OnDataChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            tmWorkTimer.Interval = Program.SendInterval;
            tmWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmWorker);
            tmWorkTimer.Start();
        }
        private void tmWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();

                foreach (KeyValuePair<string, Car> kvp in dicCar)
                {
                    //反馈给总控WCS设备状态
                    string id = Guid.NewGuid().ToString();
                    string deviceNo = kvp.Value.CarNo;
                    string mode = kvp.Value.Mode.ToString();
                    string status = kvp.Value.Status.ToString();
                    string taskNo = kvp.Value.TaskNo;
                    string fork = kvp.Value.ForkStatus.ToString();
                    string load = kvp.Value.Load.ToString();
                    string aisleNo = kvp.Value.CarNo.Substring(0,2);
                    string column = kvp.Value.Column.ToString();
                    string layer = kvp.Value.Layer.ToString();
                    string alarmCode = kvp.Value.AlarmCode.ToString();
                    DataRow[] drs = dtDeviceAlarm.Select(string.Format("Flag=2 and AlarmCode={0}", alarmCode));
                    string alarmDesc = "";
                    if (alarmCode != "0")
                    {
                        if (drs.Length > 0)
                            alarmDesc = drs[0]["AlarmDesc"].ToString();
                        else
                            alarmDesc = "穿梭车未知错误！";
                    }
                    string sender1 = "admin";

                    string Json = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + Program.WarehouseCode + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender1 + "\",\"field1\":\"\",\"field2\":\"" + alarmDesc + "\",\"field3\":\"\"" + "}]";
                    Logger.Info("上报设备编号[" + deviceNo + "]的状态");
                    string message = Program.send("transWCSDevice", Json);
                    App.Dispatching.Process.RtnMessage rtnMessage = JsonHelper.JSONToObject<App.Dispatching.Process.RtnMessage>(message);
                    Logger.Info("上报设备编号[" + deviceNo + "]的状态,收到反馈：" + rtnMessage.returnCode + ":" + rtnMessage.message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("frmMonitor在上报设备状态时发生错误：" + ex.Message);
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        //反馈给总控WCS设备状态
        //string m = "[{\"id\":\"" + id + "\",\"deviceNo\":\"" + deviceNo + "\",\"mode\":\"" + mode + "\",\"status\":\"" + status + "\",\"taskNo\":\"" + taskNo + "\",\"fork\":\"" + fork + "\",\"load\":\"" + load + "\",\"aisleNo\":\"" + aisleNo + "\",\"column\":\"" + column + "\",\"layer\":\"" + layer + "\",\"alarmCode\":\"" + alarmCode + "\",\"sendDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\",\"sender\":\"" + sender + "\",\"field1\":\"\",\"field2\":\"\",\"field3\":\"\"" + "}]";
        #region 堆垛机监控
        void Car_OnDataChanged(object sender, DataChangedEventArgs e)
        {
            if (e.State == null)
                return;
            if (e.ItemName == "abc")
                return;

            string AisleNo = e.ServerName.Substring(5, 2);
            string CarNo = "";
            if (e.ItemName.IndexOf("CarStatus") >= 0 || e.ItemName.IndexOf("CarAlarm") >= 0 || e.ItemName.IndexOf("CarTask") >= 0)
                CarNo = e.ItemName.Substring(e.ItemName.Length - 2, 2);

            CarNo = AisleNo + CarNo;
            GetCar(CarNo);

            if (e.ItemName.IndexOf("CarStatus") >= 0)
            {
                dicCar[CarNo].Mode = int.Parse(e.States[0].ToString());
                dicCar[CarNo].Status = int.Parse(e.States[12].ToString());
                dicCar[CarNo].Row = int.Parse(e.States[1].ToString());
                dicCar[CarNo].Column = int.Parse(e.States[2].ToString());
                dicCar[CarNo].Layer = int.Parse(e.States[3].ToString());
                dicCar[CarNo].Load = int.Parse(e.States[10].ToString());
                dicCar[CarNo].ForkStatus = int.Parse(e.States[11].ToString());
            }
            else if (e.ItemName.IndexOf("CarAlarm") >= 0)
                dicCar[CarNo].AlarmCode = int.Parse(e.States[0].ToString());
            else if (e.ItemName.IndexOf("CarTask") >= 0)
                dicCar[CarNo].TaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(e.States));
            
            Cars.CarInfo(dicCar[CarNo]);
        }

        void Monitor_OnCar(CarEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CarEventHandler(Monitor_OnCar), args);
            }
            else
            {
                Car Car = args.car;
                
                TextBox txt = GetTextBox("txtTaskNo", Car.CarNo);
                if (txt != null)
                    txt.Text = Car.TaskNo;

                txt = GetTextBox("txtStatus", Car.CarNo);
                if (txt != null && dicStatus.ContainsKey(Car.Status))
                {
                    txt.Text = dicStatus[Car.Status];
                    if (txt.Text == "空闲")
                    {
                        txt.BackColor = Color.Lime;
                    }
                    else
                    {
                        txt.BackColor = Color.Yellow;
                    }
                }

                txt = GetTextBox("txtWorkMode", Car.CarNo);
                if (txt != null && dicWorkMode.ContainsKey(Car.Mode))
                {
                    txt.Text = dicWorkMode[Car.Mode];
                    if (Car.Mode == 1)
                    {
                        txt.BackColor = Color.Lime;
                    }
                    else
                    {
                        txt.BackColor = Color.Yellow;
                    }
                }

                Button btn = GetButton(Car.CarNo);
                if (btn == null)
                    return;
                if (Car.Mode == 1)
                    btn.BackColor = Color.Lime;
                else
                    btn.BackColor = Color.Yellow;
                txt = GetTextBox("txtRow", Car.CarNo);
                if (txt != null)
                    txt.Text = Car.Row.ToString();

                txt = GetTextBox("txtColumn", Car.CarNo);
                if (txt != null)
                    txt.Text = Car.Column.ToString();

                
                
                //穿梭车位置
                Point P = new Point();
                if (Car.CarNo == "0101" || Car.CarNo == "0102")
                    P = InitialP1;                    
                else if(Car.CarNo == "0201" || Car.CarNo == "0202")
                    P = InitialP2;
                else if(Car.CarNo == "0301" || Car.CarNo == "0302")
                    P = InitialP3;
                else if(Car.CarNo == "0401" || Car.CarNo == "0402")
                    P = InitialP4;
                else if(Car.CarNo == "0501" || Car.CarNo == "0502")
                    P = InitialP5;
                else if(Car.CarNo == "0601" || Car.CarNo == "0602")
                    P = InitialP6;
                else if(Car.CarNo == "0701" || Car.CarNo == "0702")
                    P = InitialP7;                

                if (Car.Column == 0)
                    P.X = 1010;
                else if (Car.Column == 1)
                    P.X = 984;
                else
                {
                    int res = (Car.Column - 1) % 4;
                    int col = 0;
                    if (res == 0)
                    {
                        col = (Car.Column - 1) / 4 - 1;
                        res = 3;
                    }
                    else
                    {
                        col = (Car.Column - 1) / 4;
                        res = (Car.Column - 1) % 4-1;
                    }
                    P.X = (int)(dicLocationX[col] - (res * 11.34f));
                }                    
                btn.Location = P;                

                txt = GetTextBox("txtLayer", Car.CarNo);
                if (txt != null)
                    txt.Text = Car.Layer.ToString();

                txt = GetTextBox("txtForkStatus", Car.CarNo);
                if (txt != null && dicFork.ContainsKey(Car.ForkStatus))
                    txt.Text = dicFork[Car.ForkStatus];
                txt = GetTextBox("txtAlarmCode", Car.CarNo);
                if (txt != null)
                {
                    txt.Text = Car.AlarmCode.ToString();
                    if (txt.Text == "0")
                    {
                        txt.BackColor = DefaultBackColor;
                    }
                    else
                    {
                        txt.BackColor = Color.Red;
                    }
                }

                string strAlarmDesc = "";
                txt = GetTextBox("txtAlarmDesc", Car.CarNo);
                if (txt != null)
                {
                    if (Car.AlarmCode != 0)
                    {
                        DataRow[] drs = dtDeviceAlarm.Select(string.Format("Flag=2 and AlarmCode={0}", Car.AlarmCode));
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
                    if (txt.Text == "")
                    {
                        txt.BackColor = DefaultBackColor;
                    }
                    else
                    {
                        txt.BackColor = Color.Red;
                    }
                }

                //更新错误代码、错误描述
                //更新任务状态为执行中
                //bll.ExecNonQuery("WCS.UpdateTaskError", new DataParameter[] { new DataParameter("@CarErrCode", Car.ErrCode.ToString()), new DataParameter("@CarErrDesc", dicCarError[Car.ErrCode]), new DataParameter("@TaskNo", Car.TaskNo) });
                if (Car.AlarmCode > 0)
                {
                    //DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", Car.TaskNo), new DataParameter("@CarErrCode", Car.AlarmCode.ToString()), new DataParameter("@CarErrDesc", strAlarmDesc) };
                    //bll.ExecNonQueryTran("WCS.Sp_UpdateTaskError", param);
                    //Logger.Error(Car.CarNo.ToString() + "堆垛机执行时出现错误,代码:" + Car.AlarmCode.ToString() + ",描述:" + strAlarmDesc);
                }
            }
        }
        private Dictionary<string, Car> dicCar = new Dictionary<string, Car>();
        private Car GetCar(string carno)
        {
            Car car = null;
            if (dicCar.ContainsKey(carno))
            {
                car = dicCar[carno];
            }
            else
            {
                car = new Car();
                car.CarNo = carno;
                dicCar.Add(carno, car);
            }
            return car;
        }
        #endregion

        private void AddDicKeyValue()
        {
            dicFork.Add(1, "货叉在原位");
            dicFork.Add(0, "货叉非原位");

            dicStatus.Add(0, "空闲");
            dicStatus.Add(1, "定位");
            dicStatus.Add(2, "左伸取货");
            dicStatus.Add(3, "右伸取货");
            dicStatus.Add(4, "左伸放货");
            dicStatus.Add(5, "右伸放货");
            dicStatus.Add(6, "左伸");
            dicStatus.Add(7, "右伸");
            dicStatus.Add(8, "收叉");
            dicStatus.Add(9, "移库");
            dicStatus.Add(10, "入库");
            dicStatus.Add(11, "出库");
            dicStatus.Add(12, "定位放货");

            dicWorkMode.Add(0, "手动");
            dicWorkMode.Add(1, "自动");

            dicLocationX.Add(0, 960);
            dicLocationX.Add(1, 910);
            dicLocationX.Add(2, 858);
            dicLocationX.Add(3, 806);
            dicLocationX.Add(4, 754);
            dicLocationX.Add(5, 703);
            dicLocationX.Add(6, 651);
            dicLocationX.Add(7, 600);
            dicLocationX.Add(8, 548);
            dicLocationX.Add(9, 496);
            dicLocationX.Add(10, 444);
            dicLocationX.Add(11, 393);
            dicLocationX.Add(12, 341);
            dicLocationX.Add(13, 289);
            dicLocationX.Add(14, 237);
            dicLocationX.Add(15, 186);
            dicLocationX.Add(16, 135);
            dicLocationX.Add(17, 83);

            dtDeviceAlarm = bll.FillDataTable("WCS.SelectDeviceAlarm", new DataParameter[] { new DataParameter("{0}", "Flag=2") });
        }


        Button GetButton(string CarNo)
        {
            Control[] ctl = this.Controls.Find("btnCar" + CarNo, true);
            if (ctl.Length > 0)
                return (Button)ctl[0];
            else
                return null;
        }

        TextBox GetTextBox(string name, string CarNo)
        {
            Control[] ctl = this.Controls.Find(name + CarNo, true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }

        private void Send2Cmd(int CmdType,string DeviceNo)
        {

            string serviceName = "PLC03" + DeviceNo.Substring(0,2);
            string carNo = DeviceNo.Substring(2, 2);
            int[] cellAddr = new int[12];

            object obj = MCP.ObjectUtil.GetObject(base.Context.ProcessDispatcher.WriteToService(serviceName, "CarAlarm" + carNo)).ToString();
            if (obj.ToString() != "0")
            {
                if (CmdType == 0)
                    cellAddr[1] = 1;
                else
                    cellAddr[2] = 1;
                cellAddr[10] = int.Parse(carNo);
                base.Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
                base.Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 1);

                MCP.Logger.Info("已给设备" + DeviceNo + "下发取消任务指令");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string name = (sender as Button).Name;
            string DeviceNo = name.Substring(9, 4);
            Send2Cmd(0,DeviceNo);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            string name = (sender as Button).Name;
            string DeviceNo = name.Substring(7, 4);
            Send2Cmd(1, DeviceNo);
        }

    }
}