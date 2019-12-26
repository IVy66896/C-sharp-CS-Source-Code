using BarcodeLib;
using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class printsimple : Form
{
	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private Panel panel4;

	private Panel Hsno;

	private Label label1;

	private Label label2;

	private Panel panel6;

	private Label label3;

	private Panel buytime;

	private Panel panel8;

	private Label label4;

	private Panel panel9;

	private Panel panel10;

	private Label label5;

	private Panel panel11;

	private Panel panel12;

	private Label label6;

	private Panel panel13;

	private Panel panel14;

	private Label label7;

	private Panel panel15;

	private PictureBox barcodeimg;

	private Label shopname;

	private Label label8;

	private Label label9;

	private Label MemName;

	private Label age;

	private Label tel;

	private Label label10;

	private Label label11;

	private Panel panel5;

	private Panel panel7;

	private Label itemsnum;

	private Panel panel16;

	private Label label12;

	private Panel panel17;

	private Label totalnum;

	private TableLayoutPanel tableLayoutPanel1;

	public printsimple()
	{
		InitializeComponent();
		tableLayoutPanel1.RowCount = 1;
		Label label = new Label
		{
			Text = "商品名稱"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 1, 0);
		label = new Label
		{
			Text = "數量"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 2, 0);
		label = new Label
		{
			Text = "用藥範圍"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 3, 0);
	}

	public printsimple(frmMainShopSimple frs, DataGridView infolist)
	{
		InitializeComponent();
		string text = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
		string[,] strFieldArray = new string[11, 2]
		{
			{
				"sellNo",
				frs.getHseqNo()
			},
			{
				"sellTime",
				text
			},
			{
				"memberId",
				"test"
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
				frs.gettotalprice()
			},
			{
				"itemstotal",
				frs.gettotalpriceDiscount()
			},
			{
				"status",
				"1"
			}
		};
		DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_main_sell", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
		for (int i = 0; i < infolist.Rows.Count; i++)
		{
			strFieldArray = new string[10, 2]
			{
				{
					"sellNo",
					frs.getHseqNo()
				},
				{
					"barcode",
					infolist.Rows[i].Cells[3].Value.ToString()
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
					infolist.Rows[i].Cells[2].Value.ToString()
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
					"test"
				},
				{
					"BLNO",
					"test2"
				}
			};
			DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_detail_sell", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
		}
		MessageBox.Show("新增成功");
		Barcode barcode = new Barcode
		{
			IncludeLabel = true,
			LabelFont = new Font("Verdana", 8f),
			Width = 120,
			Height = 60
		};
		Image image = barcode.Encode(TYPE.CODE128, frs.getHseqNo(), barcode.Width, barcode.Height);
		barcodeimg.Image = image;
		label8.Text = frs.getHseqNo();
		label9.Text = text;
		itemsnum.Text = frs.gettotalprice();
		totalnum.Text = frs.gettotalpriceDiscount();
		tableLayoutPanel1.RowCount = 1;
		Label label = new Label
		{
			Text = "商品名稱"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 1, 0);
		label = new Label
		{
			Text = "數量"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 2, 0);
		label = new Label
		{
			Text = "用藥範圍"
		};
		label.Anchor = AnchorStyles.None;
		tableLayoutPanel1.Controls.Add(label, 3, 0);
		for (int j = 0; j < infolist.Rows.Count; j++)
		{
			tableLayoutPanel1.RowCount += 1;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			label = new Label
			{
				Text = infolist.Rows[j].Cells[0].Value.ToString()
			};
			label.Anchor = AnchorStyles.None;
			tableLayoutPanel1.Controls.Add(label, 0, tableLayoutPanel1.RowCount - 1);
			label = new Label
			{
				Text = infolist.Rows[j].Cells[1].Value.ToString()
			};
			label.Anchor = AnchorStyles.None;
			tableLayoutPanel1.Controls.Add(label, 1, tableLayoutPanel1.RowCount - 1);
			label = new Label
			{
				Text = infolist.Rows[j].Cells[2].Value.ToString()
			};
			label.Anchor = AnchorStyles.None;
			tableLayoutPanel1.Controls.Add(label, 2, tableLayoutPanel1.RowCount - 1);
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
		panel1 = new System.Windows.Forms.Panel();
		barcodeimg = new System.Windows.Forms.PictureBox();
		panel3 = new System.Windows.Forms.Panel();
		shopname = new System.Windows.Forms.Label();
		panel2 = new System.Windows.Forms.Panel();
		label1 = new System.Windows.Forms.Label();
		panel4 = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		Hsno = new System.Windows.Forms.Panel();
		label8 = new System.Windows.Forms.Label();
		panel6 = new System.Windows.Forms.Panel();
		label3 = new System.Windows.Forms.Label();
		buytime = new System.Windows.Forms.Panel();
		label9 = new System.Windows.Forms.Label();
		panel8 = new System.Windows.Forms.Panel();
		label4 = new System.Windows.Forms.Label();
		panel9 = new System.Windows.Forms.Panel();
		MemName = new System.Windows.Forms.Label();
		panel10 = new System.Windows.Forms.Panel();
		label5 = new System.Windows.Forms.Label();
		panel11 = new System.Windows.Forms.Panel();
		age = new System.Windows.Forms.Label();
		panel12 = new System.Windows.Forms.Panel();
		label6 = new System.Windows.Forms.Label();
		panel13 = new System.Windows.Forms.Panel();
		tel = new System.Windows.Forms.Label();
		panel14 = new System.Windows.Forms.Panel();
		label7 = new System.Windows.Forms.Label();
		panel15 = new System.Windows.Forms.Panel();
		label10 = new System.Windows.Forms.Label();
		label11 = new System.Windows.Forms.Label();
		panel5 = new System.Windows.Forms.Panel();
		panel7 = new System.Windows.Forms.Panel();
		itemsnum = new System.Windows.Forms.Label();
		panel16 = new System.Windows.Forms.Panel();
		label12 = new System.Windows.Forms.Label();
		panel17 = new System.Windows.Forms.Panel();
		totalnum = new System.Windows.Forms.Label();
		tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)barcodeimg).BeginInit();
		panel3.SuspendLayout();
		panel2.SuspendLayout();
		panel4.SuspendLayout();
		Hsno.SuspendLayout();
		panel6.SuspendLayout();
		buytime.SuspendLayout();
		panel8.SuspendLayout();
		panel9.SuspendLayout();
		panel10.SuspendLayout();
		panel11.SuspendLayout();
		panel12.SuspendLayout();
		panel13.SuspendLayout();
		panel14.SuspendLayout();
		panel15.SuspendLayout();
		panel5.SuspendLayout();
		panel7.SuspendLayout();
		panel16.SuspendLayout();
		panel17.SuspendLayout();
		SuspendLayout();
		panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel1.Controls.Add(barcodeimg);
		panel1.Location = new System.Drawing.Point(32, 12);
		panel1.Name = "panel1";
		panel1.Size = new System.Drawing.Size(158, 204);
		panel1.TabIndex = 0;
		barcodeimg.Location = new System.Drawing.Point(5, 9);
		barcodeimg.Name = "barcodeimg";
		barcodeimg.Size = new System.Drawing.Size(146, 182);
		barcodeimg.TabIndex = 8;
		barcodeimg.TabStop = false;
		panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel3.Controls.Add(shopname);
		panel3.Location = new System.Drawing.Point(307, 12);
		panel3.Name = "panel3";
		panel3.Size = new System.Drawing.Size(250, 34);
		panel3.TabIndex = 2;
		shopname.AutoSize = true;
		shopname.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		shopname.Location = new System.Drawing.Point(6, 10);
		shopname.Name = "shopname";
		shopname.Size = new System.Drawing.Size(78, 20);
		shopname.TabIndex = 0;
		shopname.Text = "銷售店家!";
		panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel2.Controls.Add(label1);
		panel2.Location = new System.Drawing.Point(188, 12);
		panel2.Name = "panel2";
		panel2.Size = new System.Drawing.Size(122, 34);
		panel2.TabIndex = 1;
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(45, 10);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(74, 21);
		label1.TabIndex = 0;
		label1.Text = "銷售店家";
		panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel4.Controls.Add(label2);
		panel4.Location = new System.Drawing.Point(188, 46);
		panel4.Name = "panel4";
		panel4.Size = new System.Drawing.Size(122, 34);
		panel4.TabIndex = 2;
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Location = new System.Drawing.Point(44, 7);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(74, 21);
		label2.TabIndex = 1;
		label2.Text = "銷售單號";
		Hsno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		Hsno.Controls.Add(label8);
		Hsno.Location = new System.Drawing.Point(307, 46);
		Hsno.Name = "Hsno";
		Hsno.Size = new System.Drawing.Size(250, 34);
		Hsno.TabIndex = 3;
		label8.AutoSize = true;
		label8.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.Location = new System.Drawing.Point(6, 7);
		label8.Name = "label8";
		label8.Size = new System.Drawing.Size(73, 20);
		label8.TabIndex = 1;
		label8.Text = "銷售單號";
		panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel6.Controls.Add(label3);
		panel6.Location = new System.Drawing.Point(187, 80);
		panel6.Name = "panel6";
		panel6.Size = new System.Drawing.Size(122, 34);
		panel6.TabIndex = 3;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label3.Location = new System.Drawing.Point(45, 8);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(74, 21);
		label3.TabIndex = 2;
		label3.Text = "銷售時間";
		buytime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		buytime.Controls.Add(label9);
		buytime.Location = new System.Drawing.Point(308, 79);
		buytime.Name = "buytime";
		buytime.Size = new System.Drawing.Size(249, 34);
		buytime.TabIndex = 4;
		label9.AutoSize = true;
		label9.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label9.Location = new System.Drawing.Point(5, 9);
		label9.Name = "label9";
		label9.Size = new System.Drawing.Size(82, 20);
		label9.TabIndex = 2;
		label9.Text = "銷售時間1";
		panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel8.Controls.Add(label4);
		panel8.Location = new System.Drawing.Point(188, 114);
		panel8.Name = "panel8";
		panel8.Size = new System.Drawing.Size(122, 34);
		panel8.TabIndex = 4;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label4.Location = new System.Drawing.Point(44, 9);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(74, 21);
		label4.TabIndex = 3;
		label4.Text = "會員姓名";
		panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel9.Controls.Add(MemName);
		panel9.Location = new System.Drawing.Point(309, 113);
		panel9.Name = "panel9";
		panel9.Size = new System.Drawing.Size(248, 34);
		panel9.TabIndex = 5;
		MemName.AutoSize = true;
		MemName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		MemName.Location = new System.Drawing.Point(4, 10);
		MemName.Name = "MemName";
		MemName.Size = new System.Drawing.Size(82, 20);
		MemName.TabIndex = 3;
		MemName.Text = "會員姓名1";
		panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel10.Controls.Add(label5);
		panel10.Location = new System.Drawing.Point(190, 148);
		panel10.Name = "panel10";
		panel10.Size = new System.Drawing.Size(122, 34);
		panel10.TabIndex = 5;
		label5.AutoSize = true;
		label5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label5.Location = new System.Drawing.Point(71, 10);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(42, 21);
		label5.TabIndex = 4;
		label5.Text = "年齡";
		panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel11.Controls.Add(age);
		panel11.Location = new System.Drawing.Point(309, 147);
		panel11.Name = "panel11";
		panel11.Size = new System.Drawing.Size(53, 34);
		panel11.TabIndex = 6;
		age.AutoSize = true;
		age.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		age.Location = new System.Drawing.Point(5, 6);
		age.Name = "age";
		age.Size = new System.Drawing.Size(41, 19);
		age.TabIndex = 3;
		age.Text = "年齡";
		panel12.Controls.Add(label6);
		panel12.Location = new System.Drawing.Point(356, 147);
		panel12.Name = "panel12";
		panel12.Size = new System.Drawing.Size(79, 34);
		panel12.TabIndex = 7;
		label6.AutoSize = true;
		label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label6.Location = new System.Drawing.Point(5, 7);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(74, 21);
		label6.TabIndex = 4;
		label6.Text = "聯絡電話";
		panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel13.Controls.Add(tel);
		panel13.Location = new System.Drawing.Point(435, 147);
		panel13.Name = "panel13";
		panel13.Size = new System.Drawing.Size(122, 34);
		panel13.TabIndex = 6;
		tel.AutoSize = true;
		tel.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		tel.Location = new System.Drawing.Point(3, 8);
		tel.Name = "tel";
		tel.Size = new System.Drawing.Size(89, 19);
		tel.TabIndex = 4;
		tel.Text = "0900000000";
		panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel14.Controls.Add(label7);
		panel14.Location = new System.Drawing.Point(190, 182);
		panel14.Name = "panel14";
		panel14.Size = new System.Drawing.Size(122, 34);
		panel14.TabIndex = 6;
		label7.AutoSize = true;
		label7.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label7.Location = new System.Drawing.Point(69, 9);
		label7.Name = "label7";
		label7.Size = new System.Drawing.Size(42, 21);
		label7.TabIndex = 5;
		label7.Text = "地址";
		panel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel15.Controls.Add(label10);
		panel15.Location = new System.Drawing.Point(310, 181);
		panel15.Name = "panel15";
		panel15.Size = new System.Drawing.Size(247, 34);
		panel15.TabIndex = 6;
		label10.AutoSize = true;
		label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.Location = new System.Drawing.Point(3, 10);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(41, 20);
		label10.TabIndex = 4;
		label10.Text = "地址";
		label11.AutoSize = true;
		label11.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label11.Location = new System.Drawing.Point(69, 9);
		label11.Name = "label11";
		label11.Size = new System.Drawing.Size(42, 21);
		label11.TabIndex = 5;
		label11.Text = "品項";
		panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel5.Controls.Add(label11);
		panel5.Location = new System.Drawing.Point(32, 376);
		panel5.Name = "panel5";
		panel5.Size = new System.Drawing.Size(122, 34);
		panel5.TabIndex = 7;
		panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel7.Controls.Add(itemsnum);
		panel7.Location = new System.Drawing.Point(154, 376);
		panel7.Name = "panel7";
		panel7.Size = new System.Drawing.Size(130, 34);
		panel7.TabIndex = 8;
		itemsnum.AutoSize = true;
		itemsnum.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		itemsnum.Location = new System.Drawing.Point(3, 9);
		itemsnum.Name = "itemsnum";
		itemsnum.Size = new System.Drawing.Size(74, 21);
		itemsnum.TabIndex = 5;
		itemsnum.Text = "品項數量";
		panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel16.Controls.Add(label12);
		panel16.Location = new System.Drawing.Point(282, 376);
		panel16.Name = "panel16";
		panel16.Size = new System.Drawing.Size(114, 34);
		panel16.TabIndex = 8;
		label12.AutoSize = true;
		label12.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label12.Location = new System.Drawing.Point(69, 9);
		label12.Name = "label12";
		label12.Size = new System.Drawing.Size(42, 21);
		label12.TabIndex = 5;
		label12.Text = "數量";
		panel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		panel17.Controls.Add(totalnum);
		panel17.Location = new System.Drawing.Point(396, 376);
		panel17.Name = "panel17";
		panel17.Size = new System.Drawing.Size(149, 34);
		panel17.TabIndex = 9;
		totalnum.AutoSize = true;
		totalnum.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		totalnum.Location = new System.Drawing.Point(3, 9);
		totalnum.Name = "totalnum";
		totalnum.Size = new System.Drawing.Size(58, 21);
		totalnum.TabIndex = 5;
		totalnum.Text = "總數量";
		tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
		tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
		tableLayoutPanel1.ColumnCount = 4;
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10f));
		tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
		tableLayoutPanel1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tableLayoutPanel1.Location = new System.Drawing.Point(32, 273);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		tableLayoutPanel1.Size = new System.Drawing.Size(513, 60);
		tableLayoutPanel1.TabIndex = 8;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(579, 750);
		base.Controls.Add(panel17);
		base.Controls.Add(panel16);
		base.Controls.Add(panel7);
		base.Controls.Add(panel5);
		base.Controls.Add(tableLayoutPanel1);
		base.Controls.Add(panel15);
		base.Controls.Add(panel14);
		base.Controls.Add(panel13);
		base.Controls.Add(panel12);
		base.Controls.Add(panel11);
		base.Controls.Add(panel10);
		base.Controls.Add(panel9);
		base.Controls.Add(panel8);
		base.Controls.Add(buytime);
		base.Controls.Add(panel6);
		base.Controls.Add(Hsno);
		base.Controls.Add(panel4);
		base.Controls.Add(panel3);
		base.Controls.Add(panel2);
		base.Controls.Add(panel1);
		base.Name = "printsimple";
		Text = "Form1";
		panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)barcodeimg).EndInit();
		panel3.ResumeLayout(false);
		panel3.PerformLayout();
		panel2.ResumeLayout(false);
		panel2.PerformLayout();
		panel4.ResumeLayout(false);
		panel4.PerformLayout();
		Hsno.ResumeLayout(false);
		Hsno.PerformLayout();
		panel6.ResumeLayout(false);
		panel6.PerformLayout();
		buytime.ResumeLayout(false);
		buytime.PerformLayout();
		panel8.ResumeLayout(false);
		panel8.PerformLayout();
		panel9.ResumeLayout(false);
		panel9.PerformLayout();
		panel10.ResumeLayout(false);
		panel10.PerformLayout();
		panel11.ResumeLayout(false);
		panel11.PerformLayout();
		panel12.ResumeLayout(false);
		panel12.PerformLayout();
		panel13.ResumeLayout(false);
		panel13.PerformLayout();
		panel14.ResumeLayout(false);
		panel14.PerformLayout();
		panel15.ResumeLayout(false);
		panel15.PerformLayout();
		panel5.ResumeLayout(false);
		panel5.PerformLayout();
		panel7.ResumeLayout(false);
		panel7.PerformLayout();
		panel16.ResumeLayout(false);
		panel16.PerformLayout();
		panel17.ResumeLayout(false);
		panel17.PerformLayout();
		ResumeLayout(false);
	}
}
