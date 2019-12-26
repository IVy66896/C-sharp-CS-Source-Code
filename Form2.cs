using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class Form2 : MasterForm
{
	private IContainer components;

	private Button button1;

	private Label label1;

	public Form2()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Dispose();
		Close();
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
		label1 = new System.Windows.Forms.Label();
		SuspendLayout();
		button1.Location = new System.Drawing.Point(107, 150);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(75, 23);
		button1.TabIndex = 0;
		button1.Text = "button1";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(98, 78);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(39, 12);
		label1.TabIndex = 1;
		label1.Text = "Form 2";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 261);
		base.Controls.Add(label1);
		base.Controls.Add(button1);
		base.Name = "Form2";
		Text = "Form2";
		ResumeLayout(false);
		PerformLayout();
	}
}
