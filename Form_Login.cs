using NIISOL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_Login : Form
{
	private string iniPath = Application.StartupPath + "\\Setting.ini";

	private IContainer components = null;

	private Label label1;

	private Label label2;

	private Button button1;

	private TextBox tb_AgencyCode;

	private TextBox tb_UserName;

	private Label label3;

	private Label label4;

	public Form_Login()
	{
		InitializeComponent();
		DelExportedData();
		label4.Text = "版號:beta2.1";
		if (File.Exists(iniPath))
		{
			tb_AgencyCode.Text = File.ReadAllText(iniPath);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (tb_AgencyCode.Text.Trim() == "")
		{
			MessageBox.Show("請填入醫事機構代碼");
			tb_AgencyCode.Focus();
			return;
		}
		if (tb_UserName.Text.Trim() == "" || !Utility.IsIdNo(tb_UserName.Text.Trim()))
		{
			MessageBox.Show("使用者證號錯誤");
			tb_UserName.Focus();
			return;
		}
		Form_Main form_Main = new Form_Main();
		form_Main.FormClosed += F2_FormClosed;
		form_Main.AgencyCode = tb_AgencyCode.Text;
		form_Main.UserName = tb_UserName.Text;
		if (!File.Exists(iniPath))
		{
			using (File.Create(iniPath))
			{
			}
		}
		using (StreamWriter streamWriter = new StreamWriter(iniPath))
		{
			streamWriter.Write(tb_AgencyCode.Text);
			streamWriter.Close();
		}
		form_Main.Show();
		Hide();
	}

	private void F2_FormClosed(object sender, FormClosedEventArgs e)
	{
		Close();
	}

	private void DelExportedData()
	{
		string sql = "UPDATE Record SET LogicDel=1 WHERE ExportedDate < '" + Utility.ToRocDateString(DateTime.Now) + "'";
		DataBaseUtilities.DBOperation(Program.ConnectionString, sql, new string[0], CommandOperationType.ExecuteNonQuery);
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
		label1 = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		button1 = new System.Windows.Forms.Button();
		tb_AgencyCode = new System.Windows.Forms.TextBox();
		tb_UserName = new System.Windows.Forms.TextBox();
		label3 = new System.Windows.Forms.Label();
		label4 = new System.Windows.Forms.Label();
		SuspendLayout();
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(116, 165);
		label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(283, 21);
		label1.TabIndex = 0;
		label1.Text = "接種單位十碼章醫事機構代碼";
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(284, 230);
		label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(115, 21);
		label2.TabIndex = 1;
		label2.Text = "使用者證號";
		button1.Font = new System.Drawing.Font("新細明體", 10f);
		button1.Location = new System.Drawing.Point(393, 306);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(128, 43);
		button1.TabIndex = 2;
		button1.Text = "開始使用";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		tb_AgencyCode.Location = new System.Drawing.Point(409, 162);
		tb_AgencyCode.Name = "tb_AgencyCode";
		tb_AgencyCode.Size = new System.Drawing.Size(312, 33);
		tb_AgencyCode.TabIndex = 3;
		tb_UserName.ImeMode = System.Windows.Forms.ImeMode.Alpha;
		tb_UserName.Location = new System.Drawing.Point(409, 227);
		tb_UserName.Name = "tb_UserName";
		tb_UserName.Size = new System.Drawing.Size(312, 33);
		tb_UserName.TabIndex = 4;
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("新細明體", 21.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.Location = new System.Drawing.Point(318, 64);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(274, 29);
		label3.TabIndex = 5;
		label3.Text = "離線版預防接種登錄";
		label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label4.AutoSize = true;
		label4.Location = new System.Drawing.Point(663, 328);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(58, 21);
		label4.TabIndex = 6;
		label4.Text = "label4";
		base.AutoScaleDimensions = new System.Drawing.SizeF(11f, 21f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(898, 457);
		base.Controls.Add(label4);
		base.Controls.Add(label3);
		base.Controls.Add(tb_UserName);
		base.Controls.Add(tb_AgencyCode);
		base.Controls.Add(button1);
		base.Controls.Add(label2);
		base.Controls.Add(label1);
		Font = new System.Drawing.Font("新細明體", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		base.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
		base.Name = "Form_Login";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		ResumeLayout(false);
		PerformLayout();
	}
}
