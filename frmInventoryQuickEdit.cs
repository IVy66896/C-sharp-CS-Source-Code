
using POS_Client;
using POS_Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class frmInventoryQuickEdit : MasterThinForm
{
	private int pageSize = 10;

	private int pageNow = 1;

	public int pageTotal = 1;

	public DataTable dt;

	private IContainer components;

	private Button btn_enter;

	private Button btn_goManage;

	private Button btn_reset;

	private Panel panel5;

	private Label label10;

	private Panel panel3;

	private Label label6;

	private TextBox tb_GDSNO;

	private TextBox tb_GDName;

	private TableLayoutPanel tableLayoutPanel1;

	private Label label1;

	private Button btn_pageLeft;

	private Button btn_pageRight;

	private Label l_pageInfo;

	private Label label2;

	private DataGridView dataGridView1;

	private DataGridViewTextBoxColumn GDV_Index;

	private DataGridViewTextBoxColumn DGV_GDSNAME;

	private DataGridViewTextBoxColumn DGV_Remain;

	private DataGridViewTextBoxColumn DVG_Status;

	private DataGridViewTextBoxColumn hiddenGDSNO;

	private Panel panel1;

	private Label label3;

	private Panel panel2;

	private Label label4;

	private Panel panel4;

	private CheckBox checkBox4;

	private CheckBox checkBox3;

	private CheckBox checkBox2;

	private CheckBox checkBox1;

	private Panel panel6;

	private CheckBox checkBox7;

	private CheckBox checkBox6;

	private CheckBox checkBox5;

	public frmInventoryQuickEdit()
		: base("庫存快速管理")
	{
		InitializeComponent();
		string sql = "select a.GDSNO,a.GDName,a.status,a.spec, a.capacity,a.formCode,a.CName,a.contents,a.brandName, a.inventory  from hypos_GOODSLST a order by a.UpdateDate DESC LIMIT 0,10";
		dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		pageTotal = (int)Math.Ceiling((double)dt.Rows.Count / (double)pageSize);
		changePage(1);
	}

	private string setCommodityName(DataRow row)
	{
		string text = row["GDSNO"].ToString() + " " + row["GDName"].ToString();
		string text2 = row["formCode"].ToString();
		string text3 = row["CName"].ToString();
		string text4 = row["contents"].ToString();
		string text5 = row["brandName"].ToString();
		string[] array = new string[2]
		{
			text3,
			text2
		};
		string[] array2 = new string[2]
		{
			text4,
			text5
		};
		if (!string.IsNullOrEmpty(text2) || !string.IsNullOrEmpty(text3) || !string.IsNullOrEmpty(text4) || !string.IsNullOrEmpty(text5))
		{
			text += "[";
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					text = text + array[i] + "-";
				}
			}
			if (text.LastIndexOf("-") > 0)
			{
				text = text.Substring(0, text.LastIndexOf("-")) + " . ";
			}
			for (int j = 0; j < array2.Length; j++)
			{
				if (!string.IsNullOrEmpty(array2[j]))
				{
					text = text + array2[j] + "-";
				}
			}
			if (text.LastIndexOf("-") > 0)
			{
				text = text.Substring(0, text.LastIndexOf("-")) + "]";
			}
		}
		return text + string.Format(" ({0} {1})", row["spec"].ToString(), row["capacity"].ToString());
	}

	public void changePage(int page)
	{
		dataGridView1.Rows.Clear();
		int num = 0;
		pageNow = page;
		for (int i = (pageNow - 1) * pageSize; i < pageNow * pageSize; i++)
		{
			if (i < dt.Rows.Count)
			{
				string text = "";
				switch (dt.Rows[i]["Status"].ToString())
				{
				case "N":
					text = "未使用";
					break;
				case "U":
					text = "使用中";
					break;
				case "S":
					text = "停用";
					break;
				case "D":
					text = "禁用";
					break;
				}
				string.Format("{0} {1} ({2} {3})", dt.Rows[i]["GDSNO"].ToString(), dt.Rows[i]["GDName"].ToString(), dt.Rows[i]["spec"].ToString(), dt.Rows[i]["capacity"].ToString());
				int num2 = (!string.IsNullOrEmpty(dt.Rows[i]["inventory"].ToString())) ? int.Parse(dt.Rows[i]["inventory"].ToString()) : 0;
				dataGridView1.Rows.Insert(num, 0, setCommodityName(dt.Rows[i]), num2, text, dt.Rows[i]["GDSNO"].ToString());
				num++;
			}
		}
		dataGridView1.CurrentCell = null;
		dataGridView1.ClearSelection();
		l_pageInfo.Text = $"共{dt.Rows.Count}筆．{Math.Ceiling((double)dt.Rows.Count / (double)pageSize)}頁｜目前在第{pageNow}頁";
	}

	private void tb_GDSNO_Enter(object sender, EventArgs e)
	{
		if (tb_GDSNO.Text == "請刷商品條碼或輸入條碼")
		{
			tb_GDSNO.Text = "";
		}
	}

	private void tb_GDSNO_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_GDSNO.Text))
		{
			tb_GDSNO.Text = "請刷商品條碼或輸入條碼";
		}
	}

	private void tb_GDName_Enter(object sender, EventArgs e)
	{
		if (tb_GDName.Text == "請輸入商品名稱關鍵字")
		{
			tb_GDName.Text = "";
		}
	}

	private void tb_GDName_Leave(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(tb_GDName.Text))
		{
			tb_GDName.Text = "請輸入商品名稱關鍵字";
		}
	}

	private void btn_reset_Click(object sender, EventArgs e)
	{
		checkBox1.Checked = false;
		checkBox2.Checked = false;
		checkBox3.Checked = false;
		checkBox4.Checked = false;
		checkBox5.Checked = false;
		checkBox6.Checked = false;
		checkBox7.Checked = false;
		tb_GDName.Text = "請輸入商品名稱關鍵字";
		tb_GDSNO.Text = "";
		tb_GDSNO.Focus();
	}

	private void btn_pageRight_Click(object sender, EventArgs e)
	{
		if (pageNow < pageTotal)
		{
			changePage(pageNow + 1);
		}
	}

	private void btn_pageLeft_Click(object sender, EventArgs e)
	{
		if (pageNow > 1)
		{
			changePage(pageNow - 1);
		}
	}

	private void btn_enter_Click(object sender, EventArgs e)
	{
		btn_pageLeft.Visible = true;
		btn_pageRight.Visible = true;
		l_pageInfo.Visible = true;
		label1.Text = "商品搜尋結果";
		int num = 0;
		string str = "";
		List<string> list = new List<string>();
		string text = "select a.GDSNO,a.GDName,a.status,a.spec, a.capacity,a.formCode,a.CName,a.contents,a.brandName, a.inventory  from hypos_GOODSLST a where 1 = 1 ";
		if (tb_GDSNO.Text.Trim() != "請刷商品條碼或輸入條碼" && tb_GDSNO.Text.Trim() != "")
		{
			text = text + " AND a.GDSNO like {" + num + "}";
			list.Add("%" + tb_GDSNO.Text.Trim() + "%");
			num++;
		}
		if (tb_GDName.Text.Trim() != "請輸入商品名稱關鍵字" && tb_GDName.Text.Trim() != "")
		{
			text = text + " AND a.GDName like {" + num + "}";
			list.Add("%" + tb_GDName.Text.Trim() + "%");
			num++;
		}
		if (checkBox1.Checked | checkBox2.Checked | checkBox3.Checked | checkBox4.Checked)
		{
			str += " AND a.CLA1NO IN ( ";
			if (checkBox1.Checked)
			{
				str += " '0302' ";
			}
			if (checkBox2.Checked)
			{
				if (checkBox1.Checked)
				{
					str += " , ";
				}
				str += " '0303' ";
			}
			if (checkBox3.Checked)
			{
				if (checkBox1.Checked || checkBox2.Checked)
				{
					str += " , ";
				}
				str += " '0305' ";
			}
			if (checkBox4.Checked)
			{
				if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked)
				{
					str += " , ";
				}
				str += " '0308' ";
			}
			str += " ) ";
		}
		if (checkBox5.Checked | checkBox6.Checked | checkBox7.Checked)
		{
			str += " AND a.status IN ( ";
			if (checkBox5.Checked)
			{
				str += " 'U' ";
			}
			if (checkBox6.Checked)
			{
				if (checkBox5.Checked)
				{
					str += " , ";
				}
				str += " 'N' ";
			}
			if (checkBox7.Checked)
			{
				if (checkBox5.Checked || checkBox6.Checked)
				{
					str += " , ";
				}
				str += " 'S' ";
			}
			str += " ) ";
		}
		else
		{
			str += " AND a.status IN ('U','S','N') ";
		}
		text += str;
		text += " order by a.UpdateDate DESC ";
		dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, text.ToString(), list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
		pageTotal = (int)Math.Ceiling((double)dt.Rows.Count / (double)pageSize);
		changePage(1);
	}

	private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
	{
		dataGridView1.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1 + (pageNow - 1) * pageSize).ToString();
	}

	private void btn_goManage_Click(object sender, EventArgs e)
	{
		switchForm(new frmInventoryMangement());
	}

	private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0 && e.ColumnIndex == 1)
		{
			using (dialogInventoryAdjustment dialogInventoryAdjustment = new dialogInventoryAdjustment(dataGridView1[4, e.RowIndex].Value.ToString(), dataGridView1[2, e.RowIndex].Value.ToString()))
			{
				dialogInventoryAdjustment.ShowDialog();
				dataGridView1[2, e.RowIndex].Value = dialogInventoryAdjustment.l_inventoryCountTotal.Text;
			}
		}
	}

	private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.RowIndex >= 0)
		{
			dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(252, 252, 237);
		}
	}

	private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0)
		{
			dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
		}
	}

	private void dataGridView1_SelectionChanged(object sender, EventArgs e)
	{
		dataGridView1.ClearSelection();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.frmInventoryQuickEdit));
		btn_enter = new System.Windows.Forms.Button();
		btn_goManage = new System.Windows.Forms.Button();
		btn_reset = new System.Windows.Forms.Button();
		panel5 = new System.Windows.Forms.Panel();
		label10 = new System.Windows.Forms.Label();
		panel3 = new System.Windows.Forms.Panel();
		label6 = new System.Windows.Forms.Label();
		tb_GDSNO = new System.Windows.Forms.TextBox();
		tb_GDName = new System.Windows.Forms.TextBox();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		panel2 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		panel1 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		checkBox4 = new System.Windows.Forms.CheckBox();
		checkBox3 = new System.Windows.Forms.CheckBox();
		checkBox2 = new System.Windows.Forms.CheckBox();
		checkBox1 = new System.Windows.Forms.CheckBox();
		panel6 = new System.Windows.Forms.Panel();
		checkBox7 = new System.Windows.Forms.CheckBox();
		checkBox6 = new System.Windows.Forms.CheckBox();
		checkBox5 = new System.Windows.Forms.CheckBox();
		label1 = new System.Windows.Forms.Label();
		btn_pageLeft = new System.Windows.Forms.Button();
		btn_pageRight = new System.Windows.Forms.Button();
		l_pageInfo = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		dataGridView1 = new System.Windows.Forms.DataGridView();
		GDV_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
		DGV_GDSNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
		DGV_Remain = new System.Windows.Forms.DataGridViewTextBoxColumn();
		DVG_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
		hiddenGDSNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).BeginInit();
		panel5.SuspendLayout();
		panel3.SuspendLayout();
		tableLayoutPanel1.SuspendLayout();
		panel2.SuspendLayout();
		panel1.SuspendLayout();
		panel4.SuspendLayout();
		panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
		SuspendLayout();
		btn_enter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_enter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_enter.ForeColor = System.Drawing.Color.White;
		btn_enter.Location = new System.Drawing.Point(375, 186);
		btn_enter.Name = "btn_enter";
		btn_enter.Size = new System.Drawing.Size(106, 35);
		btn_enter.TabIndex = 1;
		btn_enter.TabStop = false;
		btn_enter.Text = "查詢";
		btn_enter.UseVisualStyleBackColor = false;
		btn_enter.Click += new System.EventHandler(btn_enter_Click);
		btn_goManage.BackColor = System.Drawing.Color.FromArgb(36, 168, 208);
		btn_goManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_goManage.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_goManage.ForeColor = System.Drawing.Color.White;
		btn_goManage.Location = new System.Drawing.Point(833, 38);
		btn_goManage.Name = "btn_goManage";
		btn_goManage.Size = new System.Drawing.Size(90, 37);
		btn_goManage.TabIndex = 44;
		btn_goManage.TabStop = false;
		btn_goManage.Text = "進貨管理";
		btn_goManage.UseVisualStyleBackColor = false;
		btn_goManage.Visible = false;
		btn_goManage.Click += new System.EventHandler(btn_goManage_Click);
		btn_reset.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
		btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_reset.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_reset.ForeColor = System.Drawing.Color.White;
		btn_reset.Location = new System.Drawing.Point(507, 186);
		btn_reset.Name = "btn_reset";
		btn_reset.Size = new System.Drawing.Size(106, 35);
		btn_reset.TabIndex = 42;
		btn_reset.TabStop = false;
		btn_reset.Text = "重設";
		btn_reset.UseVisualStyleBackColor = false;
		btn_reset.Click += new System.EventHandler(btn_reset_Click);
		panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel5.Controls.Add(label10);
		panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		panel5.Location = new System.Drawing.Point(433, 50);
		panel5.Margin = new System.Windows.Forms.Padding(0);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(129, 48);
		panel5.TabIndex = 23;
		label10.AutoSize = true;
		label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.Color.White;
		label10.Location = new System.Drawing.Point(30, 13);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(74, 21);
		label10.TabIndex = 0;
		label10.Text = "商品狀態";
		panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel3.Controls.Add(label6);
		panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		panel3.Location = new System.Drawing.Point(1, 1);
		panel3.Margin = new System.Windows.Forms.Padding(0);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(129, 48);
		panel3.TabIndex = 21;
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.Color.Transparent;
		label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.Color.White;
		label6.Location = new System.Drawing.Point(30, 13);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(74, 21);
		label6.TabIndex = 0;
		label6.Text = "商品編號";
		tb_GDSNO.BackColor = System.Drawing.Color.White;
		tb_GDSNO.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_GDSNO.ForeColor = System.Drawing.Color.DarkGray;
		tb_GDSNO.Location = new System.Drawing.Point(141, 11);
		tb_GDSNO.Margin = new System.Windows.Forms.Padding(10);
		tb_GDSNO.MaxLength = 20;
		tb_GDSNO.Name = "tb_GDSNO";
		tb_GDSNO.Size = new System.Drawing.Size(281, 29);
		tb_GDSNO.TabIndex = 1;
		tb_GDSNO.Text = "請刷商品條碼或輸入條碼";
		tb_GDSNO.Enter += new System.EventHandler(tb_GDSNO_Enter);
		tb_GDSNO.Leave += new System.EventHandler(tb_GDSNO_Leave);
		tb_GDName.BackColor = System.Drawing.Color.White;
		tb_GDName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_GDName.ForeColor = System.Drawing.Color.DarkGray;
		tb_GDName.Location = new System.Drawing.Point(573, 11);
		tb_GDName.Margin = new System.Windows.Forms.Padding(10);
		tb_GDName.Name = "tb_GDName";
		tb_GDName.Size = new System.Drawing.Size(281, 29);
		tb_GDName.TabIndex = 2;
		tb_GDName.Text = "請輸入商品名稱關鍵字";
		tb_GDName.Enter += new System.EventHandler(tb_GDName_Enter);
		tb_GDName.Leave += new System.EventHandler(tb_GDName_Leave);
		tableLayoutPanel1.BackColor = System.Drawing.Color.White;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35f));
		tableLayoutPanel1.Controls.Add(panel2, 0, 1);
		tableLayoutPanel1.Controls.Add(panel1, 2, 0);
		tableLayoutPanel1.Controls.Add(tb_GDName, 3, 0);
		tableLayoutPanel1.Controls.Add(panel3, 0, 0);
		tableLayoutPanel1.Controls.Add(panel5, 2, 1);
		tableLayoutPanel1.Controls.Add(tb_GDSNO, 1, 0);
		tableLayoutPanel1.Controls.Add(panel4, 1, 1);
		tableLayoutPanel1.Controls.Add(panel6, 3, 1);
		tableLayoutPanel1.Location = new System.Drawing.Point(57, 80);
		tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 2;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		tableLayoutPanel1.Size = new System.Drawing.Size(866, 99);
		tableLayoutPanel1.TabIndex = 40;
		panel2.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel2.Controls.Add(label4);
		panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		panel2.Location = new System.Drawing.Point(1, 50);
		panel2.Margin = new System.Windows.Forms.Padding(0);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(129, 48);
		panel2.TabIndex = 25;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.Color.White;
		label4.Location = new System.Drawing.Point(30, 13);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 0;
		label4.Text = "商品類型";
		panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		panel1.Controls.Add(label3);
		panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		panel1.Location = new System.Drawing.Point(433, 1);
		panel1.Margin = new System.Windows.Forms.Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(129, 48);
		panel1.TabIndex = 24;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.Color.White;
		label3.Location = new System.Drawing.Point(34, 13);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(74, 21);
		label3.TabIndex = 0;
		label3.Text = "商品名稱";
		panel4.Controls.Add(checkBox4);
		panel4.Controls.Add(checkBox3);
		panel4.Controls.Add(checkBox2);
		panel4.Controls.Add(checkBox1);
		panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		panel4.Location = new System.Drawing.Point(134, 53);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(295, 42);
		panel4.TabIndex = 26;
		checkBox4.AutoSize = true;
		checkBox4.Checked = true;
		checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
		checkBox4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox4.Location = new System.Drawing.Point(213, 9);
		checkBox4.Name = "checkBox4";
		checkBox4.Size = new System.Drawing.Size(61, 25);
		checkBox4.TabIndex = 3;
		checkBox4.Text = "其他";
		checkBox4.UseVisualStyleBackColor = true;
		checkBox3.AutoSize = true;
		checkBox3.Checked = true;
		checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
		checkBox3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox3.Location = new System.Drawing.Point(145, 9);
		checkBox3.Name = "checkBox3";
		checkBox3.Size = new System.Drawing.Size(61, 25);
		checkBox3.TabIndex = 2;
		checkBox3.Text = "資材";
		checkBox3.UseVisualStyleBackColor = true;
		checkBox2.AutoSize = true;
		checkBox2.Checked = true;
		checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
		checkBox2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox2.Location = new System.Drawing.Point(76, 9);
		checkBox2.Name = "checkBox2";
		checkBox2.Size = new System.Drawing.Size(61, 25);
		checkBox2.TabIndex = 1;
		checkBox2.Text = "肥料";
		checkBox2.UseVisualStyleBackColor = true;
		checkBox1.AutoSize = true;
		checkBox1.Checked = true;
		checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
		checkBox1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox1.Location = new System.Drawing.Point(7, 9);
		checkBox1.Name = "checkBox1";
		checkBox1.Size = new System.Drawing.Size(61, 25);
		checkBox1.TabIndex = 0;
		checkBox1.Text = "農藥";
		checkBox1.UseVisualStyleBackColor = true;
		panel6.Controls.Add(checkBox7);
		panel6.Controls.Add(checkBox6);
		panel6.Controls.Add(checkBox5);
		panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		panel6.Location = new System.Drawing.Point(566, 53);
		panel6.Name = "panel6";
		panel6.Size = new System.Drawing.Size(296, 42);
		panel6.TabIndex = 27;
		checkBox7.AutoSize = true;
		checkBox7.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox7.Location = new System.Drawing.Point(189, 9);
		checkBox7.Name = "checkBox7";
		checkBox7.Size = new System.Drawing.Size(77, 25);
		checkBox7.TabIndex = 2;
		checkBox7.Text = "已停用";
		checkBox7.UseVisualStyleBackColor = true;
		checkBox6.AutoSize = true;
		checkBox6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox6.Location = new System.Drawing.Point(103, 9);
		checkBox6.Name = "checkBox6";
		checkBox6.Size = new System.Drawing.Size(77, 25);
		checkBox6.TabIndex = 1;
		checkBox6.Text = "未使用";
		checkBox6.UseVisualStyleBackColor = true;
		checkBox5.AutoSize = true;
		checkBox5.Checked = true;
		checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
		checkBox5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
		checkBox5.Location = new System.Drawing.Point(16, 9);
		checkBox5.Name = "checkBox5";
		checkBox5.Size = new System.Drawing.Size(77, 25);
		checkBox5.TabIndex = 0;
		checkBox5.Text = "使用中";
		checkBox5.UseVisualStyleBackColor = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.Image = POS_Client.Properties.Resources.oblique;
		label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label1.Location = new System.Drawing.Point(53, 226);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(152, 23);
		label1.TabIndex = 46;
		label1.Text = "最近編修商品";
		label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		btn_pageLeft.FlatAppearance.BorderSize = 0;
		btn_pageLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageLeft.Image = POS_Client.Properties.Resources.left;
		btn_pageLeft.Location = new System.Drawing.Point(0, 308);
		btn_pageLeft.Name = "btn_pageLeft";
		btn_pageLeft.Size = new System.Drawing.Size(48, 306);
		btn_pageLeft.TabIndex = 53;
		btn_pageLeft.UseVisualStyleBackColor = true;
		btn_pageLeft.Visible = false;
		btn_pageLeft.Click += new System.EventHandler(btn_pageLeft_Click);
		btn_pageRight.FlatAppearance.BorderSize = 0;
		btn_pageRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_pageRight.Image = POS_Client.Properties.Resources.right;
		btn_pageRight.Location = new System.Drawing.Point(933, 308);
		btn_pageRight.Name = "btn_pageRight";
		btn_pageRight.Size = new System.Drawing.Size(48, 306);
		btn_pageRight.TabIndex = 52;
		btn_pageRight.UseVisualStyleBackColor = true;
		btn_pageRight.Visible = false;
		btn_pageRight.Click += new System.EventHandler(btn_pageRight_Click);
		l_pageInfo.AutoSize = true;
		l_pageInfo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_pageInfo.Location = new System.Drawing.Point(382, 629);
		l_pageInfo.Name = "l_pageInfo";
		l_pageInfo.Size = new System.Drawing.Size(216, 20);
		l_pageInfo.TabIndex = 59;
		l_pageInfo.Text = "共{0}筆．{1}頁｜目前在第1頁\r\n";
		l_pageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		l_pageInfo.Visible = false;
		label2.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Image = POS_Client.Properties.Resources.oblique;
		label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label2.Location = new System.Drawing.Point(53, 45);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(109, 23);
		label2.TabIndex = 46;
		label2.Text = "庫存編修";
		label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		dataGridView1.AllowUserToAddRows = false;
		dataGridView1.AllowUserToDeleteRows = false;
		dataGridView1.AllowUserToResizeColumns = false;
		dataGridView1.AllowUserToResizeRows = false;
		dataGridView1.BackgroundColor = System.Drawing.Color.White;
		dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(102, 102, 102);
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dataGridView1.Columns.AddRange(GDV_Index, DGV_GDSNAME, DGV_Remain, DVG_Status, hiddenGDSNO);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
		dataGridView1.EnableHeadersVisualStyles = false;
		dataGridView1.Location = new System.Drawing.Point(57, 260);
		dataGridView1.Margin = new System.Windows.Forms.Padding(0);
		dataGridView1.Name = "dataGridView1";
		dataGridView1.ReadOnly = true;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		dataGridView1.RowHeadersVisible = false;
		dataGridView1.RowTemplate.Height = 32;
		dataGridView1.Size = new System.Drawing.Size(866, 361);
		dataGridView1.TabIndex = 71;
		dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellClick);
		dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridView1_CellMouseLeave);
		dataGridView1.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dataGridView1_CellMouseMove);
		dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(dataGridView1_RowPrePaint);
		dataGridView1.SelectionChanged += new System.EventHandler(dataGridView1_SelectionChanged);
		GDV_Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		dataGridViewCellStyle4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
		GDV_Index.DefaultCellStyle = dataGridViewCellStyle4;
		GDV_Index.HeaderText = "";
		GDV_Index.Name = "GDV_Index";
		GDV_Index.ReadOnly = true;
		GDV_Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		GDV_Index.Width = 14;
		DGV_GDSNAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue;
		dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Blue;
		DGV_GDSNAME.DefaultCellStyle = dataGridViewCellStyle5;
		DGV_GDSNAME.HeaderText = "商品名稱";
		DGV_GDSNAME.Name = "DGV_GDSNAME";
		DGV_GDSNAME.ReadOnly = true;
		DGV_GDSNAME.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		DGV_GDSNAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		DGV_Remain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
		dataGridViewCellStyle6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
		DGV_Remain.DefaultCellStyle = dataGridViewCellStyle6;
		DGV_Remain.FillWeight = 200f;
		DGV_Remain.HeaderText = "現品庫存";
		DGV_Remain.Name = "DGV_Remain";
		DGV_Remain.ReadOnly = true;
		DGV_Remain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		DGV_Remain.Width = 90;
		DVG_Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle7.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
		DVG_Status.DefaultCellStyle = dataGridViewCellStyle7;
		DVG_Status.FillWeight = 60f;
		DVG_Status.HeaderText = "狀態";
		DVG_Status.Name = "DVG_Status";
		DVG_Status.ReadOnly = true;
		DVG_Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		DVG_Status.Width = 58;
		hiddenGDSNO.HeaderText = "";
		hiddenGDSNO.Name = "hiddenGDSNO";
		hiddenGDSNO.ReadOnly = true;
		hiddenGDSNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		hiddenGDSNO.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(981, 661);
		base.Controls.Add(l_pageInfo);
		base.Controls.Add(btn_pageLeft);
		base.Controls.Add(btn_pageRight);
		base.Controls.Add(label2);
		base.Controls.Add(label1);
		base.Controls.Add(btn_reset);
		base.Controls.Add(btn_enter);
		base.Controls.Add(btn_goManage);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(dataGridView1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmInventoryQuickEdit";
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		Text = "會員選擇";
		base.Controls.SetChildIndex(dataGridView1, 0);
		base.Controls.SetChildIndex(tableLayoutPanel1, 0);
		base.Controls.SetChildIndex(btn_goManage, 0);
		base.Controls.SetChildIndex(btn_enter, 0);
		base.Controls.SetChildIndex(btn_reset, 0);
		base.Controls.SetChildIndex(label1, 0);
		base.Controls.SetChildIndex(label2, 0);
		base.Controls.SetChildIndex(btn_pageRight, 0);
		base.Controls.SetChildIndex(btn_pageLeft, 0);
		base.Controls.SetChildIndex(l_pageInfo, 0);
		base.Controls.SetChildIndex(pb_virtualKeyBoard, 0);
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).EndInit();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		tableLayoutPanel1.ResumeLayout(false);
		tableLayoutPanel1.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		panel6.ResumeLayout(false);
		panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
