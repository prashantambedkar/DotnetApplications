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
using Telerik.Web.UI;


public partial class Assettransfer_manual : System.Web.UI.Page
{
    public static DataTable dt = new DataTable();
    public static DataTable dt_search = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bincategory();
            ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
            bindlocation();
            bindTolocation();
            ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

            cboBuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            cboFloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

            BindDepartment();
            grid_view();
        }
    }


    //public DataTable dt_search
    //{
    //    get
    //    {
    //        return ViewState["dt_search"] as DataTable;
    //    }
    //    set
    //    {
    //        ViewState["dt_search"] = value;

    //    }
    //}
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

    private void bindTolocation()
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");

        cboLoc.DataSource = ds;
        cboLoc.DataTextField = "LocationName";
        cboLoc.DataValueField = "LocationId";
        cboLoc.DataBind();
        cboLoc.Items.Insert(0, new ListItem("--Select Location--", "0", true));

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

    protected void OnSelectedIndexChangedToLocation(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.Getbuilding, new SqlParameter[] { 
                      new SqlParameter("@LocationId",  cboLoc.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        cboBuild.DataSource = ds;
        cboBuild.DataTextField = "BuildingName";
        cboBuild.DataValueField = "BuildingId";
        cboBuild.DataBind();
        cboBuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));

        cboFloor.Items.Clear();
        cboFloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

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


    protected void OnSelectedIndexChangedToBuilding(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();

        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getfloorforasset, new SqlParameter[] { 
                      new SqlParameter("@BuildingId",  cboBuild.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        cboFloor.DataSource = ds;
        cboFloor.DataTextField = "FloorName";
        cboFloor.DataValueField = "FloorId";
        cboFloor.DataBind();
        cboFloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));

    }

    public string StrSort;
    private void grid_view()
    {
        string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
        string SubCatId = null;
        string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
        string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
        string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
        string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;

        try
        {
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
                            });
            DataSet ds = help.ExecuteDataset();
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {

                lblMessage.Text = "Problem occured while retrieving Product records. Please try again.";
            }
            else
            {
                //this.dt_search = ds.Tables[0];
                dt_search = new DataTable();
                dt_search = ds.Tables[0];
                dt = ds.Tables[0];

                DataView myView;
                myView = ds.Tables[0].DefaultView;
                ////lblcnt.Text = Convert.ToString(dt.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                //gridlist.DataSource = myView;
                //gridlist.DataBind();

                gvData.DataSource = myView;
            }

        }
        catch (Exception ex)
        {
            ////lblMsg.Visible = true;
            ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }
    public void btnreset_Click(object sender, EventArgs e)
    {

        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        txtFrmTranDate.Text = "";
        txtToTranDate.Text = "";
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //PrepareForExport(gridlist);
        ExportToExcel();
    }
    private void ExportToExcel()
    {
        if (gvData.Items.Count > 0)
        {
            if (dt.Columns.Contains("TranID"))
            {
                dt.Columns.Remove("TranID");
                dt.Columns.Remove("BuildingId");
                dt.Columns.Remove("CategoryId");
                dt.Columns.Remove("CategoryCode");
                dt.Columns.Remove("CustodianId");
                dt.Columns.Remove("DepartmentId");
                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("FloorId");
                dt.Columns.Remove("SubCatId");
                dt.Columns.Remove("SubCatCode");
                dt.Columns.Remove("AssetId1");
                dt.Columns.Remove("AssetId");
            }


            GridView GridView2 = new GridView();
            GridView2.AllowPaging = false;
            GridView2.DataSource = dt;
            GridView2.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Search.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);


                GridView2.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView2.HeaderRow.Cells)
                {
                    cell.BackColor = GridView2.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView2.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView2.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView2.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GridView2.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

            if (cboLoc.SelectedItem.ToString() != "Dispose")
            {
                if (cboBuild.SelectedIndex == 0 || cboFloor.SelectedIndex == 0 || cboLoc.SelectedIndex == 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select proper destination location.');", true);
                    return;
                }
            }


            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (chkitem.Checked == true)
                {
                    dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                }
            }

            dt_search.PrimaryKey = new DataColumn[] { dt_search.Columns["AssetId"] };
            dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
            var results = (from table1 in dt_search.AsEnumerable()
                           join table2 in dt_SelectedAsset.AsEnumerable()
                           on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                           select table1).ToList();

            DataTable dt_Transfer = new DataTable();
            if (results.Count() > 0)
            {
                dt_Transfer = results.CopyToDataTable();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Select Assets..!!');", true);
                return;
            }




            AssetVerification objVer = new AssetVerification();
            int MaxID = objVer.GetMaxTransferID("location");
            String Asset_Transfer_ID = "";
            if (MaxID == 0)
            {
                Asset_Transfer_ID = "T0001";

            }
            else
            {
                var res = MaxID + 1;
                Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#0000");
            }

            string ToDestination = "";

            ToDestination = cboLoc.SelectedItem.ToString() + "->" + cboBuild.SelectedItem.ToString() + "->" + cboFloor.SelectedItem.ToString();


            if (cboLoc.SelectedItem.ToString() == "Dispose")
            {
                ToDestination = cboLoc.SelectedItem.ToString() + "->No Building->No Floor";
                objVer.SaveAssetTransferDetails_Manual(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, "Manual Transfer", ToDestination, cboLoc.SelectedValue.ToString(), "1", "1", Session["UserName"].ToString());
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
            }
            else
            {
                objVer.SaveAssetTransferDetails_Manual(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, "Manual Transfer", ToDestination, cboLoc.SelectedValue.ToString(), cboBuild.SelectedValue.ToString(), cboFloor.SelectedValue.ToString(), Session["UserName"].ToString());
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
            }
            
            grid_view();
            gvData.DataBind();

            cboLoc.Items.Clear();
            cboLoc.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
            cboBuild.Items.Clear();
            cboBuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));


        }
        catch (Exception ex)
        {

        }
    }
}