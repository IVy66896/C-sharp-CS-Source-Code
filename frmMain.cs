# Csharp-coding

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;
using 離線列印Client程式.L1_1O;
using 離線列印Client程式.OfflinePrintWebService;
using 離線列印Client程式.Properties;

public class frmMain : Form
{
	private IContainer components;

	private StatusStrip SS_Status;

	private ToolStripStatusLabel lblStatus;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem 伺服器ToolStripMenuItem;

	private ToolStripProgressBar pbStatus;

	private ToolStripMenuItem 列印ToolStripMenuItem;

	private Label lblProducerName;

	private ToolStripMenuItem menuItemVersion;

	private ToolStripMenuItem 參數設定ToolStripMenuItem;

	private ToolStripMenuItem 清除歷史資料ToolStripMenuItem;

	public frmMain()
	{
		InitializeComponent();
		pbStatus.Visible = false;
		Text = "行政院農委會 產銷履歷標籤列印程式 (V" + Program.Version + "版)";
		if (Program.ProducerName != "")
		{
			lblProducerName.Text = Program.ProducerName;
		}
		else
		{
			lblProducerName.Text = "";
		}
	}

	private int ImportToDB(DataTable dt, string WSTime, List<int> resumeIDToDL)
	{
		if (dt == null || dt.Rows.Count == 0)
		{
			return 0;
		}
		pbStatus.Visible = true;
		pbStatus.Maximum = 1000;
		pbStatus.Minimum = 1;
		pbStatus.Value = 1;
		int num = pbStatus.Maximum / dt.Rows.Count;
		int num2 = 0;
		int num3 = 0;
		string strWhereClause = "ProductName = {0} and PackDate= {1} and FarmerName = {2} and AuthName = {3} and AuthName = {4} and TraceCode= {5} and EanCode = {6} and initPrintCodeSrt = {7} and initPrintCodeEnd = {8}";
		foreach (DataRow row in dt.Rows)
		{
			if (pbStatus.Value + num <= pbStatus.Maximum)
			{
				pbStatus.Value += num;
			}
			else
			{
				pbStatus.Value = pbStatus.Maximum;
			}
			Application.DoEvents();
			num2 = Convert.ToInt32(row["ID"]);
			if (resumeIDToDL.Contains(num2))
			{
				string text = row["ExpDate"].Equals(DBNull.Value) ? "NULL" : ((DateTime.Compare(Convert.ToDateTime(row["ExpDate"]), Convert.ToDateTime("1900/01/02")) > 0) ? Convert.ToDateTime(row["ExpDate"]).ToString("yyyy/MM/dd") : "NULL");
				string[,] strFieldArray = new string[20, 2]
				{
					{
						"ProductName",
						row["ProductName"].ToString()
					},
					{
						"PackDate",
						row["PackDate"].ToString()
					},
					{
						"CreateDate",
						row["CreateDate"].ToString()
					},
					{
						"FarmerName",
						row["FarmerName"].ToString()
					},
					{
						"AuthName",
						row["AuthName"].ToString()
					},
					{
						"Producer",
						row["Producer"].ToString()
					},
					{
						"TraceCode",
						row["TraceCode"].ToString()
					},
					{
						"EanCode",
						row["EanCode"].ToString()
					},
					{
						"QRCodeUrl",
						row["QRCodeUrl"].ToString()
					},
					{
						"initPrintCodeSrt",
						row["PrintCodeSrt"].ToString()
					},
					{
						"initPrintCodeEnd",
						row["PrintCodeEnd"].ToString()
					},
					{
						"initAvailablePrint",
						Convert.ToInt32(Convert.ToInt64(row["PrintCodeEnd"]) - Convert.ToInt64(row["PrintCodeSrt"]) + 1).ToString()
					},
					{
						"Unit",
						row["Unit"].ToString()
					},
					{
						"ExpDate",
						text
					},
					{
						"PreTraceCode",
						row["PreTraceCode"].ToString()
					},
					{
						"PrintCodeSrt",
						row["PrintCodeSrt"].ToString()
					},
					{
						"PrintCodeEnd",
						row["PrintCodeEnd"].ToString()
					},
					{
						"ReceiveOrg",
						row["ReceiveOrg"].ToString()
					},
					{
						"ResumeID",
						Program.NextResumeID.ToString()
					},
					{
						"SourceProducer",
						row["SourceProducer"].ToString()
					}
				};
				string[,] strFieldArray2 = new string[14, 2]
				{
					{
						"ProductName",
						row["ProductName"].ToString()
					},
					{
						"PackDate",
						row["PackDate"].ToString()
					},
					{
						"CreateDate",
						row["CreateDate"].ToString()
					},
					{
						"FarmerName",
						row["FarmerName"].ToString()
					},
					{
						"AuthName",
						row["AuthName"].ToString()
					},
					{
						"TraceCode",
						row["TraceCode"].ToString()
					},
					{
						"EanCode",
						row["EanCode"].ToString()
					},
					{
						"Unit",
						row["Unit"].ToString()
					},
					{
						"Producer",
						row["Producer"].ToString()
					},
					{
						"ExpDate",
						text
					},
					{
						"PreTraceCode",
						row["PreTraceCode"].ToString()
					},
					{
						"ReceiveOrg",
						row["ReceiveOrg"].ToString()
					},
					{
						"QRCodeUrl",
						row["QRCodeUrl"].ToString()
					},
					{
						"SourceProducer",
						row["SourceProducer"].ToString()
					}
				};
				string[] strWhereParameterArray = new string[9]
				{
					row["ProductName"].ToString(),
					row["PackDate"].ToString(),
					row["FarmerName"].ToString(),
					row["AuthName"].ToString(),
					row["AuthName"].ToString(),
					row["TraceCode"].ToString(),
					row["EanCode"].ToString(),
					row["PrintCodeSrt"].ToString(),
					row["PrintCodeEnd"].ToString()
				};
				if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "ResumeCurrent", strWhereClause, "", strFieldArray2, strWhereParameterArray, CommandOperationType.ExecuteNonQuery)) == 0)
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "ResumeCurrent", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
					Program.NextResumeID++;
				}
				num3++;
			}
		}
		pbStatus.Visible = false;
		return num3;
	}

	private void flushPrintedBackTo1O()
	{
		string wSTime = new Service
		{
			Url = Program.OLPUrl,
			Timeout = 800000
		}.GetWSTime();
		L1_1O l1_1O = new L1_1O();
		l1_1O.Url = Program.L1Url;
		l1_1O.Timeout = 800000;
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd, PintCodeCount, DisposeReason", "FlushPrintCode", "", "PrintCodeSrt ASC", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			pbStatus.Visible = true;
			pbStatus.Maximum = 1000;
			pbStatus.Minimum = 1;
			pbStatus.Value = 1;
			int num = pbStatus.Maximum / dataTable.Rows.Count;
			try
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					if (pbStatus.Value + num <= pbStatus.Maximum)
					{
						pbStatus.Value += num;
					}
					else
					{
						pbStatus.Value = pbStatus.Maximum;
					}
					Application.DoEvents();
					l1_1O.UploadData(GenFlushString(dataTable.Rows[i][0].ToString(), dataTable.Rows[i][1].ToString(), dataTable.Rows[i][2].ToString(), dataTable.Rows[i][3].Equals(DBNull.Value) ? string.Empty : dataTable.Rows[i][3].ToString(), wSTime));
				}
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Delete, "", "FlushPrintCode", "", "", null, null, CommandOperationType.ExecuteNonQuery);
			}
			catch
			{
			}
		}
	}

	private string GenFlushString(string PrintLabNumS, string PrintLabNumE, string PrintLabAmt, string DisposeReason, string WSdate)
	{
		StringBuilder stringBuilder = new StringBuilder("");
		stringBuilder.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		stringBuilder.AppendFormat("<PrintLabUseUpload>");
		stringBuilder.AppendFormat("<OrgID>{0}</OrgID>", Program.OrgID);
		stringBuilder.AppendFormat("<TrustCode><![CDATA[{0}]]></TrustCode>", Encrypt.EncryptAES(Program.OrgID + "," + WSdate, "offLINETrustCODE"));
		stringBuilder.AppendFormat("<SysCode>{0}</SysCode>", Program.SysSerialNo);
		stringBuilder.AppendFormat("   <PrintUpload>");
		stringBuilder.AppendFormat("       <PrintLabNumS>{0}</PrintLabNumS>", PrintLabNumS);
		stringBuilder.AppendFormat("       <PrintLabNumE>{0}</PrintLabNumE>", PrintLabNumE);
		stringBuilder.AppendFormat("       <PrintLabAmt>{0}</PrintLabAmt>", PrintLabAmt);
		if (DisposeReason != string.Empty)
		{
			stringBuilder.AppendFormat("       <MemoDescp>{0}</MemoDescp>", DisposeReason);
		}
		else
		{
			stringBuilder.AppendFormat("       <NeverPrint>{0}</NeverPrint>", "1");
		}
		stringBuilder.AppendFormat("   </PrintUpload>");
		stringBuilder.AppendFormat("</PrintLabUseUpload>");
		return stringBuilder.ToString();
	}

	private void 連接伺服器ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ConnectToWebService();
	}

	private void MoveDataToHistory(string WStime)
	{
		string text = Convert.ToDateTime(WStime).AddDays(Program.DayToFlush).ToString("yyyy/MM/dd");
		StringBuilder stringBuilder = new StringBuilder("");
		stringBuilder.AppendFormat("SELECT ProductName, PackDate, CreateDate, FarmerName, AuthName, TraceCode, EanCOde, QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, initAvailablePrint, PrintCodeSrt, PrintCodeEnd, Unit, Sum(TotalPrinted) as Printed, PreTraceCode, ExpDate, ResumeID, ReceiveOrg, Producer,SourceProducer ");
		stringBuilder.AppendFormat("FROM   ResumeCurrent ");
		stringBuilder.AppendFormat("WHERE  PackDate <  datevalue('{0}') ", text);
		stringBuilder.AppendFormat("Group by ProductName, PackDate, CreateDate, FarmerName, AuthName, TraceCode, EanCOde, QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, initAvailablePrint, PrintCodeSrt, PrintCodeEnd, Unit, PreTraceCode, ExpDate, ResumeID, ReceiveOrg, Producer,SourceProducer ");
		using (OleDbConnection oleDbConnection = new OleDbConnection(Program.ConnectionString))
		{
			using (OleDbCommand oleDbCommand = new OleDbCommand(stringBuilder.ToString(), oleDbConnection))
			{
				try
				{
					oleDbCommand.CommandTimeout = 0;
					oleDbConnection.Open();
					object obj = oleDbCommand.ExecuteReader();
					int num = 0;
					string text2 = "";
					string empty = string.Empty;
					if (obj != null)
					{
						DataTable dataTable = new DataTable();
						dataTable.Load((OleDbDataReader)obj);
						foreach (DataRow row in dataTable.Rows)
						{
							empty = (row["ExpDate"].Equals(DBNull.Value) ? "NULL" : ((DateTime.Compare(Convert.ToDateTime(row["ExpDate"]), Convert.ToDateTime("1900/01/02")) > 0) ? Convert.ToDateTime(row["ExpDate"]).ToString("yyyy/MM/dd") : "NULL"));
							string[,] strFieldArray = new string[19, 2]
							{
								{
									"ProductName",
									row["ProductName"].ToString()
								},
								{
									"PackDate",
									row["PackDate"].ToString()
								},
								{
									"CreateDate",
									row["CreateDate"].ToString()
								},
								{
									"FarmerName",
									row["FarmerName"].ToString()
								},
								{
									"AuthName",
									row["AuthName"].ToString()
								},
								{
									"TraceCode",
									row["TraceCode"].ToString()
								},
								{
									"EanCode",
									row["EanCode"].ToString()
								},
								{
									"QRCodeUrl",
									row["QRCodeUrl"].ToString()
								},
								{
									"Unit",
									row["Unit"].ToString()
								},
								{
									"Producer",
									row["Producer"].ToString()
								},
								{
									"ExpDate",
									empty
								},
								{
									"initPrintCodeSrt",
									row["initPrintCodeSrt"].ToString()
								},
								{
									"initPrintCodeEnd",
									row["initPrintCodeEnd"].ToString()
								},
								{
									"initAvailablePrint",
									row["initAvailablePrint"].ToString()
								},
								{
									"PreTraceCode",
									row["PreTraceCode"].ToString()
								},
								{
									"TotalPrinted",
									row["Printed"].ToString()
								},
								{
									"ReceiveOrg",
									row["ReceiveOrg"].ToString()
								},
								{
									"ResumeID",
									row["ResumeID"].ToString()
								},
								{
									"SourceProducer",
									row["SourceProducer"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "ResumeHistory", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							if (PrintCodeUtilties.GetNextNPrintCode(row["initPrintCodeEnd"].ToString(), 2) == row["PrintCodeSrt"].ToString())
							{
								num = 0;
								text2 = PrintCodeUtilties.GetNextNPrintCode(row["initPrintCodeEnd"].ToString(), 2);
							}
							else
							{
								num = (int)(Convert.ToInt64(row["initPrintCodeEnd"]) - Convert.ToInt64(row["PrintCodeSrt"])) + 1;
								text2 = row["PrintCodeSrt"].ToString();
							}
							strFieldArray = new string[4, 2]
							{
								{
									"PrintCodeSrt",
									text2
								},
								{
									"PrintCodeEnd",
									row["initPrintCodeEnd"].ToString()
								},
								{
									"PintCodeCount",
									num.ToString()
								},
								{
									"DisposeReason",
									"NULL"
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "FlushPrintCode", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							string[] strWhereParameterArray = new string[1]
							{
								row["ResumeID"].ToString()
							};
							foreach (DataRow row2 in ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd, PintCodeCount, DisposeReason", "ResumeDispose", "ResumeID = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable)).Rows)
							{
								strFieldArray = new string[4, 2]
								{
									{
										"PrintCodeSrt",
										row2["PrintCodeSrt"].ToString()
									},
									{
										"PrintCodeEnd",
										row2["PrintCodeEnd"].ToString()
									},
									{
										"PintCodeCount",
										row2["PintCodeCount"].ToString()
									},
									{
										"DisposeReason",
										row2["DisposeReason"].ToString()
									}
								};
								DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "FlushPrintCode", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							}
						}
					}
					string[] strWhereParameterArray2 = new string[1]
					{
						text
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Delete, "", "ResumeCurrent", "PackDate <  datevalue({0})", "", null, strWhereParameterArray2, CommandOperationType.ExecuteNonQuery);
				}
				catch (Exception)
				{
				}
			}
		}
	}

	private void ConnectToWebService()
	{
		try
		{
			Application.DoEvents();
			if (!NetworkInfo.IsConnectionExist(Program.WebServiceHostName1O))
			{
				Application.DoEvents();
				lblStatus.Text = "無法與" + (Program.IsHyweb ? "Hyweb測試機" : "主機") + "連線，請稍候重試。1";
				return;
			}
			if (!NetworkInfo.IsConnectionExist(Program.WebServiceHostNameOL))
			{
				Application.DoEvents();
				lblStatus.Text = "無法與" + (Program.IsHyweb ? "Hyweb測試機" : "主機") + "連線，請稍候重試。2";
				return;
			}
			列印ToolStripMenuItem.Enabled = false;
			伺服器ToolStripMenuItem.Enabled = false;
			參數設定ToolStripMenuItem.Enabled = false;
			Application.DoEvents();
			Service service = new Service();
			service.Url = Program.OLPUrl;
			service.Timeout = 800000;
			Program.ReloadCRC();
			if (Program.CRCStatus != "CRCLogOK")
			{
				service.ReportC(Program.SysSerialNo, Program.CRCStatus);
				Program.CRCStatus = "CRCLogOK";
				Program.WriteCRC();
			}
			if (Program.ProducerName == "")
			{
				Program.ProducerName = service.GetOrgName(Program.SysSerialNo);
				string[,] strFieldArray = new string[1, 2]
				{
					{
						"ProducerName",
						Program.ProducerName
					}
				};
				if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery)) == 0)
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
				}
				if (Program.ProducerName != "")
				{
					lblProducerName.Text = Program.ProducerName;
				}
			}
			if (Program.OrgID == "")
			{
				Program.OrgID = service.GetOrgID(Program.SysSerialNo);
				string[,] strFieldArray2 = new string[1, 2]
				{
					{
						"OrgID",
						Program.OrgID
					}
				};
				if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray2, null, CommandOperationType.ExecuteNonQuery)) == 0)
				{
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray2, null, CommandOperationType.ExecuteNonQuery);
				}
			}
			if (Program.UploadNewSysSN)
			{
				Program.UploadNewSysSN = (service.UpdateSysSN(Program.PrevSysSerialNo, Program.SysSerialNo) == "0");
			}
			lblStatus.Text = "連線" + (Program.IsHyweb ? "Hyweb測試機" : "主機") + "中…";
			Application.DoEvents();
			string wSTime = service.GetWSTime();
			try
			{
				service.ReportVerNo(Program.SysSerialNo, Program.Version);
			}
			catch
			{
			}
			lblStatus.Text = "DB Handling ...請稍後...";
			Application.DoEvents();
			if (service.IsUploadDB(Program.SysSerialNo) == "1")
			{
				string path = ApplicationDeployment.CurrentDeployment.DataDirectory.ToString() + "\\DB.mdb";
				try
				{
					service.Timeout = 60000;
					service.UploadDB(Program.SysSerialNo, File.ReadAllBytes(path));
				}
				catch (Exception ex)
				{
					MessageBox.Show("DB Handling error!" + ex.Message);
				}
			}
			DataSet printCodes = service.GetPrintCodes(Program.SysSerialNo);
			int num = 0;
			List<int> list = new List<int>();
			if (printCodes != null)
			{
				DataTable dataTable = printCodes.Tables[0];
				lblStatus.Text = "取得資料中…";
				Application.DoEvents();
				if (Program.UserSettings.DontDownloadAll)
				{
					list = ShowDownLoadedPrintCodesForSelect(dataTable);
					num = ImportToDB(dataTable, wSTime, list);
				}
				else
				{
					foreach (DataRow row in dataTable.Rows)
					{
						list.Add(Convert.ToInt32(row["ID"]));
					}
					num = ImportToDB(dataTable, wSTime, list);
				}
			}
			pbStatus.Visible = false;
			lblStatus.Text = "本次共取得" + num + "筆資料";
			try
			{
				service.DledPrintCodes(Program.SysSerialNo, list.ToArray());
			}
			catch (Exception)
			{
			}
			Application.DoEvents();
			MessageBox.Show("取得資料完成!");
			MoveDataToHistory(wSTime);
			lblStatus.Text = "註銷己列印標籤…";
			Application.DoEvents();
			flushPrintedBackTo1O();
			lblStatus.Text = "取得列印標籤…";
			Application.DoEvents();
			lblStatus.Text = "本次共取得" + num + "筆資料";
			Application.DoEvents();
			Program.WriteCRC();
			ShowPrintListDialog();
		}
		catch (Exception ex3)
		{
			lblStatus.Text = "無法與" + (Program.IsHyweb ? "Hyweb測試機" : "主機") + "連線，請稍候重試。";
			MessageBox.Show(ex3.Message + ex3.StackTrace);
		}
		列印ToolStripMenuItem.Enabled = true;
		伺服器ToolStripMenuItem.Enabled = true;
		參數設定ToolStripMenuItem.Enabled = true;
	}

	private void 列印標籤ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ShowPrintListDialog();
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		if (Program.IsDeployClickOnce)
		{
			ShowUpdateDialog();
		}
	}

	private void frmMain_Shown(object sender, EventArgs e)
	{
		if (MessageBox.Show("是否立即取得可列印標籤?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			ConnectToWebService();
		}
	}

	private List<int> ShowDownLoadedPrintCodesForSelect(DataTable dt)
	{
		frmListPrintCodeForSelect frmListPrintCodeForSelect = new frmListPrintCodeForSelect(dt);
		List<int> result = new List<int>();
		if (frmListPrintCodeForSelect.ShowDialog(this) == DialogResult.OK)
		{
			MessageBox.Show($"您選擇了{frmListPrintCodeForSelect.ResumeIDSelected.Count}筆資料。");
			result = frmListPrintCodeForSelect.ResumeIDSelected;
		}
		frmListPrintCodeForSelect.Dispose();
		return result;
	}

	private void ShowPrintListDialog()
	{
		frmList frmList = new frmList();
		frmList.ShowDialog(this);
		_ = 1;
		frmList.Dispose();
	}

	private void ShowSetParamDialog()
	{
		frmSetParameter frmSetParameter = new frmSetParameter();
		frmSetParameter.ShowDialog(this);
		_ = 1;
		frmSetParameter.Dispose();
	}

	private void ShowClearHistoryDialog()
	{
		frmClearHistory frmClearHistory = new frmClearHistory();
		frmClearHistory.ShowDialog(this);
		_ = 1;
		frmClearHistory.Dispose();
	}

	private void 伺服器ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ConnectToWebService();
	}

	private void 列印ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ShowPrintListDialog();
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

	private void 參數設定ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ShowSetParamDialog();
	}

	private void 清除歷史資料ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ShowClearHistoryDialog();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmMain));
		SS_Status = new System.Windows.Forms.StatusStrip();
		lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		pbStatus = new System.Windows.Forms.ToolStripProgressBar();
		menuStrip1 = new System.Windows.Forms.MenuStrip();
		伺服器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		列印ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		menuItemVersion = new System.Windows.Forms.ToolStripMenuItem();
		參數設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		lblProducerName = new System.Windows.Forms.Label();
		清除歷史資料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		SS_Status.SuspendLayout();
		menuStrip1.SuspendLayout();
		SuspendLayout();
		SS_Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
		{
			lblStatus,
			pbStatus
		});
		SS_Status.Location = new System.Drawing.Point(0, 430);
		SS_Status.Name = "SS_Status";
		SS_Status.Size = new System.Drawing.Size(747, 22);
		SS_Status.TabIndex = 1;
		SS_Status.Text = "statusStrip1";
		lblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		lblStatus.Name = "lblStatus";
		lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
		lblStatus.Size = new System.Drawing.Size(0, 17);
		lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		pbStatus.Name = "pbStatus";
		pbStatus.Size = new System.Drawing.Size(200, 16);
		menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5]
		{
			伺服器ToolStripMenuItem,
			列印ToolStripMenuItem,
			menuItemVersion,
			參數設定ToolStripMenuItem,
			清除歷史資料ToolStripMenuItem
		});
		menuStrip1.Location = new System.Drawing.Point(0, 0);
		menuStrip1.Name = "menuStrip1";
		menuStrip1.Size = new System.Drawing.Size(747, 24);
		menuStrip1.TabIndex = 2;
		menuStrip1.Text = "menuStrip1";
		伺服器ToolStripMenuItem.Name = "伺服器ToolStripMenuItem";
		伺服器ToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
		伺服器ToolStripMenuItem.Text = "取得可列印標籤";
		伺服器ToolStripMenuItem.Click += new System.EventHandler(伺服器ToolStripMenuItem_Click);
		列印ToolStripMenuItem.Name = "列印ToolStripMenuItem";
		列印ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
		列印ToolStripMenuItem.Text = "列印標籤";
		列印ToolStripMenuItem.Click += new System.EventHandler(列印ToolStripMenuItem_Click);
		menuItemVersion.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		menuItemVersion.Margin = new System.Windows.Forms.Padding(0, 0, 50, 0);
		menuItemVersion.Name = "menuItemVersion";
		menuItemVersion.Size = new System.Drawing.Size(65, 20);
		menuItemVersion.Text = "版本資訊";
		menuItemVersion.Visible = false;
		參數設定ToolStripMenuItem.Name = "參數設定ToolStripMenuItem";
		參數設定ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
		參數設定ToolStripMenuItem.Text = "參數設定";
		參數設定ToolStripMenuItem.Click += new System.EventHandler(參數設定ToolStripMenuItem_Click);
		lblProducerName.BackColor = System.Drawing.Color.Transparent;
		lblProducerName.Font = new System.Drawing.Font("標楷體", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		lblProducerName.Location = new System.Drawing.Point(410, 172);
		lblProducerName.Name = "lblProducerName";
		lblProducerName.Size = new System.Drawing.Size(283, 122);
		lblProducerName.TabIndex = 3;
		lblProducerName.Text = "廠商名…";
		清除歷史資料ToolStripMenuItem.Name = "清除歷史資料ToolStripMenuItem";
		清除歷史資料ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
		清除歷史資料ToolStripMenuItem.Text = "清除歷史資料";
		清除歷史資料ToolStripMenuItem.Click += new System.EventHandler(清除歷史資料ToolStripMenuItem_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackgroundImage = 離線列印Client程式.Properties.Resources.login_bg;
		base.ClientSize = new System.Drawing.Size(747, 452);
		base.Controls.Add(lblProducerName);
		base.Controls.Add(SS_Status);
		base.Controls.Add(menuStrip1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MainMenuStrip = menuStrip1;
		base.MaximizeBox = false;
		base.Name = "frmMain";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "行政院農委會 產銷履歷標籤列印程式";
		base.Load += new System.EventHandler(frmMain_Load);
		base.Shown += new System.EventHandler(frmMain_Shown);
		SS_Status.ResumeLayout(false);
		SS_Status.PerformLayout();
		menuStrip1.ResumeLayout(false);
		menuStrip1.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
