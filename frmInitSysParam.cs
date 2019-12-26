
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;
using 離線列印Client程式.OfflinePrintWebService;
using 離線列印Client程式.Properties;

public class frmInitSysParam : Form
{
	private IContainer components;

	private Panel panelPrint;

	private Label label3;

	private Label label4;

	private Label label2;

	private Label label1;

	private Button btnCancel;

	private Button btnSubmit;

	private TextBox txtValadationCode;

	private Label label17;

	private Label lblsysSerialNo;

	private Label label8;

	private Button btnUploadSysSN;

	public frmInitSysParam()
	{
		InitializeComponent();
		lblsysSerialNo.Text = Program.SysSerialNo;
		txtValadationCode.Text = Program.SysSerialNo;
		Program.ReloadCRC();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		if (txtValadationCode.Text == Program.ValadationCode)
		{
			MessageBox.Show("授權碼正確!");
			string[,] strFieldArray = new string[3, 2]
			{
				{
					"SysSerialNo",
					Program.SysSerialNo
				},
				{
					"UploadedSysSN",
					Convert.ToInt32(UploadSySNType.已上傳序號).ToString()
				},
				{
					"ProducerName",
					""
				}
			};
			if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery)) == 0)
			{
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
			}
			Program.WriteCRC();
			Program.ProducerName = "";
			ShowMainDialog();
		}
		else
		{
			MessageBox.Show("授權碼錯誤");
		}
	}

	private void txtValadationCode_TextChanged(object sender, EventArgs e)
	{
		txtValadationCode.Text = txtValadationCode.Text.ToUpper();
		txtValadationCode.SelectionStart = txtValadationCode.Text.Length;
	}

	private void ShowMainDialog()
	{
		base.Visible = false;
		new frmMain().ShowDialog();
		Dispose();
		Close();
	}

	private void ShowUpdateDialog()
	{
		base.Visible = false;
		new frmUpdate().ShowDialog(this);
		if (Program.Upgraded)
		{
			Close();
		}
	}

	private void frmInitSysParam_Load(object sender, EventArgs e)
	{
		if (Program.IsDeployClickOnce)
		{
			ShowUpdateDialog();
		}
		btnUploadSysSN.Enabled = ((Program.UploadedSysSN == UploadSySNType.未上傳序號 || Program.UploadedSysSN == UploadSySNType.序號改變未上傳) ? true : false);
		AutoGetSN();
	}

	private void AutoGetSN()
	{
		try
		{
			if (NetworkInfo.IsConnectionExist(Program.WebServiceHostNameOL))
			{
				string orgName = new Service
				{
					Url = Program.OLPUrl,
					Timeout = 800000
				}.GetOrgName(Program.SysSerialNo);
				if (orgName != "")
				{
					MessageBox.Show("從主機上成功取得援權碼!");
					string[,] strFieldArray = new string[3, 2]
					{
						{
							"SysSerialNo",
							Program.SysSerialNo
						},
						{
							"UploadedSysSN",
							Convert.ToInt32(UploadSySNType.已上傳序號).ToString()
						},
						{
							"ProducerName",
							orgName
						}
					};
					if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery)) == 0)
					{
						DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
					}
					Program.WriteCRC();
					Program.ProducerName = orgName;
					ShowMainDialog();
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void btnUploadSysSN_Click(object sender, EventArgs e)
	{
		frmUploadSysSN frmUploadSysSN = new frmUploadSysSN();
		try
		{
			if (frmUploadSysSN.ShowDialog(this) == DialogResult.OK)
			{
				Program.UploadedSysSN = ((Program.UploadedSysSN == UploadSySNType.未上傳序號) ? UploadSySNType.已上傳序號 : UploadSySNType.序號改變已上傳);
				btnUploadSysSN.Enabled = false;
				string[,] strFieldArray = new string[1, 2]
				{
					{
						"UploadedSysSN",
						Convert.ToInt32(Program.UploadedSysSN).ToString()
					}
				};
				if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery)) == 0)
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				}
				AutoGetSN();
			}
		}
		catch (Exception)
		{
		}
		frmUploadSysSN.Dispose();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmInitSysParam));
		panelPrint = new System.Windows.Forms.Panel();
		btnUploadSysSN = new System.Windows.Forms.Button();
		label3 = new System.Windows.Forms.Label();
		label4 = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		btnCancel = new System.Windows.Forms.Button();
		btnSubmit = new System.Windows.Forms.Button();
		txtValadationCode = new System.Windows.Forms.TextBox();
		label17 = new System.Windows.Forms.Label();
		lblsysSerialNo = new System.Windows.Forms.Label();
		label8 = new System.Windows.Forms.Label();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(btnUploadSysSN);
		panelPrint.Controls.Add(label3);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnSubmit);
		panelPrint.Controls.Add(txtValadationCode);
		panelPrint.Controls.Add(label17);
		panelPrint.Controls.Add(lblsysSerialNo);
		panelPrint.Controls.Add(label8);
		panelPrint.Location = new System.Drawing.Point(127, 135);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(584, 211);
		panelPrint.TabIndex = 2;
		btnUploadSysSN.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnUploadSysSN.Location = new System.Drawing.Point(120, 168);
		btnUploadSysSN.Name = "btnUploadSysSN";
		btnUploadSysSN.Size = new System.Drawing.Size(133, 31);
		btnUploadSysSN.TabIndex = 25;
		btnUploadSysSN.Text = "上傳系統序號";
		btnUploadSysSN.UseVisualStyleBackColor = true;
		btnUploadSysSN.Click += new System.EventHandler(btnUploadSysSN_Click);
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.SystemColors.Control;
		label3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.SystemColors.ControlText;
		label3.Location = new System.Drawing.Point(18, 142);
		label3.MaximumSize = new System.Drawing.Size(280, 0);
		label3.MinimumSize = new System.Drawing.Size(350, 0);
		label3.Name = "label3";
		label3.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label3.Size = new System.Drawing.Size(350, 26);
		label3.TabIndex = 24;
		label3.Text = "列印動作。";
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.Control;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlText;
		label4.Location = new System.Drawing.Point(3, 116);
		label4.MaximumSize = new System.Drawing.Size(280, 0);
		label4.MinimumSize = new System.Drawing.Size(550, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(550, 26);
		label4.TabIndex = 23;
		label4.Text = "2. 輸入授權代碼後，經由網路驗證完畢，方可下載標籤清單，以進行";
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.Control;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlText;
		label2.Location = new System.Drawing.Point(18, 87);
		label2.MaximumSize = new System.Drawing.Size(280, 0);
		label2.MinimumSize = new System.Drawing.Size(200, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(200, 26);
		label2.TabIndex = 22;
		label2.Text = "索取合法授權代碼。";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.Control;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlText;
		label1.Location = new System.Drawing.Point(3, 61);
		label1.MaximumSize = new System.Drawing.Size(280, 0);
		label1.MinimumSize = new System.Drawing.Size(550, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(550, 26);
		label1.TabIndex = 21;
		label1.Text = "1. 請電話洽詢 行政院農委會產銷履歷客服專線(02-23510182游小姐)";
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(345, 168);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnSubmit.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSubmit.Location = new System.Drawing.Point(259, 168);
		btnSubmit.Name = "btnSubmit";
		btnSubmit.Size = new System.Drawing.Size(80, 31);
		btnSubmit.TabIndex = 19;
		btnSubmit.Text = "確認";
		btnSubmit.UseVisualStyleBackColor = true;
		btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		txtValadationCode.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtValadationCode.Location = new System.Drawing.Point(259, 31);
		txtValadationCode.Name = "txtValadationCode";
		txtValadationCode.Size = new System.Drawing.Size(318, 27);
		txtValadationCode.TabIndex = 1;
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
		label17.Text = "請輸入合法授權代碼";
		lblsysSerialNo.AutoSize = true;
		lblsysSerialNo.BackColor = System.Drawing.SystemColors.ControlLight;
		lblsysSerialNo.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblsysSerialNo.ForeColor = System.Drawing.SystemColors.ControlText;
		lblsysSerialNo.Location = new System.Drawing.Point(249, 3);
		lblsysSerialNo.MaximumSize = new System.Drawing.Size(325, 0);
		lblsysSerialNo.MinimumSize = new System.Drawing.Size(325, 0);
		lblsysSerialNo.Name = "lblsysSerialNo";
		lblsysSerialNo.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblsysSerialNo.Size = new System.Drawing.Size(325, 26);
		lblsysSerialNo.TabIndex = 9;
		lblsysSerialNo.Text = "系統序號";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackgroundImage = 離線列印Client程式.Properties.Resources.login_bg;
		BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(749, 428);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmInitSysParam";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "授權碼驗證";
		base.Load += new System.EventHandler(frmInitSysParam_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
