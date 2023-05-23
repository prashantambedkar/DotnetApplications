using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.DataAccess;
using Telerik.Web.UI;

public partial class adminx_usercontrols_top_menu : System.Web.UI.UserControl
{
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Asset = System.Configuration.ConfigurationManager.AppSettings["Asset"];

    public String WarrantyAMC = System.Configuration.ConfigurationManager.AppSettings["WarrantyAMC"];
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.DataBind();
        SetCurrentPage();
        //if (WarrantyAMC == "true")
        // ddWarranty.Visible = true;
        //else { }
        // ddWarranty.Visible = false;

    }

    private void SetCurrentPage()
    {
        var pageName = GetPageName();

        if (pageName.Contains("Transfer"))
        {

        }
        else
        {

        }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }


    private string GetPageName()
    {
        return Request.Url.ToString().Split('/').Last();
    }
    public void onclickbtnPage()
    {
        Response.Redirect("DocumentRequestt.aspx");
    }
}