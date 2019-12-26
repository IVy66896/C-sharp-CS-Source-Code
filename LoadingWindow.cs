
using DownloadManagerModule;
using LoadingPage.Control;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

public class LoadingWindow : Window, IComponentConnector
{
	public string textInLoadingRing = "";

	public bool closedByCancelButton;

	private BookType createBookType;

	private object bookInfo;

	private string clickedVendorId;

	internal LoadingAnimation loadingPanel;

	internal Button cancelButton;

	private bool _contentLoaded;

	public LoadingWindow(BookType _bookType, object _bookInfo, string _textInLoadingRing)
	{
		textInLoadingRing = _textInLoadingRing;
		InitializeComponent();
		loadingPanel.LoadingPageText.Text = textInLoadingRing;
		createBookType = _bookType;
		bookInfo = _bookInfo;
		base.Loaded += LoadingToReadingPage_Loaded;
	}

	private void LoadingToReadingPage_Loaded(object sender, RoutedEventArgs e)
	{
		IAsyncResult result = null;
	}

	private void cancelButton_Click(object sender, RoutedEventArgs e)
	{
		textInLoadingRing = "";
		closedByCancelButton = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocater = new Uri("/HyReadLibraryHD;component/loadingwindow.xaml", UriKind.Relative);
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
			loadingPanel = (LoadingAnimation)target;
			break;
		case 2:
			cancelButton = (Button)target;
			cancelButton.Click += cancelButton_Click;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
