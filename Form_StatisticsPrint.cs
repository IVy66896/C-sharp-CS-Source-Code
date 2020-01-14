using NIISOL;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_StatisticsPrint : Form
{
	public string constr = ConfigurationManager.ConnectionStrings["NIISOL.Properties.Settings.DataConnectionString"].ToString();

	public string startdate = "";

	public string enddate = "";

	public string vaccinecode = "";

	public string batchno = "";

	private IContainer components = null;

	private GroupBox groupBox1;

	private DataGridView gv_Data;

	private Label lb_Cnt;

	private Label lb_Date;

	public Form_StatisticsPrint()
	{
		InitializeComponent();
	}

	private void Form_StatisticsPrint_Load(object sender, EventArgs e)
	{
		DataTable dataTable = new DataTable();
		string str = "select VaccineCode 疫苗名稱,VaccineNo '劑別/型',VaccBatchNo 疫苗批號,AgencyCode 十碼章,count(*) 數量 from Record where LogicDel=0 AND InoculationDate>='" + startdate + "' and InoculationDate<='" + enddate + "' ";
		if (vaccinecode != "")
		{
			str = str + "and VaccineCode='" + vaccinecode + "' ";
		}
		if (batchno != "")
		{
			str = str + "and VaccBatchNo='" + batchno + "' ";
		}
		str += "group by VaccineCode,VaccineNo,VaccBatchNo,AgencyCode";
		dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, str, null, CommandOperationType.ExecuteReaderReturnDataTable);
		gv_Data.DataSource = dataTable;
		lb_Cnt.Text = "資料總筆數︰" + dataTable.Rows.Count;
		lb_Date.Text = "製表日期︰" + Utility.ToRocDateString(DateTime.Now);
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
		lb_Cnt = new System.Windows.Forms.Label();
		gv_Data = new System.Windows.Forms.DataGridView();
		lb_Date = new System.Windows.Forms.Label();
		groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)gv_Data).BeginInit();
		SuspendLayout();
		groupBox1.Controls.Add(lb_Date);
		groupBox1.Controls.Add(lb_Cnt);
		groupBox1.Controls.Add(gv_Data);
		groupBox1.Font = new System.Drawing.Font("新細明體", 10f);
		groupBox1.Location = new System.Drawing.Point(13, 13);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(895, 524);
		groupBox1.TabIndex = 0;
		groupBox1.TabStop = false;
		groupBox1.Text = "疫苗接種人次統計表";
		lb_Cnt.AutoSize = true;
		lb_Cnt.Location = new System.Drawing.Point(7, 23);
		lb_Cnt.Name = "lb_Cnt";
		lb_Cnt.Size = new System.Drawing.Size(0, 14);
		lb_Cnt.TabIndex = 1;
		gv_Data.AllowUserToAddRows = false;
		gv_Data.AllowUserToDeleteRows = false;
		gv_Data.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		gv_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		gv_Data.Location = new System.Drawing.Point(7, 45);
		gv_Data.Name = "gv_Data";
		gv_Data.ReadOnly = true;
		gv_Data.RowHeadersVisible = false;
		gv_Data.RowTemplate.Height = 24;
		gv_Data.Size = new System.Drawing.Size(882, 473);
		gv_Data.TabIndex = 0;
		lb_Date.AutoSize = true;
		lb_Date.Location = new System.Drawing.Point(690, 24);
		lb_Date.Name = "lb_Date";
		lb_Date.Size = new System.Drawing.Size(0, 14);
		lb_Date.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(920, 549);
		base.Controls.Add(groupBox1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Form_StatisticsPrint";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		base.Load += new System.EventHandler(Form_StatisticsPrint_Load);
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)gv_Data).EndInit();
		ResumeLayout(false);
	}
}
