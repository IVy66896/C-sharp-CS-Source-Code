
using HyReadLibraryHD;
using Microsoft.Win32;
using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

public class SplashWindow : Window, IComponentConnector
{
	private static Logger logger = LogManager.GetCurrentClassLogger();

	internal SplashWindow splashWindow;

	internal TextBlock statusText;

	internal ProgressBar progBar;

	private bool _contentLoaded;

	[DllImport("user32.dll")]
	[DebuggerStepThrough]
	private static extern bool IsWindowEnabled(IntPtr hWnd);

	public SplashWindow()
	{
		InitializeComponent();
		base.Loaded += Splash_Loaded;
		if (Global.regPath.Equals("NCLReader"))
		{
			base.Height = 280.0;
			Thickness i = progBar.Margin;
			i.Left = 120.0;
			i.Bottom = 20.0;
			progBar.Margin = i;
		}
	}

	private void Splash_Loaded(object sender, RoutedEventArgs e)
	{
		logger.Trace("begin Splash_Loaded");
		checkXPandFix();
		IAsyncResult result = null;
		logger.Trace("");
		AsyncCallback initCompleted = delegate
		{
			App.Current.ApplicationInitialize.EndInvoke(result);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
			{
				Close();
			});
		};
		result = App.Current.ApplicationInitialize.BeginInvoke(this, initCompleted, null);
		logger.Trace("end Splash_Loaded");
	}

	public void SetProgress(double progress)
	{
		base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
		{
			progBar.Value = progress;
		});
	}

	public void OpenLoadingPanel(string canvasText)
	{
		base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Invoker)delegate
		{
			statusText.Text = canvasText;
		});
	}

	private void checkSystem32()
	{
		if (!Environment.Is64BitOperatingSystem)
		{
			string value = "C:\\Program Files\\HyReadLibraryHD\\HyReadLibraryHD.exe /url \"%1\"";
			Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Classes\\hyread\\shell\\open\\command", "", value, RegistryValueKind.String);
		}
	}

	private void checkXPandFix()
	{
		checkSystem32();
		OperatingSystem osInfo = Environment.OSVersion;
		if (osInfo.Platform.GetHashCode() != 2 || osInfo.Version.Major.GetHashCode() != 5 || osInfo.Version.Minor.GetHashCode() != 1)
		{
			return;
		}
		if (Directory.Exists("c:\\windows\\prefetch\\"))
		{
			string[] files = Directory.GetFiles("c:\\windows\\prefetch\\", "*.pf");
			foreach (string file in files)
			{
				try
				{
					File.Delete(file);
				}
				catch
				{
				}
			}
		}
		object readvalue = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\" + Global.regPath, "FirstRun", "");
		if (readvalue != null && readvalue.ToString().Equals("TRUE"))
		{
			try
			{
				Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\Schedule", "Start", 4, RegistryValueKind.DWord);
				Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management\\PrefetchParameters", "EnablePrefetcher", 2, RegistryValueKind.DWord);
			}
			catch
			{
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocater = new Uri("/HyReadLibraryHD;component/splashwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocater);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			splashWindow = (SplashWindow)target;
			break;
		case 2:
			statusText = (TextBlock)target;
			break;
		case 3:
			progBar = (ProgressBar)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
