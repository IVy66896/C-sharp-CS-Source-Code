
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;
using 離線列印Client程式.Properties;

public class frmDisposePrint : Form
{
	private class DisposeEmement
	{
		public int DisposeID;

		public Button BtnDel;

		public Label SrtPrefix;

		public TextBox TxtSrt;

		public Label SrtPostfix;

		public Label EndPrefix;

		public TextBox TxtEnd;

		public Label EndPostfix;

		public TextBox DisposeAmount;

		public TextBox DisposeReason;

		public DisposeEmement(int offsetY, int id, string txtSrt, string txtEnd, string printCodePrefix, string amount, string reason, bool readOnly)
		{
			DisposeID = id;
			BtnDel = new Button();
			BtnDel.Image = Resources.del;
			BtnDel.Location = new Point(21, 3 + offsetY);
			BtnDel.Name = $"btnDel_{id}";
			BtnDel.Size = new Size(23, 23);
			BtnDel.TextImageRelation = TextImageRelation.ImageBeforeText;
			BtnDel.UseVisualStyleBackColor = true;
			SrtPrefix = new Label();
			SrtPrefix.BackColor = SystemColors.ControlLight;
			SrtPrefix.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			SrtPrefix.ForeColor = SystemColors.ControlText;
			SrtPrefix.Location = new Point(65, offsetY);
			SrtPrefix.MaximumSize = new Size(280, 26);
			SrtPrefix.MinimumSize = new Size(100, 26);
			SrtPrefix.Name = $"lblSrtPrefix_{id}";
			SrtPrefix.Padding = new Padding(10, 5, 5, 5);
			SrtPrefix.Size = new Size(120, 26);
			SrtPrefix.Text = printCodePrefix;
			TxtSrt = new TextBox();
			TxtSrt.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			TxtSrt.Location = new Point(170, 2 + offsetY);
			TxtSrt.Name = $"txtSrt_{id}";
			TxtSrt.Size = new Size(36, 27);
			TxtSrt.Text = txtSrt;
			SrtPostfix = new Label();
			SrtPostfix.BackColor = SystemColors.ControlLight;
			SrtPostfix.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			SrtPostfix.ForeColor = SystemColors.ControlText;
			SrtPostfix.Location = new Point(196, offsetY);
			SrtPostfix.MaximumSize = new Size(280, 26);
			SrtPostfix.MinimumSize = new Size(30, 26);
			SrtPostfix.Name = $"lblSrtPostfix_{id}";
			SrtPostfix.Padding = new Padding(10, 5, 5, 5);
			SrtPostfix.Size = new Size(42, 26);
			SrtPostfix.Text = "**";
			EndPrefix = new Label();
			EndPrefix.BackColor = SystemColors.ControlLight;
			EndPrefix.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			EndPrefix.ForeColor = SystemColors.ControlText;
			EndPrefix.Location = new Point(239, offsetY);
			EndPrefix.MaximumSize = new Size(280, 26);
			EndPrefix.MinimumSize = new Size(100, 26);
			EndPrefix.Name = $"lblEndPrefix_{id}";
			EndPrefix.Padding = new Padding(10, 5, 5, 5);
			EndPrefix.Size = new Size(120, 26);
			EndPrefix.Text = printCodePrefix;
			TxtEnd = new TextBox();
			TxtEnd.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			TxtEnd.Location = new Point(344, 2 + offsetY);
			TxtEnd.Name = $"TxtEnd_{id}";
			TxtEnd.Size = new Size(36, 27);
			TxtEnd.Text = txtEnd;
			EndPostfix = new Label();
			EndPostfix.BackColor = SystemColors.ControlLight;
			EndPostfix.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			EndPostfix.ForeColor = SystemColors.ControlText;
			EndPostfix.Location = new Point(370, offsetY);
			EndPostfix.MaximumSize = new Size(280, 26);
			EndPostfix.MinimumSize = new Size(30, 26);
			EndPostfix.Name = $"lblEndPostfix_{id}";
			EndPostfix.Padding = new Padding(10, 5, 5, 5);
			EndPostfix.Size = new Size(42, 26);
			EndPostfix.Text = "**";
			DisposeAmount = new TextBox();
			DisposeAmount.BackColor = SystemColors.ControlLight;
			DisposeAmount.Font = new Font("PMingLiU", 11f, FontStyle.Regular, GraphicsUnit.Point, 136);
			DisposeAmount.ForeColor = SystemColors.ControlText;
			DisposeAmount.Location = new Point(419, offsetY);
			DisposeAmount.MaximumSize = new Size(280, 26);
			DisposeAmount.MinimumSize = new Size(30, 26);
			DisposeAmount.Name = $"lblAmt_{id}";
			DisposeAmount.Padding = new Padding(10, 5, 5, 5);
			DisposeAmount.Size = new Size(75, 26);
			DisposeAmount.Text = amount;
			DisposeReason = new TextBox();
			DisposeReason.Font = new Font("PMingLiU", 12f, FontStyle.Regular, GraphicsUnit.Point, 136);
			DisposeReason.Location = new Point(499, 2 + offsetY);
			DisposeReason.Name = $"txtDisposeReason_{id}";
			DisposeReason.Size = new Size(247, 27);
			DisposeReason.Text = reason;
			if (readOnly)
			{
				BtnDel.Visible = false;
				BtnDel.Enabled = false;
				TxtSrt.Enabled = false;
				TxtEnd.Enabled = false;
				DisposeReason.Enabled = false;
			}
		}
	}

