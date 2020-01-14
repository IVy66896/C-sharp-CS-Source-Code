using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Text;

internal class Utility
{
	public static string ToDateString(string TaiwanYearDate)
	{
		string result = "";
		if (TaiwanYearDate.Length == 6)
		{
			TaiwanYearDate = "0" + TaiwanYearDate;
		}
		if (TaiwanYearDate.Length == 7)
		{
			try
			{
				result = Convert.ToDateTime(Convert.ToInt32(TaiwanYearDate.Substring(0, 3)) + 1911 + "/" + TaiwanYearDate.Substring(3, 2) + "/" + TaiwanYearDate.Substring(5, 2)).ToString("yyyy/MM/dd");
			}
			catch
			{
			}
		}
		return result;
	}

	public static string ToRocDateString(DateTime datetime)
	{
		string text = "";
		text = datetime.Year - 1911 + datetime.ToString("MMdd");
		if (text.Length == 6)
		{
			text = "0" + text;
		}
		return text;
	}

	public static bool IsResNo(string arg_ResNo)
	{
		bool result = false;
		if (arg_ResNo.Length == 10)
		{
			arg_ResNo = arg_ResNo.ToUpper();
			if (arg_ResNo[0] >= 'A' && arg_ResNo[0] <= 'Z' && arg_ResNo[1] >= 'A' && arg_ResNo[1] <= 'D')
			{
				int[] array = new int[26]
				{
					10,
					11,
					12,
					13,
					14,
					15,
					16,
					17,
					34,
					18,
					19,
					20,
					21,
					22,
					35,
					23,
					24,
					25,
					26,
					27,
					28,
					29,
					32,
					30,
					31,
					33
				};
				int[] array2 = new int[11]
				{
					array[arg_ResNo[0] - 65] / 10,
					array[arg_ResNo[0] - 65] % 10,
					array[arg_ResNo[1] - 65] % 10,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				};
				int num = array2[0];
				num += array2[1] * 9;
				for (int i = 2; i <= 9; i++)
				{
					array2[i + 1] = arg_ResNo[i] - 48;
					num += array2[i] * (10 - i);
				}
				if ((num % 10 + array2[10]) % 10 == 0)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public static bool IsIdNo(string arg_Identify)
	{
		bool result = false;
		if (arg_Identify.Length == 10)
		{
			arg_Identify = arg_Identify.ToUpper();
			if (arg_Identify[0] >= 'A' && arg_Identify[0] <= 'Z')
			{
				int[] array = new int[26]
				{
					10,
					11,
					12,
					13,
					14,
					15,
					16,
					17,
					34,
					18,
					19,
					20,
					21,
					22,
					35,
					23,
					24,
					25,
					26,
					27,
					28,
					29,
					32,
					30,
					31,
					33
				};
				int[] array2 = new int[11]
				{
					0,
					array[arg_Identify[0] - 65] % 10,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				};
				int num = array2[0] = array[arg_Identify[0] - 65] / 10;
				for (int i = 1; i <= 9; i++)
				{
					array2[i + 1] = arg_Identify[i] - 48;
					num += array2[i] * (10 - i);
				}
				if ((num % 10 + array2[10]) % 10 == 0)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public static Stream RenderDataTableToExcel(DataTable SourceTable)
	{
		HSSFWorkbook hSSFWorkbook = new HSSFWorkbook();
		MemoryStream memoryStream = new MemoryStream();
		ISheet sheet = hSSFWorkbook.CreateSheet();
		IRow row = sheet.CreateRow(0);
		foreach (DataColumn column in SourceTable.Columns)
		{
			row.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
		}
		int num = 1;
		foreach (DataRow row3 in SourceTable.Rows)
		{
			IRow row2 = sheet.CreateRow(num);
			foreach (DataColumn column2 in SourceTable.Columns)
			{
				row2.CreateCell(column2.Ordinal).SetCellValue(row3[column2].ToString());
			}
			num++;
		}
		hSSFWorkbook.Write(memoryStream);
		memoryStream.Flush();
		memoryStream.Position = 0L;
		sheet = null;
		row = null;
		hSSFWorkbook = null;
		return memoryStream;
	}

	public static void WriteToFile(string strFileName, string strToWrite, char chFileMode, string strEncode)
	{
		Encoding encoding = Encoding.GetEncoding((strEncode == "") ? "UTF-8" : strEncode);
		using (StreamWriter streamWriter = new StreamWriter(File.Open(strFileName, (File.Exists(strFileName) && chFileMode == 'A') ? FileMode.Append : FileMode.Create), encoding))
		{
			streamWriter.WriteLine(strToWrite);
		}
	}
}
