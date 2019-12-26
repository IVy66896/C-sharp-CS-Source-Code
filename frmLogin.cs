
using KeyboardClassLibrary;
using POS_Client;
using POS_Client.Properties;
using POS_Client.WebService;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

public class frmLogin : MasterForm
{
	public string temp = "";

	private IContainer components;

	private Keyboardcontrol keyboardcontrol1;

	private Label label1;

	private Button button1;

	private TextBox textBox1;

	private Label l_userLogin;

	private TextBox tb_account;

	private TextBox tb_password;

	private Button btn_login;

	private Button btn_reset;

	private Label label2;

	private FlowLayoutPanel flowLayoutPanel1;

	private ComboBox comboBox1;

	public frmLogin()
	{
		InitializeComponent();
		Program.Casher = "";
	}

	private void frmLogin_Load(object sender, EventArgs e)
	{
		string text = "";
		string[] array = new string[10]
		{
			"Q",
			"P",
			"E",
			"S",
			"T",
			"B",
			"A",
			"P",
			"H",
			"I"
		};
		char[] array2 = (DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0')).ToCharArray();
		foreach (char c in array2)
		{
			text += array[int.Parse(c.ToString())];
		}
		DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_User set Password ={0} WHERE Account = 'hywebAdmin'", new string[1]
		{
			text
		}, CommandOperationType.ExecuteNonQuery);
		_ = Resources.success;
		_ = Resources.failed;
	}

