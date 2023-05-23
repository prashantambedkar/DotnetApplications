using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    public static String pdfconfigid = "";
    public static String LocationName = "";
    public static String imgPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            HdnLocation.Value = Location;
            CompanyImg.Src = "images/" + _Logo;
            fillddlClients();
            fillGrid();
            lblMessage.Text = "";
        }
    }

    public void fillddlClients()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        DataTable dt = new DataTable();
        //select distinct LTRIM(RTRIM(column3)) as ClientsName from AssetMaster order by ClientsName asc
        //select distinct LTRIM(RTRIM(AM.column3)) as ClientsName from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where CP.UserID = @UserID and LP.UserID = @UserID order by ClientsName asc
        using (SqlCommand cmd = new SqlCommand("select distinct LTRIM(RTRIM(AM.column3)) as ClientsName,(select Count(am_.Column3) from AssetMaster as am_ where am_.column3=AM.column3) as columncount from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where CP.UserID = @UserID and LP.UserID = @UserID order by columncount desc", con))
        {
            cmd.Parameters.AddWithValue("@UserID", Session["userid"]);
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dt);
            }
        }
        ddlClients.Items.Clear();
        if (dt.Rows.Count > 0)
        {
            ddlClients.DataSource = dt;
            ddlClients.DataTextField = "ClientsName";
            ddlClients.DataValueField = "ClientsName";
            ddlClients.DataBind();
            ddlClients.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        else
        {
            ddlClients.DataSource = null;
            ddlClients.DataBind();
            ddlClients.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
    }

    public void fillGrid()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        DataSet ds = new DataSet();
        using (SqlCommand cmd = new SqlCommand("select * from tblClientData", con))
        {
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(ds);
            }
        }
        if (ds.Tables.Count > 0)
        {
            gvData.DataSource = ds.Tables[0];
            gvData.DataBind();
        }
        else
        {
            gvData.DataSource = null;
        }
    }

    [WebMethod]
    public static void Dashboard_Filtered_Location(string name)
    {
        HttpContext.Current.Session["Dashboard_Filtered_Location"] = name;

    }

    [System.Web.Services.WebMethod()]
    public static string SendParameters(string name)
    {
        return string.Format("Name: {0}", name);
    }

    [WebMethod]
    public static int LogoutCheck()
    {
        if (HttpContext.Current.Session["userid"] == null)
        {
            return 0;
        }
        return 1;
    }

    public class LocationStock
    {
        public string LocationName { get; set; }
        public double Stock { get; set; }

    }
    public class PrintVsTagged
    {
        public double? PrintStatus { get; set; }
        public double? IsTagged { get; set; }
        public double? LocationId { get; set; }
    }
    public class PrintVsTaggedCount
    {
        public double? Printed { get; set; }
        public double? Tagged { get; set; }

    }
    public class ChartData
    {
        public string StockDate { get; set; }
        public int Found { get; set; }
        public int MissMatch { get; set; }
        public int Missing { get; set; }
        public int Extra { get; set; }
    }
    public class PrintVsTaggedInfo
    {
        public string StringColumn { get; set; }
        public double dataCount { get; set; }
    }
    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        HttpContext.Current.Session["VAL"] = e.PostBackValue;
        Response.Redirect("Default.aspx");
    }






    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddClients.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        DataTable dtcount = new DataTable();
        using (SqlCommand cmd0 = new SqlCommand("select count(*) from tblClientData", con))
        {
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd0))
            {
                adp.Fill(dtcount);
            }
        }
        if (Convert.ToInt16(dtcount.Rows[0].ItemArray[0]) < 10)
        {
            if (Session["clientName"] != null)
            {
                using (SqlCommand cmd1 = new SqlCommand("update tblClientData set clientName=@clientName where clientName=@clientNamez", con))
                {
                    cmd1.Parameters.AddWithValue("@clientName", ddlClients.SelectedValue);
                    //cmd1.Parameters.AddWithValue("@priority", ddlPriority.SelectedValue);
                    cmd1.Parameters.AddWithValue("@clientNamez", Session["clientName"]);
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
                Session["clientName"] = null;
            }
            else
            {
                lblMessage.Text = "";
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select * from tblClientData where clientName=@clientName", con))
                {
                    cmd.Parameters.AddWithValue("@clientName", ddlClients.SelectedValue);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    using (SqlCommand cmd1 = new SqlCommand("update tblClientData set clientName=@clientName where clientListID=@clientListID", con))
                    {
                        cmd1.Parameters.AddWithValue("@clientName", ddlClients.SelectedValue);
                        //cmd1.Parameters.AddWithValue("@priority", ddlPriority.SelectedValue);
                        cmd1.Parameters.AddWithValue("@clientListID", dt.Rows[0]["clientListID"]);
                        con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    using (SqlCommand cmd1 = new SqlCommand("INSERT INTO tblClientData(clientName) values(@clientName)", con))
                    {
                        cmd1.Parameters.AddWithValue("@clientName", ddlClients.SelectedValue);
                        //cmd1.Parameters.AddWithValue("@priority", ddlPriority.SelectedValue);
                        // cmd1.Parameters.AddWithValue("@clientListID", dt.Rows[0]["clientListID"]);
                        con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            Response.Redirect("AddClients.aspx");
        }
        else
        {
            lblMessage.Text = "ONLY 10 CLIENTS DATA CAN BE ADDED!!";
        }
    }

    //protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "dit")
    //    {
    //        SqlConnection con = new SqlConnection();
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    //        GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
    //        int rowIndex = gvr.RowIndex;
    //        string clientName = gvData.Rows[rowIndex].Cells[2].Text;
    //        ddlClients.SelectedValue = clientName;
    //        Session["clientName"] = clientName;
    //    }
    //    if (e.CommandName == "del")
    //    {
    //        SqlConnection con = new SqlConnection();
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    //        GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
    //        int rowIndex = gvr.RowIndex;
    //        string clientName = gvData.Rows[rowIndex].Cells[2].Text;
    //        using (SqlCommand cmd = new SqlCommand("delete from tblClientData where clientName=@clientName", con))
    //        {
    //            cmd.Parameters.AddWithValue("@clientName", clientName);
    //            con.Open();
    //            cmd.ExecuteNonQuery();
    //            con.Close();
    //        }
    //        Response.Redirect("AddClients.aspx");
    //    }
    //}
    protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        fillGrid();
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "dels")
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                DataTable dt = new DataTable();
                GridDataItem item = (GridDataItem)e.Item;
                string clientName = item["clientName"].Text;
                using (SqlCommand cmd = new SqlCommand("delete from tblClientData where clientName=@clientName", con))
                {
                    cmd.Parameters.AddWithValue("@clientName", clientName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                Response.Redirect("AddClients.aspx");
            }
        }
        catch (Exception ex)
        {

        }
    }
}