using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ECommerce.DataAccess;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.Utilities;
using ECommerce.Common;
using Serco;
using System.Drawing;
using Telerik.Web.UI;

public partial class REncodedlabels : System.Web.UI.Page
{
    public static DataTable dt_result = new DataTable();
    public static string path = "";
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];

    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    //public DataTable dt_result
    //{
    //    get
    //    {
    //        return ViewState["dt_result"] as DataTable;
    //    }
    //    set
    //    {
    //        ViewState["dt_result"] = value;

    //    }
    //}
    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_PageIndexChanged", path);
        }
    }

    protected void gvData_Init(object sender, EventArgs e)
    {
        try
        {
            Telerik.Web.UI.GridFilterMenu menu = gvData.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == "Contains")
                {
                    i++;
                }
                else if (menu.Items[i].Text == "EqualTo")
                {
                    i++;
                }
                else if (menu.Items[i].Text == "StartsWith")
                {
                    i++;
                }
                else if (menu.Items[i].Text == "NoFilter")
                {
                    i++;
                }
                else
                {
                    menu.Items.RemoveAt(i);
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_Init", path);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                Page.DataBind();
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.Statistics, Session["userid"].ToString()) == true)
                {
                    divSearch.Style.Add("display", "none");
                    //txtFrmDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    //txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    Bincategory();
                    ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindlocation();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    BindDepartment();
                    bindCustodian();
                    SetGridOrder();
                    ////lblTotHeader.Visible = false;
                }
                else
                {
                    divSearch.Style.Add("display", "none");
                    //txtFrmDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    //txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    Bincategory();
                    ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindlocation();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    BindDepartment();
                    bindCustodian();
                    SetGridOrder();
                    Response.Redirect("AcceessError.aspx");

                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "Page_Load", path);
        }
    }
    public void SetGridOrder()
    {
        try
        {
            if (_Order == "true")
            {
                foreach (Telerik.Web.UI.GridColumn col in gvData.MasterTableView.RenderColumns)
                {
                    if (col.UniqueName.Equals("Column1"))
                    {
                        col.OrderIndex = 4;
                    }
                    else if (col.UniqueName.Equals("Column2"))
                    {
                        col.OrderIndex = 4;
                    }
                    else if (col.UniqueName.Equals("Column3"))
                    {
                        col.OrderIndex = 4;
                    }
                }
                gvData.Rebind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "SetGridOrder", path);
        }
    }
    // Check User is Authorize to view this page
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }


    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    private void bindCustodian()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("select cm.CustodianId ,cm.CustodianName from CustodianMaster as cm left join CustodianPermission as cp on cp.CustodianId=cm.CustodianId where cp.UserID=@UserID and cm.Active=1", con))
            {
                cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }

            ddlCustodian.DataSource = ds;
            ddlCustodian.DataTextField = "CustodianName";
            ddlCustodian.DataValueField = "CustodianId";
            ddlCustodian.DataBind();
            ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "bindCustodian", path);

        }
    }
    private void BindDepartment()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

            ddldept.DataSource = ds;
            ddldept.DataTextField = "DepartmentName";
            ddldept.DataValueField = "DepartmentId";
            ddldept.DataBind();
            ddldept.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "BindDepartment", path);
        }
    }
    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {
        try
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();



            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getSubCategory, new SqlParameter[] {
                      new SqlParameter("@CategoryID",  ddlproCategory.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();

            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "OnSelectedIndexChangedCategory", path);
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "RegisterUser", "TCM_Controller");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "RegisterUser", "TCM_Controller");
        }
    }
    protected void OnSelectedIndexChangedLocation(object sender, EventArgs e)
    {
        try
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
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "OnSelectedIndexChangedLocation", path);
        }
    }
    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        try
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
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }
    private void bindlocation()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + Session["userid"].ToString() + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }

            ddlloc.DataSource = ds;
            ddlloc.DataTextField = "LocationName";
            ddlloc.DataValueField = "LocationId";
            ddlloc.DataBind();
            ddlloc.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "bindlocation", path);
        }
    }
    private void Bincategory()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            ddlproCategory.DataSource = ds;
            ddlproCategory.DataTextField = "CategoryName";
            ddlproCategory.DataValueField = "CategoryID";
            ddlproCategory.DataBind();
            ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "Bincategory", path);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            //txtFrmDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            //txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtAssetCode.Text = "";
            ddlloc.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlproCategory.SelectedIndex = 0;
            //ddlsubcat.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlCustodian.SelectedIndex = 0;
            txtSearch.Text = "";
            grid_view();
            gvData.DataBind();

            //ddlsubcat.Items.Clear();
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "btnRefresh_Click", path);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "btnSearch_Click", path);
        }
    }

    private void grid_view()
    {
        try
        {
            //string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            //string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
            //FromDate = FromDate == null ? ToDate = null : FromDate;
            //ToDate = ToDate == null ? FromDate = null : ToDate;

            //string LocationId = (ddlloc.SelectedValue == "0") ? "0" : ddlloc.SelectedValue;
            //string BuildingId = (ddlbuild.SelectedValue == "0") ? "1" : ddlbuild.SelectedValue;
            //string FloorId = (ddlfloor.SelectedValue == "0") ? "1" : ddlfloor.SelectedValue;
            //string CatID = (ddlproCategory.SelectedValue == "0") ? "0" : ddlproCategory.SelectedValue;
            //string SubCatID = (ddlsubcat.SelectedValue == "0") ? "1" : ddlsubcat.SelectedValue;

            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string CatID = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
            string SubCatId = null;
            string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;
            string DepID = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
            string CustodianID = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();

            ReportBL objReport = new ReportBL();
            DataSet ds = objReport.GetEncodedLabelsV2(null, null, LocationId, BuildingId, FloorId, CatID, null, AssetCode, DepID, CustodianID, SearchText, Session["userid"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ////gridlist.DataSource = ds;
                //this.dt_result = ds.Tables[0];
                dt_result = ds.Tables[0];
                ////gridlist.DataBind();
                ////lblTotHeader.Visible = true;
                gvData.Visible = true;
                gvData.DataSource = ds;
            }
            else
            {
                ////gridlist.DataSource = null;
                ////gridlist.DataBind();
                ////lblTotHeader.Visible = false;
                gvData.Visible = false;
                gvData.DataSource = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "grid_view", path);
        }
    }
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        //PrepareForExport(gridlist);
        ExportToExcel();
    }
    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    grid_view();
    //}
    private void PrepareForExport(Control ctrl)
    {
        try
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
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "PrepareForExport", path);
        }
    }
    private void ExportToExcel()
    {
        try
        {
            DataTable DT2 = new DataTable();
            //if (gridlist.Items.Count > 0)
            //{
            //    Response.Clear();
            //    Response.Clear();
            //    Response.AddHeader("content-disposition",
            //                          "attachment;filename=PrintedLabelsHistory.xls");
            //    Response.Charset = String.Empty;
            //    Response.ContentType = "application/ms-excel";
            //    StringWriter stringWriter = new StringWriter();
            //    HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            //    gridlist.AllowPaging = false;
            //    gridlist.DataSource = this.dt_result;
            //    gridlist.DataBind();
            //    gridlist.RenderControl(HtmlTextWriter);
            //    Response.Write(stringWriter.ToString());
            //    Response.End();
            //}

            if (gvData.Items.Count > 0)
            {
                //Response.Clear();
                //Response.Clear();
                //Response.AddHeader("content-disposition",
                //                      "attachment;filename=PrintedLabelsHistory.xls");
                //Response.Charset = String.Empty;
                //Response.ContentType = "application/ms-excel";
                //StringWriter stringWriter = new StringWriter();
                //HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
                //gridlist.AllowPaging = false;
                //gridlist.DataSource = this.dt_result;
                //gridlist.DataBind();
                //gridlist.RenderControl(HtmlTextWriter);
                //Response.Write(stringWriter.ToString());
                //Response.End();
                //added by ponraj
                try { dt_result.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets + " "); } catch { }
                try { dt_result.Columns["LocationName"].ColumnName = Location; } catch { }
                try { dt_result.Columns["BuildingName"].ColumnName = Building; } catch { }
                try { dt_result.Columns["FloorName"].ColumnName = Floor; } catch { }
                try { dt_result.Columns["CategoryName"].ColumnName = Category; } catch { }
                try { dt_result.Columns["SubCatName"].ColumnName = SubCategory; } catch { }

                if (dt_result.Columns.Contains("Price1"))
                {
                    dt_result.Columns.Remove("Price1");
                }
                AssetBL objAsset = new AssetBL();
                List<MappingInfo> clientColumns = new List<MappingInfo>();
                clientColumns = objAsset.GetMappingListFromDB();

                foreach (DataColumn col in dt_result.Columns)
                {
                    if (col.ColumnName.Contains("Column"))
                    {
                        var clientValue = clientColumns.Where(c => c.ColumnName == col.ColumnName.ToString().Trim());
                        foreach (var a in clientValue)
                        {
                            if (a.MappingColumnName != null && a.MappingColumnName != "")
                            {
                                dt_result.Columns[a.ColumnName].ColumnName = a.MappingColumnName + "#";
                            }
                        }
                    }
                }
                DT2 = dt_result.Copy();
                foreach (DataColumn column in DT2.Columns)
                {
                    if (column.ColumnName.ToString().Contains("Column"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("SerialNo"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Price"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Description"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Sub Category"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Case Assignee Name"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("DepartmentName"))
                    {
                        dt_result.Columns.Remove(column.ColumnName);
                    }

                }

                GridView GridView2 = new GridView();
                GridView2.AllowPaging = false;
                GridView2.DataSource = dt_result;
                GridView2.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Encoded_Labels.xls");
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
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "ExportToExcel", path);
        }
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            grid_view();

            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_NeedDataSource", path);
        }
    }


    //Added by ponraj
    protected void gvData_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["Location"].Text = Location.ToUpper();
                item["Building"].Text = Building.ToUpper();
                item["Floor"].Text = Floor.ToUpper();
                item["Category"].Text = Category.ToUpper();
                item["SubCategory"].Text = SubCategory.ToUpper();
            }
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["Location"].Text = Location.ToUpper();
                item["Building"].Text = Building.ToUpper();
                item["Floor"].Text = Floor.ToUpper();
                item["Category"].Text = Category.ToUpper();
                item["SubCategory"].Text = SubCategory.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_ItemCreated", path);
        }
    }
    protected void gvData_DataBinding(object sender, EventArgs e)
    {
        try
        {
            AssetBL objAsset = new AssetBL();
            LabelConfigBL objLB = new LabelConfigBL();
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            ListMapping = objAsset.GetMappingListFromDB();
            DataSet ds = objLB.GetLabelConfigDetails("T6");
            DataTable dt_Col = new DataTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt_Col = ds.Tables[0];
            }

            int i = 1;
            foreach (var ColMap in ListMapping)
            {
                if (ColMap.ColumnName.Contains("Column"))
                {
                    if (ListMapping.Any(L => L.ColumnName == "Column" + i.ToString()) == true)
                    {
                        string MapSerialName = ListMapping.Where(x => x.ColumnName == "Column" + i.ToString()).SingleOrDefault().MappingColumnName;
                        gvData.MasterTableView.GetColumn("column" + i.ToString()).HeaderText = MapSerialName.ToUpper();

                        DataRow[] dr = dt_Col.Select("FieldName='Column" + i.ToString() + "' and printStatus='1'");
                        if (dr.Length == 0)
                        {
                            gvData.MasterTableView.GetColumn("column" + i.ToString()).Display = true;
                        }
                        i = i + 1;
                    }
                }
            }

            foreach (GridColumn column in gvData.Columns)
            {
                if (column.HeaderText.ToString().Contains("Column"))
                {
                    column.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "gvData_DataBinding", path);
        }
    }

    protected void GetGrid_Click(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REncodedlabels.aspx", "GetGrid_Click", path);
        }
    }
}