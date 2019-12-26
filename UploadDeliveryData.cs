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

public class UploadDeliveryData
{
	public void uploadDeliverySales(DataTable DeliveryMaster, frmUploadData frmUD)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
		XmlElement xmlElement = xmlDocument.CreateElement("shipData");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("storedId");
		xmlElement2.InnerText = Program.LincenseCode;
		xmlElement.AppendChild(xmlElement2);
		bool flag = false;
		if (DeliveryMaster.Rows.Count > 0)
		{
			for (int i = 0; i < DeliveryMaster.Rows.Count; i++)
			{
				DeliveryMaster.Rows[i]["editDate"].ToString();
				string text = DeliveryMaster.Rows[i]["DeliveryNo"].ToString();
				string text2 = DeliveryMaster.Rows[i]["vendorNo"].ToString();
				string text3 = DeliveryMaster.Rows[i]["status"].ToString();
				string innerText = DateTime.ParseExact(DeliveryMaster.Rows[i]["DeliveryDate"].ToString(), "yyyy-MM-dd", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyyMMddHHmmss");
				XmlElement xmlElement3 = xmlDocument.CreateElement("record");
				XmlElement xmlElement4 = xmlDocument.CreateElement("oddNO");
				xmlElement4.InnerText = text;
				xmlElement3.AppendChild(xmlElement4);
				string innerText2 = "";
				string innerText3 = "";
				string sql = "SELECT vendorId,vendorName FROM hypos_Supplier where SupplierNo = '" + text2 + "'";
				DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
				if (dataTable.Rows.Count > 0)
				{
					innerText2 = dataTable.Rows[0]["vendorId"].ToString();
					innerText3 = dataTable.Rows[0]["vendorName"].ToString();
				}
				XmlElement xmlElement5 = xmlDocument.CreateElement("vendorId");
				xmlElement5.InnerText = innerText2;
				xmlElement3.AppendChild(xmlElement5);
				XmlElement xmlElement6 = xmlDocument.CreateElement("vendorName");
				xmlElement6.InnerText = innerText3;
				xmlElement3.AppendChild(xmlElement6);
				XmlElement xmlElement7 = xmlDocument.CreateElement("oddStatus");
				xmlElement7.InnerText = text3;
				xmlElement3.AppendChild(xmlElement7);
				XmlElement xmlElement8 = xmlDocument.CreateElement("shipDateTime");
				xmlElement8.InnerText = innerText;
				xmlElement3.AppendChild(xmlElement8);
				XmlElement xmlElement9 = xmlDocument.CreateElement("vendorNO");
				xmlElement9.InnerText = text2;
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
						string text4 = (string.IsNullOrEmpty(detailSell.Rows[j]["DeliveryDeatialId"].ToString()) ? "-1" : detailSell.Rows[j]["DeliveryDeatialId"].ToString()).ToString();
						string text5 = detailSell.Rows[j]["barcode"].ToString();
						string value = goodsList.Rows[int.Parse(goodsIndexList[text5].ToString())]["ISWS"].ToString();
						string text6 = goodsList.Rows[int.Parse(goodsIndexList[text5].ToString())]["CLA1NO"].ToString();
						string innerText4 = "";
						string text7 = "";
						bool flag2 = false;
						flag2 = true;
						switch (text6)
						{
						case "0302":
							innerText4 = "1";
							break;
						case "0303":
							innerText4 = "2";
							break;
						case "0305":
							innerText4 = "3";
							break;
						case "0308":
							innerText4 = "4";
							break;
						}
						text7 = ((!"Y".Equals(value)) ? "2" : "1");
						if (!flag2)
						{
							continue;
						}
						string innerText5 = detailSell.Rows[j]["num"].ToString();
						string innerText6 = detailSell.Rows[j]["BatchNo"].ToString();
						string innerText7 = detailSell.Rows[j]["MFGDate"].ToString();
						string text8 = "";
						if (text3.Equals("1") || text3.Equals("2"))
						{
							string sql2 = "SELECT UploadLastUpdateDate FROM hypos_SysParam";
							string text9 = ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql2, null, CommandOperationType.ExecuteReaderReturnDataTable)).Rows[0]["UploadLastUpdateDate"].ToString();
							DataTable dataTable2;
							if ("".Equals(text9))
							{
								string[] strParameterArray = new string[1]
								{
									text
								};
								string sql3 = "SELECT DeliveryLogId FROM hypos_DeliveryGoods_Master_log where DeliveryNo = {0} and (ischange = '1' or iscancel = '1')";
								dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql3, strParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
							}
							else
							{
								string text10 = DateTime.ParseExact(text9, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd HH:mm:ss");
								string[] strParameterArray2 = new string[2]
								{
									text,
									text10
								};
								string sql4 = "SELECT DeliveryLogId FROM hypos_DeliveryGoods_Master_log where DeliveryNo = {0} and (ischange = '1' or iscancel = '1') and changeDate > {1}";
								dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql4, strParameterArray2, CommandOperationType.ExecuteReaderReturnDataTable);
							}
							string text11 = "";
							if (dataTable2.Rows.Count > 0)
							{
								for (int k = 0; k < dataTable2.Rows.Count; k++)
								{
									text11 = text11 + "'" + dataTable2.Rows[k]["DeliveryLogId"].ToString() + "'";
									if (dataTable2.Rows.Count - k > 1)
									{
										text11 += ",";
									}
								}
							}
							string[] strParameterArray3 = new string[2]
							{
								text5,
								text4
							};
							string sql5 = "SELECT sum(diffNum) as returnItemNum FROM hypos_DeliveryGoods_Detail_Log where DeliveryLogId in (" + text11 + ") and barcode = {0} and DeliveryDeatialId = {1}";
							DataTable dataTable3 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql5, strParameterArray3, CommandOperationType.ExecuteReaderReturnDataTable);
							text8 = ((dataTable3.Rows.Count <= 0) ? "" : dataTable3.Rows[0]["returnItemNum"].ToString());
						}
						else
						{
							text8 = "";
						}
						XmlElement xmlElement10 = xmlDocument.CreateElement("item");
						xmlElement3.AppendChild(xmlElement10);
						XmlElement xmlElement11 = xmlDocument.CreateElement("itemNO");
						xmlElement11.InnerText = num.ToString("00");
						xmlElement10.AppendChild(xmlElement11);
						num++;
						XmlElement xmlElement12 = xmlDocument.CreateElement("strBARCODE");
						xmlElement12.InnerText = text5;
						xmlElement10.AppendChild(xmlElement12);
						XmlElement xmlElement13 = xmlDocument.CreateElement("itemType");
						xmlElement13.InnerText = innerText4;
						xmlElement10.AppendChild(xmlElement13);
						XmlElement xmlElement14 = xmlDocument.CreateElement("batchNO");
						xmlElement14.InnerText = innerText6;
						xmlElement10.AppendChild(xmlElement14);
						XmlElement xmlElement15 = xmlDocument.CreateElement("MFD");
						xmlElement15.InnerText = innerText7;
						xmlElement10.AppendChild(xmlElement15);
						XmlElement xmlElement16 = xmlDocument.CreateElement("shipQTY");
						if (text3.Equals("1"))
						{
							xmlElement16.InnerText = Math.Abs(int.Parse(text8)).ToString();
						}
						else
						{
							xmlElement16.InnerText = innerText5;
						}
						xmlElement10.AppendChild(xmlElement16);
						XmlElement xmlElement17 = xmlDocument.CreateElement("salesTYPE");
						xmlElement17.InnerText = "2";
						xmlElement10.AppendChild(xmlElement17);
						XmlElement xmlElement18 = xmlDocument.CreateElement("differNUM");
						xmlElement18.InnerText = text8;
						xmlElement10.AppendChild(xmlElement18);
						XmlElement xmlElement19 = xmlDocument.CreateElement("dataType");
						xmlElement19.InnerText = text7;
						xmlElement10.AppendChild(xmlElement19);
						string sql6 = "SELECT GDName FROM hypos_GOODSLST where GDSNO='" + text5 + "'";
						DataTable dataTable4 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql6, null, CommandOperationType.ExecuteReaderReturnDataTable);
						XmlElement xmlElement20 = xmlDocument.CreateElement("dataName");
						if (dataTable4.Rows.Count > 0)
						{
							xmlElement20.InnerText = dataTable4.Rows[0][0].ToString();
						}
						else
						{
							xmlElement20.InnerText = "";
						}
						xmlElement10.AppendChild(xmlElement20);
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
			uploadData.uploadShipData(Program.Encrypt(xmlDocument.OuterXml));
			if (!Directory.Exists("uploadXmlData"))
			{
				Directory.CreateDirectory("uploadXmlData");
			}
			FileStream fileStream = null;
			try
			{
				string text12 = "uploadXmlData\\ShipData\\";
				string str = DateTime.Now.ToString("yyyyMMdd") + "\\";
				string str2 = "ShipData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
				frmUD._shipDataXMLPath = text12 + str + str2 + ".xml";
				if (!Directory.Exists(text12))
				{
					Directory.CreateDirectory(text12);
				}
				if (!Directory.Exists(text12 + str))
				{
					Directory.CreateDirectory(text12 + str);
				}
				fileStream = new FileStream(frmUD._shipDataXMLPath, FileMode.Create);
				new UTF8Encoding();
				byte[] bytes = Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
				fileStream.Write(bytes, 0, bytes.Length);
				frmUD._DeliverysalesUploadStatus = 2;
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

	private DataTable getDetailSell(string DeliveryNo)
	{
		string sql = "SELECT DeliveryDeatialId, barcode, BatchNo, MFGDate, num FROM hypos_DeliveryGoods_Detail where DeliveryNo ='" + DeliveryNo + "'";
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
		string strSelectField = "CLA1NO, GDSNO, ISWS";
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
