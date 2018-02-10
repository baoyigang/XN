using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCP;
using Util;
using DataGridViewAutoFilter;

namespace App
{
    public partial class Main : Form
    {
        private bool IsActiveForm = false;
        public bool IsActiveTab = false;
        private Context context = null;
        private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();
        private System.Timers.Timer tmUpErpWorkTimer = new System.Timers.Timer();
        BLL.BLLBase bll = new BLL.BLLBase();

        public Main()
        {
            InitializeComponent();
        }

        private void inStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInStock f = new View.Task.frmInStock();
            ShowForm(f);
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            try
            {
                lbLog.Scrollable = true;
                Logger.OnLog += new LogEventHandler(Logger_OnLog);
                FormDialog.OnDialog += new DialogEventHandler(FormDialog_OnDialog);
                context = new Context();

                ContextInitialize initialize = new ContextInitialize();
                initialize.InitializeContext(context);
                View.BaseForm f;
                if (Program.WarehouseCode == "A")
                    f = new View.frmMonitorA();
                else if (Program.WarehouseCode == "B")
                    f = new View.frmMonitorB();
                else
                    f = new View.frmMonitor();
                ShowForm(f);

                MainData.OnTask += new TaskEventHandler(Data_OnTask);
                this.BindData();
                for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                    ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;

                tmWorkTimer.Interval = 3000;
                tmWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmWorker);
                tmWorkTimer.Start();

                tmUpErpWorkTimer.Interval = 600000;
                tmUpErpWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmUpErpWorker);
                tmUpErpWorkTimer.Start();

