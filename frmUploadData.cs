using POS_Client;
using POS_Client.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using T00SharedLibraryDotNet20;

public class frmUploadData : Form
{
	public string _salesDataXMLPath = "";

	public string _inventoryDataXMLPath = "";

	public string _shipDataXMLPath = "";

	public int _salesUploadStatus;

	public int _inventoryUploadStatus;

	public int _DeliverysalesUploadStatus;

	private IContainer components;

	private Label label1;

	private ProgressBar progressBar1;

	private BackgroundWorker backgroundWorker1;

	private TextBox tb_status;

	public frmUploadData()
	{
		InitializeComponent();
		backgroundWorker1.RunWorkerAsync();
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker1.ReportProgress(1);
		string updateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		string sql = "SELECT UploadLastUpdateDate, UploadCountLastUpdateDate, UploadShipLastUpdateDate FROM hypos_SysParam";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		string text = dataTable.Rows[0]["UploadLastUpdateDate"].ToString();
		string lastUploadDate = dataTable.Rows[0]["UploadShipLastUpdateDate"].ToString();
		string lastUploadDate2 = string.IsNullOrEmpty(dataTable.Rows[0]["UploadCountLastUpdateDate"].ToString()) ? text : dataTable.Rows[0]["UploadCountLastUpdateDate"].ToString();
		UploadData uploadData = new UploadData();
		try
		{
			string sql2 = "SELECT a.sellNo,a.sellTime FROM hypos_main_sell as a left join hypos_detail_sell as b on a.sellNo = b.sellNo where b.sellNo is null";
			DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable2.Rows)
				{
					string[] strParameterArray = new string[2]
					{
						row["sellNo"].ToString(),
						row["sellTime"].ToString()
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, "DELETE FROM hypos_main_sell where sellNo = {0} and sellTime = {1} ", strParameterArray, CommandOperationType.ExecuteNonQuery);
				}
			}
			DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, getMainSql(text), null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable3.Rows.Count > 0)
			{
				uploadData.uploadSales(dataTable3, this);
				backgroundWorker1.ReportProgress(40);
			}
		}
		catch (Exception ex)
		{
			_salesUploadStatus = 1;
			MessageBox.Show("銷售紀錄上傳失敗: " + ex.Message);
		}
		statusAppendText("sellLog");
		UploadDataLogSave("sellLog", updateDate);
		UploadDeliveryData uploadDeliveryData = new UploadDeliveryData();
		try
		{
			DataTable dataTable4 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, getDeliveryMainSql(lastUploadDate), null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable4.Rows.Count > 0)
			{
				uploadDeliveryData.uploadDeliverySales(dataTable4, this);
				backgroundWorker1.ReportProgress(70);
			}
		}
		catch (Exception ex2)
		{
			_DeliverysalesUploadStatus = 1;
			MessageBox.Show("出貨紀錄上傳失敗: " + ex2.Message);
		}
		statusAppendText("deliveryLog");
		UploadDataLogSave("deliveryLog", updateDate);
		try
		{
			DataTable dataTable5 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, getInventoryAdjustment(lastUploadDate2), null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable5.Rows.Count > 0)
			{
				uploadData.uploadCount(dataTable5, this);
				backgroundWorker1.ReportProgress(90);
			}
		}
		catch (Exception ex3)
		{
			_inventoryUploadStatus = 1;
			MessageBox.Show("庫存調整紀錄上傳失敗: " + ex3.Message);
		}
		statusAppendText("inventoryLog");
		UploadDataLogSave("inventoryLog", updateDate);
		if (_salesUploadStatus != 1)
		{
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SysParam SET UploadLastUpdateDate ={0} ", new string[1]
			{
				DateTime.Now.ToString("yyyyMMddHHmmss")
			}, CommandOperationType.ExecuteNonQuery);
		}
		if (_inventoryUploadStatus != 1)
		{
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SysParam SET UploadCountLastUpdateDate ={0} ", new string[1]
			{
				DateTime.Now.ToString("yyyyMMddHHmmss")
			}, CommandOperationType.ExecuteNonQuery);
		}
		if (_DeliverysalesUploadStatus != 1)
		{
			DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SysParam SET UploadShipLastUpdateDate ={0} ", new string[1]
			{
				DateTime.Now.ToString("yyyyMMddHHmmss")
			}, CommandOperationType.ExecuteNonQuery);
		}
		backgroundWorker1.ReportProgress(100);
	}

	private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		progressBar1.Value = e.ProgressPercentage;
	}

	private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (progressBar1.Value == 1)
		{
			MessageBox.Show("無資料上傳");
			Close();
		}
		else if (progressBar1.Value == 100)
		{
			MessageBox.Show("上傳完成");
			Close();
		}
		else
		{
			MessageBox.Show("點選確認結束");
			Close();
		}
	}

	private string getMainSql(string lastUploadDate)
	{
		string str = "SELECT hms.sellNo, hms.memberId, hms.status, hms.sellTime, hms.editDate, hcr.Name, hcr.IdNo FROM hypos_main_sell as hms left outer join hypos_CUST_RTL as hcr on hms.memberId =hcr.VipNo WHERE 1=1 and hms.oldECRHDHSNo is null ";
		if (!string.IsNullOrEmpty(lastUploadDate))
		{
			string str2 = DateTime.ParseExact(lastUploadDate, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
			str = str + "and hms.editDate >='" + str2 + "' ";
		}
		return str + "ORDER BY hms.sellNo asc ";
	}

	private string getDeliveryMainSql(string lastUploadDate)
	{
		string str = "SELECT DeliveryNo, vendorNo, status, DeliveryDate, editDate FROM hypos_DeliveryGoods_Master WHERE 1=1 ";
		if (!string.IsNullOrEmpty(lastUploadDate))
		{
			string str2 = DateTime.ParseExact(lastUploadDate, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
			str = str + "and editDate >='" + str2 + "' ";
		}
		return str + "ORDER BY DeliveryNo asc ";
	}

	private string getInventoryAdjustment(string lastUploadDate)
	{
		string str = "SELECT ia.*, gl.CLA1NO, gl.ISWS FROM hypos_InventoryAdjustment as ia LEFT join hypos_GOODSLST as gl on ia.GDSNO=gl.GDSNO where 1=1 ";
		if (!string.IsNullOrEmpty(lastUploadDate))
		{
			string str2 = DateTime.ParseExact(lastUploadDate, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
			str = str + "and ia.updateDate >='" + str2 + "' ";
		}
		return str + " and gl.CLA1NO = '0302' and gl.ISWS ='Y' and  ia.adjustType <> '0' ORDER BY ia.AdjustNo asc ";
	}

	public void UploadDataLogSave(string syncName, string updateDate)
	{
		if ("sellLog".Equals(syncName))
		{
			if (_salesUploadStatus == 2)
			{
				string text = SynchronizeMain(updateDate, 0.ToString(), "0");
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(_salesDataXMLPath);
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//record");
					for (int i = 0; i < xmlNodeList.Count; i++)
					{
						string innerText = xmlNodeList[i].SelectSingleNode("oddNO").InnerText;
						string innerText2 = xmlNodeList[i].SelectSingleNode("oddStatus").InnerText;
						string text2 = DateTime.ParseExact(xmlNodeList[i].SelectSingleNode("purchaseDateTime").InnerText, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
						string innerText3 = xmlNodeList[i].SelectSingleNode("cusCardNO").InnerText;
						foreach (XmlNode item in xmlNodeList[i].SelectNodes("item"))
						{
							string text3 = "";
							switch (item.SelectSingleNode("itemType").InnerText)
							{
							case "1":
								text3 = "0302";
								break;
							case "2":
								text3 = "0303";
								break;
							case "3":
								text3 = "0305";
								break;
							case "4":
								text3 = "0308";
								break;
							default:
								text3 = "N/A";
								break;
							}
							string[,] strFieldArray = new string[11, 2]
							{
								{
									"mainLogId",
									text.ToString()
								},
								{
									"sellNo",
									innerText
								},
								{
									"status",
									innerText2
								},
								{
									"BuyDate",
									text2
								},
								{
									"VipNo",
									innerText3
								},
								{
									"num",
									int.Parse(item.SelectSingleNode("itemNO").InnerText).ToString()
								},
								{
									"barcode",
									item.SelectSingleNode("strBARCODE").InnerText
								},
								{
									"PRNO",
									item.SelectSingleNode("CROPID").InnerText
								},
								{
									"BLNO",
									item.SelectSingleNode("PESTID").InnerText
								},
								{
									"CLA1NO",
									text3
								},
								{
									"count",
									item.SelectSingleNode("purchaseQTY").InnerText
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Sync_sell_detail_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
						}
					}
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步紀錄銷售詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
					});
				}
				catch (Exception ex)
				{
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步紀錄銷售詳細資訊儲存失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
					});
					MessageBox.Show("同步紀錄銷售詳細資訊儲存失敗: " + ex.Message);
				}
			}
			else if (_salesUploadStatus == 1)
			{
				SynchronizeMain(updateDate, 0.ToString(), "1");
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無同步紀錄銷售詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
		}
		if ("inventoryLog".Equals(syncName))
		{
			if (_inventoryUploadStatus == 2)
			{
				string text4 = SynchronizeMain(updateDate, 1.ToString(), "0");
				try
				{
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.Load(_inventoryDataXMLPath);
					XmlNodeList xmlNodeList2 = xmlDocument2.SelectNodes("//record");
					if (xmlNodeList2.Count > 0)
					{
						List<string> list = new List<string>();
						for (int j = 0; j < xmlNodeList2.Count; j++)
						{
							list.Add(xmlNodeList2[j].SelectSingleNode("recordID").InnerText);
						}
						string text5 = "AdjustNo in (";
						for (int k = 0; k < list.Count; k++)
						{
							text5 = text5 + "{" + k + "},";
						}
						text5 = text5.Substring(0, text5.Length - 1) + ")";
						text5 += " and adjustType <> '0' ";
						DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "AdjustNo,GoodsTotalCountLog", "hypos_InventoryAdjustment", text5, "", null, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						for (int l = 0; l < dataTable.Rows.Count; l++)
						{
							dictionary.Add(dataTable.Rows[l]["AdjustNo"].ToString(), dataTable.Rows[l]["GoodsTotalCountLog"].ToString());
						}
						for (int m = 0; m < xmlNodeList2.Count; m++)
						{
							string innerText4 = xmlNodeList2[m].SelectSingleNode("recordID").InnerText;
							string[,] strFieldArray = new string[7, 2]
							{
								{
									"mainLogId",
									text4.ToString()
								},
								{
									"AdjustNo",
									innerText4
								},
								{
									"GDSNO",
									xmlNodeList2[m].SelectSingleNode("strBARCODE").InnerText
								},
								{
									"adjustType",
									xmlNodeList2[m].SelectSingleNode("countReason").InnerText
								},
								{
									"adjustCount",
									xmlNodeList2[m].SelectSingleNode("countQTY").InnerText
								},
								{
									"GoodsTotalCountLog",
									dictionary[innerText4]
								},
								{
									"updateDate",
									updateDate
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Sync_inventory_detail_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
						}
					}
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步記錄庫存調整詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
					});
				}
				catch (Exception ex2)
				{
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步記錄庫存調整詳細資訊儲存失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
					});
					MessageBox.Show("同步記錄庫存調整詳細資訊儲存失敗: " + ex2.Message);
				}
			}
			else if (_inventoryUploadStatus == 1)
			{
				SynchronizeMain(updateDate, 1.ToString(), "1");
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無同步記錄庫存調整詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
		}
		if ("deliveryLog".Equals(syncName))
		{
			if (_DeliverysalesUploadStatus == 2)
			{
				string text6 = SynchronizeMain(updateDate, 9.ToString(), "0");
				try
				{
					XmlDocument xmlDocument3 = new XmlDocument();
					xmlDocument3.Load(_shipDataXMLPath);
					XmlNodeList xmlNodeList3 = xmlDocument3.SelectNodes("//record");
					for (int n = 0; n < xmlNodeList3.Count; n++)
					{
						string innerText5 = xmlNodeList3[n].SelectSingleNode("oddNO").InnerText;
						string innerText6 = xmlNodeList3[n].SelectSingleNode("vendorId").InnerText;
						string innerText7 = xmlNodeList3[n].SelectSingleNode("vendorName").InnerText;
						string innerText8 = xmlNodeList3[n].SelectSingleNode("oddStatus").InnerText;
						string text7 = DateTime.ParseExact(xmlNodeList3[n].SelectSingleNode("shipDateTime").InnerText, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
						string innerText9 = xmlNodeList3[n].SelectSingleNode("vendorNO").InnerText;
						foreach (XmlNode item2 in xmlNodeList3[n].SelectNodes("item"))
						{
							string innerText10 = item2.SelectSingleNode("strBARCODE").InnerText;
							string text8 = "";
							string innerText11 = item2.SelectSingleNode("batchNO").InnerText;
							string innerText12 = item2.SelectSingleNode("MFD").InnerText;
							string innerText13 = item2.SelectSingleNode("shipQTY").InnerText;
							string innerText14 = item2.SelectSingleNode("salesTYPE").InnerText;
							string innerText15 = item2.SelectSingleNode("differNUM").InnerText;
							string text9 = "";
							string innerText16 = item2.SelectSingleNode("dataName").InnerText;
							switch (item2.SelectSingleNode("itemType").InnerText)
							{
							case "1":
								text8 = "0302";
								break;
							case "2":
								text8 = "0303";
								break;
							case "3":
								text8 = "0305";
								break;
							case "4":
								text8 = "0308";
								break;
							default:
								text8 = "N/A";
								break;
							}
							string innerText17 = item2.SelectSingleNode("dataType").InnerText;
							text9 = ((!(innerText17 == "1")) ? "N" : "Y");
							string[,] strFieldArray = new string[18, 2]
							{
								{
									"mainLogId",
									text6.ToString()
								},
								{
									"DeliveryNo",
									innerText5
								},
								{
									"status",
									innerText8
								},
								{
									"shipDateTime",
									text7
								},
								{
									"vendorId",
									innerText6
								},
								{
									"vendorName",
									innerText7
								},
								{
									"vendorNO",
									innerText9
								},
								{
									"itemNO",
									int.Parse(item2.SelectSingleNode("itemNO").InnerText).ToString()
								},
								{
									"barcode",
									innerText10
								},
								{
									"itemType",
									text8
								},
								{
									"batchNO",
									innerText11
								},
								{
									"itemType",
									text8
								},
								{
									"MFD",
									innerText12
								},
								{
									"shipQTY",
									innerText13
								},
								{
									"salesTYPE",
									innerText14
								},
								{
									"differNUM",
									innerText15
								},
								{
									"dataType",
									text9
								},
								{
									"dataName",
									innerText16
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Sync_ship_detail_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
						}
					}
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步紀錄出貨單詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
					});
				}
				catch (Exception ex3)
				{
					tb_status.Invoke((MethodInvoker)delegate
					{
						tb_status.AppendText("同步紀錄出貨單詳細資訊儲存失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
					});
					MessageBox.Show("同步紀錄出貨單詳細資訊儲存失敗: " + ex3.Message);
				}
			}
			else if (_DeliverysalesUploadStatus == 1)
			{
				SynchronizeMain(updateDate, 9.ToString(), "1");
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無同步紀錄銷售詳細資訊儲存:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
		}
	}

	private string SynchronizeMain(string updateDate, string updateType, string status)
	{
		string[,] strFieldArray = new string[3, 2]
		{
			{
				"status",
				status
			},
			{
				"updateType",
				updateType
			},
			{
				"updateDate",
				updateDate
			}
		};
		DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "hypos_Synchronize_main_log", null, null, strFieldArray, null, CommandOperationType.ExecuteNonQuery);
		string sql = "select mainLogId from hypos_Synchronize_main_log order by mainLogId desc LIMIT 1";
		return DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteScalar).ToString();
	}

	private void statusAppendText(string status)
	{
		if ("sellLog".Equals(status))
		{
			if (_salesUploadStatus == 2)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("銷售紀錄上傳:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
				});
			}
			else if (_salesUploadStatus == 1)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("銷售紀錄上傳失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無銷售紀錄上傳資料 \r\n");
				});
			}
		}
		if ("inventoryLog".Equals(status))
		{
			if (_inventoryUploadStatus == 2)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("庫存調整紀錄上傳:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
				});
			}
			else if (_inventoryUploadStatus == 1)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("庫存調整紀錄上傳失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無庫存調整紀錄上傳資料 \r\n");
				});
			}
		}
		if ("deliveryLog".Equals(status))
		{
			if (_DeliverysalesUploadStatus == 2)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("出貨紀錄上傳:" + DateTime.Now.ToLongTimeString() + " OK!\r\n");
				});
			}
			else if (_DeliverysalesUploadStatus == 1)
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("出貨紀錄上傳失敗:" + DateTime.Now.ToLongTimeString() + " \r\n");
				});
			}
			else
			{
				tb_status.Invoke((MethodInvoker)delegate
				{
					tb_status.AppendText("無出貨紀錄上傳資料 \r\n");
				});
			}
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
		label1 = new System.Windows.Forms.Label();
		progressBar1 = new System.Windows.Forms.ProgressBar();
		backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		tb_status = new System.Windows.Forms.TextBox();
		SuspendLayout();
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.Red;
		label1.Location = new System.Drawing.Point(12, 9);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(266, 42);
		label1.TabIndex = 0;
		label1.Text = "正在上傳資料中，請停留在此頁面、\r\n不要關閉程式或電腦！";
		progressBar1.Location = new System.Drawing.Point(12, 67);
		progressBar1.Name = "progressBar1";
		progressBar1.Size = new System.Drawing.Size(386, 35);
		progressBar1.Step = 1;
		progressBar1.TabIndex = 1;
		backgroundWorker1.WorkerReportsProgress = true;
		backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		tb_status.Location = new System.Drawing.Point(16, 122);
		tb_status.Multiline = true;
		tb_status.Name = "tb_status";
		tb_status.ReadOnly = true;
		tb_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		tb_status.Size = new System.Drawing.Size(382, 197);
		tb_status.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(406, 366);
		base.ControlBox = false;
		base.Controls.Add(tb_status);
		base.Controls.Add(progressBar1);
		base.Controls.Add(label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmUploadData";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "上傳資料中，請稍候";
		ResumeLayout(false);
		PerformLayout();
	}
}
