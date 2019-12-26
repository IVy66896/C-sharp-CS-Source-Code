using MultiLanquageModule;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : Form
{
	private MultiLanquageManager mulLangMng = new MultiLanquageManager("zh-TW");

	private IContainer components;

	private Button button1;

	private RadioButton radioButton1;

	private RadioButton radioButton2;

	private RadioButton radioButton3;

	public Form1()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		MessageBox.Show(mulLangMng.getLangString("appTitle"));
	}

	private void radioButton3_CheckedChanged(object sender, EventArgs e)
	{
		if (radioButton1.Checked)
		{
			mulLangMng.setLanquage("zh-TW");
		}
		else if (radioButton2.Checked)
		{
			mulLangMng.setLanquage("zh-CN");
		}
		else if (radioButton3.Checked)
		{
			mulLangMng.setLanquage("en");
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
		radioButton1 = new System.Windows.Forms.RadioButton();
		radioButton2 = new System.Windows.Forms.RadioButton();
		radioButton3 = new System.Windows.Forms.RadioButton();
		SuspendLayout();
		button1.Location = new System.Drawing.Point(118, 207);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(75, 23);
		button1.TabIndex = 0;
		button1.Text = "button1";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		radioButton1.AutoSize = true;
		radioButton1.Location = new System.Drawing.Point(47, 31);
		radioButton1.Name = "radioButton1";
		radioButton1.Size = new System.Drawing.Size(71, 16);
		radioButton1.TabIndex = 1;
		radioButton1.TabStop = true;
		radioButton1.Text = "中文繁體";
		radioButton1.UseVisualStyleBackColor = true;
		radioButton1.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
		radioButton2.AutoSize = true;
		radioButton2.Location = new System.Drawing.Point(47, 53);
		radioButton2.Name = "radioButton2";
		radioButton2.Size = new System.Drawing.Size(71, 16);
		radioButton2.TabIndex = 2;
		radioButton2.TabStop = true;
		radioButton2.Text = "中文簡體";
		radioButton2.UseVisualStyleBackColor = true;
		radioButton2.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
		radioButton3.AutoSize = true;
		radioButton3.Location = new System.Drawing.Point(47, 75);
		radioButton3.Name = "radioButton3";
		radioButton3.Size = new System.Drawing.Size(47, 16);
		radioButton3.TabIndex = 3;
		radioButton3.TabStop = true;
		radioButton3.Text = "英文";
		radioButton3.UseVisualStyleBackColor = true;
		radioButton3.CheckedChanged += new System.EventHandler(radioButton3_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 261);
		base.Controls.Add(radioButton3);
		base.Controls.Add(radioButton2);
		base.Controls.Add(radioButton1);
		base.Controls.Add(button1);
		base.Name = "Form1";
		Text = "Form1";
		ResumeLayout(false);
		PerformLayout();
	}
}
