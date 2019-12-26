using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmListPrintCodeForSelect : Form
{
	private int _SortingColIndex = -1;

	private string _SqlTimeCond = string.Empty;

	private DataTable _DtData;

	public List<int> ResumeIDSelected;

	private IContainer components;

	private DataGridView dgvPrint;

	private BindingSource bs;

	private BindingNavigator bn;

	private ToolStripLabel bindingNavigatorCountItem;

	private ToolStripButton bindingNavigatorMoveFirstItem;

	private ToolStripButton bindingNavigatorMovePreviousItem;

	private ToolStripSeparator bindingNavigatorSeparator;

	private ToolStripSeparator bindingNavigatorSeparator1;

	private ToolStripButton bindingNavigatorMoveNextItem;

	private ToolStripSeparator bindingNavigatorSeparator2;

	private ToolStripTextBox bindingNavigatorPositionItem;

	private ToolStripButton bindingNavigatorMoveLastItem;

	private ToolStripButton btnConfirm;

	public frmListPrintCodeForSelect(DataTable dtData)
	{
		_DtData = dtData;
		ResumeIDSelected = new List<int>();
		InitializeComponent();
	}

	private void InitdgvPrint()
	{
		dgvPrint.DataSource = _DtData;
		DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		dataGridViewCheckBoxColumn.HeaderText = "選取";
		dataGridViewCheckBoxColumn.Name = "選取";
		dataGridViewCheckBoxColumn.ReadOnly = false;
		dataGridViewCheckBoxColumn.Width = 43;
		dgvPrint.Columns.Add(dataGridViewCheckBoxColumn);
		dgvPrint.Columns["選取"].DisplayIndex = 0;
		dgvPrint.Columns[0].HeaderText = "品項";
		dgvPrint.Columns[1].HeaderText = "包裝日期";
		dgvPrint.Columns[2].HeaderText = "上傳日期";
		dgvPrint.Columns[3].HeaderText = "農民";
		dgvPrint.Columns[4].HeaderText = "收貨單位";
		dgvPrint.Columns[5].HeaderText = "追溯號碼";
		dgvPrint.Columns[6].Visible = false;
		dgvPrint.Columns[7].Visible = false;
		dgvPrint.Columns[8].Visible = false;
		dgvPrint.Columns[9].HeaderText = "EAN";
		dgvPrint.Columns[10].HeaderText = "規格";
		dgvPrint.Columns[11].HeaderText = "原栽培編碼";
		foreach (DataGridViewColumn column in dgvPrint.Columns)
		{
			column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			if (column.HeaderText == "申請數量" || column.HeaderText == "可列印數" || column.HeaderText == "已列印數")
			{
				column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
			}
			else
			{
				column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			}
		}
		foreach (DataGridViewRow item in (IEnumerable)dgvPrint.Rows)
		{
			item.Cells["選取"].Value = true;
		}
		DoResizeDGV();
		bs.DataSource = dgvPrint.DataSource;
		bn.BindingSource = bs;
		dgvPrint.EnableHeadersVisualStyles = true;
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
		dgvPrint.ReadOnly = true;
		dgvPrint.AllowUserToDeleteRows = false;
	}

	private void dgvPrint_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (!IsANonHeaderLinkCell(e) && IsANonHeaderCheckBoxCell(e) && dgvPrint.Columns[e.ColumnIndex].HeaderText == "選取")
		{
			dgvPrint.Rows[e.RowIndex].Cells["選取"].Value = !Convert.ToBoolean(dgvPrint.Rows[e.RowIndex].Cells["選取"].Value);
		}
	}

	private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorPositionItem_Click(object sender, EventArgs e)
	{
	}

	private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int num = Convert.ToInt32(bindingNavigatorPositionItem.Text) - 1;
			if (dgvPrint.Rows.Count > num && num > -1)
			{
				dgvPrint.CurrentCell = dgvPrint.Rows[num].Cells[1];
			}
		}
		catch
		{
		}
	}

	private void dgvPrint_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
		{
			bs.Position = e.RowIndex;
			bindingNavigatorPositionItem.Text = Convert.ToString(e.RowIndex + 1);
			dgvPrint.CurrentCell = dgvPrint.Rows[e.RowIndex].Cells[e.ColumnIndex];
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		Program.ShowHistory = !Program.ShowHistory;
		dgvPrint.DataSource = _DtData;
		bs.DataSource = dgvPrint.DataSource;
		bn.BindingSource = bs;
	}

	private void DoResizeDGV()
	{
		if (base.Size.Width > 31 && base.Size.Height > 76)
		{
			double num = (double)(base.Size.Width - 31) / (double)dgvPrint.Width;
			dgvPrint.Width = base.Size.Width - 31;
			dgvPrint.Height = base.Size.Height - 76;
			for (int i = 0; i < dgvPrint.Columns.Count; i++)
			{
				dgvPrint.Columns[i].Width = Convert.ToInt32((double)dgvPrint.Columns[i].Width * num);
			}
			dgvPrint.Refresh();
		}
	}

	private void dgvPrint_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		_SortingColIndex = ((dgvPrint.SortedColumn == null) ? (-1) : dgvPrint.SortedColumn.Index);
	}

	private void frmListPrintCodeForSelect_Load(object sender, EventArgs e)
	{
		InitdgvPrint();
	}

	private void frmListPrintCodeForSelect_MaximumSizeChanged(object sender, EventArgs e)
	{
	}

	private void frmListPrintCodeForSelect_SizeChanged(object sender, EventArgs e)
	{
		DoResizeDGV();
	}

	private bool IsANonHeaderLinkCell(DataGridViewCellEventArgs cellEvent)
	{
		if (dgvPrint.Columns[cellEvent.ColumnIndex] is DataGridViewLinkColumn && cellEvent.RowIndex != -1)
		{
			return true;
		}
		return false;
	}

	private bool IsANonHeaderCheckBoxCell(DataGridViewCellEventArgs cellEvent)
	{
		if (dgvPrint.Columns[cellEvent.ColumnIndex] is DataGridViewCheckBoxColumn && cellEvent.RowIndex != -1)
		{
			return true;
		}
		return false;
	}

	private void btnConfirm_Click(object sender, EventArgs e)
	{
		ResumeIDSelected = new List<int>();
		foreach (DataGridViewRow item in (IEnumerable)dgvPrint.Rows)
		{
			if (Convert.ToBoolean(item.Cells["選取"].Value))
			{
				ResumeIDSelected.Add(Convert.ToInt32(item.Cells["ID"].Value));
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmListPrintCodeForSelect));
		dgvPrint = new System.Windows.Forms.DataGridView();
		bn = new System.Windows.Forms.BindingNavigator(components);
		bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
		bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
		bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
		bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		bs = new System.Windows.Forms.BindingSource(components);
		btnConfirm = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)dgvPrint).BeginInit();
		((System.ComponentModel.ISupportInitialize)bn).BeginInit();
		bn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)bs).BeginInit();
		SuspendLayout();
		dgvPrint.AllowUserToAddRows = false;
		dgvPrint.AllowUserToDeleteRows = false;
		dgvPrint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dgvPrint.Location = new System.Drawing.Point(12, 28);
		dgvPrint.Margin = new System.Windows.Forms.Padding(0);
		dgvPrint.Name = "dgvPrint";
		dgvPrint.ReadOnly = true;
		dgvPrint.RowTemplate.Height = 24;
		dgvPrint.Size = new System.Drawing.Size(988, 309);
		dgvPrint.TabIndex = 0;
		dgvPrint.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvPrint_ColumnHeaderMouseClick);
		dgvPrint.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvPrint_CellClick);
		dgvPrint.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvPrint_CellContentClick);
		bn.AddNewItem = null;
		bn.CountItem = bindingNavigatorCountItem;
		bn.DeleteItem = null;
		bn.Items.AddRange(new System.Windows.Forms.ToolStripItem[10]
		{
			bindingNavigatorMoveFirstItem,
			bindingNavigatorMovePreviousItem,
			bindingNavigatorSeparator,
			bindingNavigatorPositionItem,
			bindingNavigatorCountItem,
			bindingNavigatorSeparator1,
			bindingNavigatorMoveNextItem,
			bindingNavigatorMoveLastItem,
			bindingNavigatorSeparator2,
			btnConfirm
		});
		bn.Location = new System.Drawing.Point(0, 0);
		bn.MoveFirstItem = bindingNavigatorMoveFirstItem;
		bn.MoveLastItem = bindingNavigatorMoveLastItem;
		bn.MoveNextItem = bindingNavigatorMoveNextItem;
		bn.MovePreviousItem = bindingNavigatorMovePreviousItem;
		bn.Name = "bn";
		bn.PositionItem = null;
		bn.Size = new System.Drawing.Size(1009, 25);
		bn.TabIndex = 1;
		bn.Text = "bindingNavigator1";
		bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
		bindingNavigatorCountItem.Size = new System.Drawing.Size(24, 22);
		bindingNavigatorCountItem.Text = "/{0}";
		bindingNavigatorCountItem.ToolTipText = "項目總數";
		bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveFirstItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveFirstItem.Image");
		bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
		bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveFirstItem.Text = "移到最前面";
		bindingNavigatorMoveFirstItem.Click += new System.EventHandler(bindingNavigatorMoveFirstItem_Click);
		bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMovePreviousItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMovePreviousItem.Image");
		bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
		bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMovePreviousItem.Text = "移到上一個";
		bindingNavigatorMovePreviousItem.Click += new System.EventHandler(bindingNavigatorMovePreviousItem_Click);
		bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
		bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
		bindingNavigatorPositionItem.AccessibleName = "位置";
		bindingNavigatorPositionItem.AutoSize = false;
		bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
		bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 22);
		bindingNavigatorPositionItem.Text = "0";
		bindingNavigatorPositionItem.ToolTipText = "目前的位置";
		bindingNavigatorPositionItem.TextChanged += new System.EventHandler(bindingNavigatorPositionItem_TextChanged);
		bindingNavigatorPositionItem.Click += new System.EventHandler(bindingNavigatorPositionItem_Click);
		bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
		bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
		bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveNextItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveNextItem.Image");
		bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
		bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveNextItem.Text = "移到下一個";
		bindingNavigatorMoveNextItem.Click += new System.EventHandler(bindingNavigatorMoveNextItem_Click);
		bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveLastItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveLastItem.Image");
		bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
		bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveLastItem.Text = "移到最後面";
		bindingNavigatorMoveLastItem.Click += new System.EventHandler(bindingNavigatorMoveLastItem_Click);
		bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
		bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
		btnConfirm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		btnConfirm.Image = (System.Drawing.Image)resources.GetObject("btnConfirm.Image");
		btnConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
		btnConfirm.Name = "btnConfirm";
		btnConfirm.Size = new System.Drawing.Size(57, 22);
		btnConfirm.Text = "確定選取";
		btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1009, 349);
		base.Controls.Add(bn);
		base.Controls.Add(dgvPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmListPrintCodeForSelect";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "請選擇您想下載的標籤";
		base.Load += new System.EventHandler(frmListPrintCodeForSelect_Load);
		base.SizeChanged += new System.EventHandler(frmListPrintCodeForSelect_SizeChanged);
		base.MaximumSizeChanged += new System.EventHandler(frmListPrintCodeForSelect_MaximumSizeChanged);
		((System.ComponentModel.ISupportInitialize)dgvPrint).EndInit();
		((System.ComponentModel.ISupportInitialize)bn).EndInit();
		bn.ResumeLayout(false);
		bn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)bs).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
