
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmJetExeList : Form
{
	private IContainer components;

	private Panel panelPrint;

	private ListBox ltExes;

	private Button btnRemove;

	private Button btnInsert;

	private OpenFileDialog fdJetExe;

	private Button btnConfirm;

	public frmJetExeList()
	{
		InitializeComponent();
	}

	private void UpdateLt()
	{
		ltExes.Items.Clear();
		foreach (string jetExecPath in Program.UserSettings.JetPrintSettings.JetExecPaths)
		{
			ltExes.Items.Add(jetExecPath);
		}
	}

	private void btnInsert_Click(object sender, EventArgs e)
	{
		if (fdJetExe.ShowDialog() == DialogResult.OK)
		{
			if (Program.UserSettings.JetPrintSettings.JetExecPaths.IndexOf(fdJetExe.FileName) >= 0)
			{
				MessageBox.Show("執行檔已存在，請重新選擇。");
				return;
			}
			ltExes.Items.Add(fdJetExe.FileName);
			Program.UserSettings.JetPrintSettings.JetExecPaths.Add(fdJetExe.FileName);
		}
	}

	private void btnRemove_Click(object sender, EventArgs e)
	{
		for (int num = ltExes.SelectedIndices.Count - 1; num >= 0; num--)
		{
			Program.UserSettings.JetPrintSettings.JetExecPaths.Remove(Convert.ToString(ltExes.Items[num]));
		}
		ltExes.ClearSelected();
		UpdateLt();
	}

	private void frmJetExeList_Load(object sender, EventArgs e)
	{
		UpdateLt();
	}

	private void btnConfirm_Click(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmJetExeList));
		panelPrint = new System.Windows.Forms.Panel();
		btnInsert = new System.Windows.Forms.Button();
		btnRemove = new System.Windows.Forms.Button();
		ltExes = new System.Windows.Forms.ListBox();
		fdJetExe = new System.Windows.Forms.OpenFileDialog();
		btnConfirm = new System.Windows.Forms.Button();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(btnConfirm);
		panelPrint.Controls.Add(ltExes);
		panelPrint.Controls.Add(btnRemove);
		panelPrint.Controls.Add(btnInsert);
		panelPrint.Location = new System.Drawing.Point(6, 7);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(482, 360);
		panelPrint.TabIndex = 3;
		btnInsert.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnInsert.Location = new System.Drawing.Point(159, 4);
		btnInsert.Name = "btnInsert";
		btnInsert.Size = new System.Drawing.Size(75, 29);
		btnInsert.TabIndex = 0;
		btnInsert.Text = "新增";
		btnInsert.UseVisualStyleBackColor = true;
		btnInsert.Click += new System.EventHandler(btnInsert_Click);
		btnRemove.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnRemove.Location = new System.Drawing.Point(240, 4);
		btnRemove.Name = "btnRemove";
		btnRemove.Size = new System.Drawing.Size(75, 29);
		btnRemove.TabIndex = 1;
		btnRemove.Text = "刪除";
		btnRemove.UseVisualStyleBackColor = true;
		btnRemove.Click += new System.EventHandler(btnRemove_Click);
		ltExes.FormattingEnabled = true;
		ltExes.HorizontalScrollbar = true;
		ltExes.ItemHeight = 12;
		ltExes.Location = new System.Drawing.Point(4, 39);
		ltExes.Name = "ltExes";
		ltExes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
		ltExes.Size = new System.Drawing.Size(469, 316);
		ltExes.TabIndex = 2;
		fdJetExe.Filter = "執行檔 (*.exe)|*.exe";
		fdJetExe.Title = "請選擇噴墨執行檔";
		btnConfirm.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnConfirm.Location = new System.Drawing.Point(398, 4);
		btnConfirm.Name = "btnConfirm";
		btnConfirm.Size = new System.Drawing.Size(75, 29);
		btnConfirm.TabIndex = 3;
		btnConfirm.Text = "確認";
		btnConfirm.UseVisualStyleBackColor = true;
		btnConfirm.Click += new System.EventHandler(btnConfirm_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(493, 370);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmJetExeList";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "噴墨列印執行檔增修";
		base.Load += new System.EventHandler(frmJetExeList_Load);
		panelPrint.ResumeLayout(false);
		ResumeLayout(false);
	}
}
