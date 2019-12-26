
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmSetDisplayListItem : Form
{
	private int _DisplayListItem;

	private IContainer components;

	private Panel panelPrint;

	private Button btnCancel;

	private Button btnChangeSetting;

	private Label label2;

	private Label label1;

	private Label label4;

	private CheckBox cb_1;

	private Label label13;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private CheckBox cb_12;

	private CheckBox cb_11;

	private CheckBox cb_10;

	private CheckBox cb_9;

	private CheckBox cb_8;

	private CheckBox cb_7;

	private CheckBox cb_6;

	private CheckBox cb_5;

	private CheckBox cb_4;

	private CheckBox cb_3;

	private CheckBox cb_2;

	private CheckBox cb_13;

	private Label label3;

	public int DisplayListItem
	{
		get
		{
			return _DisplayListItem;
		}
		set
		{
		}
	}

	public frmSetDisplayListItem(int currentListItems)
	{
		InitializeComponent();
		_DisplayListItem = currentListItems;
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		try
		{
			_DisplayListItem = GetList();
			base.DialogResult = DialogResult.OK;
		}
		catch (Exception)
		{
			MessageBox.Show("清單欄位儲存失敗。");
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

	private void frmSetDisplayListItem_Load(object sender, EventArgs e)
	{
		cb_1.Checked = ((_DisplayListItem & 1) > 0);
		cb_2.Checked = ((_DisplayListItem & 2) > 0);
		cb_3.Checked = ((_DisplayListItem & 4) > 0);
		cb_4.Checked = ((_DisplayListItem & 8) > 0);
		cb_5.Checked = ((_DisplayListItem & 0x10) > 0);
		cb_6.Checked = ((_DisplayListItem & 0x20) > 0);
		cb_7.Checked = ((_DisplayListItem & 0x40) > 0);
		cb_8.Checked = ((_DisplayListItem & 0x80) > 0);
		cb_9.Checked = ((_DisplayListItem & 0x100) > 0);
		cb_10.Checked = ((_DisplayListItem & 0x200) > 0);
		cb_11.Checked = ((_DisplayListItem & 0x400) > 0);
		cb_12.Checked = ((_DisplayListItem & 0x800) > 0);
		cb_13.Checked = ((_DisplayListItem & 0x1000) > 0);
	}

	private int GetList()
	{
		int num = 0;
		if (cb_1.Checked)
		{
			num++;
		}
		if (cb_2.Checked)
		{
			num += 2;
		}
		if (cb_3.Checked)
		{
			num += 4;
		}
		if (cb_4.Checked)
		{
			num += 8;
		}
		if (cb_5.Checked)
		{
			num += 16;
		}
		if (cb_6.Checked)
		{
			num += 32;
		}
		if (cb_7.Checked)
		{
			num += 64;
		}
		if (cb_8.Checked)
		{
			num += 128;
		}
		if (cb_9.Checked)
		{
			num += 256;
		}
		if (cb_10.Checked)
		{
			num += 512;
		}
		if (cb_11.Checked)
		{
			num += 1024;
		}
		if (cb_12.Checked)
		{
			num += 2048;
		}
		if (cb_13.Checked)
		{
			num += 4096;
		}
		return num;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmSetDisplayListItem));
		panelPrint = new System.Windows.Forms.Panel();
		cb_13 = new System.Windows.Forms.CheckBox();
		label3 = new System.Windows.Forms.Label();
		cb_12 = new System.Windows.Forms.CheckBox();
		cb_11 = new System.Windows.Forms.CheckBox();
		cb_10 = new System.Windows.Forms.CheckBox();
		cb_9 = new System.Windows.Forms.CheckBox();
		cb_8 = new System.Windows.Forms.CheckBox();
		cb_7 = new System.Windows.Forms.CheckBox();
		cb_6 = new System.Windows.Forms.CheckBox();
		cb_5 = new System.Windows.Forms.CheckBox();
		cb_4 = new System.Windows.Forms.CheckBox();
		cb_3 = new System.Windows.Forms.CheckBox();
		cb_2 = new System.Windows.Forms.CheckBox();
		label13 = new System.Windows.Forms.Label();
		label12 = new System.Windows.Forms.Label();
		label11 = new System.Windows.Forms.Label();
		label10 = new System.Windows.Forms.Label();
		label9 = new System.Windows.Forms.Label();
		label8 = new System.Windows.Forms.Label();
		label7 = new System.Windows.Forms.Label();
		label6 = new System.Windows.Forms.Label();
		label5 = new System.Windows.Forms.Label();
		cb_1 = new System.Windows.Forms.CheckBox();
		label4 = new System.Windows.Forms.Label();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(cb_13);
		panelPrint.Controls.Add(label3);
		panelPrint.Controls.Add(cb_12);
		panelPrint.Controls.Add(cb_11);
		panelPrint.Controls.Add(cb_10);
		panelPrint.Controls.Add(cb_9);
		panelPrint.Controls.Add(cb_8);
		panelPrint.Controls.Add(cb_7);
		panelPrint.Controls.Add(cb_6);
		panelPrint.Controls.Add(cb_5);
		panelPrint.Controls.Add(cb_4);
		panelPrint.Controls.Add(cb_3);
		panelPrint.Controls.Add(cb_2);
		panelPrint.Controls.Add(label13);
		panelPrint.Controls.Add(label12);
		panelPrint.Controls.Add(label11);
		panelPrint.Controls.Add(label10);
		panelPrint.Controls.Add(label9);
		panelPrint.Controls.Add(label8);
		panelPrint.Controls.Add(label7);
		panelPrint.Controls.Add(label6);
		panelPrint.Controls.Add(label5);
		panelPrint.Controls.Add(cb_1);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Location = new System.Drawing.Point(2, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(213, 411);
		panelPrint.TabIndex = 2;
		cb_13.AutoSize = true;
		cb_13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_13.Location = new System.Drawing.Point(142, 88);
		cb_13.Name = "cb_13";
		cb_13.Size = new System.Drawing.Size(59, 20);
		cb_13.TabIndex = 57;
		cb_13.Text = "顯示";
		cb_13.UseVisualStyleBackColor = true;
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label3.Location = new System.Drawing.Point(3, 86);
		label3.MaximumSize = new System.Drawing.Size(200, 0);
		label3.MinimumSize = new System.Drawing.Size(130, 0);
		label3.Name = "label3";
		label3.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label3.Size = new System.Drawing.Size(130, 26);
		label3.TabIndex = 56;
		label3.Text = "收貨單位";
		cb_12.AutoSize = true;
		cb_12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_12.Location = new System.Drawing.Point(142, 342);
		cb_12.Name = "cb_12";
		cb_12.Size = new System.Drawing.Size(59, 20);
		cb_12.TabIndex = 55;
		cb_12.Text = "顯示";
		cb_12.UseVisualStyleBackColor = true;
		cb_11.AutoSize = true;
		cb_11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_11.Location = new System.Drawing.Point(142, 314);
		cb_11.Name = "cb_11";
		cb_11.Size = new System.Drawing.Size(59, 20);
		cb_11.TabIndex = 54;
		cb_11.Text = "顯示";
		cb_11.UseVisualStyleBackColor = true;
		cb_10.AutoSize = true;
		cb_10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_10.Location = new System.Drawing.Point(142, 287);
		cb_10.Name = "cb_10";
		cb_10.Size = new System.Drawing.Size(59, 20);
		cb_10.TabIndex = 53;
		cb_10.Text = "顯示";
		cb_10.UseVisualStyleBackColor = true;
		cb_9.AutoSize = true;
		cb_9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_9.Location = new System.Drawing.Point(142, 260);
		cb_9.Name = "cb_9";
		cb_9.Size = new System.Drawing.Size(59, 20);
		cb_9.TabIndex = 52;
		cb_9.Text = "顯示";
		cb_9.UseVisualStyleBackColor = true;
		cb_8.AutoSize = true;
		cb_8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_8.Location = new System.Drawing.Point(142, 232);
		cb_8.Name = "cb_8";
		cb_8.Size = new System.Drawing.Size(59, 20);
		cb_8.TabIndex = 51;
		cb_8.Text = "顯示";
		cb_8.UseVisualStyleBackColor = true;
		cb_7.AutoSize = true;
		cb_7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_7.Location = new System.Drawing.Point(142, 203);
		cb_7.Name = "cb_7";
		cb_7.Size = new System.Drawing.Size(59, 20);
		cb_7.TabIndex = 50;
		cb_7.Text = "顯示";
		cb_7.UseVisualStyleBackColor = true;
		cb_6.AutoSize = true;
		cb_6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_6.Location = new System.Drawing.Point(142, 175);
		cb_6.Name = "cb_6";
		cb_6.Size = new System.Drawing.Size(59, 20);
		cb_6.TabIndex = 49;
		cb_6.Text = "顯示";
		cb_6.UseVisualStyleBackColor = true;
		cb_5.AutoSize = true;
		cb_5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_5.Location = new System.Drawing.Point(142, 147);
		cb_5.Name = "cb_5";
		cb_5.Size = new System.Drawing.Size(59, 20);
		cb_5.TabIndex = 48;
		cb_5.Text = "顯示";
		cb_5.UseVisualStyleBackColor = true;
		cb_4.AutoSize = true;
		cb_4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_4.Location = new System.Drawing.Point(142, 116);
		cb_4.Name = "cb_4";
		cb_4.Size = new System.Drawing.Size(59, 20);
		cb_4.TabIndex = 47;
		cb_4.Text = "顯示";
		cb_4.UseVisualStyleBackColor = true;
		cb_3.AutoSize = true;
		cb_3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_3.Location = new System.Drawing.Point(142, 60);
		cb_3.Name = "cb_3";
		cb_3.Size = new System.Drawing.Size(59, 20);
		cb_3.TabIndex = 46;
		cb_3.Text = "顯示";
		cb_3.UseVisualStyleBackColor = true;
		cb_2.AutoSize = true;
		cb_2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_2.Location = new System.Drawing.Point(142, 33);
		cb_2.Name = "cb_2";
		cb_2.Size = new System.Drawing.Size(59, 20);
		cb_2.TabIndex = 45;
		cb_2.Text = "顯示";
		cb_2.UseVisualStyleBackColor = true;
		label13.AutoSize = true;
		label13.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label13.Location = new System.Drawing.Point(3, 338);
		label13.MaximumSize = new System.Drawing.Size(200, 0);
		label13.MinimumSize = new System.Drawing.Size(130, 0);
		label13.Name = "label13";
		label13.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label13.Size = new System.Drawing.Size(130, 26);
		label13.TabIndex = 44;
		label13.Text = "已列印數量";
		label12.AutoSize = true;
		label12.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label12.Location = new System.Drawing.Point(3, 310);
		label12.MaximumSize = new System.Drawing.Size(200, 0);
		label12.MinimumSize = new System.Drawing.Size(130, 0);
		label12.Name = "label12";
		label12.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label12.Size = new System.Drawing.Size(130, 26);
		label12.TabIndex = 43;
		label12.Text = "可列印數量";
		label11.AutoSize = true;
		label11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label11.Location = new System.Drawing.Point(3, 282);
		label11.MaximumSize = new System.Drawing.Size(200, 0);
		label11.MinimumSize = new System.Drawing.Size(130, 0);
		label11.Name = "label11";
		label11.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label11.Size = new System.Drawing.Size(130, 26);
		label11.TabIndex = 42;
		label11.Text = "申請數量";
		label10.AutoSize = true;
		label10.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label10.Location = new System.Drawing.Point(3, 254);
		label10.MaximumSize = new System.Drawing.Size(200, 0);
		label10.MinimumSize = new System.Drawing.Size(130, 0);
		label10.Name = "label10";
		label10.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label10.Size = new System.Drawing.Size(130, 26);
		label10.TabIndex = 41;
		label10.Text = "原栽培編號";
		label9.AutoSize = true;
		label9.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label9.Location = new System.Drawing.Point(3, 226);
		label9.MaximumSize = new System.Drawing.Size(200, 0);
		label9.MinimumSize = new System.Drawing.Size(130, 0);
		label9.Name = "label9";
		label9.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label9.Size = new System.Drawing.Size(130, 26);
		label9.TabIndex = 40;
		label9.Text = "起始號碼";
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 198);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(130, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(130, 26);
		label8.TabIndex = 39;
		label8.Text = "EAN";
		label7.AutoSize = true;
		label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label7.Location = new System.Drawing.Point(3, 170);
		label7.MaximumSize = new System.Drawing.Size(200, 0);
		label7.MinimumSize = new System.Drawing.Size(130, 0);
		label7.Name = "label7";
		label7.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label7.Size = new System.Drawing.Size(130, 26);
		label7.TabIndex = 38;
		label7.Text = "追溯號碼";
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label6.Location = new System.Drawing.Point(3, 142);
		label6.MaximumSize = new System.Drawing.Size(200, 0);
		label6.MinimumSize = new System.Drawing.Size(130, 0);
		label6.Name = "label6";
		label6.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label6.Size = new System.Drawing.Size(130, 26);
		label6.TabIndex = 37;
		label6.Text = "規格";
		label5.AutoSize = true;
		label5.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label5.Location = new System.Drawing.Point(3, 114);
		label5.MaximumSize = new System.Drawing.Size(200, 0);
		label5.MinimumSize = new System.Drawing.Size(130, 0);
		label5.Name = "label5";
		label5.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label5.Size = new System.Drawing.Size(130, 26);
		label5.TabIndex = 36;
		label5.Text = "品項";
		cb_1.AutoSize = true;
		cb_1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_1.Location = new System.Drawing.Point(142, 7);
		cb_1.Name = "cb_1";
		cb_1.Size = new System.Drawing.Size(59, 20);
		cb_1.TabIndex = 35;
		cb_1.Text = "顯示";
		cb_1.UseVisualStyleBackColor = true;
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label4.Location = new System.Drawing.Point(3, 58);
		label4.MaximumSize = new System.Drawing.Size(200, 0);
		label4.MinimumSize = new System.Drawing.Size(130, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(130, 26);
		label4.TabIndex = 33;
		label4.Text = "農民";
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(109, 371);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(14, 371);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 19;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(3, 30);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(130, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(130, 26);
		label2.TabIndex = 2;
		label2.Text = "包裝日期";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(3, 2);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(130, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(130, 26);
		label1.TabIndex = 0;
		label1.Text = "上傳日期";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(217, 416);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmSetDisplayListItem";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "項目顯示設定";
		base.Load += new System.EventHandler(frmSetDisplayListItem_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
