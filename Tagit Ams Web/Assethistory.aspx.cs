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

public partial class Assethistory : System.Web.UI.Page
{

    public static DataTable dt_rpt = new DataTable();
    DataTable dt_grdDetails = new DataTable();
    public static string path = "";
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    CompanyBL objcomp = new CompanyBL();
    //objcomp.Insertlogmaster("AssetHistory:Grid Loading Ended");
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];

    //public DataTable dt_grdDetails
    //{
    //    get
    //    {
    //        return ViewState["dt_grdDetails"] as DataTable;
    //    }
    //    set
    //    {
    //        ViewState["dt_grdDetails"] = value;

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gvData_Init", path);
        }
    }

    public DataTable dt_result
    {
        get
        {
            return ViewState["dt_result"] as DataTable;
        }
        set
        {
            ViewState["dt_result"] = value;

        }
    }

    private void bindType()
    {
        try
        {
            DataTable dt_Type = new DataTable();
            dt_Type.Columns.Add("Type");

            dt_Type.Rows.Add("Category Variance");
            dt_Type.Rows.Add("Mismatch Variance");


            DdlType.DataSource = dt_Type;
            DdlType.DataTextField = "Type";
            DdlType.DataValueField = "Type";
            DdlType.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "bindType", path);
        }
    }

    // Check User is Authorize to view this page
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
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
                    bindType();

                    txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    bindstatus();
                    lblTotHeader.Visible = false;
                    Bincategory();
                    bindCustodian();
                    SetGridOrder();
                }
                else
                {
                    divSearch.Style.Add("display", "none");
                    bindType();

                    txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    bindstatus();
                    lblTotHeader.Visible = false;
                    Bincategory();
                    bindCustodian();
                    SetGridOrder();
                    Response.Redirect("AcceessError.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "Page_Load", path);
        }
    }
    public void SetGridOrder()
    {
        try
        {
            if (_Order == "true")
            {
                foreach (Telerik.Web.UI.GridColumn col in gv_Popup.MasterTableView.RenderColumns)
                {
                    if (col.UniqueName.Equals("Column1"))
                    {
                        col.OrderIndex = 3;
                    }
                    else if (col.UniqueName.Equals("Column2"))
                    {
                        col.OrderIndex = 3;
                    }
                    else if (col.UniqueName.Equals("Column3"))
                    {
                        col.OrderIndex = 3;
                    }
                }
                gv_Popup.Rebind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "SetGridOrder", path);
        }
    }

    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void Bincategory()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            ddlCategorys.DataSource = ds;
            ddlCategorys.DataTextField = "CategoryName";
            ddlCategorys.DataValueField = "CategoryID";
            ddlCategorys.DataBind();
            ddlCategorys.Items.Insert(0, new ListItem("--Category--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "Bincategory", path);
        }
    }

    private void bindCustodian()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCustodian");
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("select cm.CustodianId ,cm.CustodianName from CustodianMaster as cm left join CustodianPermission as cp on cp.CustodianId=cm.CustodianId where cp.UserID=@UserID and cm.Active=1", con))
            {
                cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCustodian.DataSource = ds;
                ddlCustodian.DataTextField = "CustodianName";
                ddlCustodian.DataValueField = "CustodianId";
                ddlCustodian.DataBind();
                ddlCustodian.Items.Insert(0, new ListItem("--Custodian--", "0", true));
            }
            else
            {
                ddlCustodian.DataSource = null;
                ddlCustodian.DataBind();
                ddlCustodian.Items.Insert(0, new ListItem("--Custodian--", "0", true));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "bindCustodian", path);
        }
    }

    private void bindstatus()
    {
        try
        {
            DataTable dt_status = new DataTable();
            dt_status.Columns.Add("Status");

            dt_status.Rows.Add("All");
            dt_status.Rows.Add("Missing");
            dt_status.Rows.Add("Found");
            dt_status.Rows.Add("Extra");
            dt_status.Rows.Add("Mismatch");

            ddlStatus.DataSource = dt_status;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "Status";
            ddlStatus.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "bindstatus", path);
        }
    }

    protected void btnSearchPop_Click(object sender, EventArgs e)
    {
        try
        {
            grid_view_Popup();
            gv_Popup.DataBind();
            GriddetailsPopup.Show();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "btnSearchPop_Click", path);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
            if (gvData.Items.Count == 0)
            {
                //string Message = "No records found..";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('No records found!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "btnSearch_Click", path);
        }
    }

    private void grid_view()
    {
        try
        {
            //objcomp.Insertlogmaster("AssetHistory:Grid Loading Started");
            string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
            FromDate = FromDate == null ? ToDate = null : FromDate;
            ToDate = ToDate == null ? FromDate = null : ToDate;

            ReportBL objReport = new ReportBL();
            DataSet ds = objReport.GetAssetHistry(FromDate, ToDate, DdlType.SelectedValue.ToString(), Session["userid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //gridlist.DataSource = ds;
                    //gridlist.DataBind();
                    gvData.DataSource = ds;
                    lblTotHeader.Visible = true;

                    if (DdlType.SelectedValue.ToString() == "Mismatch Variance")
                    {
                        gvData.Columns[4].Visible = false;
                        gvData.Columns[5].Visible = false;
                        gvData.Columns[6].Visible = true;
                        gvData.Columns[7].Visible = true;
                    }
                    else
                    {
                        gvData.Columns[4].Visible = true;
                        gvData.Columns[5].Visible = true;
                        gvData.Columns[6].Visible = false;
                        //gvData.Columns[7].Visible = false;
                    }
                }
                else
                {
                    lblTotHeader.Visible = false;
                    gvData.DataSource = string.Empty;
                    gv_Popup.DataSource = string.Empty;
                    gv_Popup.DataBind();
                }
            }
            else
            {
                //gridlist.DataSource = null;
                //gridlist.DataBind();
                lblTotHeader.Visible = false;
                gvData.DataSource = string.Empty;
                gv_Popup.DataSource = string.Empty;
                gv_Popup.DataBind();
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);

                //string Message = "No records found..";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('No records found!!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }


            objcomp.Insertlogmaster("AssetHistory:Grid Loading Ended");
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "grid_view", path);
        }
    }

    private void grid_view_Popup()
    {
        string Status = (ddlStatus.SelectedValue == "All") ? null : ddlStatus.SelectedValue;
        string Category = (ddlCategorys.SelectedValue == "0") ? null : ddlCategorys.SelectedValue;
        string Custodian = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;

        try
        {
            string fhty = Session["veriId"].ToString();
            DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.AssetVerificationreportSearch, new SqlParameter[] {
                      new SqlParameter("@Status",  Status),
                      new SqlParameter("@Category",  Category),
                       new SqlParameter("@Custodian",  Custodian),
                        new SqlParameter("@VID",  Session["veriId"].ToString()),
                            });
            DataSet ds = help.ExecuteDataset();
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count == 0)
            {
                dt_rpt = new DataTable();
                gv_Popup.DataSource = dt_rpt;
                gv_Popup.DataBind();
                gv_Popup.Visible = false;
            }
            else
            {
                dt_rpt = new DataTable();
                dt_rpt = ds.Tables[0];
                gv_Popup.DataSource = dt_rpt;
                gv_Popup.Visible = true;
                if (Status == "Mismatch")
                {
                    gv_Popup.Columns[4].Visible = false;
                    gv_Popup.Columns[5].Visible = false;
                }
                else
                {
                    //gv_Popup.Columns[4].Visible = false;
                    //gv_Popup.Columns[5].Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "grid_view_Popup", path);
            ////lblMsg.Visible = true;
            ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }

    protected void BindGridOnStatusChange(object sender, EventArgs e)
    {



        //try
        //{
        //    if (ddlStatus.SelectedValue == "All")
        //    {

        //        gv_Popup.DataSource = dt_grdDetails;
        //        gv_Popup.DataBind();
        //        ////LblDetails.Text = dt_grdDetails.Rows.Count.ToString();
        //        gv_Popup.Columns[5].Visible = false;
        //        gv_Popup.Columns[6].Visible = false;
        //        gv_Popup.Columns[7].Visible = true;
        //        gv_Popup.Columns[8].Visible = true;
        //        gv_Popup.Columns[9].Visible = true;
        //        gv_Popup.Columns[10].Visible = true;
        //        gv_Popup.Columns[11].Visible = true;
        //        gv_Popup.Columns[12].Visible = true;
        //        gv_Popup.Columns[13].Visible = true;
        //        GriddetailsPopup.Show();
        //    }
        //    else
        //    {
        //        DataView dv = new DataView(dt_grdDetails);
        //        if (ddlStatus.SelectedValue.ToString() == "Mismatch")
        //        {
        //            dv.RowFilter = "Status='" + ddlStatus.SelectedValue.ToString() + "'";

        //            gv_Popup.DataSource = dv;
        //            gv_Popup.DataBind();
        //            gv_Popup.Columns[5].Visible = true;
        //            gv_Popup.Columns[6].Visible = true;
        //            gv_Popup.Columns[7].Visible = false;
        //            gv_Popup.Columns[8].Visible = false;
        //            gv_Popup.Columns[9].Visible = false;
        //            gv_Popup.Columns[10].Visible = false;
        //            gv_Popup.Columns[11].Visible = false;
        //            gv_Popup.Columns[12].Visible = false;
        //            gv_Popup.Columns[13].Visible = false;

        //            GriddetailsPopup.Show();

        //        }
        //        else
        //        {
        //            dv.RowFilter = "Status='" + ddlStatus.SelectedValue.ToString() + "'";

        //            gv_Popup.DataSource = dv;
        //            gv_Popup.DataBind();
        //            gv_Popup.Columns[5].Visible = false;
        //            gv_Popup.Columns[6].Visible = false;
        //            gv_Popup.Columns[7].Visible = true;
        //            gv_Popup.Columns[8].Visible = true;
        //            gv_Popup.Columns[9].Visible = true;
        //            gv_Popup.Columns[10].Visible = true;
        //            gv_Popup.Columns[11].Visible = true;
        //            gv_Popup.Columns[12].Visible = true;
        //            gv_Popup.Columns[13].Visible = true;

        //            GriddetailsPopup.Show();
        //        }

        //        //LblDetails.Text = dv.Count.ToString();
        //    }

        //}
        //catch (Exception ex)
        //{

        //    throw;
        //}
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "PrepareForExport", path);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            txtFrmDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            //gridlist.DataSource = null;
            //gridlist.DataBind();
            gvData.DataSource = string.Empty;
            gvData.DataBind();
            gv_Popup.DataSource = string.Empty;
            gv_Popup.DataBind();
            lblTotHeader.Visible = false;
            DdlType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "btnRefresh_Click", path);
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            //PrepareForExport(gridDetails);
            ExportToExcel();
            grid_view_Popup();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "BtnExportExcel_Click", path);
        }
    }

    private void ExportToExcel()
    {
        try
        {
            DataTable DT2 = new DataTable();
            if (gv_Popup.Items.Count > 0)
            {
                //Response.Clear();
                //Response.Clear();
                //Response.AddHeader("content-disposition",
                //                      "attachment;filename=AssetHistory.xls");
                //Response.Charset = String.Empty;
                //Response.ContentType = "application/ms-excel";
                //StringWriter stringWriter = new StringWriter();
                //HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
                //gridDetails.AllowPaging = false;
                //gridDetails.DataSource = this.dt_grdDetails;
                //gridDetails.DataBind();
                //gridDetails.RenderControl(HtmlTextWriter);
                //Response.Write(stringWriter.ToString());
                //Response.End();

                if (dt_rpt.Columns.Contains("VID"))
                {
                    dt_rpt.Columns.Remove("VID");
                    dt_rpt.Columns.Remove("FromLocation");
                    dt_rpt.Columns.Remove("ToLocation");
                }

                string status = ddlStatus.SelectedItem.ToString();

                AssetBL objAsset = new AssetBL();
                List<MappingInfo> clientColumns = new List<MappingInfo>();
                clientColumns = objAsset.GetMappingListFromDB();

                try { dt_rpt.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets); } catch { }
                try { dt_rpt.Columns["LocationName"].ColumnName = Location; } catch { }
                try { dt_rpt.Columns["BuildingName"].ColumnName = Building; } catch { }
                try { dt_rpt.Columns["FloorName"].ColumnName = Floor; } catch { }
                try { dt_rpt.Columns["CategoryName"].ColumnName = Category; } catch { }
                try { dt_rpt.Columns["SubCatName"].ColumnName = SubCategory; } catch { }

                foreach (DataColumn col in dt_rpt.Columns)
                {
                    if (col.ColumnName.Contains("Column"))
                    {
                        var clientValue = clientColumns.Where(c => c.ColumnName == col.ColumnName.ToString().Trim());
                        foreach (var a in clientValue)
                        {
                            if (a.MappingColumnName != null && a.MappingColumnName != "")
                            {
                                dt_rpt.Columns[a.ColumnName].ColumnName = a.MappingColumnName + "#";
                            }
                        }
                    }
                }
                DT2 = dt_rpt.Copy();
                foreach (DataColumn column in DT2.Columns)
                {
                    if (column.ColumnName.ToString().Contains("Column"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("SerialNo"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Price"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Sub Category"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Type"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("StartDate"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("ENDDate"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("DepartmentName"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("VerificationID"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Major Location"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Minor Location"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                    if (column.ColumnName.ToString().Contains("Minor Sub Location"))
                    {
                        dt_rpt.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }






                }


                dt_rpt.Columns["DocumentCode"].SetOrdinal(0);
                dt_rpt.Columns["Category"].SetOrdinal(1);
                dt_rpt.Columns["Status"].SetOrdinal(2);
                dt_rpt.Columns["CustodianName"].SetOrdinal(3);
                dt_rpt.Columns["Reason"].SetOrdinal(4);
                dt_rpt.Columns["FC Number#"].SetOrdinal(5);
                dt_rpt.Columns["Case Assignee Name#"].SetOrdinal(6);
                dt_rpt.Columns["Client Name#"].SetOrdinal(7);
                dt_rpt.Columns["ImageName"].SetOrdinal(8);

                DataView dv = new DataView(dt_rpt);
                GridView GridView2 = new GridView();
                if (ddlStatus.SelectedValue.ToString() != "All")
                {
                    dv.RowFilter = "Status='" + ddlStatus.SelectedValue.ToString() + "'";


                    GridView2.AllowPaging = false;
                    GridView2.DataSource = dv;
                    GridView2.DataBind();
                }
                else
                {

                    GridView2.AllowPaging = false;
                    GridView2.DataSource = dt_rpt;
                    GridView2.DataBind();
                }



                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AssetHistory.xls");
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
                // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);

                //string Message = "No records found..";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('No records found!!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                GriddetailsPopup.Show();

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "ExportToExcel", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gvData_NeedDataSource", path);
        }
    }

    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "VerificationID")
            {
                ddlStatus.SelectedIndex = -1;
                ddlCategorys.SelectedIndex = -1;
                ddlCustodian.SelectedIndex = -1;

                Session["veriId"] = "";
                this.dt_grdDetails = null;

                GridDataItem item = (GridDataItem)e.Item;
                string HdnVerID = item["VID"].Text;
                Session["veriId"] = HdnVerID;
                LblDoneBy.Text = "DoneBy: " + item["USER_NAME"].Text.ToString();
                lblDate.Text = Convert.ToDateTime(item["CreatedDate"].Text).ToString("dd-MM-yyyy");
                lblLocation.Text = "Location:&nbsp;&nbsp;&nbsp; " + item["CurrentLocation"].Text.ToString();
                ReportBL objReport = new ReportBL();
                DataSet ds = objReport.GetAssetVerificationDetailsByID(HdnVerID);
                dt_rpt = new DataTable();
                dt_rpt = ds.Tables[0];
                this.dt_grdDetails = ds.Tables[0];

                gv_Popup.Visible = true;
                gv_Popup.DataSource = dt_rpt;
                gv_Popup.DataBind();
                gv_Popup.Columns[4].Visible = false;
                gv_Popup.Columns[5].Visible = false;



                DataSet ds_Summary = objReport.GetAssetVerificationDetailsByID_Summary(HdnVerID);
                DataTable dt_Summary = new DataTable();
                dt_Summary = ds_Summary.Tables[0];
                if (dt_Summary.Rows.Count > 0)
                {
                    DataRow[] dr_Found = dt_Summary.Select("status='Found'");
                    DataRow[] dr_Missing = dt_Summary.Select("status='Missing'");
                    DataRow[] dr_Extra = dt_Summary.Select("status='Extra'");
                    DataRow[] dr_Mismatch = dt_Summary.Select("status='Mismatch'");

                    if (dr_Missing.Length > 0)
                    {
                        lblmissing.Text = dr_Missing[0][1].ToString();
                    }
                    else
                    {
                        lblmissing.Text = "0";
                    }

                    if (dr_Found.Length > 0)
                    {
                        lblFound.Text = dr_Found[0][1].ToString();
                    }
                    else
                    {
                        lblFound.Text = "0";
                    }

                    if (dr_Extra.Length > 0)
                    {
                        lblExtra.Text = dr_Extra[0][1].ToString();
                    }
                    else
                    {
                        lblExtra.Text = "0";
                    }

                    if (dr_Mismatch.Length > 0)
                    {
                        lblMismatch.Text = dr_Mismatch[0][1].ToString();
                    }
                    else
                    {
                        lblMismatch.Text = "0";
                    }
                }

                GriddetailsPopup.Show();
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gv_data_ItemCommand", path);
        }
        finally
        {

        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;
                dataBoundItem["VerificationID"].ForeColor = Color.BlueViolet;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gvData_ItemDataBound", path);
        }
    }


    protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
    {
        try
        {
            this.UpdateToolTip(args.Value, args.UpdatePanel);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "OnAjaxUpdate", path);
        }
    }

    private void UpdateToolTip(string elementID, UpdatePanel panel)
    {
        try
        {
            Control ctrl = Page.LoadControl("ProductDetailsCS.ascx");
            ctrl.ID = "UcProductDetails1";
            panel.ContentTemplateContainer.Controls.Add(ctrl);
            usercontrol_ProductDetailsCS details = (usercontrol_ProductDetailsCS)ctrl;
            details.ImageName = elementID;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "UpdateToolTip", path);
        }
    }

    protected void grid_view_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
            {
                RadToolTipManager1.TargetControls.Clear();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "grid_view_ItemCommand", path);
        }
    }

    protected void grid_view_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null))
                {
                    if (!Object.Equals(this.RadToolTipManager1, null))
                    {
                        string t = item["ImageName"].Text.ToLower();
                        this.RadToolTipManager1.TargetControls.Add(target.ClientID, t, true);

                    }
                }
            }
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["LocationName"].Text = Location.ToUpper();
                item["Buildingname"].Text = Building.ToUpper();
                item["FloorName"].Text = Floor.ToUpper();
                item["CategoryName"].Text = Category.ToUpper();
                item["SubCatName"].Text = SubCategory.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "grid_view_ItemDataBound", path);
        }
    }


    protected void grid_view_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["LocationName"].Text = Location.ToUpper();
                item["Buildingname"].Text = Building.ToUpper();
                item["FloorName"].Text = Floor.ToUpper();
                item["CategoryName"].Text = Category.ToUpper();
                item["SubCatName"].Text = SubCategory.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "grid_view_ItemCreated", path);
        }
    }

    protected void gv_Popup_DataBinding(object sender, EventArgs e)
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
                    if (ListMapping.Any(L => L.ColumnName == "Column" + i.ToString()) == true && i <= 3)
                    {
                        string MapSerialName = ListMapping.Where(x => x.ColumnName == "Column" + i.ToString()).SingleOrDefault().MappingColumnName;
                        gv_Popup.MasterTableView.GetColumn("column" + i.ToString()).HeaderText = MapSerialName.ToUpper();

                        DataRow[] dr = dt_Col.Select("FieldName='Column" + i.ToString() + "' and printStatus='1'");
                        if (dr.Length == 0)
                        {
                            //gv_Popup.MasterTableView.GetColumn("column" + i.ToString()).Display = true;
                        }
                        i = i + 1;
                    }
                }
            }

            foreach (GridColumn column in gv_Popup.Columns)
            {
                if (column.HeaderText.ToString().Contains("Column"))
                {
                    column.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Assethistory.aspx", "gv_Popup_DataBinding", path);
        }
    }
}