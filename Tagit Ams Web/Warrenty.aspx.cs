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
using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;

public partial class Warrenty : System.Web.UI.Page
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
            ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
            BindDepartment();

            BindSupplier();
            txtstartdate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtenddate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            dt_search = new DataTable();
            grid_view();

        }
    }


    public string AssetId
    {
        get
        {
            return Convert.ToString(ViewState["AssetId"]);
        }
        set
        {
            ViewState["AssetId"] = value;
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
    private void BindSupplier()
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");

        ddlsupplier.DataSource = ds;
        ddlsupplier.DataTextField = "SupplierName";
        ddlsupplier.DataValueField = "SupplierId";
        ddlsupplier.DataBind();
        ddlsupplier.Items.Insert(0, new ListItem("--Select Supplier--", "0", true));

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
    //protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
    //{
    //    if (ddldept.SelectedIndex == 0)
    //    {
    //        ddlAsstID.Items.Clear();
    //    }
    //    else
    //    {
    //        SqlConnection con = new SqlConnection();
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    //        SqlDataAdapter dpt = new SqlDataAdapter();


    //        DataAccessHelper1 help = new DataAccessHelper1(
    //        StoredProcedures.getAssetinWarrenty, new SqlParameter[] { 
    //                        new SqlParameter("@CategoryId",  ddlproCategory.SelectedValue),
    //                  new SqlParameter("@SubCatId",  ddlsubcat.SelectedValue),
    //                   new SqlParameter("@LocationId",  ddlloc.SelectedValue),
    //                    new SqlParameter("@BuildingId",  ddlbuild.SelectedValue),
    //                     new SqlParameter("@FloorId",  ddlfloor.SelectedValue),
    //                      new SqlParameter("@DepartmentId",  ddldept.SelectedValue),

    //                        });
    //        DataSet ds = help.ExecuteDataset();

    //        ddlAsstID.DataSource = ds;
    //        ddlAsstID.DataTextField = "AssetCode";
    //        ddlAsstID.DataValueField = "AssetId";
    //        ddlAsstID.DataBind();
    //    }

    //}


    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));



            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (((CheckBox)item.FindControl("cboxSelect")).Checked)
                {
                    dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                }
            }

            if (dt_SelectedAsset.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select items..');", true);
                return;
            }

            //foreach (DataGridItem item in gridlist.Items)
            //{
            //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //    {
            //        HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
            //        CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("chkitem");
            //        if (chkitem.Checked == true)
            //        {
            //            dt_SelectedAsset.Rows.Add(hdnAstID.Value);
            //        }
            //    }

            //}

            dt_search.PrimaryKey = new DataColumn[] { dt_search.Columns["AssetId"] };
            dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
            var results = (from table1 in dt_search.AsEnumerable()
                           join table2 in dt_SelectedAsset.AsEnumerable()
                           on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                           select table1).ToList();

            DataTable dt_AMCData = new DataTable();
            if (results.Count() > 0)
            {
                dt_AMCData = results.CopyToDataTable();
            }
            if (dt_AMCData.Rows.Count > 0)
            {
                if (txtstartdate.Text != "" && txtenddate.Text != "")
                {
                    if (Convert.ToDateTime(txtenddate.Text) < Convert.ToDateTime(txtstartdate.Text))
                    {
                        string Message = "AMC End Date should be greater than Start Date";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Message + " ..!!');", true);
                        return;
                    }
                }

                this.AssetId = string.Join(",", dt_AMCData.AsEnumerable().Select(s => s.Field<int>("AssetId")).ToArray<int>());

                //string dt1 = DateTime.Parse(txtstartdate.Text.Trim()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                DataAccessHelper1 help = new DataAccessHelper1(
                                StoredProcedures.PinsertUpdateWarrentyOrAMC, new SqlParameter[] 
                        {
                            new SqlParameter("@Type", txtwarr.Text.Trim()),
                           
                    new SqlParameter("@StartDate", txtstartdate.Text.Trim()),
                    new SqlParameter("@EndDate", txtenddate.Text.Trim()),
                    new SqlParameter("@Remarks", txtrmk.Text.Trim()),
                      new SqlParameter("@Assetid", this.AssetId),  
                       new SqlParameter("@Supplier", ddlsupplier.SelectedValue),           
                    
                    }
                          );
                if (help.ExecuteNonQuery() >= 1)
                {
                    dt_search.Rows.Clear();
                    grid_view();
                    gvData.DataBind();
                    btnreset_Click(sender, e);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Update Successfully..!!');", true);

                }
            }


        }
        catch (Exception ex)
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }

        //try
        //{
        //    if (btnsubmit.Text == "Submit")
        //    {
        //        DataAccessHelper1 help = new DataAccessHelper1(
        //                StoredProcedures.PinsertUpdateWarrentyOrAMC, new SqlParameter[] 
        //                {
        //                    new SqlParameter("@Type", txtwarr.Text.Trim()),

        //            new SqlParameter("@StartDate", txtstartdate.Text.Trim()),
        //            new SqlParameter("@EndDate", txtenddate.Text.Trim()),
        //            new SqlParameter("@Remarks", txtrmk.Text.Trim()),
        //              new SqlParameter("@Assetid", this.AssetId),
        //              new SqlParameter("@AMCtype", "Insert"),


        //                }
        //                );


        //        if (help.ExecuteNonQuery() <= 1)
        //        {


        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Inserted Successfully..!!');", true);
        //        }


        //    }


        //    else if (btnsubmit.Text == "Update")
        //    {


        //            DataAccessHelper1 help = new DataAccessHelper1(
        //               StoredProcedures.PinsertUpdateWarrentyOrAMC, new SqlParameter[] { 
        //                  new SqlParameter("@Type", txtwarr.Text.Trim()),

        //            new SqlParameter("@StartDate", txtstartdate.Text.Trim()),
        //            new SqlParameter("@EndDate", txtenddate.Text.Trim().ToString()),
        //            new SqlParameter("@Remarks", txtrmk.Text.Trim().ToString()),
        //              new SqlParameter("@Assetid", this.AssetId),
        //              new SqlParameter("@AMCtype", "Update"),

        //        }
        //               );

        //            if (help.ExecuteNonQuery() <= 1)
        //            {
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Update Successfully..!!');", true);
        //            }

        //            btnsubmit.Text = "Submit";
        //        }



        //}
        //catch (Exception ex)
        //{
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);

        //}
        //hdncatidId.Value = "";
        //grid_view();

    }

    private void grid_view()
    {
        try
        {
            if (dt_search.Rows.Count == 0)
            {
                // string Asset = (Convert.ToString(this.AssetId) == "0") ? null : Convert.ToString(this.AssetId);
                string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
                string SubCatId = null;
                string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
                string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
                string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
                string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
                string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;


                DataAccessHelper1 help = new DataAccessHelper1(
              StoredProcedures.GetAssetWiseAMCDetails, new SqlParameter[] { 
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                          new SqlParameter("@AssetCode",  AssetCode),
                          // new SqlParameter("@AssetId",  Asset),
                    });

                DataSet ds = help.ExecuteDataset();
                dt_search = new DataTable();
                dt_search = ds.Tables[0];
                dt = new DataTable();
                dt = ds.Tables[0];
                dt_search = dt;
                DataView myView;
                myView = ds.Tables[0].DefaultView;
                lblcnt.Text = Convert.ToString(dt.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                ////gridlist.DataSource = myView;
                ////gridlist.DataBind();

                gvData.DataSource = myView;
            }
            else
            {
                DataView myView;
                myView = dt_search.DefaultView;
                lblcnt.Text = Convert.ToString(dt_search.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                ////gridlist.DataSource = myView;
                ////gridlist.DataBind();

                gvData.DataSource = myView;
            }


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }
    }

    public string StrSort;

    public void btnreset_Click(object sender, EventArgs e)
    {
        //this.AssetId = 0;
        txtAssetCode.Text = "";
        dt_search.Rows.Clear();
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        ddlsupplier.SelectedIndex = 0;
        //OnSelectedIndexChangedDepartment(sender, e);
        txtrmk.Text = string.Empty;
        txtstartdate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
        txtenddate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
        txtwarr.Text = string.Empty;
        grid_view();
        gvData.DataBind();
        //btnsubmit.Text = "Submit";
    }




    protected void btnsearchsubmit_Click(object sender, EventArgs e)
    {
        dt_search.Rows.Clear();
        // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
        grid_view();
        gvData.DataBind();
        //this.AssetId = 0;
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        // OnSelectedIndexChangedDepartment(sender, e);

    }

    protected void gridlist_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
       e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblBalance = (Label)e.Item.FindControl("Type");


            if (lblBalance.Text != "")
            {
                btnsubmit.Text = "Update";
            }

        }
    }

    protected void exptxl_Click(object sender, EventArgs e)
    {
        //PrepareForExport(gridlist);
        ExportToExcel();
    }
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
    private void ExportToExcel()
    {
        if (gvData.Items.Count > 0)
        {
            //Response.Clear();
            //Response.Clear();
            //Response.AddHeader("content-disposition",
            //                      "attachment;filename=Warrenty.xls");
            //Response.Charset = String.Empty;
            //Response.ContentType = "application/ms-excel";
            //StringWriter stringWriter = new StringWriter();
            //HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            //gridlist.AllowPaging = false;
            //gridlist.DataSource = this.dt_search;
            //gridlist.DataBind();
            //gridlist.RenderControl(HtmlTextWriter);
            //Response.Write(stringWriter.ToString());
            //Response.End();


            if (dt.Columns.Contains("AssetId"))
            {
                dt.Columns.Remove("BuildingId");
                dt.Columns.Remove("CategoryId");
                dt.Columns.Remove("CustodianId");
                dt.Columns.Remove("DepartmentId");

                dt.Columns.Remove("LocationId");
                dt.Columns.Remove("FloorId");
                dt.Columns.Remove("SubCatId");
                dt.Columns.Remove("AssetId");

            }

            GridView GridView2 = new GridView();
            GridView2.AllowPaging = false;
            GridView2.DataSource = dt;
            GridView2.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Warrenty.xls");
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
            //exptxl.Enabled = false;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        grid_view();
    }

    protected void HeaderCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox headercheckbox = (CheckBox)sender;
        if (headercheckbox.Checked)
        {
            foreach (GridDataItem item in gvData.Items)
            {
                item.Selected = headercheckbox.Checked;
                CheckBox checkbox = (CheckBox)item.FindControl("CheckBox1");
                checkbox.Checked = headercheckbox.Checked;
            }
        }
    }

}