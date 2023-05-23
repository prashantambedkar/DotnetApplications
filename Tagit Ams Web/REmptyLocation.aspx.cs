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

public partial class REmptyLocation : System.Web.UI.Page
{
    public static DataTable dt_result = new DataTable();

    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    public static string path = "";
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
        gvData.ClientSettings.Scrolling.ScrollTop = "0";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "gvData_Init", path);

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
                    bindlocation();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");

                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "bindlocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "OnSelectedIndexChangedLocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            txtSearch.Text = "";
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));

            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "btnRefresh_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "btnSearch_Click", path);
        }
    }

    private void grid_view()
    {
        try
        {
            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();

            ReportBL objReport = new ReportBL();
            DataSet ds = objReport.GetEmptyLocationV2(LocationId, BuildingId, FloorId, SearchText, Session["userid"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt_result = ds.Tables[0];
                gvData.Visible = true;
                gvData.DataSource = ds;
            }
            else
            {
                gvData.Visible = false;
                gvData.DataSource = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "grid_view", path);
        }
    }
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "PrepareForExport", path);
        }
    }
    private void ExportToExcel()
    {
        try
        {
            DataTable DT2 = new DataTable();


            if (gvData.Items.Count > 0)
            {
                if (dt_result.Columns.Contains("FloorId"))
                {
                    dt_result.Columns.Remove("FloorId");
                    dt_result.Columns.Remove("LocationId");
                    dt_result.Columns.Remove("BuildingId");
                }

                //added by ponraj
                //try { dt_result.Columns["FloorCode"].ColumnName = Floor+" Code"; } catch { }
                try { dt_result.Columns["LocationName"].ColumnName = Location; } catch { }
                try { dt_result.Columns["BuildingName"].ColumnName = Building; } catch { }
                try { dt_result.Columns["FloorName"].ColumnName = Floor; } catch { }



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
                }

                GridView GridView2 = new GridView();
                GridView2.AllowPaging = false;
                GridView2.DataSource = dt_result;
                GridView2.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Empty_Location.xls");
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "ExportToExcel", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "gvData_NeedDataSource", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "GetGrid_Click", path);

        }
    }
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["LocationName"].Text = Location.ToUpper();
                item["BuildingName"].Text = Building.ToUpper();
                item["FloorName"].Text = Floor.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["LocationName"].Text = Location.ToUpper();
                item["BuildingName"].Text = Building.ToUpper();
                item["FloorName"].Text = Floor.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "REmptyLocation.aspx", "gvData_ItemCreated", path);
        }
    }
}