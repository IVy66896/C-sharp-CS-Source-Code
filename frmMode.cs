using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class frmMode : MasterForm
{
	private IContainer components;

	private Button btn_mode_1;

	private Button btn_mode_0;

	private Label label3;

	private Label label4;

	public frmMode()
	{
		InitializeComponent();
	}

	private void btn_mode_0_Click(object sender, EventArgs e)
	{
		DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SysParam SET SystemMode = 0", null, CommandOperationType.ExecuteNonQuery);
		Program.SystemMode = 0;
		switchForm(new frmMain());
	}

	private void btn_mode_1_Click(object sender, EventArgs e)
	{
		DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_SysParam SET SystemMode = 1", null, CommandOperationType.ExecuteNonQuery);
		Program.SystemMode = 1;
		switchForm(new frmMain());
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.frmMode));
		btn_mode_1 = new System.Windows.Forms.Button();
		btn_mode_0 = new System.Windows.Forms.Button();
		label3 = new System.Windows.Forms.Label();
		label4 = new System.Windows.Forms.Label();
		SuspendLayout();
		btn_mode_1.BackColor = System.Drawing.Color.FromArgb(36, 168, 208);
		btn_mode_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_mode_1.Font = new System.Drawing.Font("微軟正黑體", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_mode_1.ForeColor = System.Drawing.Color.White;
		btn_mode_1.Location = new System.Drawing.Point(580, 172);
		btn_mode_1.Name = "btn_mode_1";
		btn_mode_1.Size = new System.Drawing.Size(258, 163);
		btn_mode_1.TabIndex = 34;
		btn_mode_1.Text = "成 品 陳 報 模 式";
		btn_mode_1.UseVisualStyleBackColor = false;
		btn_mode_1.Click += new System.EventHandler(btn_mode_1_Click);
		btn_mode_0.BackColor = System.Drawing.Color.FromArgb(255, 109, 49);
		btn_mode_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_mode_0.Font = new System.Drawing.Font("微軟正黑體", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_mode_0.ForeColor = System.Drawing.Color.White;
		btn_mode_0.Location = new System.Drawing.Point(146, 172);
		btn_mode_0.Name = "btn_mode_0";
		btn_mode_0.Size = new System.Drawing.Size(258, 163);
		btn_mode_0.TabIndex = 33;
		btn_mode_0.Text = "標 準 銷 售 模 式";
		btn_mode_0.UseVisualStyleBackColor = false;
		btn_mode_0.Click += new System.EventHandler(btn_mode_0_Click);
		label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.Location = new System.Drawing.Point(146, 356);
		label3.Margin = new System.Windows.Forms.Padding(0);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(265, 104);
		label3.TabIndex = 35;
		label3.Text = "標準銷售模式可進行商品銷售(包含農藥、補助肥料、以及其他、資材等自建商品)、退貨，客戶(會員)管理，以及商品的銷售金額、進貨、庫存等銷售重要功能權限。";
		label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.Location = new System.Drawing.Point(576, 356);
		label4.Margin = new System.Windows.Forms.Padding(0);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(271, 104);
		label4.TabIndex = 35;
		label4.Text = "成品陳報單純提供農藥銷售回報防檢局管理機制。無須設定售價等資訊，僅需進行銷售對象與商品數量、或商品退貨之處理。";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(981, 661);
		base.Controls.Add(label4);
		base.Controls.Add(label3);
		base.Controls.Add(btn_mode_1);
		base.Controls.Add(btn_mode_0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmMode";
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		base.Controls.SetChildIndex(btn_mode_0, 0);
		base.Controls.SetChildIndex(btn_mode_1, 0);
		base.Controls.SetChildIndex(label3, 0);
		base.Controls.SetChildIndex(label4, 0);
		ResumeLayout(false);
	}
}
