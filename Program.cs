
using System;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;

internal static class Program
{
	private static bool _isDeployClickOnce = true;

	private static bool _isHyweb = false;

	private static int _DBSchemaV = 10;

	private static string _WebService1O = _isHyweb ? "taftadmin.hyweb.com.tw" : "taftadmin.coa.gov.tw";

	private static string _WebServiceOL = _isHyweb ? "taftadmin.hyweb.com.tw" : "taftadmin.coa.gov.tw";

	private static string _WSL1Url = "http://" + (_isHyweb ? "taftadmin.hyweb.com.tw" : "taftadmin.coa.gov.tw") + "/L1_1O.asmx";

	private static string _WSOLPUrl = "http://" + (_isHyweb ? "taftadmin.hyweb.com.tw" : "taftadmin.coa.gov.tw") + "/OffLinePrintWS/Service.asmx";

	private static string _PWD = "20n6JY81xY9QUvs";

	private static string _DBName = "DB.mdb";

	private static string _ExePath;

	private static string _ConnString;

	private static string _CRCFile;

	private static string _sysSerialNo;

	private static string _prevSysSerialNo;

	private static string _sysSerialNoWithoutHostName;

	private static string _valadationCode;

	private static string _ProducerName;

	private static string _OrgID;

	private static string _crcStatus;

	private static string _version;

	private static string _printerName;

	private static bool _isProgramUpgraded = false;

	private static bool _showHistory;

	private static bool _uploadNewSysSN = false;

	private static int _flushSomeDaySince;

	private static int _DisplayListItems;

	private static int _NextResumeID;

	private static int _ExpireDateAccum;

	private static PrintFormat _printFormat;

	private static UploadSySNType _UploadedSysSN;

	public static ClProgramSettings UserSettings;

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

	public static string CRCStatus
	{
		get
		{
			return _crcStatus;
		}
		set
		{
			_crcStatus = value;
		}
	}

	public static string ExePath
	{
		get
		{
			return _ExePath;
		}
		set
		{
		}
	}

	public static string SysSerialNo
	{
		get
		{
			return _sysSerialNo;
		}
		set
		{
		}
	}

	public static string SysSerialNoWithoutHostName
	{
		get
		{
			return _sysSerialNoWithoutHostName;
		}
		set
		{
		}
	}

	public static string PrevSysSerialNo
	{
		get
		{
			return _prevSysSerialNo;
		}
		set
		{
		}
	}

	public static string ValadationCode
	{
		get
		{
			return _valadationCode;
		}
		set
		{
		}
	}

	public static string WebServiceHostName1O
	{
		get
		{
			return _WebService1O;
		}
		set
		{
		}
	}

	public static string WebServiceHostNameOL
	{
		get
		{
			return _WebServiceOL;
		}
		set
		{
		}
	}

	public static string ProducerName
	{
		get
		{
			return _ProducerName;
		}
		set
		{
			_ProducerName = value;
		}
	}

	public static string OrgID
	{
		get
		{
			return _OrgID;
		}
		set
		{
			_OrgID = value;
		}
	}

	public static string Version
	{
		get
		{
			return _version;
		}
		set
		{
		}
	}

	public static string L1Url
	{
		get
		{
			return _WSL1Url;
		}
		set
		{
		}
	}

	public static string OLPUrl
	{
		get
		{
			return _WSOLPUrl;
		}
		set
		{
		}
	}

	public static string PrinterName
	{
		get
		{
			return _printerName;
		}
		set
		{
			_printerName = value;
		}
	}

	public static bool IsHyweb
	{
		get
		{
			return _isHyweb;
		}
		set
		{
		}
	}

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

	public static bool ShowHistory
	{
		get
		{
			return _showHistory;
		}
		set
		{
			_showHistory = value;
		}
	}

	public static bool Upgraded
	{
		get
		{
			return _isProgramUpgraded;
		}
		set
		{
			_isProgramUpgraded = value;
		}
	}

	public static bool UploadNewSysSN
	{
		get
		{
			return _uploadNewSysSN;
		}
		set
		{
			_uploadNewSysSN = value;
		}
	}

