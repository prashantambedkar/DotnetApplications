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
using System.Net;
using System.Xml.Linq;
using System.Threading;
using System.Net.Mail;
using TagitEncrypt;
using ClosedXML.Excel;
using System.Text;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;

public partial class TransferAssets : System.Web.UI.Page
{
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String Dispose = System.Configuration.ConfigurationManager.AppSettings["Dispose"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    public static string path = "";

    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];

    public static DataTable dt = new DataTable();
    public static DataTable dt_search = new DataTable();

    private void bindFromCustodian()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataSet ds = new DataSet();
            ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());


            cboFromCustodian.DataSource = ds;
            cboFromCustodian.DataTextField = "CustodianName";
            cboFromCustodian.DataValueField = "CustodianId";
            cboFromCustodian.DataBind();
            cboFromCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "bindFromCustodian", path);

        }
    }
    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_Init", path);
        }
    }

    private void bindToCustodian(string FromCustodian)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            // DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCustodian");
            DataSet ds = new DataSet(); ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
            if (FromCustodian != null)
            {
                DataRow[] dt_filter = ds.Tables[0].Select("CustodianName<>'" + cboFromCustodian.SelectedItem.ToString() + "'");
                if (dt_filter.Length > 0)
                {
                    DataTable dt_Cust = dt_filter.CopyToDataTable<DataRow>();

                    cboToCustodian.DataSource = dt_Cust;
                    cboToCustodian.DataTextField = "CustodianName";
                    cboToCustodian.DataValueField = "CustodianId";
                    cboToCustodian.DataBind();
                    //cboToCustodian.Items.Insert(0, new ListItem("--Custodian--", "0", true));

                }
            }
            else
            {
                cboToCustodian.DataSource = ds;
                cboToCustodian.DataTextField = "CustodianName";
                cboToCustodian.DataValueField = "CustodianId";
                cboToCustodian.DataBind();
                cboToCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "bindToCustodian", path);
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popzz", "getLocation();", true);
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
                    Response.Redirect("login.aspx");
                }
                if (userAuthorize((int)pages.Transfer_Manual, Session["userid"].ToString()) == true)
                {
                    divSearch.Style.Add("display", "none");
                    divbyCustodian.Style.Add("display", "none");
                    divbyLocation.Style.Add("display", "none");
                    Bincategory();
                    ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindlocation();
                    bindTolocation();

                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

                    cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    bindFromCustodian();
                    bindToCustodian(null);
                    BindDepartment();
                    bindCustodian();
                    grid_view();
                    //if (HdnLocation.Value == "")
                    //{ GetGPSLocation(); }
                    SetGridOrder();
                    //worker = new Thread(new ThreadStart(work));
                    //worker.Start();

                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }
                //gvData.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "Page_Load", path);
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
                        col.OrderIndex = 5;
                    }
                    else if (col.UniqueName.Equals("Column2"))
                    {
                        col.OrderIndex = 5;
                    }
                    else if (col.UniqueName.Equals("Column3"))
                    {
                        col.OrderIndex = 5;
                    }
                }
                gvData.Rebind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "SetGridOrder", path);
        }
    }
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "bindCustodian", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "Bincategory", path);
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
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "bindlocation", path);
        }
    }

    private void bindTolocation()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = new DataSet();
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + Session["userid"].ToString() + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }

            cboLoc.DataSource = ds;
            cboLoc.DataTextField = "LocationName";
            cboLoc.DataValueField = "LocationId";
            cboLoc.DataBind();
            cboLoc.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "bindTolocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "BindDepartment", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "OnSelectedIndexChangedCategory", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "OnSelectedIndexChangedLocation", path);
        }
    }

    protected void OnSelectedIndexChangedToLocation(object sender, EventArgs e)
    {
        try
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
            cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));

            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "OnSelectedIndexChangedToLocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }


    protected void OnSelectedIndexChangedToBuilding(object sender, EventArgs e)
    {
        try
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
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "OnSelectedIndexChangedToBuilding", path);
        }
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
        string AssetCode = txtAssetCode.Text.ToString() == "" ? null : txtAssetCode.Text.ToString();
        string CustodianId = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;
        string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();

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
                          new SqlParameter("@AssetCode",  AssetCode),
                          new SqlParameter("@SearchText",  SearchText),
                          new SqlParameter("@CustodianId",  CustodianId),
                          new SqlParameter("@UserId",Session["userid"].ToString()),
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
                DataView EmpltyView;
                if (cboFromCustodian.SelectedIndex > 0)
                {
                    DataRow[] dt_filter = dt_search.Select("Custodian='" + cboFromCustodian.SelectedItem.ToString() + "'");
                    if (dt_filter.Length > 0)
                    {
                        dt_search = dt_filter.CopyToDataTable<DataRow>();
                        gvData.DataSource = dt_search;

                    }
                    else
                    {
                        EmpltyView = ds.Tables[0].DefaultView;
                        EmpltyView.RowFilter = "1=2";
                        gvData.DataSource = EmpltyView;
                    }
                }
                else
                {
                    DataView myView;
                    myView = ds.Tables[0].DefaultView;
                    if (StrSort != "")
                    {
                        myView.Sort = StrSort;
                    }

                    gvData.DataSource = myView;
                }

            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "grid_view", path);
            ////lblMsg.Visible = true;
            ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }


    public void btnClearCustodianTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            ddlproCategory.SelectedIndex = 0;
            //ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlCustodian.SelectedIndex = 0;
            //txtFrmTranDate.Text = "";
            //txtToTranDate.Text = "";
            cboLoc.SelectedIndex = 0;
            cboBuild.SelectedIndex = 0;
            cboFloor.SelectedIndex = 0;
            //ddlsubcat.Items.Clear();
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));


            cboBuild.Items.Clear();
            cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));

            txtAssetCode.Text = "";
            cboFromCustodian.SelectedIndex = 0;
            cboToCustodian.SelectedIndex = 0;
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "btnClearCustodianTransfer_Click", path);
        }
    }

    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlproCategory.SelectedIndex = 0;
            //ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlCustodian.SelectedIndex = 0;
            //txtFrmTranDate.Text = "";
            //txtToTranDate.Text = "";
            cboLoc.SelectedIndex = 0;
            cboBuild.SelectedIndex = 0;
            cboFloor.SelectedIndex = 0;
            //ddlsubcat.Items.Clear();
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));


            cboBuild.Items.Clear();
            cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));

            txtAssetCode.Text = "";
            txtSearch.Text = "";
            grid_view();
            gvData.DataBind();

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "btnreset_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "btnsubmit_Click", path);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            //PrepareForExport(gridlist);
            ExportToExcel();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "btnExport_Click", path);
        }
    }
    private void ExportToExcel()
    {
        try
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
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                string Message = "No records found..";
                imgpopup.ImageUrl = "images/info.jpg";
                lblpopupmsg.Text = Message;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "ExportToExcel", path);

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "PrepareForExport", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_DataBinding", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_NeedDataSource", path);
        }
    }

    protected void btnCustodianTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            //GetGPSLocation();
            string loc = "";
            try
            {
                if (Request.Cookies["locationparam"].Value != null)
                {
                    loc = Request.Cookies["locationparam"].Value;

                }

            }
            catch (Exception ex)
            {
                loc = "";
            }



            HdnLocation.Value = loc;
            if (HdnLocation.Value == "")
            {
                HdnLocation.Value = HdnLastLocation.Value;
            }
            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

            if (cboFromCustodian.SelectedIndex == 0 || cboToCustodian.SelectedIndex == -1)
            {
                ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select proper Custodian.');", true);
                //string Message = "Please select proper Custodian.";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please select proper Custodian.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }
            if (txtReasonCusChange.Text == "" || txtReasonCusChange.Text == string.Empty)
            {
                ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select proper Custodian.');", true);
                //string Message = "Please enter the reason.";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please enter the reason.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (chkitem.Checked == true)
                {
                    dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    if (hdnAstID.Value == "" || hdnAstID.Value == string.Empty)
                    {
                        StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                        sw.WriteLine("Error-TransferAssets_Custodian_Web- Hidden field hdnAstID Value is Empty -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                        sw.Close();
                    }
                }
            }
            if (dt_SelectedAsset.Rows.Count == 0)
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Custodian_Web- Checked Rows NOT Found from Grid -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();
            }

            if (dt_search.Rows.Count == 0)
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Custodian_Web- dt_search Table is Empty -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();
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
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Custodian_Web- results Table is Empty (Linq Query) -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Select Assets..!!');", true);
                //string Message = "Please Select " + Assets + ".";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select " + Assets + ".');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            AssetVerification objVer = new AssetVerification();
            int MaxID = objVer.GetMaxTransferID("Custodian");
            String Asset_Transfer_ID = "";
            if (MaxID == 0)
            {
                //9 ZEROS
                Asset_Transfer_ID = "T000000001";
            }
            else
            {
                //9 ZEROS
                var res = MaxID + 1;
                Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#000000000");
            }
            objVer.SendAssets_For_Approval(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, txtReasonCusChange.Text, "", "", "", "", Session["UserName"].ToString(), "Custodian Transfer", cboFromCustodian.SelectedItem.ToString(), cboToCustodian.SelectedItem.ToString(), cboToCustodian.SelectedValue.ToString(), "Manual Transfer", HdnLocation.Value.ToString());
            //objVer.SaveAssetTransfer_Custodian(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, "Manual Custodian Transfer", "", cboToCustodian.SelectedValue.ToString(), Session["UserName"].ToString(), cboFromCustodian.SelectedItem.ToString(), cboToCustodian.SelectedItem.ToString());
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset sent for approval successfully');", true);
            int IsApprovalNeeded = objVer.GetIsApprovalNeeded();
            string Messages = "";
            if (IsApprovalNeeded == 1)
            {
                Messages = Assets + " sent for approval successfully.";
            }
            else
            {
                Messages = Assets + " Transfered successfully.";
            }
            //new code
            //if (cboLoc.SelectedItem.ToString() == "Document Returned")
            //{
            DataTable dttableNew = dt_Transfer.Clone();

            foreach (DataRow drtableOld in dt_Transfer.Rows)
            {
                dttableNew.ImportRow(drtableOld);
            }
            for (int j = 0; j < dttableNew.Rows.Count; j++)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                string AssetID = dttableNew.Rows[j]["AssetID"].ToString();
                using (SqlCommand cmd = new SqlCommand("update AssetMaster set LocationInDateTime=@LocationInDateTime where AssetId=@AssetId", con))
                {
                    cmd.Parameters.AddWithValue("@LocationInDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AssetId", AssetID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dttableNew.Columns.Remove("AssetID");
            dttableNew.Columns.Remove("TranID");
            dttableNew.Columns.Remove("CategoryId");
            dttableNew.Columns.Remove("CategoryCode");
            dttableNew.Columns.Remove("SubCatId");
            dttableNew.Columns.Remove("SubCategory");
            dttableNew.Columns.Remove("SubCatCode");
            dttableNew.Columns.Remove("LocationId");
            dttableNew.Columns.Remove("FloorId");
            dttableNew.Columns.Remove("BuildingId");
            dttableNew.Columns.Remove("CustodianId");
            dttableNew.Columns.Remove("DepartmentId");
            dttableNew.Columns.Remove("Department");
            dttableNew.Columns.Remove("AssetId1");
            dttableNew.Columns.Remove("Description");
            dttableNew.Columns.Remove("SerialNo");
            dttableNew.Columns.Remove("Price");
            dttableNew.Columns.Remove("Quantity");
            dttableNew.Columns.Remove("SupplierName");
            dttableNew.Columns.Remove("Column11");
            dttableNew.Columns.Remove("Column12");
            dttableNew.Columns.Remove("Column13");
            dttableNew.Columns.Remove("Column14");
            dttableNew.Columns.Remove("AssignDate");
            dttableNew.Columns.Remove("TagType");

            dttableNew.Columns["AssetCode"].SetOrdinal(0);
            dttableNew.Columns["Column2"].SetOrdinal(1);
            dttableNew.Columns["Column3"].SetOrdinal(2);
            dttableNew.Columns["Column1"].SetOrdinal(3);
            dttableNew.Columns["Category"].SetOrdinal(4);
            dttableNew.Columns["Location"].SetOrdinal(5);
            dttableNew.Columns["Building"].SetOrdinal(6);
            dttableNew.Columns["Floor"].SetOrdinal(7);
            dttableNew.Columns["Custodian"].SetOrdinal(8);
            dttableNew.Columns["DeliveryDate"].SetOrdinal(9);
            dttableNew.Columns["Status"].SetOrdinal(10);
            dttableNew.Columns["Column4"].SetOrdinal(11);
            dttableNew.Columns["Column5"].SetOrdinal(12);
            dttableNew.Columns["Column6"].SetOrdinal(13);
            dttableNew.Columns["Column7"].SetOrdinal(14);
            dttableNew.Columns["Column8"].SetOrdinal(15);
            dttableNew.Columns["Column9"].SetOrdinal(16);
            dttableNew.Columns["Column10"].SetOrdinal(17);

            dttableNew.Columns["AssetCode"].ColumnName = "DOCUMENT CODE";
            dttableNew.Columns["Category"].ColumnName = "CATEGORY";
            dttableNew.Columns["Location"].ColumnName = "MAJOR LOCATION";
            dttableNew.Columns["Floor"].ColumnName = "MINOR SUB LOCATION";
            dttableNew.Columns["Building"].ColumnName = "MINOR LOCATION";
            dttableNew.Columns["Custodian"].ColumnName = "CUSTODIAN";
            dttableNew.Columns["DeliveryDate"].ColumnName = "DELIVERY DATE";
            dttableNew.Columns["Status"].ColumnName = "STATUS";
            dttableNew.Columns["Column1"].ColumnName = "FC NUMBER";
            dttableNew.Columns["Column2"].ColumnName = "CASE ASSIGNEE NAME";
            dttableNew.Columns["Column3"].ColumnName = "CLIENT NAME";
            dttableNew.Columns["Column4"].ColumnName = "DOCUMENT CONTROLLER NAME";
            dttableNew.Columns["Column5"].ColumnName = "CASE MANAGER FULL NAME";
            dttableNew.Columns["Column6"].ColumnName = "CASE MANAGER EMAIL";
            dttableNew.Columns["Column7"].ColumnName = "CASE WORKER NAME";
            dttableNew.Columns["Column8"].ColumnName = "CASE WORKER EMAIL";
            dttableNew.Columns["Column9"].ColumnName = "CASE STATUS";
            dttableNew.Columns["Column10"].ColumnName = "CASE PERSON ASSOCIATION";
            string[] TobeDistinct = { "CASE WORKER EMAIL", "CASE WORKER NAME" };
            DataTable dtDistinct = GetDistinctRecords(dttableNew, TobeDistinct);
            //string pdfname = pdfGenerate(dttableNew);
            //string excelsheetname = excelg(dttableNew);
            string pdfname = pdfGenerate(dttableNew);
            for (int z = 0; z < dtDistinct.Rows.Count; z++)
            {
                string toemailid = dtDistinct.Rows[z].ItemArray[0].ToString();
                string name = dtDistinct.Rows[z].ItemArray[1].ToString();
                SendEmail(toemailid, name, pdfname);
            }
            string usremail = findLoggedInUserEmail();
            string[] usrdet = usremail.Split(',');
            SendEmail(usrdet[1].ToString(), usrdet[0].ToString(), pdfname);




            //new code

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + Messages + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

            cboLoc.SelectedIndex = 0;
            cboBuild.Items.Clear();
            cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));
            cboFromCustodian.SelectedIndex = 0;
            cboToCustodian.SelectedIndex = 0;
            txtReasonCusChange.Text = "";

            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "btnCustodianTransfer_Click", path);
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string loc = "";
            try
            {
                if (Request.Cookies["locationparam"].Value != null)
                {
                    loc = Request.Cookies["locationparam"].Value;

                }

            }
            catch (Exception ex)
            {
                loc = "";
            }



            HdnLocation.Value = loc;
            if (HdnLocation.Value == "")
            {
                HdnLocation.Value = HdnLastLocation.Value;
            }

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('" + loc + "');", true);
            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

            if (cboLoc.SelectedItem.ToString() != Dispose)
            {
                if (cboBuild.SelectedIndex == 0 || cboFloor.SelectedIndex == 0 || cboLoc.SelectedIndex == 0)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select proper destination location.');", true);
                    //string Message = "Please select proper destination Location.";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please select proper destination Location.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
            }
            if (txtReasonLocChange.Text == "" || txtReasonLocChange.Text == string.Empty)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select proper Custodian.');", true);
                //string Message = "Please enter the reason.";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please enter the reason.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }

            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (chkitem.Checked == true)
                {
                    dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    if (hdnAstID.Value == "" || hdnAstID.Value == string.Empty)
                    {
                        StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                        sw.WriteLine("Error-TransferAssets_Location_Web- Hidden field hdnAstID Value is Empty -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                        sw.Close();
                    }
                }

            }

            if (dt_SelectedAsset.Rows.Count == 0)
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Location_Web- Checked Rows NOT Found from Grid -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();
            }

            if (dt_search.Rows.Count == 0)
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Location_Web- dt_search Table is Empty -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();
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
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErroLogFile.txt", true);
                sw.WriteLine("Error-TransferAssets_Location_Web- results Table is Empty (Linq Query) -" + Session["UserName"].ToString() + "-" + DateTime.Now.ToString());
                sw.Close();

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Select Assets..!!');", true);
                //string Message = "Please Select " + Assets + ".";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select " + Assets + ".');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }




            AssetVerification objVer = new AssetVerification();
            int MaxID = objVer.GetMaxTransferID("location");
            String Asset_Transfer_ID = "";
            if (MaxID == 0)
            {
                //9 ZEROS
                Asset_Transfer_ID = "T000000001";
            }
            else
            {
                //9 ZEROS
                var res = MaxID + 1;
                Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#000000000");
            }

            string ToDestination = "";

            ToDestination = cboLoc.SelectedItem.ToString() + "->" + cboBuild.SelectedItem.ToString() + "->" + cboFloor.SelectedItem.ToString();

            int IsApprovalNeeded = objVer.GetIsApprovalNeeded();
            if (cboLoc.SelectedItem.ToString() == Dispose)
            {
                ToDestination = cboLoc.SelectedItem.ToString() + "->No " + Building + "->No " + Floor;

                objVer.SendAssets_For_Approval(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, txtReasonLocChange.Text, ToDestination, cboLoc.SelectedValue.ToString(), "1", "1", Session["UserName"].ToString(), "Location Transfer", "", "", null, "Manual Transfer", HdnLocation.Value);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset sent for approval successfully.');", true);


                string Messages = "";
                if (IsApprovalNeeded == 1)
                {
                    Messages = Assets + " sent for approval successfully.";
                }
                else
                {
                    Messages = Assets + " Transfered successfully.";
                }
                //imgpopup.ImageUrl = "images/Success.png";
                //lblpopupmsg.Text = Messages;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + Messages + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                //objVer.SaveAssetTransferDetails_Manual(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, "Manual Transfer", ToDestination, cboLoc.SelectedValue.ToString(), "1", "1", Session["UserName"].ToString());
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
            }
            else
            {
                objVer.SendAssets_For_Approval(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, txtReasonLocChange.Text, ToDestination, cboLoc.SelectedValue.ToString(), cboBuild.SelectedValue.ToString(), cboFloor.SelectedValue.ToString(), Session["UserName"].ToString(), "Location Transfer", "", "", null, "Manual Transfer", HdnLocation.Value);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset sent for approval successfully');", true);


                string Messages = "";
                if (IsApprovalNeeded == 1)
                {
                    Messages = Assets + " sent for approval successfully.";
                }
                else
                {
                    Messages = Assets + " Transfered successfully.";
                }
                //imgpopup.ImageUrl = "images/Success.png";
                //lblpopupmsg.Text = Messages;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + Messages + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                //objVer.SaveAssetTransferDetails_Manual(dt_Transfer, Session["userid"].ToString(), Asset_Transfer_ID, "Manual Transfer", ToDestination, cboLoc.SelectedValue.ToString(), cboBuild.SelectedValue.ToString(), cboFloor.SelectedValue.ToString(), Session["UserName"].ToString());
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
            }

            //if (cboLoc.SelectedItem.ToString() == "Document Returned")
            //{
            DataTable dttableNew = dt_Transfer.Clone();

            foreach (DataRow drtableOld in dt_Transfer.Rows)
            {
                dttableNew.ImportRow(drtableOld);

            }
            for (int j = 0; j < dttableNew.Rows.Count; j++)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                string AssetID = dttableNew.Rows[j]["AssetID"].ToString();
                using (SqlCommand cmd = new SqlCommand("update AssetMaster set LocationInDateTime=@LocationInDateTime where AssetId=@AssetId", con))
                {
                    cmd.Parameters.AddWithValue("@LocationInDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@AssetId", AssetID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dttableNew.Columns.Remove("AssetID");
            dttableNew.Columns.Remove("TranID");
            dttableNew.Columns.Remove("CategoryId");
            dttableNew.Columns.Remove("CategoryCode");
            dttableNew.Columns.Remove("SubCatId");
            dttableNew.Columns.Remove("SubCategory");
            dttableNew.Columns.Remove("SubCatCode");
            dttableNew.Columns.Remove("LocationId");
            dttableNew.Columns.Remove("FloorId");
            dttableNew.Columns.Remove("BuildingId");
            dttableNew.Columns.Remove("CustodianId");
            dttableNew.Columns.Remove("DepartmentId");
            dttableNew.Columns.Remove("Department");
            dttableNew.Columns.Remove("AssetId1");
            dttableNew.Columns.Remove("Description");
            dttableNew.Columns.Remove("SerialNo");
            dttableNew.Columns.Remove("Price");
            dttableNew.Columns.Remove("Quantity");
            dttableNew.Columns.Remove("SupplierName");
            dttableNew.Columns.Remove("Column11");
            dttableNew.Columns.Remove("Column12");
            dttableNew.Columns.Remove("Column13");
            dttableNew.Columns.Remove("Column14");
            dttableNew.Columns.Remove("AssignDate");
            dttableNew.Columns.Remove("TagType");

            dttableNew.Columns["AssetCode"].SetOrdinal(0);
            dttableNew.Columns["Column2"].SetOrdinal(1);
            dttableNew.Columns["Column3"].SetOrdinal(2);
            dttableNew.Columns["Column1"].SetOrdinal(3);
            dttableNew.Columns["Category"].SetOrdinal(4);
            dttableNew.Columns["Location"].SetOrdinal(5);
            dttableNew.Columns["Building"].SetOrdinal(6);
            dttableNew.Columns["Floor"].SetOrdinal(7);
            dttableNew.Columns["Custodian"].SetOrdinal(8);
            dttableNew.Columns["DeliveryDate"].SetOrdinal(9);
            dttableNew.Columns["Status"].SetOrdinal(10);
            dttableNew.Columns["Column4"].SetOrdinal(11);
            dttableNew.Columns["Column5"].SetOrdinal(12);
            dttableNew.Columns["Column6"].SetOrdinal(13);
            dttableNew.Columns["Column7"].SetOrdinal(14);
            dttableNew.Columns["Column8"].SetOrdinal(15);
            dttableNew.Columns["Column9"].SetOrdinal(16);
            dttableNew.Columns["Column10"].SetOrdinal(17);

            dttableNew.Columns["AssetCode"].ColumnName = "DOCUMENT CODE";
            dttableNew.Columns["Category"].ColumnName = "CATEGORY";
            dttableNew.Columns["Location"].ColumnName = "MAJOR LOCATION";
            dttableNew.Columns["Floor"].ColumnName = "MINOR SUB LOCATION";
            dttableNew.Columns["Building"].ColumnName = "MINOR LOCATION";
            dttableNew.Columns["Custodian"].ColumnName = "CUSTODIAN";
            dttableNew.Columns["DeliveryDate"].ColumnName = "DELIVERY DATE";
            dttableNew.Columns["Status"].ColumnName = "STATUS";
            dttableNew.Columns["Column1"].ColumnName = "FC NUMBER";
            dttableNew.Columns["Column2"].ColumnName = "CASE ASSIGNEE NAME";
            dttableNew.Columns["Column3"].ColumnName = "CLIENT NAME";
            dttableNew.Columns["Column4"].ColumnName = "DOCUMENT CONTROLLER NAME";
            dttableNew.Columns["Column5"].ColumnName = "CASE MANAGER FULL NAME";
            dttableNew.Columns["Column6"].ColumnName = "CASE MANAGER EMAIL";
            dttableNew.Columns["Column7"].ColumnName = "CASE WORKER NAME";
            dttableNew.Columns["Column8"].ColumnName = "CASE WORKER EMAIL";
            dttableNew.Columns["Column9"].ColumnName = "CASE STATUS";
            dttableNew.Columns["Column10"].ColumnName = "CASE PERSON ASSOCIATION";
            string[] TobeDistinct = { "CASE WORKER EMAIL", "CASE WORKER NAME" };
            DataTable dtDistinct = GetDistinctRecords(dttableNew, TobeDistinct);
           // string excelsheetname = excelg(dttableNew);
            string pdfname = pdfGenerate(dttableNew);
            for (int z = 0; z < dtDistinct.Rows.Count; z++)
            {
                string toemailid = dtDistinct.Rows[z].ItemArray[0].ToString();
                string name = dtDistinct.Rows[z].ItemArray[1].ToString();
                SendEmail(toemailid, name, pdfname);
            }
            string usremail = findLoggedInUserEmail();
            string[] usrdet = usremail.Split(',');
            SendEmail(usrdet[1].ToString(), usrdet[0].ToString(), pdfname);
            //}
            grid_view();
            gvData.DataBind();

            cboLoc.SelectedIndex = 0;
            cboBuild.Items.Clear();
            cboBuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            cboFloor.Items.Clear();
            cboFloor.Items.Insert(0, new ListItem("--Select--", "0", true));
            txtReasonLocChange.Text = "";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "Button1_Click", path);
        }
    }
    private string pdfGenerate(DataTable dataTable)
    {
        try
        {

            StringBuilder html = new StringBuilder();
            //html.Append("<html><head><style> .tbl{ border: 1px solid black; } .tablerow{ text-align:center; font - size:11px; } #trheader{ text-align:center; background-color:#d8d8d8!important;font-size:12px;}</style>");
            html.Append("<html><head>");
            html.Append("</head>");
            html.Append("<body>");




            html.Append("<table style='width: 100%'>");
            html.Append("<tr style='' class='bgcol'> ");
            html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DOCUMENT CODE</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CATEGORY</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MAJOR LOCATION</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CUSTODIAN</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE ASSIGNEE NAME</th>");//-----------------Static


            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CLIENT NAME</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>FC NUMBER</th>");//-----------------Static


            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MINOR LOCATION</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MINOR SUB LOCATION</th>");//-----------------Static

            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DELIVERY DATE</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>STATUS</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DOCUMENT CONTROLLER NAME</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE MANAGER FULL NAME</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE MANAGER EMAIL</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE WORKER NAME</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE WORKER EMAIL</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE STATUS</th>");//-----------------Static
            //html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE PERSON ASSOCIATION</th>");//-----------------Static
            html.Append("</tr>");
            //----------------------------Dynamic Binding Start
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                html.Append("<tr>");
                html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DOCUMENT CODE"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CATEGORY"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MAJOR LOCATION"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CUSTODIAN"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE ASSIGNEE NAME"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CLIENT NAME"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["FC NUMBER"].ToString() + "</td>");              
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MINOR LOCATION"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MINOR SUB LOCATION"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DELIVERY DATE"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["STATUS"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DOCUMENT CONTROLLER NAME"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE MANAGER FULL NAME"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE MANAGER EMAIL"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE WORKER NAME"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE WORKER EMAIL"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE STATUS"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE PERSON ASSOCIATION"].ToString() + "</td>");
                html.Append("</tr>");
            }
            html.Append("</table><br>");
            html.Append("</body></html>");






            iTextSharp.text.FontFactory.RegisterDirectories();

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.BORDER, "1");
            //styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FONTFAMILY, "Verdana");
            styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FACE, "Calibri");
            styles.LoadTagStyle("bgcol", "color", "#d8d8d8");
            styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TH, iTextSharp.text.html.HtmlTags.BGCOLOR, "#d8d8d8");
            // string path = AppDomain.CurrentDomain.BaseDirectory + "/AAF/";
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();



            using (iTextSharp.text.Document document1 = new iTextSharp.text.Document())
            {
                document1.SetPageSize(iTextSharp.text.PageSize._11X17);
                document1.SetMargins(10, 10, 10, 10);
                iTextSharp.text.pdf.PdfWriter writer1 = iTextSharp.text.pdf.PdfWriter.GetInstance(document1, mStream);
                document1.Open();
                //document1.Add(image);TransferredDocumentDetails-22092022075817
                document1.Add(new iTextSharp.text.Paragraph("\n"));
                iTextSharp.text.Paragraph Pr = new iTextSharp.text.Paragraph("\n TRANSFERRED DOCUMENT DETAILS", iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 16.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                Pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                iTextSharp.text.pdf.draw.LineSeparator underline = new iTextSharp.text.pdf.draw.LineSeparator(1, 40, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -3);
                Pr.Add(underline);
                document1.Add(Pr);
                List<iTextSharp.text.IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
               new StringReader(html.ToString()), styles
               );
                document1.Add(new iTextSharp.text.Paragraph("\n"));
                Pr = new iTextSharp.text.Paragraph(" ");

                foreach (iTextSharp.text.IElement element in objects)
                {
                    document1.Add(element);
                }
            }
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "TransferAssetData\\";
            string fileName = "TransferredDocumentDetails-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "");
            Response.Clear();
            Response.BinaryWrite(mStream.ToArray());
            File.WriteAllBytes((folderPath + fileName), mStream.ToArray());
            return fileName;

            //var fileStream = new FileStream((folderPath+ fileName), FileMode.Open, FileAccess.Read);
            //fileStream.CopyTo(mStream);
            //return folderPath + fileName;
            //Response.End();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "GeneratePDF", path);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.ToString() + "')", true);
            return "";
            //Response.Redirect("RAssetMovement.aspx");
            //GriddetailsPopup.Hide();
        }
        return "";
    }
    //private string pdfGenerate(DataTable dataTable)
    //{
    //    try
    //    {

    //        StringBuilder html = new StringBuilder();
    //        //html.Append("<html><head><style> .tbl{ border: 1px solid black; } .tablerow{ text-align:center; font - size:11px; } #trheader{ text-align:center; background-color:#d8d8d8!important;font-size:12px;}</style>");
    //        html.Append("<html><head>");
    //        html.Append("</head>");
    //        html.Append("<body>");




    //        html.Append("<table style='width: 100%'>");
    //        html.Append("<tr style='' class='bgcol'> ");
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DOCUMENT CODE</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE ASSIGNEE NAME</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CLIENT NAME</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>FC NUMBER</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CATEGORY</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MAJOR LOCATION</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MINOR LOCATION</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>MINOR SUB LOCATION</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CUSTODIAN</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DELIVERY DATE</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>STATUS</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>DOCUMENT CONTROLLER NAME</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE MANAGER FULL NAME</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE MANAGER EMAIL</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE WORKER NAME</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE WORKER EMAIL</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE STATUS</th>");//-----------------Static
    //        html.Append("<th style='text-align:center;font-size:8px;font-weight: bold;'>CASE PERSON ASSOCIATION</th>");//-----------------Static
    //        html.Append("</tr>");
    //        //----------------------------Dynamic Binding Start
    //        for (int i = 0; i < dataTable.Rows.Count; i++)
    //        {
    //            html.Append("<tr>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DOCUMENT CODE"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE ASSIGNEE NAME"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CLIENT NAME"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["FC NUMBER"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CATEGORY"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MAJOR LOCATION"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MINOR LOCATION"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["MINOR SUB LOCATION"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CUSTODIAN"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DELIVERY DATE"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["STATUS"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["DOCUMENT CONTROLLER NAME"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE MANAGER FULL NAME"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE MANAGER EMAIL"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE WORKER NAME"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE WORKER EMAIL"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE STATUS"].ToString() + "</td>");
    //            html.Append("<td style='text-align:center;font-size:8px;'>" + dataTable.Rows[i]["CASE PERSON ASSOCIATION"].ToString() + "</td>");
    //            html.Append("</tr>");
    //        }
    //        html.Append("</table><br>");
    //        html.Append("</body></html>");






    //        iTextSharp.text.FontFactory.RegisterDirectories();

    //        iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
    //        styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.BORDER, "1");
    //        //styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FONTFAMILY, "Verdana");
    //        styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FACE, "Calibri");
    //        styles.LoadTagStyle("bgcol", "color", "#d8d8d8");
    //        styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TH, iTextSharp.text.html.HtmlTags.BGCOLOR, "#d8d8d8");
    //        // string path = AppDomain.CurrentDomain.BaseDirectory + "/AAF/";
    //        System.IO.MemoryStream mStream = new System.IO.MemoryStream();



    //        using (iTextSharp.text.Document document1 = new iTextSharp.text.Document())
    //        {
    //            document1.SetPageSize(iTextSharp.text.PageSize._11X17);
    //            document1.SetMargins(10, 10, 10, 10);
    //            iTextSharp.text.pdf.PdfWriter writer1 = iTextSharp.text.pdf.PdfWriter.GetInstance(document1, mStream);
    //            document1.Open();
    //            //document1.Add(image);TransferredDocumentDetails-22092022075817
    //            document1.Add(new iTextSharp.text.Paragraph("\n"));
    //            iTextSharp.text.Paragraph Pr = new iTextSharp.text.Paragraph("\n TRANSFERRED DOCUMENT DETAILS", iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 16.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
    //            Pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
    //            iTextSharp.text.pdf.draw.LineSeparator underline = new iTextSharp.text.pdf.draw.LineSeparator(1, 40, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -3);
    //            Pr.Add(underline);
    //            document1.Add(Pr);
    //            List<iTextSharp.text.IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
    //           new StringReader(html.ToString()), styles
    //           );
    //            document1.Add(new iTextSharp.text.Paragraph("\n"));
    //            Pr = new iTextSharp.text.Paragraph(" ");

    //            foreach (iTextSharp.text.IElement element in objects)
    //            {
    //                document1.Add(element);
    //            }
    //        }
    //        string folderPath = AppDomain.CurrentDomain.BaseDirectory + "TransferAssetData\\";
    //        string fileName = "TransferredDocumentDetails-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
    //        Response.ContentType = "application/octet-stream";
    //        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "");
    //        Response.Clear();
    //        Response.BinaryWrite(mStream.ToArray());
    //        File.WriteAllBytes((folderPath+fileName), mStream.ToArray());
    //        return fileName;

    //        //var fileStream = new FileStream((folderPath+ fileName), FileMode.Open, FileAccess.Read);
    //        //fileStream.CopyTo(mStream);
    //        //return folderPath + fileName;
    //        //Response.End();
    //    }
    //    catch (Exception ex)
    //    {
    //        Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "GeneratePDF", path);
    //        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.ToString() + "')", true);
    //        return "";
    //        //Response.Redirect("RAssetMovement.aspx");
    //        //GriddetailsPopup.Hide();
    //    }
    //    return "";
    //}
    //public string pdfGenerate(DataTable dt)
    //{
    //    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
    //    WebKitConverterSettings settings = new WebKitConverterSettings();
    //    //HTML string and base URL 
    //    string htmlText = "<html><body Align='Left'><br><p> <font size='12'>Hello World </p></font> </body></html>";
    //    string baseUrl = string.Empty;
    //    //Set WebKit path
    //    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "QtBinaries\\";
    //    settings.WebKitPath = folderPath;
    //    //Set the page orientation 
    //    settings.Orientation = PdfPageOrientation.Portrait;
    //    //Assign WebKit settings to HTML converter
    //    htmlConverter.ConverterSettings = settings;
    //    //Convert HTML to PDF
    //    PdfDocument document = htmlConverter.Convert(htmlText, baseUrl);
    //    //Save and close the PDF document
    //    string folderPathtoSave = AppDomain.CurrentDomain.BaseDirectory + "TransferAssetData\\";
    //    string fileName = "TransferredDocumentDetails-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
    //    document.Save(folderPathtoSave + fileName);
    //    document.Close(true);
    //    return "";
    //}
    public string excelg(DataTable dt)
    {
        try
        {
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "TransferAssetData\\";
            string fileName = "TransferredDocumentDetails-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "TransferredDocumentDetails");
                wb.SaveAs(folderPath + fileName);
            }
            return fileName;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "excelgen", path);
            return "";
        }
    }
    public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
    {
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
        return dtUniqRecords;
    }
    public void SendEmail(string emailid, string username, string ExcelFileNamePath)
    {
        try
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            EncryptManager em = new EncryptManager();
            string WebApiUrlApprove = "";
            SqlCommand select = new SqlCommand();
            select.CommandText = "select * from TagitEmailConfig where Application='tagit'";
            select.CommandType = CommandType.Text;
            select.Connection = conn;
            SqlDataAdapter daEmail = new SqlDataAdapter(select);
            DataTable dt_MailConfig = new DataTable();
            daEmail.Fill(dt_MailConfig);
            System.Net.Mail.Attachment attachment;
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(dt_MailConfig.Rows[0][1].ToString());
            message.Subject = "DOCUMENT DELIVERY " + DateTime.Now;
            message.To.Add(new MailAddress(emailid));
            message.IsBodyHtml = true;
            string bcc = "ragesh@tagitglobal.com";
            string[] bccid = bcc.Split(',');

            foreach (string bccEmailId in bccid)
            {
                message.Bcc.Add(new MailAddress(bccEmailId));
            }
            try
            {
                attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + "\\TransferAssetData\\" + ExcelFileNamePath);
                message.Attachments.Add(attachment);
            }
            catch
            {

            }
            string MessageBody = "";
            message.Body = @"
                <html>
                    <head>    
                    <title> Tagit DMS </title>
                    <meta xmlns = ""http://www.w3.org/1999/xhtml"" content = ""text /html; charset=utf-8"" />
                    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
                    <!-- jQuery library -->
                    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js""></script>
                    <!-- Latest compiled JavaScript -->
                    <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
                    </head>      
                    <body>
                        <div id = ""stylized"" class=""myform"">
                            <form
                                id = ""form""
                                action = ""#""
                                name =""form"" >                                                                                                                           
                            </form>
                <table>
				    <tr>
				    <td align=""center"" colspan=""3"">
                             
                    </td>
       
                    </tr>
       
                    <tr>
                        <td style=""width: 85%""><b>Dear " + username + @",	</b>
                        </td>
                        <td></td>
                        <td style=""width: 15%"" rowspan=""3"">
                            
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan=""4"">
                           New Document has been transferred .Kindly login to the application and check for Document Transfer Details.
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>&nbsp;</td>
                    </tr>  
                        <tr><td>** Please note that this email is auto-generated from Tagit DMS application.**</td><td></td></tr>
                       <tr>
                           <td>   
                               <br>   
                               <b> Thank You,</b><br>      
                               <br>
                                  Tagit Dms <br>      
                               <a href = ""https://www.tagitglobal.com/"" target =""_blank"" > www.tagitglobal.com</a>
                        </td>
                    </tr>
                </table>
                        </div>
                    </body>
                </html> ";


            smtp.Port = Convert.ToInt32(dt_MailConfig.Rows[0][4].ToString());
            smtp.Host = dt_MailConfig.Rows[0][3].ToString();
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(dt_MailConfig.Rows[0][1].ToString(), em.Decode(dt_MailConfig.Rows[0][2].ToString()));
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "SendEmail", path);
        }
    }
    public string findLoggedInUserEmail()
    {
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        using (SqlCommand cmd = new SqlCommand("select USER_NAME, EmailID from TBL_USERMST where USER_ID = @USER_ID", conn))
        {
            cmd.Parameters.AddWithValue("@USER_ID", Session["userid"].ToString());
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dt);
            }
            //Session["userid"]
            return dt.Rows[0]["USER_NAME"].ToString() + "," + dt.Rows[0]["EmailID"].ToString();
        }

    }
    protected void cboFromCustodian_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
            bindToCustodian(cboFromCustodian.SelectedItem.ToString());
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "cboFromCustodian_SelectedIndexChanged", path);
            ex.Message.ToString();
        }

    }

    //protected void BtnByLocation_Click(object sender, EventArgs e)
    //{
    //    //divbyLocation.Visible = true;
    //    //divbyCustodian.Visible = false;
    //}

    //protected void BtnByCustodian_Click(object sender, EventArgs e)
    //{
    //    //divbyLocation.Visible = false;
    //    //divbyCustodian.Visible = true;
    //}


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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_ItemDataBound", path);
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


                CheckBox chk1 = (CheckBox)item.FindControl("checkAll");
                HiddenField3.Value = chk1.ClientID.ToString();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "gvData_ItemCreated", path);
        }
    }
    static string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
    static string Maplocation = string.Empty;

    public static void RetrieveFormatedAddress(string lat, string lng)
    {
        try
        {
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where elm.Name == "formatted_address"
                               select elm).FirstOrDefault();
                    requestUri = res.Value;
                    Maplocation = res.Value;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "RetrieveFormatedAddress", path);
        }
    }

    public void GetGPSLocation()
    {
        try
        {
            string x = HdnError.Value;
            if (HdnLatValue.Value != "" && HdnLogValue.Value != "")
            {
                try
                {
                    RetrieveFormatedAddress((HdnLatValue.Value).ToString(), (HdnLogValue.Value).ToString());
                }
                catch { }
                HdnLocation.Value = Maplocation;
                if (HdnLocation.Value != "")
                {
                    HdnLastLocation.Value = HdnLocation.Value;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "GetGPSLocation", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TransferAssets.aspx", "GetGrid_Click", path);
        }
    }

    protected void fetchgpsbtn_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "getLocation();", true);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["locationparam"].Value != null)
            {
                string loca = Request.Cookies["locationparam"].Value;
                //txtloca.Text = loca;
            }

        }
        catch (Exception ex)
        {

        }
    }
}