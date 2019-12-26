using POS_Client;
using POS_Client.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

public class MasterForm : Form
{
	public string frmName = "";

	private const int WS_SYSMENU = 524288;

	private IContainer components;

	private Panel topPanel;

	private Label l_sysTime;

	private Label l_Casher;

	private Label l_siteNumber;

	private Label l_shopCode;

	private Panel footPanel;

	private Label l_sysLastUpdate2;

	private Label l_sysLastUpdate;

	private Label l_tel;

	private Label l_telephone;

	private Timer t_sysTime;

	private Label l_licenseCode;

	private Label l_CasherDisplay;

	private Label l_exit;

	private Label l_sysVersion;

	private Label label1;

	private Label label2;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = base.CreateParams;
			createParams.Style &= -524289;
			return createParams;
		}
	}

	public MasterForm()
	{
		InitializeComponent();
	}

	private void t_sysTime_Tick(object sender, EventArgs e)
	{
		l_sysTime.Text = "目前日期時間： " + DateTime.Now.ToString();
	}

	public void backToPreviousForm()
	{
		if (string.IsNullOrEmpty(frmName))
		{
			AutoClosingMessageBox.Show("返回失敗, 錯誤的Form名稱: " + ((frmName == null) ? "null" : frmName));
			return;
		}
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

	public void switchForm(MasterThinForm f)
	{
		f.frmName = base.Name;
		f.Location = new Point(base.Location.X, base.Location.Y);
		f.Show();
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

	private void MasterForm_Load(object sender, EventArgs e)
	{
		l_licenseCode.Text = Program.LincenseCode;
		l_CasherDisplay.Text = Program.Casher;
		l_siteNumber.Text = "收銀機台號：" + Program.SiteNo;
		l_sysVersion.Text = Program.Version + (Program.IsHyweb ? " [測試版本!]" : "");
		l_sysLastUpdate2.Text = Program.SysLastUpdate;
	}

	private void l_exit_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("確定離開系統？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			frmExtendScreen.OnlyInstance.Dispose();
			Application.Exit();
		}
	}

	private void label2_Click(object sender, EventArgs e)
	{
		Process.Start("TeamViewerQS_zhtw.exe");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.MasterForm));
		topPanel = new System.Windows.Forms.Panel();
		l_exit = new System.Windows.Forms.Label();
		l_CasherDisplay = new System.Windows.Forms.Label();
		l_licenseCode = new System.Windows.Forms.Label();
		l_sysTime = new System.Windows.Forms.Label();
		l_Casher = new System.Windows.Forms.Label();
		l_siteNumber = new System.Windows.Forms.Label();
		l_shopCode = new System.Windows.Forms.Label();
		footPanel = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		l_sysVersion = new System.Windows.Forms.Label();
		l_sysLastUpdate2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		l_sysLastUpdate = new System.Windows.Forms.Label();
		l_tel = new System.Windows.Forms.Label();
		l_telephone = new System.Windows.Forms.Label();
		t_sysTime = new System.Windows.Forms.Timer(components);
		topPanel.SuspendLayout();
		footPanel.SuspendLayout();
		SuspendLayout();
		topPanel.BackgroundImage = POS_Client.Properties.Resources.top_bg;
		topPanel.Controls.Add(l_exit);
		topPanel.Controls.Add(l_CasherDisplay);
		topPanel.Controls.Add(l_licenseCode);
		topPanel.Controls.Add(l_sysTime);
		topPanel.Controls.Add(l_Casher);
		topPanel.Controls.Add(l_siteNumber);
		topPanel.Controls.Add(l_shopCode);
		topPanel.Location = new System.Drawing.Point(10, 0);
		topPanel.Margin = new System.Windows.Forms.Padding(0);
		topPanel.Name = "topPanel";
		topPanel.Size = new System.Drawing.Size(960, 71);
		topPanel.TabIndex = 31;
		l_exit.BackColor = System.Drawing.Color.Transparent;
		l_exit.Cursor = System.Windows.Forms.Cursors.Hand;
		l_exit.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_exit.ForeColor = System.Drawing.Color.White;
		l_exit.Location = new System.Drawing.Point(846, 9);
		l_exit.Name = "l_exit";
		l_exit.Size = new System.Drawing.Size(98, 52);
		l_exit.TabIndex = 11;
		l_exit.Text = "離開系統";
		l_exit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		l_exit.Click += new System.EventHandler(l_exit_Click);
		l_CasherDisplay.AutoSize = true;
		l_CasherDisplay.BackColor = System.Drawing.Color.Transparent;
		l_CasherDisplay.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_CasherDisplay.ForeColor = System.Drawing.Color.White;
		l_CasherDisplay.Location = new System.Drawing.Point(538, 40);
		l_CasherDisplay.Name = "l_CasherDisplay";
		l_CasherDisplay.Size = new System.Drawing.Size(0, 21);
		l_CasherDisplay.TabIndex = 10;
		l_licenseCode.AutoSize = true;
		l_licenseCode.BackColor = System.Drawing.Color.Transparent;
		l_licenseCode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_licenseCode.ForeColor = System.Drawing.Color.White;
		l_licenseCode.Location = new System.Drawing.Point(538, 9);
		l_licenseCode.Name = "l_licenseCode";
		l_licenseCode.Size = new System.Drawing.Size(0, 21);
		l_licenseCode.TabIndex = 9;
		l_sysTime.AutoSize = true;
		l_sysTime.BackColor = System.Drawing.Color.Transparent;
		l_sysTime.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_sysTime.ForeColor = System.Drawing.Color.White;
		l_sysTime.Location = new System.Drawing.Point(40, 9);
		l_sysTime.Name = "l_sysTime";
		l_sysTime.Size = new System.Drawing.Size(122, 21);
		l_sysTime.TabIndex = 4;
		l_sysTime.Text = "目前日期時間：";
		l_Casher.AutoSize = true;
		l_Casher.BackColor = System.Drawing.Color.Transparent;
		l_Casher.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_Casher.ForeColor = System.Drawing.Color.White;
		l_Casher.Location = new System.Drawing.Point(454, 40);
		l_Casher.Name = "l_Casher";
		l_Casher.Size = new System.Drawing.Size(90, 21);
		l_Casher.TabIndex = 8;
		l_Casher.Text = "收銀人員：";
		l_siteNumber.AutoSize = true;
		l_siteNumber.BackColor = System.Drawing.Color.Transparent;
		l_siteNumber.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_siteNumber.ForeColor = System.Drawing.Color.White;
		l_siteNumber.Location = new System.Drawing.Point(41, 40);
		l_siteNumber.Name = "l_siteNumber";
		l_siteNumber.Size = new System.Drawing.Size(106, 21);
		l_siteNumber.TabIndex = 6;
		l_siteNumber.Text = "收銀機台號：";
		l_shopCode.AutoSize = true;
		l_shopCode.BackColor = System.Drawing.Color.Transparent;
		l_shopCode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_shopCode.ForeColor = System.Drawing.Color.White;
		l_shopCode.Location = new System.Drawing.Point(454, 9);
		l_shopCode.Name = "l_shopCode";
		l_shopCode.Size = new System.Drawing.Size(90, 21);
		l_shopCode.TabIndex = 7;
		l_shopCode.Text = "門市代號：";
		footPanel.BackgroundImage = POS_Client.Properties.Resources.bottom_bg;
		footPanel.Controls.Add(label2);
		footPanel.Controls.Add(l_sysVersion);
		footPanel.Controls.Add(l_sysLastUpdate2);
		footPanel.Controls.Add(label1);
		footPanel.Controls.Add(l_sysLastUpdate);
		footPanel.Controls.Add(l_tel);
		footPanel.Controls.Add(l_telephone);
		footPanel.Location = new System.Drawing.Point(-5, 623);
		footPanel.Margin = new System.Windows.Forms.Padding(0);
		footPanel.Name = "footPanel";
		footPanel.Size = new System.Drawing.Size(990, 39);
		footPanel.TabIndex = 32;
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.Color.Transparent;
		label2.Cursor = System.Windows.Forms.Cursors.Hand;
		label2.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.Color.White;
		label2.Location = new System.Drawing.Point(902, 11);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(73, 17);
		label2.TabIndex = 33;
		label2.Text = "連線請按我";
		label2.Click += new System.EventHandler(label2_Click);
		l_sysVersion.AutoSize = true;
		l_sysVersion.BackColor = System.Drawing.Color.Transparent;
		l_sysVersion.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_sysVersion.ForeColor = System.Drawing.Color.FromArgb(170, 218, 47);
		l_sysVersion.Location = new System.Drawing.Point(740, 11);
		l_sysVersion.Name = "l_sysVersion";
		l_sysVersion.Size = new System.Drawing.Size(26, 17);
		l_sysVersion.TabIndex = 32;
		l_sysVersion.Text = "{0}";
		l_sysLastUpdate2.AutoSize = true;
		l_sysLastUpdate2.BackColor = System.Drawing.Color.Transparent;
		l_sysLastUpdate2.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_sysLastUpdate2.ForeColor = System.Drawing.Color.FromArgb(170, 218, 47);
		l_sysLastUpdate2.Location = new System.Drawing.Point(352, 11);
		l_sysLastUpdate2.Name = "l_sysLastUpdate2";
		l_sysLastUpdate2.Size = new System.Drawing.Size(141, 17);
		l_sysLastUpdate2.TabIndex = 32;
		l_sysLastUpdate2.Text = "2016/09/06 08:00:00";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.Color.Transparent;
		label1.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.White;
		label1.Location = new System.Drawing.Point(674, 11);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(73, 17);
		label1.TabIndex = 31;
		label1.Text = "系統版本：";
		l_sysLastUpdate.AutoSize = true;
		l_sysLastUpdate.BackColor = System.Drawing.Color.Transparent;
		l_sysLastUpdate.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_sysLastUpdate.ForeColor = System.Drawing.Color.White;
		l_sysLastUpdate.Location = new System.Drawing.Point(243, 11);
		l_sysLastUpdate.Name = "l_sysLastUpdate";
		l_sysLastUpdate.Size = new System.Drawing.Size(112, 17);
		l_sysLastUpdate.TabIndex = 31;
		l_sysLastUpdate.Text = "｜系統最後更新：";
		l_tel.AutoSize = true;
		l_tel.BackColor = System.Drawing.Color.Transparent;
		l_tel.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_tel.ForeColor = System.Drawing.Color.FromArgb(170, 218, 47);
		l_tel.Location = new System.Drawing.Point(148, 11);
		l_tel.Name = "l_tel";
		l_tel.Size = new System.Drawing.Size(100, 17);
		l_tel.TabIndex = 30;
		l_tel.Text = "0800-035-228";
		l_telephone.AutoSize = true;
		l_telephone.BackColor = System.Drawing.Color.Transparent;
		l_telephone.Font = new System.Drawing.Font("微軟正黑體", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_telephone.ForeColor = System.Drawing.Color.White;
		l_telephone.Location = new System.Drawing.Point(30, 11);
		l_telephone.Name = "l_telephone";
		l_telephone.Size = new System.Drawing.Size(126, 17);
		l_telephone.TabIndex = 3;
		l_telephone.Text = "POS免費咨詢專線：";
		t_sysTime.Enabled = true;
		t_sysTime.Tick += new System.EventHandler(t_sysTime_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(981, 661);
		base.Controls.Add(footPanel);
		base.Controls.Add(topPanel);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "MasterForm";
		Text = "農藥銷售簡易POS";
		base.Load += new System.EventHandler(MasterForm_Load);
		topPanel.ResumeLayout(false);
		topPanel.PerformLayout();
		footPanel.ResumeLayout(false);
		footPanel.PerformLayout();
		ResumeLayout(false);
	}
}
