using NIISOL;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_Statistics : Form
{
	public DataTable dt_VaccineData = new DataTable();

	private IContainer components = null;

	private Button btn_Back;

	private GroupBox groupBox1;

	private ComboBox cb_BatchNo;

	private ComboBox cb_VaccineCode;

	private DateTimePicker tb_EndDate;

	private Label label4;

	private Label label3;

	private Label label2;

	private DateTimePicker tb_StartDate;

	private Label label1;

	private Button btn_Query;

	private Button btn_Export;

	public Form_Statistics()
	{
		InitializeComponent();
		InitVaccineData();
	}

	public void InitVaccineData()
	{
		dt_VaccineData.Clear();
		string sql = "select VaccineCode,VaccineCode||' ('||chName||')' VaccineName,VaccBatchNo  from VaccineData";
		dt_VaccineData = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		Init_cb_VaccineCode();
		Init_cb_BatchNo();
	}

	private void cb_VaccineCode_SelectedIndexChanged(object sender, EventArgs e)
	{
		Init_cb_BatchNo();
	}

	private void Init_cb_VaccineCode()
	{
		DataTable dataTable = dt_VaccineData.DefaultView.ToTable(true, "VaccineCode", "VaccineName");
		cb_VaccineCode.DisplayMember = "VaccineName";
		cb_VaccineCode.ValueMember = "VaccineCode";
		DataRow dataRow = dataTable.NewRow();
		dataRow["VaccineName"] = "全部";
		dataRow["VaccineCode"] = "%";
		dataTable.Rows.InsertAt(dataRow, 0);
		cb_VaccineCode.DataSource = dataTable;
		cb_VaccineCode.SelectedIndex = 0;
	}

	private void Init_cb_BatchNo()
	{
		DataRow[] array = dt_VaccineData.Select("VaccineCode like '" + cb_VaccineCode.SelectedValue.ToString() + "'");
		DataTable dataTable = dt_VaccineData.Clone();
		for (int i = 0; i < array.Length; i++)
		{
			dataTable.ImportRow(array[i]);
		}
		DataRow dataRow = dataTable.NewRow();
		dataRow["VaccineName"] = "";
		dataRow["VaccineCode"] = "";
		dataRow["VaccBatchNo"] = "全部";
		dataTable.Rows.InsertAt(dataRow, 0);
		cb_BatchNo.DisplayMember = "VaccBatchNo";
		cb_BatchNo.ValueMember = "VaccBatchNo";
		cb_BatchNo.DataSource = dataTable.DefaultView.ToTable(true, "VaccBatchNo");
		cb_BatchNo.SelectedIndex = 0;
	}

	private void btn_Back_Click(object sender, EventArgs e)
	{
		base.Owner.Show();
		Close();
	}

	private void btn_Query_Click(object sender, EventArgs e)
	{
		Form_StatisticsPrint form_StatisticsPrint = new Form_StatisticsPrint();
		string text = cb_VaccineCode.SelectedValue.ToString();
		if (text == "%")
		{
			text = "";
		}
		string text2 = cb_BatchNo.SelectedValue.ToString();
		if (text2 == "全部")
		{
			text2 = "";
		}
		form_StatisticsPrint.startdate = Utility.ToRocDateString(Convert.ToDateTime(tb_StartDate.Text));
		form_StatisticsPrint.enddate = Utility.ToRocDateString(Convert.ToDateTime(tb_EndDate.Text));
		form_StatisticsPrint.vaccinecode = text;
		form_StatisticsPrint.batchno = text2;
		form_StatisticsPrint.FormClosed += Fsp_FormClosed;
		form_StatisticsPrint.Show();
		Hide();
	}

	private void Fsp_FormClosed(object sender, FormClosedEventArgs e)
	{
		Show();
	}

	private void btn_Export_Click(object sender, EventArgs e)
	{
		try
		{
			string text = Utility.ToRocDateString(Convert.ToDateTime(tb_StartDate.Text));
			string text2 = Utility.ToRocDateString(Convert.ToDateTime(tb_EndDate.Text));
			string text3 = cb_VaccineCode.SelectedValue.ToString();
			if (text3 == "%")
			{
				text3 = "";
			}
			string text4 = cb_BatchNo.SelectedValue.ToString();
			if (text4 == "全部")
			{
				text4 = "";
			}
			DataTable dataTable = new DataTable();
			string str = "select VaccineCode 疫苗名稱,VaccineNo '劑別/型',VaccBatchNo 疫苗批號,AgencyCode 十碼章,count(*) 數量 from Record where LogicDel=0 AND InoculationDate>='" + text + "' and InoculationDate<='" + text2 + "' ";
			if (text3 != "")
			{
				str = str + "and VaccineCode='" + text3 + "' ";
			}
			if (text4 != "")
			{
				str = str + "and VaccBatchNo='" + text4 + "' ";
			}
			str += "group by VaccineCode,VaccineNo,VaccBatchNo,AgencyCode";
			dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, str, null, CommandOperationType.ExecuteReaderReturnDataTable);
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.Description = "請選擇匯出檔案要儲存的資料夾";
			folderBrowserDialog.ShowDialog();
			if (folderBrowserDialog.SelectedPath == "")
			{
				MessageBox.Show("請選擇匯出檔案要儲存的資料夾");
			}
			else
			{
				string str2 = folderBrowserDialog.SelectedPath + "\\";
				string text5 = "疫苗接種人次統計表" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
				MemoryStream memoryStream = Utility.RenderDataTableToExcel(dataTable) as MemoryStream;
				FileStream fileStream = new FileStream(str2 + text5, FileMode.Create, FileAccess.Write);
				byte[] array = memoryStream.ToArray();
				fileStream.Write(array, 0, array.Length);
				fileStream.Flush();
				fileStream.Close();
				array = null;
				memoryStream = null;
				fileStream = null;
				MessageBox.Show(text5 + " 匯出成功", "匯出成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("匯出失敗，原因：" + ex.Message, "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			string str3 = "C:\\NIISOL\\";
			Utility.WriteToFile(str3 + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t匯出失敗，原因： " + ex.ToString(), 'A', "");
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
		btn_Back = new System.Windows.Forms.Button();
		groupBox1 = new System.Windows.Forms.GroupBox();
		cb_BatchNo = new System.Windows.Forms.ComboBox();
		cb_VaccineCode = new System.Windows.Forms.ComboBox();
		tb_EndDate = new System.Windows.Forms.DateTimePicker();
		label4 = new System.Windows.Forms.Label();
		label3 = new System.Windows.Forms.Label();
		label2 = new System.Windows.Forms.Label();
		tb_StartDate = new System.Windows.Forms.DateTimePicker();
		label1 = new System.Windows.Forms.Label();
		btn_Query = new System.Windows.Forms.Button();
		btn_Export = new System.Windows.Forms.Button();
		groupBox1.SuspendLayout();
		SuspendLayout();
		btn_Back.Font = new System.Drawing.Font("新細明體", 10f);
		btn_Back.Location = new System.Drawing.Point(545, 6);
		btn_Back.Name = "btn_Back";
		btn_Back.Size = new System.Drawing.Size(100, 28);
		btn_Back.TabIndex = 0;
		btn_Back.Text = "回上一頁";
		btn_Back.UseVisualStyleBackColor = true;
		btn_Back.Click += new System.EventHandler(btn_Back_Click);
		groupBox1.Controls.Add(cb_BatchNo);
		groupBox1.Controls.Add(cb_VaccineCode);
		groupBox1.Controls.Add(tb_EndDate);
		groupBox1.Controls.Add(label4);
		groupBox1.Controls.Add(label3);
		groupBox1.Controls.Add(label2);
		groupBox1.Controls.Add(tb_StartDate);
		groupBox1.Controls.Add(label1);
		groupBox1.Font = new System.Drawing.Font("新細明體", 10f);
		groupBox1.Location = new System.Drawing.Point(13, 40);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(632, 155);
		groupBox1.TabIndex = 1;
		groupBox1.TabStop = false;
		groupBox1.Text = "疫苗接種人次統計";
		cb_BatchNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_BatchNo.FormattingEnabled = true;
		cb_BatchNo.Location = new System.Drawing.Point(96, 109);
		cb_BatchNo.Name = "cb_BatchNo";
		cb_BatchNo.Size = new System.Drawing.Size(530, 21);
		cb_BatchNo.TabIndex = 7;
		cb_VaccineCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_VaccineCode.FormattingEnabled = true;
		cb_VaccineCode.Location = new System.Drawing.Point(96, 71);
		cb_VaccineCode.Name = "cb_VaccineCode";
		cb_VaccineCode.Size = new System.Drawing.Size(530, 21);
		cb_VaccineCode.TabIndex = 6;
		cb_VaccineCode.SelectedIndexChanged += new System.EventHandler(cb_VaccineCode_SelectedIndexChanged);
		tb_EndDate.Location = new System.Drawing.Point(240, 26);
		tb_EndDate.Name = "tb_EndDate";
		tb_EndDate.Size = new System.Drawing.Size(125, 23);
		tb_EndDate.TabIndex = 5;
		label4.AutoSize = true;
		label4.Location = new System.Drawing.Point(20, 116);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(70, 14);
		label4.TabIndex = 4;
		label4.Text = "*疫苗批號";
		label3.AutoSize = true;
		label3.Location = new System.Drawing.Point(20, 74);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(70, 14);
		label3.TabIndex = 3;
		label3.Text = "*疫苗名稱";
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(220, 33);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(14, 14);
		label2.TabIndex = 2;
		label2.Text = "~";
		tb_StartDate.Location = new System.Drawing.Point(89, 26);
		tb_StartDate.Name = "tb_StartDate";
		tb_StartDate.Size = new System.Drawing.Size(125, 23);
		tb_StartDate.TabIndex = 1;
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(20, 33);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(63, 14);
		label1.TabIndex = 0;
		label1.Text = "接種日期";
		btn_Query.Font = new System.Drawing.Font("新細明體", 10f);
		btn_Query.Location = new System.Drawing.Point(180, 201);
		btn_Query.Name = "btn_Query";
		btn_Query.Size = new System.Drawing.Size(125, 30);
		btn_Query.TabIndex = 2;
		btn_Query.Text = "預覽";
		btn_Query.UseVisualStyleBackColor = true;
		btn_Query.Click += new System.EventHandler(btn_Query_Click);
		btn_Export.Font = new System.Drawing.Font("新細明體", 10f);
		btn_Export.Location = new System.Drawing.Point(344, 201);
		btn_Export.Name = "btn_Export";
		btn_Export.Size = new System.Drawing.Size(125, 30);
		btn_Export.TabIndex = 3;
		btn_Export.Text = "匯出Excel";
		btn_Export.UseVisualStyleBackColor = true;
		btn_Export.Click += new System.EventHandler(btn_Export_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(657, 246);
		base.Controls.Add(btn_Export);
		base.Controls.Add(btn_Query);
		base.Controls.Add(groupBox1);
		base.Controls.Add(btn_Back);
		base.Name = "Form_Statistics";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		ResumeLayout(false);
	}
}
