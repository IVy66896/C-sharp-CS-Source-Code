using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;

public class frmPrint : Form
{
	private int _MAXCOMMNAME = 28;

	private string _ReceiveOrg;

	private string _PreTraceCode;

	private string _PrintDataPath;

	private string _PrintDataFilePrefix;

	private string _Producer;

	private string _SourceProducer;

	private string _AuthName;

	private IContainer components;

	private Panel panelPrint;

	private Label label1;

	private Label lblCreateDate;

	private Label lblPackDate;

	private Label label2;

	private Label lblAvailableForPrinting;

	private Label label14;

	private Label lblTraceCode;

	private Label label10;

	private Label lblProductName;

	private Label label8;

	private Label lblFarmerName;

	private Label label6;

	private Label lblAuthName;

	private Label label4;

	private TextBox txtNumToPrint;

	private Label label17;

	private Label lblAlreadyPrinted;

	private Label label16;

	private Button btnCancel;

	private Button btnPrint;

	private Label lblEanCode;

	private Label label5;

	private Label lblQRCodeUrl;

	private Label lblPrtCodeToStart;

	private Label label7;

	private Label lblUnit;

	private Label label9;

	private Label lblExpDate;

	private Label label11;

	private Label lblPrinterName;

	private Label label12;

	private TextBox txtCommName;

	private Label lblCommName;

	private DateTimePicker dtExpDate;

	private TextBox txtUserInput;

	private Label lblUserInputDesc;

	private ComboBox cbJetDefaultExePath;

	private TextBox txtFarmerTel;

	private Label lblFarmerTel;

	private TextBox txtFarmerAddr;

	private Label lblFarmerAddr;