                PermissionControl();
            }
            catch (Exception ee)
            {
                Logger.Error("初始化处理失败请检查配置，原因：" + ee.Message);
            }
        }
        private void PermissionControl()
        {            
            DataTable dt = Program.dtUserPermission;
            //监控任务--取消堆垛机任务
            string filter = "SubModuleCode='MNU_W00A_00A' and OperatorCode='1'";
            DataRow[] drs = dt.Select(filter);
            //if (drs.Length <= 0)
            //    this.ToolStripMenuItemDelCraneTask.Visible = false;  
            //else
            //    this.ToolStripMenuItemDelCraneTask.Visible = true;  
            //监控任务--重新下发堆垛机任务
            filter = "SubModuleCode='MNU_W00A_00A' and OperatorCode='2'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItemReassign.Visible = false;
            else
                this.ToolStripMenuItemReassign.Visible = true;
            //监控任务--任务状态切换
            filter = "SubModuleCode='MNU_W00A_00A' and OperatorCode='3'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItemStateChange.Visible = false;
            else
                this.ToolStripMenuItemStateChange.Visible = true;
            //监控任务--重新申请货位
            filter = "SubModuleCode='MNU_W00A_00A' and OperatorCode='4'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItemCellCode.Visible = true;
            else
                this.ToolStripMenuItemCellCode.Visible = false;
            //入库任务
            filter = "SubModuleCode='MNU_W00A_00B' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.toolStripButton_InStockTask.Visible = false;
                this.inStockToolStripMenuItem.Visible = false;
            }
            else
            {
                this.toolStripButton_InStockTask.Visible = true;
                this.inStockToolStripMenuItem.Visible = true;
            }
            //出库任务
            filter = "SubModuleCode='MNU_W00A_00C' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.toolStripButton_OutStock.Visible = false;
                this.OutStockToolStripMenuItem.Visible = false;
            }
            else
            {
                this.toolStripButton_OutStock.Visible = true;
                this.OutStockToolStripMenuItem.Visible = true;
            }
            //盘点任务
            filter = "SubModuleCode='MNU_W00A_00D' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.toolStripButton_Inventor.Visible = false;
                this.InventortoolStripMenuItem.Visible = false;
            }

            //移库任务
            filter = "SubModuleCode='MNU_W00A_00E' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.toolStripButton_MoveStock.Visible = false;
                this.MoveStockToolStripMenuItem.Visible = false;
            }
            //设备测试
            filter = "SubModuleCode='MNU_W00B_00A' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.toolStripButton_UnitLoad.Visible = false;
            else
                this.toolStripButton_UnitLoad.Visible = true;
            //货位监控
            filter = "SubModuleCode='MNU_W00B_00C' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
            {
                this.toolStripButton_CellMonitor.Visible = false;
                this.ToolStripMenuItem_Cell.Visible = false;
            }
            else
            {
                this.toolStripButton_CellMonitor.Visible = true;
                this.ToolStripMenuItem_Cell.Visible = true;
            }
            //联机自动
            filter = "SubModuleCode='MNU_W00B_00D' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.toolStripButton_StartCrane.Visible = false;
            else
                this.toolStripButton_StartCrane.Visible = true;
            //退出系统
            filter = "SubModuleCode='MNU_W00B_00E' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.toolStripButton_Close.Visible = false;
            else
                this.toolStripButton_Close.Visible = true;
            //参数设定
            filter = "SubModuleCode='MNU_W00C_00A' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_Param.Visible = false;
            else
                this.ToolStripMenuItem_Param.Visible = true;
            //设备管理
            filter = "SubModuleCode='MNU_W00C_00B' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_Device.Visible = false;
            else
                this.ToolStripMenuItem_Device.Visible = true;
            //用户资料
            filter = "SubModuleCode='MNU_W00C_00C' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_Device.Visible = false;
            else
                this.ToolStripMenuItem_Device.Visible = true;
            //用户组管理
            filter = "SubModuleCode='MNU_W00C_00D' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_Group.Visible = false;
            else
                this.ToolStripMenuItem_Group.Visible = true;
            //权限管理
            filter = "SubModuleCode='MNU_W00C_00E' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_Power.Visible = false;
            else
                this.ToolStripMenuItem_Power.Visible = true;
            //密码修改
            filter = "SubModuleCode='MNU_W00C_00F' and OperatorCode='1'";
            drs = dt.Select(filter);
            if (drs.Length <= 0)
                this.ToolStripMenuItem_ChangPwd.Visible = false;
            else
                this.ToolStripMenuItem_ChangPwd.Visible = true;
        }
        private void tmUpErpWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    tmUpErpWorkTimer.Stop();
                    App.Dispatching.Process.Report report = new Dispatching.Process.Report();
                    DataParameter[] para = new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.State in (7,9) and WCS_TASK.FinishReturnCode!='000' and WCS_TASK.WarehouseCode='{0}'", Program.WarehouseCode)) };
                    DataTable dt = bll.FillDataTable("WCS.SelectTask", para);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string TaskNo = dt.Rows[i]["TaskNo"].ToString();
                        int Flag = 3;
                        if (dt.Rows[i]["State"].ToString() == "9")
                            Flag = 5;
                        report.Send2MJWcs(context, Flag, TaskNo);
                        Logger.Info("WCS重新上传任务完成标志，任务号:" + TaskNo);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
                finally
                {
                    tmUpErpWorkTimer.Start();
                }
            }
        }      

        private void tmWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();
                DataTable dt = GetMonitorData();
                MainData.TaskInfo(dt);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }

   

        void Logger_OnLog(MCP.LogEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LogEventHandler(Logger_OnLog), args);
            }
            else
            {
                lock (lbLog)
                {
                    string msg1 = string.Format("[{0}]", args.LogLevel);
                    string msg2 = string.Format(" {0}", DateTime.Now);
                    string msg3 = string.Format(" {0}", args.Message);
                    if (args.LogLevel != LogLevel.DEBUG)
                    {
                        this.lbLog.BeginUpdate();
                        ListViewItem item = new ListViewItem(new string[] { msg1, msg2, msg3 });

                        if (msg1.Contains("[ERROR]"))
                        {
                            //item.ForeColor = Color.Red;
                            item.BackColor = Color.Red;
                        }
                        lbLog.Items.Insert(0, item);
                        this.lbLog.EndUpdate();
                    }
                    WriteLoggerFile(msg1 + msg2  + msg3);
                }
            }
        }
        string FormDialog_OnDialog(DialogEventArgs args)
        {

            string strValue = "";
            if (InvokeRequired)
            {
                return (string)this.Invoke(new DialogEventHandler(FormDialog_OnDialog), args);
            }
            else
            {
                if (args.Message[0] == "1")//出库
                {
                    View.Dispatcher.frmOutView frm = new View.Dispatcher.frmOutView(int.Parse(args.Message[0]), args.dtInfo,context);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        strValue = frm.strValue;
                        //bool tt = context.ProcessDispatcher.WriteToService("CarPLC2", "PickFinished", 1);
                    }
                }
                if (args.Message[0] == "2")//盘点
                {
                    View.Dispatcher.frmScan frm = new View.Dispatcher.frmScan(int.Parse(args.Message[0]), args.dtInfo);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        strValue = frm.strValue;                        
                    }
                }
            }
            return strValue;
        }

        private void CreateDirectory(string directoryName)
        {
            if (!System.IO.Directory.Exists(directoryName))
                System.IO.Directory.CreateDirectory(directoryName);
        }

        private void WriteLoggerFile(string text)
        {
            try
            {
                string path = "";
                CreateDirectory("日志");
                path = "日志";
                path = path + @"/" + DateTime.Now.ToString().Substring(0, 4).Trim();
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 7).Trim();
                path = path.TrimEnd(new char[] { '-' });
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                System.IO.File.AppendAllText(path, string.Format("{0}",  text + "\r\n"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        #region 公共方法
        /// <summary>
        /// 打开一个窗体

        /// </summary>
        /// <param name="frm"></param>
        private void ShowForm(Form frm)
        {
            if (OpenOnce(frm))
            {
                frm.MdiParent = this;
                ((View.BaseForm)frm).Context = context;
                frm.Show();
                frm.WindowState = FormWindowState.Maximized;
                AddTabPage(frm.Handle.ToString(), frm.Text);
            }
        }
        /// <summary>
        /// 判断窗体是否已打开
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool OpenOnce(Form frm)
        {
            foreach (Form mdifrm in this.MdiChildren)
            {
                int index = mdifrm.Text.IndexOf(" ");
                if (index > 0)
                {
                    if (frm.Name == mdifrm.Name && frm.Text == mdifrm.Text.Substring(0, index))
                    {
                        mdifrm.Activate();
                        return false;
                    }
                }
                else
                {
                    if (frm.Name == mdifrm.Name && frm.Text == mdifrm.Text)
                    {
                        mdifrm.Activate();
                        return false;
                    }
                }
            }
            return true;
        }
        
        private void AddTabPage(string strKey, string strText)
        {
            IsActiveForm = true;
            TabPage tab = new TabPage();
            tab.Name = strKey.ToString();
            tab.Text = strText;
            tabForm.TabPages.Add(tab);
            tabForm.SelectedTab = tab;
            this.pnlTab.Visible = true;
            IsActiveForm = false;
        }
        
        public void SetActiveTab(string strKey, bool blnActive)
        {
            foreach (TabPage tab in this.tabForm.TabPages)
            {
                if (tab.Name == strKey)
                {
                    IsActiveForm = true;

                    if (blnActive)
                        tabForm.SelectedTab = tab;
                    else
                    {
                        tabForm.TabPages.Remove(tab);
                        if (this.MdiChildren.Length > 1)
                            this.pnlTab.Visible = true;
                        else
                            this.pnlTab.Visible = false;
                    }

                    IsActiveForm = false; ;
                }
            }
        }
        private void tabForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsActiveForm)
                return;
            foreach (Form mdifrm in this.MdiChildren)
            {
                if (mdifrm.Handle.ToInt32() == int.Parse(((TabControl)sender).SelectedTab.Name))
                {
                    IsActiveTab = true;
                    mdifrm.Activate();
                    IsActiveTab = false;
                }
            }
        }
        #endregion

        private void Main_Load(object sender, EventArgs e)
        {
           
                
        }

        private void ToolStripMenuItem_Cell_Click(object sender, EventArgs e)
        {
            App.View.Dispatcher.frmCellQuery f = new App.View.Dispatcher.frmCellQuery();
            ShowForm(f);
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Logger.Info("退出系统");
                System.Environment.Exit(0);
            }
        }

        private void toolStripButton_InStockTask_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInStock f = new View.Task.frmInStock();
            ShowForm(f);
        }

        private void toolStripButton_OutStock_Click(object sender, EventArgs e)
        {
            App.View.Task.frmOutStock f = new View.Task.frmOutStock();
            ShowForm(f);
        }

        private void OutStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmOutStock f = new View.Task.frmOutStock();
            ShowForm(f);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Logger.Info("退出系统");
                System.Environment.Exit(0);
            }
            else
                e.Cancel = true;
        }

        private void MoveStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmMoveStock f = new View.Task.frmMoveStock();
            ShowForm(f);
        }

        private void toolStripButton_MoveStock_Click(object sender, EventArgs e)
        {
            App.View.Task.frmMoveStock f = new View.Task.frmMoveStock();
            ShowForm(f);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            View.Task.frmDeviceTask f = new View.Task.frmDeviceTask();
            ShowForm(f);
        }

        private void toolStripButton_StartCrane_Click(object sender, EventArgs e)
        {
            if (this.toolStripButton_StartCrane.Text == "联机自动")
            {
                if(Program.WarehouseCode=="S")
                    context.ProcessDispatcher.WriteToProcess("ElevatorProcess", "Run", 1);
                else
                    context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 1);
                
                this.toolStripButton_StartCrane.Image = App.Properties.Resources.process_accept;
                this.toolStripButton_StartCrane.Text = "脱机";
            }
            else
            {
                if (Program.WarehouseCode == "S")
                    context.ProcessDispatcher.WriteToProcess("ElevatorProcess", "Run", 0);
                else
                    context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 0);
                this.toolStripButton_StartCrane.Image = App.Properties.Resources.process_remove;
                this.toolStripButton_StartCrane.Text = "联机自动";
            }            
        }

        private void toolStripButton_Inventor_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInventor f = new View.Task.frmInventor();
            ShowForm(f);
        }

        private void InventortoolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInventor f = new View.Task.frmInventor();
            ShowForm(f);
        }

        private void toolStripButton_CellMonitor_Click(object sender, EventArgs e)
        {
            App.View.Dispatcher.frmCellQuery f = new App.View.Dispatcher.frmCellQuery();
            ShowForm(f);
        }

        private void ToolStripMenuItem_Param_Click(object sender, EventArgs e)
        {
            App.View.Param.ParameterForm f = new App.View.Param.ParameterForm();
            ShowForm(f);
        }

        private void toolStripButton_Scan_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInStockTask f = new App.View.Task.frmInStockTask();
            f.ShowDialog();
        }

        #region 正执行任务处理
        void Data_OnTask(TaskEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TaskEventHandler(Data_OnTask), args);
            }
            else
            {
                lock (this.dgvMain)
                {
                    DataTable dt = args.datatTable;
                    this.bsMain.DataSource = dt;
                    for (int i = 0; i < this.dgvMain.Rows.Count; i++)
                    {
                        if (dgvMain.Rows[i].Cells["colAlarmCode"].Value.ToString() != "0")
                            this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        else
                        {
                            if (i % 2 == 0)
                                this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            else
                                this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);

                        }
                    }
                }
            }
        }


        private void BindData()
        {
            bsMain.DataSource = GetMonitorData();
        }
        private DataTable GetMonitorData()
        {
            string filter = string.Format("WCS_TASK.WarehouseCode='{0}' and WCS_TASK.State not in('7','9')", Program.WarehouseCode);
            DataTable dt = bll.FillDataTable("WCS.SelectTask1", new DataParameter[] { new DataParameter("{0}", filter) });
            return dt;
        }

        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvMain.Rows[e.RowIndex].Selected == false)
                    {
                        dgvMain.ClearSelection();
                        dgvMain.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvMain.SelectedRows.Count == 1)
                    {
                        dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    string TaskType = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells["colTaskType"].Value.ToString();
                    if (TaskType == "11" || TaskType == "16")
                    {
                        this.ToolStripMenuItem11.Visible = true;
                        this.ToolStripMenuItem12.Visible = true;
                        this.ToolStripMenuItem13.Visible = true;
                        this.ToolStripMenuItem14.Visible = false;
                        this.ToolStripMenuItem15.Visible = false;
                        this.ToolStripMenuItem16.Visible = false;
                        this.ToolStripMenuItem17.Visible = true;
                        this.ToolStripMenuItem18.Visible = false;
                        this.ToolStripMenuItem19.Visible = true;
                    }
                    else if (TaskType == "12" || TaskType == "15")
                    {
                        this.ToolStripMenuItem11.Visible = false;
                        this.ToolStripMenuItem12.Visible = false;
                        this.ToolStripMenuItem13.Visible = false;
                        this.ToolStripMenuItem14.Visible = true;
                        this.ToolStripMenuItem15.Visible = false;
                        this.ToolStripMenuItem16.Visible = false;
                        this.ToolStripMenuItem17.Visible = true;
                        this.ToolStripMenuItem18.Visible = false;
                        this.ToolStripMenuItem19.Visible = true;
                    }
                    else if (TaskType == "13")
                    {
                        this.ToolStripMenuItem11.Visible = false;
                        this.ToolStripMenuItem12.Visible = false;
                        this.ToolStripMenuItem13.Visible = false;
                        this.ToolStripMenuItem14.Visible = true;
                        this.ToolStripMenuItem15.Visible = false;
                        this.ToolStripMenuItem16.Visible = false;
                        this.ToolStripMenuItem17.Visible = true;
                        this.ToolStripMenuItem18.Visible = false;
                        this.ToolStripMenuItem19.Visible = true;
                    }
                    else if (TaskType == "14")
                    {
                        this.ToolStripMenuItem10.Visible = true;
                        this.ToolStripMenuItem11.Visible = true;
                        this.ToolStripMenuItem12.Visible = true;
                        this.ToolStripMenuItem13.Visible = true;
                        this.ToolStripMenuItem14.Visible = true;
                        this.ToolStripMenuItem15.Visible = true;
                        this.ToolStripMenuItem16.Visible = true;
                        this.ToolStripMenuItem17.Visible = true;
                        this.ToolStripMenuItem18.Visible = true;
                        this.ToolStripMenuItem19.Visible = true;
                    }
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void ToolStripMenuItemCellCode_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string TaskType = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells["colTaskType"].Value.ToString();
                string AlarmCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells["colAlarmCode"].ToString();
                if (AlarmCode == "503" || AlarmCode == "504")
                {
                    App.Dispatching.Process.Report report = new Dispatching.Process.Report();
                    report.Send2MJWcs(context, 4, TaskNo);
                }
                else
                    Logger.Info("非重入或入库货位阻塞不可重新申请货位");
            }
        }

        private void ToolStripMenuItemReassign_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                DataRowView drv = dgvMain.SelectedRows[0].DataBoundItem as DataRowView;
                DataRow dr = drv.Row;
                string TaskType = dr["TaskType"].ToString();
                string State = dr["State"].ToString();
                string DeviceNo = dr["DeviceNo"].ToString();
                string serviceName = "PLC" + DeviceNo;
                if (Program.WarehouseCode == "S")
                    serviceName = "PLC03" + DeviceNo.Substring(0, 2);
                string TaskNo = dr["TaskNo"].ToString();
                string PalletBarcode = dr["PalletBarcode"].ToString();
                string NewCellCode = dr["NewCellCode"].ToString();
                string NewAddress = dr["NewAddress"].ToString();
                int AlarmCode = int.Parse(dr["AlarmCode"].ToString());

                string fromStation = dr["FromStation"].ToString();
                string toStation = dr["ToStation"].ToString();
                string FromStationAdd = dr["FromAddress"].ToString();
                string ToStationAdd = dr["ToAddress"].ToString();

                if (fromStation.Trim() == "" || toStation.Trim() == "")
                {
                    Logger.Info(TaskNo + "目标位置或者起始位置错误,无法重新下达任务！");
                    return;
                }
                int[] cellAddr = new int[12];

                if (Program.WarehouseCode != "S")
                {
                    cellAddr[6] = 1;
                    //判断是否要用MJ-WCS重新下发的货位下发任务
                    if (AlarmCode > 500 && AlarmCode < 503)
                    {
                        fromStation = NewCellCode;
                        FromStationAdd = NewAddress;
                        cellAddr[6] = 1;
                    }
                    if (AlarmCode > 502 && AlarmCode < 505)
                    {
                        toStation = NewCellCode;
                        ToStationAdd = NewAddress;
                        cellAddr[6] = 2;
                    }
                    if (State == "3" || State == "4")
                    {

                        cellAddr[0] = byte.Parse(FromStationAdd.Substring(4, 3));
                        cellAddr[1] = byte.Parse(FromStationAdd.Substring(7, 3));
                        cellAddr[2] = byte.Parse(FromStationAdd.Substring(1, 3));
                        cellAddr[3] = byte.Parse(ToStationAdd.Substring(4, 3));
                        cellAddr[4] = byte.Parse(ToStationAdd.Substring(7, 3));
                        cellAddr[5] = byte.Parse(ToStationAdd.Substring(1, 3));
                        sbyte[] taskNo = new sbyte[20];
                        //Util.ConvertStringChar.stringToBytes(TaskNo, 20).CopyTo(taskNo, 0);
                        Util.ConvertStringChar.stringToBytes(PalletBarcode, 20).CopyTo(taskNo, 0);
                        context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
                        context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
                        context.ProcessDispatcher.WriteToService(serviceName, "STB", 1);

                        Logger.Info("任务:" + TaskNo + "条码为:" + PalletBarcode + "已下发给设备" + DeviceNo + "起始地址:" + fromStation + ",目标地址:" + toStation);
                    }
                    else
                    {
                        Logger.Info("非正在上下架的任务无法重新下发");
                        return;
                    }
                }
                else
                {
                    //判断是否要用MJ-WCS重新下发的货位下发任务
                    if (AlarmCode == 14)
                    {
                        toStation = NewCellCode;
                        ToStationAdd = NewAddress;

                        cellAddr[1] = 2;
                    }
                    if (AlarmCode == 11 || AlarmCode == 13)
                    {
                        fromStation = NewCellCode;
                        FromStationAdd = NewAddress;
                        cellAddr[1] = 3;
                    }
                    if (State == "3" || State == "4")
                    {
                        cellAddr[3] = byte.Parse(FromStationAdd.Substring(1, 3));
                        cellAddr[4] = byte.Parse(FromStationAdd.Substring(4, 3));
                        cellAddr[5] = byte.Parse(FromStationAdd.Substring(7, 3));
                        cellAddr[6] = byte.Parse(ToStationAdd.Substring(1, 3));
                        cellAddr[7] = byte.Parse(ToStationAdd.Substring(4, 3));
                        cellAddr[8] = byte.Parse(ToStationAdd.Substring(7, 3));
                        if (TaskType == "11")
                        {
                            cellAddr[9] = 10;
                        }
                        else if (TaskType == "12")
                        {
                            cellAddr[7] = 0;
                            cellAddr[8] = cellAddr[5];
                            cellAddr[9] = 11;
                        }
                        else if (TaskType == "13")
                            cellAddr[9] = 9;

                        string CellCode = dr["CellCode"].ToString();
                        //判断是否需要避车
                        //if (CellCode.Substring(6, 2) != NewCellCode.Substring(6, 2))
                        //{
                        //    int CurLayer = 0;
                        //    int ToLayer = 0;
                        //    if (TaskType == "11")
                        //    {
                        //        CurLayer = 0;
                        //        ToLayer = 0;
                        //    }
                        //    else if (TaskType == "12")
                        //    {
                        //        CurLayer = 0;
                        //        ToLayer = 0;
                        //    }

                        //}
                        cellAddr[10] = int.Parse(DeviceNo.Substring(2, 2));
                        sbyte[] taskNo = new sbyte[30];
                        Util.ConvertStringChar.stringToBytes(TaskNo, 30).CopyTo(taskNo, 0);
                        //Util.ConvertStringChar.stringToBytes(PalletBarcode, 30).CopyTo(taskNo, 0);
                        context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
                        context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
                        context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 1);
                        Logger.Info("任务:" + TaskNo + "条码为:" + PalletBarcode + "已下发给设备" + DeviceNo + "起始地址:" + fromStation + ",目标地址:" + toStation);
                    }
                    else
                    {
                        Logger.Info("非正在上下架的任务无法重新下发");
                        return;
                    }
                }
                this.BindData();
            }
        }
       
        
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ItemName = ((ToolStripMenuItem)sender).Name;
            string State = ItemName.Substring(ItemName.Length - 1, 1);

            if (this.dgvMain.CurrentCell != null)
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string TaskState = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                if (TaskState=="等待" && State == "7")
                {
                    frmChangefalse frm = new frmChangefalse();
                    frm.ShowDialog();
                    return;
                }

                DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo), new DataParameter("@State", State) };
                bll.ExecNonQueryTran("WCS.Sp_UpdateTaskState", param);

                BindData();
                Logger.Info("任务:" + TaskNo + " 手动切换状态为:" + State);
                try
                {
                    App.Dispatching.Process.Report report = new Dispatching.Process.Report();
                    if (State == "7")
                        report.Send2MJWcs(context, 3, TaskNo);
                    else if (State == "9")
                        report.Send2MJWcs(context, 5, TaskNo);
                }
                catch (Exception ex)
                {
                    Logger.Error("切换状态，上报MJ-WCS时发生错误：" + ex.Message);
                }
            }
        }

        private void ToolStripMenuItemDelCraneTask_Click(object sender, EventArgs e)
        {
            string DeviceNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells["colDeviceNo"].Value.ToString();
            if (DeviceNo.Trim().Length <= 0)
                return;
            string serviceName = "PLC" + DeviceNo;
            int[] cellAddr = new int[12];
            
            object obj = MCP.ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "AlarmCode")).ToString();
            if (obj.ToString() != "0")
            {
                cellAddr[6] = 5;
                sbyte[] taskNo = new sbyte[30];
                for (int i = 0; i < 30; i++)
                    taskNo[i] = 32;
                //Util.ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);
                context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
                context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
                context.ProcessDispatcher.WriteToService(serviceName, "STB", 1);

                MCP.Logger.Info("已给设备" + DeviceNo + "下发取消任务指令");
            }
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            View.Task.frmCarTask f = new View.Task.frmCarTask();
            ShowForm(f);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            View.Task.frmMiniLoadTask f = new View.Task.frmMiniLoadTask();
            ShowForm(f);
        }

        private void toolStripButton_Barcode_Click(object sender, EventArgs e)
        {
            View.Base.frmBarcode f = new View.Base.frmBarcode();
            ShowForm(f);
        }
    
        private void Send2PLC(DataRow dr)
        {
            string DeviceNo = dr["DeviceNo"].ToString();
            string serviceName = "PLC" + DeviceNo;
            string TaskNo = dr["TaskNo"].ToString();

            string fromStation = dr["FromStation"].ToString();
            string toStation = dr["ToStation"].ToString();

            int[] cellAddr = new int[12];

            string FromStationAdd = dr["FromAddress"].ToString();
            string ToStationAdd = dr["ToAddress"].ToString();

            cellAddr[0] = byte.Parse(FromStationAdd.Substring(4, 3));
            cellAddr[1] = byte.Parse(FromStationAdd.Substring(7, 3));
            cellAddr[2] = byte.Parse(FromStationAdd.Substring(1, 3));
            cellAddr[3] = byte.Parse(ToStationAdd.Substring(4, 3));
            cellAddr[4] = byte.Parse(ToStationAdd.Substring(7, 3));
            cellAddr[5] = byte.Parse(ToStationAdd.Substring(1, 3));

            cellAddr[6] = 1;

            sbyte[] taskNo = new sbyte[20];
            Util.ConvertStringChar.stringToBytes(TaskNo, 20).CopyTo(taskNo, 0);
            context.ProcessDispatcher.WriteToService(serviceName, "TaskNo", taskNo);
            context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);
            context.ProcessDispatcher.WriteToService(serviceName, "STB", 1);

            Logger.Info("任务:" + TaskNo + "已下发给设备" + DeviceNo + "起始地址:" + fromStation + ",目标地址:" + toStation);
        }        

        private void ToolStripMenuItem_User_Click(object sender, EventArgs e)
        {
            App.Account.frmUserList f = new App.Account.frmUserList();
            ShowForm(f);
        }

        private void ToolStripMenuItem_Group_Click(object sender, EventArgs e)
        {
            App.Account.frmGroupList f = new App.Account.frmGroupList();
            ShowForm(f);
        }

        private void ToolStripMenuItem_Power_Click(object sender, EventArgs e)
        {
            App.Account.frmGroupManage f = new App.Account.frmGroupManage();
            ShowForm(f);
        }

        private void ToolStripMenuItem_ChangPwd_Click(object sender, EventArgs e)
        {
            App.Account.frmChangePWD f = new Account.frmChangePWD();
            f.ShowDialog();
        }

        private void ToolStripMenuItem_Device_Click(object sender, EventArgs e)
        {
            App.View.Base.frmDevices f = new App.View.Base.frmDevices();
            ShowForm(f);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void ToolStripMenuItem_Efficiency_Click(object sender, EventArgs e)
        {
            App.View.Report.frmDeviceEfficiency f = new View.Report.frmDeviceEfficiency();
            f.ShowDialog();
        }
    }
}
