
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;
using 離線列印Client程式.OfflinePrintWebService;
using 離線列印Client程式.Properties;

public class frmUploadSysSN : Form
{
	private IContainer components;

	private Label label8;

	private Label lblsysSerialNo;

	private Label label17;

	private TextBox txtOrgID;

	private Button btnSubmit;

	private Button btnCancel;

	private Panel panelPrint;

	public frmUploadSysSN()
	{
		InitializeComponent();
		lblsysSerialNo.Text = Program.SysSerialNo;
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtOrgID.Text.Length <= 0)
			{
				MessageBox.Show("組織代碼不得為空");
			}
			else if (!CommonUtilities.isInteger(txtOrgID.Text))
			{
				MessageBox.Show("請輸入正確組織代碼格式");
			}
			else if (NetworkInfo.IsConnectionExist(Program.WebServiceHostNameOL))
			{
				string a = new Service
				{
					Url = Program.OLPUrl,
					Timeout = 800000
				}.UploadSN(lblsysSerialNo.Text, txtOrgID.Text);
				if (a == "0")
				{
					MessageBox.Show("上傳成功!");
					DataBaseUtilities.DBOperation(Program.ConnectionString, "Update SysParam set OrgID = {0}", new string[1]
					{
						txtOrgID.Text
					}, CommandOperationType.ExecuteNonQuery);
					base.DialogResult = DialogResult.OK;
					Close();
				}
				else if (a == "1")
				{
					MessageBox.Show("上傳失敗，請輸入正確組織代碼!");
				}
				else if (a == "2")
				{
					MessageBox.Show("上傳組織代碼失敗，請稍候重試。");
				}
				else
				{
					MessageBox.Show("上傳主機失敗，請稍候重試。");
				}
			}
			else
			{
				MessageBox.Show("目前無法連上主機，請稍候重試。");
			}
		}
		catch (Exception)
		{
			MessageBox.Show("目前無法連上主機，請稍候重試。");
			Close();
		}
	}

	private void frmUploadSysSN_Load(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmUploadSysSN));
		label8 = new System.Windows.Forms.Label();
		lblsysSerialNo = new System.Windows.Forms.Label();
		label17 = new System.Windows.Forms.Label();
		txtOrgID = new System.Windows.Forms.TextBox();
		btnSubmit = new System.Windows.Forms.Button();
		btnCancel = new System.Windows.Forms.Button();
		panelPrint = new System.Windows.Forms.Panel();
		panelPrint.SuspendLayout();
		SuspendLayout();
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 3);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(240, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(240, 26);
		label8.TabIndex = 8;
		label8.Text = "您目前所使用之系統序號為";
		lblsysSerialNo.AutoSize = true;
		lblsysSerialNo.BackColor = System.Drawing.SystemColors.ControlLight;
		lblsysSerialNo.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblsysSerialNo.ForeColor = System.Drawing.SystemColors.ControlText;
		lblsysSerialNo.Location = new System.Drawing.Point(249, 3);
		lblsysSerialNo.MaximumSize = new System.Drawing.Size(319, 0);
		lblsysSerialNo.MinimumSize = new System.Drawing.Size(319, 0);
		lblsysSerialNo.Name = "lblsysSerialNo";
		lblsysSerialNo.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblsysSerialNo.Size = new System.Drawing.Size(319, 26);
		lblsysSerialNo.TabIndex = 9;
		lblsysSerialNo.Text = "系統序號";
		label17.AutoSize = true;
		label17.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label17.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label17.Location = new System.Drawing.Point(3, 31);
		label17.MaximumSize = new System.Drawing.Size(200, 0);
		label17.MinimumSize = new System.Drawing.Size(240, 0);
		label17.Name = "label17";
		label17.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label17.Size = new System.Drawing.Size(240, 26);
		label17.TabIndex = 18;
		label17.Text = "請輸入您的組織代碼";
		txtOrgID.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtOrgID.Location = new System.Drawing.Point(259, 31);
		txtOrgID.Name = "txtOrgID";
		txtOrgID.Size = new System.Drawing.Size(319, 27);
		txtOrgID.TabIndex = 1;
		btnSubmit.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSubmit.Location = new System.Drawing.Point(143, 82);
		btnSubmit.Name = "btnSubmit";
		btnSubmit.Size = new System.Drawing.Size(113, 31);
		btnSubmit.TabIndex = 19;
		btnSubmit.Text = "上傳系統序號";
		btnSubmit.UseVisualStyleBackColor = true;
		btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(262, 82);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnSubmit);
		panelPrint.Controls.Add(txtOrgID);
		panelPrint.Controls.Add(label17);
		panelPrint.Controls.Add(lblsysSerialNo);
		panelPrint.Controls.Add(label8);
		panelPrint.Location = new System.Drawing.Point(117, 170);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(585, 123);
		panelPrint.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackgroundImage = 離線列印Client程式.Properties.Resources.login_bg;
		BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(749, 428);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmUploadSysSN";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "上傳系統序號";
		base.Load += new System.EventHandler(frmUploadSysSN_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
