using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : MasterForm
{
	public string temp = "";

	private IContainer components;

	private Button button1;

	private Label label2;

	public Form1()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Dispose();
		Close();
	}

	private void Form1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.D0)
		{
			temp += "0";
		}
		else if (e.KeyCode == Keys.D1)
		{
			temp += "1";
		}
		else if (e.KeyCode == Keys.D2)
		{
			temp += "2";
		}
		else if (e.KeyCode == Keys.D3)
		{
			temp += "3";
		}
		else if (e.KeyCode == Keys.D4)
		{
			temp += "4";
		}
		else if (e.KeyCode == Keys.D5)
		{
			temp += "5";
		}
		else if (e.KeyCode == Keys.D6)
		{
			temp += "6";
		}
		else if (e.KeyCode == Keys.D7)
		{
			temp += "7";
		}
		else if (e.KeyCode == Keys.D8)
		{
			temp += "8";
		}
		else if (e.KeyCode == Keys.D9)
		{
			temp += "9";
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
		button1 = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		SuspendLayout();
		button1.Location = new System.Drawing.Point(247, 299);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(75, 23);
		button1.TabIndex = 1;
		button1.Text = "button1";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(244, 225);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(33, 12);
		label2.TabIndex = 34;
		label2.Text = "label2";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(977, 660);
		base.Controls.Add(label2);
		base.Controls.Add(button1);
		base.KeyPreview = true;
		base.Name = "Form1";
		Text = "Form1";
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(Form1_KeyDown);
		base.Controls.SetChildIndex(button1, 0);
		base.Controls.SetChildIndex(label2, 0);
		ResumeLayout(false);
		PerformLayout();
	}
}
