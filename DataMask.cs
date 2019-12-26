using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// DataMask 隱碼
/// </summary>
public class DataMask
{
	public DataMask()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    /// <summary>
    /// 身分證隱碼
    /// </summary>
    /// <param name="IdNo">A123459789</param>
    /// <returns>A12345****</returns>
    public static string IdentityNumber(string IdNo)
    {
        string MaskData = ""; int Len = ( IdNo != null ? IdNo.Length : 0) ;
        if (Len != 0)  MaskData = string.Format("{0}****", IdNo.Substring(0, (Len >= 10 ? 6 : Len)));

        return MaskData;

    }

    /// <summary>
    /// 地址隱碼
    /// </summary>
    /// <param name="Addr">台北市中正區重慶南路三段83號</param>
    /// <returns>台北市中正區重慶南******</returns>
    public static string Address(string Addr)
    {
        string MaskData = ""; int Len = (Addr != null ? Addr.Length : 0);
        if (Len >0)  MaskData = string.Format("{0}******", Addr.Substring(0, (Len > 6 ? Len-6 : 0)));

        return MaskData;

    }
    /// <summary>
    /// 生日隱碼
    /// </summary>
    /// <param name="birthdate">2015/01/01</param>
    /// <returns>104****</returns>
    public static string BirthDateTaiwan(string birthdate)
    {
        string MaskData = ""; int Len = ( birthdate != null ? birthdate.Length : 0) ;


        if (Len >= 4)
        {
            int TaiwanYear = 0; int  Year = 0;
            int.TryParse(birthdate.Substring(0, 4), out Year);

            if (Year > 0)
            {
                TaiwanYear = Year - 1911;
                MaskData = string.Format("{0}****", TaiwanYear.ToString());
            }

        }

        return MaskData;

    }
    /// <summary>
    /// 姓名隱碼
    /// </summary>
    /// <param name="Name">劉德華</param>
    /// <returns>劉*華</returns>
    public static string Name(string name)
    {
        string MaskData = ""; int Len = (name != null ? name.Length : 0);

        if (Len >0)
            MaskData = string.Format("{0}{2}{1}", name.Substring(0, 1), (Len>2? name.Substring(Len - 1, 1) :""),(Len>2?   new String('*', Len-2) :  (Len==2 ?"*":"") ));
         

        return MaskData;

    }
}