	private const int eachDisposeDataOffset = 40;

	private const int StartingOffset = 68;

	private int DisposeID;

	private List<DisposeEmement> ltDispose;

	private string _PrintCodePrefix;

	private bool _ShowOnly = true;

	private int _ResumeID = -1;

	private int _Printed = -1;

	private int _StartSN = -1;

	private int _SNDigit = -1;

	private IContainer components;

	private Panel panelPrint;

	private Label label1;

	private Label lblCreateDate;

	private Label lblPackDate;

	private Label label2;

	private Label lblTraceCode;

	private Label label10;

	private Label lblProductName;

	private Label label8;

	private Label lblFarmerName;

	private Label label6;

	private Button btnCancel;

	private Button btnSubmit;

	private Label lblPrtCodeSrt;

	private Label label7;

	private Label lblPrtCodeEnd;

	private Label label4;

	private Label label3;

	private Label lblDisposeNum;

	private Label label5;

	private Panel pDispose;

	private Label lblDisposeDesc;

	private Button btnInsertDispose;

	private Label label15;

	private Label label14;

	private Label label12;

	private Label label11;

	private Label label20;

	public frmDisposePrint(string CreateDate, string PackDate, string FarmerName, string ProductName, string TraceCode, string initPrintCodeSrt, string initPrintCodeEnd, string AlreadyPrinted, string ResumeID, bool showOnly)
	{
		InitializeComponent();
		Program.ReloadCRC();
		lblCreateDate.Text = CreateDate;
		lblPackDate.Text = PackDate;
		lblFarmerName.Text = FarmerName;
		lblProductName.Text = ProductName;
		lblTraceCode.Text = TraceCode;
		lblPrtCodeSrt.Text = initPrintCodeSrt;
		lblPrtCodeEnd.Text = initPrintCodeEnd;
		_ShowOnly = showOnly;
		_ResumeID = -1;
		_Printed = Convert.ToInt32(AlreadyPrinted);
		lblDisposeNum.Text = "0";
		if (ResumeID != string.Empty)
		{
			try
			{
				_ResumeID = Convert.ToInt32(ResumeID);
			}
			catch
			{
			}
		}
		if (initPrintCodeSrt.Length >= 14)
		{
			int num = Convert.ToInt32(initPrintCodeSrt.Substring(7, 1));
			if (num == 9)
			{
				lblDisposeDesc.Text = "註：說明第1~10碼，撤消起迄號碼請填寫第11~14碼，後兩碼為驗證碼";
				_PrintCodePrefix = initPrintCodeSrt.Substring(0, 10);
				_StartSN = Convert.ToInt32(initPrintCodeSrt.Substring(10, 4));
				_SNDigit = 4;
			}
			else if (5 <= num && num <= 8)
			{
				lblDisposeDesc.Text = "註：說明第1~9碼，撤消起迄號碼請填寫第10~14碼，後兩碼為驗證碼";
				_PrintCodePrefix = initPrintCodeSrt.Substring(0, 9);
				_StartSN = Convert.ToInt32(initPrintCodeSrt.Substring(9, 5));
				_SNDigit = 5;
			}
			else
			{
				lblDisposeDesc.Text = "註：說明第1~11碼，撤消起迄號碼請填寫第12~14碼，後兩碼為驗證碼";
				_PrintCodePrefix = initPrintCodeSrt.Substring(0, 11);
				_StartSN = Convert.ToInt32(initPrintCodeSrt.Substring(11, 3));
				_SNDigit = 3;
			}
		}
		if (_ResumeID == -1)
		{
			_ShowOnly = true;
		}
		if (_ShowOnly)
		{
			btnInsertDispose.Visible = false;
			btnInsertDispose.Enabled = false;
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnSubmit_Click(object sender, EventArgs e)
	{
		if (_ShowOnly)
		{
			base.DialogResult = DialogResult.OK;
			Close();
			return;
		}
		string[] strWhereParameterArray = new string[1]
		{
			_ResumeID.ToString()
		};
		if (CheckDisposeData() && MessageBox.Show("確定編修撤銷資料嗎?", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			try
			{
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Delete, "", "ResumeDispose", "ResumeID = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteNonQuery);
				foreach (DisposeEmement item in ltDispose)
				{
					string[,] strFieldArray = new string[5, 2]
					{
						{
							"ResumeID",
							_ResumeID.ToString()
						},
						{
							"PrintCodeSrt",
							$"{_PrintCodePrefix}{item.TxtSrt.Text}"
						},
						{
							"PrintCodeEnd",
							$"{_PrintCodePrefix}{item.TxtEnd.Text}"
						},
						{
							"PintCodeCount",
							item.DisposeAmount.Text
						},
						{
							"DisposeReason",
							item.DisposeReason.Text
						}
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "ResumeDispose", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				}
				Program.WriteCRC();
				base.DialogResult = DialogResult.OK;
			}
			catch (Exception)
			{
				MessageBox.Show("儲存撤銷過程中發生例外狀況，請檢查資料是否輸入正確。");
			}
			Close();
		}
	}

	private void frmDisposePrint_Load(object sender, EventArgs e)
	{
		ltDispose = new List<DisposeEmement>();
		DisposeID = 0;
		if (_ResumeID > 0)
		{
			LoadData();
		}
	}

	private void LoadData()
	{
		int num = 0;
		string[] strWhereParameterArray = new string[1]
		{
			_ResumeID.ToString()
		};
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd, PintCodeCount, DisposeReason", "ResumeDispose", "ResumeID = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				num += Convert.ToInt32(row["PintCodeCount"]);
				DisposeEmement disposeEmement = new DisposeEmement(68 + ((DisposeID != 0) ? (40 * DisposeID) : 0), DisposeID, row["PrintCodeSrt"].ToString().Replace(_PrintCodePrefix, string.Empty), row["PrintCodeEnd"].ToString().Replace(_PrintCodePrefix, string.Empty), _PrintCodePrefix, row["PintCodeCount"].ToString(), row["DisposeReason"].ToString(), _ShowOnly);
				disposeEmement.BtnDel.Click += BtnDel_Click;
				ltDispose.Add(disposeEmement);
				pDispose.Controls.Add(disposeEmement.BtnDel);
				pDispose.Controls.Add(disposeEmement.TxtSrt);
				pDispose.Controls.Add(disposeEmement.SrtPrefix);
				pDispose.Controls.Add(disposeEmement.SrtPostfix);
				pDispose.Controls.Add(disposeEmement.TxtEnd);
				pDispose.Controls.Add(disposeEmement.EndPrefix);
				pDispose.Controls.Add(disposeEmement.EndPostfix);
				pDispose.Controls.Add(disposeEmement.DisposeAmount);
				pDispose.Controls.Add(disposeEmement.DisposeReason);
				DisposeID++;
			}
			lblDisposeNum.Text = num.ToString();
		}
	}

	private void LoadDataBak()
	{
		for (int i = 0; i < 20; i++)
		{
			DisposeEmement disposeEmement = new DisposeEmement(68 + ((i != 0) ? (40 * i) : 0), i, $"{i:000}", $"{i + 20:000}", _PrintCodePrefix, (i * 3).ToString(), $"這一張被{i}個小朋友吃了", readOnly: false);
			disposeEmement.BtnDel.Click += BtnDel_Click;
			ltDispose.Add(disposeEmement);
			pDispose.Controls.Add(disposeEmement.BtnDel);
			pDispose.Controls.Add(disposeEmement.TxtSrt);
			pDispose.Controls.Add(disposeEmement.SrtPrefix);
			pDispose.Controls.Add(disposeEmement.SrtPostfix);
			pDispose.Controls.Add(disposeEmement.TxtEnd);
			pDispose.Controls.Add(disposeEmement.EndPrefix);
			pDispose.Controls.Add(disposeEmement.EndPostfix);
			pDispose.Controls.Add(disposeEmement.DisposeAmount);
			pDispose.Controls.Add(disposeEmement.DisposeReason);
			DisposeID = i + 1;
		}
	}

	private void btnInsertDispose_Click(object sender, EventArgs e)
	{
		if (CheckDisposeData())
		{
			DisposeEmement disposeEmement = new DisposeEmement(68 + ((ltDispose.Count != 0) ? (40 * ltDispose.Count) : 0), DisposeID, string.Empty, string.Empty, _PrintCodePrefix, string.Empty, string.Empty, _ShowOnly);
			disposeEmement.BtnDel.Click += BtnDel_Click;
			ltDispose.Add(disposeEmement);
			pDispose.Controls.Add(disposeEmement.BtnDel);
			pDispose.Controls.Add(disposeEmement.TxtSrt);
			pDispose.Controls.Add(disposeEmement.SrtPrefix);
			pDispose.Controls.Add(disposeEmement.SrtPostfix);
			pDispose.Controls.Add(disposeEmement.TxtEnd);
			pDispose.Controls.Add(disposeEmement.EndPrefix);
			pDispose.Controls.Add(disposeEmement.EndPostfix);
			pDispose.Controls.Add(disposeEmement.DisposeAmount);
			pDispose.Controls.Add(disposeEmement.DisposeReason);
			DisposeID++;
		}
	}

	private void BtnDel_Click(object sender, EventArgs e)
	{
		int num = Convert.ToInt32(((Button)sender).Name.Replace("btnDel_", ""));
		int num2 = 0;
		while (true)
		{
			if (num2 < ltDispose.Count)
			{
				if (ltDispose[num2].DisposeID == num)
				{
					break;
				}
				num2++;
				continue;
			}
			return;
		}
		pDispose.Controls.Remove(ltDispose[num2].BtnDel);
		pDispose.Controls.Remove(ltDispose[num2].TxtSrt);
		pDispose.Controls.Remove(ltDispose[num2].SrtPrefix);
		pDispose.Controls.Remove(ltDispose[num2].SrtPostfix);
		pDispose.Controls.Remove(ltDispose[num2].TxtEnd);
		pDispose.Controls.Remove(ltDispose[num2].EndPrefix);
		pDispose.Controls.Remove(ltDispose[num2].EndPostfix);
		pDispose.Controls.Remove(ltDispose[num2].DisposeAmount);
		pDispose.Controls.Remove(ltDispose[num2].DisposeReason);
		for (int i = num2 + 1; i < ltDispose.Count; i++)
		{
			ltDispose[i].BtnDel.Location = new Point(ltDispose[i].BtnDel.Location.X, ltDispose[i].BtnDel.Location.Y - 40);
			ltDispose[i].TxtSrt.Location = new Point(ltDispose[i].TxtSrt.Location.X, ltDispose[i].TxtSrt.Location.Y - 40);
			ltDispose[i].SrtPrefix.Location = new Point(ltDispose[i].SrtPrefix.Location.X, ltDispose[i].SrtPrefix.Location.Y - 40);
			ltDispose[i].SrtPostfix.Location = new Point(ltDispose[i].SrtPostfix.Location.X, ltDispose[i].SrtPostfix.Location.Y - 40);
			ltDispose[i].TxtEnd.Location = new Point(ltDispose[i].TxtEnd.Location.X, ltDispose[i].TxtEnd.Location.Y - 40);
			ltDispose[i].EndPrefix.Location = new Point(ltDispose[i].EndPrefix.Location.X, ltDispose[i].EndPrefix.Location.Y - 40);
			ltDispose[i].EndPostfix.Location = new Point(ltDispose[i].EndPostfix.Location.X, ltDispose[i].EndPostfix.Location.Y - 40);
			ltDispose[i].DisposeAmount.Location = new Point(ltDispose[i].DisposeAmount.Location.X, ltDispose[i].DisposeAmount.Location.Y - 40);
			ltDispose[i].DisposeReason.Location = new Point(ltDispose[i].DisposeReason.Location.X, ltDispose[i].DisposeReason.Location.Y - 40);
		}
		ltDispose.RemoveAt(num2);
	}

	private bool CheckDisposeData()
	{
		int num = 0;
		string format = (_SNDigit == 4) ? "{0:0000}" : ((_SNDigit == 5) ? "{0:00000}" : "{0:000}");
		bool[] array = new bool[_Printed];
		foreach (DisposeEmement item in ltDispose)
		{
			if (item.TxtSrt.Text.Trim() != string.Empty || item.TxtEnd.Text.Trim() != string.Empty)
			{
				int num2;
				try
				{
					num2 = Convert.ToInt32(item.TxtSrt.Text);
				}
				catch
				{
					MessageBox.Show($"{item.TxtSrt.Text}不是正確起號。");
					item.TxtSrt.Focus();
					return false;
				}
				int num3;
				try
				{
					num3 = Convert.ToInt32(item.TxtEnd.Text);
				}
				catch
				{
					MessageBox.Show($"{item.TxtEnd.Text}不是正確迄號。");
					item.TxtEnd.Focus();
					return false;
				}
				if (num3 < num2)
				{
					MessageBox.Show("迄號不得小於起號。");
					item.TxtSrt.Focus();
					return false;
				}
				if (num2 < _StartSN || num3 > _StartSN + _Printed - 1)
				{
					MessageBox.Show($"編號 {item.TxtSrt.Text} ~ {item.TxtEnd.Text} 超出範圍。");
					item.TxtSrt.Focus();
					return false;
				}
				for (int i = num2 - _StartSN; i <= num3 - _StartSN; i++)
				{
					if (array[i])
					{
						MessageBox.Show($"編號 {item.TxtSrt.Text} ~ {item.TxtEnd.Text} 範圍重覆，請檢查資料。");
						item.TxtSrt.Focus();
						return false;
					}
					array[i] = true;
				}
				item.DisposeAmount.Text = Convert.ToString(num3 - num2 + 1);
				num += num3 - num2 + 1;
				item.TxtSrt.Text = string.Format(format, num2);
				item.TxtEnd.Text = string.Format(format, num3);
			}
			else
			{
				if (item.DisposeAmount.Text.Length == 0)
				{
					MessageBox.Show("請填撤銷起迄號或數量。");
					item.DisposeAmount.Focus();
					return false;
				}
				try
				{
					num += Convert.ToInt32(item.DisposeAmount.Text);
					if (Convert.ToInt32(item.DisposeAmount.Text) <= 0)
					{
						MessageBox.Show($"{item.DisposeAmount.Text}不是正確數量。");
						item.DisposeAmount.Focus();
						return false;
					}
				}
				catch
				{
					MessageBox.Show($"{item.DisposeAmount.Text}不是正確數量。");
					item.DisposeAmount.Focus();
					return false;
				}
				if (num > _Printed)
				{
					MessageBox.Show(string.Format("超過已列印總量。", item.DisposeAmount.Text));
					item.DisposeAmount.Focus();
					return false;
				}
			}
			if (item.DisposeReason.Text.Length == 0)
			{
				MessageBox.Show("撤銷原因為必填。");
				item.DisposeReason.Focus();
				return false;
			}
			if (item.DisposeReason.Text.Length > 100)
			{
				MessageBox.Show("撤銷原因長度不可超過100。");
				item.DisposeReason.Focus();
				return false;
			}
		}
		lblDisposeNum.Text = num.ToString();
		return true;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmDisposePrint));
		panelPrint = new System.Windows.Forms.Panel();
		pDispose = new System.Windows.Forms.Panel();
		label20 = new System.Windows.Forms.Label();
		btnInsertDispose = new System.Windows.Forms.Button();
		label15 = new System.Windows.Forms.Label();
		label14 = new System.Windows.Forms.Label();
		label12 = new System.Windows.Forms.Label();
		label11 = new System.Windows.Forms.Label();
		lblDisposeDesc = new System.Windows.Forms.Label();
		lblDisposeNum = new System.Windows.Forms.Label();
		label5 = new System.Windows.Forms.Label();
		label3 = new System.Windows.Forms.Label();
		lblPrtCodeEnd = new System.Windows.Forms.Label();
		label4 = new System.Windows.Forms.Label();
		lblPrtCodeSrt = new System.Windows.Forms.Label();
		label7 = new System.Windows.Forms.Label();
		btnCancel = new System.Windows.Forms.Button();
		btnSubmit = new System.Windows.Forms.Button();
		lblTraceCode = new System.Windows.Forms.Label();
		label10 = new System.Windows.Forms.Label();
		lblProductName = new System.Windows.Forms.Label();
		label8 = new System.Windows.Forms.Label();
		lblFarmerName = new System.Windows.Forms.Label();
		label6 = new System.Windows.Forms.Label();
		lblPackDate = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		lblCreateDate = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		panelPrint.SuspendLayout();
		pDispose.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(pDispose);
		panelPrint.Controls.Add(lblDisposeNum);
		panelPrint.Controls.Add(label5);
		panelPrint.Controls.Add(label3);
		panelPrint.Controls.Add(lblPrtCodeEnd);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(lblPrtCodeSrt);
		panelPrint.Controls.Add(label7);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnSubmit);
		panelPrint.Controls.Add(lblTraceCode);
		panelPrint.Controls.Add(label10);
		panelPrint.Controls.Add(lblProductName);
		panelPrint.Controls.Add(label8);
		panelPrint.Controls.Add(lblFarmerName);
		panelPrint.Controls.Add(label6);
		panelPrint.Controls.Add(lblPackDate);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(lblCreateDate);
		panelPrint.Controls.Add(label1);
		panelPrint.Location = new System.Drawing.Point(4, 5);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(772, 578);
		panelPrint.TabIndex = 0;
		pDispose.AutoScroll = true;
		pDispose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		pDispose.Controls.Add(label20);
		pDispose.Controls.Add(btnInsertDispose);
		pDispose.Controls.Add(label15);
		pDispose.Controls.Add(label14);
		pDispose.Controls.Add(label12);
		pDispose.Controls.Add(label11);
		pDispose.Controls.Add(lblDisposeDesc);
		pDispose.Location = new System.Drawing.Point(6, 259);
		pDispose.Name = "pDispose";
		pDispose.Size = new System.Drawing.Size(756, 271);
		pDispose.TabIndex = 35;
		label20.AutoSize = true;
		label20.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label20.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label20.Location = new System.Drawing.Point(237, 31);
		label20.MaximumSize = new System.Drawing.Size(200, 0);
		label20.MinimumSize = new System.Drawing.Size(170, 0);
		label20.Name = "label20";
		label20.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label20.Size = new System.Drawing.Size(170, 26);
		label20.TabIndex = 46;
		label20.Text = "撤銷迄號";
		label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		btnInsertDispose.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnInsertDispose.Location = new System.Drawing.Point(6, 4);
		btnInsertDispose.Name = "btnInsertDispose";
		btnInsertDispose.Size = new System.Drawing.Size(123, 25);
		btnInsertDispose.TabIndex = 38;
		btnInsertDispose.Text = "新增撤銷號碼";
		btnInsertDispose.UseVisualStyleBackColor = true;
		btnInsertDispose.Click += new System.EventHandler(btnInsertDispose_Click);
		label15.AutoSize = true;
		label15.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label15.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label15.Location = new System.Drawing.Point(499, 31);
		label15.MaximumSize = new System.Drawing.Size(200, 0);
		label15.MinimumSize = new System.Drawing.Size(250, 0);
		label15.Name = "label15";
		label15.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label15.Size = new System.Drawing.Size(250, 26);
		label15.TabIndex = 37;
		label15.Text = "撤銷原因說明";
		label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label14.AutoSize = true;
		label14.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label14.Location = new System.Drawing.Point(409, 31);
		label14.MaximumSize = new System.Drawing.Size(200, 0);
		label14.MinimumSize = new System.Drawing.Size(85, 0);
		label14.Name = "label14";
		label14.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label14.Size = new System.Drawing.Size(85, 26);
		label14.TabIndex = 36;
		label14.Text = "數量";
		label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label12.AutoSize = true;
		label12.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label12.Location = new System.Drawing.Point(65, 31);
		label12.MaximumSize = new System.Drawing.Size(200, 0);
		label12.MinimumSize = new System.Drawing.Size(170, 0);
		label12.Name = "label12";
		label12.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label12.Size = new System.Drawing.Size(170, 26);
		label12.TabIndex = 34;
		label12.Text = "撤銷起號";
		label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		label11.AutoSize = true;
		label11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label11.Location = new System.Drawing.Point(3, 31);
		label11.MaximumSize = new System.Drawing.Size(200, 0);
		label11.MinimumSize = new System.Drawing.Size(60, 0);
		label11.Name = "label11";
		label11.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label11.Size = new System.Drawing.Size(60, 26);
		label11.TabIndex = 33;
		label11.Text = "刪除";
		lblDisposeDesc.BackColor = System.Drawing.SystemColors.ControlLight;
		lblDisposeDesc.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblDisposeDesc.ForeColor = System.Drawing.Color.Red;
		lblDisposeDesc.Location = new System.Drawing.Point(135, 3);
		lblDisposeDesc.MaximumSize = new System.Drawing.Size(280, 26);
		lblDisposeDesc.MinimumSize = new System.Drawing.Size(590, 26);
		lblDisposeDesc.Name = "lblDisposeDesc";
		lblDisposeDesc.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblDisposeDesc.Size = new System.Drawing.Size(590, 26);
		lblDisposeDesc.TabIndex = 32;
		lblDisposeDesc.Text = "說明";
		lblDisposeNum.BackColor = System.Drawing.SystemColors.ControlLight;
		lblDisposeNum.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblDisposeNum.ForeColor = System.Drawing.SystemColors.ControlText;
		lblDisposeNum.Location = new System.Drawing.Point(349, 230);
		lblDisposeNum.MaximumSize = new System.Drawing.Size(80, 26);
		lblDisposeNum.MinimumSize = new System.Drawing.Size(405, 26);
		lblDisposeNum.Name = "lblDisposeNum";
		lblDisposeNum.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblDisposeNum.Size = new System.Drawing.Size(405, 26);
		lblDisposeNum.TabIndex = 34;
		lblDisposeNum.Text = "撤銷數";
		label5.AutoSize = true;
		label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
		label5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label5.Location = new System.Drawing.Point(3, 230);
		label5.MaximumSize = new System.Drawing.Size(320, 0);
		label5.MinimumSize = new System.Drawing.Size(340, 0);
		label5.Name = "label5";
		label5.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label5.Size = new System.Drawing.Size(340, 26);
		label5.TabIndex = 33;
		label5.Text = "撤銷標籤編號登錄，撤銷數量合計";
		label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
		label3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label3.Location = new System.Drawing.Point(3, 6);
		label3.MaximumSize = new System.Drawing.Size(200, 0);
		label3.MinimumSize = new System.Drawing.Size(755, 0);
		label3.Name = "label3";
		label3.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label3.Size = new System.Drawing.Size(755, 26);
		label3.TabIndex = 32;
		label3.Text = "標籤資訊";
		lblPrtCodeEnd.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPrtCodeEnd.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPrtCodeEnd.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPrtCodeEnd.Location = new System.Drawing.Point(349, 202);
		lblPrtCodeEnd.MaximumSize = new System.Drawing.Size(280, 26);
		lblPrtCodeEnd.MinimumSize = new System.Drawing.Size(405, 26);
		lblPrtCodeEnd.Name = "lblPrtCodeEnd";
		lblPrtCodeEnd.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPrtCodeEnd.Size = new System.Drawing.Size(405, 26);
		lblPrtCodeEnd.TabIndex = 31;
		lblPrtCodeEnd.Text = "標籤結束編號";
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label4.Location = new System.Drawing.Point(3, 202);
		label4.MaximumSize = new System.Drawing.Size(200, 0);
		label4.MinimumSize = new System.Drawing.Size(340, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(340, 26);
		label4.TabIndex = 30;
		label4.Text = "標籤結束編號";
		label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		lblPrtCodeSrt.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPrtCodeSrt.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPrtCodeSrt.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPrtCodeSrt.Location = new System.Drawing.Point(349, 174);
		lblPrtCodeSrt.MaximumSize = new System.Drawing.Size(280, 26);
		lblPrtCodeSrt.MinimumSize = new System.Drawing.Size(405, 26);
		lblPrtCodeSrt.Name = "lblPrtCodeSrt";
		lblPrtCodeSrt.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPrtCodeSrt.Size = new System.Drawing.Size(405, 26);
		lblPrtCodeSrt.TabIndex = 25;
		lblPrtCodeSrt.Text = "標籤啟始編號";
		label7.AutoSize = true;
		label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label7.Location = new System.Drawing.Point(3, 174);
		label7.MaximumSize = new System.Drawing.Size(200, 0);
		label7.MinimumSize = new System.Drawing.Size(340, 0);
		label7.Name = "label7";
		label7.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label7.Size = new System.Drawing.Size(340, 26);
		label7.TabIndex = 24;
		label7.Text = "標籤啟始編號";
		label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(382, 536);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnSubmit.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSubmit.Location = new System.Drawing.Point(296, 536);
		btnSubmit.Name = "btnSubmit";
		btnSubmit.Size = new System.Drawing.Size(80, 31);
		btnSubmit.TabIndex = 19;
		btnSubmit.Text = "確定";
		btnSubmit.UseVisualStyleBackColor = true;
		btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
		lblTraceCode.BackColor = System.Drawing.SystemColors.ControlLight;
		lblTraceCode.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblTraceCode.ForeColor = System.Drawing.SystemColors.ControlText;
		lblTraceCode.Location = new System.Drawing.Point(349, 146);
		lblTraceCode.MaximumSize = new System.Drawing.Size(280, 26);
		lblTraceCode.MinimumSize = new System.Drawing.Size(405, 26);
		lblTraceCode.Name = "lblTraceCode";
		lblTraceCode.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblTraceCode.Size = new System.Drawing.Size(405, 26);
		lblTraceCode.TabIndex = 11;
		lblTraceCode.Text = "Trace code";
		label10.AutoSize = true;
		label10.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label10.Location = new System.Drawing.Point(3, 146);
		label10.MaximumSize = new System.Drawing.Size(200, 0);
		label10.MinimumSize = new System.Drawing.Size(340, 0);
		label10.Name = "label10";
		label10.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label10.Size = new System.Drawing.Size(340, 26);
		label10.TabIndex = 10;
		label10.Text = "追溯號碼";
		label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		lblProductName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblProductName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblProductName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblProductName.Location = new System.Drawing.Point(349, 118);
		lblProductName.MaximumSize = new System.Drawing.Size(280, 26);
		lblProductName.MinimumSize = new System.Drawing.Size(405, 26);
		lblProductName.Name = "lblProductName";
		lblProductName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblProductName.Size = new System.Drawing.Size(405, 26);
		lblProductName.TabIndex = 9;
		lblProductName.Text = "品項";
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 118);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(340, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(340, 26);
		label8.TabIndex = 8;
		label8.Text = "品項";
		label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		lblFarmerName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblFarmerName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblFarmerName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblFarmerName.Location = new System.Drawing.Point(349, 90);
		lblFarmerName.MaximumSize = new System.Drawing.Size(280, 26);
		lblFarmerName.MinimumSize = new System.Drawing.Size(405, 26);
		lblFarmerName.Name = "lblFarmerName";
		lblFarmerName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblFarmerName.Size = new System.Drawing.Size(405, 26);
		lblFarmerName.TabIndex = 7;
		lblFarmerName.Text = "農民";
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label6.Location = new System.Drawing.Point(3, 90);
		label6.MaximumSize = new System.Drawing.Size(200, 0);
		label6.MinimumSize = new System.Drawing.Size(340, 0);
		label6.Name = "label6";
		label6.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label6.Size = new System.Drawing.Size(340, 26);
		label6.TabIndex = 6;
		label6.Text = "農民";
		label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		lblPackDate.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPackDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPackDate.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPackDate.Location = new System.Drawing.Point(349, 62);
		lblPackDate.MaximumSize = new System.Drawing.Size(280, 26);
		lblPackDate.MinimumSize = new System.Drawing.Size(405, 26);
		lblPackDate.Name = "lblPackDate";
		lblPackDate.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPackDate.Size = new System.Drawing.Size(405, 26);
		lblPackDate.TabIndex = 3;
		lblPackDate.Text = "包裝日期";
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(3, 62);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(340, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(340, 26);
		label2.TabIndex = 2;
		label2.Text = "包裝日期";
		label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		lblCreateDate.BackColor = System.Drawing.SystemColors.ControlLight;
		lblCreateDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblCreateDate.ForeColor = System.Drawing.SystemColors.ControlText;
		lblCreateDate.Location = new System.Drawing.Point(349, 34);
		lblCreateDate.MaximumSize = new System.Drawing.Size(280, 26);
		lblCreateDate.MinimumSize = new System.Drawing.Size(405, 26);
		lblCreateDate.Name = "lblCreateDate";
		lblCreateDate.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblCreateDate.Size = new System.Drawing.Size(405, 26);
		lblCreateDate.TabIndex = 1;
		lblCreateDate.Text = "上傳日期";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(3, 34);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(340, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(340, 26);
		label1.TabIndex = 0;
		label1.Text = "上傳日期";
		label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(781, 586);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmDisposePrint";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "撤消標籤";
		base.Load += new System.EventHandler(frmDisposePrint_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		pDispose.ResumeLayout(false);
		pDispose.PerformLayout();
		ResumeLayout(false);
	}
}
