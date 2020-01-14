using NIISOL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class Form_Main : Form
{
	private struct SCARD_IO_REQUEST
	{
		public int dwProtocol;

		public int cbPciLength;
	}

	public string AgencyCode = "";

	public string UserName = "";

	public DataTable dt = new DataTable();

	public DataTable dt_VaccineData = new DataTable();

	public int tmpID = 0;

	public string EditTmpID = "";

	public List<string> DelIDs;

	private IContainer components = null;

	private Label label1;

	private Button btn_ImportBatchNo;

	private Button btn_QueryRecord;

	private Button btn_Statistics;

	private GroupBox groupBox1;

	private Button btn_ReadCard;

	private DateTimePicker tb_Birthday;

	private TextBox tb_RocID;

	private Label label3;

	private TextBox tb_CaseName;

	private Label label2;

	private GroupBox groupBox2;

	private ComboBox cb_BirthSeq;

	private Label label6;

	private DateTimePicker tb_Birthday2;

	private Label label5;

	private TextBox tb_ParentRocID;

	private Label label4;

	private GroupBox groupBox3;

	private TextBox tb_Address;

	private Label label8;

	private TextBox tb_Tel;

	private Label label7;

	private GroupBox groupBox4;

	private Button btn_Submit;

	private DateTimePicker tb_InoculationDate;

	private Label label13;

	private ComboBox cb_BatchNo;

	private Label label11;

	private ComboBox cb_VaccineNo;

	private Label label10;

	private ComboBox cb_VaccineCode;

	private Label label9;

	private Button btn_Save;

	private Label label12;

	private DataGridView gv_Record;

	private Button btn_Edit;

	private Button btn_Cancel;

	private Button btn_Del;

	private ComboBox cb_Sex;

	private Label label14;

	private Button btn_LoadRecord;

	private Label label15;

	private Button button1;

	private Label label16;

	private DataGridViewTextBoxColumn Seq;

	private DataGridViewTextBoxColumn gvtb_TmpID;

	private DataGridViewTextBoxColumn gvtb_CaseName;

	private DataGridViewTextBoxColumn gvtb_RocID;

	private DataGridViewTextBoxColumn gvtb_ParentRocID;

	private DataGridViewTextBoxColumn gvtb_InoculationDate;

	private DataGridViewTextBoxColumn gvtb_VaccineCode;

	private DataGridViewTextBoxColumn gvtb_VaccineNo;

	private DataGridViewTextBoxColumn gvtb_BatchNo;

	private DataGridViewTextBoxColumn gvtb_ExportedDate;

	public Form_Main()
	{
		InitializeComponent();
		InitForm();
		InitDataTable();
		InitVaccineData();
		base.FormClosing += Form_Main_FormClosing;
	}

	private void Form_Main_Load(object sender, EventArgs e)
	{
		string sql = "select VaccineCode,VaccineNo,VaccBatchNo,VaccineCode || ' ('||ChName|| ')' ChName from VaccineData";
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count == 0)
		{
			MessageBox.Show("尚未匯入疫苗批次資料，請先進行檔案匯入！", "系統提醒", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Form_ImportBatchNo form_ImportBatchNo = new Form_ImportBatchNo();
			form_ImportBatchNo.FormClosed += ChildFormClosed;
			form_ImportBatchNo.AgencyCode = AgencyCode;
			form_ImportBatchNo.Show(this);
			Hide();
		}
	}

	private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (dt.Select("Modified = 1").Length != 0 && MessageBox.Show("個案施打紀錄尚未儲存，確定要離開嗎？", "紀錄尚未儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
		{
			e.Cancel = true;
		}
	}

	public void InitDataTable()
	{
		dt = new DataTable();
		dt.Columns.Add("Id", typeof(string));
		dt.Columns.Add("CaseName", typeof(string));
		dt.Columns.Add("RocID", typeof(string));
		dt.Columns.Add("Birthday", typeof(string));
		dt.Columns.Add("ParentRocID", typeof(string));
		dt.Columns.Add("BirthSeq", typeof(int));
		dt.Columns.Add("Tel", typeof(string));
		dt.Columns.Add("Address", typeof(string));
		dt.Columns.Add("VaccineCode", typeof(string));
		dt.Columns.Add("InoculationDate", typeof(string));
		dt.Columns.Add("VaccineNo", typeof(string));
		dt.Columns.Add("VaccBatchNo", typeof(string));
		dt.Columns.Add("IsFlu", typeof(int));
		dt.Columns.Add("AgencyCode", typeof(string));
		dt.Columns.Add("Sex", typeof(string));
		dt.Columns.Add("ExportedDate", typeof(string));
		dt.Columns.Add("Saved", typeof(int));
		dt.Columns.Add("Modified", typeof(int));
		BindGV(dt);
	}

	public void InitForm()
	{
		tb_CaseName.Text = "";
		tb_RocID.Text = "";
		tb_ParentRocID.Text = "";
		tb_Birthday.Text = DateTime.Now.ToString("yyyy/MM/dd");
		tb_Birthday2.Text = DateTime.Now.ToString("yyyy/MM/dd");
		tb_Address.Text = "";
		tb_Tel.Text = "";
		cb_BirthSeq.Text = "1";
		cb_Sex.SelectedIndex = 0;
		tb_InoculationDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
		EditTmpID = "";
		DelIDs = new List<string>();
		btn_Edit.Visible = false;
		btn_Cancel.Visible = false;
		btn_Del.Visible = false;
		btn_Submit.Visible = true;
	}

	public void InitVaccineData()
	{
		dt_VaccineData.Clear();
		string sql = "select VaccineCode,VaccineNo,VaccBatchNo,VaccineCode || ' ('||ChName|| ')' ChName from VaccineData";
		dt_VaccineData = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
		Init_cb_VaccineCode();
		Init_cb_VaccineNo();
		Init_cb_BatchNo();
	}

	private void btn_LoadRecord_Click(object sender, EventArgs e)
	{
		DelIDs = new List<string>();
		DataRow[] array = dt.Select("Saved = 1");
		DataRow[] array2 = array;
		foreach (DataRow row in array2)
		{
			dt.Rows.Remove(row);
		}
		string text = "SELECT [Id], [CaseName], [RocID], [Birthday], [ParentRocID], [BirthSeq], [Tel], [Address], [VaccineCode], [InoculationDate], [VaccineNo], [VaccBatchNo], [IsFlu], [AgencyCode], [Sex], [ExportedDate], '1' [Saved], '0' [Modified] FROM Record WHERE LogicDel=0 AND ";
		dt = (DataTable)DataBaseUtilities.DBOperation(sql: (tb_RocID.Text.Trim() != "") ? (text + "RocID = '" + tb_RocID.Text + "' AND Birthday = '" + Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday.Text)) + "'") : ((!(tb_ParentRocID.Text.Trim() != "")) ? (text + "InoculationDate = '" + Utility.ToRocDateString(Convert.ToDateTime(tb_InoculationDate.Text)) + "'") : (text + "ParentRocID = '" + tb_ParentRocID.Text + "' AND BirthSeq='" + cb_BirthSeq.Text + "' AND Birthday = '" + Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday2.Text)) + "'")), ConnectionString: Program.ConnectionString, strParameterArray: null, cmdType: CommandOperationType.ExecuteReaderReturnDataTable);
		if (dt.Rows.Count == 0)
		{
			MessageBox.Show("查無個案未刪除的接種資料", "接種資料查詢", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		BindGV(dt);
	}

	private void btn_Submit_Click(object sender, EventArgs e)
	{
		try
		{
			if (tb_RocID.Text.Trim() == "" && tb_ParentRocID.Text.Trim() == "")
			{
				MessageBox.Show("個案資料填寫不完整!", "資料錯誤", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (tb_RocID.Text != "" && !Utility.IsIdNo(tb_RocID.Text) && !Utility.IsResNo(tb_RocID.Text))
			{
				MessageBox.Show("證號輸入錯誤!", "資料錯誤", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				tb_RocID.Focus();
			}
			else if (tb_ParentRocID.Text != "" && !Utility.IsIdNo(tb_ParentRocID.Text) && !Utility.IsResNo(tb_ParentRocID.Text))
			{
				MessageBox.Show("父或母證號輸入錯誤!", "資料錯誤", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				tb_ParentRocID.Focus();
			}
			else if (cb_VaccineCode.SelectedValue.ToString() == "" || cb_VaccineNo.SelectedValue.ToString() == "" || cb_BatchNo.SelectedValue.ToString() == "")
			{
				MessageBox.Show("預防接種資料填寫不完整!", "資料錯誤", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (CheckRepeat_fuzzy() <= 0 || MessageBox.Show("本個案已有相同疫苗之接種紀錄，是否仍要建立此筆資料？", "重複施打警示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No)
			{
				DataRow dataRow = dt.NewRow();
				if (tb_CaseName.Text.Trim() != "")
				{
					dataRow["CaseName"] = tb_CaseName.Text.Trim();
				}
				if (tb_RocID.Text.Trim() != "")
				{
					dataRow["RocID"] = tb_RocID.Text.Trim();
				}
				if (tb_Birthday.Text.Trim() != "" && tb_RocID.Text.Trim() != "")
				{
					dataRow["Birthday"] = Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday.Text));
				}
				if (tb_ParentRocID.Text.Trim() != "")
				{
					dataRow["ParentRocID"] = tb_ParentRocID.Text.Trim();
				}
				if (tb_Birthday2.Text.Trim() != "" && tb_ParentRocID.Text.Trim() != "")
				{
					dataRow["Birthday"] = Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday2.Text));
				}
				dataRow["BirthSeq"] = cb_BirthSeq.Text;
				if (tb_Tel.Text.Trim() != "")
				{
					dataRow["Tel"] = tb_Tel.Text.Trim();
				}
				if (tb_Address.Text.Trim() != "")
				{
					dataRow["Address"] = tb_Address.Text.Trim();
				}
				if (cb_VaccineCode.SelectedValue.ToString() != "")
				{
					dataRow["VaccineCode"] = cb_VaccineCode.SelectedValue.ToString();
				}
				if (cb_VaccineNo.SelectedValue.ToString() != "")
				{
					dataRow["VaccineNo"] = cb_VaccineNo.SelectedValue.ToString();
				}
				if (cb_BatchNo.SelectedValue.ToString() != "")
				{
					dataRow["VaccBatchNo"] = cb_BatchNo.SelectedValue.ToString();
				}
				if (tb_InoculationDate.Text.Trim() != "")
				{
					dataRow["InoculationDate"] = Utility.ToRocDateString(Convert.ToDateTime(tb_InoculationDate.Text));
				}
				if (cb_VaccineCode.SelectedValue.ToString().ToLower().IndexOf("flu") == -1)
				{
					dataRow["IsFlu"] = 0;
				}
				else
				{
					dataRow["IsFlu"] = 1;
				}
				if (tb_RocID.Text.Trim() != "")
				{
					dataRow["Sex"] = (tb_RocID.Text.Substring(1, 1).Equals("1") ? "M" : "F");
				}
				else
				{
					dataRow["Sex"] = (cb_Sex.SelectedItem.ToString().Equals("男") ? "M" : "F");
				}
				dataRow["AgencyCode"] = AgencyCode;
				dataRow["Id"] = tmpID.ToString();
				dataRow["Saved"] = "0";
				dataRow["Modified"] = "1";
				tmpID++;
				dt.Rows.Add(dataRow);
				BindGV(dt);
				gv_Record.CurrentCell = gv_Record.Rows[dt.Rows.Count - 1].Cells[0];
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("發生錯誤：" + ex.Message, "發生錯誤", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			string str = "C:\\NIISOL\\";
			Utility.WriteToFile(str + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\tForm_Main.cs: " + ex.Message, 'A', "");
		}
	}

	private void btn_Edit_Click(object sender, EventArgs e)
	{
		DataRow[] array = dt.Select("Id = '" + EditTmpID + "'");
		array[0]["InoculationDate"] = Utility.ToRocDateString(Convert.ToDateTime(tb_InoculationDate.Text));
		array[0]["VaccineCode"] = cb_VaccineCode.SelectedValue.ToString();
		array[0]["VaccineNo"] = cb_VaccineNo.SelectedValue.ToString();
		array[0]["VaccBatchNo"] = cb_BatchNo.SelectedValue.ToString();
		array[0]["Modified"] = "1";
		BindGV(dt);
		EditTmpID = "";
		btn_Edit.Visible = false;
		btn_Cancel.Visible = false;
		btn_Del.Visible = false;
		btn_Submit.Visible = true;
		cb_VaccineCode.SelectedIndex = 0;
		cb_VaccineNo.SelectedIndex = 0;
		cb_BatchNo.SelectedIndex = 0;
		tb_InoculationDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
	}

	private void btn_Cancel_Click(object sender, EventArgs e)
	{
		EditTmpID = "";
		btn_Edit.Visible = false;
		btn_Cancel.Visible = false;
		btn_Del.Visible = false;
		btn_Submit.Visible = true;
		cb_VaccineCode.SelectedIndex = 0;
		cb_VaccineNo.SelectedIndex = 0;
		cb_BatchNo.SelectedIndex = 0;
		tb_InoculationDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
	}

	private void btn_Del_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("確定要刪除資料嗎？", "刪除資料", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
		{
			if (EditTmpID.IndexOf("tmp") == -1)
			{
				DelIDs.Add(EditTmpID);
			}
			DataRow[] array = dt.Select("Id = '" + EditTmpID + "'");
			dt.Rows.Remove(array[0]);
			BindGV(dt);
			EditTmpID = "";
			btn_Edit.Visible = false;
			btn_Cancel.Visible = false;
			btn_Del.Visible = false;
			btn_Submit.Visible = true;
			cb_VaccineCode.SelectedIndex = 0;
			cb_VaccineNo.SelectedIndex = 0;
			cb_BatchNo.SelectedIndex = 0;
			tb_InoculationDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
		}
	}

	private void cb_VaccineCode_SelectedIndexChanged(object sender, EventArgs e)
	{
		Init_cb_VaccineNo();
		Init_cb_BatchNo();
	}

	private void cb_VaccineNo_SelectedIndexChanged(object sender, EventArgs e)
	{
		Init_cb_BatchNo();
	}

	private void Init_cb_VaccineCode()
	{
		if (dt_VaccineData.Rows.Count > 0)
		{
			DataTable dataSource = dt_VaccineData.DefaultView.ToTable(true, "VaccineCode", "ChName");
			cb_VaccineCode.DisplayMember = "ChName";
			cb_VaccineCode.ValueMember = "VaccineCode";
			cb_VaccineCode.DataSource = dataSource;
		}
	}

	private void Init_cb_VaccineNo()
	{
		if (dt_VaccineData.Rows.Count > 0)
		{
			DataRow[] array = dt_VaccineData.Select("VaccineCode='" + cb_VaccineCode.SelectedValue.ToString() + "'");
			DataTable dataTable = dt_VaccineData.Clone();
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			cb_VaccineNo.DisplayMember = "VaccineNo";
			cb_VaccineNo.ValueMember = "VaccineNo";
			cb_VaccineNo.DataSource = dataTable.DefaultView.ToTable(true, "VaccineNo");
		}
	}

	private void Init_cb_BatchNo()
	{
		if (dt_VaccineData.Rows.Count > 0)
		{
			DataRow[] array = dt_VaccineData.Select("VaccineCode='" + cb_VaccineCode.SelectedValue.ToString() + "' and VaccineNo='" + cb_VaccineNo.SelectedValue.ToString() + "'");
			DataTable dataTable = dt_VaccineData.Clone();
			for (int i = 0; i < array.Length; i++)
			{
				dataTable.ImportRow(array[i]);
			}
			cb_BatchNo.DisplayMember = "VaccBatchNo";
			cb_BatchNo.ValueMember = "VaccBatchNo";
			cb_BatchNo.DataSource = dataTable.DefaultView.ToTable(true, "VaccBatchNo");
		}
	}

	private void btn_ReadCard_Click(object sender, EventArgs e)
	{
		int phContext = 0;
		int phCard = 0;
		int ActiveProtocol = 0;
		string empty = string.Empty;
		byte[] array = new byte[21]
		{
			0,
			164,
			4,
			0,
			16,
			209,
			88,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			17,
			0
		};
		byte[] array2 = new byte[7]
		{
			0,
			202,
			17,
			0,
			2,
			0,
			0
		};
		byte[] array3 = new byte[2];
		int pcbRecvLength = 2;
		byte[] array4 = new byte[59];
		int pcbRecvLength2 = 59;
		uint num = 0u;
		num = SCardEstablishContext(0u, 0, 0, ref phContext);
		if (num != 0)
		{
			MessageBox.Show(ErrCode.errMsg(num), "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		int num2 = 0;
		byte[] array5 = new byte[num2];
		List<string> list = new List<string>();
		int pcchReaders = 0;
		num = SCardListReaders(phContext, null, null, ref pcchReaders);
		if (num == 0)
		{
			byte[] array6 = new byte[pcchReaders];
			num = SCardListReaders(phContext, null, array6, ref pcchReaders);
			if (num == 0)
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				string text = aSCIIEncoding.GetString(array6);
				int num3 = 0;
				char c = '\0';
				int num4 = pcchReaders;
				while (text[0] != c)
				{
					num3 = text.IndexOf(c);
					string text2 = text.Substring(0, num3);
					list.Add(text2);
					num4 -= text2.Length + 1;
					text = text.Substring(num3 + 1, num4);
				}
			}
		}
		if (num != 0)
		{
			MessageBox.Show("請確認卡片類別，並重新插卡", "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (list.Count == 0)
		{
			MessageBox.Show("請接上讀卡機裝置", "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			uint num5 = 0u;
			foreach (string item in list)
			{
				num = SCardConnect(phContext, item, 1u, 2u, ref phCard, ref ActiveProtocol);
				if (num != 0)
				{
					num5 = num;
				}
				else
				{
					SCARD_IO_REQUEST pioSendPci = default(SCARD_IO_REQUEST);
					SCARD_IO_REQUEST pioRecvPci = default(SCARD_IO_REQUEST);
					pioSendPci.dwProtocol = (pioRecvPci.dwProtocol = ActiveProtocol);
					pioSendPci.cbPciLength = (pioRecvPci.cbPciLength = 8);
					num = SCardTransmit(phCard, ref pioSendPci, array, array.Length, ref pioRecvPci, ref array3[0], ref pcbRecvLength);
					if (num == 0)
					{
						num = SCardTransmit(phCard, ref pioSendPci, array2, array2.Length, ref pioRecvPci, ref array4[0], ref pcbRecvLength2);
						if (num != 0)
						{
							MessageBox.Show(ErrCode.errMsg(num), "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else
						{
							try
							{
								if (dt.Select("Modified = 1").Length != 0)
								{
									if (MessageBox.Show("本個案接種資料未完成儲存，是否放棄儲存繼續讀取下一位個案？", "紀錄尚未儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
									{
										return;
									}
									InitDataTable();
								}
								string text3 = Encoding.Default.GetString(array4, 0, 12).Replace("\0", "");
								double result = 0.0;
								double.TryParse(text3, out result);
								if (text3.Length != 12 || result == 0.0)
								{
									throw new Exception();
								}
								tb_CaseName.Text = Encoding.Default.GetString(array4, 12, 20).Replace("\0", "");
								tb_RocID.Text = Encoding.Default.GetString(array4, 32, 10);
								tb_Birthday.Text = Convert.ToInt32(Encoding.Default.GetString(array4, 42, 3)) + 1911 + "/" + Encoding.Default.GetString(array4, 45, 2) + "/" + Encoding.Default.GetString(array4, 47, 2);
								tb_Birthday2.Text = tb_Birthday.Text;
								cb_Sex.SelectedIndex = ((!tb_RocID.Text.Substring(1, 1).Equals("1")) ? 1 : 0);
							}
							catch (Exception ex)
							{
								MessageBox.Show("請確認卡片類別，並重新插卡", "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								string str = "C:\\NIISOL\\";
								Utility.WriteToFile(str + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t請確認卡片類別，並重新插卡,讀卡失敗" + ex.Message, 'A', "");
							}
						}
						SCardDisconnect(phCard, 0);
						break;
					}
					MessageBox.Show(ErrCode.errMsg(num), "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			if (num5 != 0)
			{
				MessageBox.Show(ErrCode.errMsg(num), "讀卡失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		SCardReleaseContext(phContext);
	}

	[DllImport("WinScard.dll")]
	private static extern uint SCardEstablishContext(uint dwScope, int nNotUsed1, int nNotUsed2, ref int phContext);

	[DllImport("WinScard.dll")]
	private static extern uint SCardReleaseContext(int phContext);

	[DllImport("WinScard.dll")]
	private static extern uint SCardConnect(int hContext, string cReaderName, uint dwShareMode, uint dwPrefProtocol, ref int phCard, ref int ActiveProtocol);

	[DllImport("WinScard.dll")]
	private static extern uint SCardDisconnect(int hCard, int Disposition);

	[DllImport("WinScard.dll")]
	private static extern uint SCardListReaders(int hContext, byte[] mszGroups, byte[] mszReaders, ref int pcchReaders);

	[DllImport("WinScard.dll")]
	private static extern uint SCardTransmit(int hCard, ref SCARD_IO_REQUEST pioSendPci, byte[] pbSendBuffer, int cbSendLength, ref SCARD_IO_REQUEST pioRecvPci, ref byte pbRecvBuffer, ref int pcbRecvLength);

	public void BindGV(DataTable dt)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("Seq", typeof(int));
		dataTable.Columns.Add("Id", typeof(string));
		dataTable.Columns.Add("CaseName", typeof(string));
		dataTable.Columns.Add("RocID", typeof(string));
		dataTable.Columns.Add("ParentRocID", typeof(string));
		dataTable.Columns.Add("InoculationDate", typeof(string));
		dataTable.Columns.Add("VaccineCode", typeof(string));
		dataTable.Columns.Add("VaccineNo", typeof(string));
		dataTable.Columns.Add("VaccBatchNo", typeof(string));
		dataTable.Columns.Add("ExportedDate", typeof(string));
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow["Seq"] = i + 1;
			dataRow["Id"] = dt.Rows[i]["Id"].ToString();
			dataRow["CaseName"] = dt.Rows[i]["CaseName"].ToString();
			dataRow["RocID"] = dt.Rows[i]["RocID"].ToString();
			dataRow["ParentRocID"] = dt.Rows[i]["ParentRocID"].ToString();
			dataRow["InoculationDate"] = dt.Rows[i]["InoculationDate"].ToString();
			dataRow["VaccineCode"] = dt.Rows[i]["VaccineCode"].ToString();
			dataRow["VaccineNo"] = dt.Rows[i]["VaccineNo"].ToString();
			dataRow["VaccBatchNo"] = dt.Rows[i]["VaccBatchNo"].ToString();
			dataRow["ExportedDate"] = dt.Rows[i]["ExportedDate"].ToString();
			dataTable.Rows.Add(dataRow);
		}
		gv_Record.DataSource = dataTable;
	}

	private void gv_Record_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
	{
		if (gv_Record.CurrentRow.Cells["gvtb_ExportedDate"].Value.ToString() != "")
		{
			MessageBox.Show("已匯出的接種紀錄無法編修", "編修資料", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (MessageBox.Show("確定要刪除資料嗎？", "刪除資料", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
		{
			string text = gv_Record.CurrentRow.Cells["gvtb_TmpID"].Value.ToString();
			if (EditTmpID.IndexOf("tmp") == -1)
			{
				DelIDs.Add(text);
			}
			DataRow[] array = dt.Select("Id = '" + text + "'");
			dt.Rows.Remove(array[0]);
		}
	}

	private void gv_Record_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
	{
		BindGV(dt);
	}

	private void gv_Record_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (gv_Record.CurrentRow.Cells["gvtb_ExportedDate"].Value.ToString() != "")
		{
			MessageBox.Show("已匯出的接種紀錄無法編修", "編修資料", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		string text = EditTmpID = gv_Record.CurrentRow.Cells["gvtb_TmpID"].Value.ToString();
		tb_InoculationDate.Text = Utility.ToDateString(gv_Record.CurrentRow.Cells["gvtb_InoculationDate"].Value.ToString());
		cb_VaccineCode.SelectedValue = gv_Record.CurrentRow.Cells["gvtb_VaccineCode"].Value.ToString();
		cb_VaccineNo.SelectedValue = gv_Record.CurrentRow.Cells["gvtb_VaccineNo"].Value.ToString();
		cb_BatchNo.SelectedValue = gv_Record.CurrentRow.Cells["gvtb_BatchNo"].Value.ToString();
		btn_Submit.Visible = false;
		btn_Edit.Visible = true;
		btn_Cancel.Visible = true;
		btn_Del.Visible = true;
	}

	private void gv_Record_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
	{
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			if (dt.Rows[i]["Modified"].ToString().Equals("1"))
			{
				gv_Record.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
			}
		}
	}

	private void btn_ImportBatchNo_Click(object sender, EventArgs e)
	{
		if (dt.Select("Modified = 1").Length == 0 || MessageBox.Show("個案施打紀錄尚未儲存，確定要離開嗎？", "紀錄尚未儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No)
		{
			Form_ImportBatchNo form_ImportBatchNo = new Form_ImportBatchNo();
			form_ImportBatchNo.FormClosed += ChildFormClosed;
			form_ImportBatchNo.AgencyCode = AgencyCode;
			form_ImportBatchNo.Show(this);
			Hide();
		}
	}

	private void btn_QueryRecord_Click(object sender, EventArgs e)
	{
		if (dt.Select("Modified = 1").Length == 0 || MessageBox.Show("個案施打紀錄尚未儲存，確定要離開嗎？", "紀錄尚未儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No)
		{
			Form_QueryRecord form_QueryRecord = new Form_QueryRecord();
			form_QueryRecord.FormClosed += ChildFormClosed;
			form_QueryRecord.Show(this);
			Hide();
		}
	}

	private void btn_Statistics_Click(object sender, EventArgs e)
	{
		if (dt.Select("Modified = 1").Length == 0 || MessageBox.Show("個案施打紀錄尚未儲存，確定要離開嗎？", "紀錄尚未儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No)
		{
			Form_Statistics form_Statistics = new Form_Statistics();
			form_Statistics.FormClosed += ChildFormClosed;
			form_Statistics.Show(this);
			Hide();
		}
	}

	private void ChildFormClosed(object sender, FormClosedEventArgs e)
	{
		Show();
		InitForm();
		InitDataTable();
		InitVaccineData();
	}

	private void btn_Save_Click(object sender, EventArgs e)
	{
		string text = string.Join(",", DelIDs.ToArray());
		bool flag = false;
		DataRow[] array = dt.Select("ExportedDate is not null");
		DataRow[] array2 = array;
		foreach (DataRow row in array2)
		{
			dt.Rows.Remove(row);
		}
		if (text != "")
		{
			string sql = "DELETE FROM RECORD WHERE ID IN (" + text + ")";
			DataBaseUtilities.DBOperation(Program.ConnectionString, sql, new string[0], CommandOperationType.ExecuteNonQuery);
		}
		for (int j = 0; j < dt.Rows.Count; j++)
		{
			DataRow dataRow = dt.Rows[j];
			string text2 = "";
			string text3 = dataRow["VaccineCode"].ToString();
			string text4 = dataRow["VaccineNo"].ToString();
			string text5 = dataRow["VaccBatchNo"].ToString();
			string text6 = dataRow["InoculationDate"].ToString();
			text2 = ((!dataRow["Saved"].ToString().Equals("1")) ? ("INSERT INTO Record (CaseName,RocID,Birthday,ParentRocID,BirthSeq,Tel,Address,VaccineCode,InoculationDate,VaccineNo,VaccBatchNo,AgencyCode,Sex,IsFlu) VALUES ('" + dataRow["CaseName"].ToString() + "','" + dataRow["RocID"].ToString() + "','" + dataRow["Birthday"].ToString() + "','" + dataRow["ParentRocID"].ToString() + "','" + dataRow["BirthSeq"].ToString() + "','" + dataRow["Tel"].ToString() + "','" + dataRow["Address"].ToString() + "','" + dataRow["VaccineCode"].ToString() + "','" + dataRow["InoculationDate"].ToString() + "','" + dataRow["VaccineNo"].ToString() + "','" + dataRow["VaccBatchNo"].ToString() + "','" + dataRow["AgencyCode"].ToString() + "','" + dataRow["Sex"].ToString() + "','" + dataRow["IsFlu"].ToString() + "')") : ("UPDATE Record SET VaccineCode='" + text3 + "',VaccineNo='" + text4 + "',VaccBatchNo='" + text5 + "',InoculationDate='" + text6 + "' WHERE Id='" + dataRow["Id"].ToString() + "'"));
			try
			{
				DataBaseUtilities.DBOperation(Program.ConnectionString, text2, new string[0], CommandOperationType.ExecuteNonQuery);
			}
			catch (Exception ex)
			{
				flag = true;
				MessageBox.Show("錯誤原因：" + ex.Message, "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				string str = "C:\\NIISOL\\";
				Utility.WriteToFile(str + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t新增個案儲存失敗: " + ex.Message, 'A', "");
			}
		}
		InitDataTable();
		InitForm();
		if (!flag)
		{
			MessageBox.Show("儲存成功!", "儲存接種資料", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void tb_RocID_TextChanged(object sender, EventArgs e)
	{
		if (tb_RocID.Text.Length > 2)
		{
			cb_Sex.SelectedIndex = ((!tb_RocID.Text.Substring(1, 1).Equals("1")) ? 1 : 0);
		}
	}

	private int CheckRepeat_fuzzy()
	{
		int num = 0;
		DataTable dataTable = new DataTable();
		string text = "SELECT * FROM Record WHERE LogicDel=0 AND VaccineCode = '" + cb_VaccineCode.SelectedValue.ToString() + "' AND ";
		dataTable = (DataTable)DataBaseUtilities.DBOperation(sql: (!(tb_RocID.Text.Trim() != "")) ? (text + "ParentRocID = '" + tb_ParentRocID.Text + "' AND BirthSeq='" + cb_BirthSeq.Text + "' AND Birthday = '" + Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday2.Text)) + "' and (ExportedDate is null or ExportedDate='" + Utility.ToRocDateString(DateTime.Now) + "')") : (text + "RocID = '" + tb_RocID.Text + "' AND Birthday = '" + Utility.ToRocDateString(Convert.ToDateTime(tb_Birthday.Text)) + "' and (ExportedDate is null or ExportedDate='" + Utility.ToRocDateString(DateTime.Now) + "')"), ConnectionString: Program.ConnectionString, strParameterArray: null, cmdType: CommandOperationType.ExecuteReaderReturnDataTable);
		DataRow[] array = dt.Select("VaccineCode='" + cb_VaccineCode.SelectedValue.ToString() + "'");
		return dataTable.Rows.Count + array.Length;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		InitDataTable();
		InitForm();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
		label1 = new System.Windows.Forms.Label();
		btn_ImportBatchNo = new System.Windows.Forms.Button();
		btn_QueryRecord = new System.Windows.Forms.Button();
		btn_Statistics = new System.Windows.Forms.Button();
		groupBox1 = new System.Windows.Forms.GroupBox();
		label16 = new System.Windows.Forms.Label();
		label12 = new System.Windows.Forms.Label();
		btn_ReadCard = new System.Windows.Forms.Button();
		tb_Birthday = new System.Windows.Forms.DateTimePicker();
		tb_RocID = new System.Windows.Forms.TextBox();
		label3 = new System.Windows.Forms.Label();
		tb_CaseName = new System.Windows.Forms.TextBox();
		label2 = new System.Windows.Forms.Label();
		groupBox2 = new System.Windows.Forms.GroupBox();
		cb_Sex = new System.Windows.Forms.ComboBox();
		label14 = new System.Windows.Forms.Label();
		cb_BirthSeq = new System.Windows.Forms.ComboBox();
		label6 = new System.Windows.Forms.Label();
		tb_Birthday2 = new System.Windows.Forms.DateTimePicker();
		label5 = new System.Windows.Forms.Label();
		tb_ParentRocID = new System.Windows.Forms.TextBox();
		label4 = new System.Windows.Forms.Label();
		groupBox3 = new System.Windows.Forms.GroupBox();
		tb_Address = new System.Windows.Forms.TextBox();
		label8 = new System.Windows.Forms.Label();
		tb_Tel = new System.Windows.Forms.TextBox();
		label7 = new System.Windows.Forms.Label();
		groupBox4 = new System.Windows.Forms.GroupBox();
		btn_LoadRecord = new System.Windows.Forms.Button();
		btn_Del = new System.Windows.Forms.Button();
		btn_Cancel = new System.Windows.Forms.Button();
		btn_Edit = new System.Windows.Forms.Button();
		btn_Submit = new System.Windows.Forms.Button();
		tb_InoculationDate = new System.Windows.Forms.DateTimePicker();
		label13 = new System.Windows.Forms.Label();
		cb_BatchNo = new System.Windows.Forms.ComboBox();
		label11 = new System.Windows.Forms.Label();
		cb_VaccineNo = new System.Windows.Forms.ComboBox();
		label10 = new System.Windows.Forms.Label();
		cb_VaccineCode = new System.Windows.Forms.ComboBox();
		label9 = new System.Windows.Forms.Label();
		btn_Save = new System.Windows.Forms.Button();
		gv_Record = new System.Windows.Forms.DataGridView();
		Seq = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_TmpID = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_CaseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_RocID = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_ParentRocID = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_InoculationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_VaccineCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_VaccineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
		gvtb_ExportedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
		label15 = new System.Windows.Forms.Label();
		button1 = new System.Windows.Forms.Button();
		groupBox1.SuspendLayout();
		groupBox2.SuspendLayout();
		groupBox3.SuspendLayout();
		groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)gv_Record).BeginInit();
		SuspendLayout();
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.Location = new System.Drawing.Point(12, 9);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(104, 16);
		label1.TabIndex = 0;
		label1.Text = "預防接種登錄";
		btn_ImportBatchNo.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_ImportBatchNo.Location = new System.Drawing.Point(490, 9);
		btn_ImportBatchNo.Name = "btn_ImportBatchNo";
		btn_ImportBatchNo.Size = new System.Drawing.Size(115, 26);
		btn_ImportBatchNo.TabIndex = 1;
		btn_ImportBatchNo.Text = "疫苗批號匯入";
		btn_ImportBatchNo.UseVisualStyleBackColor = true;
		btn_ImportBatchNo.Click += new System.EventHandler(btn_ImportBatchNo_Click);
		btn_QueryRecord.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_QueryRecord.Location = new System.Drawing.Point(611, 9);
		btn_QueryRecord.Name = "btn_QueryRecord";
		btn_QueryRecord.Size = new System.Drawing.Size(115, 26);
		btn_QueryRecord.TabIndex = 2;
		btn_QueryRecord.Text = "接種紀錄明細";
		btn_QueryRecord.UseVisualStyleBackColor = true;
		btn_QueryRecord.Click += new System.EventHandler(btn_QueryRecord_Click);
		btn_Statistics.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_Statistics.Location = new System.Drawing.Point(732, 9);
		btn_Statistics.Name = "btn_Statistics";
		btn_Statistics.Size = new System.Drawing.Size(144, 26);
		btn_Statistics.TabIndex = 3;
		btn_Statistics.Text = "疫苗接種人次統計";
		btn_Statistics.UseVisualStyleBackColor = true;
		btn_Statistics.Click += new System.EventHandler(btn_Statistics_Click);
		groupBox1.Controls.Add(label16);
		groupBox1.Controls.Add(label12);
		groupBox1.Controls.Add(btn_ReadCard);
		groupBox1.Controls.Add(tb_Birthday);
		groupBox1.Controls.Add(tb_RocID);
		groupBox1.Controls.Add(label3);
		groupBox1.Controls.Add(tb_CaseName);
		groupBox1.Controls.Add(label2);
		groupBox1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		groupBox1.Location = new System.Drawing.Point(15, 52);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new System.Drawing.Size(874, 98);
		groupBox1.TabIndex = 4;
		groupBox1.TabStop = false;
		groupBox1.Text = "一、請插入健保IC卡讀取個案資料";
		label16.AutoSize = true;
		label16.Location = new System.Drawing.Point(40, 29);
		label16.Name = "label16";
		label16.Size = new System.Drawing.Size(152, 16);
		label16.TabIndex = 33;
		label16.Text = "或自行輸入個案資料";
		label12.AutoSize = true;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.Location = new System.Drawing.Point(488, 61);
		label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		label12.Name = "label12";
		label12.Size = new System.Drawing.Size(72, 16);
		label12.TabIndex = 32;
		label12.Text = "出生日期";
		btn_ReadCard.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_ReadCard.Location = new System.Drawing.Point(253, -1);
		btn_ReadCard.Name = "btn_ReadCard";
		btn_ReadCard.Size = new System.Drawing.Size(100, 28);
		btn_ReadCard.TabIndex = 27;
		btn_ReadCard.Text = "讀取";
		btn_ReadCard.UseVisualStyleBackColor = true;
		btn_ReadCard.Click += new System.EventHandler(btn_ReadCard_Click);
		tb_Birthday.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_Birthday.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		tb_Birthday.Location = new System.Drawing.Point(568, 58);
		tb_Birthday.Name = "tb_Birthday";
		tb_Birthday.Size = new System.Drawing.Size(114, 27);
		tb_Birthday.TabIndex = 26;
		tb_RocID.ImeMode = System.Windows.Forms.ImeMode.Alpha;
		tb_RocID.Location = new System.Drawing.Point(310, 58);
		tb_RocID.Name = "tb_RocID";
		tb_RocID.Size = new System.Drawing.Size(156, 27);
		tb_RocID.TabIndex = 3;
		tb_RocID.TextChanged += new System.EventHandler(tb_RocID_TextChanged);
		label3.AutoSize = true;
		label3.Location = new System.Drawing.Point(235, 61);
		label3.Name = "label3";
		label3.Size = new System.Drawing.Size(72, 16);
		label3.TabIndex = 2;
		label3.Text = "身分證號";
		tb_CaseName.Location = new System.Drawing.Point(71, 58);
		tb_CaseName.Name = "tb_CaseName";
		tb_CaseName.Size = new System.Drawing.Size(151, 27);
		tb_CaseName.TabIndex = 1;
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(25, 61);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(40, 16);
		label2.TabIndex = 0;
		label2.Text = "姓名";
		groupBox2.Controls.Add(cb_Sex);
		groupBox2.Controls.Add(label14);
		groupBox2.Controls.Add(cb_BirthSeq);
		groupBox2.Controls.Add(label6);
		groupBox2.Controls.Add(tb_Birthday2);
		groupBox2.Controls.Add(label5);
		groupBox2.Controls.Add(tb_ParentRocID);
		groupBox2.Controls.Add(label4);
		groupBox2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		groupBox2.Location = new System.Drawing.Point(15, 156);
		groupBox2.Name = "groupBox2";
		groupBox2.Size = new System.Drawing.Size(874, 63);
		groupBox2.TabIndex = 5;
		groupBox2.TabStop = false;
		groupBox2.Text = "二、若個案沒有身分證號，請自行輸入以下欄位";
		cb_Sex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_Sex.FormattingEnabled = true;
		cb_Sex.Items.AddRange(new object[2]
		{
			"男",
			"女"
		});
		cb_Sex.Location = new System.Drawing.Point(618, 24);
		cb_Sex.Name = "cb_Sex";
		cb_Sex.Size = new System.Drawing.Size(64, 24);
		cb_Sex.TabIndex = 30;
		label14.AutoSize = true;
		label14.Location = new System.Drawing.Point(524, 29);
		label14.Name = "label14";
		label14.Size = new System.Drawing.Size(88, 16);
		label14.TabIndex = 29;
		label14.Text = "嬰幼兒性別";
		cb_BirthSeq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_BirthSeq.FormattingEnabled = true;
		cb_BirthSeq.Items.AddRange(new object[5]
		{
			"1",
			"2",
			"3",
			"4",
			"5"
		});
		cb_BirthSeq.Location = new System.Drawing.Point(779, 25);
		cb_BirthSeq.Name = "cb_BirthSeq";
		cb_BirthSeq.Size = new System.Drawing.Size(62, 24);
		cb_BirthSeq.TabIndex = 28;
		label6.AutoSize = true;
		label6.Location = new System.Drawing.Point(701, 29);
		label6.Name = "label6";
		label6.Size = new System.Drawing.Size(72, 16);
		label6.TabIndex = 6;
		label6.Text = "同胎次序";
		tb_Birthday2.CalendarFont = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_Birthday2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_Birthday2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		tb_Birthday2.Location = new System.Drawing.Point(394, 24);
		tb_Birthday2.Name = "tb_Birthday2";
		tb_Birthday2.Size = new System.Drawing.Size(114, 27);
		tb_Birthday2.TabIndex = 27;
		label5.AutoSize = true;
		label5.Location = new System.Drawing.Point(264, 29);
		label5.Name = "label5";
		label5.Size = new System.Drawing.Size(120, 16);
		label5.TabIndex = 5;
		label5.Text = "嬰幼兒出生日期";
		tb_ParentRocID.Location = new System.Drawing.Point(147, 26);
		tb_ParentRocID.Name = "tb_ParentRocID";
		tb_ParentRocID.Size = new System.Drawing.Size(111, 27);
		tb_ParentRocID.TabIndex = 4;
		label4.AutoSize = true;
		label4.Location = new System.Drawing.Point(23, 29);
		label4.Name = "label4";
		label4.Size = new System.Drawing.Size(120, 16);
		label4.TabIndex = 1;
		label4.Text = "父或母身分證號";
		groupBox3.Controls.Add(tb_Address);
		groupBox3.Controls.Add(label8);
		groupBox3.Controls.Add(tb_Tel);
		groupBox3.Controls.Add(label7);
		groupBox3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		groupBox3.Location = new System.Drawing.Point(15, 225);
		groupBox3.Name = "groupBox3";
		groupBox3.Size = new System.Drawing.Size(874, 63);
		groupBox3.TabIndex = 6;
		groupBox3.TabStop = false;
		groupBox3.Text = "三、聯絡資訊更新（非必填)";
		tb_Address.Location = new System.Drawing.Point(342, 26);
		tb_Address.Name = "tb_Address";
		tb_Address.Size = new System.Drawing.Size(519, 27);
		tb_Address.TabIndex = 7;
		label8.AutoSize = true;
		label8.Location = new System.Drawing.Point(263, 32);
		label8.Name = "label8";
		label8.Size = new System.Drawing.Size(72, 16);
		label8.TabIndex = 6;
		label8.Text = "通訊地址";
		tb_Tel.ImeMode = System.Windows.Forms.ImeMode.Alpha;
		tb_Tel.Location = new System.Drawing.Point(71, 26);
		tb_Tel.Name = "tb_Tel";
		tb_Tel.Size = new System.Drawing.Size(156, 27);
		tb_Tel.TabIndex = 5;
		label7.AutoSize = true;
		label7.Location = new System.Drawing.Point(25, 32);
		label7.Name = "label7";
		label7.Size = new System.Drawing.Size(40, 16);
		label7.TabIndex = 3;
		label7.Text = "電話";
		groupBox4.Controls.Add(btn_LoadRecord);
		groupBox4.Controls.Add(btn_Del);
		groupBox4.Controls.Add(btn_Cancel);
		groupBox4.Controls.Add(btn_Edit);
		groupBox4.Controls.Add(btn_Submit);
		groupBox4.Controls.Add(tb_InoculationDate);
		groupBox4.Controls.Add(label13);
		groupBox4.Controls.Add(cb_BatchNo);
		groupBox4.Controls.Add(label11);
		groupBox4.Controls.Add(cb_VaccineNo);
		groupBox4.Controls.Add(label10);
		groupBox4.Controls.Add(cb_VaccineCode);
		groupBox4.Controls.Add(label9);
		groupBox4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		groupBox4.Location = new System.Drawing.Point(15, 294);
		groupBox4.Name = "groupBox4";
		groupBox4.Size = new System.Drawing.Size(874, 110);
		groupBox4.TabIndex = 7;
		groupBox4.TabStop = false;
		groupBox4.Text = "四、預防接種登錄";
		btn_LoadRecord.Location = new System.Drawing.Point(142, -1);
		btn_LoadRecord.Name = "btn_LoadRecord";
		btn_LoadRecord.Size = new System.Drawing.Size(193, 28);
		btn_LoadRecord.TabIndex = 32;
		btn_LoadRecord.Text = "檢視離線版接種資料";
		btn_LoadRecord.UseVisualStyleBackColor = true;
		btn_LoadRecord.Click += new System.EventHandler(btn_LoadRecord_Click);
		btn_Del.Location = new System.Drawing.Point(796, 69);
		btn_Del.Name = "btn_Del";
		btn_Del.Size = new System.Drawing.Size(62, 33);
		btn_Del.TabIndex = 31;
		btn_Del.Text = "刪除";
		btn_Del.UseVisualStyleBackColor = true;
		btn_Del.Visible = false;
		btn_Del.Click += new System.EventHandler(btn_Del_Click);
		btn_Cancel.Location = new System.Drawing.Point(728, 69);
		btn_Cancel.Name = "btn_Cancel";
		btn_Cancel.Size = new System.Drawing.Size(62, 33);
		btn_Cancel.TabIndex = 30;
		btn_Cancel.Text = "取消";
		btn_Cancel.UseVisualStyleBackColor = true;
		btn_Cancel.Visible = false;
		btn_Cancel.Click += new System.EventHandler(btn_Cancel_Click);
		btn_Edit.Location = new System.Drawing.Point(660, 69);
		btn_Edit.Name = "btn_Edit";
		btn_Edit.Size = new System.Drawing.Size(62, 33);
		btn_Edit.TabIndex = 29;
		btn_Edit.Text = "修改";
		btn_Edit.UseVisualStyleBackColor = true;
		btn_Edit.Visible = false;
		btn_Edit.Click += new System.EventHandler(btn_Edit_Click);
		btn_Submit.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_Submit.Location = new System.Drawing.Point(376, 73);
		btn_Submit.Name = "btn_Submit";
		btn_Submit.Size = new System.Drawing.Size(116, 33);
		btn_Submit.TabIndex = 28;
		btn_Submit.Text = "建立接種資料";
		btn_Submit.UseVisualStyleBackColor = true;
		btn_Submit.Click += new System.EventHandler(btn_Submit_Click);
		tb_InoculationDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		tb_InoculationDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		tb_InoculationDate.Location = new System.Drawing.Point(749, 33);
		tb_InoculationDate.Name = "tb_InoculationDate";
		tb_InoculationDate.Size = new System.Drawing.Size(114, 27);
		tb_InoculationDate.TabIndex = 27;
		label13.AutoSize = true;
		label13.Location = new System.Drawing.Point(666, 37);
		label13.Name = "label13";
		label13.Size = new System.Drawing.Size(80, 16);
		label13.TabIndex = 10;
		label13.Text = "*接種日期";
		cb_BatchNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_BatchNo.DropDownWidth = 160;
		cb_BatchNo.FormattingEnabled = true;
		cb_BatchNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		cb_BatchNo.Location = new System.Drawing.Point(485, 33);
		cb_BatchNo.Name = "cb_BatchNo";
		cb_BatchNo.Size = new System.Drawing.Size(167, 24);
		cb_BatchNo.TabIndex = 9;
		label11.AutoSize = true;
		label11.Location = new System.Drawing.Point(402, 37);
		label11.Name = "label11";
		label11.Size = new System.Drawing.Size(80, 16);
		label11.TabIndex = 8;
		label11.Text = "*疫苗批號";
		cb_VaccineNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_VaccineNo.FormattingEnabled = true;
		cb_VaccineNo.Location = new System.Drawing.Point(299, 33);
		cb_VaccineNo.Name = "cb_VaccineNo";
		cb_VaccineNo.Size = new System.Drawing.Size(90, 24);
		cb_VaccineNo.TabIndex = 7;
		cb_VaccineNo.SelectedIndexChanged += new System.EventHandler(cb_VaccineNo_SelectedIndexChanged);
		label10.AutoSize = true;
		label10.Location = new System.Drawing.Point(229, 37);
		label10.Name = "label10";
		label10.Size = new System.Drawing.Size(68, 16);
		label10.TabIndex = 6;
		label10.Text = "*劑別/型";
		cb_VaccineCode.DisplayMember = "VaccineCode";
		cb_VaccineCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cb_VaccineCode.DropDownWidth = 500;
		cb_VaccineCode.FormattingEnabled = true;
		cb_VaccineCode.Location = new System.Drawing.Point(91, 33);
		cb_VaccineCode.Name = "cb_VaccineCode";
		cb_VaccineCode.Size = new System.Drawing.Size(136, 24);
		cb_VaccineCode.TabIndex = 5;
		cb_VaccineCode.ValueMember = "VaccineCode";
		cb_VaccineCode.SelectedIndexChanged += new System.EventHandler(cb_VaccineCode_SelectedIndexChanged);
		label9.AutoSize = true;
		label9.Location = new System.Drawing.Point(6, 37);
		label9.Name = "label9";
		label9.Size = new System.Drawing.Size(80, 16);
		label9.TabIndex = 4;
		label9.Text = "*疫苗名稱";
		btn_Save.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btn_Save.Location = new System.Drawing.Point(325, 596);
		btn_Save.Name = "btn_Save";
		btn_Save.Size = new System.Drawing.Size(250, 37);
		btn_Save.TabIndex = 29;
		btn_Save.Text = "儲存，並建立下一個案接種資料";
		btn_Save.UseVisualStyleBackColor = true;
		btn_Save.Click += new System.EventHandler(btn_Save_Click);
		gv_Record.AllowUserToAddRows = false;
		gv_Record.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Control;
		dataGridViewCellStyle.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		gv_Record.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		gv_Record.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		gv_Record.Columns.AddRange(Seq, gvtb_TmpID, gvtb_CaseName, gvtb_RocID, gvtb_ParentRocID, gvtb_InoculationDate, gvtb_VaccineCode, gvtb_VaccineNo, gvtb_BatchNo, gvtb_ExportedDate);
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		gv_Record.DefaultCellStyle = dataGridViewCellStyle2;
		gv_Record.Location = new System.Drawing.Point(15, 411);
		gv_Record.MultiSelect = false;
		gv_Record.Name = "gv_Record";
		gv_Record.ReadOnly = true;
		gv_Record.RowTemplate.Height = 24;
		gv_Record.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		gv_Record.Size = new System.Drawing.Size(874, 177);
		gv_Record.TabIndex = 30;
		gv_Record.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(gv_Record_DataBindingComplete);
		gv_Record.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(gv_Record_RowHeaderMouseDoubleClick);
		gv_Record.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(gv_Record_UserDeletedRow);
		gv_Record.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(gv_Record_UserDeletingRow);
		Seq.DataPropertyName = "Seq";
		dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		Seq.DefaultCellStyle = dataGridViewCellStyle3;
		Seq.FillWeight = 23.86793f;
		Seq.HeaderText = "序號";
		Seq.MinimumWidth = 50;
		Seq.Name = "Seq";
		Seq.ReadOnly = true;
		Seq.Resizable = System.Windows.Forms.DataGridViewTriState.False;
		gvtb_TmpID.DataPropertyName = "ID";
		gvtb_TmpID.HeaderText = "TmpID";
		gvtb_TmpID.Name = "gvtb_TmpID";
		gvtb_TmpID.ReadOnly = true;
		gvtb_TmpID.Visible = false;
		gvtb_CaseName.DataPropertyName = "CaseName";
		dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_CaseName.DefaultCellStyle = dataGridViewCellStyle4;
		gvtb_CaseName.HeaderText = "姓名";
		gvtb_CaseName.Name = "gvtb_CaseName";
		gvtb_CaseName.ReadOnly = true;
		gvtb_RocID.DataPropertyName = "RocID";
		dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_RocID.DefaultCellStyle = dataGridViewCellStyle5;
		gvtb_RocID.FillWeight = 107.8054f;
		gvtb_RocID.HeaderText = "身分證號";
		gvtb_RocID.Name = "gvtb_RocID";
		gvtb_RocID.ReadOnly = true;
		gvtb_ParentRocID.DataPropertyName = "ParentRocID";
		dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_ParentRocID.DefaultCellStyle = dataGridViewCellStyle6;
		gvtb_ParentRocID.FillWeight = 107.8054f;
		gvtb_ParentRocID.HeaderText = "父或母身分證號";
		gvtb_ParentRocID.Name = "gvtb_ParentRocID";
		gvtb_ParentRocID.ReadOnly = true;
		gvtb_InoculationDate.DataPropertyName = "InoculationDate";
		dataGridViewCellStyle7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_InoculationDate.DefaultCellStyle = dataGridViewCellStyle7;
		gvtb_InoculationDate.FillWeight = 107.8054f;
		gvtb_InoculationDate.HeaderText = "接種日期";
		gvtb_InoculationDate.Name = "gvtb_InoculationDate";
		gvtb_InoculationDate.ReadOnly = true;
		gvtb_VaccineCode.DataPropertyName = "VaccineCode";
		dataGridViewCellStyle8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_VaccineCode.DefaultCellStyle = dataGridViewCellStyle8;
		gvtb_VaccineCode.FillWeight = 107.8054f;
		gvtb_VaccineCode.HeaderText = "疫苗名稱";
		gvtb_VaccineCode.Name = "gvtb_VaccineCode";
		gvtb_VaccineCode.ReadOnly = true;
		gvtb_VaccineNo.DataPropertyName = "VaccineNo";
		dataGridViewCellStyle9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_VaccineNo.DefaultCellStyle = dataGridViewCellStyle9;
		gvtb_VaccineNo.FillWeight = 77.8054f;
		gvtb_VaccineNo.HeaderText = "劑別/型";
		gvtb_VaccineNo.Name = "gvtb_VaccineNo";
		gvtb_VaccineNo.ReadOnly = true;
		gvtb_BatchNo.DataPropertyName = "VaccBatchNo";
		dataGridViewCellStyle10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_BatchNo.DefaultCellStyle = dataGridViewCellStyle10;
		gvtb_BatchNo.FillWeight = 137.8054f;
		gvtb_BatchNo.HeaderText = "疫苗批號";
		gvtb_BatchNo.Name = "gvtb_BatchNo";
		gvtb_BatchNo.ReadOnly = true;
		gvtb_ExportedDate.DataPropertyName = "ExportedDate";
		dataGridViewCellStyle11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		gvtb_ExportedDate.DefaultCellStyle = dataGridViewCellStyle11;
		gvtb_ExportedDate.FillWeight = 107.8054f;
		gvtb_ExportedDate.HeaderText = "匯出日期";
		gvtb_ExportedDate.Name = "gvtb_ExportedDate";
		gvtb_ExportedDate.ReadOnly = true;
		label15.AutoSize = true;
		label15.Location = new System.Drawing.Point(21, 421);
		label15.Name = "label15";
		label15.Size = new System.Drawing.Size(29, 12);
		label15.TabIndex = 31;
		label15.Text = "編輯";
		button1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		button1.Location = new System.Drawing.Point(369, 9);
		button1.Name = "button1";
		button1.Size = new System.Drawing.Size(115, 26);
		button1.TabIndex = 33;
		button1.Text = "清除";
		button1.UseVisualStyleBackColor = true;
		button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(901, 644);
		base.Controls.Add(button1);
		base.Controls.Add(label15);
		base.Controls.Add(gv_Record);
		base.Controls.Add(btn_Save);
		base.Controls.Add(groupBox4);
		base.Controls.Add(groupBox3);
		base.Controls.Add(groupBox2);
		base.Controls.Add(groupBox1);
		base.Controls.Add(btn_Statistics);
		base.Controls.Add(btn_QueryRecord);
		base.Controls.Add(btn_ImportBatchNo);
		base.Controls.Add(label1);
		Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		base.Name = "Form_Main";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "離線版預防接種登錄";
		base.Load += new System.EventHandler(Form_Main_Load);
		groupBox1.ResumeLayout(false);
		groupBox1.PerformLayout();
		groupBox2.ResumeLayout(false);
		groupBox2.PerformLayout();
		groupBox3.ResumeLayout(false);
		groupBox3.PerformLayout();
		groupBox4.ResumeLayout(false);
		groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)gv_Record).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