	public static int DayToFlush
	{
		get
		{
			return _flushSomeDaySince;
		}
		set
		{
			_flushSomeDaySince = value;
		}
	}

	public static int DisplayListItems
	{
		get
		{
			return _DisplayListItems;
		}
		set
		{
			_DisplayListItems = value;
		}
	}

	public static int NextResumeID
	{
		get
		{
			return _NextResumeID;
		}
		set
		{
			_NextResumeID = value;
		}
	}

	public static int ExpireDateAccum
	{
		get
		{
			return _ExpireDateAccum;
		}
		set
		{
			_ExpireDateAccum = value;
		}
	}

	public static PrintFormat ThisPrintFormat
	{
		get
		{
			return _printFormat;
		}
		set
		{
			_printFormat = value;
		}
	}

	public static UploadSySNType UploadedSysSN
	{
		get
		{
			return _UploadedSysSN;
		}
		set
		{
			_UploadedSysSN = value;
		}
	}

	[STAThread]
	private static void Main()
	{
		try
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			bool createdNew;
			Mutex mutex = new Mutex(initiallyOwned: true, Application.ProductName, out createdNew);
			if (createdNew)
			{
				if (InitSystemParams())
				{
					string dBSysSerialNo = GetDBSysSerialNo();
					if (dBSysSerialNo == "")
					{
						Application.Run(new frmInitSysParam());
					}
					else if (dBSysSerialNo == _prevSysSerialNo || dBSysSerialNo == _sysSerialNoWithoutHostName)
					{
						string[,] strFieldArray = new string[1, 2]
						{
							{
								"SysSerialNo",
								_sysSerialNo
							}
						};
						DataBaseUtilities.DBOperation(ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
						_prevSysSerialNo = dBSysSerialNo;
						_uploadNewSysSN = true;
						Application.Run(new frmMain());
					}
					else if (dBSysSerialNo != _sysSerialNo)
					{
						if (UploadedSysSN != UploadSySNType.序號改變已上傳)
						{
							UploadedSysSN = UploadSySNType.序號改變未上傳;
						}
						Application.Run(new frmInitSysParam());
					}
					else
					{
						Application.Run(new frmMain());
					}
					WriteCRC();
				}
				mutex.ReleaseMutex();
				if (_isProgramUpgraded)
				{
					Application.Restart();
				}
			}
			else
			{
				MessageBox.Show($" \"{Application.ProductName}\" 執行中", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Main發生例外狀況:「{ex.ToString()}」");
		}
	}

	private static bool InitSystemParams()
	{
		try
		{
			try
			{
				_ = _isDeployClickOnce;
				_ExePath = (_isDeployClickOnce ? ApplicationDeployment.CurrentDeployment.DataDirectory : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			}
			catch (Exception)
			{
			}
			_ConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _ExePath + "\\" + _DBName + ";Jet OLEDB:Database Password=" + _PWD;
			_CRCFile = _ExePath + "\\Conn.log";
			_sysSerialNo = GetSysSerialNo("", NetworkInfo.HostName(withDomainName: false) + NetworkInfo.GetMacAddr(), "");
			_sysSerialNoWithoutHostName = GetSysSerialNo("", NetworkInfo.GetMacAddr(), "");
			_prevSysSerialNo = GetPrevSysSerialNo("", NetworkInfo.GetMacAddr(), "");
			if (_sysSerialNo == "")
			{
				MessageBox.Show("無法取得系統序號");
				return false;
			}
			_valadationCode = GetValadationCode(_sysSerialNo);
			_crcStatus = findIsDBAltered();
			_version = (_isDeployClickOnce ? $"{ApplicationDeployment.CurrentDeployment.CurrentVersion.Major}.{ApplicationDeployment.CurrentDeployment.CurrentVersion.Minor}.{ApplicationDeployment.CurrentDeployment.CurrentVersion.Build}.{ApplicationDeployment.CurrentDeployment.CurrentVersion.Revision}" : "1.0");
			CheckDBSchema();
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(ConnectionString, TableOperation.Select, "ProducerName, ShowHistory, DayToFlush, PrintFormat, DefaultPrinterName, UploadedSysSN, DisplayList, ExpireDateAccum, OrgID ", "SysParam", "", "", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable.Rows.Count > 0)
			{
				_ProducerName = dataTable.Rows[0]["ProducerName"].ToString();
				_showHistory = ((dataTable.Rows[0]["ShowHistory"].ToString() == "Y") ? true : false);
				int num = Convert.ToInt32(dataTable.Rows[0]["DayToFlush"]);
				_flushSomeDaySince = ((num > 0 && num < 91) ? (-(num - 1)) : (-2));
				_DisplayListItems = (dataTable.Rows[0]["DisplayList"].Equals(DBNull.Value) ? 8191 : Convert.ToInt32(dataTable.Rows[0]["DisplayList"]));
				_printFormat = (PrintFormat)((!dataTable.Rows[0]["PrintFormat"].Equals(DBNull.Value)) ? Convert.ToInt32(dataTable.Rows[0]["PrintFormat"]) : 0);
				_printerName = dataTable.Rows[0]["DefaultPrinterName"].ToString();
				_ExpireDateAccum = ((!dataTable.Rows[0]["ExpireDateAccum"].Equals(DBNull.Value)) ? Convert.ToInt32(dataTable.Rows[0]["ExpireDateAccum"]) : 0);
				_UploadedSysSN = (UploadSySNType)((!dataTable.Rows[0]["UploadedSysSN"].Equals(DBNull.Value)) ? Convert.ToInt32(dataTable.Rows[0]["UploadedSysSN"]) : 0);
				_OrgID = dataTable.Rows[0]["OrgID"].ToString();
			}
			int num2 = 0;
			try
			{
				num2 = Convert.ToInt32(DataBaseUtilities.DBOperation(ConnectionString, "select iif(IsNull( max(ResumeID) ),0, max(ResumeID) ) from ResumeHistory", null, CommandOperationType.ExecuteScalar));
			}
			catch (Exception)
			{
				DataBaseUtilities.DBOperation(ConnectionString, "select iif(IsNull( max(ResumeID) ),0, max(ResumeID) ) from ResumeHistory", null, CommandOperationType.ExecuteScalar);
			}
			int num3 = 0;
			try
			{
				num3 = Convert.ToInt32(DataBaseUtilities.DBOperation(ConnectionString, "select iif(IsNull( max(ResumeID) ),0, max(ResumeID) ) from ResumeCurrent", null, CommandOperationType.ExecuteScalar));
			}
			catch (Exception)
			{
				DataBaseUtilities.DBOperation(ConnectionString, "select iif(IsNull( max(ResumeID) ),0, max(ResumeID) ) from ResumeCurrent", null, CommandOperationType.ExecuteScalar);
			}
			if (num2 == 0 && num3 == 0)
			{
				_NextResumeID = 1;
			}
			else if (num2 > num3)
			{
				_NextResumeID = num2 + 1;
			}
			else
			{
				_NextResumeID = num3 + 1;
			}
			if (TaftOLP.Default.UserSetting == string.Empty)
			{
				UserSettings = new ClProgramSettings();
				TaftOLP.Default.UserSetting = UserSettings.Serialize(ident: false);
				TaftOLP.Default.Save();
			}
			else
			{
				UserSettings = new ClProgramSettings(TaftOLP.Default.UserSetting);
			}
			return true;
		}
		catch (Exception ex4)
		{
			MessageBox.Show($"初始化程式參數過程中發生例外狀況:「{ex4.ToString()}」\r\n\r\n程式執行路徑為:「{ExePath}」", "例外訊息", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return false;
	}

	private static string GetSysSerialNo(string hash1, string hash2, string hash3)
	{
		return Encrypt.GetMD5("YGwkfk5" + hash1 + hash2 + hash3 + "HOlWmPR3");
	}

	private static string GetValadationCode(string sn)
	{
		return Encrypt.GetMD5("U6Xb8qY5" + sn + "q7wf3l2");
	}

	private static string GetPrevSysSerialNo(string hash1, string hash2, string hash3)
	{
		return Encrypt.GetMD5(hash1 + hash2 + hash3).Substring(7, 20);
	}

	private static string GetPrevValadationCode(string sn)
	{
		return Encrypt.GetMD5(sn).Substring(5, 20);
	}

	private static string GetDBSysSerialNo()
	{
		return Convert.ToString(DataBaseUtilities.DBOperation(ConnectionString, TableOperation.Select, "SysSerialNo", "SysParam", "", "", null, null, CommandOperationType.ExecuteScalar));
	}

	private static int GetDBVersion()
	{
		try
		{
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(ConnectionString, TableOperation.Select, "*", "SysParam", "", "", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
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

	private static void CheckDBSchema()
	{
		int dBVersion = GetDBVersion();
		if (dBVersion != -2 && dBVersion != _DBSchemaV)
		{
			try
			{
				if (dBVersion < 1)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add DBSchemaVersion int default 0", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add Unit Text(50)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add Unit Text(50)", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 2)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add PreTraceCode Text(50)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add PreTraceCode Text(50)", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 3)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add PrintFormat int default 0", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add ExpDate datetime", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add ExpDate datetime", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 4)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add DefaultPrinterName Text(255)", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 5)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add UploadedSysSN int default 0", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 6)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add DisplayList int default 0", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add ResumeID int", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add ReceiveOrg Text(255)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add ResumeID int", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add ReceiveOrg Text(255)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table FlushPrintCode add DisposeReason Text(255)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "CREATE TABLE ResumeDispose(ResumeID int, PrintCodeSrt Text(255), PrintCodeEnd Text(255), PintCodeCount int, DisposeReason Text(255));", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 7)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table SysParam add ExpireDateAccum int default 0", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 8)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add Producer Text(255)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add Producer Text(255)", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 9)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeCurrent add SourceProducer Text(255)", null, CommandOperationType.ExecuteNonQuery);
					DataBaseUtilities.DBOperation(ConnectionString, "alter table ResumeHistory add SourceProducer Text(255)", null, CommandOperationType.ExecuteNonQuery);
				}
				if (dBVersion < 10)
				{
					DataBaseUtilities.DBOperation(ConnectionString, "CREATE TABLE FarmerData(FarmerName Text(255), FarmerAddress Text(255), FarmerTel Text(255));", null, CommandOperationType.ExecuteNonQuery);
				}
				DataBaseUtilities.DBOperation(ConnectionString, "Update SysParam set DBSchemaVersion = '" + _DBSchemaV + "'", null, CommandOperationType.ExecuteNonQuery);
			}
			catch (Exception)
			{
			}
		}
	}

	private static string findIsDBAltered()
	{
		if (GetDBSysSerialNo() == "")
		{
			return "InitializeProgram";
		}
		if (!File.Exists(_CRCFile))
		{
			return "CRCLogMissing";
		}
		using (StreamReader streamReader = new StreamReader(_CRCFile))
		{
			string text = streamReader.ReadLine();
			if (text == null)
			{
				return "CRCLogNULL";
			}
			CRC32 cRC = new CRC32();
			string text2 = "";
			text2 = cRC.GetCRC(_ExePath + "\\" + _DBName);
			if (text != Encrypt.GetMD5(text2))
			{
				return "CRCLogAltered";
			}
			return "CRCLogOK";
		}
	}

	public static void ReloadCRC()
	{
		if (_crcStatus == "CRCLogOK")
		{
			_crcStatus = findIsDBAltered();
		}
	}

	public static void WriteCRC()
	{
		if (_crcStatus == "CRCLogOK")
		{
			CRC32 cRC = new CRC32();
			string text = "";
			text = cRC.GetCRC(_ExePath + "\\" + _DBName);
			CommonUtilities.WriteToFile(_CRCFile, Encrypt.GetMD5(text), 'W', "");
		}
	}
}
