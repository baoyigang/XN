﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using DataGridViewAutoFilter;

namespace App.View.Task
{
    public partial class frmInventor : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmInventor()
        {
            InitializeComponent();
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_Refresh_Click(object sender, EventArgs e)
        {
            BindData();
            
        }        

        private void toolStripButton_Cancel_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentRow.Index >= 0)
            {
                if (this.dgvMain.SelectedRows[0].Cells["colState"].Value.ToString() == "等待")
                {
                    if (DialogResult.Yes == MessageBox.Show("您确定要取消此任务吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        string TaskNo = this.dgvMain.SelectedRows[0].Cells["colTaskNo"].Value.ToString();
                        DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                        bll.ExecNonQueryTran("WCS.Sp_TaskCancelProcess", param);
                        this.BindData();
                    }
                }
                else
                {
                    MessageBox.Show("选中的状态非[等待],请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void BindData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectTask", new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.WarehouseCode = '{0}' and WCS_TASK.State in('0','1','2','3','4','5','6') and convert(varchar(10),WCS_TASK.TaskDate,120)=convert(varchar(10),getdate(),120) and WCS_TASK.TaskType='14'", Program.WarehouseCode)) });
            bsMain.DataSource = dt;
        }
        private void BindData(string filter)
        {
            DataTable dt = bll.FillDataTable("WCS.SelectTask", new DataParameter[] { new DataParameter("{0}", string.Format("WCS_TASK.WarehouseCode = '{0}' and WCS_TASK.State in('0','1','2','3','4','5','6','7') and WCS_TASK.TaskType='14' and {1}", Program.WarehouseCode, filter)) });
            bsMain.DataSource = dt;
        }
        private void frmMoveStock_Load(object sender, EventArgs e)
        {
            //this.BindData();
            for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;

            //DataTable dt = Program.dtUserPermission;
            ////盘点任务--取消任务
            //string filter = "SubModuleCode='MNU_W00A_00D' and OperatorCode='2'";
            //DataRow[] drs = dt.Select(filter);
            //if (drs.Length <= 0)
            //    this.toolStripButton_Cancel.Visible = false;
            //else
            //    this.toolStripButton_Cancel.Visible = true;
            //filter = "SubModuleCode='MNU_W00A_00D' and OperatorCode='3'";
            //drs = dt.Select(filter);
            //if (drs.Length <= 0)
            //    this.ToolStripMenuItemState.Visible = false;
            //else
            //    this.ToolStripMenuItemState.Visible = true;
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
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("0");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("3");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("5");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("9");
        }
        private void UpdatedgvMainState(string State)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                BLL.BLLBase bll = new BLL.BLLBase();
                string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                bll.ExecNonQuery("WCS.UpdateTaskStateByTaskNo", new DataParameter[] { new DataParameter("@State", State), new DataParameter("@TaskNo", TaskNo) });

                //堆垛机完成执行
                if (State == "7")
                {
                    DataParameter[] param = new DataParameter[] { new DataParameter("@TaskNo", TaskNo) };
                    bll.ExecNonQueryTran("WCS.Sp_TaskProcess", param);
                }
                BindData();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("4");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("6");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UpdatedgvMainState("7");
        }

        private void frmInventor_Activated(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void toolStripButton_Query_Click(object sender, EventArgs e)
        {
            frmTaskDialog f = new frmTaskDialog("14");
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BindData(f.filter);
            }
        }
        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                this.dgvMain.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvMain.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvMain.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

        }
    }
}
