using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dropdownmultiselect : System.Web.UI.Page
{
    static String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    SqlConnection conn = new SqlConnection(strConnString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateDropDownList();
        }
    }
    private void PopulateDropDownList()
    {
        //string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        string query = "SELECT * from LocationMaster";
        DataTable dt = new DataTable();
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);
            }
        }
        ddl1.DataSource = dt;
        ddl1.DataTextField = "LocationName";
        ddl1.DataValueField = "LocationId";
        ddl1.DataBind();

    }
    protected void GetSelected(object sender, EventArgs e)
    {
        string selectedCountries = hfSelected.Value;
        string countries = "";
        for (int i = 0; i < selectedCountries.Split(',').Length; i++)
        {
            countries += selectedCountries.Split(',')[i] + " \\n";
        }
        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('Selected Countries: \\n" + countries + "')", true);
        ddl1.SelectedIndex = -1;
        hfSelected.Value = "";
    }


}