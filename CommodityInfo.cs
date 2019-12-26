
using POS_Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class CommodityInfo : UserControl, INotifyPropertyChanged
{
	private frmMainShopSimple fms;

	public string barcode = "";

	public string HiddenBatchNo = "";

	public string HiddenMFGDate = "";

	public string HiddenPOSBatchNo = "";

	public string HiddenIsDeliveryOnly = "";

	private IContainer components;

	private Label l_VIPNO;

	private Label l_IDNO;

	private Label commodityName;

	private Label commodityClass;

	private Label label1;

	private Label l_hiddenGDSNO;

	public event PropertyChangedEventHandler PropertyChanged;

	public event EventHandler OnClickMember;

	public void setHiddenIsDeliveryOnly(string IsDeliveryOnly)
	{
		HiddenIsDeliveryOnly = IsDeliveryOnly;
	}

	public string getHiddenIsDeliveryOnly()
	{
		return HiddenIsDeliveryOnly;
	}

	public void setHiddenPOSBatchNo(string POSBatchNo)
	{
		HiddenPOSBatchNo = POSBatchNo;
	}

	public string getHiddenPOSBatchNo()
	{
		return HiddenPOSBatchNo;
	}

	public void setHiddenBatchNo(string BatchNo)
	{
		HiddenBatchNo = BatchNo;
	}

	public string getHiddenBatchNo()
	{
		return HiddenBatchNo;
	}

	public void setHiddenMFGDate(string MFGDate)
	{
		HiddenMFGDate = MFGDate;
	}

	public string getHiddenMFGDate()
	{
		return HiddenMFGDate;
	}

	public CommodityInfo()
	{
		InitializeComponent();
	}

	public void setfms(frmMainShopSimple fms)
	{
		this.fms = fms;
	}

	public void setHiddenGDSNO(string GDSNO)
	{
		l_hiddenGDSNO.Text = GDSNO;
	}

	public string getHiddenGDSNO()
	{
		return l_hiddenGDSNO.Text;
	}

	public void setMemberIdNo(string IdNo)
	{
		l_IDNO.Text = IdNo;
	}

	public void setMemberVipNo(string VipNo)
	{
		l_VIPNO.Text = VipNo;
	}

	public void setCommodityName(string commodityName)
	{
		this.commodityName.Text = commodityName;
	}

	public string getCommodityName()
	{
		return commodityName.Text;
	}

	public void setCommodityClass(string commodityClass)
	{
		this.commodityClass.Text = commodityClass;
	}

	public void setlabe1(string cropandpest)
	{
		label1.Text = cropandpest;
	}

	public void setWidth(int w)
	{
		base.Width = w;
	}

	public string getlabe1()
	{
		return label1.Text;
	}

	private void UC_Member_MouseEnter(object sender, EventArgs e)
	{
	}

	private void UC_Member_MouseLeave(object sender, EventArgs e)
	{
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
		l_VIPNO = new System.Windows.Forms.Label();
		l_IDNO = new System.Windows.Forms.Label();
		commodityName = new System.Windows.Forms.Label();
		commodityClass = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		l_hiddenGDSNO = new System.Windows.Forms.Label();
		SuspendLayout();
		l_VIPNO.AutoSize = true;
		l_VIPNO.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_VIPNO.ForeColor = System.Drawing.Color.FromArgb(0, 126, 25);
		l_VIPNO.Location = new System.Drawing.Point(10, 9);
		l_VIPNO.Name = "l_VIPNO";
		l_VIPNO.Size = new System.Drawing.Size(47, 16);
		l_VIPNO.TabIndex = 0;
		l_VIPNO.Text = "店內碼:";
		l_VIPNO.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		l_IDNO.AutoSize = true;
		l_IDNO.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		l_IDNO.ForeColor = System.Drawing.Color.FromArgb(0, 126, 25);
		l_IDNO.Location = new System.Drawing.Point(169, 9);
		l_IDNO.Name = "l_IDNO";
		l_IDNO.Size = new System.Drawing.Size(59, 16);
		l_IDNO.TabIndex = 1;
		l_IDNO.Text = "商品條碼:";
		l_IDNO.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		commodityName.AutoSize = true;
		commodityName.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		commodityName.Location = new System.Drawing.Point(2, 37);
		commodityName.Name = "commodityName";
		commodityName.Size = new System.Drawing.Size(69, 25);
		commodityName.TabIndex = 2;
		commodityName.Text = "label1";
		commodityName.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		commodityClass.AutoSize = true;
		commodityClass.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		commodityClass.ForeColor = System.Drawing.Color.FromArgb(89, 124, 14);
		commodityClass.Location = new System.Drawing.Point(3, 73);
		commodityClass.Name = "commodityClass";
		commodityClass.Size = new System.Drawing.Size(54, 20);
		commodityClass.TabIndex = 3;
		commodityClass.Text = "label2";
		commodityClass.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		label1.AutoSize = true;
		label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.Color.FromArgb(89, 124, 14);
		label1.Location = new System.Drawing.Point(174, 73);
		label1.Name = "label1";
		label1.Size = new System.Drawing.Size(54, 20);
		label1.TabIndex = 4;
		label1.Text = "label2";
		l_hiddenGDSNO.AutoSize = true;
		l_hiddenGDSNO.Location = new System.Drawing.Point(265, 80);
		l_hiddenGDSNO.Name = "l_hiddenGDSNO";
		l_hiddenGDSNO.Size = new System.Drawing.Size(21, 12);
		l_hiddenGDSNO.TabIndex = 5;
		l_hiddenGDSNO.Text = "{0}";
		l_hiddenGDSNO.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		BackColor = System.Drawing.Color.White;
		base.Controls.Add(l_hiddenGDSNO);
		base.Controls.Add(label1);
		base.Controls.Add(commodityClass);
		base.Controls.Add(commodityName);
		base.Controls.Add(l_IDNO);
		base.Controls.Add(l_VIPNO);
		Cursor = System.Windows.Forms.Cursors.Hand;
		MaximumSize = new System.Drawing.Size(900, 102);
		MinimumSize = new System.Drawing.Size(628, 102);
		base.Name = "CommodityInfo";
		base.Size = new System.Drawing.Size(628, 102);
		base.MouseEnter += new System.EventHandler(UC_Member_MouseEnter);
		base.MouseLeave += new System.EventHandler(UC_Member_MouseLeave);
		ResumeLayout(false);
		PerformLayout();
	}
}
