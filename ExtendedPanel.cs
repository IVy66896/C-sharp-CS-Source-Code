using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class ExtendedPanel : Panel
{
	private const int WS_EX_TRANSPARENT = 32;

	private int opacity = 50;

	[DefaultValue(50)]
	public int Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			if (value < 0 || value > 100)
			{
				throw new ArgumentException("value must be between 0 and 100");
			}
			opacity = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = base.CreateParams;
			createParams.ExStyle |= 32;
			return createParams;
		}
	}

	public ExtendedPanel()
	{
		SetStyle(ControlStyles.Opaque, value: true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		using (SolidBrush brush = new SolidBrush(Color.FromArgb(opacity * 255 / 100, BackColor)))
		{
			e.Graphics.FillRectangle(brush, base.ClientRectangle);
		}
		base.OnPaint(e);
	}
}
