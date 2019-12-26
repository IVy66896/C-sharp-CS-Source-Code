using POS_Client;
using POS_Client.POS_WS_POS;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Xml;
using T00SharedLibraryDotNet20;

public class UploadVerification
{
	public string retailData()
	{
		Program.Logger.Info("[驗證店家購肥帳密] -- 開始");
		string result = "";
		if (NetworkInterface.GetIsNetworkAvailable())
		{
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "FertilizerAccount,FertilizerPassword,DealerNo", "hypos_ShopInfoManage", "", "ShopIdNo limit 1", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable.Rows.Count > 0)
			{
				try
				{
					Program.Logger.Info("[驗證店家購肥帳密] -- 資料上傳至WS 開始");
					POSService pOSService = new POSService();
					pOSService.Url = Program.VerificationURL;
					Program.Logger.Info("[驗證店家購肥帳密] -- 上傳資料\nL:" + Program.LincenseCode + "\nFA:" + dataTable.Rows[0]["FertilizerAccount"].ToString() + "\nFP:" + dataTable.Rows[0]["FertilizerPassword"].ToString() + "\nD:" + dataTable.Rows[0]["DealerNo"].ToString() + "\n");
					string xml = pOSService.sendRetailData(Program.LincenseCode, dataTable.Rows[0]["FertilizerAccount"].ToString(), dataTable.Rows[0]["FertilizerPassword"].ToString(), dataTable.Rows[0]["DealerNo"].ToString());
					Program.Logger.Info("[驗證店家購肥帳密] -- 資料上傳至WS 結束");
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//RetailData");
					if (xmlNodeList.Count <= 0)
					{
						return "無WebService驗證回傳資料";
					}
					for (int i = 0; i < xmlNodeList.Count; i++)
					{
						string innerText = xmlNodeList[i].SelectSingleNode("status").InnerText;
						result = ((!"Y".Equals(innerText)) ? "購肥帳號密碼驗證錯誤" : "驗證成功");
						Program.Logger.Info("[驗證店家購肥帳密] -- 由WS收到訊息為\nS:" + (string.IsNullOrEmpty(innerText) ? "Null" : innerText));
					}
					return result;
				}
				catch (Exception ex)
				{
					Program.Logger.Fatal("[驗證店家購肥帳密] -- 發生例外狀況:" + ex.ToString());
					return "發生錯誤 : " + ex.Message;
				}
			}
		}
		else
		{
			Program.Logger.Info("[驗證店家購肥帳密] -- 離線中，請檢查網路");
			result = "偵測不到網路連線，請確認網路正常後再選入商品";
		}
		return result;
	}

	public string farmerInfo(string idNo)
	{
		Program.Logger.Info("[驗證購肥補助身分] -- 開始");
		string result = "";
		if (NetworkInterface.GetIsNetworkAvailable())
		{
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "FertilizerAccount,FertilizerPassword", "hypos_ShopInfoManage", "", "ShopIdNo limit 1", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable.Rows.Count > 0)
			{
				try
				{
					Program.Logger.Info("[驗證購肥補助身分] -- 資料上傳至WS 開始");
					POSService pOSService = new POSService();
					pOSService.Url = Program.VerificationURL;
					Program.Logger.Info("[驗證購肥補助身分] -- 上傳資料\nL:" + Program.LincenseCode + "\nFA:" + dataTable.Rows[0]["FertilizerAccount"].ToString() + "\nFP:" + dataTable.Rows[0]["FertilizerPassword"].ToString() + "\nID:" + idNo + "\n");
					string farmerInfo = pOSService.getFarmerInfo(Program.LincenseCode, dataTable.Rows[0]["FertilizerAccount"].ToString(), dataTable.Rows[0]["FertilizerPassword"].ToString(), idNo);
					Program.Logger.Info("[驗證購肥補助身分] -- 資料上傳至WS 結束");
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(farmerInfo);
					XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//FarmerData");
					if (xmlNodeList.Count <= 0)
					{
						return "WebService驗證錯誤";
					}
					for (int i = 0; i < xmlNodeList.Count; i++)
					{
						string innerText = xmlNodeList[i].SelectSingleNode("content").InnerText;
						Program.Logger.Info("[驗證購肥補助身分] -- 由WS收到訊息為\nC:" + (string.IsNullOrEmpty(innerText) ? "Null" : innerText));
						if ("符合補助資格".Equals(innerText.Trim()))
						{
							string innerText2 = xmlNodeList[i].SelectNodes("message")[0].SelectSingleNode("FrmState").InnerText;
							Program.Logger.Info("[驗證購肥補助身分] -- 由WS收到訊息為\nC:" + (string.IsNullOrEmpty(innerText2) ? "Null" : innerText2));
							result = ((!"1".Equals(innerText2.Trim())) ? "狀態不符合補助資格" : "符合補助資格");
						}
						else
						{
							result = "不符合補助資格";
						}
					}
					return result;
				}
				catch (Exception ex)
				{
					Program.Logger.Fatal("[驗證購肥補助身分] -- 發生例外狀況:" + ex.ToString());
					return "發生錯誤 : " + ex.Message;
				}
			}
		}
		else
		{
			Program.Logger.Info("[驗證購肥補助身分] -- 離線中，請檢查網路");
			result = "偵測不到網路連線，請確認網路正常後再使用檢查補助";
		}
		return result;
	}
}