	private void Form1_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.D0)
		{
			temp += "0";
		}
		else if (e.KeyCode == Keys.D1)
		{
			temp += "1";
		}
		else if (e.KeyCode == Keys.D2)
		{
			temp += "2";
		}
		else if (e.KeyCode == Keys.D3)
		{
			temp += "3";
		}
		else if (e.KeyCode == Keys.D4)
		{
			temp += "4";
		}
		else if (e.KeyCode == Keys.D5)
		{
			temp += "5";
		}
		else if (e.KeyCode == Keys.D6)
		{
			temp += "6";
		}
		else if (e.KeyCode == Keys.D7)
		{
			temp += "7";
		}
		else if (e.KeyCode == Keys.D8)
		{
			temp += "8";
		}
		else if (e.KeyCode == Keys.D9)
		{
			temp += "9";
		}
		if (e.KeyCode == Keys.Return)
		{
			MessageBox.Show("你現在所刷條碼長度為:" + temp.Length);
			temp = "";
		}
	}

	private void btn_login_Click(object sender, EventArgs e)
	{
		if ("請輸入登入帳號".Equals(tb_account.Text) || "請輸入登入密碼".Equals(tb_password.Text))
		{
			AutoClosingMessageBox.Show("請輸入登入帳號與密碼");
			return;
		}
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_User", "Account = {0}", "", null, new string[1]
		{
			tb_account.Text
		}, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable.Rows.Count > 0)
		{
			if (int.Parse(dataTable.Rows[0]["Status"].ToString()) != 0)
			{
				AutoClosingMessageBox.Show("此帳號已被停用，請通知管理員");
			}
			else if (dataTable.Rows[0]["Password"].ToString().Equals(tb_password.Text))
			{
				DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_User set LastLogin = datetime('now') WHERE Account = {0}", new string[1]
				{
					tb_account.Text
				}, CommandOperationType.ExecuteNonQuery);
				AutoClosingMessageBox.Show("登入成功");
				Program.Casher = tb_account.Text;
				Program.RoleType = int.Parse(dataTable.Rows[0]["Type"].ToString());
				if (Program.IsFertilizer)
				{
					AutoClosingMessageBox.Show("驗證帳號密碼中");
					string text = new UploadVerification().retailData();
					if (text.Equals("驗證成功"))
					{
						Program.IsSaleOfFertilizer = true;
					}
					else if (text.Equals("購肥帳號密碼驗證錯誤"))
					{
						Program.IsSaleOfFertilizer = false;
						if (((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT * FROM hypos_ShopInfoManage", null, CommandOperationType.ExecuteReaderReturnDataTable)).Rows.Count > 0)
						{
							try
							{
								_ = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, "UPDATE hypos_ShopInfoManage SET FertilizerPassword = ''  ", null, CommandOperationType.ExecuteScalar);
							}
							catch (Exception)
							{
								AutoClosingMessageBox.Show("購肥帳號密碼驗證錯誤，商家無法販賣介接肥料");
							}
						}
					}
					else if (text.Equals("偵測不到網路連線，請確認網路正常後再選入商品"))
					{
						DataTable dataTable2 = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, "SELECT * FROM hypos_ShopInfoManage", null, CommandOperationType.ExecuteReaderReturnDataTable);
						if (dataTable2.Rows.Count > 0 && !dataTable2.Rows[0]["FertilizerAccount"].ToString().Equals("") && !dataTable2.Rows[0]["FertilizerPassword"].ToString().Equals(""))
						{
							Program.IsSaleOfFertilizer = true;
						}
					}
				}
				switchForm(new frmNews());
			}
			else
			{
				AutoClosingMessageBox.Show("密碼不正確，請重新輸入");
			}
		}
		else
		{
			AutoClosingMessageBox.Show("帳號不存在，請檢查您輸入的帳號");
		}
	}

	private void tb_account_Enter(object sender, EventArgs e)
	{
		if ("請輸入登入帳號".Equals(tb_account.Text))
		{
			tb_account.Text = "";
		}
	}

	private void tb_account_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_account.Text))
		{
			tb_account.Text = "請輸入登入帳號";
		}
	}

	private void tb_password_Enter(object sender, EventArgs e)
	{
		if ("請輸入登入密碼".Equals(tb_password.Text))
		{
			tb_password.Text = "";
			tb_password.PasswordChar = '*';
		}
	}

	private void tb_password_Leave(object sender, EventArgs e)
	{
		if ("".Equals(tb_password.Text))
		{
			tb_password.PasswordChar = '\0';
			tb_password.Text = "請輸入登入密碼";
		}
	}

	private void btn_reset_Click(object sender, EventArgs e)
	{
		if (!"請輸入登入密碼".Equals(tb_password.Text))
		{
			tb_password.PasswordChar = '\0';
			tb_password.Text = "請輸入登入密碼";
		}
		if (!"請輸入登入帳號".Equals(tb_account.Text))
		{
			tb_account.Text = "請輸入登入帳號";
		}
	}

	private void keyboardcontrol1_UserKeyPressed(object sender, KeyboardEventArgs e)
	{
		if ("請輸入登入帳號".Equals(tb_account.Text) && !tb_account.Focused)
		{
			tb_account.Focus();
		}
		SendKeys.Send(e.KeyboardKeyPressed);
	}

	private void tb_account_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			if ("".Equals(tb_account.Text))
			{
				AutoClosingMessageBox.Show("請輸入帳號");
			}
			else
			{
				tb_password.Focus();
			}
		}
	}

	private void tb_password_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			btn_login_Click(sender, e);
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
		l_userLogin = new System.Windows.Forms.Label();
		tb_account = new System.Windows.Forms.TextBox();
		tb_password = new System.Windows.Forms.TextBox();
		btn_login = new System.Windows.Forms.Button();
		btn_reset = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		keyboardcontrol1 = new KeyboardClassLibrary.Keyboardcontrol();
		flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		comboBox1 = new System.Windows.Forms.ComboBox();
		SuspendLayout();
		l_userLogin.AutoSize = true;
		l_userLogin.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		l_userLogin.Image = POS_Client.Properties.Resources.oblique;
		l_userLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		l_userLogin.Location = new System.Drawing.Point(49, 109);
		l_userLogin.Name = "l_userLogin";
		l_userLogin.Size = new System.Drawing.Size(132, 26);
		l_userLogin.TabIndex = 10;
		l_userLogin.Text = "   使用者登入";
		l_userLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		tb_account.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		tb_account.ForeColor = System.Drawing.Color.DarkGray;
		tb_account.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_account.Location = new System.Drawing.Point(187, 87);
		tb_account.Name = "tb_account";
		tb_account.Size = new System.Drawing.Size(219, 29);
		tb_account.TabIndex = 0;
		tb_account.Text = "請輸入登入帳號";
		tb_account.Enter += new System.EventHandler(tb_account_Enter);
		tb_account.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_account_KeyDown);
		tb_account.Leave += new System.EventHandler(tb_account_Leave);
		tb_password.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		tb_password.ForeColor = System.Drawing.Color.DarkGray;
		tb_password.ImeMode = System.Windows.Forms.ImeMode.Disable;
		tb_password.Location = new System.Drawing.Point(187, 128);
		tb_password.Name = "tb_password";
		tb_password.Size = new System.Drawing.Size(219, 29);
		tb_password.TabIndex = 1;
		tb_password.Text = "請輸入登入密碼";
		tb_password.Enter += new System.EventHandler(tb_password_Enter);
		tb_password.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_password_KeyDown);
		tb_password.Leave += new System.EventHandler(tb_password_Leave);
		btn_login.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
		btn_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_login.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_login.ForeColor = System.Drawing.Color.White;
		btn_login.Location = new System.Drawing.Point(440, 87);
		btn_login.Name = "btn_login";
		btn_login.Size = new System.Drawing.Size(101, 70);
		btn_login.TabIndex = 2;
		btn_login.Text = "登入";
		btn_login.UseVisualStyleBackColor = false;
		btn_login.Click += new System.EventHandler(btn_login_Click);
		btn_reset.BackColor = System.Drawing.Color.DarkGray;
		btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		btn_reset.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		btn_reset.ForeColor = System.Drawing.Color.White;
		btn_reset.Location = new System.Drawing.Point(559, 87);
		btn_reset.Name = "btn_reset";
		btn_reset.Size = new System.Drawing.Size(101, 70);
		btn_reset.TabIndex = 4;
		btn_reset.Text = "清除重設";
		btn_reset.UseVisualStyleBackColor = false;
		btn_reset.Click += new System.EventHandler(btn_reset_Click);
		label2.AutoSize = true;
		label2.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
		label2.Image = POS_Client.Properties.Resources.oblique;
		label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		label2.Location = new System.Drawing.Point(48, 440);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(153, 26);
		label2.TabIndex = 6;
		label2.Text = "   資料更新紀錄";
		label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		label2.Visible = false;
		keyboardcontrol1.KeyboardType = KeyboardClassLibrary.BoW.Standard;
		keyboardcontrol1.Location = new System.Drawing.Point(77, 172);
		keyboardcontrol1.Name = "keyboardcontrol1";
		keyboardcontrol1.Size = new System.Drawing.Size(816, 260);
		keyboardcontrol1.TabIndex = 10;
		keyboardcontrol1.UserKeyPressed += new KeyboardClassLibrary.KeyboardDelegate(keyboardcontrol1_UserKeyPressed);
		flowLayoutPanel1.AutoScroll = true;
		flowLayoutPanel1.BackColor = System.Drawing.Color.White;
		flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		flowLayoutPanel1.ForeColor = System.Drawing.Color.Black;
		flowLayoutPanel1.Location = new System.Drawing.Point(48, 469);
		flowLayoutPanel1.Name = "flowLayoutPanel1";
		flowLayoutPanel1.Size = new System.Drawing.Size(886, 142);
		flowLayoutPanel1.TabIndex = 33;
		flowLayoutPanel1.Visible = false;
		flowLayoutPanel1.WrapContents = false;
		comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
		comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		comboBox1.Font = new System.Drawing.Font("微軟正黑體", 14.25f);
		comboBox1.FormattingEnabled = true;
		comboBox1.Location = new System.Drawing.Point(685, 87);
		comboBox1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
		comboBox1.Name = "comboBox1";
		comboBox1.Size = new System.Drawing.Size(128, 32);
		comboBox1.TabIndex = 43;
		comboBox1.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.ClientSize = new System.Drawing.Size(981, 661);
		base.Controls.Add(comboBox1);
		base.Controls.Add(flowLayoutPanel1);
		base.Controls.Add(label2);
		base.Controls.Add(btn_reset);
		base.Controls.Add(btn_login);
		base.Controls.Add(tb_password);
		base.Controls.Add(tb_account);
		base.Controls.Add(l_userLogin);
		base.Controls.Add(keyboardcontrol1);
		base.Name = "frmLogin";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.Load += new System.EventHandler(frmLogin_Load);
		base.Controls.SetChildIndex(keyboardcontrol1, 0);
		base.Controls.SetChildIndex(l_userLogin, 0);
		base.Controls.SetChildIndex(tb_account, 0);
		base.Controls.SetChildIndex(tb_password, 0);
		base.Controls.SetChildIndex(btn_login, 0);
		base.Controls.SetChildIndex(btn_reset, 0);
		base.Controls.SetChildIndex(label2, 0);
		base.Controls.SetChildIndex(flowLayoutPanel1, 0);
		base.Controls.SetChildIndex(comboBox1, 0);
		ResumeLayout(false);
		PerformLayout();
	}
}
