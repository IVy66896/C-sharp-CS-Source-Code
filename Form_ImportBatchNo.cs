using NIISOL;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_ImportBatchNo : Form
{
	public string AgencyCode = "";

	private IContainer components = null;

	private GroupBox groupBox1;

	private TextBox tb_FilePath;

	private Button btn_SelectFile;

	private Label label2;

	private TextBox tb_AgencyCode;

	private Label label1;

	private OpenFileDialog openFileDialog1;

	private Button btn_Import;

	private Label label3;

	private Panel panel_result;

	private Label label4;

	private ListView lv_Imported;

	private ColumnHeader Seq;

	private ColumnHeader VaccineCode;

	private ColumnHeader VaccineNo;

	private ColumnHeader VaccBatchNo;

	private Button btn_Back;

	public Form_ImportBatchNo()
	{
		InitializeComponent();
		panel_result.Visible = false;
	}

	private void btn_SelectFile_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.ShowDialog();
		tb_FilePath.Text = openFileDialog.FileName;
	}

	private void Form_ImportBatchNo_Load(object sender, EventArgs e)
	{
		tb_AgencyCode.Text = AgencyCode;
	}

	private void btn_Import_Click(object sender, EventArgs e)
	{
		if (!File.Exists(tb_FilePath.Text))
		{
			MessageBox.Show("選擇要匯入的檔案!", "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("VaccineCode", typeof(string));
		dataTable.Columns.Add("VaccineNo", typeof(string));
		dataTable.Columns.Add("VaccBatchNo", typeof(string));
		dataTable.Columns.Add("ChName", typeof(string));
		dataTable.Columns.Add("BatchType", typeof(int));
		try
		{
			DataBaseUtilities.DBOperation(Program.ConnectionString, "DELETE FROM VaccineData WHERE Id !=''", new string[0], CommandOperationType.ExecuteNonQuery);
			string[] array = File.ReadAllLines(tb_FilePath.Text, Encoding.GetEncoding("big5"));
			for (int i = 1; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',');
				DataRow dataRow = dataTable.NewRow();
				dataRow["VaccineCode"] = array2[0];
				dataRow["ChName"] = array2[1];
				dataRow["VaccineNo"] = array2[2];
				dataRow["VaccBatchNo"] = array2[3];
				dataRow["BatchType"] = ((!array2[4].Equals("3")) ? 1 : 0);
				int num = (!array2[4].Equals("3")) ? 1 : 0;
				dataTable.Rows.Add(dataRow);
				string[,] strFieldArray = new string[5, 2]
				{
					{
						"VaccineCode",
						array2[0].ToString()
					},
					{
						"ChName",
						array2[1].ToString()
					},
					{
						"VaccineNo",
						array2[2].ToString()
					},
					{
						"VaccBatchNo",
						array2[3].ToString()
					},
					{
						"BatchType",
						num.ToString()
					}
				};
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "VaccineData", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
			}
			MessageBox.Show("匯入成功！", "匯入成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			MessageBox.Show("匯入錯誤，請確認檔案是否正確！" + ex.Message, "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			string str = "C:\\NIISOL\\";
			Utility.WriteToFile(str + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t匯入錯誤，請確認檔案是否正確！: " + ex.ToString(), 'A', "");
			return;
		}
		ReloadListView(dataTable);
	}

	private void ReloadListView(DataTable dt)
	{
		lv_Imported.Items.Clear();
		int num = 0;
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			num++;
			ListViewItem listViewItem = new ListViewItem(num.ToString());
			listViewItem.SubItems.Add(dt.Rows[i]["VaccineCode"].ToString());
			listViewItem.SubItems.Add(dt.Rows[i]["VaccineNo"].ToString());
			listViewItem.SubItems.Add(dt.Rows[i]["VaccBatchNo"].ToString());
			lv_Imported.Items.Add(listViewItem);
		}
		panel_result.Visible = true;
	}

	private void btn_Back_Click(object sender, EventArgs e)
	{
		base.Owner.Show();
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
		groupBox1 = new System.Windows.Forms.GroupBox();
		btn_Import = new System.Windows.Forms.Button();
		label3 = new System.Windows.Forms.Label();
		tb_FilePath = new System.Windows.Forms.TextBox();
		btn_SelectFile = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		tb_AgencyCode = new System.Windows.Forms.TextBox();
		label1 = new System.Windows.Forms.Label();
		openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		panel_result = new System.Windows.Forms.Panel();
		lv_Imported = new System.Windows.Forms.ListView();
		Seq = new System.Windows.Forms.ColumnHeader();
		VaccineCode = new System.Windows.Forms.ColumnHeader();
		VaccineNo = new System.Windows.Forms.ColumnHeader();
		VaccBatchNo = new System.Windows.Forms.ColumnHeader();
		label4 = new System.Windows.Forms.Label();
		btn_Back = new System.Windows.Forms.Button();
		groupBox1.SuspendLayout();
		panel_result.SuspendLayout();
		SuspendLayout();
		groupBox1.Controls.Add(btn_Import);
		groupBox1.Controls.Add(label3);
		groupBox1.Controls.Add(tb_FilePath);
		groupBox1.Controls.Add(btn_SelectFile);
		groupBox1.Controls.Add(label2);
		groupBox1.Controls.Add(tb_AgencyCode);
		groupBox1.Controls.Add(label1);
		groupBox1.Font = new System.Drawing.Font("新細明體", 10f);
		groupBox1.Location = new System.Drawing.Point(12, 45);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(705, 136);
		groupBox1.TabIndex = 0;
		groupBox1.TabStop = false;
		groupBox1.Text = "疫苗批號匯入";
		btn_Import.Location = new System.Drawing.Point(286, 102);
		btn_Import.Name = "btn_Import";
		btn_Import.Size = new System.Drawing.Size(102, 24);
		btn_Import.TabIndex = 6;
		btn_Import.Text = "匯入CSV";
		btn_Import.UseVisualStyleBackColor = true;
		btn_Import.Click += new System.EventHandler(btn_Import_Click);
		label3.AutoSize = true;
		label3.Font = new System.Drawing.Font("新細明體", 10f);
		label3.Location = new System.Drawing.Point(23, 85);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(263, 14);
		label3.TabIndex = 5;
		label3.Text = "說明︰疫苗批號匯入清單請至NIIS(Ⅱ)匯出";
		tb_FilePath.Location = new System.Drawing.Point(334, 50);
		tb_FilePath.Name = "tb_FilePath";
		tb_FilePath.Size = new System.Drawing.Size(365, 23);
		tb_FilePath.TabIndex = 4;
		btn_SelectFile.Location = new System.Drawing.Point(246, 50);
		btn_SelectFile.Name = "btn_SelectFile";
		btn_SelectFile.Size = new System.Drawing.Size(82, 23);
		btn_SelectFile.TabIndex = 3;
		btn_SelectFile.Text = "選擇檔案";
		btn_SelectFile.UseVisualStyleBackColor = true;
		btn_SelectFile.Click += new System.EventHandler(btn_SelectFile_Click);
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(135, 53);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(91, 14);
		label2.TabIndex = 2;
		label2.Text = "疫苗批號檔案";
		tb_AgencyCode.Location = new System.Drawing.Point(246, 16);
		tb_AgencyCode.Name = "tb_AgencyCode";
		tb_AgencyCode.Size = new System.Drawing.Size(167, 23);
		tb_AgencyCode.TabIndex = 1;
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(23, 23);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(189, 14);
		label1.TabIndex = 0;
		label1.Text = "接種單位十碼章醫事機構代碼";
		openFileDialog1.FileName = "openFileDialog1";
		panel_result.Controls.Add(lv_Imported);
		panel_result.Controls.Add(label4);
		panel_result.Font = new System.Drawing.Font("新細明體", 10f);
		panel_result.Location = new System.Drawing.Point(12, 187);
		panel_result.Name = "panel_result";
		panel_result.Size = new System.Drawing.Size(705, 287);
		panel_result.TabIndex = 1;
		panel_result.Visible = false;
		lv_Imported.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4]
		{
			Seq,
			VaccineCode,
			VaccineNo,
			VaccBatchNo
		});
		lv_Imported.GridLines = true;
		lv_Imported.Location = new System.Drawing.Point(15, 29);
		lv_Imported.Name = "lv_Imported";
		lv_Imported.Size = new System.Drawing.Size(684, 247);
		lv_Imported.TabIndex = 1;
		lv_Imported.UseCompatibleStateImageBehavior = false;
		lv_Imported.View = System.Windows.Forms.View.Details;
		Seq.Text = "序號";
		VaccineCode.Text = "疫苗名稱";
		VaccineCode.Width = 150;
		VaccineNo.Text = "劑別/型";
		VaccineNo.Width = 80;
		VaccBatchNo.Text = "疫苗批號";
		VaccBatchNo.Width = 306;
		label4.AutoSize = true;
		label4.Font = new System.Drawing.Font("新細明體", 10f);
		label4.Location = new System.Drawing.Point(283, 10);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(91, 14);
		label4.TabIndex = 0;
		label4.Text = "匯入批號明細";
		btn_Back.Font = new System.Drawing.Font("新細明體", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_Back.Location = new System.Drawing.Point(595, 15);
		btn_Back.Name = "btn_Back";
		btn_Back.Size = new System.Drawing.Size(106, 24);
		btn_Back.TabIndex = 2;
		btn_Back.Text = "回上一頁";
		btn_Back.UseVisualStyleBackColor = true;
		btn_Back.Click += new System.EventHandler(btn_Back_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(728, 486);
		base.Controls.Add(btn_Back);
		base.Controls.Add(panel_result);
		base.Controls.Add(groupBox1);
		base.Name = "Form_ImportBatchNo";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		base.Load += new System.EventHandler(Form_ImportBatchNo_Load);
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		panel_result.ResumeLayout(false);
		panel_result.PerformLayout();
		ResumeLayout(false);
	}
}
