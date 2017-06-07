﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Dispatcher
{
    public partial class frmCellQuery : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();
        private Dictionary<int, string> ShelfCode = new Dictionary<int, string>();
        private Dictionary<int, string> ShelfName = new Dictionary<int, string>();
        private Dictionary<int, int> ShelfRow = new Dictionary<int, int>();
        private Dictionary<int, int> ShelfColumn = new Dictionary<int, int>();

        private DataTable cellTable = null;        
        private bool needDraw = false;
        private bool filtered = false;

        private int Columns = 45;
        private int Rows = 5;
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[2];
        private int left = 5;
        string CellCode = "";
        private bool IsWheel = true;
        private string WarehouseCode = "";
        private string WarehouseFilter = "";
        private int pages = 7;
        private int fontSize = 10;

        public frmCellQuery()
        {
            InitializeComponent();
            //设置双缓冲
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);

            Filter.EnableFilter(dgvMain);
            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;

            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);

            this.PColor.Visible = false;

            MCP.Config.Configuration conf = new MCP.Config.Configuration();
            conf.Load("Config.xml");
            WarehouseCode = conf.Attributes["WarehouseCode"];
            WarehouseFilter = string.Format("CMD_Shelf.WarehouseCode='{0}'", WarehouseCode);
            if (WarehouseCode == "A")
            {
                Rows = 5;
                Columns = 45;
                pages = 7;
                sbShelf.Minimum = 0;
                sbShelf.Maximum = 210;
                fontSize = 10;
            }
            else if (WarehouseCode == "B")
            {
                Rows = 5;
                Columns = 45;
                pages = 6;
                sbShelf.Maximum = 180;
                fontSize = 10;
            }
            else
            {
                Rows = 10;
                Columns = 72;
                pages = 7;
                sbShelf.Maximum = 210;
                fontSize = 8;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Filter.Trim().Length != 0)
                {
                    DialogResult result = MessageBox.Show("重新读入数据请选择'是(Y)',清除过滤条件请选择'否(N)'", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.No:
                            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                            return;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                ShelfCode.Clear();
                ShelfName.Clear();

                DataTable dtShelf = bll.FillDataTable("CMD.SelectShelf", new DataParameter[] { new DataParameter("{0}", WarehouseFilter) });
                for (int i = 0; i < dtShelf.Rows.Count; i++)
                {
                    ShelfCode.Add(i + 1, dtShelf.Rows[i]["ShelfCode"].ToString());
                    ShelfName.Add(i + 1, dtShelf.Rows[i]["ShelfName"].ToString());
                }                

                btnRefresh.Enabled = false;
                btnChart.Enabled = false;

                pnlProgress.Top = (pnlMain.Height - pnlProgress.Height) / 3;
                pnlProgress.Left = (pnlMain.Width - pnlProgress.Width) / 2;
                pnlProgress.Visible = true;
                Application.DoEvents();


                cellTable = bll.FillDataTable("WCS.SelectCell", new DataParameter[] { new DataParameter("{0}", WarehouseFilter) });
                bsMain.DataSource = cellTable;

                pnlProgress.Visible = false;
                btnRefresh.Enabled = true;
                btnChart.Enabled = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入数据失败，原因：" + exp.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (cellTable != null && cellTable.Rows.Count != 0)
            {
                if (pnlData.Visible)
                {
                    this.PColor.Visible = true;
                    filtered = bsMain.Filter != null;
                    needDraw = true;
                    btnRefresh.Enabled = false;
                    pnlData.Visible = false;
                    pnlChart.Visible = true;
                    btnChart.Text = "列表";
                }
                else
                {
                    this.PColor.Visible = false;
                    needDraw = false;
                    btnRefresh.Enabled = true;
                    pnlData.Visible = true;
                    pnlChart.Visible = false;
                    btnChart.Text = "图形";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (needDraw)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        int key = currentPage * 2 + i - 1;
                        if (key <= ShelfCode.Count)
                        {
                            if (!shelf.ContainsKey(key))
                            {
                                DataRow[] rows = cellTable.Select(string.Format("ShelfCode='{0}'", ShelfCode[key]), "CellCode desc");
                                shelf.Add(key, rows);
                                ShelfRow.Add(key, int.Parse(rows[0]["Rows"].ToString()));
                                ShelfColumn.Add(key, int.Parse(rows[0]["Columns"].ToString()));

                                SetCellSize(ShelfColumn[key], ShelfRow[key]);
                            }
                            Font font = new Font("微软雅黑", fontSize);
                            SizeF size = e.Graphics.MeasureString("第1排第5层", font);
                            float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                            size = e.Graphics.MeasureString("13", font);
                            float adjustWidth = (cellWidth - size.Width) / 2;

                            DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth);

                            int tmpLeft = left + ShelfColumn[key] * cellWidth + 5;

                            for (int j = 0; j < Rows; j++)
                            {
                                //string s = string.Format("第{0}排第{1}层", key, Convert.ToString(Rows - j).PadLeft(2, '0'));
                                string s = string.Format("{0}-{1}", ShelfName[key], Convert.ToString(Rows - j).PadLeft(2, '0'));
                                e.Graphics.DrawString(s, font, Brushes.DarkCyan, tmpLeft, top[i] + (j + 1) * cellHeight + adjustHeight);
                            }
                        }
                    }

                    if (filtered)
                    {
                        int i = currentPage * 2;
                        foreach (DataGridViewRow gridRow in dgvMain.Rows)
                        {
                            DataRowView cellRow = (DataRowView)gridRow.DataBoundItem;
                            string depth = cellRow["CellCode"].ToString().Substring(9, 1);
                               
                            int shelf = 0;
                            for (int j = 1; j <= ShelfCode.Count; j++)
                            {
                                if (ShelfCode[j].CompareTo(cellRow["ShelfCode"].ToString()) >= 0)
                                {
                                    shelf = j;
                                    break;
                                }
                            }
                            if (shelf == i || shelf == i - 1)
                            {
                                int top = 0;
                                if (shelf % 2 == 0)
                                    top = pnlContent.Height / 2;

                                int column = Convert.ToInt32(cellRow["CellColumn"]);
                                int row = Rows - Convert.ToInt32(cellRow["CellRow"]) + 1;
                                int quantity = ReturnColorFlag(cellRow["PalletBarCode"].ToString(), cellRow["IsActive"].ToString(), cellRow["IsLock"].ToString(), cellRow["ErrorFlag"].ToString());
                                //FillCell(e.Graphics, top, row, column, quantity);
                                FillCell(e.Graphics, top, row, column, quantity, cellRow["ShelfCode"].ToString());
                            }
                        }
                    }
                }
                PColor.Refresh();
                IsWheel = false;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth)
        {
            string shelfCode = "001";
            foreach (DataRow cellRow in cellRows)
            {
                shelfCode = cellRow["ShelfCode"].ToString();
                int column = Convert.ToInt32(cellRow["CellColumn"]) ;
                

                int row = Rows - Convert.ToInt32(cellRow["CellRow"]) + 1;
                int quantity = ReturnColorFlag(cellRow["PalletBarCode"].ToString(), cellRow["IsActive"].ToString(), cellRow["IsLock"].ToString(), cellRow["ErrorFlag"].ToString());

                int x = left + (column-1) * cellWidth;
                int y = top + row * cellHeight;

                Pen pen = new Pen(Color.DarkCyan, 2);
                g.DrawRectangle(pen, new Rectangle(x, y, cellWidth, cellHeight));

                if (!filtered)
                {
                    FillCell(g, top, row, column, quantity, shelfCode);
                }
            }
            for (int j = 1; j <= Columns; j++)
            {
                if (j == 1 && cellRows.Length < Columns * Rows)
                    continue;
                g.DrawString(Convert.ToString(j), new Font("微软雅黑", fontSize), Brushes.DarkCyan, left + (j - 1) * cellWidth + adjustWidth, top + cellHeight * (Rows + 1) + 3);
            }
        }
        
        private void FillCell(Graphics g, int top, int row, int column, int quantity,string shelfCode)
        {           
            int x = left + (column-1) * cellWidth;

            int y = top + row * cellHeight;
            if (quantity == 1)  //空货位锁定
                g.FillRectangle(Brushes.Yellow, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 2) //有货未锁定
                g.FillRectangle(Brushes.Blue, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 3) //有货且锁定
                g.FillRectangle(Brushes.Green, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 4) //禁用
                g.FillRectangle(Brushes.Gray, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 5) //有问题
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 6) //托盘
                g.FillRectangle(Brushes.Orange, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 7) //托盘锁定
                g.FillRectangle(Brushes.Gold, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
        }
        private void pnlChart_Resize(object sender, EventArgs e)
        {
            
            SetCellSize(Columns, Rows);
            top[0] = 0;
            top[1] = pnlContent.Height / 2;            
        }

        private void SetCellSize(int Columns, int Rows)
        {
            cellWidth = (pnlContent.Width - sbShelf.Width - 40) / Columns;
            cellHeight = (pnlContent.Height / 2) / (Rows + 2);
        }

        private void pnlChart_MouseClick(object sender, MouseEventArgs e)
        {
            int i = e.Y < top[1] ? 0 : 1;
            int shelf = currentPage * 2 + i - 1;
            if (shelf > ShelfCode.Count)
                return;
            int column = (e.X - left) / cellWidth +1;

            int row = Rows - (e.Y - top[i]) / cellHeight + 1;

            if (column <= Columns && row <= Rows)
            {
                string filter = string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row);

                DataRow[] cellRows = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                if (cellRows.Length != 0)
                    CellCode = cellRows[0]["CellCode"].ToString();
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (cellRows.Length != 0)
                    {
                        //if (cellRows[0]["PalletBarCode"].ToString() != "")
                        //{
                        //    frmCellInfo f = new frmCellInfo(cellRows[0]["PalletBarCode"].ToString(), CellCode);
                        //    f.ShowDialog();
                        //}
                        Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
                        Dictionary<string, object> property = new Dictionary<string, object>();
                        //property.Add("产品编号", cellRows[0]["PalletBarCode"]);
                        //property.Add("产品名称", cellRows[0]["ProductName"]);
                        //property.Add("入库类型", cellRows[0]["BillTypeName"]);
                        //property.Add("产品状态", cellRows[0]["StateName"]);
                        property.Add("托盘条码", cellRows[0]["PalletBarcode"]);
                        //property.Add("产品类型", cellRows[0]["ProductTypeName"]);
                        //property.Add("托盘", cellRows[0]["PalletCode"]);

                        property.Add("任务号", cellRows[0]["BillNo"]);
                        property.Add("入库时间", cellRows[0]["InDate"]);
                        properties.Add("产品信息", property);

                        property = new Dictionary<string, object>();
                        property.Add("库区名称", cellRows[0]["AreaName"]);
                        property.Add("货架名称", cellRows[0]["ShelfName"]);
                        property.Add("列", column);
                        property.Add("层", row);
                        string strState = "正常";
                        if (cellRows[0]["IsLock"].ToString() == "0")
                            strState = "正常";
                        else
                            strState = "锁定";
                        if (cellRows[0]["ErrorFlag"].ToString() == "1")
                            strState = "异常";

                        if (cellRows[0]["IsActive"].ToString() == "0")
                            strState = "禁用";

                        property.Add("状态", strState);
                        properties.Add("货位信息", property);
                        if (cellRows[0]["PalletBarCode"].ToString().Length > 0)
                        {
                            frmCellDialog cellDialog = new frmCellDialog(properties);
                            cellDialog.ShowDialog();
                        }
                    }
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }        
        private void pnlChart_MouseEnter(object sender, EventArgs e)
        {
            pnlChart.Focus();
        }

        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            IsWheel = true;
            if (e.Delta < 0 && currentPage <= pages)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;
        }

        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos > pages)
                return;
            if (pos != currentPage)
            {
                currentPage = pos;
                pnlChart.Invalidate();
                SetCellSize(Columns, Rows);
            }
        }

        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvMain.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvMain.RowHeadersDefaultCellStyle.Font, rectangle, dgvMain.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private int ReturnColorFlag(string ProductCode, string IsActive, string IsLock, string ErrFlag)
        {
            if (ProductCode.Length > 0)
            {
                string s = "";
            }
            int Flag = 0;
            if (ProductCode == "")
            {
                if (IsLock == "1")
                {
                    Flag = 1;
                }
            }
            else
            {
                if (IsLock == "0")
                {
                    Flag = 2;
                }
                else
                {
                    Flag = 3;
                }
            }
            if (IsActive == "0")
                Flag = 4;
            if (ErrFlag == "1")
                Flag = 5;
            return Flag;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow[] drs = cellTable.Select(string.Format("CellCode='{0}'", CellCode));
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                frmCellOpDialog cellDialog = new frmCellOpDialog(dr);
                if (cellDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    cellTable = bll.FillDataTable("WCS.SelectCell");
                    bsMain.DataSource = cellTable;
                    pnlChart.Invalidate();
                }
            }
        }

        private int X, Y;
        private void pnlChart_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsWheel) return;
            if (X != e.X || Y != e.Y)
            {
                int i = e.Y < top[1] ? 0 : 1;
                
                int shelf = currentPage * 2 + i - 1;
                if (shelf > ShelfCode.Count)
                    return;
                int column =  (e.X - left) / cellWidth + 1;
                int row = Rows - (e.Y - top[i]) / cellHeight + 1;
                if (column <= Columns && row <= Rows && row > 0 && column > 0)
                {
                    string tip = "货架:" + shelf.ToString() + ";列:" + column.ToString() + ";层:" + row.ToString();
                    toolTip1.SetToolTip(pnlChart, tip);
                }
                else
                    toolTip1.SetToolTip(pnlChart, null);

                X = e.X;
                Y = e.Y;
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            cellTable = bll.FillDataTable("WCS.SelectCell");
            bsMain.DataSource = cellTable;
            pnlChart.Invalidate();
        }       
    }
}
