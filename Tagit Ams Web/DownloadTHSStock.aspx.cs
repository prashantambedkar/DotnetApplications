using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownloadTHSStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.ClearHeaders();
            Response.AppendHeader("Content-Disposition", "attachment; filename=Stock_File.txt");
            Response.AppendHeader("Content-Length", (Session["THSStock"].ToString().Length + 2).ToString());
            Response.ContentType = "xml";
            Response.Write(Session["THSStock"].ToString());
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}