using Newtonsoft.Json;
using 離線列印Client程式;

public class ClProgramSettings : SerializeClass
{
	private string _ExpDateDesc;

	private string _UserInputDesc;

	private string _UserInputDfut;

	private bool _EnableUserInput;

	private int _ShowNextNDays;

	private bool _ShowPackDate;

	private int _UserInputOption;

	private bool _IsRotate;

	public JetPrintSetting JetPrintSettings;

	public bool DontDownloadAll;

	public bool IsRotate
	{
		get
		{
			return _IsRotate;
		}
		set
		{
			_IsRotate = value;
		}
	}

	public int ShowNextNDays
	{
		get
		{
			return _ShowNextNDays;
		}
		set
		{
			_ShowNextNDays = value;
		}
	}

	public bool EnableUserInput
	{
		get
		{
			return _EnableUserInput;
		}
		set
		{
			_EnableUserInput = value;
			if (!_EnableUserInput)
			{
				_UserInputDesc = string.Empty;
				_UserInputDfut = string.Empty;
			}
		}
	}

	public bool ShowPackDate
	{
		get
		{
			return _ShowPackDate;
		}
		set
		{
			_ShowPackDate = value;
		}
	}

	public int UserInputOption
	{
		get
		{
			return _UserInputOption;
		}
		set
		{
			_UserInputOption = value;
		}
	}

	public string ExpDateDesc
	{
		get
		{
			return _ExpDateDesc;
		}
		set
		{
			if (value.Length > 1 && value.Length < 5)
			{
				_ExpDateDesc = value;
			}
		}
	}

	public string UserInputDesc
	{
		get
		{
			return _UserInputDesc;
		}
		set
		{
			if (_EnableUserInput)
			{
				_UserInputDesc = value;
			}
		}
	}

	public string UserInputDfut
	{
		get
		{
			return _UserInputDfut;
		}
		set
		{
			if (_EnableUserInput)
			{
				_UserInputDfut = value;
			}
		}
	}

	public ClProgramSettings()
	{
		_ShowPackDate = true;
		_UserInputOption = 0;
		_IsRotate = false;
		SetDefaultIfNull();
	}

	public ClProgramSettings(string jsonTask)
	{
		_ShowPackDate = true;
		_UserInputOption = 0;
		_IsRotate = false;
		JsonConvert.PopulateObject(jsonTask, this);
		SetDefaultIfNull();
	}

	private void SetDefaultIfNull()
	{
		_ = _UserInputOption;
		if (_ExpDateDesc == null)
		{
			_ExpDateDesc = "保存日期";
		}
		if (_UserInputDesc == null)
		{
			_UserInputDesc = string.Empty;
		}
		if (_UserInputDfut == null)
		{
			_UserInputDfut = string.Empty;
		}
		if (_ShowNextNDays == 1)
		{
			_ShowNextNDays = 3;
		}
		if (JetPrintSettings == null)
		{
			JetPrintSettings = new JetPrintSetting();
		}
	}
}
