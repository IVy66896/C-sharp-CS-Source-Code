public class CheckboxItem
{
	public string Text
	{
		get;
		set;
	}

	public object Value
	{
		get;
		set;
	}

	public CheckboxItem(string text, object value)
	{
		Text = text;
		Value = value;
	}

	public override string ToString()
	{
		return Text;
	}
}
