using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class testvalue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string locationname = Request.QueryString["locationname"];
        string type = Request.QueryString["type"];
        if (type == "ALL")
        {
            Session["locnamenewdata"] = locationname;
            Session["typenewdata"] = "";
        }
        else
        {
            Session["locnamenewdata"] = locationname;
            Session["typenewdata"] = type;
        }

        Response.Redirect("ViewDocumentRequestt.aspx");

    }
}