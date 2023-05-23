using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LabelPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Image1.ImageUrl = "http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/^xa^cfa,50^fo100,100^fdHelloWorld^fs^xz";
        Image1.ImageUrl = Session["PrintPreviewUrl"].ToString();
    }
}