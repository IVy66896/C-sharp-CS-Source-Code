using System;
using System.Text.RegularExpressions;
using System.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    //public int BanSingleUsed { set; get; }
    //public string  RedirectPath { set; get; }
    //public string BodyCssClass { set; get; }
    public string HeadScript = "";
    public string BodyClass = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //BodyCssClass = "";
        //BanSingleUsed = 0;
        //RedirectPath = "login.aspx";
        //base.OnInit(e);
        //Page.InitComplete += Page_InitComplete;

        BasePage basePage = (Page as BasePage);
        if (basePage != null)
        {
            HeadScript = basePage.HeadScript;
            BodyClass = basePage.BodyClass;
        }
    }

    void Page_InitComplete(object sender, EventArgs e)
    {
        //initialization complete
        //take necessary action
        //BodyCssClass = "";
        //BanSingleUsed = 1;
        //RedirectPath = "login.aspx";
    }

    protected override void Render(HtmlTextWriter writer)
    {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
        {
            base.Render(htmlwriter);
            string html = htmlwriter.InnerWriter.ToString();

            // Trim the whitespace from the 'html' variable
            html = RemoveWhitespaceFromHtml(html);
            writer.Write(html);
        }
    }

    private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
    private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);
    //This method uses the two regular expressions to remove any whitespace from a string of HTML.

    public static string RemoveWhitespaceFromHtml(string html)
    {
        html = RegexBetweenTags.Replace(html, ">");
        html = RegexLineBreaks.Replace(html, "<");

        return html.Trim();
    }
}
