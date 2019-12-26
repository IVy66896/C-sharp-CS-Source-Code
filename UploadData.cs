using POS_Client;
using POS_Client.POS_WS_Upload;
using POS_Client.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using T00SharedLibraryDotNet20;

public class UploadData
{
	public void uploadSales(DataTable mainSell, frmUploadData frmUD)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
		XmlElement xmlElement = xmlDocument.CreateElement("salesData");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("storedId");
		xmlElement2.InnerText = Program.LincenseCode;
		xmlElement.AppendChild(xmlElement2);
		bool flag = false;
		if (mainSell.Rows.Count > 0)
		{
			for (int i = 0; i < mainSell.Rows.Count; i++)
			{
				mainSell.Rows[i]["editDate"].ToString();
				string text = mainSell.Rows[i]["sellNo"].ToString();
				string innerText = mainSell.Rows[i]["memberId"].ToString();
				string text2 = mainSell.Rows[i]["status"].ToString();
				string innerText2 = DateTime.ParseExact(mainSell.Rows[i]["sellTime"].ToString(), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyyMMddHHmmss");
				string innerText3 = mainSell.Rows[i]["Name"].ToString();
				string innerText4 = mainSell.Rows[i]["IdNo"].ToString();
				XmlElement xmlElement3 = xmlDocument.CreateElement("record");
				XmlElement xmlElement4 = xmlDocument.CreateElement("oddNO");
				xmlElement4.InnerText = text;
				xmlElement3.AppendChild(xmlElement4);
				XmlElement xmlElement5 = xmlDocument.CreateElement("cusCardNO");
				xmlElement5.InnerText = innerText;
				xmlElement3.AppendChild(xmlElement5);
				XmlElement xmlElement6 = xmlDocument.CreateElement("memberName");
				xmlElement6.InnerText = innerText3;
				xmlElement3.AppendChild(xmlElement6);
				XmlElement xmlElement7 = xmlDocument.CreateElement("cusIDNO");
				xmlElement7.InnerText = innerText4;
				xmlElement3.AppendChild(xmlElement7);
				XmlElement xmlElement8 = xmlDocument.CreateElement("oddStatus");
				xmlElement8.InnerText = text2;
				xmlElement3.AppendChild(xmlElement8);
				XmlElement xmlElement9 = xmlDocument.CreateElement("purchaseDateTime");
				xmlElement9.InnerText = innerText2;
				xmlElement3.AppendChild(xmlElement9);
				DataTable detailSell = getDetailSell(text);
				List<string> barCodeList = getBarCodeList(detailSell);
				DataTable goodsList = getGoodsList(barCodeList);
				Dictionary<string, int> goodsIndexList = getGoodsIndexList(goodsList);
				if (detailSell.Rows.Count > 0)
				{
					int num = 1;
					for (int j = 0; j < detailSell.Rows.Count; j++)
					{
						string text3 = (string.IsNullOrEmpty(detailSell.Rows[j]["sellDeatialId"].ToString()) ? "-1" : detailSell.Rows[j]["sellDeatialId"].ToString()).ToString();
						string text4 = detailSell.Rows[j]["barcode"].ToString();
						string value = goodsList.Rows[int.Parse(goodsIndexList[text4].ToString())]["ISWS"].ToString();
						string text5 = goodsList.Rows[int.Parse(goodsIndexList[text4].ToString())]["CLA1NO"].ToString();
						goodsList.Rows[int.Parse(goodsIndexList[text4].ToString())]["SubsidyFertilizer"].ToString();
						string innerText5 = "";
						string text6 = "";
						bool flag2 = false;
						flag2 = true;
						switch (text5)
						{
						case "0302":
							innerText5 = "1";
							break;
						case "0303":
							innerText5 = "2";
							break;
						case "0305":
							innerText5 = "3";
							break;
						case "0308":
							innerText5 = "4";
							break;
						}
						text6 = ((!"Y".Equals(value)) ? "2" : "1");
						if (!flag2)
						{
							continue;
						}
						string innerText6 = detailSell.Rows[j]["PRNO"].ToString();
						string innerText7 = detailSell.Rows[j]["BLNO"].ToString();
						string innerText8 = detailSell.Rows[j]["num"].ToString();
						XmlElement xmlElement10 = xmlDocument.CreateElement("item");
						xmlElement3.AppendChild(xmlElement10);
						XmlElement xmlElement11 = xmlDocument.CreateElement("itemNO");
						xmlElement11.InnerText = num.ToString("00");
						xmlElement10.AppendChild(xmlElement11);
						num++;
						XmlElement xmlElement12 = xmlDocument.CreateElement("strBARCODE");
						xmlElement12.InnerText = text4;
						xmlElement10.AppendChild(xmlElement12);
						XmlElement xmlElement13 = xmlDocument.CreateElement("itemType");
						xmlElement13.InnerText = innerText5;
						xmlElement10.AppendChild(xmlElement13);
						XmlElement xmlElement14 = xmlDocument.CreateElement("PESTICIDEID");
						xmlElement14.InnerText = goodsList.Rows[int.Parse(goodsIndexList[text4].ToString())]["pesticideId"].ToString();
						xmlElement10.AppendChild(xmlElement14);
						XmlElement xmlElement15 = xmlDocument.CreateElement("CROPID");
						xmlElement15.InnerText = innerText6;
						xmlElement10.AppendChild(xmlElement15);
						XmlElement xmlElement16 = xmlDocument.CreateElement("PESTID");
						xmlElement16.InnerText = innerText7;
						xmlElement10.AppendChild(xmlElement16);
						XmlElement xmlElement17 = xmlDocument.CreateElement("purchaseQTY");
						xmlElement17.InnerText = innerText8;
						xmlElement10.AppendChild(xmlElement17);
						XmlElement xmlElement18 = xmlDocument.CreateElement("SALESTYPE");
						xmlElement18.InnerText = "1";
						xmlElement10.AppendChild(xmlElement18);
						if (text2.Equals("1") || text2.Equals("2"))
						{
							string sql = "SELECT UploadLastUpdateDate FROM hypos_SysParam";
							string text7 = ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable)).Rows[0]["UploadLastUpdateDate"].ToString();
							DataTable dataTable;
							if ("".Equals(text7))
							{
								string[] strParameterArray = new string[1]
								{
									text
								};
								string sql2 = "SELECT sellLogId FROM hypos_mainsell_log where sellNo = {0} and (ischange = '1' or iscancel = '1')";
								dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, strParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
							}
							else
							{
								string text8 = DateTime.ParseExact(text7, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
								string[] strParameterArray2 = new string[2]
								{
									text,
									text8
								};
								string sql3 = "SELECT sellLogId FROM hypos_mainsell_log where sellNo = {0} and (ischange = '1' or iscancel = '1') and changeDate > {1}";
								dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql3, strParameterArray2, CommandOperationType.ExecuteReaderReturnDataTable);
							}
							string text9 = "";
							if (dataTable.Rows.Count > 0)
							{
								for (int k = 0; k < dataTable.Rows.Count; k++)
								{
									text9 = text9 + "'" + dataTable.Rows[k]["sellLogId"].ToString() + "'";
									if (dataTable.Rows.Count - k > 1)
									{
										text9 += ",";
									}
								}
							}
							string[] strParameterArray3 = new string[2]
							{
								text4,
								text3
							};
							string sql4 = "SELECT sum(diffNum) as returnItemNum FROM hypos_detailsell_log where sellLogId in (" + text9 + ") and barcode = {0} and sellDetailId = {1}";
							DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql4, strParameterArray3, CommandOperationType.ExecuteReaderReturnDataTable);
							if (dataTable2.Rows.Count > 0)
							{
								XmlElement xmlElement19 = xmlDocument.CreateElement("DIFFERNUM");
								if (dataTable2.Rows[0]["returnItemNum"].ToString().Equals("0") || string.IsNullOrEmpty(dataTable2.Rows[0]["returnItemNum"].ToString()))
								{
									xmlElement19.InnerText = "0";
								}
								else
								{
									xmlElement19.InnerText = "-" + dataTable2.Rows[0]["returnItemNum"].ToString();
								}
								xmlElement10.AppendChild(xmlElement19);
							}
							else
							{
								XmlElement xmlElement20 = xmlDocument.CreateElement("DIFFERNUM");
								xmlElement20.InnerText = "";
								xmlElement10.AppendChild(xmlElement20);
							}
						}
						else
						{
							XmlElement xmlElement21 = xmlDocument.CreateElement("DIFFERNUM");
							xmlElement21.InnerText = "";
							xmlElement10.AppendChild(xmlElement21);
						}
						XmlElement xmlElement22 = xmlDocument.CreateElement("dataType");
						xmlElement22.InnerText = text6;
						xmlElement10.AppendChild(xmlElement22);
						string sql5 = "SELECT GDName FROM hypos_GOODSLST where GDSNO='" + text4 + "'";
						DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql5, null, CommandOperationType.ExecuteReaderReturnDataTable);
						XmlElement xmlElement23 = xmlDocument.CreateElement("dataName");
						if (dataTable3.Rows.Count > 0)
						{
							xmlElement23.InnerText = dataTable3.Rows[0][0].ToString();
						}
						else
						{
							xmlElement23.InnerText = "";
						}
						xmlElement10.AppendChild(xmlElement23);
					}
				}
				xmlElement.AppendChild(xmlElement3);
			}
			flag = true;
		}
		if (flag)
		{
			uploadData uploadData = new uploadData();
			uploadData.Url = Program.UploadDataURL;
			uploadData.uploadSalesData(Program.Encrypt(xmlDocument.OuterXml));
			if (!Directory.Exists("uploadXmlData"))
			{
				Directory.CreateDirectory("uploadXmlData");
			}
			FileStream fileStream = null;
			try
			{
				string text10 = "uploadXmlData\\SalesData\\";
				string str = DateTime.Now.ToString("yyyyMMdd") + "\\";
				string str2 = "SalesData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				frmUD._salesDataXMLPath = text10 + str + str2 + ".xml";
				if (!Directory.Exists(text10))
				{
					Directory.CreateDirectory(text10);
				}
				if (!Directory.Exists(text10 + str))
				{
					Directory.CreateDirectory(text10 + str);
				}
				fileStream = new FileStream(frmUD._salesDataXMLPath, FileMode.Create);
				new UTF8Encoding();
				byte[] bytes = Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
				fileStream.Write(bytes, 0, bytes.Length);
				frmUD._salesUploadStatus = 2;
			}
			catch (Exception ex)
			{
				Console.WriteLine("儲存上傳XML資料錯誤 ::: " + ex.Message);
			}
			finally
			{
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}

	public void uploadCount(DataTable inventoryAdjustment, frmUploadData frmUD)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
		XmlElement xmlElement = xmlDocument.CreateElement("countData");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("storedId");
		xmlElement2.InnerText = Program.LincenseCode;
		xmlElement.AppendChild(xmlElement2);
		bool flag = false;
		if (inventoryAdjustment.Rows.Count > 0)
		{
			for (int i = 0; i < inventoryAdjustment.Rows.Count; i++)
			{
				string innerText = inventoryAdjustment.Rows[i]["AdjustNo"].ToString();
				string innerText2 = DateTime.ParseExact(inventoryAdjustment.Rows[i]["updateDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyyMMddHHmmss");
				string innerText3 = inventoryAdjustment.Rows[i]["GDSNO"].ToString();
				string innerText4 = inventoryAdjustment.Rows[i]["adjustCount"].ToString();
				string innerText5 = inventoryAdjustment.Rows[i]["adjustType"].ToString();
				string innerText6 = inventoryAdjustment.Rows[i]["batchNO"].ToString();
				string innerText7 = inventoryAdjustment.Rows[i]["MFD"].ToString();
				string innerText8 = inventoryAdjustment.Rows[i]["vendorId"].ToString();
				string innerText9 = inventoryAdjustment.Rows[i]["vendorName"].ToString();
				XmlElement xmlElement3 = xmlDocument.CreateElement("record");
				XmlElement xmlElement4 = xmlDocument.CreateElement("recordID");
				xmlElement4.InnerText = innerText;
				xmlElement3.AppendChild(xmlElement4);
				XmlElement xmlElement5 = xmlDocument.CreateElement("countDateTime");
				xmlElement5.InnerText = innerText2;
				xmlElement3.AppendChild(xmlElement5);
				XmlElement xmlElement6 = xmlDocument.CreateElement("strBARCODE");
				xmlElement6.InnerText = innerText3;
				xmlElement3.AppendChild(xmlElement6);
				XmlElement xmlElement7 = xmlDocument.CreateElement("countQTY");
				xmlElement7.InnerText = innerText4;
				xmlElement3.AppendChild(xmlElement7);
				XmlElement xmlElement8 = xmlDocument.CreateElement("countReason");
				xmlElement8.InnerText = innerText5;
				xmlElement3.AppendChild(xmlElement8);
				XmlElement xmlElement9 = xmlDocument.CreateElement("batchNO");
				xmlElement9.InnerText = innerText6;
				xmlElement3.AppendChild(xmlElement9);
				XmlElement xmlElement10 = xmlDocument.CreateElement("MFD");
				xmlElement10.InnerText = innerText7;
				xmlElement3.AppendChild(xmlElement10);
				XmlElement xmlElement11 = xmlDocument.CreateElement("vendorId");
				xmlElement11.InnerText = innerText8;
				xmlElement3.AppendChild(xmlElement11);
				XmlElement xmlElement12 = xmlDocument.CreateElement("vendorName");
				xmlElement12.InnerText = innerText9;
				xmlElement3.AppendChild(xmlElement12);
				xmlElement.AppendChild(xmlElement3);
			}
			flag = true;
		}
		if (flag)
		{
			uploadData uploadData = new uploadData();
			uploadData.Url = Program.UploadDataURL;
			uploadData.uploadCountData(Program.Encrypt(xmlDocument.OuterXml));
			if (!Directory.Exists("uploadXmlData"))
			{
				Directory.CreateDirectory("uploadXmlData");
			}
			FileStream fileStream = null;
			try
			{
				string text = "uploadXmlData\\CountData\\";
				string str = DateTime.Now.ToString("yyyyMMdd") + "\\";
				string str2 = "CountData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				frmUD._inventoryDataXMLPath = text + str + str2 + ".xml";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (!Directory.Exists(text + str))
				{
					Directory.CreateDirectory(text + str);
				}
				fileStream = new FileStream(frmUD._inventoryDataXMLPath, FileMode.Create);
				new UTF8Encoding();
				byte[] bytes = Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
				fileStream.Write(bytes, 0, bytes.Length);
				frmUD._inventoryUploadStatus = 2;
			}
			catch (Exception ex)
			{
				Console.WriteLine("儲存上傳XML資料錯誤 ::: " + ex.Message);
			}
			finally
			{
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}

	private DataTable getDetailSell(string sellNo)
	{
		string sql = "SELECT sellDeatialId, barcode, PRNO, BLNO, num FROM hypos_detail_sell where sellNo='" + sellNo + "'";
		return (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
	}

	private List<string> getBarCodeList(DataTable detailSell)
	{
		List<string> list = new List<string>();
		if (detailSell.Rows.Count > 0)
		{
			for (int i = 0; i < detailSell.Rows.Count; i++)
			{
				list.Add(detailSell.Rows[i]["barcode"].ToString());
			}
		}
		return list;
	}

	private DataTable getGoodsList(List<string> barCodeList)
	{
		string strSelectField = "CLA1NO, GDSNO, pesticideId, ISWS, SubsidyFertilizer";
		string text = "GDSNO in (";
		for (int i = 0; i < barCodeList.Count; i++)
		{
			text = text + "{" + i + "},";
		}
		text = text.Substring(0, text.Length - 1) + ")";
		return (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, strSelectField, "hypos_GOODSLST", text, "", null, barCodeList.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
	}

	private Dictionary<string, int> getGoodsIndexList(DataTable goodsList)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		if (goodsList.Rows.Count > 0)
		{
			for (int i = 0; i < goodsList.Rows.Count; i++)
			{
				dictionary.Add(goodsList.Rows[i]["GDSNO"].ToString(), i);
			}
		}
		return dictionary;
	}
}
