using NIISOL;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_Export : Form
{
	public int aa = 0;

	private IContainer components = null;

	private GroupBox groupBox1;

	private Button btn_Cancel;

	private Button btn_Export;

	private Label label1;

	public Form_Export()
	{
		InitializeComponent();
	}

	public Form_Export(int a)
	{
		aa = a;
		InitializeComponent();
	}

	private void btn_Cancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btn_Export_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dataTable = new DataTable();
			DataTable dataTable2 = new DataTable();
			string str = "";
			string str2 = "";
			if (aa == 1)
			{
				str = str + "select RocID,CaseName, Sex,Birthday,BirthSeq,Address,Tel,ParentRocID,AgencyCode,InoculationDate,VaccineCode,VaccineNo,VaccBatchNo,'' as '疫苗廠商',(select  BatchType from VaccineData where VaccBatchNo=Record.VaccBatchNo order by BatchType desc LIMIT 1 ) 疫苗型別,'' AS '曾接種流感疫苗' from Record where isFlu=0 and (ExportedDate is null or ExportedDate='" + Utility.ToRocDateString(DateTime.Now) + "');";
				str2 = str2 + "select RocID,CaseName,Birthday,Address,Tel,ParentRocID,AgencyCode,InoculationDate,VaccBatchNo, (select  ChName from VaccineData where VaccBatchNo=Record.VaccBatchNo order by ChName desc limit  1),VaccineNo from Record where isFlu=1 and (ExportedDate is null or ExportedDate='" + Utility.ToRocDateString(DateTime.Now) + "');";
			}
			else
			{
				str += "select RocID,CaseName, Sex,Birthday,BirthSeq,Address,Tel,ParentRocID,AgencyCode,InoculationDate,VaccineCode,VaccineNo,VaccBatchNo,'' as '疫苗廠商',(select  BatchType from VaccineData where VaccBatchNo=Record.VaccBatchNo order by BatchType desc LIMIT 1 ) as '疫苗型別','' as '曾接種流感疫苗' from Record where isFlu=0 and ExportedDate is null;";
				str2 += "select RocID,CaseName,Birthday,Address,Tel,ParentRocID,AgencyCode,InoculationDate,VaccBatchNo, (select  ChName from VaccineData where VaccBatchNo=Record.VaccBatchNo order by ChName desc limit  1),VaccineNo from Record where isFlu=1 and ExportedDate is null;";
			}
			string str3 = "update Record set ExportedDate='" + Utility.ToRocDateString(DateTime.Now) + "' where ExportedDate is null";
			dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, str, null, CommandOperationType.ExecuteReaderReturnDataTable);
			dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, str2 + str3, null, CommandOperationType.ExecuteReaderReturnDataTable);
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.ShowDialog();
			if (folderBrowserDialog.SelectedPath == "")
			{
				MessageBox.Show("請選擇匯出檔案要儲存的資料夾");
			}
			else
			{
				string str4 = folderBrowserDialog.SelectedPath + "\\";
				string text = "預注資料" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
				GenFile(dataTable, str4 + text);
				string text2 = "流感資料" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
				GenFile_Flu(dataTable2, str4 + text2);
				MessageBox.Show(text + "、" + text2 + " 匯出成功!", "匯出成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("匯出失敗，原因：" + ex.Message, "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			string str5 = "C:\\NIISOL\\";
			Utility.WriteToFile(str5 + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tForm_Export.cs: " + ex.Message, 'A', "");
		}
	}

	private void GenFile(DataTable dt, string FileName)
	{
		StreamWriter streamWriter = new StreamWriter(FileName, append: true, Encoding.Default);
		streamWriter.WriteLine("幼兒身分證號,嬰幼兒姓名,嬰幼兒性別,幼兒出生日期,同胎次序,通訊地址,電話,父或母身分證號,接種機構,接種日期,疫苗種類,疫苗劑別,疫苗批號,疫苗廠商,疫苗型別,");
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			string text = "";
			for (int j = 0; j < dt.Columns.Count; j++)
			{
				text = text + dt.Rows[i][j].ToString() + ",";
			}
			streamWriter.WriteLine(text.TrimEnd(','));
		}
		streamWriter.Close();
	}

	private void GenFile_Flu(DataTable dt, string FileName)
	{
		StreamWriter streamWriter = new StreamWriter(FileName, append: true, Encoding.Default);
		streamWriter.WriteLine("公費流感疫苗接種紀錄批次匯入CSV檔案範本(★為必填欄位),,,,,,,,,,");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("");
		streamWriter.WriteLine("\"★本國籍請輸入身分證字號外國籍請輸入統一證號\",★,\"★請先輸入雙引號「\"\"」(必輸入)，再輸入出生日期。範例：如出生日期為民國102年1月1日，即輸入「\"\"1020101」\",,\"請先輸入雙引號「\"\"」(必輸入)，再輸入電話。範例：如電話為0900 - 000 - 000，即輸入「\"\"0900000000」\",,\"★請先輸入雙引號「\"\"」(必輸入)，再輸入醫事機構代碼。範例：如醫事機構代碼為0000000000，即輸入「\"\"0000000000」\",\"★請先輸入雙引號「\"\"」(必輸入)，再輸入接種日期。範例：如接種日期為民國106年10月1日，即輸入「\"\"1061001」\",\"★請先輸入雙引號「\"\"」(必輸入)，再依實際接種疫苗批號輸入(注意英文大小寫) 大寫則輸入大寫 ，小寫則輸入小寫。範例：如疫苗批號為164202，即輸入「\"\"164202」\",\"請依實際接種疫苗廠牌輸入：國光 / 賽諾菲 / 東洋\",請依實際接種疫苗劑型，0.25mL請輸入1，0.50mL請輸入2");
		streamWriter.WriteLine("身分證號,姓名,出生日期,通訊地址,電話,父或母身分證號,醫事機構代碼,接種日期,疫苗批號,疫苗廠商,疫苗劑別");
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			string text = "";
			for (int j = 0; j < dt.Columns.Count; j++)
			{
				if (j == 2 || j == 4 || j == 6 || j == 7 || j == 8)
				{
					text = text + "\"\"" + dt.Rows[i][j].ToString() + ",";
					continue;
				}
				switch (j)
				{
				case 9:
					text += ",";
					break;
				case 10:
					if (dt.Rows[i][10].ToString() == "0.25ML")
					{
						text += "1";
					}
					else if (dt.Rows[i][10].ToString() == "0.50ML" || dt.Rows[i][10].ToString() == "0.5ML")
					{
						text += "2";
					}
					else if (dt.Rows[i][10].ToString() == "20.00ML")
					{
						text += "3";
					}
					break;
				default:
					text = text + dt.Rows[i][j].ToString() + ",";
					break;
				}
			}
			streamWriter.WriteLine(text.TrimEnd(','));
		}
		streamWriter.Close();
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
		btn_Cancel = new System.Windows.Forms.Button();
		btn_Export = new System.Windows.Forms.Button();
		label1 = new System.Windows.Forms.Label();
		groupBox1.SuspendLayout();
		SuspendLayout();
		groupBox1.Controls.Add(btn_Cancel);
		groupBox1.Controls.Add(btn_Export);
		groupBox1.Controls.Add(label1);
		groupBox1.Font = new System.Drawing.Font("新細明體", 10f);
		groupBox1.Location = new System.Drawing.Point(12, 12);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(388, 185);
		groupBox1.TabIndex = 0;
		groupBox1.TabStop = false;
		groupBox1.Text = "匯出檔案";
		btn_Cancel.Location = new System.Drawing.Point(65, 135);
		btn_Cancel.Name = "btn_Cancel";
		btn_Cancel.Size = new System.Drawing.Size(118, 29);
		btn_Cancel.TabIndex = 2;
		btn_Cancel.Text = "取消";
		btn_Cancel.UseVisualStyleBackColor = true;
		btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
		btn_Export.Location = new System.Drawing.Point(201, 135);
		btn_Export.Name = "btn_Export";
		btn_Export.Size = new System.Drawing.Size(118, 29);
		btn_Export.TabIndex = 1;
		btn_Export.Text = "確定";
		btn_Export.UseVisualStyleBackColor = true;
		btn_Export.Click += new System.EventHandler(btn_Export_Click);
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(7, 23);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(367, 70);
		label1.TabIndex = 0;
		label1.Text = "1.匯出後請儘速確認檔案資料正確性\r\n2.匯出之紀錄若有問題請於電子檔直接修改再行匯入\r\n3.已匯出之接種紀錄將於隔日重新登入應用程式時全部清除\r\n\r\n直接匯出檔案請按“確定”";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(408, 204);
		base.Controls.Add(groupBox1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Form_Export";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		ResumeLayout(false);
	}
}
