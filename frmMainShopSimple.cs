
using POS_Client;
using POS_Client.Properties;
using POS_Client.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class frmMainShopSimple : MasterThinForm
{
	public class CustomColumn : DataGridViewColumn
	{
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null && !value.GetType().IsAssignableFrom(typeof(CustomeCell)))
				{
					throw new InvalidCastException("It should be a custom Cell");
				}
				base.CellTemplate = value;
			}
		}

		public CustomColumn()
			: base(new CustomeCell())
		{
		}
	}

	public class CustomeCell : DataGridViewCell
	{
		public override Type ValueType => typeof(CommodityInfo);

		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			CommodityInfo commodityInfo = (CommodityInfo)value;
			Bitmap bitmap = new Bitmap(cellBounds.Width, cellBounds.Height);
			commodityInfo.DrawToBitmap(bitmap, new Rectangle(0, 0, commodityInfo.Width, commodityInfo.Height));
			graphics.DrawImage(bitmap, cellBounds.Location);
		}
	}

	public int columnOfFocus;

	private int totalmoney;

	private string HseqNo = "";

	public string vipNo = "";

	private List<string> barcodelist = new List<string>();

	private bool same;

	public CommodityInfo uc;

	private string tempKeyCode = "";

	private IContainer components;

	private TextBox textBox1;

	private TextBox num;

	private Label l_sysTime;

	private DataGridView infolist;

	private Button addone;

	private Button subone;

	private Button numone;

	private Button pressEnter;

	private Panel panel1;

	private Button backspace;

	private Button pre;

	private Label totalspending;

	private Label totalpriceDiscount;

	private Label totalpriceDiscountView;

	private Label totalprice;

	private Label totalpriceview;

	private Button oneremove;

	private Button next;

	private Button numzero;

	private Button numtwo;

	private Button numthree;

	private Button numfour;

	private Button numsix;

	private Button numfive;

	private Button numseven;

	private Button numeight;

	private Button numnine;

	private Button removeall;

	private Button btn_MedDescription;

	private TextBox alertMsg;

	private Button commoditySearch;

	private Button cureGuide;

	private Button Checkout;

	private FlowLayoutPanel flp_chooseMember;

	private Label label2;

	private Label label1;

	private Button button1;

	private Label label3;

	private PictureBox pictureBox1;

	private DataGridViewTextBoxColumn Column1;

	private CustomColumn commodity;

	private DataGridViewTextBoxColumn quantity;

	private DataGridViewTextBoxColumn barcode;

	private DataGridViewTextBoxColumn cropId;

	private DataGridViewTextBoxColumn pestId;

	public frmMainShopSimple(string vipNo)
		: base("銷售作業")
	{
		init();
		this.vipNo = vipNo;
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "hcr.*,ara.area as ae,adr.city as ac", "hypos_CUST_RTL as hcr,ADDRAREA as ara,ADDRCITY as adr", "hcr.VipNo = {0} and adr.cityno = hcr.City and ara.zipcode = hcr.Area  ", "", null, new string[1]
		{
			vipNo
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			label2.Text = "會員:" + dataTable.Rows[0]["Name"].ToString();
			label2.Text = label2.Text + "\r\n電話:" + dataTable.Rows[0]["Telphone"].ToString();
			label2.Text = label2.Text + "\r\n地址:" + dataTable.Rows[0]["ac"].ToString() + dataTable.Rows[0]["ae"].ToString();
			label2.Font = new Font("微軟正黑體", 14f, FontStyle.Bold, GraphicsUnit.Point, 136);
		}
		else
		{
			AutoClosingMessageBox.Show("會員號碼錯誤!");
			Close();
		}
	}

	public frmMainShopSimple()
		: base("銷售作業")
	{
		init();
	}

	private void init()
	{
		pb_virtualKeyBoard.Visible = false;
		DateTime now = DateTime.Now;
		string text = now.ToString("yyyyMMdd");
		string text2 = now.ToString("yyyy-MM-dd");
		string[] strWhereParameterArray = new string[1]
		{
			text2
		};
		string strWhereClause = "sellTime like '%" + text2 + "%'";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_main_sell", strWhereClause, "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			string text3 = (dataTable.Rows.Count + 1).ToString();
			if (text3.Length == 1)
			{
				HseqNo = Program.LincenseCode + Program.SiteNo + text + "000" + text3;
			}
			else if (text3.Length == 2)
			{
				HseqNo = Program.LincenseCode + Program.SiteNo + text + "00" + text3;
			}
			else if (text3.Length == 3)
			{
				HseqNo = Program.LincenseCode + Program.SiteNo + text + "0" + text3;
			}
			else
			{
				HseqNo = Program.LincenseCode + Program.SiteNo + text + text3;
			}
		}
		else
		{
			HseqNo = Program.LincenseCode + Program.SiteNo + text + "0001";
		}
		setMasterFormName("銷售作業 | 單號: " + HseqNo);
		InitializeComponent();
		if (Program.goodsTemp.Count > 0)
		{
			foreach (GoodObject item in Program.goodsTemp)
			{
				infolist.Rows.Add(item._index, item._GDSName, item._number, item._barcode, item._cropId, item._pestId);
				foreach (string item2 in barcodelist)
				{
					if (item2.Equals(textBox1.Text))
					{
						same = true;
					}
				}
				infolist.CurrentCell = infolist.Rows[item._index - 1].Cells[0];
				if (infolist.CurrentCell != null)
				{
					infolist.ClearSelection();
					(infolist.CurrentRow.Cells[1].Value as CommodityInfo).BackColor = Color.White;
					infolist.CurrentRow.Selected = false;
					infolist.Refresh();
				}
				if (!same)
				{
					barcodelist.Add(textBox1.Text);
				}
				same = false;
			}
		}
		computetotalmoney("", select: true);
		textBox1.Select();
	}

	private void Form1_KeyDown(object sender, KeyEventArgs e)
	{
		if (columnOfFocus == 3)
		{
			if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
			{
				textBox1.Text = textBox1.Text.Trim() + "0";
			}
			else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
			{
				textBox1.Text = textBox1.Text.Trim() + "1";
			}
			else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
			{
				textBox1.Text = textBox1.Text.Trim() + "2";
			}
			else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
			{
				textBox1.Text = textBox1.Text.Trim() + "3";
			}
			else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
			{
				textBox1.Text = textBox1.Text.Trim() + "4";
			}
			else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
			{
				textBox1.Text = textBox1.Text.Trim() + "5";
			}
			else if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
			{
				textBox1.Text = textBox1.Text.Trim() + "6";
			}
			else if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
			{
				textBox1.Text = textBox1.Text.Trim() + "7";
			}
			else if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
			{
				textBox1.Text = textBox1.Text.Trim() + "8";
			}
			else if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
			{
				textBox1.Text = textBox1.Text.Trim() + "9";
			}
			else if (e.KeyCode == Keys.Back && textBox1.Text.Length > 0)
			{
				textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
			}
		}
		if (e.KeyCode == Keys.Add)
		{
			if (infolist.Rows.Count > 0 && infolist.CurrentRow.Selected)
			{
				int num = int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) + 1;
				infolist.CurrentRow.Cells[2].Value = num.ToString();
				computetotalmoney("", select: false);
				alertMessage("數量加1");
				setfocus();
			}
		}
		else if (e.KeyCode == Keys.Subtract)
		{
			if (infolist.Rows.Count > 0 && infolist.CurrentRow.Selected && int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) > 0)
			{
				int num2 = int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) - 1;
				if (num2 > 0)
				{
					infolist.CurrentRow.Cells[2].Value = num2.ToString();
				}
				else
				{
					infolist.Rows.RemoveAt(infolist.CurrentRow.Index);
					barcodelist.Clear();
					for (int i = 0; i < infolist.Rows.Count; i++)
					{
						foreach (string item in barcodelist)
						{
							if (item.Equals(infolist.Rows[i].Cells[3].Value.ToString()))
							{
								same = true;
							}
						}
						if (!same)
						{
							barcodelist.Add(infolist.Rows[i].Cells[3].Value.ToString());
						}
						same = false;
					}
				}
				computetotalmoney("", select: false);
				alertMessage("數量減1");
				setfocus();
			}
		}
		else if (e.Control && e.KeyCode == Keys.M)
		{
			Program.goodsTemp.Clear();
			for (int j = 0; j < infolist.Rows.Count; j++)
			{
				Program.goodsTemp.Add(new GoodObject((int)infolist.Rows[j].Cells[0].Value, (CommodityInfo)infolist.Rows[j].Cells[1].Value, infolist.Rows[j].Cells[2].Value.ToString(), infolist.Rows[j].Cells[3].Value.ToString(), infolist.Rows[j].Cells[4].Value.ToString(), infolist.Rows[j].Cells[5].Value.ToString()));
			}
			switchForm(new frmChooseMember());
		}
		else if (e.KeyCode == Keys.C)
		{
			tempKeyCode += "C";
		}
		else if (e.KeyCode == Keys.T)
		{
			tempKeyCode += "T";
		}
		else if (e.KeyCode == Keys.R)
		{
			tempKeyCode += "R";
		}
		else if (e.KeyCode == Keys.L)
		{
			tempKeyCode += "L";
		}
		else if (e.KeyCode == Keys.M)
		{
			tempKeyCode += "M";
		}
		else if (e.KeyCode == Keys.O)
		{
			tempKeyCode += "O";
		}
		if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(textBox1.Text) && columnOfFocus == 1)
		{
			if (tempKeyCode.Length >= 5)
			{
				if ("CTRLM".Equals(tempKeyCode))
				{
					Program.goodsTemp.Clear();
					for (int k = 0; k < infolist.Rows.Count; k++)
					{
						Program.goodsTemp.Add(new GoodObject(int.Parse(infolist.Rows[k].Cells["Column1"].Value.ToString()), (CommodityInfo)infolist.Rows[k].Cells["commodity"].Value, infolist.Rows[k].Cells["quantity"].Value.ToString(), infolist.Rows[k].Cells["barcode"].Value.ToString(), infolist.Rows[k].Cells["cropId"].Value.ToString(), infolist.Rows[k].Cells["pestId"].Value.ToString()));
					}
					switchForm(new frmChooseMember());
				}
				else if ("CTRLO".Equals(tempKeyCode))
				{
					Checkout_Click(sender, e);
				}
			}
			else
			{
				commodityEnter();
			}
			tempKeyCode = "";
		}
		else if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(textBox1.Text) && columnOfFocus == 3)
		{
			if (tempKeyCode.Length >= 5)
			{
				if ("CTRLM".Equals(tempKeyCode))
				{
					Program.goodsTemp.Clear();
					for (int l = 0; l < infolist.Rows.Count; l++)
					{
						Program.goodsTemp.Add(new GoodObject(int.Parse(infolist.Rows[l].Cells["Column1"].Value.ToString()), (CommodityInfo)infolist.Rows[l].Cells["commodity"].Value, infolist.Rows[l].Cells["quantity"].Value.ToString(), infolist.Rows[l].Cells["barcode"].Value.ToString(), infolist.Rows[l].Cells["cropId"].Value.ToString(), infolist.Rows[l].Cells["pestId"].Value.ToString()));
					}
					switchForm(new frmChooseMember());
				}
				else if ("CTRLO".Equals(tempKeyCode))
				{
					Checkout_Click(sender, e);
				}
			}
			else
			{
				try
				{
					int num3 = int.Parse(textBox1.Text);
					if (num3 == 0)
					{
						infolist.Rows.RemoveAt(infolist.CurrentRow.Index);
					}
					else
					{
						infolist.CurrentRow.Cells["quantity"].Value = num3.ToString();
					}
					textBox1.Text = "";
					computetotalmoney("", select: false);
				}
				catch (Exception)
				{
					AutoClosingMessageBox.Show("輸入數量錯誤");
					textBox1.Text = "";
				}
			}
			int count = infolist.Rows.Count;
			for (int m = 0; m < count; m++)
			{
				infolist.Rows[m].Selected = true;
				infolist.CurrentCell = infolist.Rows[m].Cells[0];
			}
			if (infolist.CurrentCell != null)
			{
				infolist.ClearSelection();
				(infolist.CurrentRow.Cells[1].Value as CommodityInfo).BackColor = Color.White;
				infolist.CurrentRow.Selected = false;
				infolist.Refresh();
			}
			textBox1.Select();
			tempKeyCode = "";
		}
		else
		{
			if (e.KeyCode != Keys.Return)
			{
				return;
			}
			if (tempKeyCode.Length >= 5)
			{
				if ("CTRLM".Equals(tempKeyCode))
				{
					Program.goodsTemp.Clear();
					for (int n = 0; n < infolist.Rows.Count; n++)
					{
						Program.goodsTemp.Add(new GoodObject(int.Parse(infolist.Rows[n].Cells["Column1"].Value.ToString()), (CommodityInfo)infolist.Rows[n].Cells["commodity"].Value, infolist.Rows[n].Cells["quantity"].Value.ToString(), infolist.Rows[n].Cells["barcode"].Value.ToString(), infolist.Rows[n].Cells["cropId"].Value.ToString(), infolist.Rows[n].Cells["pestId"].Value.ToString()));
					}
					switchForm(new frmChooseMember());
				}
				else if ("CTRLO".Equals(tempKeyCode))
				{
					Checkout_Click(sender, e);
				}
			}
			tempKeyCode = "";
		}
	}

	private void numEnter(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return && columnOfFocus == 2)
		{
			string[] strWhereParameterArray = new string[1]
			{
				textBox1.Text
			};
			foreach (DataRow row in ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_GOODSLST", "GDSNO = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable)).Rows)
			{
				infolist.Rows.Add(infolist.RowCount + 1, row["GDSNAME"].ToString(), num.Text, textBox1.Text);
				infolist.Rows[0].Selected = false;
				foreach (string item in barcodelist)
				{
					if (item.Equals(textBox1.Text))
					{
						same = true;
					}
				}
				if (!same)
				{
					barcodelist.Add(textBox1.Text);
				}
				same = false;
			}
			computetotalmoney("", select: true);
			textBox1.Text = "";
			num.Text = "";
		}
	}

	private void textBox1_Enter(object sender, EventArgs e)
	{
		textBox1.Focus();
		if (infolist.CurrentCell != null)
		{
			infolist.ClearSelection();
			(infolist.CurrentRow.Cells[1].Value as CommodityInfo).BackColor = Color.White;
			infolist.CurrentRow.Selected = false;
			infolist.Refresh();
		}
		columnOfFocus = 1;
	}

	private void num_Focus(object sender, EventArgs e)
	{
		columnOfFocus = 2;
	}

	private void infolist_SelectionChanged(object sender, EventArgs e)
	{
		for (int i = 0; i < infolist.RowCount; i++)
		{
			(infolist[1, i].Value as CommodityInfo).BackColor = Color.White;
		}
		if (infolist.CurrentRow != null && infolist.CurrentRow.Selected)
		{
			num.Text = infolist.CurrentRow.Cells["quantity"].Value.ToString();
			(infolist.CurrentRow.Cells[1].Value as CommodityInfo).BackColor = Color.FromArgb(255, 208, 81);
			infolist.Refresh();
		}
		columnOfFocus = 3;
	}

	private void numone_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "1";
	}

	private void numzero_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "0";
	}

	private void numtwo_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "2";
	}

	private void numthree_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "3";
	}

	private void numfour_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "4";
	}

	private void numfive_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "5";
	}

	private void numsix_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "6";
	}

	private void numseven_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "7";
	}

	private void numeight_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "8";
	}

	private void numnine_Click(object sender, EventArgs e)
	{
		textBox1.Text = textBox1.Text.Trim() + "9";
	}

	private void pressEnter_Click(object sender, EventArgs e)
	{
		if (columnOfFocus == 3)
		{
			if (!string.IsNullOrEmpty(textBox1.Text))
			{
				try
				{
					int num = int.Parse(textBox1.Text);
					if (num == 0)
					{
						infolist.Rows.RemoveAt(infolist.CurrentRow.Index);
					}
					else
					{
						infolist.CurrentRow.Cells["quantity"].Value = num.ToString();
					}
					textBox1.Text = "";
					computetotalmoney("", select: true);
				}
				catch (Exception)
				{
					AutoClosingMessageBox.Show("輸入數量錯誤");
					textBox1.Text = "";
				}
			}
		}
		else if (columnOfFocus == 1)
		{
			commodityEnter();
		}
		textBox1.Select();
	}

	private void backspace_Click(object sender, EventArgs e)
	{
		if (textBox1.Text.Length > 0)
		{
			textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
		}
	}

	private void pre_Click(object sender, EventArgs e)
	{
		if (columnOfFocus == 3 && infolist.CurrentRow != null)
		{
			if (infolist.CurrentRow.Index > 0)
			{
				int index = infolist.CurrentRow.Index;
				infolist.Rows[infolist.CurrentRow.Index - 1].Selected = true;
				infolist.Rows[index].Selected = false;
				infolist.CurrentCell = infolist.Rows[index - 1].Cells[0];
				infolist_SelectionChanged(sender, e);
			}
			else
			{
				AutoClosingMessageBox.Show("已經是第一筆商品");
			}
		}
	}

	private void addone_Click(object sender, EventArgs e)
	{
		if (columnOfFocus == 3 && infolist.CurrentRow != null)
		{
			int num = int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) + 1;
			infolist.CurrentRow.Cells[2].Value = num.ToString();
			computetotalmoney("", select: true);
			alertMessage("數量加1");
			textBox1.Select();
		}
	}

	private void subone_Click(object sender, EventArgs e)
	{
		if (columnOfFocus != 3 || infolist.CurrentRow == null || int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) <= 0)
		{
			return;
		}
		int num = int.Parse(infolist.CurrentRow.Cells[2].Value.ToString()) - 1;
		if (num > 0)
		{
			infolist.CurrentRow.Cells[2].Value = num.ToString();
		}
		else
		{
			infolist.Rows.RemoveAt(infolist.CurrentRow.Index);
			barcodelist.Clear();
			for (int i = 0; i < infolist.Rows.Count; i++)
			{
				foreach (string item in barcodelist)
				{
					if (item.Equals(infolist.Rows[i].Cells[3].Value.ToString()))
					{
						same = true;
					}
				}
				if (!same)
				{
					barcodelist.Add(infolist.Rows[i].Cells[3].Value.ToString());
				}
				same = false;
			}
		}
		computetotalmoney("", select: true);
		alertMessage("數量減1");
		textBox1.Select();
	}

	private void oneremove_Click(object sender, EventArgs e)
	{
		if (columnOfFocus == 3)
		{
			if (infolist.CurrentRow == null)
			{
				AutoClosingMessageBox.Show("請選擇商品");
				return;
			}
			infolist.Rows.RemoveAt(infolist.CurrentRow.Index);
			computetotalmoney("", select: true);
			alertMessage("移除選擇商品");
		}
	}

	private void removeall_Click(object sender, EventArgs e)
	{
		barcodelist.Clear();
		infolist.Rows.Clear();
		infolist.Refresh();
		computetotalmoney("", select: true);
		alertMessage("移除全部商品");
	}

	private void computetotalmoney(string type, bool select)
	{
		totalmoney = 0;
		int num = 0;
		barcodelist.Clear();
		for (int i = 0; i < infolist.Rows.Count; i++)
		{
			foreach (string item in barcodelist)
			{
				if (item.Equals(infolist.Rows[i].Cells[3].Value.ToString()))
				{
					same = true;
				}
			}
			if (!same)
			{
				barcodelist.Add(infolist.Rows[i].Cells[3].Value.ToString());
			}
			same = false;
			infolist.Rows[i].Selected = false;
		}
		foreach (DataGridViewRow item2 in (IEnumerable)infolist.Rows)
		{
			num++;
			totalmoney += int.Parse(item2.Cells["quantity"].Value.ToString());
		}
		if ("Enter".Equals(type))
		{
			this.num.Text = "1";
		}
		else
		{
			this.num.Text = "";
		}
		totalprice.Text = barcodelist.Count.ToString();
		totalpriceDiscount.Text = totalmoney.ToString();
		if (select)
		{
			textBox1.Select();
		}
	}

	private void next_Click(object sender, EventArgs e)
	{
		if (columnOfFocus == 3 && infolist.CurrentRow != null)
		{
			if (infolist.CurrentRow.Index < infolist.Rows.Count - 1)
			{
				int index = infolist.CurrentRow.Index;
				infolist.Rows[index + 1].Selected = true;
				infolist.Rows[index].Selected = false;
				infolist.CurrentCell = infolist.Rows[index + 1].Cells[0];
				infolist_SelectionChanged(sender, e);
			}
			else
			{
				AutoClosingMessageBox.Show("已經是最後一筆商品");
			}
		}
	}

	private void alertMessage(string msg)
	{
		alertMsg.Text = msg;
	}

	private void flp_chooseMember_Click(object sender, EventArgs e)
	{
		Program.goodsTemp.Clear();
		foreach (DataGridViewRow item in (IEnumerable)infolist.Rows)
		{
			Program.goodsTemp.Add(new GoodObject(int.Parse(item.Cells[0].Value.ToString()), (CommodityInfo)item.Cells[1].Value, item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString(), item.Cells[4].Value.ToString(), item.Cells[5].Value.ToString()));
		}
		switchForm(new frmChooseMember());
	}

	private void commoditySearch_Click(object sender, EventArgs e)
	{
		frmCommoditySearch frmCommoditySearch = new frmCommoditySearch(this);
		frmCommoditySearch.Location = new Point(base.Location.X, base.Location.Y);
		frmCommoditySearch.Show();
		Hide();
	}

	public void addOnecommodity(string barcode, string cropId, string pestId, string cropName, string pestName)
	{
		string[] strWhereParameterArray = new string[1]
		{
			barcode
		};
		foreach (DataRow row in ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_GOODSLST", "GDSNO = {0} ", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable)).Rows)
		{
			uc = new CommodityInfo();
			uc.setMemberIdNo("");
			uc.setMemberVipNo("店內碼:" + row["GDSNO"].ToString());
			uc.setCommodityName(setCommodityName(row));
			uc.setCommodityClass(row["spec"].ToString() + " " + row["capacity"].ToString());
			uc.setlabe1("作物: " + cropName + "|病蟲害: " + pestName);
			infolist.Rows.Add(infolist.RowCount + 1, uc, "1", barcode, cropId, pestId);
			foreach (string item in barcodelist)
			{
				if (item.Equals(barcode))
				{
					same = true;
				}
			}
			if (!same)
			{
				barcodelist.Add(barcode);
			}
			same = false;
		}
		infolist.ClearSelection();
		infolist.CurrentRow.Selected = false;
		computetotalmoney("", select: true);
	}

	private void Checkout_Click(object sender, EventArgs e)
	{
		Checkout.Enabled = false;
		try
		{
			if (vipNo == "")
			{
				AutoClosingMessageBox.Show("會員尚未選擇");
			}
			else if (infolist.RowCount == 0)
			{
				AutoClosingMessageBox.Show("無銷售商品");
			}
			else
			{
				string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				string[,] strFieldArray = new string[13, 2]
				{
					{
						"sellNo",
						getHseqNo()
					},
					{
						"sellTime",
						text
					},
					{
						"memberId",
						vipNo
					},
					{
						"sum",
						"0"
					},
					{
						"sumDiscount",
						"0"
					},
					{
						"sumRebate",
						"0"
					},
					{
						"cash",
						"0"
					},
					{
						"Credit",
						"0"
					},
					{
						"items",
						gettotalprice()
					},
					{
						"itemstotal",
						gettotalpriceDiscount()
					},
					{
						"status",
						"0"
					},
					{
						"editDate",
						text
					},
					{
						"changcount",
						"1"
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				strFieldArray = new string[4, 2]
				{
					{
						"sellNo",
						getHseqNo()
					},
					{
						"changeDate",
						text
					},
					{
						"isprint",
						"1"
					},
					{
						"sum",
						""
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_mainsell_log", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				strFieldArray = new string[7, 2]
				{
					{
						"memberId",
						vipNo
					},
					{
						"sellNo",
						getHseqNo()
					},
					{
						"editdate",
						text
					},
					{
						"sellType",
						"0"
					},
					{
						"Cash",
						"0"
					},
					{
						"Credit",
						"0"
					},
					{
						"status",
						"0"
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_user_consumelog", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_CUST_RTL SET BuyDate = {1} WHERE VipNo = {0}", new string[2]
				{
					vipNo,
					text
				}, CommandOperationType.ExecuteNonQuery);
				for (int i = 0; i < infolist.Rows.Count; i++)
				{
					string text2 = infolist.Rows[i].Cells["barcode"].Value.ToString();
					string text3 = infolist.Rows[i].Cells["quantity"].Value.ToString();
					string text4 = infolist.Rows[i].Cells["cropId"].Value.ToString();
					string text5 = infolist.Rows[i].Cells["pestId"].Value.ToString();
					strFieldArray = new string[10, 2]
					{
						{
							"sellNo",
							getHseqNo()
						},
						{
							"barcode",
							text2
						},
						{
							"fixedPrice",
							"0"
						},
						{
							"sellingPrice",
							"0"
						},
						{
							"num",
							text3
						},
						{
							"discount",
							"0"
						},
						{
							"subtotal",
							"0"
						},
						{
							"total",
							"0"
						},
						{
							"PRNO",
							text4
						},
						{
							"BLNO",
							text5
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
					string[] array = new string[4]
					{
						vipNo,
						text2,
						text4,
						text5
					};
					DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "total", "hypos_user_pair", " barcode ={1} and cropId={2} and pestId={3} ", "", null, array, CommandOperationType.ExecuteReaderReturnDataTable);
					if (!"".Equals(text4) && !"".Equals(text5))
					{
						if (dataTable.Rows.Count > 0)
						{
							DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_user_pair SET total = total+1 where  barcode ={1} and cropId={2} and pestId={3} ", array, CommandOperationType.ExecuteNonQuery);
						}
						else
						{
							strFieldArray = new string[5, 2]
							{
								{
									"VipNo",
									vipNo
								},
								{
									"barcode",
									text2
								},
								{
									"total",
									"1"
								},
								{
									"cropId",
									text4
								},
								{
									"pestId",
									text5
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_user_pair", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
						}
					}
					string[] strWhereParameterArray = new string[1]
					{
						text2
					};
					DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "GDSNO, inventory", "hypos_GOODSLST", "GDSNO = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
					int num = -int.Parse(text3);
					if (!string.IsNullOrEmpty(dataTable2.Rows[0]["inventory"].ToString()))
					{
						num = int.Parse(dataTable2.Rows[0]["inventory"].ToString()) - int.Parse(text3);
					}
					string[] strParameterArray = new string[2]
					{
						num.ToString(),
						text2
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_GOODSLST SET inventory ={0} where GDSNO ={1}", strParameterArray, CommandOperationType.ExecuteNonQuery);
				}
				AutoClosingMessageBox.Show("新增成功");
				Program.goodsTemp.Clear();
				switchForm(new frmMainShopSimple());
				new frmSell_SellNo(HseqNo).Show();
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			Checkout.Enabled = true;
		}
	}

	public string gettotalprice()
	{
		return totalprice.Text;
	}

	public string gettotalpriceDiscount()
	{
		return totalpriceDiscount.Text;
	}

	public string getHseqNo()
	{
		return HseqNo;
	}

	private void cureGuide_Click(object sender, EventArgs e)
	{
		frmCropGuide frmCropGuide = new frmCropGuide(this);
		frmCropGuide.Location = new Point(base.Location.X, base.Location.Y);
		frmCropGuide.Show();
		Hide();
	}

	public void addCropAndPest(int index, string crop, string pest)
	{
		infolist.Rows[index - 1].Cells[4].Value = crop;
		infolist.Rows[index - 1].Cells[5].Value = pest;
		string[] strWhereParameterArray = new string[1]
		{
			crop
		};
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "name", "HyCrop", "code = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		string[] strWhereParameterArray2 = new string[1]
		{
			pest
		};
		DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "name", "HyBlight", "code = {0}", "", null, strWhereParameterArray2, CommandOperationType.ExecuteReaderReturnDataTable);
		uc.setlabe1("作物: " + dataTable.Rows[0]["name"].ToString() + "|病蟲害: " + dataTable2.Rows[0]["name"].ToString());
	}

	private void button1_Click(object sender, EventArgs e)
	{
		switchForm(new frmSearchSell());
	}

	public void setfocus()
	{
		textBox1.Select();
	}

	public void RemoveLast()
	{
		infolist.Rows.RemoveAt(infolist.Rows.Count - 1);
		computetotalmoney("", select: true);
		textBox1.Select();
	}

	private void panal1_Paint(object sender, PaintEventArgs e)
	{
		MatchCollection matchCollection = Regex.Matches("#d9d9d9", "([0-9A-Fa-f]{2})");
		if (matchCollection.Count == 3)
		{
			int red = Convert.ToInt32(matchCollection[0].Groups[0].Value, 16);
			int green = Convert.ToInt32(matchCollection[1].Groups[0].Value, 16);
			int blue = Convert.ToInt32(matchCollection[2].Groups[0].Value, 16);
			ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle, Color.FromArgb(red, green, blue), 1, ButtonBorderStyle.Solid, Color.FromArgb(red, green, blue), 1, ButtonBorderStyle.Solid, Color.FromArgb(red, green, blue), 1, ButtonBorderStyle.Solid, Color.FromArgb(red, green, blue), 1, ButtonBorderStyle.Solid);
		}
	}

	private string setCommodityName(DataRow row)
	{
		string text = row["GDName"].ToString();
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
		return text;
	}

	private void btn_MedDescription_Click(object sender, EventArgs e)
	{
		if (infolist.CurrentRow == null)
		{
			AutoClosingMessageBox.Show("請選擇商品");
		}
		else if (infolist.CurrentRow.Selected)
		{
			new dialogMedDescription(infolist["barcode", infolist.CurrentRow.Index].Value.ToString(), infolist["cropId", infolist.CurrentRow.Index].Value.ToString(), infolist["pestId", infolist.CurrentRow.Index].Value.ToString()).ShowDialog();
		}
		else
		{
			AutoClosingMessageBox.Show("請選擇商品");
		}
	}

	private void infolist_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
	{
		infolist.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1).ToString();
	}

	private void textBox1_KeyUp(object sender, KeyEventArgs e)
	{
	}

	private void commodityEnter()
	{
		if (textBox1.Text.Trim().Length != 0)
		{
			alertMessage("選入商品");
			string text = textBox1.Text.Trim();
			string[] strWhereParameterArray = new string[1]
			{
				text
			};
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_GOODSLST as hg,HyLicence as hl", "(hg.GDSNO = {0} or hg.hot_key = {0}) and hg.ISWS ='Y' and hg.CLA1NO ='0302' and hg.licType = hl.licType and hg.domManufId = hl.licNo and hl.isDelete='N' ", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
			string text2 = "";
			if (dataTable.Rows.Count > 0)
			{
				text2 = dataTable.Rows[0]["GDSNO"].ToString();
			}
			text = text2;
			if (dataTable.Rows.Count > 0)
			{
				if (!"D".Equals(dataTable.Rows[0]["status"].ToString()))
				{
					foreach (DataRow row in dataTable.Rows)
					{
						if (row["CLA1NO"].ToString().Equals("0302"))
						{
							uc = new CommodityInfo();
							uc.setMemberIdNo("");
							uc.setMemberVipNo("店內碼:" + row["GDSNO"].ToString());
							uc.setCommodityName(setCommodityName(row));
							uc.setCommodityClass(row["spec"].ToString() + " " + row["capacity"].ToString());
							infolist.Rows.Add(infolist.RowCount + 1, uc, "1", text);
							foreach (DataGridViewRow item in (IEnumerable)infolist.Rows)
							{
								item.Height = 100;
							}
							infolist.Rows[0].Selected = false;
							foreach (string item2 in barcodelist)
							{
								if (item2.Equals(text))
								{
									same = true;
								}
							}
							if (!same)
							{
								barcodelist.Add(text);
							}
							same = false;
						}
					}
					infolist.ClearSelection();
					infolist.CurrentRow.Selected = false;
					computetotalmoney("Enter", select: true);
					int count = infolist.Rows.Count;
					new frmDialogMedNew(this, count, text).ShowDialog();
				}
				else
				{
					AutoClosingMessageBox.Show("商品已禁賣，請重新輸入");
				}
			}
			else
			{
				AutoClosingMessageBox.Show("商品不存在，請重新輸入");
			}
			textBox1.Text = textBox1.Text.Replace(Environment.NewLine, "");
			textBox1.Text = "";
			columnOfFocus = 1;
		}
		else
		{
			AutoClosingMessageBox.Show("商品不存在，請重新輸入");
		}
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
		textBox1 = new System.Windows.Forms.TextBox();
		num = new System.Windows.Forms.TextBox();
		l_sysTime = new System.Windows.Forms.Label();
		infolist = new System.Windows.Forms.DataGridView();
		addone = new System.Windows.Forms.Button();
		subone = new System.Windows.Forms.Button();
		numone = new System.Windows.Forms.Button();
		pressEnter = new System.Windows.Forms.Button();
		panel1 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		flp_chooseMember = new System.Windows.Forms.FlowLayoutPanel();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		label2 = new System.Windows.Forms.Label();
		alertMsg = new System.Windows.Forms.TextBox();
		totalspending = new System.Windows.Forms.Label();
		totalpriceDiscount = new System.Windows.Forms.Label();
		totalpriceDiscountView = new System.Windows.Forms.Label();
		totalprice = new System.Windows.Forms.Label();
		totalpriceview = new System.Windows.Forms.Label();
		backspace = new System.Windows.Forms.Button();
		pre = new System.Windows.Forms.Button();
		oneremove = new System.Windows.Forms.Button();
		next = new System.Windows.Forms.Button();
		numzero = new System.Windows.Forms.Button();
		numtwo = new System.Windows.Forms.Button();
		numthree = new System.Windows.Forms.Button();
		numfour = new System.Windows.Forms.Button();
		numsix = new System.Windows.Forms.Button();
		numfive = new System.Windows.Forms.Button();
		numseven = new System.Windows.Forms.Button();
		numeight = new System.Windows.Forms.Button();
		numnine = new System.Windows.Forms.Button();
		removeall = new System.Windows.Forms.Button();
		btn_MedDescription = new System.Windows.Forms.Button();
		commoditySearch = new System.Windows.Forms.Button();
		cureGuide = new System.Windows.Forms.Button();
		Checkout = new System.Windows.Forms.Button();
		button1 = new System.Windows.Forms.Button();
		Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		commodity = new POS_Client.frmMainShopSimple.CustomColumn();
		quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
		barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		cropId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		pestId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).BeginInit();
		((System.ComponentModel.ISupportInitialize)infolist).BeginInit();
		panel1.SuspendLayout();
		flp_chooseMember.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		SuspendLayout();
		pb_virtualKeyBoard.Location = new System.Drawing.Point(898, 620);
		pb_virtualKeyBoard.Size = new System.Drawing.Size(70, 7);
		textBox1.Font = new System.Drawing.Font("Calibri", 15.75f);
		textBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
		textBox1.Location = new System.Drawing.Point(325, 14);
		textBox1.Multiline = true;
		textBox1.Name = "textBox1";
		textBox1.Size = new System.Drawing.Size(266, 64);
		textBox1.TabIndex = 2;
		textBox1.Enter += new System.EventHandler(textBox1_Enter);
		num.Font = new System.Drawing.Font("Calibri", 15.75f);
		num.Location = new System.Drawing.Point(633, 14);
		num.Multiline = true;
		num.Name = "num";
		num.ReadOnly = true;
		num.Size = new System.Drawing.Size(92, 67);
		num.TabIndex = 3;
		l_sysTime.AutoSize = true;
		l_sysTime.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_sysTime.Location = new System.Drawing.Point(605, 40);
		l_sysTime.Name = "l_sysTime";
		l_sysTime.Size = new System.Drawing.Size(19, 20);
		l_sysTime.TabIndex = 5;
		l_sysTime.Text = "X";
		infolist.AllowUserToAddRows = false;
		infolist.AllowUserToDeleteRows = false;
		infolist.AllowUserToResizeColumns = false;
		infolist.AllowUserToResizeRows = false;
		infolist.Anchor = System.Windows.Forms.AnchorStyles.None;
		infolist.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
		infolist.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		infolist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		infolist.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.FromArgb(157, 157, 157);
		dataGridViewCellStyle.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(6);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(175, 164, 134);
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		infolist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		infolist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		infolist.Columns.AddRange(Column1, commodity, quantity, barcode, cropId, pestId);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 15f);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(255, 208, 81);
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		infolist.DefaultCellStyle = dataGridViewCellStyle2;
		infolist.EnableHeadersVisualStyles = false;
		infolist.GridColor = System.Drawing.SystemColors.ActiveBorder;
		infolist.Location = new System.Drawing.Point(21, 247);
		infolist.MultiSelect = false;
		infolist.Name = "infolist";
		infolist.ReadOnly = true;
		infolist.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
		dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(255, 208, 81);
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		infolist.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		infolist.RowHeadersVisible = false;
		infolist.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		infolist.RowTemplate.Height = 102;
		infolist.RowTemplate.ReadOnly = true;
		infolist.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		infolist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		infolist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		infolist.Size = new System.Drawing.Size(754, 408);
		infolist.TabIndex = 9;
		infolist.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(infolist_RowPostPaint);
		infolist.SelectionChanged += new System.EventHandler(infolist_SelectionChanged);
		addone.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		addone.Location = new System.Drawing.Point(790, 333);
		addone.Name = "addone";
		addone.Size = new System.Drawing.Size(40, 40);
		addone.TabIndex = 10;
		addone.Text = "+1";
		addone.UseVisualStyleBackColor = true;
		addone.Click += new System.EventHandler(addone_Click);
		subone.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		subone.Location = new System.Drawing.Point(792, 379);
		subone.Name = "subone";
		subone.Size = new System.Drawing.Size(40, 40);
		subone.TabIndex = 11;
		subone.Text = "-1";
		subone.UseVisualStyleBackColor = true;
		subone.Click += new System.EventHandler(subone_Click);
		numone.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numone.Location = new System.Drawing.Point(838, 425);
		numone.Name = "numone";
		numone.Size = new System.Drawing.Size(40, 40);
		numone.TabIndex = 12;
		numone.Text = "1";
		numone.UseVisualStyleBackColor = true;
		numone.Click += new System.EventHandler(numone_Click);
		pressEnter.BackColor = System.Drawing.Color.FromArgb(167, 202, 0);
		pressEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		pressEnter.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		pressEnter.ForeColor = System.Drawing.Color.White;
		pressEnter.Location = new System.Drawing.Point(883, 517);
		pressEnter.Name = "pressEnter";
		pressEnter.Size = new System.Drawing.Size(88, 63);
		pressEnter.TabIndex = 13;
		pressEnter.Text = "確認\r\n輸入";
		pressEnter.UseVisualStyleBackColor = false;
		pressEnter.Click += new System.EventHandler(pressEnter_Click);
		panel1.BackColor = System.Drawing.Color.White;
		panel1.Controls.Add(label3);
		panel1.Controls.Add(label1);
		panel1.Controls.Add(flp_chooseMember);
		panel1.Controls.Add(alertMsg);
		panel1.Controls.Add(totalspending);
		panel1.Controls.Add(totalpriceDiscount);
		panel1.Controls.Add(totalpriceDiscountView);
		panel1.Controls.Add(totalprice);
		panel1.Controls.Add(totalpriceview);
		panel1.Controls.Add(num);
		panel1.Controls.Add(textBox1);
		panel1.Controls.Add(l_sysTime);
		panel1.Location = new System.Drawing.Point(23, 50);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(752, 191);
		panel1.TabIndex = 14;
		panel1.Paint += new System.Windows.Forms.PaintEventHandler(panal1_Paint);
		label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		label3.Location = new System.Drawing.Point(298, 0);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(2, 191);
		label3.TabIndex = 12;
		label3.Text = "label3";
		label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		label1.Location = new System.Drawing.Point(10, 106);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(280, 2);
		label1.TabIndex = 11;
		label1.Text = "label1";
		flp_chooseMember.Controls.Add(pictureBox1);
		flp_chooseMember.Controls.Add(label2);
		flp_chooseMember.Cursor = System.Windows.Forms.Cursors.Hand;
		flp_chooseMember.Location = new System.Drawing.Point(3, 1);
		flp_chooseMember.Margin = new System.Windows.Forms.Padding(0);
		flp_chooseMember.Name = "flp_chooseMember";
		flp_chooseMember.Size = new System.Drawing.Size(292, 95);
		flp_chooseMember.TabIndex = 10;
		flp_chooseMember.Click += new System.EventHandler(flp_chooseMember_Click);
		pictureBox1.Image = POS_Client.Properties.Resources.more;
		pictureBox1.Location = new System.Drawing.Point(5, 5);
		pictureBox1.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(61, 65);
		pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox1.TabIndex = 2;
		pictureBox1.TabStop = false;
		pictureBox1.Click += new System.EventHandler(flp_chooseMember_Click);
		label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Location = new System.Drawing.Point(76, 17);
		label2.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(123, 35);
		label2.TabIndex = 3;
		label2.Text = "選擇會員";
		label2.Click += new System.EventHandler(flp_chooseMember_Click);
		alertMsg.Font = new System.Drawing.Font("微軟正黑體", 15.75f);
		alertMsg.Location = new System.Drawing.Point(325, 93);
		alertMsg.Multiline = true;
		alertMsg.Name = "alertMsg";
		alertMsg.ReadOnly = true;
		alertMsg.Size = new System.Drawing.Size(400, 78);
		alertMsg.TabIndex = 6;
		totalspending.AutoSize = true;
		totalspending.Font = new System.Drawing.Font("Arial", 32f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		totalspending.ForeColor = System.Drawing.Color.Orange;
		totalspending.Location = new System.Drawing.Point(206, 135);
		totalspending.Name = "totalspending";
		totalspending.Size = new System.Drawing.Size(0, 36);
		totalspending.TabIndex = 5;
		totalpriceDiscount.AutoSize = true;
		totalpriceDiscount.Font = new System.Drawing.Font("Arial", 32f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		totalpriceDiscount.Location = new System.Drawing.Point(110, 135);
		totalpriceDiscount.Name = "totalpriceDiscount";
		totalpriceDiscount.Size = new System.Drawing.Size(0, 36);
		totalpriceDiscount.TabIndex = 3;
		totalpriceDiscountView.AutoSize = true;
		totalpriceDiscountView.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		totalpriceDiscountView.Location = new System.Drawing.Point(110, 115);
		totalpriceDiscountView.Name = "totalpriceDiscountView";
		totalpriceDiscountView.Size = new System.Drawing.Size(36, 18);
		totalpriceDiscountView.TabIndex = 2;
		totalpriceDiscountView.Text = "數量";
		totalprice.AutoSize = true;
		totalprice.Font = new System.Drawing.Font("Arial", 32f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		totalprice.Location = new System.Drawing.Point(13, 135);
		totalprice.Name = "totalprice";
		totalprice.Size = new System.Drawing.Size(0, 36);
		totalprice.TabIndex = 1;
		totalpriceview.AutoSize = true;
		totalpriceview.Font = new System.Drawing.Font("微軟正黑體", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		totalpriceview.Location = new System.Drawing.Point(13, 115);
		totalpriceview.Name = "totalpriceview";
		totalpriceview.Size = new System.Drawing.Size(36, 18);
		totalpriceview.TabIndex = 0;
		totalpriceview.Text = "品項";
		backspace.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		backspace.Location = new System.Drawing.Point(885, 471);
		backspace.Name = "backspace";
		backspace.Size = new System.Drawing.Size(88, 40);
		backspace.TabIndex = 15;
		backspace.Text = "backspace";
		backspace.UseVisualStyleBackColor = true;
		backspace.Click += new System.EventHandler(backspace_Click);
		pre.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		pre.Location = new System.Drawing.Point(790, 425);
		pre.Name = "pre";
		pre.Size = new System.Drawing.Size(40, 40);
		pre.TabIndex = 16;
		pre.Text = "↑";
		pre.UseVisualStyleBackColor = true;
		pre.Click += new System.EventHandler(pre_Click);
		oneremove.BackColor = System.Drawing.Color.FromArgb(162, 162, 162);
		oneremove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		oneremove.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		oneremove.ForeColor = System.Drawing.Color.Transparent;
		oneremove.Location = new System.Drawing.Point(791, 257);
		oneremove.Name = "oneremove";
		oneremove.Size = new System.Drawing.Size(88, 63);
		oneremove.TabIndex = 18;
		oneremove.Text = "單筆\r\n移除";
		oneremove.UseVisualStyleBackColor = false;
		oneremove.Click += new System.EventHandler(oneremove_Click);
		next.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		next.Location = new System.Drawing.Point(790, 471);
		next.Name = "next";
		next.Size = new System.Drawing.Size(40, 40);
		next.TabIndex = 22;
		next.Text = "↓";
		next.UseVisualStyleBackColor = true;
		next.Click += new System.EventHandler(next_Click);
		numzero.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numzero.Location = new System.Drawing.Point(839, 471);
		numzero.Name = "numzero";
		numzero.Size = new System.Drawing.Size(40, 40);
		numzero.TabIndex = 23;
		numzero.Text = "0";
		numzero.UseVisualStyleBackColor = true;
		numzero.Click += new System.EventHandler(numzero_Click);
		numtwo.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numtwo.Location = new System.Drawing.Point(887, 425);
		numtwo.Name = "numtwo";
		numtwo.Size = new System.Drawing.Size(40, 40);
		numtwo.TabIndex = 24;
		numtwo.Text = "2";
		numtwo.UseVisualStyleBackColor = true;
		numtwo.Click += new System.EventHandler(numtwo_Click);
		numthree.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numthree.Location = new System.Drawing.Point(933, 425);
		numthree.Name = "numthree";
		numthree.Size = new System.Drawing.Size(40, 40);
		numthree.TabIndex = 25;
		numthree.Text = "3";
		numthree.UseVisualStyleBackColor = true;
		numthree.Click += new System.EventHandler(numthree_Click);
		numfour.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numfour.Location = new System.Drawing.Point(838, 379);
		numfour.Name = "numfour";
		numfour.Size = new System.Drawing.Size(40, 40);
		numfour.TabIndex = 26;
		numfour.Text = "4";
		numfour.UseVisualStyleBackColor = true;
		numfour.Click += new System.EventHandler(numfour_Click);
		numsix.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numsix.Location = new System.Drawing.Point(933, 379);
		numsix.Name = "numsix";
		numsix.Size = new System.Drawing.Size(40, 40);
		numsix.TabIndex = 27;
		numsix.Text = "6";
		numsix.UseVisualStyleBackColor = true;
		numsix.Click += new System.EventHandler(numsix_Click);
		numfive.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numfive.Location = new System.Drawing.Point(884, 379);
		numfive.Name = "numfive";
		numfive.Size = new System.Drawing.Size(40, 40);
		numfive.TabIndex = 28;
		numfive.Text = "5";
		numfive.UseVisualStyleBackColor = true;
		numfive.Click += new System.EventHandler(numfive_Click);
		numseven.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numseven.Location = new System.Drawing.Point(839, 333);
		numseven.Name = "numseven";
		numseven.Size = new System.Drawing.Size(40, 40);
		numseven.TabIndex = 29;
		numseven.Text = "7";
		numseven.UseVisualStyleBackColor = true;
		numseven.Click += new System.EventHandler(numseven_Click);
		numeight.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numeight.Location = new System.Drawing.Point(887, 333);
		numeight.Name = "numeight";
		numeight.Size = new System.Drawing.Size(40, 40);
		numeight.TabIndex = 30;
		numeight.Text = "8";
		numeight.UseVisualStyleBackColor = true;
		numeight.Click += new System.EventHandler(numeight_Click);
		numnine.Font = new System.Drawing.Font("Calibri", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		numnine.Location = new System.Drawing.Point(933, 333);
		numnine.Name = "numnine";
		numnine.Size = new System.Drawing.Size(40, 40);
		numnine.TabIndex = 31;
		numnine.Text = "9";
		numnine.UseVisualStyleBackColor = true;
		numnine.Click += new System.EventHandler(numnine_Click);
		removeall.BackColor = System.Drawing.Color.FromArgb(162, 162, 162);
		removeall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		removeall.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		removeall.ForeColor = System.Drawing.Color.White;
		removeall.Location = new System.Drawing.Point(887, 257);
		removeall.Name = "removeall";
		removeall.Size = new System.Drawing.Size(88, 63);
		removeall.TabIndex = 32;
		removeall.Text = "全部\r\n移除";
		removeall.UseVisualStyleBackColor = false;
		removeall.Click += new System.EventHandler(removeall_Click);
		btn_MedDescription.BackColor = System.Drawing.Color.FromArgb(192, 182, 154);
		btn_MedDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_MedDescription.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		btn_MedDescription.ForeColor = System.Drawing.Color.White;
		btn_MedDescription.Location = new System.Drawing.Point(789, 517);
		btn_MedDescription.Name = "btn_MedDescription";
		btn_MedDescription.Size = new System.Drawing.Size(88, 63);
		btn_MedDescription.TabIndex = 33;
		btn_MedDescription.Text = "用藥\r\n說明";
		btn_MedDescription.UseVisualStyleBackColor = false;
		btn_MedDescription.Click += new System.EventHandler(btn_MedDescription_Click);
		commoditySearch.BackColor = System.Drawing.Color.FromArgb(156, 187, 58);
		commoditySearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		commoditySearch.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		commoditySearch.ForeColor = System.Drawing.Color.White;
		commoditySearch.Location = new System.Drawing.Point(790, 131);
		commoditySearch.Name = "commoditySearch";
		commoditySearch.Size = new System.Drawing.Size(89, 120);
		commoditySearch.TabIndex = 34;
		commoditySearch.Text = "商品\r\n查詢";
		commoditySearch.UseVisualStyleBackColor = false;
		commoditySearch.Click += new System.EventHandler(commoditySearch_Click);
		cureGuide.BackColor = System.Drawing.Color.FromArgb(156, 187, 58);
		cureGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		cureGuide.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		cureGuide.ForeColor = System.Drawing.Color.White;
		cureGuide.Location = new System.Drawing.Point(885, 131);
		cureGuide.Name = "cureGuide";
		cureGuide.Size = new System.Drawing.Size(89, 120);
		cureGuide.TabIndex = 35;
		cureGuide.Text = "用藥\r\n指引";
		cureGuide.UseVisualStyleBackColor = false;
		cureGuide.Click += new System.EventHandler(cureGuide_Click);
		Checkout.BackColor = System.Drawing.Color.FromArgb(250, 87, 0);
		Checkout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		Checkout.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		Checkout.ForeColor = System.Drawing.Color.White;
		Checkout.Location = new System.Drawing.Point(790, 586);
		Checkout.Name = "Checkout";
		Checkout.Size = new System.Drawing.Size(183, 73);
		Checkout.TabIndex = 36;
		Checkout.Text = "收銀\r\n結帳";
		Checkout.UseVisualStyleBackColor = false;
		Checkout.Click += new System.EventHandler(Checkout_Click);
		button1.BackColor = System.Drawing.Color.FromArgb(45, 152, 165);
		button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		button1.Font = new System.Drawing.Font("微軟正黑體", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		button1.ForeColor = System.Drawing.Color.White;
		button1.Location = new System.Drawing.Point(789, 50);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(184, 78);
		button1.TabIndex = 37;
		button1.Text = "商品\r退貨";
		button1.UseVisualStyleBackColor = false;
		button1.Click += new System.EventHandler(button1_Click);
		Column1.HeaderText = "項次";
		Column1.Name = "Column1";
		Column1.ReadOnly = true;
		Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		Column1.Width = 62;
		commodity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
		commodity.DefaultCellStyle = dataGridViewCellStyle4;
		commodity.HeaderText = "商品名稱";
		commodity.Name = "commodity";
		commodity.ReadOnly = true;
		commodity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(89, 124, 14);
		quantity.DefaultCellStyle = dataGridViewCellStyle5;
		quantity.HeaderText = "數量";
		quantity.Name = "quantity";
		quantity.ReadOnly = true;
		quantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		quantity.Width = 62;
		barcode.HeaderText = "條碼";
		barcode.Name = "barcode";
		barcode.ReadOnly = true;
		barcode.Visible = false;
		barcode.Width = 62;
		cropId.HeaderText = "作物id";
		cropId.Name = "cropId";
		cropId.ReadOnly = true;
		cropId.Visible = false;
		cropId.Width = 76;
		pestId.HeaderText = "蟲害id";
		pestId.Name = "pestId";
		pestId.ReadOnly = true;
		pestId.Visible = false;
		pestId.Width = 76;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.Color.FromArgb(252, 252, 237);
		BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(981, 671);
		base.Controls.Add(Checkout);
		base.Controls.Add(button1);
		base.Controls.Add(cureGuide);
		base.Controls.Add(commoditySearch);
		base.Controls.Add(btn_MedDescription);
		base.Controls.Add(removeall);
		base.Controls.Add(numnine);
		base.Controls.Add(numeight);
		base.Controls.Add(numseven);
		base.Controls.Add(numfive);
		base.Controls.Add(numsix);
		base.Controls.Add(numfour);
		base.Controls.Add(numthree);
		base.Controls.Add(numtwo);
		base.Controls.Add(numzero);
		base.Controls.Add(next);
		base.Controls.Add(oneremove);
		base.Controls.Add(pre);
		base.Controls.Add(backspace);
		base.Controls.Add(pressEnter);
		base.Controls.Add(numone);
		base.Controls.Add(subone);
		base.Controls.Add(addone);
		base.Controls.Add(infolist);
		base.Controls.Add(panel1);
		base.Name = "frmMainShopSimple";
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		Text = "frmMainShop";
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Form1_KeyDown);
		base.Controls.SetChildIndex(pb_virtualKeyBoard, 0);
		base.Controls.SetChildIndex(panel1, 0);
		base.Controls.SetChildIndex(infolist, 0);
		base.Controls.SetChildIndex(addone, 0);
		base.Controls.SetChildIndex(subone, 0);
		base.Controls.SetChildIndex(numone, 0);
		base.Controls.SetChildIndex(pressEnter, 0);
		base.Controls.SetChildIndex(backspace, 0);
		base.Controls.SetChildIndex(pre, 0);
		base.Controls.SetChildIndex(oneremove, 0);
		base.Controls.SetChildIndex(next, 0);
		base.Controls.SetChildIndex(numzero, 0);
		base.Controls.SetChildIndex(numtwo, 0);
		base.Controls.SetChildIndex(numthree, 0);
		base.Controls.SetChildIndex(numfour, 0);
		base.Controls.SetChildIndex(numsix, 0);
		base.Controls.SetChildIndex(numfive, 0);
		base.Controls.SetChildIndex(numseven, 0);
		base.Controls.SetChildIndex(numeight, 0);
		base.Controls.SetChildIndex(numnine, 0);
		base.Controls.SetChildIndex(removeall, 0);
		base.Controls.SetChildIndex(btn_MedDescription, 0);
		base.Controls.SetChildIndex(commoditySearch, 0);
		base.Controls.SetChildIndex(cureGuide, 0);
		base.Controls.SetChildIndex(button1, 0);
		base.Controls.SetChildIndex(Checkout, 0);
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).EndInit();
		((System.ComponentModel.ISupportInitialize)infolist).EndInit();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		flp_chooseMember.ResumeLayout(false);
		flp_chooseMember.PerformLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		ResumeLayout(false);
	}
}
