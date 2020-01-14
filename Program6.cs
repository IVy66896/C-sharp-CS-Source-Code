using NIISOL;
using System;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

internal static class Program
{
	private static bool _isDeployClickOnce = true;

	private static string _DataPath;

	private static string _ConnString;

	private static int _DBSchemaV = 1;

	public static bool IsDeployClickOnce
	{
		get
		{
			return _isDeployClickOnce;
		}
		set
		{
		}
	}

	public static string DataPath
	{
		get
		{
			return _DataPath;
		}
		set
		{
		}
	}

	public static string ConnectionString
	{
		get
		{
			return _ConnString;
		}
		set
		{
		}
	}

	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		_DataPath = (_isDeployClickOnce ? ApplicationDeployment.CurrentDeployment.DataDirectory : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
		_ConnString = "Data source=" + _DataPath + "\\NIISOL.db3;Password=1031;Version=3;Page Size=2048;Cache Size=4000;Synchronous=Full;";
		int dBVersion = GetDBVersion();
		if (dBVersion == -2)
		{
			string str = " CREATE TABLE `Record` ";
			str += " ( ";
			str += " Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ";
			str += " CaseName TEXT, ";
			str += " RocID TEXT, ";
			str += " Birthday TEXT, ";
			str += " ParentRocID TEXT, ";
			str += " BirthSeq INTEGER, ";
			str += " Tel TEXT, ";
			str += " Address TEXT, ";
			str += " VaccineCode TEXT NOT NULL, ";
			str += " InoculationDate TEXT NOT NULL, ";
			str += " VaccineNo TEXT NOT NULL, ";
			str += " VaccBatchNo TEXT NOT NULL,";
			str += " CreateDate DATETIME NOT NULL DEFAULT (DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')) ,";
			str += " IsFlu INTEGER NOT NULL DEFAULT 0 , ";
			str += " AgencyCode TEXT NOT NULL DEFAULT '' , ";
			str += " Sex TEXT NOT NULL DEFAULT 'M' , ";
			str += " ExportedDate TEXT, ";
			str += " LogicDel INTEGER DEFAULT 0  ";
			str += " ) ;";
			str += " CREATE TABLE `VaccineData` ";
			str += " ( ";
			str += " Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ";
			str += " VaccineCode TEXT, ";
			str += " VaccineNo TEXT, ";
			str += " VaccBatchNo TEXT, ";
			str += " ChName TEXT, ";
			str += " BatchType INTEGER ";
			str += " );";
			str += " CREATE TABLE `NIISOL_SysParam` ";
			str += " ( ";
			str += " DBSchemaVersion INTEGER NOT NULL  PRIMARY KEY  ";
			str += " );";
			str += " INSERT INTO NIISOL_SysParam (DBSchemaVersion) VALUES (1);";
			DataBaseUtilities.DBOperation(ConnectionString, str, new string[0], CommandOperationType.ExecuteNonQuery);
		}
		if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
		{
			copyShortcut("NIIS離線版本", "NIIS離線版本.appref-ms", "NIIS離線版本.appref-ms");
		}
		string text = "C:\\NIISOL\\";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		try
		{
			Application.Run(new Form_Login());
			Utility.WriteToFile(text + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tApplication.Run 執行成功", 'A', "");
		}
		catch (Exception ex)
		{
			Utility.WriteToFile(text + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tProgram.cs line61: " + ex.ToString(), 'A', "");
		}
	}

	private static void copyShortcut(string manufacturerName, string shortcutFileName, string newShortCutName)
	{
		string destFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + newShortCutName;
		string sourceFileName = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\" + manufacturerName + "\\" + shortcutFileName;
		string text = "C:\\NIISOL\\";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		try
		{
			File.Copy(sourceFileName, destFileName, overwrite: true);
		}
		catch (Exception ex)
		{
			Utility.WriteToFile(text + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tProgram.cs copyShortcut: " + ex.ToString(), 'A', "");
		}
	}

	public static int GetDBVersion()
	{
		try
		{
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(ConnectionString, TableOperation.Select, "*", "NIISOL_SysParam", "", "", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.Caption == "DBSchemaVersion")
				{
					return Convert.ToInt32(dataTable.Rows[0]["DBSchemaVersion"]);
				}
			}
			return 0;
		}
		catch (Exception)
		{
			return -2;
		}
	}
}