	public frmPrint(string CreateDate, string PackDate, string FarmerName, string ProductName, string TraceCode, string EanCode, string QRCodeUrl, string AvailableForPrinting, string AlreadyPrinted, string PrintCodeSrt, string AuthName, string Unit, string ExpDate, string ReceiveOrg, string PreTraceCode, string Producer, string SourceProducer)
	{
		InitializeComponent();
		Program.ReloadCRC();
		lblCreateDate.Text = CreateDate;
		lblPackDate.Text = PackDate;
		lblAuthName.Text = AuthName;
		lblFarmerName.Text = FarmerName;
		lblProductName.Text = ProductName;
		lblUnit.Text = Unit;
		lblTraceCode.Text = TraceCode;
		lblEanCode.Text = EanCode;
		lblQRCodeUrl.Text = QRCodeUrl;
		lblAvailableForPrinting.Text = AvailableForPrinting;
		lblAlreadyPrinted.Text = AlreadyPrinted;
		_ReceiveOrg = ReceiveOrg;
		_PreTraceCode = PreTraceCode;
		_Producer = Producer;
		_PrintDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		_PrintDataFilePrefix = "Filename";
		_SourceProducer = SourceProducer;
		_AuthName = AuthName;
		lblPrtCodeToStart.Text = PrintCodeUtilties.printCodeTransform(PrintCodeSrt);
		if (Program.ThisPrintFormat == PrintFormat.噴墨列印)
		{
			lblPrinterName.Text = Program.UserSettings.JetPrintSettings.JetExeDefaultPath;
		}
		else
		{
			lblPrinterName.Text = ((Program.PrinterName == string.Empty) ? "系統預設印表機" : Program.PrinterName);
		}
		try
		{
			lblExpDate.Text = ((Program.ExpireDateAccum > 0) ? string.Empty : Convert.ToDateTime(ExpDate).ToString("yyyy/MM/dd"));
			dtExpDate.Value = ((Program.ExpireDateAccum > 0) ? Convert.ToDateTime(PackDate).AddDays(Program.ExpireDateAccum) : DateTime.Now);
		}
		catch
		{
			lblExpDate.Text = "";
			dtExpDate.Value = DateTime.Now;
		}
		txtUserInput.Text = string.Empty;
		txtUserInput.Enabled = false;
		if ((Program.ThisPrintFormat == PrintFormat.標籤45X52顯示保存日期 || Program.ThisPrintFormat == PrintFormat.標籤2品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN加自行輸入 || Program.ThisPrintFormat == PrintFormat.標籤7品項 || Program.ThisPrintFormat == PrintFormat.噴墨列印) && Program.UserSettings.EnableUserInput)
		{
			if (Program.UserSettings.UserInputOption == 0)
			{
				txtUserInput.Enabled = true;
				txtUserInput.Text = Program.UserSettings.UserInputDfut;
				if (Program.UserSettings.UserInputDesc.Length > 0)
				{
					lblUserInputDesc.Text = Program.UserSettings.UserInputDesc;
				}
				else
				{
					lblUserInputDesc.Text = "自訂欄位";
				}
			}
			else
			{
				txtUserInput.Text = Unit;
				txtUserInput.Enabled = false;
				lblUserInputDesc.Text = "規格";
			}
		}
		if (Program.ThisPrintFormat == PrintFormat.標籤45X52含生產者資訊 || Program.ThisPrintFormat == PrintFormat.標籤9品項 || Program.ThisPrintFormat == PrintFormat.標籤75X42含生產者資訊 || Program.ThisPrintFormat == PrintFormat.標籤10品項)
		{
			txtFarmerAddr.Enabled = true;
			txtFarmerTel.Enabled = true;
			try
			{
				DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "FarmerAddress,FarmerTel", "FarmerData", "FarmerName={0}", "", null, new string[1]
				{
					FarmerName
				}, CommandOperationType.ExecuteReaderReturnDataTable);
				if (dataTable.Rows.Count > 0)
				{
					txtFarmerAddr.Text = dataTable.Rows[0]["FarmerAddress"].ToString();
					txtFarmerTel.Text = dataTable.Rows[0]["FarmerTel"].ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		if (txtFarmerAddr.Enabled && txtFarmerTel.Enabled)
		{
			if (txtFarmerAddr.Text.Length <= 0 || txtFarmerTel.Text.Length <= 0)
			{
				MessageBox.Show(string.Format("請輸入{0}", "農民地址及電話"));
				return;
			}
			try
			{
				if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "FarmerAddress,FarmerTel", "FarmerData", "FarmerName={0}", "", null, new string[1]
				{
					lblFarmerName.Text
				}, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count == 0)
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "FarmerData", "", "", new string[3, 2]
					{
						{
							"FarmerName",
							lblFarmerName.Text
						},
						{
							"FarmerAddress",
							txtFarmerAddr.Text
						},
						{
							"FarmerTel",
							txtFarmerTel.Text
						}
					}, null, CommandOperationType.ExecuteNonQuery);
				}
				else
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "FarmerData", "FarmerName={0}", "", new string[2, 2]
					{
						{
							"FarmerAddress",
							txtFarmerAddr.Text
						},
						{
							"FarmerTel",
							txtFarmerTel.Text
						}
					}, new string[1]
					{
						lblFarmerName.Text
					}, CommandOperationType.ExecuteNonQuery);
				}
			}
			catch
			{
			}
		}
		if (txtUserInput.Enabled && txtUserInput.Text.Length == 0)
		{
			MessageBox.Show($"請輸入{lblUserInputDesc.Text}");
		}
		if (CommonUtilities.isInteger(txtNumToPrint.Text) && CommonUtilities.isInteger(lblAvailableForPrinting.Text) && CommonUtilities.isInteger(lblAlreadyPrinted.Text))
		{
			int num = Convert.ToInt32(txtNumToPrint.Text);
			int num2 = Convert.ToInt32(lblAvailableForPrinting.Text);
			if (Program.ThisPrintFormat == PrintFormat.標籤45X52顯示保存日期 || Program.ThisPrintFormat == PrintFormat.標籤2品項 || Program.ThisPrintFormat == PrintFormat.畜產標籤 || Program.ThisPrintFormat == PrintFormat.標籤5品項)
			{
				_MAXCOMMNAME = 28;
			}
			else if (Program.ThisPrintFormat == PrintFormat.標籤45X52 || Program.ThisPrintFormat == PrintFormat.標籤1品項)
			{
				_MAXCOMMNAME = 32;
			}
			else if (Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN || Program.ThisPrintFormat == PrintFormat.標籤3品項)
			{
				_MAXCOMMNAME = 33;
			}
			else if (Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN加自行輸入 || Program.ThisPrintFormat == PrintFormat.標籤45X52含生產者資訊 || Program.ThisPrintFormat == PrintFormat.標籤7品項 || Program.ThisPrintFormat == PrintFormat.標籤9品項)
			{
				_MAXCOMMNAME = 42;
			}
			if (num < 1)
			{
				MessageBox.Show("列印數量至少1張");
			}
			else if (num > num2)
			{
				MessageBox.Show("列印張數不得大於可列印數量(" + lblAvailableForPrinting.Text + ")");
			}
			else if ((Program.ThisPrintFormat == PrintFormat.畜產標籤 || Program.ThisPrintFormat == PrintFormat.標籤5品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN || Program.ThisPrintFormat == PrintFormat.標籤3品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52 || Program.ThisPrintFormat == PrintFormat.標籤1品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52顯示保存日期 || Program.ThisPrintFormat == PrintFormat.標籤2品項) && lblProductName.Text.Length + txtCommName.Text.Length > _MAXCOMMNAME)
			{
				MessageBox.Show($"品項俗名最多只能輸入{_MAXCOMMNAME - lblProductName.Text.Length}個字元");
			}
			else if (MessageBox.Show($"您要列印的包裝日期為: {lblPackDate.Text}, 請問是否正確?", "確認列印日期", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				DoPrint(lblCreateDate.Text, lblFarmerName.Text, lblProductName.Text, lblAuthName.Text, lblPackDate.Text, lblTraceCode.Text, lblEanCode.Text, lblQRCodeUrl.Text, (Program.ExpireDateAccum > 0) ? dtExpDate.Value.ToString("yyyy/MM/dd") : lblExpDate.Text, num, txtCommName.Text);
			}
		}
		else if (!CommonUtilities.isInteger(txtNumToPrint.Text))
		{
			MessageBox.Show("列印數量請輸入數字");
		}
	}

	private void DoPrint(string CreateDate, string FarmerName, string ProductName, string AuthName, string PackDate, string TraceCode, string EanCode, string QRCodeUrl, string ExpDate, int NumToPrint, string commName)
	{
		string[] strWhereParameterArray = new string[5]
		{
			PackDate,
			FarmerName,
			ProductName,
			TraceCode,
			QRCodeUrl
		};
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd", "ResumeCurrent", "PackDate = {0} and FarmerName = {1} and ProductName = {2} and TraceCode = {3} and QRCodeUrl = {4}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			MessageBox.Show("沒有可列印的序號");
			return;
		}
		int num = 0;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			num += Convert.ToInt32(Convert.ToInt64(dataTable.Rows[i][1]) - Convert.ToInt64(dataTable.Rows[i][0]) + 1);
		}
		if (num < NumToPrint)
		{
			MessageBox.Show("可列印序號不足");
			return;
		}
		int num2 = 0;
		for (int j = 0; j < dataTable.Rows.Count; j++)
		{
			if (NumToPrint <= 0)
			{
				break;
			}
			int num3 = Convert.ToInt32(Convert.ToInt64(dataTable.Rows[j][1]) - Convert.ToInt64(dataTable.Rows[j][0]) + 1);
			if (num3 >= NumToPrint)
			{
				num2 = NumToPrint;
				NumToPrint = 0;
			}
			else
			{
				num2 = num3;
				NumToPrint -= num2;
			}
			if (num2 > 0)
			{
				strWhereParameterArray = new string[7]
				{
					PackDate,
					FarmerName,
					ProductName,
					TraceCode,
					QRCodeUrl,
					dataTable.Rows[j][0].ToString(),
					dataTable.Rows[j][1].ToString()
				};
				int num4 = Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "TotalPrinted", "ResumeCurrent", "PackDate= {0} and FarmerName = {1} and ProductName = {2} and TraceCode = {3} and QRCodeUrl = {4} and PrintCodeSrt = {5} and  PrintCodeEnd = {6}", "", null, strWhereParameterArray, CommandOperationType.ExecuteScalar));
				strWhereParameterArray = new string[7]
				{
					PackDate,
					FarmerName,
					ProductName,
					TraceCode,
					QRCodeUrl,
					dataTable.Rows[j][0].ToString(),
					dataTable.Rows[j][1].ToString()
				};
				string[,] strFieldArray = new string[3, 2]
				{
					{
						"PrintCodeSrt",
						PrintCodeUtilties.GetNextNPrintCode(dataTable.Rows[j][0].ToString(), num2 + 1)
					},
					{
						"PrintCodeEnd",
						dataTable.Rows[j][1].ToString()
					},
					{
						"TotalPrinted",
						Convert.ToString(num4 + num2)
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "ResumeCurrent", "PackDate= {0} and FarmerName = {1} and ProductName = {2} and TraceCode = {3} and QRCodeUrl = {4} and PrintCodeSrt = {5} and  PrintCodeEnd = {6}", "", strFieldArray, strWhereParameterArray, CommandOperationType.ExecuteNonQuery);
				Program.WriteCRC();
				try
				{
					string currentPC = dataTable.Rows[j][0].ToString();
					string userInputLine = string.Empty;
					if ((Program.ThisPrintFormat == PrintFormat.標籤45X52顯示保存日期 || Program.ThisPrintFormat == PrintFormat.標籤2品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN加自行輸入 || Program.ThisPrintFormat == PrintFormat.標籤7品項) && Program.UserSettings.EnableUserInput)
					{
						userInputLine = ((Program.UserSettings.UserInputOption != 0) ? $"{lblUserInputDesc.Text}: {txtUserInput.Text}" : ((Program.UserSettings.UserInputDesc.Length <= 0) ? txtUserInput.Text : $"{Program.UserSettings.UserInputDesc}: {txtUserInput.Text}"));
					}
					else if (Program.ThisPrintFormat == PrintFormat.標籤45X52含生產者資訊 || Program.ThisPrintFormat == PrintFormat.標籤9品項 || Program.ThisPrintFormat == PrintFormat.標籤75X42含生產者資訊 || Program.ThisPrintFormat == PrintFormat.標籤10品項)
					{
						userInputLine = $"生產者：{lblFarmerName.Text}＠地址：{txtFarmerAddr.Text}＠電話：{txtFarmerTel.Text}";
					}
					if (Program.ThisPrintFormat == PrintFormat.噴墨列印)
					{
						StringBuilder stringBuilder = new StringBuilder();
						int num5 = num2 % Program.UserSettings.JetPrintSettings.MaxPerFile;
						int num6 = num2 / Program.UserSettings.JetPrintSettings.MaxPerFile + ((num5 > 0) ? 1 : 0);
						int num7 = 1;
						if (num6 > 1 || (num6 == 1 && num5 == 0))
						{
							for (int k = 1; k <= num6; k++)
							{
								stringBuilder = new StringBuilder();
								for (int l = 0; l < Program.UserSettings.JetPrintSettings.MaxPerFile; l++)
								{
									string text = PrintCodeUtilties.printCodeTransform(PrintCodeUtilties.GetNextNPrintCode(currentPC, num7));
									string text2 = "http://tqr.tw/?t=" + text;
									stringBuilder.AppendLine(string.Format(Program.UserSettings.JetPrintSettings.PrintFormatter, "上傳日期", CreateDate.Replace("/", string.Empty), "包裝日期", PackDate.Replace("/", string.Empty), "有效日期", ExpDate.Replace("/", string.Empty), "農民", lblFarmerName.Text, "收貨單位", _ReceiveOrg, "品項名稱", ProductName, "規格", lblUnit.Text, "追溯號碼", text, "EAN", EanCode, "原栽培編號", _PreTraceCode, lblUserInputDesc.Text, txtUserInput.Text, "QR碼網址", text2, "原料生產單位", _SourceProducer, "驗證單位", _AuthName));
									num7++;
								}
								CommonUtilities.WriteToFile($"{_PrintDataPath}\\{_PrintDataFilePrefix}-{k:000}.txt", stringBuilder.ToString(), 'W', "Big5");
							}
						}
						stringBuilder = new StringBuilder();
						if (num5 > 0)
						{
							if (num7 > Program.UserSettings.JetPrintSettings.MaxPerFile)
							{
								num7 -= Program.UserSettings.JetPrintSettings.MaxPerFile;
							}
							for (int m = 0; m < num5; m++)
							{
								string text3 = PrintCodeUtilties.printCodeTransform(PrintCodeUtilties.GetNextNPrintCode(currentPC, num7));
								string text4 = "http://tqr.tw/?t=" + text3;
								stringBuilder.AppendLine(string.Format(Program.UserSettings.JetPrintSettings.PrintFormatter, "上傳日期", CreateDate.Replace("/", string.Empty), "包裝日期", PackDate.Replace("/", string.Empty), "有效日期", ExpDate.Replace("/", string.Empty), "農民", lblFarmerName.Text, "收貨單位", _ReceiveOrg, "品項名稱", ProductName, "規格", lblUnit.Text, "追溯號碼", text3, "EAN", EanCode, "原栽培編號", _PreTraceCode, lblUserInputDesc.Text, txtUserInput.Text, "QR碼網址", text4, "原料生產單位", _SourceProducer, "驗證單位", _AuthName));
								num7++;
							}
							CommonUtilities.WriteToFile($"{_PrintDataPath}\\{_PrintDataFilePrefix}-{num6:000}.txt", stringBuilder.ToString(), 'W', "Big5");
						}
						Process process = new Process();
						try
						{
							ProcessStartInfo processStartInfo = new ProcessStartInfo(Convert.ToString(cbJetDefaultExePath.Items[cbJetDefaultExePath.SelectedIndex]));
							processStartInfo.WorkingDirectory = _PrintDataPath;
							processStartInfo.Arguments = $"\"{_PrintDataPath}\\{_PrintDataFilePrefix}-001.txt\" \"{num6}\"";
							processStartInfo.UseShellExecute = false;
							processStartInfo.RedirectStandardOutput = true;
							processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
							processStartInfo.CreateNoWindow = true;
							process.StartInfo = processStartInfo;
							process.Start();
							btnPrint.Enabled = false;
						}
						catch (Exception ex)
						{
							MessageBox.Show($"執行exe檔失敗:「{ex.ToString()}」。");
						}
					}
					else
					{
						if (!Program.UserSettings.ShowPackDate)
						{
							PackDate = "";
						}
						for (int n = 1; n <= num2; n++)
						{
							string text5 = PrintCodeUtilties.printCodeTransform(PrintCodeUtilties.GetNextNPrintCode(currentPC, n));
							QRCodeUrl = "http://tqr.tw/?t=" + text5;
							new QRCode(QRCodeUrl, ProductName, AuthName, PackDate, ExpDate, text5, 1, EanCode, Program.ThisPrintFormat, Program.PrinterName, Convert.ToInt16(1), _Producer, commName, Program.UserSettings.ExpDateDesc, userInputLine, traceCodeWithSlash: true, 0, 0, 0, 0, Program.UserSettings.IsRotate).Print();
						}
					}
					num2 = 0;
				}
				catch (Exception)
				{
					MessageBox.Show("列印過程中發生錯誤，請檢查印表機設定。");
				}
			}
		}
		Close();
	}

	private void UpdateJetExeList(List<string> lst)
	{
		cbJetDefaultExePath.Items.Clear();
		foreach (string item in lst)
		{
			cbJetDefaultExePath.Items.Add(item);
		}
	}

	private void txtNumToPrint_TextChanged(object sender, EventArgs e)
	{
	}

	private void frmPrint_Load(object sender, EventArgs e)
	{
		txtCommName.Text = string.Empty;
		txtCommName.Enabled = (Program.ThisPrintFormat == PrintFormat.畜產標籤 || Program.ThisPrintFormat == PrintFormat.標籤5品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN || Program.ThisPrintFormat == PrintFormat.標籤3品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52 || Program.ThisPrintFormat == PrintFormat.標籤1品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52顯示保存日期 || Program.ThisPrintFormat == PrintFormat.標籤2品項 || Program.ThisPrintFormat == PrintFormat.標籤45X52無EAN加自行輸入 || Program.ThisPrintFormat == PrintFormat.標籤7品項);
		if (Program.ThisPrintFormat == PrintFormat.噴墨列印)
		{
			lblPrinterName.Visible = false;
			cbJetDefaultExePath.Visible = true;
			UpdateJetExeList(Program.UserSettings.JetPrintSettings.JetExecPaths);
			cbJetDefaultExePath.SelectedIndex = cbJetDefaultExePath.Items.IndexOf(Program.UserSettings.JetPrintSettings.JetExeDefaultPath);
		}
		else
		{
			lblPrinterName.Visible = true;
			cbJetDefaultExePath.Visible = false;
		}
		dtExpDate.Visible = (Program.ExpireDateAccum > 0);
		lblExpDate.Visible = !dtExpDate.Visible;
		VerifyPrinter();
		txtNumToPrint.Select();
	}

	private void VerifyPrinter()
	{
		try
		{
			if (!(Program.PrinterName == string.Empty))
			{
				foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
				{
					if (installedPrinter == Program.PrinterName)
					{
						return;
					}
				}
				MessageBox.Show("無法取得列印標籤預設印表機，請重新設定。");
				frmSetParameter frmSetParameter = new frmSetParameter();
				frmSetParameter.ShowDialog(this);
				_ = 1;
				frmSetParameter.Dispose();
				Close();
			}
		}
		catch (Exception)
		{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmPrint));
		panelPrint = new System.Windows.Forms.Panel();
		cbJetDefaultExePath = new System.Windows.Forms.ComboBox();
		txtUserInput = new System.Windows.Forms.TextBox();
		lblUserInputDesc = new System.Windows.Forms.Label();
		dtExpDate = new System.Windows.Forms.DateTimePicker();
		txtCommName = new System.Windows.Forms.TextBox();
		lblCommName = new System.Windows.Forms.Label();
		txtNumToPrint = new System.Windows.Forms.TextBox();
		lblPrinterName = new System.Windows.Forms.Label();
		label12 = new System.Windows.Forms.Label();
		lblExpDate = new System.Windows.Forms.Label();
		label11 = new System.Windows.Forms.Label();
		lblUnit = new System.Windows.Forms.Label();
		label9 = new System.Windows.Forms.Label();
		lblPrtCodeToStart = new System.Windows.Forms.Label();
		label7 = new System.Windows.Forms.Label();
		lblEanCode = new System.Windows.Forms.Label();
		label5 = new System.Windows.Forms.Label();
		btnCancel = new System.Windows.Forms.Button();
		btnPrint = new System.Windows.Forms.Button();
		label17 = new System.Windows.Forms.Label();
		lblAlreadyPrinted = new System.Windows.Forms.Label();
		label16 = new System.Windows.Forms.Label();
		lblAvailableForPrinting = new System.Windows.Forms.Label();
		label14 = new System.Windows.Forms.Label();
		lblTraceCode = new System.Windows.Forms.Label();
		label10 = new System.Windows.Forms.Label();
		lblProductName = new System.Windows.Forms.Label();
		label8 = new System.Windows.Forms.Label();
		lblFarmerName = new System.Windows.Forms.Label();
		label6 = new System.Windows.Forms.Label();
		lblAuthName = new System.Windows.Forms.Label();
		label4 = new System.Windows.Forms.Label();
		lblPackDate = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		lblCreateDate = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		lblQRCodeUrl = new System.Windows.Forms.Label();
		txtFarmerAddr = new System.Windows.Forms.TextBox();
		lblFarmerAddr = new System.Windows.Forms.Label();
		lblFarmerTel = new System.Windows.Forms.Label();
		txtFarmerTel = new System.Windows.Forms.TextBox();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(txtFarmerTel);
		panelPrint.Controls.Add(lblFarmerTel);
		panelPrint.Controls.Add(txtFarmerAddr);
		panelPrint.Controls.Add(lblFarmerAddr);
		panelPrint.Controls.Add(cbJetDefaultExePath);
		panelPrint.Controls.Add(txtUserInput);
		panelPrint.Controls.Add(lblUserInputDesc);
		panelPrint.Controls.Add(dtExpDate);
		panelPrint.Controls.Add(txtCommName);
		panelPrint.Controls.Add(lblCommName);
		panelPrint.Controls.Add(txtNumToPrint);
		panelPrint.Controls.Add(lblPrinterName);
		panelPrint.Controls.Add(label12);
		panelPrint.Controls.Add(lblExpDate);
		panelPrint.Controls.Add(label11);
		panelPrint.Controls.Add(lblUnit);
		panelPrint.Controls.Add(label9);
		panelPrint.Controls.Add(lblPrtCodeToStart);
		panelPrint.Controls.Add(label7);
		panelPrint.Controls.Add(lblEanCode);
		panelPrint.Controls.Add(label5);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnPrint);
		panelPrint.Controls.Add(label17);
		panelPrint.Controls.Add(lblAlreadyPrinted);
		panelPrint.Controls.Add(label16);
		panelPrint.Controls.Add(lblAvailableForPrinting);
		panelPrint.Controls.Add(label14);
		panelPrint.Controls.Add(lblTraceCode);
		panelPrint.Controls.Add(label10);
		panelPrint.Controls.Add(lblProductName);
		panelPrint.Controls.Add(label8);
		panelPrint.Controls.Add(lblFarmerName);
		panelPrint.Controls.Add(label6);
		panelPrint.Controls.Add(lblAuthName);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(lblPackDate);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(lblCreateDate);
		panelPrint.Controls.Add(label1);
		panelPrint.Controls.Add(lblQRCodeUrl);
		panelPrint.Location = new System.Drawing.Point(12, 12);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(420, 577);
		panelPrint.TabIndex = 0;
		cbJetDefaultExePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cbJetDefaultExePath.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbJetDefaultExePath.FormattingEnabled = true;
		cbJetDefaultExePath.Location = new System.Drawing.Point(133, 340);
		cbJetDefaultExePath.Name = "cbJetDefaultExePath";
		cbJetDefaultExePath.Size = new System.Drawing.Size(279, 24);
		cbJetDefaultExePath.TabIndex = 81;
		txtUserInput.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtUserInput.Location = new System.Drawing.Point(132, 395);
		txtUserInput.Name = "txtUserInput";
		txtUserInput.Size = new System.Drawing.Size(280, 27);
		txtUserInput.TabIndex = 35;
		lblUserInputDesc.AutoSize = true;
		lblUserInputDesc.BackColor = System.Drawing.SystemColors.AppWorkspace;
		lblUserInputDesc.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblUserInputDesc.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		lblUserInputDesc.Location = new System.Drawing.Point(3, 396);
		lblUserInputDesc.MaximumSize = new System.Drawing.Size(200, 0);
		lblUserInputDesc.MinimumSize = new System.Drawing.Size(120, 0);
		lblUserInputDesc.Name = "lblUserInputDesc";
		lblUserInputDesc.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblUserInputDesc.Size = new System.Drawing.Size(120, 26);
		lblUserInputDesc.TabIndex = 36;
		lblUserInputDesc.Text = "自訂欄位";
		dtExpDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dtExpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		dtExpDate.Location = new System.Drawing.Point(140, 59);
		dtExpDate.Name = "dtExpDate";
		dtExpDate.Size = new System.Drawing.Size(269, 27);
		dtExpDate.TabIndex = 34;
		txtCommName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtCommName.Location = new System.Drawing.Point(132, 366);
		txtCommName.Name = "txtCommName";
		txtCommName.Size = new System.Drawing.Size(280, 27);
		txtCommName.TabIndex = 32;
		lblCommName.AutoSize = true;
		lblCommName.BackColor = System.Drawing.SystemColors.AppWorkspace;
		lblCommName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblCommName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		lblCommName.Location = new System.Drawing.Point(3, 367);
		lblCommName.MaximumSize = new System.Drawing.Size(200, 0);
		lblCommName.MinimumSize = new System.Drawing.Size(120, 0);
		lblCommName.Name = "lblCommName";
		lblCommName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblCommName.Size = new System.Drawing.Size(120, 26);
		lblCommName.TabIndex = 33;
		lblCommName.Text = "品項俗名";
		txtNumToPrint.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtNumToPrint.Location = new System.Drawing.Point(132, 481);
		txtNumToPrint.Name = "txtNumToPrint";
		txtNumToPrint.Size = new System.Drawing.Size(280, 27);
		txtNumToPrint.TabIndex = 40;
		txtNumToPrint.TextChanged += new System.EventHandler(txtNumToPrint_TextChanged);
		lblPrinterName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPrinterName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPrinterName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPrinterName.Location = new System.Drawing.Point(129, 338);
		lblPrinterName.MaximumSize = new System.Drawing.Size(280, 26);
		lblPrinterName.MinimumSize = new System.Drawing.Size(280, 26);
		lblPrinterName.Name = "lblPrinterName";
		lblPrinterName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPrinterName.Size = new System.Drawing.Size(280, 26);
		lblPrinterName.TabIndex = 31;
		lblPrinterName.Text = "印表機名稱";
		label12.AutoSize = true;
		label12.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label12.Location = new System.Drawing.Point(3, 338);
		label12.MaximumSize = new System.Drawing.Size(200, 0);
		label12.MinimumSize = new System.Drawing.Size(120, 0);
		label12.Name = "label12";
		label12.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label12.Size = new System.Drawing.Size(120, 26);
		label12.TabIndex = 30;
		label12.Text = "印表機名稱";
		lblExpDate.BackColor = System.Drawing.SystemColors.ControlLight;
		lblExpDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblExpDate.ForeColor = System.Drawing.SystemColors.ControlText;
		lblExpDate.Location = new System.Drawing.Point(129, 58);
		lblExpDate.MaximumSize = new System.Drawing.Size(280, 26);
		lblExpDate.MinimumSize = new System.Drawing.Size(280, 26);
		lblExpDate.Name = "lblExpDate";
		lblExpDate.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblExpDate.Size = new System.Drawing.Size(280, 26);
		lblExpDate.TabIndex = 29;
		lblExpDate.Text = "保存日期";
		label11.AutoSize = true;
		label11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label11.Location = new System.Drawing.Point(3, 58);
		label11.MaximumSize = new System.Drawing.Size(200, 0);
		label11.MinimumSize = new System.Drawing.Size(120, 0);
		label11.Name = "label11";
		label11.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label11.Size = new System.Drawing.Size(120, 26);
		label11.TabIndex = 28;
		label11.Text = "保存日期";
		lblUnit.BackColor = System.Drawing.SystemColors.ControlLight;
		lblUnit.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblUnit.ForeColor = System.Drawing.SystemColors.ControlText;
		lblUnit.Location = new System.Drawing.Point(129, 170);
		lblUnit.MaximumSize = new System.Drawing.Size(280, 26);
		lblUnit.MinimumSize = new System.Drawing.Size(280, 26);
		lblUnit.Name = "lblUnit";
		lblUnit.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblUnit.Size = new System.Drawing.Size(280, 26);
		lblUnit.TabIndex = 27;
		lblUnit.Text = "品項";
		label9.AutoSize = true;
		label9.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label9.Location = new System.Drawing.Point(3, 170);
		label9.MaximumSize = new System.Drawing.Size(200, 0);
		label9.MinimumSize = new System.Drawing.Size(120, 0);
		label9.Name = "label9";
		label9.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label9.Size = new System.Drawing.Size(120, 26);
		label9.TabIndex = 26;
		label9.Text = "規格";
		lblPrtCodeToStart.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPrtCodeToStart.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPrtCodeToStart.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPrtCodeToStart.Location = new System.Drawing.Point(129, 310);
		lblPrtCodeToStart.MaximumSize = new System.Drawing.Size(280, 26);
		lblPrtCodeToStart.MinimumSize = new System.Drawing.Size(280, 26);
		lblPrtCodeToStart.Name = "lblPrtCodeToStart";
		lblPrtCodeToStart.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPrtCodeToStart.Size = new System.Drawing.Size(280, 26);
		lblPrtCodeToStart.TabIndex = 25;
		lblPrtCodeToStart.Text = "標籤啟始編號";
		label7.AutoSize = true;
		label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label7.Location = new System.Drawing.Point(3, 310);
		label7.MaximumSize = new System.Drawing.Size(200, 0);
		label7.MinimumSize = new System.Drawing.Size(120, 0);
		label7.Name = "label7";
		label7.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label7.Size = new System.Drawing.Size(120, 26);
		label7.TabIndex = 24;
		label7.Text = "標籤啟始編號";
		lblEanCode.BackColor = System.Drawing.SystemColors.ControlLight;
		lblEanCode.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblEanCode.ForeColor = System.Drawing.SystemColors.ControlText;
		lblEanCode.Location = new System.Drawing.Point(129, 226);
		lblEanCode.MaximumSize = new System.Drawing.Size(280, 26);
		lblEanCode.MinimumSize = new System.Drawing.Size(280, 26);
		lblEanCode.Name = "lblEanCode";
		lblEanCode.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblEanCode.Size = new System.Drawing.Size(280, 26);
		lblEanCode.TabIndex = 22;
		lblEanCode.Text = "EAN";
		label5.AutoSize = true;
		label5.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label5.Location = new System.Drawing.Point(3, 226);
		label5.MaximumSize = new System.Drawing.Size(200, 0);
		label5.MinimumSize = new System.Drawing.Size(120, 0);
		label5.Name = "label5";
		label5.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label5.Size = new System.Drawing.Size(120, 26);
		label5.TabIndex = 21;
		label5.Text = "EAN";
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(219, 539);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnPrint.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnPrint.Location = new System.Drawing.Point(133, 539);
		btnPrint.Name = "btnPrint";
		btnPrint.Size = new System.Drawing.Size(80, 31);
		btnPrint.TabIndex = 41;
		btnPrint.Text = "列印";
		btnPrint.UseVisualStyleBackColor = true;
		btnPrint.Click += new System.EventHandler(btnPrint_Click);
		label17.AutoSize = true;
		label17.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label17.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label17.Location = new System.Drawing.Point(3, 482);
		label17.MaximumSize = new System.Drawing.Size(200, 0);
		label17.MinimumSize = new System.Drawing.Size(120, 0);
		label17.Name = "label17";
		label17.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label17.Size = new System.Drawing.Size(120, 26);
		label17.TabIndex = 18;
		label17.Text = "本次列印數量";
		lblAlreadyPrinted.BackColor = System.Drawing.SystemColors.ControlLight;
		lblAlreadyPrinted.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblAlreadyPrinted.ForeColor = System.Drawing.SystemColors.ControlText;
		lblAlreadyPrinted.Location = new System.Drawing.Point(129, 282);
		lblAlreadyPrinted.MaximumSize = new System.Drawing.Size(280, 26);
		lblAlreadyPrinted.MinimumSize = new System.Drawing.Size(280, 26);
		lblAlreadyPrinted.Name = "lblAlreadyPrinted";
		lblAlreadyPrinted.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblAlreadyPrinted.Size = new System.Drawing.Size(280, 26);
		lblAlreadyPrinted.TabIndex = 17;
		lblAlreadyPrinted.Text = "已列印數量";
		label16.AutoSize = true;
		label16.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label16.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label16.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label16.Location = new System.Drawing.Point(3, 282);
		label16.MaximumSize = new System.Drawing.Size(200, 0);
		label16.MinimumSize = new System.Drawing.Size(120, 0);
		label16.Name = "label16";
		label16.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label16.Size = new System.Drawing.Size(120, 26);
		label16.TabIndex = 16;
		label16.Text = "已列印數量";
		lblAvailableForPrinting.BackColor = System.Drawing.SystemColors.ControlLight;
		lblAvailableForPrinting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblAvailableForPrinting.ForeColor = System.Drawing.SystemColors.ControlText;
		lblAvailableForPrinting.Location = new System.Drawing.Point(129, 254);
		lblAvailableForPrinting.MaximumSize = new System.Drawing.Size(280, 26);
		lblAvailableForPrinting.MinimumSize = new System.Drawing.Size(280, 26);
		lblAvailableForPrinting.Name = "lblAvailableForPrinting";
		lblAvailableForPrinting.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblAvailableForPrinting.Size = new System.Drawing.Size(280, 26);
		lblAvailableForPrinting.TabIndex = 15;
		lblAvailableForPrinting.Text = "可列印數量";
		label14.AutoSize = true;
		label14.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label14.Location = new System.Drawing.Point(3, 254);
		label14.MaximumSize = new System.Drawing.Size(200, 0);
		label14.MinimumSize = new System.Drawing.Size(120, 0);
		label14.Name = "label14";
		label14.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label14.Size = new System.Drawing.Size(120, 26);
		label14.TabIndex = 14;
		label14.Text = "可列印數量";
		lblTraceCode.BackColor = System.Drawing.SystemColors.ControlLight;
		lblTraceCode.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblTraceCode.ForeColor = System.Drawing.SystemColors.ControlText;
		lblTraceCode.Location = new System.Drawing.Point(129, 198);
		lblTraceCode.MaximumSize = new System.Drawing.Size(280, 26);
		lblTraceCode.MinimumSize = new System.Drawing.Size(280, 26);
		lblTraceCode.Name = "lblTraceCode";
		lblTraceCode.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblTraceCode.Size = new System.Drawing.Size(280, 26);
		lblTraceCode.TabIndex = 11;
		lblTraceCode.Text = "Trace code";
		label10.AutoSize = true;
		label10.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label10.Location = new System.Drawing.Point(3, 198);
		label10.MaximumSize = new System.Drawing.Size(200, 0);
		label10.MinimumSize = new System.Drawing.Size(120, 0);
		label10.Name = "label10";
		label10.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label10.Size = new System.Drawing.Size(120, 26);
		label10.TabIndex = 10;
		label10.Text = "追溯號碼";
		lblProductName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblProductName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblProductName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblProductName.Location = new System.Drawing.Point(129, 142);
		lblProductName.MaximumSize = new System.Drawing.Size(280, 26);
		lblProductName.MinimumSize = new System.Drawing.Size(280, 26);
		lblProductName.Name = "lblProductName";
		lblProductName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblProductName.Size = new System.Drawing.Size(280, 26);
		lblProductName.TabIndex = 9;
		lblProductName.Text = "品項";
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 142);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(120, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(120, 26);
		label8.TabIndex = 8;
		label8.Text = "品項";
		lblFarmerName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblFarmerName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblFarmerName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblFarmerName.Location = new System.Drawing.Point(129, 114);
		lblFarmerName.MaximumSize = new System.Drawing.Size(280, 26);
		lblFarmerName.MinimumSize = new System.Drawing.Size(280, 26);
		lblFarmerName.Name = "lblFarmerName";
		lblFarmerName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblFarmerName.Size = new System.Drawing.Size(280, 26);
		lblFarmerName.TabIndex = 7;
		lblFarmerName.Text = "農民";
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label6.Location = new System.Drawing.Point(3, 114);
		label6.MaximumSize = new System.Drawing.Size(200, 0);
		label6.MinimumSize = new System.Drawing.Size(120, 0);
		label6.Name = "label6";
		label6.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label6.Size = new System.Drawing.Size(120, 26);
		label6.TabIndex = 6;
		label6.Text = "農民";
		lblAuthName.BackColor = System.Drawing.SystemColors.ControlLight;
		lblAuthName.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblAuthName.ForeColor = System.Drawing.SystemColors.ControlText;
		lblAuthName.Location = new System.Drawing.Point(129, 86);
		lblAuthName.MaximumSize = new System.Drawing.Size(280, 26);
		lblAuthName.MinimumSize = new System.Drawing.Size(280, 26);
		lblAuthName.Name = "lblAuthName";
		lblAuthName.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblAuthName.Size = new System.Drawing.Size(280, 26);
		lblAuthName.TabIndex = 5;
		lblAuthName.Text = "驗證機構";
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label4.Location = new System.Drawing.Point(3, 86);
		label4.MaximumSize = new System.Drawing.Size(200, 0);
		label4.MinimumSize = new System.Drawing.Size(120, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(120, 26);
		label4.TabIndex = 4;
		label4.Text = "驗證機構";
		lblPackDate.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPackDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPackDate.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPackDate.Location = new System.Drawing.Point(129, 30);
		lblPackDate.MaximumSize = new System.Drawing.Size(280, 26);
		lblPackDate.MinimumSize = new System.Drawing.Size(280, 26);
		lblPackDate.Name = "lblPackDate";
		lblPackDate.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPackDate.Size = new System.Drawing.Size(280, 26);
		lblPackDate.TabIndex = 3;
		lblPackDate.Text = "包裝日期";
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(3, 30);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(120, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(120, 26);
		label2.TabIndex = 2;
		label2.Text = "包裝日期";
		lblCreateDate.BackColor = System.Drawing.SystemColors.ControlLight;
		lblCreateDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblCreateDate.ForeColor = System.Drawing.SystemColors.ControlText;
		lblCreateDate.Location = new System.Drawing.Point(129, 2);
		lblCreateDate.MaximumSize = new System.Drawing.Size(280, 26);
		lblCreateDate.MinimumSize = new System.Drawing.Size(280, 26);
		lblCreateDate.Name = "lblCreateDate";
		lblCreateDate.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblCreateDate.Size = new System.Drawing.Size(280, 26);
		lblCreateDate.TabIndex = 1;
		lblCreateDate.Text = "上傳日期";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(3, 2);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(120, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(120, 26);
		label1.TabIndex = 0;
		label1.Text = "上傳日期";
		lblQRCodeUrl.BackColor = System.Drawing.SystemColors.ControlLight;
		lblQRCodeUrl.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblQRCodeUrl.ForeColor = System.Drawing.SystemColors.ControlText;
		lblQRCodeUrl.Location = new System.Drawing.Point(3, 539);
		lblQRCodeUrl.MaximumSize = new System.Drawing.Size(280, 26);
		lblQRCodeUrl.MinimumSize = new System.Drawing.Size(280, 26);
		lblQRCodeUrl.Name = "lblQRCodeUrl";
		lblQRCodeUrl.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblQRCodeUrl.Size = new System.Drawing.Size(280, 26);
		lblQRCodeUrl.TabIndex = 23;
		lblQRCodeUrl.Text = "lblQRCodeUrl";
		lblQRCodeUrl.Visible = false;
		txtFarmerAddr.Enabled = false;
		txtFarmerAddr.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtFarmerAddr.Location = new System.Drawing.Point(132, 424);
		txtFarmerAddr.Name = "txtFarmerAddr";
		txtFarmerAddr.Size = new System.Drawing.Size(280, 27);
		txtFarmerAddr.TabIndex = 37;
		lblFarmerAddr.AutoSize = true;
		lblFarmerAddr.BackColor = System.Drawing.SystemColors.AppWorkspace;
		lblFarmerAddr.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblFarmerAddr.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		lblFarmerAddr.Location = new System.Drawing.Point(3, 425);
		lblFarmerAddr.MaximumSize = new System.Drawing.Size(200, 0);
		lblFarmerAddr.MinimumSize = new System.Drawing.Size(120, 0);
		lblFarmerAddr.Name = "lblFarmerAddr";
		lblFarmerAddr.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblFarmerAddr.Size = new System.Drawing.Size(120, 26);
		lblFarmerAddr.TabIndex = 83;
		lblFarmerAddr.Text = "農民地址";
		lblFarmerTel.AutoSize = true;
		lblFarmerTel.BackColor = System.Drawing.SystemColors.AppWorkspace;
		lblFarmerTel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblFarmerTel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		lblFarmerTel.Location = new System.Drawing.Point(3, 454);
		lblFarmerTel.MaximumSize = new System.Drawing.Size(200, 0);
		lblFarmerTel.MinimumSize = new System.Drawing.Size(120, 0);
		lblFarmerTel.Name = "lblFarmerTel";
		lblFarmerTel.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblFarmerTel.Size = new System.Drawing.Size(120, 26);
		lblFarmerTel.TabIndex = 83;
		lblFarmerTel.Text = "農民電話";
		txtFarmerTel.Enabled = false;
		txtFarmerTel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtFarmerTel.Location = new System.Drawing.Point(132, 453);
		txtFarmerTel.Name = "txtFarmerTel";
		txtFarmerTel.Size = new System.Drawing.Size(280, 27);
		txtFarmerTel.TabIndex = 39;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(444, 601);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "frmPrint";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "列印";
		base.Load += new System.EventHandler(frmPrint_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
