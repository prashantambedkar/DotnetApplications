using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adminMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == "" || Session["userid"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            lblUser.Text = Session["UserName"].ToString();
            string pagename = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
            if (pagename != "Asset")
            {
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;

            }
        }
        catch
        { }
    }
    protected void HtmlAnchor_Click(Object sender, EventArgs e)
    {
        Session["User_Name"] = null;
        Session["userid"] = null;
        Response.Redirect("login.aspx");
    }
}
