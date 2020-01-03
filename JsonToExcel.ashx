<%@ WebHandler Language="C#" Class="JsonToExcel" %>

using System.Web;
using Newtonsoft.Json;
using System.Data;

public class JsonToExcel : IHttpHandler {

    public void ProcessRequest (HttpContext context)
    {
        //AllowHttpMethod(context.Request.HttpMethod,"GET","POST");

        //if (context.Request.HttpMethod.Equals("POST"))
        //{
        //    string json = HttpUtility.UrlDecode(context.Request.Form["json"] ?? "");
        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=ErrorWorkbook.xls"));
        //        ExcelToolT tool = new ExcelToolT();
        //        tool.RenderDataTableToExcel(dt).CopyTo(context.Response.OutputStream);
        //    }
        //}
        //else
        //{
        //    context.Response.Write("");
        //    context.Response.End();
        //}
    }

    public bool IsReusable {
        get {
            return false;
        }
    }


    protected void AllowHttpMethod(string myMethod,params string[] methods)
    {
        bool HasPower = false;

        System.Collections.Generic.List<string> list = new  System.Collections.Generic.List<string>(methods);

        for (int i = 0; i <= methods.Length - 1; i++)
        {
            if (methods[i].Trim().ToUpper().Equals(myMethod))
            {
                HasPower = true;
                break;
            }
        }


        if (HasPower == false)
        {
            throw new HttpException(404, "Not found");
            //Response.Redirect("~/html/ErrorPage/NoPower.html");
            //string myScript = "<script>alert('您無權操作此頁面');history.go(-1);</script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "DisableTop", myScript, false);
        }
    }
}
