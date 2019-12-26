
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

internal class NetworkInfo
{
	public static string ShowInterfaceInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		stringBuilder.AppendFormat("{0}.{1}的網路介面資訊:\n", iPGlobalProperties.HostName, iPGlobalProperties.DomainName);
		if (allNetworkInterfaces == null || allNetworkInterfaces.Length < 1)
		{
			stringBuilder.AppendFormat("找不到網路介面。\n");
			return stringBuilder.ToString();
		}
		stringBuilder.AppendFormat("找到{0}個網路介面\n", allNetworkInterfaces.Length);
		NetworkInterface[] array = allNetworkInterfaces;
		foreach (NetworkInterface networkInterface in array)
		{
			PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
			stringBuilder.AppendFormat("MAC address: {0}, name: {1}, 種類: {2}, OperationalStatus: {3}\n", physicalAddress, networkInterface.Name, networkInterface.NetworkInterfaceType, networkInterface.OperationalStatus);
		}
		return stringBuilder.ToString();
	}

	public static bool IsConnectionExist(string hostname)
	{
		try
		{
			Dns.GetHostEntry(hostname);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string GetMacAddr()
	{
		try
		{
			IPGlobalProperties.GetIPGlobalProperties();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			if (allNetworkInterfaces == null || allNetworkInterfaces.Length < 1)
			{
				return "";
			}
			NetworkInterface[] array = allNetworkInterfaces;
			foreach (NetworkInterface networkInterface in array)
			{
				PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
				if (physicalAddress.ToString() != null)
				{
					return physicalAddress.ToString();
				}
			}
		}
		catch (Exception)
		{
		}
		return "";
	}
}
