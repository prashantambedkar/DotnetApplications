using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownloadTHS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["THSEncode"].ToString() != "")
            {
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=Encode_File.txt");
                Response.AppendHeader("Content-Length", (Session["THSEncode"].ToString().Length + 2).ToString());
                Response.ContentType = "xml";
                Response.Write(Session["THSEncode"].ToString());
            }

            if (Session["THSTagging"].ToString() != "")
            {
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=AssetTag_File.txt");
                Response.AppendHeader("Content-Length", (Session["THSTagging"].ToString().Length + 2).ToString());
                Response.ContentType = "xml";
                Response.Write(Session["THSTagging"].ToString());
            }
         }
        catch (Exception ex)
        {
            
            throw;
        }
    }
}