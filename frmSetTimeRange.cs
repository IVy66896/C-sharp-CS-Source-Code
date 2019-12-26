using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmSetTimeRange : Form
{
	private DateTime _SrtTime;

	private DateTime _EndTime;

	private IContainer components;

	private Panel panelPrint;

	private Button btnCancel;

	private Button btnChangeSetting;

	private DateTimePicker dtSrtTime;

	private Label label1;

	private Label label2;

	private DateTimePicker dtEndTime;

	public DateTime SrtTime
	{
		get
		{
			return _SrtTime;
		}
		set
		{
		}
	}

	public DateTime EndTime
	{
		get
		{
			return _EndTime;
		}
		set
		{
		}
	}

	public frmSetTimeRange(DateTime srtTime, DateTime endTime)
	{
		InitializeComponent();
		dtSrtTime.Value = srtTime;
		dtEndTime.Value = endTime;
		_SrtTime = srtTime;
		_EndTime = endTime;
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		try
		{
			_SrtTime = dtSrtTime.Value;
			_EndTime = dtEndTime.Value;
			base.DialogResult = DialogResult.OK;
		}
		catch (Exception)
		{
			MessageBox.Show("選取時間範圍失敗。");
		}
		finally
		{
			Close();
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmSetTimeRange));
		panelPrint = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		dtSrtTime = new System.Windows.Forms.DateTimePicker();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		dtEndTime = new System.Windows.Forms.DateTimePicker();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(dtEndTime);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Controls.Add(dtSrtTime);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Location = new System.Drawing.Point(2, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(491, 92);
		panelPrint.TabIndex = 2;
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(280, 9);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(43, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(43, 26);
		label2.TabIndex = 23;
		label2.Text = "至";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(8, 9);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(80, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(135, 26);
		label1.TabIndex = 22;
		label1.Text = "選取時間範圍從";
		dtSrtTime.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dtSrtTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		dtSrtTime.Location = new System.Drawing.Point(149, 9);
		dtSrtTime.Name = "dtSrtTime";
		dtSrtTime.Size = new System.Drawing.Size(125, 27);
		dtSrtTime.TabIndex = 21;
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(240, 47);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(154, 47);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 19;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		dtEndTime.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dtEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		dtEndTime.Location = new System.Drawing.Point(329, 9);
		dtEndTime.Name = "dtEndTime";
		dtEndTime.Size = new System.Drawing.Size(125, 27);
		dtEndTime.TabIndex = 24;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(498, 97);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmSetTimeRange";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "選取時間範圍";
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
