using POS_Client;

public class GoodObjectWithMoney
{
	public int _index
	{
		get;
		set;
	}

	public CommodityInfo _GDSName
	{
		get;
		set;
	}

	public string _setprice
	{
		get;
		set;
	}

	public string _sellingprice
	{
		get;
		set;
	}

	public string _number
	{
		get;
		set;
	}

	public string _subtotal
	{
		get;
		set;
	}

	public string _discount
	{
		get;
		set;
	}

	public string _sum
	{
		get;
		set;
	}

	public string _barcode
	{
		get;
		set;
	}

	public string _cropId
	{
		get;
		set;
	}

	public string _pestId
	{
		get;
		set;
	}

	public string _specialPrice1
	{
		get;
		set;
	}

	public string _specialPrice2
	{
		get;
		set;
	}

	public string _openPrice
	{
		get;
		set;
	}

	public string _subsidyMoney
	{
		get;
		set;
	}

	public string _subsidyFertilizer
	{
		get;
		set;
	}

	public string _ISWS
	{
		get;
		set;
	}

	public string _CLA1NO
	{
		get;
		set;
	}

	public GoodObjectWithMoney(int index, CommodityInfo GDSName, string setprice, string sellingprice, string number, string subtotal, string discount, string sum, string barcode, string cropId, string pestId, string specialPrice1, string specialPrice2, string openPrice, string subsidyFertilizer, string subsidyMoney, string ISWS, string CLA1NO)
	{
		_index = index;
		_GDSName = GDSName;
		_setprice = setprice;
		_sellingprice = sellingprice;
		_number = number;
		_subtotal = subtotal;
		_discount = discount;
		_sum = sum;
		_barcode = barcode;
		_cropId = cropId;
		_pestId = pestId;
		_specialPrice1 = specialPrice1;
		_specialPrice2 = specialPrice2;
		_openPrice = openPrice;
		_subsidyFertilizer = subsidyFertilizer;
		_subsidyMoney = subsidyMoney;
		_ISWS = ISWS;
		_CLA1NO = CLA1NO;
	}
}
