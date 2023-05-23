using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AcceessError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ModalPopupExtender3.Show();
    }

    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
}