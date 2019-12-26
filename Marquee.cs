
using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

public class Marquee : UserControl
{
	private float currentPos;

	private bool mBorder;

	private string mText;

	private IContainer components;

	private System.Timers.Timer timer1;

	public string MarqueeText
	{
		get
		{
			return mText;
		}
		set
		{
			mText = value;
		}
	}

	public bool Border
	{
		get
		{
			return mBorder;
		}
		set
		{
			mBorder = value;
		}
	}

	public double Interval
	{
		get
		{
			return timer1.Interval * 10.0;
		}
		set
		{
			timer1.Interval = value / 10.0;
		}
	}

	public Marquee()
	{
		InitializeComponent();
		base.Size = new Size(base.Width, Font.Height);
	}

	private void Marquee_Resize(object sender, EventArgs e)
	{
		base.Height = Font.Height;
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (mBorder)
		{
			e.Graphics.DrawRectangle(new Pen(ForeColor), 0, 0, base.Width - 1, base.Height - 1);
		}
		float num = e.Graphics.MeasureString(mText, Font).Height - (float)base.Height;
		if ((float)base.Height < e.Graphics.MeasureString(mText, Font).Height - 1f)
		{
			if (Math.Abs(currentPos) % (float)base.FontHeight == 0f && Math.Abs(currentPos) > 0f)
			{
				e.Graphics.DrawString(mText, Font, new SolidBrush(ForeColor), 0f, currentPos);
			}
			e.Graphics.DrawString(mText, Font, new SolidBrush(ForeColor), 0f, currentPos);
			e.Graphics.DrawString(mText, Font, new SolidBrush(ForeColor), 0f, (float)base.Height + currentPos + num);
			currentPos -= 1f;
			if (currentPos < 0f && Math.Abs(currentPos) >= e.Graphics.MeasureString(mText, Font).Height - 1f)
			{
				currentPos = 0f;
			}
		}
		else
		{
			e.Graphics.DrawString(mText, Font, new SolidBrush(ForeColor), 0f, currentPos);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		timer1 = new System.Timers.Timer();
		SuspendLayout();
		timer1.Enabled = true;
		timer1.Interval = 1000.0;
		timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
		Font = new System.Drawing.Font("微軟正黑體", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		base.Name = "Marquee";
		base.Size = new System.Drawing.Size(150, 136);
		base.Resize += new System.EventHandler(Marquee_Resize);
		ResumeLayout(false);
	}
}
