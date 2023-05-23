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
using System.Web.Services;
using System.IO;
using Serco;
using System.Drawing;

public partial class UpdateAssets : System.Web.UI.Page
{
    public static DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bincategory();
            ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
            bindlocation();
            ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
            BindDepartment();
            bind_Field();
            gvData.Visible = false;            

            ////grid_view();
        }
    }

    private void bind_Field()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetMappingListsForUpdate");

        cboField.DataSource = ds;
        cboField.DataTextField = "MappingColumnName";
        cboField.DataValueField = "ColumnName";
        cboField.DataBind();
        cboField.Items.Insert(0, new ListItem("--Select Field--", "0", true));

    }


    public DataTable dt_search
    {
        get
        {
            return ViewState["dt_search"] as DataTable;
        }
        set
        {
            ViewState["dt_search"] = value;

        }
    }
    private void Bincategory()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

        ddlproCategory.DataSource = ds;
        ddlproCategory.DataTextField = "CategoryName";
        ddlproCategory.DataValueField = "CategoryId";
        ddlproCategory.DataBind();
        ddlproCategory.Items.Insert(0, new ListItem("--Select Category--", "0", true));

    }
    private void bindlocation()
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");

        ddlloc.DataSource = ds;
        ddlloc.DataTextField = "LocationName";
        ddlloc.DataValueField = "LocationId";
        ddlloc.DataBind();
        ddlloc.Items.Insert(0, new ListItem("--Select Location--", "0", true));

    }
    private void BindDepartment()
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

        ddldept.DataSource = ds;
        ddldept.DataTextField = "DepartmentName";
        ddldept.DataValueField = "DepartmentId";
        ddldept.DataBind();
        ddldept.Items.Insert(0, new ListItem("--Select Department--", "0", true));

    }
    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();



        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getcategoryinsubcatasset, new SqlParameter[] { 
                      new SqlParameter("@CategoryId",  ddlproCategory.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        ddlsubcat.DataSource = ds;
        ddlsubcat.DataTextField = "SubCatName";
        ddlsubcat.DataValueField = "SubCatId";
        ddlsubcat.DataBind();
        ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));

    }

    protected void OnSelectedIndexChangedLocation(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.Getbuilding, new SqlParameter[] { 
                      new SqlParameter("@LocationId",  ddlloc.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        ddlbuild.DataSource = ds;
        ddlbuild.DataTextField = "BuildingName";
        ddlbuild.DataValueField = "BuildingId";
        ddlbuild.DataBind();
        ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));

        ddlfloor.Items.Clear();
        ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

    }

    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();

        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getfloorforasset, new SqlParameter[] { 
                      new SqlParameter("@BuildingId",  ddlbuild.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        ddlfloor.DataSource = ds;
        ddlfloor.DataTextField = "FloorName";
        ddlfloor.DataValueField = "FloorId";
        ddlfloor.DataBind();
        ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

    }
    public string StrSort;
    private void grid_view()
    {
        try
        {
            //string Asset = (Convert.ToString(this.AssetId) == "0") ? null : Convert.ToString(this.AssetId);
            string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
            string SubCatId = null;
            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
            string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;

            PrintBL objBL = new PrintBL();
            //DataSet ds = objBL.GetAssetDetailsForEncode(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId);
            //DataSet ds = objBL.GetAssetDetailsForEncode(CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId);


            DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetAssetsAccordingToDateandType, new SqlParameter[] { 
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId",  SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                          new SqlParameter("@FromDate",  null),
                          new SqlParameter("@Todate",  null),
                           new SqlParameter("@AssetCode",  AssetCode),
                            });
            DataSet ds = help.ExecuteDataset();


            if (ds.Tables[0].Rows.Count > 0)
            {
                dt_search = new DataTable();
                dt_search = ds.Tables[0];

                gvData.DataSource = ds;
                gvData.Visible = true;
            }
            else
            {
                //gridlist.DataSource = null;
                //gridlist.DataBind();

                gvData.DataSource = ds;
                gvData.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }
    }
    public void btnreset_Click(object sender, EventArgs e)
    {
        txtAssetCode.Text = "";
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        ddlsubcat.Items.Clear();
        ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
        ddlbuild.Items.Clear();
        ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
        ddlfloor.Items.Clear();
        ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
        grid_view();
        gvData.DataBind();
    }


    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        grid_view();
        gvData.DataBind();

    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{ }
    private void PrepareForExport(Control ctrl)
    {
        //iterate through all the grid controls
        foreach (Control childControl in ctrl.Controls)
        {
            //if the control type is link button, remove it
            //from the collection
            if (childControl.GetType() == typeof(LinkButton))
            {
                ctrl.Controls.Remove(childControl);
            }

            //if the child control is not empty, repeat the process
            // for all its controls
            else if (childControl.HasControls())
            {
                PrepareForExport(childControl);
            }
        }
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        grid_view();
    }

    protected void cboField_SelectedIndexChanged(object sender, EventArgs e)
    {

        string value = cboField.SelectedValue;
        txtValue.Text = "";

        if (value == "AssignDate" || value == "DeliveryDate")
        {
            txtValue.CssClass = "date";   // class date
            txtValue.ReadOnly = true;
        }
        else
        {
            txtValue.CssClass = "";
            txtValue.ReadOnly = false;
        }
        if (value == "DepartmentName" || value == "CustodianName" || value == "SupplierName")
        {
            cboValue.Visible = true;
            txtValue.Enabled = false;
            txtValue.ReadOnly = true;
            txtValue.Text = "";
            if (cboValue.DataSource != null)
            {
                cboValue.SelectedIndex = 0;
            }
            //txtValue.Visible = false;
        }
        else
        {
            cboValue.Visible = false;
            txtValue.Enabled = true;
            txtValue.ReadOnly = false;
            // txtValue.Visible = true;
        }

        if (value == "DepartmentName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "DepartmentName";
            cboValue.DataValueField = "DepartmentId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        if (value == "CustodianName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCustodian");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "CustodianName";
            cboValue.DataValueField = "CustodianId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        if (value == "SupplierName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "SupplierName";
            cboValue.DataValueField = "SupplierId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (txtValue.Text == "" && cboValue.SelectedIndex <= 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('value should not be empty..!!');", true);
            return;
        }


        if (gvData.Visible == false)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No data available to update..!!');", true);
            return;
        }

        string confirmValue = Request.Form["confirm_value"];
        string[] values = confirmValue.Split(',');
        int len = values.Length;
        confirmValue = values[len - 1].ToString();
        if (confirmValue == "Yes")
        {
            if (cboField.SelectedIndex <= 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select field to update..!!');", true);
                return;
            }

            try
            {
                if (dt_search.Rows.Count > 0)
                {
                    if (cboField.SelectedIndex >= 0)
                    {
                        string output = "";
                        for (int i = 0; i < dt_search.Rows.Count; i++)
                        {
                            output = output + dt_search.Rows[i]["AssetCode"].ToString();
                            output += (i < dt_search.Rows.Count) ? "," : string.Empty;
                        }

                        //var Assets = new SqlParameter("@Assets", output);
                        //var Column = new SqlParameter("@Column", cboField.SelectedValue);
                        //var Values = new SqlParameter("@Values", txtValue.Text);
                        //var user = new SqlParameter("@user", Session["UserName"].ToString());
                        string val = "";
                        if (txtValue.Enabled == true)
                        {
                            val = txtValue.Text;
                        }
                        else
                        {
                            val = cboValue.SelectedValue.ToString();
                        }
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SP_ASSET_BULKUPDATE", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@Assets", SqlDbType.VarChar).Value = output;
                                cmd.Parameters.Add("@Column", SqlDbType.VarChar).Value = cboField.SelectedValue;
                                cmd.Parameters.Add("@Values", SqlDbType.VarChar).Value = val;
                                cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = Session["UserName"].ToString();

                                con.Open();
                                cmd.ExecuteNonQuery();
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully..!!');", true);
                            }
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No data found to update..!!');", true);
                }
            }
            catch
            {

            }

            grid_view();
            gvData.DataBind();
            confirmValue = "";
            gvData.Visible = true;
        }
    }
}