using Network;
using System.Xml;

public class IpadService
{
	private HttpRequest request = new HttpRequest();

	public string message;

	public string realUserId;

	public string serialId;

	private string getInnerTextSafely(XmlNode targetNode)
	{
		if (targetNode == null)
		{
			return "";
		}
		if (targetNode.HasChildNodes)
		{
			return targetNode.InnerText;
		}
		return "";
	}

	public string userOnlineLogin(string urlBase, string vendorId, string colibId, string hyreadType, string account, string password)
	{
		string serviceUrl = urlBase + "/" + vendorId + "/user/check";
		string result = "";
		string postData4 = "<body>";
		postData4 = postData4 + "<account><![CDATA[" + account + "]]></account>";
		postData4 = postData4 + "<password><![CDATA[" + password + "]]></password>";
		postData4 += "</body>";
		XmlDocument xmlDoc = request.postXMLAndLoadXML(serviceUrl, postData4);
		try
		{
			result = xmlDoc.SelectSingleNode("//result/text()").Value;
			message = xmlDoc.SelectSingleNode("//message/text()").Value;
			realUserId = xmlDoc.SelectSingleNode("//userId/text()").Value;
			serialId = xmlDoc.SelectSingleNode("//serialId/text()").Value;
		}
		catch
		{
			message = "登入服務失敗";
		}
		if (result.ToUpper().Equals("TRUE"))
		{
			return "TRUE";
		}
		return message;
	}

	public XmlDocument userBookList(string urlBase, string vendorId, string colibId, string hyreadType, string account)
	{
		string serviceUrl = urlBase + "/" + vendorId + "/book/lendlist";
		if (hyreadType.Equals("2"))
		{
			serviceUrl = urlBase + "/" + vendorId + "/book/list";
		}
		string postData6 = "<body>";
		postData6 = postData6 + "<account>" + account + "</account>";
		postData6 = postData6 + "<colibId>" + colibId + "</colibId>";
		postData6 = postData6 + "<hyreadType>" + hyreadType + "</hyreadType>";
		postData6 += "<merge>1</merge>";
		postData6 += "</body>";
		return request.postXMLAndLoadXML(serviceUrl, postData6);
	}

	public XmlDocument deviceList(string urlBase, string vendorId, string colibId, string hyreadType, string account)
	{
		string serviceUrl2 = urlBase + "/" + vendorId + "/device/list";
		serviceUrl2 = serviceUrl2.Replace("https://service.ebook.hyread.com.tw", "http://openebook.hyread.com.tw");
		string postData5 = "<body>";
		postData5 = postData5 + "<userId><![CDATA[" + account + "]]></userId>";
		postData5 = postData5 + "<colibId>" + colibId + "</colibId>";
		postData5 = postData5 + "<hyreadType>" + hyreadType + "</hyreadType>";
		postData5 += "</body>";
		return new HttpRequest().postXMLAndLoadXML(serviceUrl2, postData5);
	}

	public string deviceAdd(string urlBase, string vendorId, string colibId, string hyreadType, string account, string deviceId, string deviceName)
	{
		string result = "";
		string serviceUrl2 = urlBase + "/" + vendorId + "/device/add";
		serviceUrl2 = serviceUrl2.Replace("https://service.ebook.hyread.com.tw", "http://openebook.hyread.com.tw");
		string postData11 = "<body>";
		postData11 = postData11 + "<userId>" + account + "</userId>";
		postData11 = postData11 + "<vendor>" + vendorId + "</vendor>";
		postData11 = postData11 + "<colibId>" + colibId + "</colibId>";
		postData11 = postData11 + "<deviceId>" + hyreadType + "</deviceId>";
		postData11 = postData11 + "<deviceName>" + hyreadType + "</deviceName>";
		postData11 += "<device>3</device>";
		postData11 += "<brandName>PC</brandName>";
		postData11 += "<modelName>PC</modelName>";
		postData11 += "<version>1.0.0</version>";
		postData11 += "</body>";
		XmlDocument xmlDoc = new HttpRequest().postXMLAndLoadXML(serviceUrl2, postData11);
		try
		{
			result = xmlDoc.SelectSingleNode("//result/text()").Value;
			message = xmlDoc.SelectSingleNode("//message/text()").Value;
		}
		catch
		{
			message = "註冊裝置服務失敗";
		}
		if (result.ToUpper().Equals("TRUE"))
		{
			return "TRUE";
		}
		return message;
	}

	public string deviceRemove(string urlBase, string vendorId, string colibId, string hyreadType, string account, string deviceId)
	{
		string result = "";
		string serviceUrl2 = urlBase + "/" + vendorId + "/device/remove";
		serviceUrl2 = serviceUrl2.Replace("https://service.ebook.hyread.com.tw", "http://openebook.hyread.com.tw");
		string postData5 = "<body>";
		postData5 = postData5 + "<userId>" + account + "</userId>";
		postData5 = postData5 + "<colibId>" + colibId + "</colibId>";
		postData5 = postData5 + "<deviceId>" + hyreadType + "</deviceId>";
		postData5 += "</body>";
		XmlDocument xmlDoc = request.postXMLAndLoadXML(serviceUrl2, postData5);
		try
		{
			result = xmlDoc.SelectSingleNode("//result/text()").Value;
			message = xmlDoc.SelectSingleNode("//message/text()").Value;
		}
		catch
		{
			message = "刪除裝置服務失敗";
		}
		if (result.ToUpper().Equals("TRUE"))
		{
			return "TRUE";
		}
		return message;
	}

	public string deviceExist(string urlBase, string vendorId, string colibId, string hyreadType, string account, string deviceId)
	{
		string result = "";
		string serviceUrl2 = urlBase + "/" + vendorId + "/device/exist";
		serviceUrl2 = serviceUrl2.Replace("https://service.ebook.hyread.com.tw", "http://openebook.hyread.com.tw");
		string postData5 = "<body>";
		postData5 = postData5 + "<userId>" + account + "</userId>";
		postData5 = postData5 + "<colibId>" + colibId + "</colibId>";
		postData5 = postData5 + "<deviceId>" + hyreadType + "</deviceId>";
		postData5 += "</body>";
		XmlDocument xmlDoc = request.postXMLAndLoadXML(serviceUrl2, postData5);
		try
		{
			result = xmlDoc.SelectSingleNode("//result/text()").Value;
			message = xmlDoc.SelectSingleNode("//message/text()").Value;
		}
		catch
		{
			message = "檢查裝置服務失敗";
		}
		if (result.ToUpper().Equals("TRUE"))
		{
			return "TRUE";
		}
		return message;
	}
}
