
using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using 家禽;

internal static class Program
{
	private const string VERSION = "3.3.0.1";

	private static bool _IsHyweb;

	public static bool IsDeployClickOnce
	{
		get;
		set;
	}

	public static bool Upgraded
	{
		get;
		set;
	}

	public static string ClickOnceUpdateDN
	{
		get;
		set;
	}

	public static int Counter
	{
		get;
		set;
	}

	[STAThread]
	private static void Main()
	{
		IsDeployClickOnce = true;
		ClickOnceUpdateDN = (_IsHyweb ? "mhr.hyweb.com.tw" : "mi.baphiq.gov.tw");
		Counter = 0;
		Upgraded = false;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
		{
			CommUtil.WriteToFile("Config.ini", "\r\nMDBPath=" + ApplicationDeployment.CurrentDeployment.DataDirectory, 'A', string.Empty);
			copyShortcut("家禽屠宰衛生檢查系統", "家禽屠宰衛生檢查系統.appref-ms", "家禽屠宰衛生檢查系統(V3.3.0.1版).appref-ms");
		}
		if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Common Files\\MSSoap\\Binaries\\MSSOAP30.dll"))
		{
			MessageBox.Show("您尚未安裝過SOAP, 現在開始安裝。");
			Process process = Process.Start("soapsdk.exe");
			while (!process.HasExited)
			{
				Application.DoEvents();
				process.WaitForExit(50);
			}
		}
		Application.Run(new frmUpdate());
		if (Upgraded)
		{
			Application.Restart();
		}
	}

	private static void copyShortcut(string manufacturerName, string shortcutFileName, string newShortCutName)
	{
		string destFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + newShortCutName;
		string sourceFileName = Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\" + manufacturerName + "\\" + shortcutFileName;
		try
		{
			File.Copy(sourceFileName, destFileName, overwrite: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}
}
