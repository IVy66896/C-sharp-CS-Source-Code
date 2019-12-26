
using System;
using System.Security.Cryptography;
using System.Text;
using 離線列印Client程式;

internal class PrintCodeUtilties
{
	public static string printCodeTransform(string str)
	{
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
		int num = 0;
		try
		{
			num = ((str.Length <= 6) ? ((int)array[Convert.ToInt64(str) % array.Length] % 100) : ((int)array[(Convert.ToInt64(str) - Convert.ToInt32(str.Substring(3, 4))) % array.Length] % 100));
		}
		catch
		{
			num = (int)array[(array.Length > 5) ? 5 : array.Length] % 100;
		}
		return $"{str}{num:00}";
	}

	public static string GetNextNPrintCode(string currentPC, int accum)
	{
		if (!CommonUtilities.isInteger(currentPC) || accum < 0)
		{
			return "";
		}
		if (accum == 1)
		{
			return currentPC;
		}
		int length = currentPC.Length;
		string text = Convert.ToString(Convert.ToInt64(currentPC) + accum - 1);
		while (text.Length < length)
		{
			text = "0" + text;
		}
		return text;
	}

	public static string PrtFmtShowName(PrintFormat thisFormat)
	{
		switch (thisFormat)
		{
		case PrintFormat.標籤45X52:
			return "標籤1(45X52)";
		case PrintFormat.標籤45X52顯示保存日期:
			return "標籤2(45X52)";
		case PrintFormat.標籤45X52無EAN:
			return "標籤3(45X52)";
		case PrintFormat.標籤56X30:
			return "標籤4(56X30)";
		case PrintFormat.畜產標籤:
			return "標籤5(45X42)";
		case PrintFormat.標籤75X42:
			return "標籤6(75X42)";
		case PrintFormat.標籤45X52無EAN加自行輸入:
			return "標籤7(45X52)";
		case PrintFormat.噴墨列印:
			return "噴墨列印";
		case PrintFormat.標籤75X42無EAN:
			return "標籤8(75X42)";
		case PrintFormat.標籤45X52含生產者資訊:
			return "標籤9(45X52)";
		case PrintFormat.標籤75X42含生產者資訊:
			return "標籤10(75X42)";
		case PrintFormat.標籤1品項:
			return "標籤1(品項字數32)";
		case PrintFormat.標籤2品項:
			return "標籤2(品項字數28)";
		case PrintFormat.標籤3品項:
			return "標籤3(品項字數33)";
		case PrintFormat.標籤5品項:
			return "標籤5(品項字數28)";
		case PrintFormat.標籤6品項:
			return "標籤6(品項字數36)";
		case PrintFormat.標籤7品項:
			return "標籤7(品項字數42)";
		case PrintFormat.標籤8品項:
			return "標籤8(品項字數32)";
		case PrintFormat.標籤9品項:
			return "標籤9(品項字數42)";
		case PrintFormat.標籤10品項:
			return "標籤10(品項字數36)";
		default:
			return "其他";
		}
	}
}
