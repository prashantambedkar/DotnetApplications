﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownloadTHRStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=StockFile.xml");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(Session["THRStock"].ToString());
                Response.Flush();
                Response.End();
                Session["THRStock"] = "";
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }
}