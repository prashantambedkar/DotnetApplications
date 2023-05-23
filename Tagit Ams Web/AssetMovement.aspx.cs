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
using ECommerce.Common;

public partial class AssetMovement : System.Web.UI.Page
{

    public static DataTable dt = new DataTable();
    public static DataTable dt_search = new DataTable();
    public static string path = "";
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];

    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_PageIndexChanged", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_Init", path);
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
                txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                Page.DataBind();
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.Statistics, Session["userid"].ToString()) == true)
                {
                    divSearch.Style.Add("display", "none");
                    Bincategory();
                    ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindlocation();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    // ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

                    bindCustodian();
                    BindDepartment();
                    grid_view();
                    SetGridOrder();
                }
                else
                {
                    divSearch.Style.Add("display", "none");
                    Bincategory();
                    ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindlocation();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    // ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

                    bindCustodian();
                    BindDepartment();
                    SetGridOrder();
                    Response.Redirect("AcceessError.aspx");
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "SetGridOrder", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "bindCustodian", path);
        }
    }
    private void grid_view()
    {
        try
        {
            //AssetBL objCatBL = new AssetBL();
            //DataSet ds = objCatBL.GetAssetMovement();

            string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
            string SubCatId = null;
            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
            string AssetCode = txtAssetCode.Text.ToString() == "" ? null : txtAssetCode.Text.ToString();
            string CustodianId = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            string fromdate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            string todate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;

            fromdate = fromdate == null ? todate = null : fromdate;
            todate = todate == null ? fromdate = null : todate;
            DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getMovementHistoryV2, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId",  SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                          new SqlParameter("@AssetCode",  AssetCode),
                           new SqlParameter("@CustodianId",CustodianId),
                             new SqlParameter("@SearchText",  SearchText),
                             new SqlParameter("@UserID",  Session["userid"].ToString()),
                              new SqlParameter("@fromdate",fromdate),
                                 new SqlParameter("@todate", todate),
                            });
            DataSet ds = help.ExecuteDataset();
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                //gvData.Visible = false;
                //lblMessage.Text = "Problem occured while retrieving Product records. Please try again.";
            }
            else
            {
                dt_search = new DataTable();
                dt_search = ds.Tables[0];
                dt = ds.Tables[0];

                DataView myView;
                myView = ds.Tables[0].DefaultView;
                gvData.DataSource = myView;
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "grid_view", path);
            ////lblMsg.Visible = true;
            ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
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
            StoredProcedures.getcategoryinsubcatasset, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  ddlproCategory.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
           
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "OnSelectedIndexChangedCategory", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "OnSelectedIndexChangedBuilding", path);
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

            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "OnSelectedIndexChangedLocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "BindDepartment", path);
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
            ddlproCategory.DataValueField = "CategoryId";
            ddlproCategory.DataBind();
            ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "Bincategory", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "bindlocation", path);
        }
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_NeedDataSource", path);
        }
    }

    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlproCategory.SelectedIndex = 0;
           // ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlCustodian.SelectedIndex = 0;

            //ddlsubcat.Items.Clear();
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));

            txtAssetCode.Text = "";
            txtSearch.Text = "";
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "btnreset_Click", path);
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "btnsubmit_Click", path);
        }
    }


    //added by ponraj
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_ItemCreated", path);
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }
    private void ExportToExcel()
    {
        try
        {
            DataTable rpt_DT = new DataTable();
            DataTable DT2 = new DataTable();
            if (dt == null || dt.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                rpt_DT = dt.Copy();
            }
            if (rpt_DT.Rows.Count > 0)
            {

                try { rpt_DT.Columns.Remove("CategoryId"); } catch { }
                try { rpt_DT.Columns.Remove("SubCatId"); } catch { }
                try { rpt_DT.Columns.Remove("LocationId"); } catch { }
                try { rpt_DT.Columns.Remove("BuildingId"); } catch { }
                try { rpt_DT.Columns.Remove("FloorId"); } catch { }
                try { rpt_DT.Columns.Remove("DepartmentId"); } catch { }
                try { rpt_DT.Columns.Remove("CustodianId"); } catch { }
                try { rpt_DT.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets + " "); } catch { }
                try { rpt_DT.Columns["LocationName"].ColumnName = Location; } catch { }
                try { rpt_DT.Columns["BuildingName"].ColumnName = Building; } catch { }
                try { rpt_DT.Columns["FloorName"].ColumnName = Floor; } catch { }
                try { rpt_DT.Columns["CategoryName"].ColumnName = Category; } catch { }
                try { rpt_DT.Columns["SubCatName"].ColumnName = SubCategory; } catch { }


                AssetBL objAsset = new AssetBL();
                List<MappingInfo> clientColumns = new List<MappingInfo>();
                clientColumns = objAsset.GetMappingListFromDB();

                foreach (DataColumn col in rpt_DT.Columns)
                {
                    if (col.ColumnName.Contains("Column"))
                    {
                        var clientValue = clientColumns.Where(c => c.ColumnName == col.ColumnName.ToString().Trim());
                        foreach (var a in clientValue)
                        {
                            if (a.MappingColumnName != null && a.MappingColumnName != "")
                            {
                                rpt_DT.Columns[a.ColumnName].ColumnName = a.MappingColumnName + "#";
                            }
                        }
                    }
                }
                DT2 = rpt_DT.Copy();
                foreach (DataColumn column in DT2.Columns)
                {
                    if (column.ColumnName.ToString().Contains("Column"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("SerialNo"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Price"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Description"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Sub Category"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("Case Assignee Name"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                    if (column.ColumnName.ToString().Contains("DepartmentName"))
                    {
                        rpt_DT.Columns.Remove(column.ColumnName);
                    }
                }
                rpt_DT.Columns["Created_Date"].ColumnName = "Document Movement Date";
                ////Response.Clear();
                ////Response.Clear();
                ////Response.AddHeader("content-disposition",
                ////                      "attachment;filename=Transfer.xls");
                ////Response.Charset = String.Empty;
                ////Response.ContentType = "application/ms-excel";
                ////StringWriter stringWriter = new StringWriter();
                ////HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
                ////gridDetails.AllowPaging = false;
                ////gridDetails.DataSource = this.dt_grdDetails;
                ////gridDetails.DataBind();
                ////gridDetails.RenderControl(HtmlTextWriter);
                ////Response.Write(stringWriter.ToString());
                ////Response.End();

                GridView GridView2 = new GridView();
                GridView2.AllowPaging = false;
                GridView2.DataSource = rpt_DT;
                GridView2.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Transfer.xls");
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "ExportToExcel", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "gvData_DataBinding", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetMovement.aspx", "GetGrid_Click", path);
        }
    }
}