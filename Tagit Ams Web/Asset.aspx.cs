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
using System.Text;
using System.Data.OleDb;
using ECommerce.Utilities;
using ECommerce.Common;




public partial class Asset : System.Web.UI.Page
{

    public static DataTable dt = new DataTable();
    public static DataTable Exceldt = new DataTable();

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    bool _MappingExist = false;
    public static string path = "";
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    public String WarrantyAMC = System.Configuration.ConfigurationManager.AppSettings["WarrantyAMC"];
    public String DocumentUpdate = System.Configuration.ConfigurationManager.AppSettings["DocumentUpdate"];
    public bool MappingExist
    {
        get
        {
            return _MappingExist;
        }
        set
        {
            _MappingExist = value;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_Init", path);

        }
    }

    public List<string> ExcelColumns
    {
        get
        {
            return ViewState["ExcelColumns"] as List<string>;
        }
        set
        {
            ViewState["ExcelColumns"] = value;
        }
    }
    public DataTable dt_cat
    {
        get
        {
            return ViewState["dt_cat"] as DataTable;
        }
        set
        {
            ViewState["dt_cat"] = value;

        }
    }
    public DataTable dt_subcat
    {
        get
        {
            return ViewState["dt_subcat"] as DataTable;
        }
        set
        {
            ViewState["dt_subcat"] = value;

        }
    }
    public DataTable dt_Loc
    {
        get
        {
            return ViewState["dt_Loc"] as DataTable;
        }
        set
        {
            ViewState["dt_Loc"] = value;

        }
    }
    public DataTable dt_Build
    {
        get
        {
            return ViewState["dt_Build"] as DataTable;
        }
        set
        {
            ViewState["dt_Build"] = value;

        }
    }
    public DataTable dt_floor
    {
        get
        {
            return ViewState["dt_floor"] as DataTable;
        }
        set
        {
            ViewState["dt_floor"] = value;

        }
    }
    public DataTable dt_department
    {
        get
        {
            return ViewState["dt_department"] as DataTable;
        }
        set
        {
            ViewState["dt_department"] = value;

        }
    }
    public DataTable dt_custodian
    {
        get
        {
            return ViewState["dt_custodian"] as DataTable;
        }
        set
        {
            ViewState["dt_custodian"] = value;

        }
    }
    public DataTable dt_supplier
    {
        get
        {
            return ViewState["dt_supplier"] as DataTable;
        }
        set
        {
            ViewState["dt_supplier"] = value;

        }
    }
    public string FileName
    {
        get
        {
            return ViewState["FileName"].ToString();
        }
        set
        {
            ViewState["FileName"] = value;
        }
    }

