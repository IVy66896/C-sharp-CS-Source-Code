using POS_Client;

public class GoodObject
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

	public string _number
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

	public GoodObject(int index, CommodityInfo GDSName, string number, string barcode, string cropId, string pestId)
	{
		_index = index;
		_GDSName = GDSName;
		_number = number;
		_barcode = barcode;
		_cropId = cropId;
		_pestId = pestId;
	}
