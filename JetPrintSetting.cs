
using System.Collections.Generic;

public class JetPrintSetting
{
	public int MaxPerFile;

	public int NumPrintSelected;

	public int NumFieldSelected;

	public string JetExeDefaultPath;

	public List<string> JetExecPaths;

	public string PrintFormatter;

	public JetPrintSetting()
	{
		MaxPerFile = 300;
		JetExeDefaultPath = string.Empty;
		PrintFormatter = string.Empty;
		JetExecPaths = new List<string>();
	}
}