    public Boolean IsQuantitybase
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsQuantitybase"]);
        }
        set
        {
            ViewState["IsQuantitybase"] = value;
        }
    }
    public Boolean IsImport
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsImport"]);
        }
        set
        {
            ViewState["IsImport"] = value;
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
    //public DataTable dt_asset { get; set; }


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


    // Check User is Authorize to view this page
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    //protected override void OnLoad(EventArgs e)
    //{
    //    if (Session["userid"] == null)
    //    {
    //        Response.Redirect("Login.aspx");
    //    }
    //    if (userAuthorize((int)pages.AssetMaster, Session["userid"].ToString()) == false)
    //    {
    //        Response.Redirect("Home.aspx");
    //    }
    //    else
    //    {
    //        base.OnLoad(e);
    //    }
    //    //  base.OnLoad(e);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                divWarantyClose.Visible = false;
                divSearch.Style.Add("display", "none");
                divUpdateFields.Visible = false;
                txtValue.Visible = false;
                ddlstatus.Visible = false;
                if (WarrantyAMC == "true")
                    btnWarnaty.Visible = true;
                else
                    btnWarnaty.Visible = false;
                if (DocumentUpdate == "true")
                    btnUpdateInfo.Visible = true;
                else
                    btnUpdateInfo.Visible = false;
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.AssetMaster, Session["userid"].ToString()) == true)
                {
                    CompanyBL objcomp = new CompanyBL();

                    objcomp.Insertlogmaster("AssetMaster:Page Loaded");
                    DataSet ds = objcomp.getUserSetting();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.IsImport = ds.Tables[0].Rows[0]["ImportType"].ToString() == "1" ? true : false;
                        hdfImport.Value = Convert.ToString(IsImport);
                        this.IsQuantitybase = ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1" ? true : false;

                        if (ds.Tables[0].Rows[0]["ImportType"].ToString() == "1")
                        {
                            gvData.MasterTableView.GetColumn("Edit").Display = false;
                        }
                    }
                    else
                    {
                        this.IsImport = true;
                        hdfImport.Value = Convert.ToString(IsImport);
                        this.IsQuantitybase = true;
                    }
                    if (IsImport == true)//When import required
                    {
                        divAssetMaster.Visible = false;
                        divSelectFile.Visible = true;
                        divBrowse.Visible = true;
                        //SearchDiv.Visible = false;
                        //divImport.Visible = true;
                        // divUpload.Style.Add("Display", "block");

                        //btnsubmit.Visible = false;
                        //btnreset.Visible = false;
                        //btnImport.Visible = true;
                        //divFileSelect.Style.Add("Display", "block");
                        //divfileSpan.Style.Add("Display", "block");
                        btnImport.Visible = true;
                        //divUpload.Style.Add("Display", "inline");
                    }

                    if (IsImport == false)//When import not required
                    {
                        divAssetMaster.Visible = true;
                        divSelectFile.Visible = false;
                        divBrowse.Visible = false;
                        //SearchDiv.Visible = false;
                        //divImport.Visible = true;
                        // divUpload.Style.Add("Display", "block");

                        //btnsubmit.Visible = false;
                        //btnreset.Visible = false;
                        //btnImport.Visible = true;
                        //divFileSelect.Style.Add("Display", "block");
                        //divfileSpan.Style.Add("Display", "block");
                        btnImport.Visible = false;
                    }
                    DataSet dsuser = objcomp.getUserDetails(Session["userid"].ToString());
                    if (dsuser.Tables[1].Rows.Count > 0)
                    {
                        if (dsuser.Tables[1].Rows[0]["CanDelete"].ToString() == "Yes")
                        {
                            btnDelete.Visible = true;
                        }
                        else
                        {
                            btnDelete.Visible = false;
                        }
                    }
                    else
                    {
                        btnDelete.Visible = false;
                    }

                    // ddlsubcatSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlbuildSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloorSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

                    BincategorySearch();
                    Bincategory();
                    BindDepartmentSearch();
                    objcomp.Insertlogmaster("AssetMaster:Category Loaded");
                    // ddlsubcat.Items.Insert(0, new ListItem("--Select--", "1", true));
                    bindlocation();
                    bindlocationSearch();
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "1", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "1", true));
                    ddlcust.Items.Insert(0, new ListItem("--Select--", "1", true));
                    BindDepartment();
                    BindDepartmentSearch();
                    objcomp.Insertlogmaster("AssetMaster:Department Loaded");
                    BindSupplier();
                    bindCustodian();
                    txtdeldate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    txtass.Text = System.DateTime.Now.ToString("MM/dd/yyyy");



                    /*prashant*/
                    if (!string.IsNullOrEmpty(Session["Dashboard_Filtered_Location"] as string))
                    {
                        string value = Session["Dashboard_Filtered_Location"].ToString();
                        ListItem item = ddllocSearch.Items.FindByText(value);
                        if (item != null)
                        {
                            ddllocSearch.Items.FindByText(value).Selected = true;
                        }
                        else
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                            SqlCommand cmd = new SqlCommand("SELECT l.LocationName,l.LocationId,b.BuildingName FROM dbo.BuildingMaster b INNER JOIN dbo.LocationMaster l ON b.LocationId=l.LocationId WHERE b.BuildingName='" + value + "'", con);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string location_Name = dt.Rows[0]["LocationName"].ToString();
                                string location_id = dt.Rows[0]["LocationId"].ToString();
                                ddllocSearch.Items.FindByText(location_Name).Selected = true;


                                SqlDataAdapter dpt = new SqlDataAdapter();
                                DataAccessHelper1 help = new DataAccessHelper1(
                                StoredProcedures.Getbuilding, new SqlParameter[] {
                      new SqlParameter("@LocationId",  location_id),

                                            });
                                DataSet ds_bldg = help.ExecuteDataset();
                                ddlbuildSearch.DataSource = ds_bldg;
                                ddlbuildSearch.DataTextField = "BuildingName";
                                ddlbuildSearch.DataValueField = "BuildingId";
                                ddlbuildSearch.DataBind();
                                ddlbuildSearch.Items.Insert(0, new ListItem("--Select--", "0", true));

                                ddlbuildSearch.Items.FindByText(value).Selected = true;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Session["Dashboard_Filtered_Category"] as string))
                    {
                        ddlCategorySearch.Items.FindByText(Session["Dashboard_Filtered_Category"].ToString()).Selected = true;
                    }

                    if (!string.IsNullOrEmpty(Session["Dashboard_Filtered_Department"] as string))
                    {
                        ddldeptSearch.Items.FindByText(Session["Dashboard_Filtered_Department"].ToString()).Selected = true;
                    }
                    objcomp.Insertlogmaster("AssetMaster:Grid Loading Started");
                    grid_view();
                    gvData_DataBinding(sender, e);
                    objcomp.Insertlogmaster("AssetMaster:Grid Loading Completed");
                    SetGridOrder();

                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                    //ModalPopupExtender3.Show();
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "Page_Load", path);
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
                        col.OrderIndex = 6;
                    }
                    else if (col.UniqueName.Equals("Column2"))
                    {
                        col.OrderIndex = 6;
                    }
                    else if (col.UniqueName.Equals("Column3"))
                    {
                        col.OrderIndex = 6;
                    }
                }
                gvData.Rebind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "SetGridOrder", path);
        }
    }
    private void bind_Field()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetMappingListsForUpdate");
            if (ds != null && ds.Tables.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //if (dr["id"]. == 7)
                    //    dr.Delete();
                }
                dt.AcceptChanges();
            }

            cboField.DataSource = ds;
            cboField.DataTextField = "MappingColumnName";
            cboField.DataValueField = "ColumnName";
            cboField.DataBind();
            cboField.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "bind_Field", path);
        }
    }

    protected void cboField_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string value = cboField.SelectedValue;
            txtValue.Text = "";
            if (value == "Column9")
            {
                txtValue.Text = "";
                txtValue.Visible = false;
                ddlstatus.Visible = true;
                ddlstatus.SelectedValue = "0";
                //statusUpdate
            }
            else
            {
                txtValue.Text = "";
                txtValue.Visible = true;
                ddlstatus.Visible = false;
                ddlstatus.SelectedValue = "0";
            }
            if (value == "DepartmentName" || value == "CustodianName" || value == "SupplierName" || value == "TagType" || value == "FC Number")
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

            if (value == "TagType")
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                SqlDataAdapter dpt = new SqlDataAdapter();


                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTagType");

                cboValue.DataSource = ds;
                cboValue.DataTextField = "Name";
                cboValue.DataValueField = "Name";
                cboValue.DataBind();
                cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "cboField_SelectedIndexChanged", path);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlstatus.SelectedValue != "0")
            {
                DataTable dt_SelectedAsset = new DataTable();
                dt_SelectedAsset.Columns.Add("AssetCode");
                foreach (GridDataItem item in gvData.Items)
                {
                    HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                    CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                    if (((CheckBox)item.FindControl("cboxSelect")).Checked)
                    {
                        dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    }
                }

                if (dt_SelectedAsset.Rows.Count != 0)
                {
                    for (int i = 0; i < dt_SelectedAsset.Rows.Count; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("update AssetMaster  set Column9=@Column9 where AssetId=@AssetId", con))
                        {
                            cmd.Parameters.AddWithValue("@Column9", ddlstatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@AssetId", dt_SelectedAsset.Rows[i]["AssetCode"]);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                ddlstatus.SelectedValue = "0";
                //string Message = "Status updated successfully.";
                //imgpopup.ImageUrl = "images/Success.png";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender4.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Status updated successfully.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                grid_view();
                gvData.DataBind();
                gvData.Visible = true;
                return;
            }
            if (txtValue.Text == "" && cboValue.SelectedIndex <= 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('value should not be empty.');", true);
                return;
            }

            if (gvData.Visible == false)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No data available to update.');", true);
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select field to update.');", true);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    if (cboField.SelectedIndex >= 0)
                    {

                        DataTable dt_SelectedAsset = new DataTable();
                        dt_SelectedAsset.Columns.Add("AssetCode");
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
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select items..');", true);
                            //string Message = "Please select items..";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender4.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select Item!');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            return;
                        }

                        string output = "";
                        for (int i = 0; i < dt_SelectedAsset.Rows.Count; i++)
                        {
                            output = output + dt_SelectedAsset.Rows[i]["AssetCode"].ToString();
                            output += (i < dt_SelectedAsset.Rows.Count) ? "," : string.Empty;
                        }

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
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Assets updated successfully.');", true);

                                //string Message = Assets + " updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender4.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                                txtValue.Text = "";
                                cboField.SelectedIndex = -1;
                            }
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No data found to update.');", true);
                }

                grid_view();
                gvData.DataBind();
                confirmValue = "";
                gvData.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnUpdate_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "GetGrid_Click", path);
        }
    }
    //public void disablebrowserbackbutton()
    //{

    //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    HttpContext.Current.Response.Cache.SetNoServerCaching();
    //    HttpContext.Current.Response.Cache.SetNoStore();

    //}
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;


            string CategoryId = (ddlCategorySearch.SelectedValue == "0") ? null : ddlCategorySearch.SelectedValue;
            string SubCatId = null;
            string LocationId = (ddllocSearch.SelectedValue == "0") ? null : ddllocSearch.SelectedValue;
            string BuildingId = (ddlbuildSearch.SelectedValue == "0") ? null : ddlbuildSearch.SelectedValue;
            string FloorId = (ddlfloorSearch.SelectedValue == "0") ? null : ddlfloorSearch.SelectedValue;
            string DepartmentId = (ddldeptSearch.SelectedValue == "0") ? null : ddldeptSearch.SelectedValue;
            string AssetCode = (txtAssetCode.Text.ToString() == "") ? null : txtAssetCode.Text.ToString();
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();

            string CustodianId = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;

            try
            {
                DataSet ds = new DataSet();
                if (HttpContext.Current.Session["Dashboard_Filtered_Location"] != null)
                {
                    string Dashboard_Filtered_Location_statpage = null;
                    if (HttpContext.Current.Session["Dashboard_Filtered_Location_statpage"].ToString() == "1")
                    {
                        Dashboard_Filtered_Location_statpage = "1";
                    }
                    using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_top10Clients_newSP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                        cmd.Parameters.AddWithValue("@SubCatId", SubCatId);
                        cmd.Parameters.AddWithValue("@LocationId", LocationId);
                        cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                        cmd.Parameters.AddWithValue("@FloorId", FloorId);
                        cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                        cmd.Parameters.AddWithValue("@FromDate", null);
                        cmd.Parameters.AddWithValue("@Todate", null);
                        cmd.Parameters.AddWithValue("@AssetCode", AssetCode);
                        cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                        cmd.Parameters.AddWithValue("@SearchText", SearchText);
                        cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@Column3", HttpContext.Current.Session["Dashboard_Filtered_Location"].ToString());
                        cmd.Parameters.AddWithValue("@Column7", HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"].ToString());
                        cmd.Parameters.AddWithValue("@Dashboard_Filtered_Location_statpage", Dashboard_Filtered_Location_statpage);
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.SelectCommand.CommandTimeout = 600;
                            adp.Fill(ds);
                        }
                    }

                }
                else
                {
                    if (HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                            cmd.Parameters.AddWithValue("@SubCatId", SubCatId);
                            cmd.Parameters.AddWithValue("@LocationId", LocationId);
                            cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                            cmd.Parameters.AddWithValue("@FloorId", FloorId);
                            cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                            cmd.Parameters.AddWithValue("@FromDate", null);
                            cmd.Parameters.AddWithValue("@Todate", null);
                            cmd.Parameters.AddWithValue("@AssetCode", AssetCode);
                            cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                            cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                            cmd.Parameters.AddWithValue("@column7", HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"].ToString());
                            cmd.Parameters.AddWithValue("@SearchText", HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"].ToString());
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.SelectCommand.CommandTimeout = 600;
                                adp.Fill(ds);
                            }
                        }

                    }
                    else if (HttpContext.Current.Session["SessionofHealthDataColumn9"] != null)
                    {
                        string LocationIdd = null, BuildingIdd = null, FloorIdd = null, CaseManager = null, Column1FCNumberr = null, Column3ClientNamee = null, Column2AssigneeNamee = null, CustodianIdd = null;
                        if (HttpContext.Current.Session["LocationID"].ToString() != "0")
                        {
                            LocationIdd = HttpContext.Current.Session["LocationID"].ToString();
                        }
                        if (HttpContext.Current.Session["BuildingID"].ToString() != "0")
                        {
                            BuildingIdd = HttpContext.Current.Session["BuildingID"].ToString();
                        }
                        if (HttpContext.Current.Session["FloorId"].ToString() != "0")
                        {
                            FloorIdd = HttpContext.Current.Session["FloorId"].ToString();
                        }
                        if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
                        {
                            CaseManager = HttpContext.Current.Session["Column5CaseManager"].ToString();
                        }
                        if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
                        {
                            Column1FCNumberr = HttpContext.Current.Session["Column1FCNumber"].ToString();
                        }
                        if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
                        {
                            Column3ClientNamee = HttpContext.Current.Session["Column3ClientName"].ToString();
                        }
                        if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
                        {
                            Column2AssigneeNamee = HttpContext.Current.Session["Column2AssigneeName"].ToString();
                        }
                        if (HttpContext.Current.Session["CustodianId"].ToString() != "0")
                        {
                            CustodianIdd = HttpContext.Current.Session["CustodianId"].ToString();
                        }
                        if (HttpContext.Current.Session["SessionofHealthDataColumn9"].ToString() == "Not Started")
                        {
                            HttpContext.Current.Session["SessionofHealthDataColumn9"] = "Active";
                        }
                        using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_1_27082022", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CategoryId", null);
                            cmd.Parameters.AddWithValue("@SubCatId", null);
                            cmd.Parameters.AddWithValue("@LocationId", LocationIdd);
                            cmd.Parameters.AddWithValue("@BuildingId", BuildingIdd);
                            cmd.Parameters.AddWithValue("@FloorId", FloorIdd);
                            cmd.Parameters.AddWithValue("@DepartmentId", null);
                            cmd.Parameters.AddWithValue("@FromDate", null);
                            cmd.Parameters.AddWithValue("@Todate", null);
                            cmd.Parameters.AddWithValue("@AssetCode", null);
                            cmd.Parameters.AddWithValue("@caseManager", CaseManager);
                            cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                            cmd.Parameters.AddWithValue("@SearchText", HttpContext.Current.Session["SessionofHealthDataColumn9"].ToString());
                            cmd.Parameters.AddWithValue("@Column1FCNumber", Column1FCNumberr);
                            cmd.Parameters.AddWithValue("@CustodianId", CustodianIdd);
                            cmd.Parameters.AddWithValue("@Column3ClientName", Column3ClientNamee);
                            cmd.Parameters.AddWithValue("@Column2AssigneeName", Column2AssigneeNamee);
                            cmd.Parameters.AddWithValue("@caseWorker1", HttpContext.Current.Session["SessionofCaseWorker1"].ToString());
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.SelectCommand.CommandTimeout = 600;
                                adp.Fill(ds);
                            }
                        }

                        // HttpContext.Current.Session["LocationID"] = HttpContext.Current.Session["BuildingID"] = HttpContext.Current.Session["FloorId"] = HttpContext.Current.Session["Column5CaseManager"] = HttpContext.Current.Session["Column1FCNumber"] = HttpContext.Current.Session["Column3ClientName"] = HttpContext.Current.Session["Column2AssigneeName"] = HttpContext.Current.Session["CustodianId"] = null;
                    }
                    else if (HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] != null)
                    {
                        //GetAssetsAccordingToDateandTypeV2_forcasemanagerData
                        using (SqlCommand cmdw = new SqlCommand("GetAssetsAccordingToDateandTypeV2_forcasemanagerData_withoutDocReturned", con))
                        {
                            cmdw.CommandType = CommandType.StoredProcedure;
                            cmdw.Parameters.AddWithValue("@CategoryId", null);
                            cmdw.Parameters.AddWithValue("@SubCatId", null);
                            cmdw.Parameters.AddWithValue("@LocationId", LocationId);
                            cmdw.Parameters.AddWithValue("@BuildingId", BuildingId);
                            cmdw.Parameters.AddWithValue("@FloorId", FloorId);
                            cmdw.Parameters.AddWithValue("@DepartmentId", null);
                            cmdw.Parameters.AddWithValue("@FromDate", null);
                            cmdw.Parameters.AddWithValue("@Todate", null);
                            cmdw.Parameters.AddWithValue("@AssetCode", null);
                            cmdw.Parameters.AddWithValue("@CustodianId", CustodianId);
                            cmdw.Parameters.AddWithValue("@SearchText", null);
                            cmdw.Parameters.AddWithValue("@Column1FCNumber", null);
                            cmdw.Parameters.AddWithValue("@Column2AssigneeName", null);
                            cmdw.Parameters.AddWithValue("@Column3ClientName", null);
                            cmdw.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                            cmdw.Parameters.AddWithValue("@Column5", HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"]);
                            cmdw.Parameters.AddWithValue("@Column7", HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"].ToString());
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmdw))
                            {
                                adp.SelectCommand.CommandTimeout = 600;
                                adp.Fill(ds);
                            }
                        }

                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_2", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                            cmd.Parameters.AddWithValue("@SubCatId", SubCatId);
                            cmd.Parameters.AddWithValue("@LocationId", LocationId);
                            cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                            cmd.Parameters.AddWithValue("@FloorId", FloorId);
                            cmd.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                            cmd.Parameters.AddWithValue("@FromDate", null);
                            cmd.Parameters.AddWithValue("@Todate", null);
                            cmd.Parameters.AddWithValue("@AssetCode", AssetCode);
                            cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                            cmd.Parameters.AddWithValue("@SearchText", SearchText);
                            cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.SelectCommand.CommandTimeout = 600;
                                adp.Fill(ds);
                            }
                        }
                    }
                }
                if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
                {
                }
                else
                {
                    dt = ds.Tables[0];
                    dt = new DataTable();
                    dt = ds.Tables[0];

                    DataView myView;
                    myView = ds.Tables[0].DefaultView;
                    gvData.DataSource = myView;

                }


            }
            catch (Exception ex)
            {
                Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "grid_view", path);
                ////lblMsg.Visible = true;
                ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "grid_view", path);
            ////lblMsg.Visible = true;
            ////lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }

    protected void OnSelectedIndexChangedCategory_Search(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();



            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getcategoryinsubcatasset, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  ddlCategorySearch.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
            //ddlsubcatSearch.DataSource = ds;
            //ddlsubcatSearch.DataTextField = "SubCatName";
            //ddlsubcatSearch.DataValueField = "SubCatId";
            //ddlsubcatSearch.DataBind();
            //ddlsubcatSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedCategory_Search", path);
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
            //ddlsubcat.DataSource = ds;
            //ddlsubcat.DataTextField = "SubCatName";
            //ddlsubcat.DataValueField = "SubCatId";
            //ddlsubcat.DataBind();
            //////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedCategory", path);
        }
    }

    protected void OnSelectedIndexChangedLocation_Search(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.Getbuilding, new SqlParameter[] {
                      new SqlParameter("@LocationId",  ddllocSearch.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
            ddlbuildSearch.DataSource = ds;
            ddlbuildSearch.DataTextField = "BuildingName";
            ddlbuildSearch.DataValueField = "BuildingId";
            ddlbuildSearch.DataBind();
            ddlbuildSearch.Items.Insert(0, new ListItem("--Select--", "0", true));

            ddlfloorSearch.Items.Clear();
            ddlfloorSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedLocation_Search", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedLocation", path);
        }
    }

    protected void OnSelectedIndexChangedBuilding_Search(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getfloorforasset, new SqlParameter[] {
                      new SqlParameter("@BuildingId",  ddlbuildSearch.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
            ddlfloorSearch.DataSource = ds;
            ddlfloorSearch.DataTextField = "FloorName";
            ddlfloorSearch.DataValueField = "FloorId";
            ddlfloorSearch.DataBind();
            ddlfloorSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedBuilding_Search", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }
    private void bindlocation()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");

            ddlloc.DataSource = ds;
            ddlloc.DataTextField = "LocationName";
            ddlloc.DataValueField = "LocationId";
            ddlloc.DataBind();
            ddlloc.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "bindlocation", path);

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
            //ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
            //DataTable dt = ds.Tables[0];
            //for (int i = dt.Rows.Count - 1; i >= 0; i--)
            //{
            //    DataRow dr = dt.Rows[i];
            //    if (dr["Status"] == "Inactive")
            //        dr.Delete();
            //}
            //dt.AcceptChanges();
            ddlCustodian.DataSource = ds;
            ddlCustodian.DataTextField = "CustodianName";
            ddlCustodian.DataValueField = "CustodianId";
            ddlCustodian.DataBind();
            ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "bindCustodian", path);
        }
    }

    private void bindlocationSearch()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            // DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + Session["userid"].ToString() + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            ddllocSearch.DataSource = ds;
            ddllocSearch.DataTextField = "LocationName";
            ddllocSearch.DataValueField = "LocationId";
            ddllocSearch.DataBind();
            ddllocSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "bindlocationSearch", path);
        }
    }
    //
    private void BincategorySearch()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            ddlCategorySearch.DataSource = ds;
            ddlCategorySearch.DataTextField = "CategoryName";
            ddlCategorySearch.DataValueField = "CategoryId";
            ddlCategorySearch.DataBind();
            ddlCategorySearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BincategorySearch", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "Bincategory", path);
        }
    }
    private void BindDepartmentSearch()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

            ddldeptSearch.DataSource = ds;
            ddldeptSearch.DataTextField = "DepartmentName";
            ddldeptSearch.DataValueField = "DepartmentId";
            ddldeptSearch.DataBind();
            ddldeptSearch.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindDepartmentSearch", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindDepartment", path);
        }
    }
    private void BindSupplier()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");

            DropDownList1.DataSource = ds;
            DropDownList1.DataTextField = "SupplierName";
            DropDownList1.DataValueField = "SupplierId";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem("--Select--", "1", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindSupplier", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_NeedDataSource", path);
        }
    }

    // For editing and deleting
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {




        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gv_data_ItemCommand", path);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

        try
        {
            ddlproCategory.SelectedValue = "0";
            //ddlsubcat.Items.Clear();
            //////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlloc.SelectedValue = "0";
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddldept.SelectedValue = "0";
            ddlCustodian.SelectedValue = "0";
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnClear_Click", path);
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            divUpdateFields.Visible = false;
            divWarantyClose.Visible = false;
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnsubmit_Click", path);
        }
    }
    protected void btnManualEdition_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnManualEdition.Text == "Submit")
            {
                CompanyBL objcomp = new CompanyBL();
                DataSet ds = objcomp.getUserSetting();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ImportType"].ToString() == "0" && ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1" && (txtquant.Text.ToString() == ""))
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Quantity should not be empty.');", true);
                        return;
                    }
                }

                if (exist_SerialNumber(txtserail.Text.Trim()) == true)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial No. Already Exist, \\n Please Insert Different Serial No.');", true);
                    return;
                }
                else
                {
                    con.Open();

                    using (SqlTransaction Trans = con.BeginTransaction())
                    {
                        try
                        {
                            //string DeliveryDate = txtdeldate.Text == "" ? DBNull.Value.ToString() : txtdeldate.Text;
                            SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PinsertAsset", new SqlParameter[] {
                             new SqlParameter("@TranID", 0),
                             new SqlParameter("@CategoryId", ddlproCategory.SelectedValue),
                             new SqlParameter("@CategoryName", ddlproCategory.SelectedItem.Text),
                             new SqlParameter("@SubCategoryId", ""),
                             new SqlParameter("@SubCategoryName", ""),
                             new SqlParameter("@BuildingId", ddlbuild.SelectedValue),
                             new SqlParameter("@FloorId", ddlfloor.SelectedValue),
                             new SqlParameter("@LocationId", ddlloc.SelectedValue),
                             new SqlParameter("@DepartmentId", ddldept.SelectedValue),
                             new SqlParameter("@CustodianId", ddlcust.SelectedValue),
                             new SqlParameter("@SupplierId", DropDownList1.SelectedValue),
                             new SqlParameter("@SerialNo", txtserail.Text.Trim()),
                             new SqlParameter("@Description", txtdesc.Text.Trim()),
                             new SqlParameter("@Quantity", txtquant.Text.Trim()),
                             new SqlParameter("@Price", txtprice.Text.Trim()),
                             new SqlParameter("@DeliveryDate",txtdeldate.Text.Trim() == "" ? "" : Convert.ToDateTime(txtdeldate.Text.Trim()).ToString("dd-MMM-yyyy")),
                             new SqlParameter("@AssignDate", txtass.Text.Trim() == "" ? "" : Convert.ToDateTime(txtass.Text.Trim()).ToString("dd-MMM-yyyy")),
                             new SqlParameter("@Active", chkstatus.Checked == true ? 1 : 0),
                             new SqlParameter("@ImportType", "Manual"),
                             new SqlParameter("@LocationInDateTime",DateTime.Now.ToString("dd-MMM-yyyy")),

                        }
                                    );

                            Trans.Commit();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Inserted Successfully.');", true);
                            grid_view();
                            gvData.DataBind();
                        }
                        catch (Exception ex)
                        {
                            Trans.Rollback();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnManualEdition_Click", path);
                        }
                    }
                }


            }
            else if (btnManualEdition.Text == "Update")
            {
                // if (exist_SerialNumber(txtserail.Text.Trim()) == true)
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial No. Already Exist, \\n Please Insert Different Serial No.');", true);
                //    return;
                //}
                //else
                //{
                con.Open();
                using (SqlTransaction Trans = con.BeginTransaction())
                {

                    try
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "pupdateasset", new SqlParameter[] {

                            new SqlParameter("@AssetId", Convert.ToInt32(Session["AssetIdForUpdate"].ToString())), 
                           //new SqlParameter("@AssetCode",hidcatcode.Value.ToString()),
                   new SqlParameter("@CategoryId", ddlproCategory.SelectedValue),
                   new SqlParameter("@CategoryName", ddlproCategory.SelectedItem.Text),
                             new SqlParameter("@SubCategoryId", ""),
                             new SqlParameter("@SubCategoryName",""),
                             new SqlParameter("@BuildingId", ddlbuild.SelectedValue),
                             new SqlParameter("@FloorId", ddlfloor.SelectedValue),

                             new SqlParameter("@LocationId", ddlloc.SelectedValue),
                             new SqlParameter("@DepartmentId", ddldept.SelectedValue),
                             new SqlParameter("@CustodianId", ddlcust.SelectedValue),
                             new SqlParameter("@SupplierId", DropDownList1.SelectedValue),

                             new SqlParameter("@LocationInDateTime",DateTime.Now.ToString("dd-MMM-yyyy")),
                             new SqlParameter("@SerialNo", txtserail.Text.Trim()),
                                   new SqlParameter("@Description", txtdesc.Text.Trim()),
                                   new SqlParameter("@Quantity", txtquant.Text.Trim()),
                                   new SqlParameter("@Price", txtprice.Text.Trim()),
                                   //new SqlParameter("@DeliveryDate", txtdeldate.Text.Trim().ToString()),
                                   //new SqlParameter("@AssignDate", txtass.Text.Trim().ToString()),
                             new SqlParameter("@DeliveryDate",txtdeldate.Text.Trim() == "" ? "" : Convert.ToDateTime(txtdeldate.Text.Trim()).ToString("dd-MMM-yyyy")),
                             new SqlParameter("@AssignDate", txtass.Text.Trim() == "" ? "" : Convert.ToDateTime(txtass.Text.Trim()).ToString("dd-MMM-yyyy")),

                    new SqlParameter("@Active", chkstatus.Checked == true ? 1 : 0),


                }
                   );


                        Trans.Commit();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Assets + " updated successfully.');", true);
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully.');", true);
                    }
                    catch (Exception ex)
                    {
                        Trans.Rollback();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                        Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnManualEdition_Click", path);
                    }

                    btnsubmit.Text = "Add";

                }

                // }

            }
            Session["AssetIdForUpdate"] = "";
            hdncatidId.Value = "";
            grid_view();
            gvData.DataBind();
            btnreset_Click(sender, e);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnManualEdition_Click", path);
            txtquant.Enabled = true;
            Session["AssetIdForUpdate"] = "";
        }



    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if ((productimguploder.HasFile))
            {
                DataSet ds = GetDateFromExcelSheet();
                if (ds == null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Sheet1 not found in excel...');", true);
                    return;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ////this.Exceldt = ds.Tables[0];
                    Exceldt = ds.Tables[0];

                    AssetBL objAsset = new AssetBL();
                    this.MappingExist = objAsset.CheckMappingExistsForTheClient();

                    if (MappingExist == false)
                    {
                        GetColumnNamesFromTheExcelSheet(Exceldt);
                        ModalPopupExtender2.Show();
                    }

                    else
                    {
                        List<MappingInfo> ListMapping = new List<MappingInfo>();
                        ListMapping = objAsset.GetMappingListFromDB();
                        CreateTables();
                        if (ValidateData(Exceldt, ListMapping) == true)
                        {
                            //if (this.IsQuantitybase == false)
                            //{
                            if (ListMapping.Any(L => L.ColumnName == "SerialNo") == true)
                            {
                                string MapSerialName = ListMapping.Where(x => x.ColumnName == "SerialNo").SingleOrDefault().MappingColumnName;
                                object SerialNo = string.Join(",", Exceldt.AsEnumerable().Where(s => s.Field<object>(MapSerialName) != null).Select(s => s.Field<object>(MapSerialName)).ToArray<object>());

                                ////if (exist_SerialNumber(SerialNo.ToString()) == true)
                                ////{
                                ////    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Assets with Serial no already exists.\n Can not import Assets.');", true);
                                ////    //return;
                                ////    lblmodmsg.Text = "Assets with Serial no already exists. Can not import Assets.";
                                ////    ModalPopupExtender1.Show();
                                ////}

                                string SerialNo_ColumnNameinExel = MapSerialName;
                                /* Check duplicate serial number in Excel */
                                foreach (DataRow row in Exceldt.Rows)
                                {

                                    string s = row[SerialNo_ColumnNameinExel].ToString();
                                    string qry = "[" + SerialNo_ColumnNameinExel + "]= '" + s + "'";
                                    DataRow[] drx = Exceldt.Select(qry);

                                    //string s = row["SerialNo"].ToString();
                                    //DataRow[] drx = Exceldt.Select("[SerialNo]= '" + s + "'");
                                    if (drx.Length > 1)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial number should be unique..');", true);
                                        return;
                                    }
                                }

                                DataTable Dt_Exists = new DataTable();
                                Dt_Exists = exist_SerialNumber_chk(SerialNo.ToString());
                                if (Dt_Exists.Rows.Count > 0)
                                {
                                    // If Quantiti based with SerialNo column mapped, Then show duplicate Serial No
                                    if (this.IsQuantitybase == true)
                                    {
                                        string Sno = string.Join(",", Dt_Exists.AsEnumerable().Select(s => s["SerialNo"].ToString()).ToArray<string>());
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The below given assets with Serial No already exists,cannot import assets. - " + Sno.ToString() + "');", true);
                                    }
                                    else
                                    {
                                        ValidateExcelAndBindGrid(Exceldt, ListMapping);
                                    }
                                }
                                else
                                {
                                    ValidateExcelAndBindGrid(Exceldt, ListMapping);
                                }

                            }
                            else
                            {

                                ValidateExcelAndBindGrid(Exceldt, ListMapping);
                            }
                            //}
                        }


                    }
                }
            }

            else
            {
                ////lblMessage.Text = "Please select an excel file first";
                ////lblMessage.ForeColor = System.Drawing.Color.Red;
                ////lblMessage.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnImport_Click", path);
        }
    }

    public static void SetColumnsOrder(DataTable table, List<MappingInfo> ListMapping)
    {
        try
        {
            String[] _Columns = new String[5];
            _Columns[0] = "CategoryName";
            _Columns[1] = "SubCategoryName";
            _Columns[2] = "LocationName";
            _Columns[3] = "BuildingName";
            _Columns[4] = "FloorName";
            int i = 0;
            foreach (var columnName in ListMapping)
            {
                if (columnName.ColumnName.ToString() == _Columns[i])
                {
                    table.Columns[columnName.MappingColumnName].SetOrdinal(i);
                    i++;
                    break;
                }
                //switch (columnName.ColumnName.ToString())
                //{
                //    case "CategoryName":
                //        {
                //            table.Columns[columnName.MappingColumnName].SetOrdinal(0);
                //            break;
                //        }
                //    case "SubCategoryName":
                //        {
                //            table.Columns[columnName.MappingColumnName].SetOrdinal(1);
                //            break;
                //        }
                //    case "LocationName":
                //        {
                //            table.Columns[columnName.MappingColumnName].SetOrdinal(2);
                //            break;
                //        }
                //    case "BuildingName":
                //        {
                //            table.Columns[columnName.MappingColumnName].SetOrdinal(3);
                //            break;
                //        }
                //    case "FloorName":
                //        {
                //            table.Columns[columnName.MappingColumnName].SetOrdinal(4);
                //            break;
                //        }
                //}
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "SetColumnsOrder", path);

        }
    }

    private void ValidateExcelAndBindGrid(DataTable Exceldt, List<MappingInfo> ListMapping)
    {
        try
        {
            SetColumnsOrder(Exceldt, ListMapping);
            StringBuilder sb = new StringBuilder();
            int MaxID = 0;
            int TotalColumn = Exceldt.Columns.Count;
            BindColumntoExcel(Exceldt);
            int i = 0;
            int j = 1;
            int colCount = 0;
            List<CurrentMapping> CurrentMapping = GetCurrentMapping(ListMapping);//To keep the current Map

            foreach (DataRow dr in Exceldt.Rows)
            {


                foreach (var Coulmn in Exceldt.Columns)
                {
                    if (++colCount > TotalColumn) break;
                    string tblHeaderCoulmn = Coulmn.ToString();

                    bool ColumnExists = CheckColumnExists(tblHeaderCoulmn, CurrentMapping);

                    if (ColumnExists)
                    {
                        var MappingItem = ListMapping.Where(M => M.MappingColumnName == tblHeaderCoulmn).ToList();
                        string DBColumnname = MappingItem[0].ColumnName.ToString();
                        string ExcelValue = dr[tblHeaderCoulmn].ToString();
                        ExcelValue = ExcelValue.ToString().Replace("'", "''");
                        switch (DBColumnname)
                        {



                            case "CategoryName":
                                {
                                    DataRow[] Exist_CatName = dt_cat.Select("CategoryName='" + ExcelValue.Trim() + "'");

                                    if (Exist_CatName.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " not Exists in the master" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["CategoryId"] = Convert.ToInt32(Exist_CatName[0].ItemArray[0]);
                                        Exceldt.Rows[i]["ECatCode"] = Exist_CatName[0].ItemArray[2].ToString();

                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }

                            case "SubCategoryName":
                                {
                                    DataRow[] Exist_SubCatName = dt_subcat.Select("SubCatName='" + ExcelValue.Trim() + "'");

                                    if (Exist_SubCatName.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + SubCategory + " not available in  master" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["SubCatId"] = Convert.ToInt32(Exist_SubCatName[0].ItemArray[0]);
                                        Exceldt.Rows[i]["ESubCatCode"] = Exist_SubCatName[0].ItemArray[2].ToString();

                                        Exceldt.AcceptChanges();

                                        string CategoryId = Convert.ToString(Exceldt.Rows[i]["CategoryId"]);
                                        DataRow[] drData = dt_cat.Select("CategoryId='" + CategoryId + "'");
                                        if (drData != null && drData.Length == 0)
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " not found for this " + SubCategory + " in  master" + "');", true);
                                            return;
                                        }
                                        string ExcelColumnName = CurrentMapping.Where(x => x.ColumnName == "SubCategoryName").FirstOrDefault().MappingColumnName;
                                        string FurnitureNameFromExcel = Exceldt.Rows[i][ExcelColumnName].ToString();
                                        DataRow[] drSubcat = dt_subcat.Select("SubCatName='" + FurnitureNameFromExcel.Trim() + "'" + " and CategoryId= '" + CategoryId.Trim() + "'");
                                        if (drSubcat.Length == 0)
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " and " + SubCategory + " is not mattched in  master" + "');", true);
                                            return;
                                        }

                                    }
                                    break;
                                }

                            case "FloorName":
                                {
                                    string _Building = Exceldt.Rows[i]["BuildingId"].ToString();
                                    string _Location = Exceldt.Rows[i]["LocationId"].ToString();
                                    DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue.Trim() + "' and BuildingID ='" + _Building + "' and LocationId ='" + _Location + "'");
                                    //DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue.Trim() + "'");
                                    if (Exist_Floor.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Floor + " is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["FloorId"] = Convert.ToInt32(Exist_Floor[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "BuildingName":
                                {
                                    string _Location = Exceldt.Rows[i]["LocationId"].ToString();
                                    DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + ExcelValue.Trim() + "' and LocationId ='" + _Location + "'");
                                    if (Exist_BUILDING.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Building + "  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["BuildingId"] = Convert.ToInt32(Exist_BUILDING[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "LocationName":
                                {
                                    DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + ExcelValue.Trim() + "'");

                                    if (Exist_LOCATION.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Location + "  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["LocationId"] = Convert.ToInt32(Exist_LOCATION[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                        string LocationId = "", BuildingId = "", FloorId = "", LocationNmfrmExcel = "", LocationTextFromExcel = "";

                                        var LocInfo = CurrentMapping.Where(x => x.ColumnName == "LocationName").ToList();
                                        if (LocInfo.Count > 0)
                                        {
                                            LocationNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "LocationName").FirstOrDefault().MappingColumnName;
                                            LocationTextFromExcel = Exceldt.Rows[i][LocationNmfrmExcel].ToString();
                                        }

                                        var BldInfo = CurrentMapping.Where(x => x.ColumnName == "BuildingName").ToList();
                                        string BuildingNmfrmExcel = "", BuildingTextFromExcel = "";
                                        if (BldInfo.Count > 0)
                                        {
                                            BuildingNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "BuildingName").FirstOrDefault().MappingColumnName; ;
                                            BuildingTextFromExcel = Exceldt.Rows[i][BuildingNmfrmExcel].ToString();
                                        }


                                        var FlrInfo = CurrentMapping.Where(x => x.ColumnName == "FloorName").ToList();
                                        string FloorNmfrmExcel = "", FloorTextFromExcel = "", FloorNmfromExcel = "";
                                        if (FlrInfo.Count > 0)
                                        {
                                            FloorNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "FloorName").FirstOrDefault().MappingColumnName;
                                            FloorTextFromExcel = Convert.ToString(Exceldt.Rows[i][FloorNmfrmExcel]);
                                            FloorNmfromExcel = CurrentMapping.Where(x => x.ColumnName == "FloorName").FirstOrDefault().MappingColumnName;
                                        }

                                        DataRow[] drLoc = dt_Loc.Select("LocationName='" + LocationTextFromExcel.Trim() + "'");
                                        if (drLoc != null && drLoc.Length > 0)
                                        {
                                            LocationId = drLoc[0].ItemArray[0].ToString();

                                        }



                                        if (BldInfo.Count > 0)
                                        {
                                            DataRow[] drBuilding = dt_Build.Select("BuildingName='" + BuildingTextFromExcel.Trim() + "'" + " and LocationId= '" + LocationId.Trim() + "'");
                                            if (drBuilding.Length == 0)
                                            {
                                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Building + " is not mapped with existing " + Location + " master " + "');", true);
                                                return;
                                            }
                                            if (drBuilding.Length > 0)
                                            {
                                                BuildingId = Convert.ToString(drBuilding[0].ItemArray[0]);
                                                DataRow[] drFloor = dt_floor.Select("FloorName='" + FloorTextFromExcel.Trim() + "'" + " and BuildingId='" + BuildingId.Trim() + "'");

                                                if (FlrInfo.Count > 0)
                                                {
                                                    if (drLoc.Length == 0 || drBuilding.Length == 0 || drFloor.Length == 0)
                                                    {
                                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + FloorTextFromExcel.Trim() + " -  " + Floor + " is not mapped with existing " + Building + " and " + Location + " in master" + "');", true);
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    break;
                                }
                            case "DepartmentName":
                                {
                                    DataRow[] Exist_DEPARTMENT = dt_department.Select("DepartmentName='" + ExcelValue.Trim() + "'");
                                    if (Exist_DEPARTMENT.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Department  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["DepartmentId"] = Convert.ToInt32(Exist_DEPARTMENT[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "CustodianName":
                                {
                                    DataRow[] Exist_CUSTODIAN = dt_custodian.Select("CustodianName='" + ExcelValue.Trim() + "'");
                                    if (Exist_CUSTODIAN.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Custodian  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["CustodianId"] = Convert.ToInt32(Exist_CUSTODIAN[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "SupplierName":
                                {
                                    DataRow[] Exist_SUPPLIERNAME = dt_supplier.Select("SupplierName='" + ExcelValue.Trim() + "'");
                                    if (Exist_SUPPLIERNAME.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Supplier  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["SupplierId"] = Convert.ToInt32(Exist_SUPPLIERNAME[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }

                            case "SerialNo":
                                {

                                    if (ExcelValue.ToString() == "")
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.ToString() + " - Serial number should not be empty" + "');", true);
                                        return;
                                    }

                                    Exceldt.Rows[i]["NSerialNo"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Quantity":
                                {
                                    if (this.IsQuantitybase == false && Convert.ToInt32(ExcelValue) > 1)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('While Import Quantity Should not be greater than 1');", true);
                                        return;
                                    }
                                    else
                                    {
                                        int value;
                                        if (int.TryParse(ExcelValue, out value))
                                        {
                                            Exceldt.Rows[i]["NQuantity"] = ExcelValue.ToString();
                                            Exceldt.AcceptChanges();
                                        }
                                        else
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " -Quantity should be a integer value" + "');", true);
                                            return;
                                        }
                                    }

                                    break;
                                }
                            case "Description":
                                {
                                    Exceldt.Rows[i]["NDescription"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Price":
                                {
                                    Exceldt.Rows[i]["NPrice"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "DeliveryDate":
                                {
                                    Exceldt.Rows[i]["NDeliveryDate"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "AssignDate":
                                {
                                    Exceldt.Rows[i]["NAssignDate"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Active":
                                {
                                    Exceldt.Rows[i]["NActive"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Column1":
                                {
                                    Exceldt.Rows[i]["Column1"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column2":
                                {
                                    Exceldt.Rows[i]["Column2"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column3":
                                {
                                    Exceldt.Rows[i]["Column3"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column4":
                                {
                                    Exceldt.Rows[i]["Column4"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column5":
                                {
                                    Exceldt.Rows[i]["Column5"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column6":
                                {
                                    Exceldt.Rows[i]["Column6"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column7":
                                {
                                    Exceldt.Rows[i]["Column7"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column8":
                                {
                                    Exceldt.Rows[i]["Column8"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column9":
                                {
                                    Exceldt.Rows[i]["Column9"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column10":
                                {
                                    Exceldt.Rows[i]["Column10"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column11":
                                {
                                    Exceldt.Rows[i]["Column11"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column12":
                                {
                                    Exceldt.Rows[i]["Column12"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column13":
                                {
                                    Exceldt.Rows[i]["Column13"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column14":
                                {
                                    Exceldt.Rows[i]["Column14"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column15":
                                {
                                    Exceldt.Rows[i]["Column15"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Image":
                                {
                                    Exceldt.Rows[i]["Image"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }


                        }
                    }
                    ////else
                    ////{
                    ////    if (j <= 15)
                    ////    {
                    ////        MappingInfo objInfo = new MappingInfo();
                    ////        string ExcelValue = dr[tblHeaderCoulmn].ToString();
                    ////        string dbColumn = "Column" + j;
                    ////        Exceldt.Rows[i]["Column" + j] = ExcelValue;
                    ////        Exceldt.AcceptChanges();
                    ////        if (ListMapping.Any(M => M.ColumnName == dbColumn) == false)
                    ////        {
                    ////            var maxID = ListMapping.Count();
                    ////            switch (dbColumn)
                    ////            {
                    ////                case "Column1":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column2":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column3":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column4":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column5":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column6":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column7":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column8":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column9":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column10":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column11":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column12":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column13":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column14":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                case "Column15":
                    ////                    {
                    ////                        objInfo.id = j;
                    ////                        objInfo.ColumnName = dbColumn;
                    ////                        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                        ListMapping.Add(objInfo);
                    ////                        break;
                    ////                    }
                    ////                    //case "Column16":
                    ////                    //    {
                    ////                    //        objInfo.id = Convert.ToInt32(maxID) + 1;
                    ////                    //        objInfo.ColumnName = dbColumn;
                    ////                    //        objInfo.MappingColumnName = tblHeaderCoulmn;
                    ////                    //        ListMapping.Add(objInfo);
                    ////                    //        break;
                    ////                    //    }

                    ////            }
                    ////        }
                    ////        j++;
                    ////    }
                    ////}

                }
                i++;
                colCount = 0;
                j = 1;
            }


            //MaxID = InsertImportedFile();
            //if (MaxID > 0)
            //{
            BindImportAssetToGrid(Exceldt, ListMapping, this.MappingExist);
            grid_view();
            gvData.DataBind();
            //}
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "ValidateExcelAndBindGrid", path);
        }
    }

    private string GetValidateExcel(DataTable Exceldt)
    {
        string message = "";


        return message;
    }

    private void ValidateExcelAndBindGrid(DataTable Exceldt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            //int MaxID = 0;

            BindColumntoExcel(Exceldt);
            int i = 0;
            foreach (DataRow dr in Exceldt.Rows)
            {
                string CategoryName = dr["CATEGORY"].ToString();
                DataRow[] Exist_CatName = dt_cat.Select("CategoryName='" + CategoryName + "'");
                if (Exist_CatName.Length == 0)
                {
                    sb.Append(CategoryName + " - " + Category + " not Exists in the master \\n");


                }
                else
                {
                    Exceldt.Rows[i]["CategoryId"] = Convert.ToInt32(Exist_CatName[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }

                string SubCategoryName = dr["SUB-CATEGORY"].ToString();
                DataRow[] Exist_SubCatName = dt_subcat.Select("SubCatName='" + SubCategoryName + "'");
                if (Exist_SubCatName.Length == 0)
                {
                    sb.Append(SubCategoryName + " - " + SubCategory + " not available in  master \\n ");


                }
                else
                {
                    Exceldt.Rows[i]["SubCatId"] = Convert.ToInt32(Exist_SubCatName[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }


                string _Floor = dr["FLOOR"].ToString();
                DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + _Floor + "'");
                if (Exist_Floor.Length == 0)
                {
                    sb.Append(_Floor + " - " + Floor + " is not available in masters \\n ");


                }
                else
                {
                    Exceldt.Rows[i]["FloorId"] = Convert.ToInt32(Exist_Floor[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }
                string _BUILDING = dr["BUILDING"].ToString();
                DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + _BUILDING + "'");
                if (Exist_BUILDING.Length == 0)
                {
                    sb.Append(_BUILDING + " - " + Building + "  is not available in masters \\n ");
                }

                else
                {
                    Exceldt.Rows[i]["BuildingId"] = Convert.ToInt32(Exist_BUILDING[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }
                string _LOCATION = dr["LOCATION"].ToString();
                DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + _LOCATION + "'");
                if (Exist_LOCATION.Length == 0)
                {
                    sb.Append(_LOCATION + " - " + Location + "  is not available in masters \\n ");

                }
                else
                {
                    Exceldt.Rows[i]["LocationId"] = Convert.ToInt32(Exist_LOCATION[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }

                string DEPARTMENT = dr["DEPARTMENT"].ToString();
                DataRow[] Exist_DEPARTMENT = dt_department.Select("DepartmentName='" + DEPARTMENT + "'");
                if (Exist_DEPARTMENT.Length == 0)
                {
                    sb.Append(DEPARTMENT + " -  Department  is not available in masters \\n ");

                }

                else
                {
                    Exceldt.Rows[i]["DepartmentId"] = Convert.ToInt32(Exist_DEPARTMENT[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }
                string CUSTODIAN = dr["CUSTODIAN"].ToString();
                DataRow[] Exist_CUSTODIAN = dt_custodian.Select("CustodianName='" + CUSTODIAN + "'");
                if (Exist_CUSTODIAN.Length == 0)
                {
                    sb.Append(CUSTODIAN + " - Custodian  is not available in masters \\n ");


                }
                else
                {
                    Exceldt.Rows[i]["CustodianId"] = Convert.ToInt32(Exist_CUSTODIAN[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }
                string SUPPLIERNAME = dr["SUPPLIERNAME"].ToString();
                DataRow[] Exist_SUPPLIERNAME = dt_supplier.Select("SupplierName='" + SUPPLIERNAME + "'");
                if (Exist_SUPPLIERNAME.Length == 0)
                {
                    sb.Append(SUPPLIERNAME + " - Supplier  is not available in masters \\n ");


                }
                else
                {
                    Exceldt.Rows[i]["SupplierId"] = Convert.ToInt32(Exist_SUPPLIERNAME[0].ItemArray[0]);
                    Exceldt.AcceptChanges();
                }

                if (sb.ToString().Length > 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + sb.ToString() + "');", true);
                    return;
                }
                i++;

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "ValidateExcelAndBindGrid", path);
        }
    }

    private void CreateTables()
    {
        try
        {
            this.dt_cat = GetActiveCategory();
            this.dt_subcat = GetActiveSubCategory();
            this.dt_Loc = GetActiveLocatio();
            this.dt_Build = GetActiveBuilding();
            this.dt_floor = GetActiveFloor();
            this.dt_department = GetActiveDepartment();
            this.dt_custodian = GetCustodian();
            this.dt_supplier = GetActiveSupplier();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "CreateTables", path);
        }
    }
    private DataTable GetActiveSupplier()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");
        con.Close();
        return ds.Tables[0];

    }

    private DataTable GetCustodian()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveCustodian");
        con.Close();
        return ds.Tables[0];

    }

    private DataTable GetActiveDepartment()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");
        con.Close();
        return ds.Tables[0];

    }

    private DataTable GetActiveFloor()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveFloor");
        con.Close();
        return ds.Tables[0];
    }

    private DataTable GetActiveBuilding()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveBuilding");
        con.Close();
        return ds.Tables[0];


    }

    private DataTable GetActiveLocatio()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
        con.Close();
        return ds.Tables[0];
    }

    private DataTable GetActiveSubCategory()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveSubCat");
        con.Close();
        return ds.Tables[0];
    }

    private DataTable GetActiveCategory()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");
        con.Close();
        return ds.Tables[0];
    }

    private DataTable exist_SerialNumber_chk(string SerialNo)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        DataSet ds_exist = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "CheckSerialNos", new SqlParameter[] {
                        new SqlParameter("@SerialNo", SerialNo),
                 });
        if (ds_exist.Tables[0].Rows.Count > 0)
        {
            return ds_exist.Tables[0];
        }
        else
        {
            return ds_exist.Tables[0];
        }
    }

    private bool exist_SerialNumber(string SerialNo)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckSerialNos", new SqlParameter[] {
                        new SqlParameter("@SerialNo", SerialNo),
                 }));
        if (exist == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private List<MappingInfo> GetSelectedMapping()
    {
        try
        {
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            if (ddl1.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "CategoryName";
                objInfo.MappingColumnName = ddl1.SelectedValue;
                ListMapping.Add(objInfo);

            }
            //if (ddl7.SelectedValue != "-Select-")
            //{
            //    MappingInfo objInfo = new MappingInfo();
            //    objInfo.ColumnName = "SubCategoryName";
            //    objInfo.MappingColumnName = ddl7.SelectedValue;
            //    ListMapping.Add(objInfo);
            //}
            if (ddl10.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "BuildingName";
                objInfo.MappingColumnName = ddl10.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl5.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "FloorName";
                objInfo.MappingColumnName = ddl5.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl3.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "LocationName";
                objInfo.MappingColumnName = ddl3.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl12.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "DepartmentName";
                objInfo.MappingColumnName = ddl12.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl14.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "CustodianName";
                objInfo.MappingColumnName = ddl14.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl16.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "SupplierName";
                objInfo.MappingColumnName = ddl16.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl18.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "SerialNo";
                objInfo.MappingColumnName = ddl18.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl19.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Description";
                objInfo.MappingColumnName = ddl19.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl20.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Quantity";
                objInfo.MappingColumnName = ddl20.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl21.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Price";
                objInfo.MappingColumnName = ddl21.SelectedValue;
                ListMapping.Add(objInfo);
            }

            if (ddl22.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "DeliveryDate";
                objInfo.MappingColumnName = ddl22.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl23.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "AssignDate";
                objInfo.MappingColumnName = ddl23.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl24.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column1";
                objInfo.MappingColumnName = ddl24.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl4.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column2";
                objInfo.MappingColumnName = ddl4.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl11.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column3";
                objInfo.MappingColumnName = ddl11.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl17.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column4";
                objInfo.MappingColumnName = ddl17.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl6.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column5";
                objInfo.MappingColumnName = ddl6.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl2.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column6";
                objInfo.MappingColumnName = ddl2.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl8.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column7";
                objInfo.MappingColumnName = ddl8.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl9.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column8";
                objInfo.MappingColumnName = ddl9.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl13.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column9";
                objInfo.MappingColumnName = ddl13.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl15.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column10";
                objInfo.MappingColumnName = ddl15.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl25.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column11";
                objInfo.MappingColumnName = ddl25.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl26.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column12";
                objInfo.MappingColumnName = ddl26.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl27.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column13";
                objInfo.MappingColumnName = ddl27.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl28.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column14";
                objInfo.MappingColumnName = ddl28.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl29.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Column15";
                objInfo.MappingColumnName = ddl29.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl30.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Active";
                objInfo.MappingColumnName = ddl30.SelectedValue;
                ListMapping.Add(objInfo);
            }
            if (ddl300.SelectedValue != "-Select-")
            {
                MappingInfo objInfo = new MappingInfo();
                objInfo.ColumnName = "Image";
                objInfo.MappingColumnName = ddl300.SelectedValue;
                ListMapping.Add(objInfo);
            }
            return ListMapping;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "GetSelectedMapping", path);
            return null;
        }
    }
    // compare matching column name between excelsheet and List of Mapping
    private bool ValidateData(DataTable Exceldt, List<MappingInfo> ListMapping)
    {


        if (MappingExist == true)
        {
            ////Check Columns Count
            //if (Exceldt.Columns.Count.ToString() != ListMapping.Count().ToString())
            //{

            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel File Dont have all columns which are required..');", true);
            //    return false;
            //}


            if (Exceldt.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel file does not contain any Row..');", true);
                return false;
            }


            //if (Exceldt.Columns.Count != ListMapping.Count)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel file does not contain all the column you mapped.');", true);
            //    return false;
            //}

            foreach (var Column in ListMapping)
            {
                if (!Exceldt.Columns.Contains(Column.MappingColumnName))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column : " + Column.MappingColumnName + " is missing..!!');", true);
                    return false;
                }
            }

            // Loop On Columns
            //for (int i = 0; i < ListMapping.Count; i++)
            //{
            //    if (ListMapping.Any(L => L.MappingColumnName == Exceldt.Columns[i].ColumnName) == false)
            //    {
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + Exceldt.Columns[i].ColumnName + " is Invalid.');", true);
            //        return false;
            //    }
            //}

            return true;
        }
        else
        {
            return false;
        }


    }
    //Get Dataset from Excelsheet
    private DataSet GetDateFromExcelSheet()
    {
        try
        {
            OleDbConnection conn = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            string query = null;
            string connString = "";
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            this.FileName = strFileName + strFileType;
            //Check file type

            validateExcelFile(strFileType, strFileName);
            string strNewPath = Server.MapPath("~/UploadedExcel/" + strFileName + strFileType);

            //Connection String to Excel Workbook
            if (strFileType.Trim() == ".xls")
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else if (strFileType.Trim() == ".xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }



            query = "SELECT * FROM [Sheet1$]";


            //Create the connection object
            conn = new OleDbConnection(connString);
            //Open connection
            if (conn.State == ConnectionState.Closed) conn.Open();

            DataTable Sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            DataRow[] drt = Sheets.Select("TABLE_NAME='Sheet1$'");
            if (drt.Length == 0)
            {
                return null;
            }

            //Create the command object
            cmd = new OleDbCommand(query, conn);
            da = new OleDbDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "GetDateFromExcelSheet", path);
            return null;
        }
    }

    //Validate import excel file adn 
    private void validateExcelFile(string strFileType, string strFileName)
    {
        try
        {
            if (strFileType == ".xls" || strFileType == ".xlsx")
            {
                productimguploder.SaveAs(Server.MapPath("~/UploadedExcel/" + strFileName + strFileType));

            }
            else
            {
                ////lblMessage.Text = "Only excel files allowed";
                ////lblMessage.ForeColor = System.Drawing.Color.Red;
                ////lblMessage.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "validateExcelFile", path);
        }
    }

    private void GetColumnNamesFromTheExcelSheet(DataTable Exceldt)
    {
        try
        {
            string[] columnNames = Exceldt.Columns.Cast<DataColumn>()
                                    .Select(x => x.ColumnName)
                                    .ToArray();

            List<string> lstColumnNames = columnNames.OfType<string>().ToList();

            BindMapingColumnsDropDown(ddl1, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl2, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl3, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl4, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl5, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl6, lstColumnNames, "");
            // BindMapingColumnsDropDown(ddl7, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl8, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl9, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl10, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl11, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl12, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl13, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl14, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl15, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl16, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl17, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl18, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl19, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl20, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl21, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl22, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl23, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl24, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl25, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl26, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl27, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl28, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl29, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl30, lstColumnNames, "");
            BindMapingColumnsDropDown(ddl300, lstColumnNames, "");
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "GetColumnNamesFromTheExcelSheet", path);
        }

    }
    // Add Cuurent Mapping to the list
    private List<CurrentMapping> GetCurrentMapping(List<MappingInfo> ListMapping)
    {
        try
        {
            List<CurrentMapping> objMap = new List<CurrentMapping>();
            foreach (var item in ListMapping)
            {
                CurrentMapping CurrMap = new CurrentMapping();
                CurrMap.id = item.id;
                CurrMap.ColumnName = item.ColumnName;
                CurrMap.MappingColumnName = item.MappingColumnName;
                objMap.Add(CurrMap);
            }
            return objMap;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "GetCurrentMapping", path);
            return null;
        }
    }
    //Binding exceldata to mapping of dropdown
    public void BindMapingColumnsDropDown(DropDownList dropDownList, object dataSource, string selectedValue = "-Select-")
    {
        dropDownList.Items.Clear();
        dropDownList.Items.Add(new ListItem("-Select-"));
        dropDownList.DataSource = dataSource;
        dropDownList.AppendDataBoundItems = true;
        dropDownList.DataBind();

        try
        {
            dropDownList.SelectedValue = selectedValue;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindMapingColumnsDropDown", path);
            dropDownList.SelectedValue = "-Select-";
        }
    }
    // Check ExcelHeader column Exists in the List of Mapping
    private bool CheckColumnExists(string tblHeaderCoulmn, List<CurrentMapping> ListMapping)
    {
        if (ListMapping.Any(M => M.MappingColumnName == tblHeaderCoulmn))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Add coumn to DataTable
    private void BindColumntoExcel(DataTable Exceldt)
    {
        try
        {
            Exceldt.Columns.Add(new DataColumn("CategoryId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("ECatCode", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("SubCatId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("ESubCatCode", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("LocationId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("BuildingId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("FloorId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("DepartmentId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("CustodianId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("SupplierId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("Column1", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column2", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column3", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column4", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column5", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column6", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column7", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column8", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column9", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column10", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column11", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column12", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column13", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column14", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column15", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NSerialNo", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NDescription", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NQuantity", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NPrice", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NDeliveryDate", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NAssignDate", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NActive", Type.GetType("System.String")));
            //Exceldt.Columns.Add(new DataColumn("Image", Type.GetType("System.String")));

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindColumntoExcel", path);
        }

    }
    // Import Excel Date To DataBase
    private void BindImportAssetToGrid(DataTable Exceldt, List<MappingInfo> ListMapping, bool IsMappingExist)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            con.Open();
            int i = 0;
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int MaxID = 0;
                    MaxID = InsertImportedFile();
                    if (IsMappingExist == false)
                    {
                        foreach (var item in ListMapping)
                        {
                            SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportMapping", new SqlParameter[] {
                          new SqlParameter("@columnName", item.ColumnName.ToString()),
                             new SqlParameter("@MappingColumnName", item.MappingColumnName.ToString()),

                     });
                        }
                    }
                    if (!Exceldt.Columns.Contains("Image"))
                    {
                        System.Data.DataColumn StringCol = new System.Data.DataColumn("Image", typeof(System.String));
                        StringCol.DefaultValue = "";
                        Exceldt.Columns.Add(StringCol);
                    }
                    foreach (DataRow dr in Exceldt.Rows)
                    {

                        //zzz
                        using (SqlCommand cmd = new SqlCommand("PImportAsset", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TranID", MaxID);
                            cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(dr["CategoryId"]));
                            cmd.Parameters.AddWithValue("@CategoryCode", Convert.ToString(dr["ECatCode"]));
                            cmd.Parameters.AddWithValue("@SubCategoryId", Convert.ToString(dr["SubCatId"]));
                            cmd.Parameters.AddWithValue("@SubCategoryCode", Convert.ToString(dr["ESubCatCode"]));
                            cmd.Parameters.AddWithValue("@BuildingId", Convert.ToString(dr["BuildingId"]));
                            cmd.Parameters.AddWithValue("@FloorId", Convert.ToString(dr["FloorId"]));
                            cmd.Parameters.AddWithValue("@LocationId", Convert.ToString(dr["LocationId"]));
                            cmd.Parameters.AddWithValue("@DepartmentId", Convert.ToString(dr["DepartmentId"]));
                            cmd.Parameters.AddWithValue("@CustodianId", Convert.ToString(dr["CustodianId"]));
                            cmd.Parameters.AddWithValue("@SupplierId", Convert.ToString(dr["SupplierId"]));
                            cmd.Parameters.AddWithValue("@SerialNo", Convert.ToString(dr["NSerialNo"]).Trim());
                            cmd.Parameters.AddWithValue("@Description", Convert.ToString(dr["NDescription"]).Trim());
                            cmd.Parameters.AddWithValue("@Quantity", Convert.ToString(dr["NQuantity"]).Trim());
                            cmd.Parameters.AddWithValue("@Price", Convert.ToString(dr["NPrice"]).Trim());
                            cmd.Parameters.AddWithValue("@DeliveryDate", dr["NDeliveryDate"].ToString());
                            cmd.Parameters.AddWithValue("@AssignDate", dr["NAssignDate"].ToString());
                            cmd.Parameters.AddWithValue("@Active", Convert.ToString(dr["NActive"]));
                            cmd.Parameters.AddWithValue("@Column1", Convert.ToString(dr["Column1"]));
                            cmd.Parameters.AddWithValue("@Column2", Convert.ToString(dr["Column2"]));
                            cmd.Parameters.AddWithValue("@Column3", Convert.ToString(dr["Column3"]));
                            cmd.Parameters.AddWithValue("@Column4", Convert.ToString(dr["Column4"]));
                            cmd.Parameters.AddWithValue("@Column5", Convert.ToString(dr["Column5"]));
                            cmd.Parameters.AddWithValue("@Column6", Convert.ToString(dr["Column6"]));
                            cmd.Parameters.AddWithValue("@Column7", Convert.ToString(dr["Column7"]));
                            cmd.Parameters.AddWithValue("@Column8", Convert.ToString(dr["Column8"]));
                            cmd.Parameters.AddWithValue("@Column9", Convert.ToString(dr["Column9"]));
                            cmd.Parameters.AddWithValue("@Column10", Convert.ToString(dr["Column10"]));
                            cmd.Parameters.AddWithValue("@Column11", Convert.ToString(dr["Column11"]));
                            cmd.Parameters.AddWithValue("@Column12", Convert.ToString(dr["Column12"]));
                            cmd.Parameters.AddWithValue("@Column13", Convert.ToString(dr["Column13"]));
                            cmd.Parameters.AddWithValue("@Column14", Convert.ToString(dr["Column14"]));
                            cmd.Parameters.AddWithValue("@Column15", Convert.ToString(dr["Column15"]));
                            cmd.Parameters.AddWithValue("@ImportType", "Master");
                            cmd.Parameters.AddWithValue("@Image", Convert.ToString(dr["Image"]));
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close();
                                //mySQLConnection.Open();
                            }
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                                //mySQLConnection.Open();
                            }
                            //con.Open();
                            cmd.ExecuteNonQuery();
                            //con.Close();
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close();
                                //mySQLConnection.Open();
                            }
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                                //mySQLConnection.Open();
                            }
                        }
                        //       SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportAsset", new SqlParameter[] {
                        //            new SqlParameter("@TranID", MaxID),
                        //            new SqlParameter("@CategoryId", Convert.ToInt32(dr["CategoryId"])),
                        //            new SqlParameter("@CategoryCode", Convert.ToString(dr["ECatCode"])),
                        //            new SqlParameter("@SubCategoryId", Convert.ToString(dr["SubCatId"])),
                        //            new SqlParameter("@SubCategoryCode", Convert.ToString(dr["ESubCatCode"])),
                        //            new SqlParameter("@BuildingId", Convert.ToString(dr["BuildingId"])),
                        //            new SqlParameter("@FloorId", Convert.ToString(dr["FloorId"])),
                        //            new SqlParameter("@LocationId", Convert.ToString(dr["LocationId"])),
                        //            new SqlParameter("@DepartmentId", Convert.ToString(dr["DepartmentId"])),
                        //            new SqlParameter("@CustodianId", Convert.ToString(dr["CustodianId"])),
                        //            new SqlParameter("@SupplierId", Convert.ToString(dr["SupplierId"])),
                        //            new SqlParameter("@SerialNo", Convert.ToString(dr["NSerialNo"]).Trim()),
                        //            new SqlParameter("@Description", Convert.ToString(dr["NDescription"]).Trim()),
                        //            new SqlParameter("@Quantity", Convert.ToString(dr["NQuantity"]).Trim()),
                        //            new SqlParameter("@Price", Convert.ToString(dr["NPrice"]).Trim()),
                        //            new SqlParameter("@DeliveryDate", dr["NDeliveryDate"].ToString()),
                        //            new SqlParameter("@AssignDate", dr["NAssignDate"].ToString()),
                        //            new SqlParameter("@Active", Convert.ToString(dr["NActive"])),
                        //            new SqlParameter("@Column1", Convert.ToString(dr["Column1"])),
                        //            new SqlParameter("@Column2", Convert.ToString(dr["Column2"])),
                        //            new SqlParameter("@Column3", Convert.ToString(dr["Column3"])),
                        //            new SqlParameter("@Column4", Convert.ToString(dr["Column4"])),
                        //            new SqlParameter("@Column5", Convert.ToString(dr["Column5"])),
                        //            new SqlParameter("@Column6", Convert.ToString(dr["Column6"])),
                        //            new SqlParameter("@Column7", Convert.ToString(dr["Column7"])),
                        //            new SqlParameter("@Column8", Convert.ToString(dr["Column8"])),
                        //            new SqlParameter("@Column9", Convert.ToString(dr["Column9"])),
                        //            new SqlParameter("@Column10", Convert.ToString(dr["Column10"])),
                        //            new SqlParameter("@Column11", Convert.ToString(dr["Column11"])),
                        //            new SqlParameter("@Column12", Convert.ToString(dr["Column12"])),
                        //            new SqlParameter("@Column13", Convert.ToString(dr["Column13"])),
                        //            new SqlParameter("@Column14", Convert.ToString(dr["Column14"])),
                        //            new SqlParameter("@Column15", Convert.ToString(dr["Column15"])),
                        //            new SqlParameter("@ImportType", "Master"),
                        //            new SqlParameter("@Image", Convert.ToString(dr["Image"])),

                        //});
                        i++;
                    }

                    Trans.Commit();
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Imported Successfully.');", true);

                    //string Message = "Imported Successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender4.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Imported successfully!');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                catch (Exception ex)
                {
                    i = i + 1;
                    Trans.Rollback();
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " error at line number in excel file- " + (i) + "');", true);
                    Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindImportAssetToGrid", path);
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindImportAssetToGrid", path);
        }
    }
    // Insert Excel file to database to get transactionID
    public int InsertImportedFile()
    {
        int MaxId = 0;
        try
        {

            MaxId = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "PinsertExcelTransaction", new SqlParameter[] {
                        new SqlParameter("@UserId",Session["userid"] .ToString() ),
                        new SqlParameter("@FileName",this.FileName.ToString() ),
                         new SqlParameter("@CreatedDate",System.DateTime.Now ),
                          new SqlParameter("@Type","Master" ),
                           new SqlParameter("@IsDeleted","0" ),

                 }));


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "InsertImportedFile", path);
        }
        return MaxId;
    }
    // If Mapping Is already there then check the existing coulmn name with the mapping column name
    protected void btnMapping_Click(object sender, EventArgs e)
    {

        try
        {
            ModalPopupExtender2.Hide();
            AssetBL objAsset = new AssetBL();
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            ListMapping = GetSelectedMapping();
            CreateTables();



            if (ValidateData(Exceldt, ListMapping) == true)
            {
                //if (this.IsQuantitybase == true)
                //{


                if (ListMapping.Any(L => L.ColumnName == "SerialNo") == true)
                {

                    string MapSerialName = ListMapping.Where(x => x.ColumnName == "SerialNo").SingleOrDefault().MappingColumnName;
                    string SerialNo = string.Join(",", Exceldt.AsEnumerable().Where(s => s.Field<string>(MapSerialName) != null).Select(s => s.Field<string>(MapSerialName)).ToArray<string>());


                    string SerialNo_ColumnNameinExel = MapSerialName;
                    /* Check duplicate serial number in Excel */
                    foreach (DataRow row in Exceldt.Rows)
                    {

                        string s = row[SerialNo_ColumnNameinExel].ToString();
                        string qry = "[" + SerialNo_ColumnNameinExel + "]= '" + s + "'";
                        DataRow[] drx = Exceldt.Select(qry);

                        //string s = row["SerialNo"].ToString();
                        //DataRow[] drx = Exceldt.Select("[SerialNo]= '" + s + "'");
                        if (drx.Length > 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial number should be unique..');", true);
                            return;
                        }
                    }


                    if (exist_SerialNumber(SerialNo) == true)
                    {
                        lblmodmsg.Text = Assets + " already uploaded, Do you want to update new information against existing " + Assets;
                        ModalPopupExtender1.Show();
                    }
                }
                else
                {
                    ////ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
                    ValidateExcelAndBindGrid(Exceldt, ListMapping);
                }
                //}

            }
            else
            {
                ////ValidateExcelAndBindGrid(this.Exceldt, ListMapping);


                if (ddl18.SelectedIndex != 0)
                {
                    string SerialNo_ColumnNameinExel = Convert.ToString(ddl18.SelectedValue);

                    /* Check duplicate serial number in Excel */
                    foreach (DataRow row in Exceldt.Rows)
                    {
                        string s = row[SerialNo_ColumnNameinExel].ToString();
                        string qry = "[" + SerialNo_ColumnNameinExel + "]= '" + s + "'";
                        DataRow[] drx = Exceldt.Select(qry);
                        if (drx.Length > 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial number should be unique..');", true);
                            return;
                        }
                    }
                }

                ValidateExcelAndBindGrid(Exceldt, ListMapping);


            }
        }
        catch (Exception ex)
        {
            StreamWriter sw111 = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw111.WriteLine("Exception:" + ex.Message.ToString());
            sw111.Close();
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnMapping_Click", path);
        }

    }
    public void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = this.FileName;
            string path = Server.MapPath("~/UploadedExcel/" + filename);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnNo_Click", path);

        }
    }
    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    public void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.IsImport == true)
            {
                AssetBL objAsset = new AssetBL();
                List<MappingInfo> ListMapping = new List<MappingInfo>();
                ListMapping = objAsset.GetMappingListFromDB();
                ////ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
                ValidateExcelAndBindGrid(Exceldt, ListMapping);
            }
            else
            {
                // ValidateExcelAndBindGrid(this.Exceldt);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnYes_Click", path);
        }
    }
    protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();



            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getcustodianforasset, new SqlParameter[] {
                      new SqlParameter("@DepartmentId",  ddldept.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
            ddlcust.DataSource = ds;
            ddlcust.DataTextField = "CustodianName";
            ddlcust.DataValueField = "CustodianId";
            ddlcust.DataBind();
            ddlcust.Items.Insert(0, new ListItem("--Select Custodian--", "1", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "OnSelectedIndexChangedDepartment", path);
        }
    }
    protected void btnUpdateInfo_Click(object sender, System.EventArgs e)
    {
        try
        {
            divWarantyClose.Visible = false;
            divUpdateFields.Visible = true;
            txtValue.Visible = true;
            ddlstatus.Visible = false;
            bind_Field();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnUpdateInfo_Click", path);
        }
    }

    protected void btnWarnaty_Click(object sender, System.EventArgs e)
    {
        try
        {
            divWarantyClose.Visible = true;
            divUpdateFields.Visible = false;
            // divUpdateFields.Visible = true;
            BindSupplier_Warranty();
            txtstartdate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtenddate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            bind_Field();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnWarnaty_Click", path);
        }
    }


    protected void btnSubmit_Warranty_Click(object sender, EventArgs e)
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
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please select items..');", true);
                //string Message = "Please select items.";
                //imgpopup.ImageUrl = "images/Success.png";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender4.Show();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select Items');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                return;
            }


            dt.PrimaryKey = new DataColumn[] { dt.Columns["AssetId"] };
            dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
            var results = (from table1 in dt.AsEnumerable()
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Message + " .');", true);
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
                    dt.Rows.Clear();
                    grid_view();
                    gvData.DataBind();
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Assets updated successfully.');", true);

                    //string Message = Assets + " updated successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender4.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Updated successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                    ddlsupplier.SelectedIndex = 0;
                    //OnSelectedIndexChangedDepartment(sender, e);
                    txtrmk.Text = string.Empty;
                    txtstartdate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    txtenddate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    txtwarr.Text = string.Empty;
                }
            }


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnSubmit_Warranty_Click", path);
        }


    }

    private void BindSupplier_Warranty()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");

            ddlsupplier.DataSource = ds;
            ddlsupplier.DataTextField = "SupplierName";
            ddlsupplier.DataValueField = "SupplierId";
            ddlsupplier.DataBind();
            ddlsupplier.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "BindSupplier_Warranty", path);
        }

    }

    public void btnreset_Click(object sender, EventArgs e)
    {

        try
        {
            //ddlsupplier.SelectedIndex = 0;
            //txtrmk.Text = string.Empty;
            //txtstartdate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            //txtenddate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            //txtwarr.Text = string.Empty;

            txtdeldate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtass.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtprice.Text = string.Empty;
            txtdesc.Text = string.Empty;
            txtquant.Text = string.Empty;
            txtserail.Text = string.Empty;
            ddlproCategory.SelectedIndex = 0;
            // ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlcust.SelectedIndex = 0;
            DropDownList1.SelectedIndex = 0;
            chkstatus.Checked = false;

            //ddlsubcat.Items.Clear();
            //////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));

            btnsubmit.Text = "Submit";
            txtquant.Enabled = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnreset_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_PageIndexChanged", path);
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

                        //DataRow[] dr = dt_Col.Select("FieldName='Column" + i.ToString() + "' and printStatus='1'");
                        //if (dr.Length == 0)
                        //{
                        //    gvData.MasterTableView.GetColumn("column" + i.ToString()).Display = false;
                        //}
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_DataBinding", path);
        }

    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            //if (this.dt_result!=null)
            //{
            ////PrepareForExport(gridlist);
            ExportToExcel();
            //}
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnExportExcel_Click", path);
        }
    }

    private void ExportToExcel()
    {

        try
        {
            DataTable Dt_Export = new DataTable();
            DataTable DT2 = new DataTable();
            if (dt.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Dt_Export = dt.Copy();
            }
            if (Dt_Export.Rows.Count > 0)
            {

                try { Dt_Export.Columns.Remove("LocationId"); } catch { }
                try { Dt_Export.Columns.Remove("BuildingId"); } catch { }
                try { Dt_Export.Columns.Remove("FloorId"); } catch { }
                try { Dt_Export.Columns.Remove("CategoryId"); } catch { }
                try { Dt_Export.Columns.Remove("CategoryCode"); } catch { }
                try { Dt_Export.Columns.Remove("SubCatId"); } catch { }
                try { Dt_Export.Columns.Remove("SubCatCode"); } catch { }
                try { Dt_Export.Columns.Remove("CustodianId"); } catch { }
                try { Dt_Export.Columns.Remove("DepartmentId"); } catch { }
                try { Dt_Export.Columns.Remove("AssetID"); } catch { }
                try { Dt_Export.Columns.Remove("TranID"); } catch { }
                try { Dt_Export.Columns.Remove("AssetId1"); } catch { }

                //added by ponraj
                try { Dt_Export.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets); } catch { }
                try { Dt_Export.Columns["Location"].ColumnName = Location; } catch { }
                try { Dt_Export.Columns["Building"].ColumnName = Building; } catch { }
                try { Dt_Export.Columns["Floor"].ColumnName = Floor; } catch { }
                try { Dt_Export.Columns["Category"].ColumnName = Category; } catch { }
                try { Dt_Export.Columns["SubCategory"].ColumnName = SubCategory; } catch { }


                AssetBL objAsset = new AssetBL();
                List<MappingInfo> clientColumns = new List<MappingInfo>();
                clientColumns = objAsset.GetMappingListFromDB();

                foreach (DataColumn col in Dt_Export.Columns)
                {
                    if (col.ColumnName.Contains("Column"))
                    {
                        var clientValue = clientColumns.Where(c => c.ColumnName == col.ColumnName.ToString().Trim());
                        foreach (var a in clientValue)
                        {
                            if (a.MappingColumnName != null && a.MappingColumnName != "")
                            {
                                Dt_Export.Columns[a.ColumnName].ColumnName = a.MappingColumnName + "#";
                            }
                        }
                    }
                }
                DT2 = Dt_Export.Copy();
                foreach (DataColumn column in DT2.Columns)
                {
                    if (column.ColumnName.ToString().Contains("Column"))
                    {
                        Dt_Export.Columns.Remove(column.ColumnName);
                        // rpt_DT.AcceptChanges();
                    }
                }
                DataTable dtexcepcopy = Dt_Export.Copy();

                dtexcepcopy.Columns.Remove("Sub Category");
                dtexcepcopy.Columns.Remove("Department");
                dtexcepcopy.Columns.Remove("Description");
                dtexcepcopy.Columns.Remove("SerialNo");
                dtexcepcopy.Columns.Remove("Price");
                dtexcepcopy.Columns.Remove("SupplierName");
                dtexcepcopy.Columns.Remove("AssignDate");

                GridView GridView2 = new GridView();
                GridView2.AllowPaging = false;
                GridView2.DataSource = dtexcepcopy;
                GridView2.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=AssetMaster.xls");
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "ExportToExcel", path);

        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetCode");
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

            string output = "";
            for (int i = 0; i < dt_SelectedAsset.Rows.Count; i++)
            {
                output = output + dt_SelectedAsset.Rows[i]["AssetCode"].ToString();
                output += (i < dt_SelectedAsset.Rows.Count) ? "," : string.Empty;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DELETE_ASSET", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Assets", SqlDbType.VarChar).Value = output;
                    cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = Session["UserName"].ToString();

                    con.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Deleted Successfully.');", true);
                }
            }
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "btnDelete_Click", path);
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
            if (e.Item is GridDataItem)
            {
                var item = (GridDataItem)e.Item;
                if (item["Column4"].Text == "Document Controller Name")
                {
                    item["Column4"].Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_ItemDataBound", path);
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
                //item["Location"].Text = Location.ToUpper();

                CheckBox chk1 = (CheckBox)item.FindControl("checkAll");
                HiddenField3.Value = chk1.ClientID.ToString();

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_ItemCreated", path);
        }
    }
}