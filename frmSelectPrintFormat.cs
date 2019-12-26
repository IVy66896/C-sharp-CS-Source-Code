using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmSelectPrintFormat : Form
{
	private PrintFormat _selectedPrintFormat;

	private RadioButton[] _RB;

	private IContainer components;

	private Panel panelPrint;

	private Button btnCancel;

	private Button btnChangeSetting;

	private RadioButton radioButton1;

	private RadioButton radioButton4;

	private RadioButton radioButton3;

	private RadioButton radioButton2;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	private PictureBox pictureBox4;

	private PictureBox pictureBox5;

	private RadioButton radioButton5;

	private PictureBox pictureBox6;

	private RadioButton radioButton6;

	private PictureBox pictureBox7;

	private RadioButton radioButton7;

	private RadioButton radioButton8;

	private Button btnJetPrintSetting;

	private PictureBox pictureBox8;

	private RadioButton radioButton9;

	private RadioButton radioButton10;

	private PictureBox pictureBox9;

	private RadioButton radioButton11;

	private PictureBox pictureBox10;

	private RadioButton radioButton20;

	private RadioButton radioButton19;

	private RadioButton radioButton18;

	private RadioButton radioButton17;

	private RadioButton radioButton16;

	private RadioButton radioButton15;

	private RadioButton radioButton14;

	private RadioButton radioButton13;

	private RadioButton radioButton12;

	private Button button1;

	public PrintFormat PrintFormatSelected
	{
		get
		{
			return _selectedPrintFormat;
		}
		set
		{
		}
	}

	public frmSelectPrintFormat(PrintFormat CurrentPrintFormat)
	{
		InitializeComponent();
		_RB = new RadioButton[20]
		{
			radioButton1,
			radioButton2,
			radioButton3,
			radioButton4,
			radioButton5,
			radioButton6,
			radioButton7,
			radioButton8,
			radioButton9,
			radioButton10,
			radioButton11,
			radioButton12,
			radioButton13,
			radioButton14,
			radioButton15,
			radioButton16,
			radioButton17,
			radioButton18,
			radioButton19,
			radioButton20
		};
		for (int i = 0; i < _RB.Length; i++)
		{
			_RB[i].Text = PrintCodeUtilties.PrtFmtShowName((PrintFormat)i);
			if (i == Convert.ToInt32(CurrentPrintFormat))
			{
				_RB[i].Checked = true;
			}
		}
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < _RB.Length; i++)
			{
				if (_RB[i].Checked)
				{
					_selectedPrintFormat = (PrintFormat)i;
					break;
				}
			}
			if (_selectedPrintFormat == PrintFormat.噴墨列印 && (Program.UserSettings.JetPrintSettings.JetExeDefaultPath == string.Empty || Program.UserSettings.JetPrintSettings.JetExecPaths.Count == 0 || Program.UserSettings.JetPrintSettings.MaxPerFile < 1 || Program.UserSettings.JetPrintSettings.PrintFormatter == string.Empty))
			{
				MessageBox.Show("噴墨列印設定尚未完成，請重新設定。");
			}
			else
			{
				base.DialogResult = DialogResult.OK;
				Close();
			}
		}
		catch (Exception)
		{
			Close();
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnJetPrintSetting_Click(object sender, EventArgs e)
	{
		new frmJetPrintSetting().ShowDialog(this);
		_ = 1;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		frmViewParameter frmViewParameter = new frmViewParameter();
		frmViewParameter.ShowDialog();
		frmViewParameter.Dispose();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmSelectPrintFormat));
		panelPrint = new System.Windows.Forms.Panel();
		button1 = new System.Windows.Forms.Button();
		radioButton20 = new System.Windows.Forms.RadioButton();
		radioButton19 = new System.Windows.Forms.RadioButton();
		radioButton18 = new System.Windows.Forms.RadioButton();
		radioButton17 = new System.Windows.Forms.RadioButton();
		radioButton16 = new System.Windows.Forms.RadioButton();
		radioButton15 = new System.Windows.Forms.RadioButton();
		radioButton14 = new System.Windows.Forms.RadioButton();
		radioButton13 = new System.Windows.Forms.RadioButton();
		radioButton12 = new System.Windows.Forms.RadioButton();
		radioButton11 = new System.Windows.Forms.RadioButton();
		radioButton10 = new System.Windows.Forms.RadioButton();
		pictureBox10 = new System.Windows.Forms.PictureBox();
		pictureBox9 = new System.Windows.Forms.PictureBox();
		pictureBox8 = new System.Windows.Forms.PictureBox();
		radioButton9 = new System.Windows.Forms.RadioButton();
		btnJetPrintSetting = new System.Windows.Forms.Button();
		radioButton8 = new System.Windows.Forms.RadioButton();
		pictureBox7 = new System.Windows.Forms.PictureBox();
		radioButton7 = new System.Windows.Forms.RadioButton();
		pictureBox6 = new System.Windows.Forms.PictureBox();
		radioButton6 = new System.Windows.Forms.RadioButton();
		pictureBox5 = new System.Windows.Forms.PictureBox();
		radioButton5 = new System.Windows.Forms.RadioButton();
		pictureBox4 = new System.Windows.Forms.PictureBox();
		pictureBox3 = new System.Windows.Forms.PictureBox();
		pictureBox2 = new System.Windows.Forms.PictureBox();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		radioButton4 = new System.Windows.Forms.RadioButton();
		radioButton3 = new System.Windows.Forms.RadioButton();
		radioButton2 = new System.Windows.Forms.RadioButton();
		radioButton1 = new System.Windows.Forms.RadioButton();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		panelPrint.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(button1);
		panelPrint.Controls.Add(radioButton20);
		panelPrint.Controls.Add(radioButton19);
		panelPrint.Controls.Add(radioButton18);
		panelPrint.Controls.Add(radioButton17);
		panelPrint.Controls.Add(radioButton16);
		panelPrint.Controls.Add(radioButton15);
		panelPrint.Controls.Add(radioButton14);
		panelPrint.Controls.Add(radioButton13);
		panelPrint.Controls.Add(radioButton12);
		panelPrint.Controls.Add(radioButton11);
		panelPrint.Controls.Add(radioButton10);
		panelPrint.Controls.Add(pictureBox10);
		panelPrint.Controls.Add(pictureBox9);
		panelPrint.Controls.Add(pictureBox8);
		panelPrint.Controls.Add(radioButton9);
		panelPrint.Controls.Add(btnJetPrintSetting);
		panelPrint.Controls.Add(radioButton8);
		panelPrint.Controls.Add(pictureBox7);
		panelPrint.Controls.Add(radioButton7);
		panelPrint.Controls.Add(pictureBox6);
		panelPrint.Controls.Add(radioButton6);
		panelPrint.Controls.Add(pictureBox5);
		panelPrint.Controls.Add(radioButton5);
		panelPrint.Controls.Add(pictureBox4);
		panelPrint.Controls.Add(pictureBox3);
		panelPrint.Controls.Add(pictureBox2);
		panelPrint.Controls.Add(pictureBox1);
		panelPrint.Controls.Add(radioButton4);
		panelPrint.Controls.Add(radioButton3);
		panelPrint.Controls.Add(radioButton2);
		panelPrint.Controls.Add(radioButton1);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Location = new System.Drawing.Point(3, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(841, 800);
		panelPrint.TabIndex = 23;
		button1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		button1.Location = new System.Drawing.Point(274, 755);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(80, 31);
		button1.TabIndex = 56;
		button1.Text = "預覽";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		radioButton20.AutoSize = true;
		radioButton20.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton20.Location = new System.Drawing.Point(582, 721);
		radioButton20.Name = "radioButton20";
		radioButton20.Size = new System.Drawing.Size(195, 23);
		radioButton20.TabIndex = 55;
		radioButton20.TabStop = true;
		radioButton20.Text = "75X42 含生產者資訊";
		radioButton20.UseVisualStyleBackColor = true;
		radioButton19.AutoSize = true;
		radioButton19.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton19.Location = new System.Drawing.Point(295, 721);
		radioButton19.Name = "radioButton19";
		radioButton19.Size = new System.Drawing.Size(190, 23);
		radioButton19.TabIndex = 54;
		radioButton19.TabStop = true;
		radioButton19.Text = "45X52含生產者資訊";
		radioButton19.UseVisualStyleBackColor = true;
		radioButton18.AutoSize = true;
		radioButton18.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton18.Location = new System.Drawing.Point(71, 721);
		radioButton18.Name = "radioButton18";
		radioButton18.Size = new System.Drawing.Size(76, 23);
		radioButton18.TabIndex = 53;
		radioButton18.TabStop = true;
		radioButton18.Text = "75X42";
		radioButton18.UseVisualStyleBackColor = true;
		radioButton17.AutoSize = true;
		radioButton17.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton17.Location = new System.Drawing.Point(460, 493);
		radioButton17.Name = "radioButton17";
		radioButton17.Size = new System.Drawing.Size(163, 23);
		radioButton17.TabIndex = 52;
		radioButton17.TabStop = true;
		radioButton17.Text = "45X52(無EAN碼)";
		radioButton17.UseVisualStyleBackColor = true;
		radioButton16.AutoSize = true;
		radioButton16.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton16.Location = new System.Drawing.Point(263, 493);
		radioButton16.Name = "radioButton16";
		radioButton16.Size = new System.Drawing.Size(76, 23);
		radioButton16.TabIndex = 51;
		radioButton16.TabStop = true;
		radioButton16.Text = "75X42";
		radioButton16.UseVisualStyleBackColor = true;
		radioButton15.AutoSize = true;
		radioButton15.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton15.Location = new System.Drawing.Point(42, 493);
		radioButton15.Name = "radioButton15";
		radioButton15.Size = new System.Drawing.Size(103, 23);
		radioButton15.TabIndex = 50;
		radioButton15.TabStop = true;
		radioButton15.Text = "畜產標籤";
		radioButton15.UseVisualStyleBackColor = true;
		radioButton14.AutoSize = true;
		radioButton14.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton14.Location = new System.Drawing.Point(409, 232);
		radioButton14.Name = "radioButton14";
		radioButton14.Size = new System.Drawing.Size(163, 23);
		radioButton14.TabIndex = 49;
		radioButton14.TabStop = true;
		radioButton14.Text = "45X52(無EAN碼)";
		radioButton14.UseVisualStyleBackColor = true;
		radioButton13.AutoSize = true;
		radioButton13.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton13.Location = new System.Drawing.Point(189, 232);
		radioButton13.Name = "radioButton13";
		radioButton13.Size = new System.Drawing.Size(202, 23);
		radioButton13.TabIndex = 48;
		radioButton13.TabStop = true;
		radioButton13.Text = "45X52(顯示保存日期)";
		radioButton13.UseVisualStyleBackColor = true;
		radioButton12.AutoSize = true;
		radioButton12.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton12.Location = new System.Drawing.Point(12, 232);
		radioButton12.Name = "radioButton12";
		radioButton12.Size = new System.Drawing.Size(76, 23);
		radioButton12.TabIndex = 47;
		radioButton12.TabStop = true;
		radioButton12.Text = "45X52";
		radioButton12.UseVisualStyleBackColor = true;
		radioButton11.AutoSize = true;
		radioButton11.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton11.Location = new System.Drawing.Point(583, 692);
		radioButton11.Name = "radioButton11";
		radioButton11.Size = new System.Drawing.Size(195, 23);
		radioButton11.TabIndex = 46;
		radioButton11.TabStop = true;
		radioButton11.Text = "75X42 含生產者資訊";
		radioButton11.UseVisualStyleBackColor = true;
		radioButton10.AutoSize = true;
		radioButton10.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton10.Location = new System.Drawing.Point(295, 692);
		radioButton10.Name = "radioButton10";
		radioButton10.Size = new System.Drawing.Size(190, 23);
		radioButton10.TabIndex = 46;
		radioButton10.TabStop = true;
		radioButton10.Text = "45X52含生產者資訊";
		radioButton10.UseVisualStyleBackColor = true;
		pictureBox10.Image = (System.Drawing.Image)resources.GetObject("pictureBox10.Image");
		pictureBox10.Location = new System.Drawing.Point(550, 523);
		pictureBox10.Name = "pictureBox10";
		pictureBox10.Size = new System.Drawing.Size(242, 170);
		pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox10.TabIndex = 45;
		pictureBox10.TabStop = false;
		pictureBox9.Image = (System.Drawing.Image)resources.GetObject("pictureBox9.Image");
		pictureBox9.Location = new System.Drawing.Point(299, 535);
		pictureBox9.Name = "pictureBox9";
		pictureBox9.Size = new System.Drawing.Size(170, 151);
		pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox9.TabIndex = 45;
		pictureBox9.TabStop = false;
		pictureBox8.Image = (System.Drawing.Image)resources.GetObject("pictureBox8.Image");
		pictureBox8.Location = new System.Drawing.Point(4, 556);
		pictureBox8.Name = "pictureBox8";
		pictureBox8.Size = new System.Drawing.Size(242, 130);
		pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox8.TabIndex = 44;
		pictureBox8.TabStop = false;
		radioButton9.AutoSize = true;
		radioButton9.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton9.Location = new System.Drawing.Point(71, 692);
		radioButton9.Name = "radioButton9";
		radioButton9.Size = new System.Drawing.Size(76, 23);
		radioButton9.TabIndex = 43;
		radioButton9.TabStop = true;
		radioButton9.Text = "75X42";
		radioButton9.UseVisualStyleBackColor = true;
		btnJetPrintSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnJetPrintSetting.Location = new System.Drawing.Point(661, 427);
		btnJetPrintSetting.Name = "btnJetPrintSetting";
		btnJetPrintSetting.Size = new System.Drawing.Size(120, 31);
		btnJetPrintSetting.TabIndex = 42;
		btnJetPrintSetting.Text = "噴墨列印設定";
		btnJetPrintSetting.UseVisualStyleBackColor = true;
		btnJetPrintSetting.Click += new System.EventHandler(btnJetPrintSetting_Click);
		radioButton8.AutoSize = true;
		radioButton8.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton8.Location = new System.Drawing.Point(661, 464);
		radioButton8.Name = "radioButton8";
		radioButton8.Size = new System.Drawing.Size(103, 23);
		radioButton8.TabIndex = 41;
		radioButton8.TabStop = true;
		radioButton8.Text = "噴墨列印";
		radioButton8.UseVisualStyleBackColor = true;
		pictureBox7.Image = (System.Drawing.Image)resources.GetObject("pictureBox7.Image");
		pictureBox7.Location = new System.Drawing.Point(454, 269);
		pictureBox7.Name = "pictureBox7";
		pictureBox7.Size = new System.Drawing.Size(170, 189);
		pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		pictureBox7.TabIndex = 40;
		pictureBox7.TabStop = false;
		radioButton7.AutoSize = true;
		radioButton7.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton7.Location = new System.Drawing.Point(460, 464);
		radioButton7.Name = "radioButton7";
		radioButton7.Size = new System.Drawing.Size(163, 23);
		radioButton7.TabIndex = 39;
		radioButton7.TabStop = true;
		radioButton7.Text = "45X52(無EAN碼)";
		radioButton7.UseVisualStyleBackColor = true;
		pictureBox6.Image = (System.Drawing.Image)resources.GetObject("pictureBox6.Image");
		pictureBox6.Location = new System.Drawing.Point(196, 328);
		pictureBox6.Name = "pictureBox6";
		pictureBox6.Size = new System.Drawing.Size(242, 130);
		pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox6.TabIndex = 38;
		pictureBox6.TabStop = false;
		radioButton6.AutoSize = true;
		radioButton6.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton6.Location = new System.Drawing.Point(263, 464);
		radioButton6.Name = "radioButton6";
		radioButton6.Size = new System.Drawing.Size(76, 23);
		radioButton6.TabIndex = 37;
		radioButton6.TabStop = true;
		radioButton6.Text = "75X42";
		radioButton6.UseVisualStyleBackColor = true;
		pictureBox5.Image = (System.Drawing.Image)resources.GetObject("pictureBox5.Image");
		pictureBox5.Location = new System.Drawing.Point(7, 307);
		pictureBox5.Name = "pictureBox5";
		pictureBox5.Size = new System.Drawing.Size(170, 151);
		pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pictureBox5.TabIndex = 36;
		pictureBox5.TabStop = false;
		radioButton5.AutoSize = true;
		radioButton5.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton5.Location = new System.Drawing.Point(42, 464);
		radioButton5.Name = "radioButton5";
		radioButton5.Size = new System.Drawing.Size(103, 23);
		radioButton5.TabIndex = 35;
		radioButton5.TabStop = true;
		radioButton5.Text = "畜產標籤";
		radioButton5.UseVisualStyleBackColor = true;
		pictureBox4.Image = (System.Drawing.Image)resources.GetObject("pictureBox4.Image");
		pictureBox4.Location = new System.Drawing.Point(12, 8);
		pictureBox4.Name = "pictureBox4";
		pictureBox4.Size = new System.Drawing.Size(170, 189);
		pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		pictureBox4.TabIndex = 34;
		pictureBox4.TabStop = false;
		pictureBox3.Image = (System.Drawing.Image)resources.GetObject("pictureBox3.Image");
		pictureBox3.Location = new System.Drawing.Point(587, 66);
		pictureBox3.Name = "pictureBox3";
		pictureBox3.Size = new System.Drawing.Size(242, 130);
		pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		pictureBox3.TabIndex = 33;
		pictureBox3.TabStop = false;
		pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		pictureBox2.Location = new System.Drawing.Point(196, 8);
		pictureBox2.Name = "pictureBox2";
		pictureBox2.Size = new System.Drawing.Size(170, 189);
		pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		pictureBox2.TabIndex = 32;
		pictureBox2.TabStop = false;
		pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		pictureBox1.Location = new System.Drawing.Point(402, 7);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(170, 189);
		pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		pictureBox1.TabIndex = 31;
		pictureBox1.TabStop = false;
		radioButton4.AutoSize = true;
		radioButton4.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton4.Location = new System.Drawing.Point(409, 203);
		radioButton4.Name = "radioButton4";
		radioButton4.Size = new System.Drawing.Size(163, 23);
		radioButton4.TabIndex = 30;
		radioButton4.TabStop = true;
		radioButton4.Text = "45X52(無EAN碼)";
		radioButton4.UseVisualStyleBackColor = true;
		radioButton3.AutoSize = true;
		radioButton3.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton3.Location = new System.Drawing.Point(661, 202);
		radioButton3.Name = "radioButton3";
		radioButton3.Size = new System.Drawing.Size(76, 23);
		radioButton3.TabIndex = 29;
		radioButton3.TabStop = true;
		radioButton3.Text = "56X30";
		radioButton3.UseVisualStyleBackColor = true;
		radioButton2.AutoSize = true;
		radioButton2.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton2.Location = new System.Drawing.Point(189, 203);
		radioButton2.Name = "radioButton2";
		radioButton2.Size = new System.Drawing.Size(202, 23);
		radioButton2.TabIndex = 28;
		radioButton2.TabStop = true;
		radioButton2.Text = "45X52(顯示保存日期)";
		radioButton2.UseVisualStyleBackColor = true;
		radioButton1.AutoSize = true;
		radioButton1.Font = new System.Drawing.Font("新細明體", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		radioButton1.Location = new System.Drawing.Point(12, 202);
		radioButton1.Name = "radioButton1";
		radioButton1.Size = new System.Drawing.Size(76, 23);
		radioButton1.TabIndex = 27;
		radioButton1.TabStop = true;
		radioButton1.Text = "45X52";
		radioButton1.UseVisualStyleBackColor = true;
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(442, 755);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 24;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(358, 755);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 23;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(847, 698);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmSelectPrintFormat";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "選擇標籤列印樣式";
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		ResumeLayout(false);
	}
}
