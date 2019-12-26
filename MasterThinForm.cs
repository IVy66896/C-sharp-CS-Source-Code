using KeyboardClassLibrary;
using POS_Client;
using POS_Client.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

public class MasterThinForm : Form
{
	public string frmName = "";

	private const int WS_SYSMENU = 524288;

	private IContainer components;

	protected Marquee marquee1;

	private Label l_Name;

	private Panel topPanel;

	private Label l_ReturnMain;

	private Panel pa_keyboard;

	private PictureBox pb_keyboard;

	private Keyboardcontrol keyboardcontrol1;

	private Button btn_down;

	private Button btn_top;

	public PictureBox pb_virtualKeyBoard;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = base.CreateParams;
			createParams.Style &= -524289;
			return createParams;
		}
	}

	public MasterThinForm()
	{
		InitializeComponent();
	}

	public MasterThinForm(string Name)
	{
		InitializeComponent();
		l_Name.Text = Name;
	}

	public void setMasterFormName(string Name)
	{
		l_Name.Text = Name;
	}

	public void backToPreviousForm()
	{
		if (!string.IsNullOrEmpty(frmName))
		{
			object obj = Assembly.GetExecutingAssembly().CreateInstance("POS_Client." + frmName);
			Type type = obj.GetType();
			if ("MasterForm".Equals(type.BaseType.Name))
			{
				switchForm((MasterForm)obj);
			}
			else if ("MasterThinForm".Equals(type.BaseType.Name))
			{
				switchForm((MasterThinForm)obj);
			}
		}
	}

	public void switchForm(MasterThinForm f, Form owner)
	{
		f.frmName = base.Name;
		f.Location = new Point(base.Location.X, base.Location.Y);
		f.Show(owner);
		f.Focus();
		owner.Hide();
	}

	public void switchForm(Form f)
	{
		if ("MasterThinForm".Equals(f.GetType().BaseType.Name))
		{
			switchForm((MasterThinForm)f);
		}
		else if ("MasterForm".Equals(f.GetType().BaseType.Name))
		{
			switchForm((MasterForm)f);
		}
	}

	public void switchForm(MasterThinForm f)
	{
		f.frmName = base.Name;
		f.Location = new Point(base.Location.X, base.Location.Y);
		f.Show();
		f.Focus();
		Dispose();
		Close();
	}

	public void switchForm(MasterForm f, Form owner)
	{
		f.frmName = base.Name;
		f.Location = new Point(base.Location.X, base.Location.Y);
		f.Show(owner);
		f.Focus();
		Dispose();
		Close();
	}

	public void switchForm(MasterForm f)
	{
		f.frmName = base.Name;
		f.Location = new Point(base.Location.X, base.Location.Y);
		f.Show();
		f.Focus();
		Dispose();
		Close();
	}

	private void MasterThinForm_Load(object sender, EventArgs e)
	{
		Text = "農委會防檢局POS系統";
		pb_virtualKeyBoard.BringToFront();
	}

	protected virtual void l_ReturnMain_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("確定放棄所有操作、結束目前功能？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			frmExtendScreen.RemoveAll();
			Program.goodsTemp.Clear();
			Program.goodsWithMoneyTemp.Clear();
			Program.membersTemp.Clear();
			if (base.Owner != null)
			{
				base.Owner.Close();
			}
			switchForm(new frmMain());
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		pa_keyboard.BringToFront();
		if (pa_keyboard.Location.X > 900)
		{
			pa_keyboard.Location = new Point(0, pa_keyboard.Location.Y);
		}
		else
		{
			pa_keyboard.Location = new Point(997, pa_keyboard.Location.Y);
		}
	}

	private void keyboardcontrol1_UserKeyPressed(object sender, KeyboardEventArgs e)
	{
		SendKeys.Send(e.KeyboardKeyPressed);
	}

	private void btn_KeyboardLocation_Click(object sender, EventArgs e)
	{
		if (pa_keyboard.Location.Y > 300)
		{
			pa_keyboard.Location = new Point(pa_keyboard.Location.X, 0);
		}
		else
		{
			pa_keyboard.Location = new Point(pa_keyboard.Location.X, 391);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.MasterThinForm));
		l_Name = new System.Windows.Forms.Label();
		topPanel = new System.Windows.Forms.Panel();
		l_ReturnMain = new System.Windows.Forms.Label();
		pa_keyboard = new System.Windows.Forms.Panel();
		btn_down = new System.Windows.Forms.Button();
		btn_top = new System.Windows.Forms.Button();
		pb_keyboard = new System.Windows.Forms.PictureBox();
		keyboardcontrol1 = new KeyboardClassLibrary.Keyboardcontrol();
		pb_virtualKeyBoard = new System.Windows.Forms.PictureBox();
		topPanel.SuspendLayout();
		pa_keyboard.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)pb_keyboard).BeginInit();
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).BeginInit();
		SuspendLayout();
		l_Name.AutoSize = true;
		l_Name.BackColor = System.Drawing.Color.Transparent;
		l_Name.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_Name.ForeColor = System.Drawing.Color.White;
		l_Name.Location = new System.Drawing.Point(25, 6);
		l_Name.Name = "l_Name";
		l_Name.Size = new System.Drawing.Size(70, 24);
		l_Name.TabIndex = 3;
		l_Name.Text = "NAME";
		topPanel.BackgroundImage = POS_Client.Properties.Resources.inside_top_bg;
		topPanel.Controls.Add(l_ReturnMain);
		topPanel.Controls.Add(l_Name);
		topPanel.Location = new System.Drawing.Point(0, 0);
		topPanel.Margin = new System.Windows.Forms.Padding(0);
		topPanel.Name = "topPanel";
		topPanel.Size = new System.Drawing.Size(981, 35);
		topPanel.TabIndex = 32;
		l_ReturnMain.BackColor = System.Drawing.Color.Transparent;
		l_ReturnMain.Cursor = System.Windows.Forms.Cursors.Hand;
		l_ReturnMain.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_ReturnMain.ForeColor = System.Drawing.Color.White;
		l_ReturnMain.Location = new System.Drawing.Point(906, 6);
		l_ReturnMain.Name = "l_ReturnMain";
		l_ReturnMain.Size = new System.Drawing.Size(72, 25);
		l_ReturnMain.TabIndex = 30;
		l_ReturnMain.Text = "結束";
		l_ReturnMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		l_ReturnMain.Click += new System.EventHandler(l_ReturnMain_Click);
		pa_keyboard.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
		pa_keyboard.Controls.Add(btn_down);
		pa_keyboard.Controls.Add(btn_top);
		pa_keyboard.Controls.Add(pb_keyboard);
		pa_keyboard.Controls.Add(keyboardcontrol1);
		pa_keyboard.Location = new System.Drawing.Point(997, 391);
		pa_keyboard.Name = "pa_keyboard";
		pa_keyboard.Size = new System.Drawing.Size(981, 269);
		pa_keyboard.TabIndex = 50;
		btn_down.Location = new System.Drawing.Point(919, 113);
		btn_down.Name = "btn_down";
		btn_down.Size = new System.Drawing.Size(58, 40);
		btn_down.TabIndex = 52;
		btn_down.Text = "Down";
		btn_down.UseVisualStyleBackColor = true;
		btn_down.Click += new System.EventHandler(btn_KeyboardLocation_Click);
		btn_top.Location = new System.Drawing.Point(919, 55);
		btn_top.Name = "btn_top";
		btn_top.Size = new System.Drawing.Size(58, 40);
		btn_top.TabIndex = 51;
		btn_top.Text = "Top";
		btn_top.UseVisualStyleBackColor = true;
		btn_top.Click += new System.EventHandler(btn_KeyboardLocation_Click);
		pb_keyboard.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pb_keyboard.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
		pb_keyboard.Image = POS_Client.Properties.Resources.keyboard_close;
		pb_keyboard.Location = new System.Drawing.Point(919, 6);
		pb_keyboard.Name = "pb_keyboard";
		pb_keyboard.Size = new System.Drawing.Size(59, 34);
		pb_keyboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		pb_keyboard.TabIndex = 50;
		pb_keyboard.TabStop = false;
		pb_keyboard.Click += new System.EventHandler(pictureBox1_Click);
		keyboardcontrol1.KeyboardType = KeyboardClassLibrary.BoW.Standard;
		keyboardcontrol1.Location = new System.Drawing.Point(12, 6);
		keyboardcontrol1.Name = "keyboardcontrol1";
		keyboardcontrol1.Size = new System.Drawing.Size(816, 260);
		keyboardcontrol1.TabIndex = 0;
		keyboardcontrol1.UserKeyPressed += new KeyboardClassLibrary.KeyboardDelegate(keyboardcontrol1_UserKeyPressed);
		pb_virtualKeyBoard.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom);
		pb_virtualKeyBoard.BackColor = System.Drawing.Color.FromArgb(50, 153, 153, 153);
		pb_virtualKeyBoard.Cursor = System.Windows.Forms.Cursors.Hand;
		pb_virtualKeyBoard.Image = POS_Client.Properties.Resources.keyboard;
		pb_virtualKeyBoard.Location = new System.Drawing.Point(910, 620);
		pb_virtualKeyBoard.Name = "pb_virtualKeyBoard";
		pb_virtualKeyBoard.Size = new System.Drawing.Size(70, 40);
		pb_virtualKeyBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		pb_virtualKeyBoard.TabIndex = 51;
		pb_virtualKeyBoard.TabStop = false;
		pb_virtualKeyBoard.Click += new System.EventHandler(pictureBox1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(981, 661);
		base.Controls.Add(pa_keyboard);
		base.Controls.Add(pb_virtualKeyBoard);
		base.Controls.Add(topPanel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.Name = "MasterThinForm";
		Text = "農藥銷售簡易POS";
		base.Load += new System.EventHandler(MasterThinForm_Load);
		topPanel.ResumeLayout(false);
		topPanel.PerformLayout();
		pa_keyboard.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)pb_keyboard).EndInit();
		((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).EndInit();
		ResumeLayout(false);
	}
}
