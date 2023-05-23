using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECommerce.Common;
using System.Data;
using System.IO;
using Tagit;
using ECommerce.Utilities;
using Serco;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;
using Microsoft.ApplicationBlocks.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class AssetIdentificationDetails : System.Web.UI.Page
{
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];

    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];

    public static DataTable dtAssetDetails = new DataTable();
    public static DataTable Exceldt = new DataTable();
    bool _MappingExist = false;

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

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

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

    public int AssetId
    {
        get
        {
            return Convert.ToInt32(ViewState["AssetId"]);
        }
        set
        {
            ViewState["AssetId"] = value;
        }
    }

    public void Bindcategory(DropDownList ddlproCategory)
    {


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");

        //ddlproCategory.DataSource = ds;
        //ddlproCategory.DataTextField = "Name";
        //ddlproCategory.DataValueField = "Name";
        //ddlproCategory.DataBind();
        //ddlproCategory.Items.Insert(0, new ListItem("--Select Category--", "0", true));

        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Category'";
        dv.RowFilter = expression;
        ddlproCategory.DataTextField = "Name";
        ddlproCategory.DataValueField = "Name";
        ddlproCategory.DataSource = dv;
        ddlproCategory.DataBind();
        ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    public void BindLocation(DropDownList ddlloc)
    {
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");

        //ddlloc.DataSource = ds;
        //ddlloc.DataTextField = "Name";
        //ddlloc.DataValueField = "Name";
        //ddlloc.DataBind();
        //ddlloc.Items.Insert(0, new ListItem("--Select Location--", "0", true));


        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Location'";
        dv.RowFilter = expression;
        ddlloc.DataTextField = "Name";
        ddlloc.DataValueField = "Name";
        ddlloc.DataSource = dv;
        ddlloc.DataBind();
        ddlloc.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    public void BindDepartMent(DropDownList ddldept)
    {
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");

        //ddldept.DataSource = ds;
        //ddldept.DataTextField = "Name";
        //ddldept.DataValueField = "Name";
        //ddldept.DataBind();

        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Department'";
        dv.RowFilter = expression;
        ddldept.DataTextField = "Name";
        ddldept.DataValueField = "Name";
        ddldept.DataSource = dv;
        ddldept.DataBind();
        ddldept.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    private void bindCustodian()
    {

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");

        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Custodian'";
        dv.RowFilter = expression;
        ddlCustodian.DataTextField = "Name";
        ddlCustodian.DataValueField = "Name";
        ddldept.DataSource = dv;
        ddlCustodian.DataBind();
        ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

    }

    public void bindBuilding(DropDownList ddlbuild, string LocationValue)
    {
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");
        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Building' and Mapping='" + LocationValue + "'";
        dv.RowFilter = expression;
        ddlbuild.DataTextField = "Name";
        ddlbuild.DataValueField = "Name";
        ddlbuild.DataSource = dv;
        ddlbuild.DataBind();
        ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));


    }

    public void bindSubCategory(DropDownList ddlsubcat, string CategoryValue)
    {

        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");
        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='SubCategory' and Mapping='" + CategoryValue + "'";
        dv.RowFilter = expression;
        ddlsubcat.DataTextField = "Name";
        ddlsubcat.DataValueField = "Name";
        ddlsubcat.DataSource = dv;
        ddlsubcat.DataBind();
        ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    public void BindFloor(DropDownList ddlfloor, string BuldingValue)
    {
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Usp_GetCompleteMasterData");
        DataView dv = new DataView(ds.Tables[0]);
        string expression = "Type='Floor' and Mapping='" + BuldingValue + "'";
        dv.RowFilter = expression;
        ddlfloor.DataTextField = "Name";
        ddlfloor.DataValueField = "Name";
        ddlfloor.DataSource = dv;
        ddlfloor.DataBind();
        ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            if (Session["userid"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            CreateTables();
            divSearch.Style.Add("display", "none");
            if (userAuthorize((int)pages.LabelPrinting, Session["userid"].ToString()) == true)
            {

                //Common.Bindcategory((DropDownList)ddlproCategory);
                //Common.BindLocation((DropDownList)ddlloc);
                //Common.BindDepartMent((DropDownList)ddldept);
                Bindcategory((DropDownList)ddlproCategory);
                BindLocation((DropDownList)ddlloc);
                BindDepartMent((DropDownList)ddldept);
                ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddldept.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));

                bindCustodian();
                grid_view();
                bind_Field();
                CompanyBL objcomp = new CompanyBL();
                DataSet ds = objcomp.getUserSetting();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.IsQuantitybase = ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1" ? true : false;
                }
                else
                {
                    this.IsQuantitybase = true;
                }
            }
            else
            {
                Response.Redirect("AcceessError.aspx");
            }

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

    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {
        bindSubCategory((DropDownList)ddlsubcat, ddlproCategory.SelectedValue);
        ddlsubcat.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        //ddlAsstID.Items.Clear();

    }

    protected void OnSelectedIndexChangedLcocation(object sender, EventArgs e)
    {
        bindBuilding((DropDownList)ddlbuild, ddlloc.SelectedValue);
        ddlfloor.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
    }

    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        BindFloor((DropDownList)ddlfloor, ddlbuild.SelectedValue);
        ddldept.SelectedIndex = 0;
    }

    protected void btnsearchsubmit_Click(object sender, EventArgs e)
    {
        // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
        grid_view();
        gvData.DataBind();
        ////ResetSearch();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
        grid_view();
        gvData.DataBind();
        ////ResetSearch();
    }

    private void ResetSearch()
    {
        ddlproCategory.SelectedValue = "0";
        ddlsubcat.Items.Clear();
        ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
        ddlloc.SelectedValue = "0";
        ddlbuild.Items.Clear();
        ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
        ddlfloor.Items.Clear();
        ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
        ddldept.SelectedValue = "0";
        //ddlAsstID.Items.Clear();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        txtAssetCode.Text = "";
        this.AssetId = 0;
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        ddlCustodian.SelectedIndex = 0;
        ddlCustodian.SelectedIndex = 0;
        // OnSelectedIndexChangedDepartment(sender, e);
        grid_view();
        gvData.DataBind();
    }


    private void grid_view()
    {
        try
        {

            string TranId = Session["Identification_ID"].ToString();
            string Asset = (Convert.ToString(this.AssetId) == "0") ? null : Convert.ToString(this.AssetId);
            string CategoryId = (ddlproCategory.SelectedItem.ToString() == "--Select--") ? null : ddlproCategory.SelectedItem.ToString();
            string SubCatId = (ddlsubcat.SelectedItem.ToString() == "--Select--") ? null : ddlsubcat.SelectedItem.ToString();
            string LocationId = (ddlloc.SelectedItem.ToString() == "--Select--") ? null : ddlloc.SelectedItem.ToString();
            string BuildingId = (ddlbuild.SelectedItem.ToString() == "--Select--") ? null : ddlbuild.SelectedItem.ToString();
            string FloorId = (ddlfloor.SelectedItem.ToString() == "--Select--") ? null : ddlfloor.SelectedItem.ToString();
            string DepartmentId = (ddldept.SelectedItem.ToString() == "--Select--") ? null : ddldept.SelectedItem.ToString();
            string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            string CustodianId = (ddlCustodian.SelectedItem.ToString() == "--Select--") ? null : ddlCustodian.SelectedItem.ToString();
            string TagType = "T6";// ddlTagType.SelectedItem.ToString();


            DataSet ds = Common.GetIdentifiedAssetDetails(TranId, Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode, CustodianId, SearchText, TagType);
            ////this.dtAssetDetails = ds.Tables[0];
            dtAssetDetails = new DataTable();
            dtAssetDetails = ds.Tables[0];

            lblcnt.Text = Convert.ToString(ds.Tables[0].Rows.Count);

            //gridlist.DataSource = ds;
            //gridlist.DataBind();
            gvData.DataSource = ds;

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }
    }

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

    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvData.ClientSettings.Scrolling.ScrollTop = "0";
    }

    protected void gvData_DataBinding(object sender, EventArgs e)
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

                    DataRow[] dr = dt_Col.Select("FieldName='Column" + i.ToString() + "'"); // and printStatus='1'
                    if (dr.Length == 0)
                    {
                        gvData.MasterTableView.GetColumn("column" + i.ToString()).Display = false;
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

    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = e.Item as GridDataItem;

        // using columnuniquename
        string ID = item["ID"].Text;

        try
        {
            AssetVerification objVer = new AssetVerification();
            objVer.ApproveOrRejectIdentifiedAssets(ID);
            grid_view();
            gvData.DataBind();

        }
        catch (Exception ex)
        {

        }

    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            Control target = e.Item.FindControl("targetControl");
            if (!Object.Equals(target, null))
            {
                if (!Object.Equals(this.RadToolTipManager1, null))
                {
                    //Add the button (target) id to the tooltip manager
                    //this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("AssetId").ToString(), true);
                    string t = item["ImageName"].Text.ToLower();
                    this.RadToolTipManager1.TargetControls.Add(target.ClientID, t, true);

                }
            }
        }
       
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRow[] Exist_CatName = dt_cat.Select("CategoryName='" + item["Category"].Text.ToLower() + "'");
            if (Exist_CatName.Length == 0)
            {
                item["Category"].ForeColor = Color.Red; // chanmge particuler cell
                //e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
            }

            DataRow[] Exist_SubCatName = dt_subcat.Select("SubCatName='" + item["SubCategory"].Text.ToLower() + "'");

            if (Exist_SubCatName.Length == 0)
            {
                item["SubCategory"].ForeColor = Color.Red;
            }

            DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + item["Location"].Text.ToLower() + "'");
            if (Exist_LOCATION.Length == 0)
            {
                item["Location"].ForeColor = Color.Red; // chanmge particuler cell
                                                        //e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
            }

            DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + item["Building"].Text.ToLower() + "'");
            if (Exist_BUILDING.Length == 0)
            {
                item["Building"].ForeColor = Color.Red;
            }

            DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + item["Floor"].Text.ToLower() + "'");
            if (Exist_Floor.Length == 0)
            {
                item["Floor"].ForeColor = Color.Red;
            }

            DataRow[] Exist_DEPARTMENT = dt_department.Select("DepartmentName='" + item["Department"].Text.ToLower() + "'");
            if (Exist_DEPARTMENT.Length == 0)
            {
                item["Department"].ForeColor = Color.Red; // chanmge particuler cell
            }

            DataRow[] Exist_CUSTODIAN = dt_custodian.Select("CustodianName='" + item["Custodian"].Text.ToLower() + "'");
            if (Exist_CUSTODIAN.Length == 0)
            {
                item["Custodian"].ForeColor = Color.Red; // chanmge particuler cell
            }

            DataRow[] Exist_SUPPLIERNAME = dt_supplier.Select("SupplierName='" + item["Supplier"].Text.ToLower() + "'");
            if (Exist_SUPPLIERNAME.Length == 0)
            {
                item["Supplier"].ForeColor = Color.Red; // chanmge particuler cell
            }

        }

        //Added By ponraj
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = e.Item as GridHeaderItem;
            item["AssetCode"].Text = Assets.ToUpper() + "CODE";
            item["Location"].Text = Location.ToUpper();
            item["Building"].Text = Building.ToUpper();
            item["Floor"].Text = Floor.ToUpper();
            item["Category"].Text = Category.ToUpper();
            item["SubCategory"].Text = SubCategory.ToUpper();
        }

    }

    private void CreateTables()
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

    protected void cboField_SelectedIndexChanged(object sender, EventArgs e)
    {

        string value = cboField.SelectedValue;
        txtValue.Text = "";
        if (value == "DepartmentName" || value == "CustodianName" || value == "SupplierName" || value == "TagType"
            || value == "CategoryName" || value == "SubCategoryName" || value == "LocationName" || value == "BuildingName"
            || value == "FloorName")
        {
            cboValue.Enabled = true;
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

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "CategoryName";
            cboValue.DataValueField = "CategoryId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));

            cboValue.Enabled = false;            
            txtValue.Enabled = true;
            txtValue.ReadOnly = false;
            // txtValue.Visible = true;
        }

        if (value == "CategoryName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "CategoryName";
            cboValue.DataValueField = "CategoryId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }


        if (value == "SubCategoryName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveSubCat");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "SubCatName";
            cboValue.DataValueField = "SubCatId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        if (value == "LocationName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "LocationName";
            cboValue.DataValueField = "LocationId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        if (value == "BuildingName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveBuilding");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "BuildingName";
            cboValue.DataValueField = "BuildingId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        if (value == "FloorName")
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlDataAdapter dpt = new SqlDataAdapter();


            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveFloor");

            cboValue.DataSource = ds;
            cboValue.DataTextField = "FloorName";
            cboValue.DataValueField = "FloorId";
            cboValue.DataBind();
            cboValue.Items.Insert(0, new ListItem("--Select--", "0", true));
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

    private void bind_Field()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetMappingLists");
        // if (ds != null && ds.Tables.Count > 0)
        //{

        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //if (dr["id"]. == 7)
        //    dr.Delete();
        //}
        //dt.AcceptChanges();
        //}
        if (ds != null && ds.Tables.Count > 0)
        {
            cboField.DataSource = ds;
            cboField.DataTextField = "MappingColumnName";
            cboField.DataValueField = "ColumnName";
            cboField.DataBind();
            foreach (ListItem ls in cboField.Items)
            {
                if (ls.Text.Contains("Asset"))
                {
                    ls.Text = ls.Text.Replace("Asset", Assets);
                }
                if (ls.Text=="Category")
                {
                    ls.Text = ls.Text.Replace("Category", Category);

                }
                if (ls.Text=="Sub Category")
                {
                    ls.Text = ls.Text.Replace("Sub Category", SubCategory);
                }
                if (ls.Text == "Location")
                {
                    ls.Text = ls.Text.Replace("Location", Location);
                }
                if (ls.Text == "Building")
                {
                    ls.Text = ls.Text.Replace("Building", Building);
                }
                if (ls.Text == "Floor")
                {
                    ls.Text = ls.Text.Replace("Floor", Floor);
                }
            }
        }
        cboField.Items.Insert(0, new ListItem("--Select--", "0", true));

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
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


                if (dtAssetDetails.Rows.Count > 0)
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
                            string Message = "Please select items..";
                            imgpopup.ImageUrl = "images/Success.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
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
                            //val = cboValue.SelectedValue.ToString();
                            val = cboValue.SelectedItem.ToString();
                        }
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SP_ASSET_BULKUPDATE_IDENTIFIED_ASSETS", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@Assets", SqlDbType.VarChar).Value = output;
                                cmd.Parameters.Add("@Column", SqlDbType.VarChar).Value = cboField.SelectedValue;
                                cmd.Parameters.Add("@Values", SqlDbType.VarChar).Value = val;
                                cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = Session["UserName"].ToString();

                                con.Open();
                                cmd.ExecuteNonQuery();
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Assets updated successfully.');", true);

                                string Message = Assets+" updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();

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
        catch
        {

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AssetIdentification.aspx");
        }
        catch
        {
        }
    }

    #region ApproveAssets

    public void ImportAssets()
    {

        DataSet ds = new DataSet();

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

    // Add Cuurent Mapping to the list
    private List<CurrentMapping> GetCurrentMapping(List<MappingInfo> ListMapping)
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

    // Add coumn to DataTable
    private void BindColumntoExcel(DataTable Exceldt)
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



    }

    public static void SetColumnsOrder(DataTable table, List<MappingInfo> ListMapping)
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

        }

        //foreach (var columnName in ListMapping)
        //{
        //    switch (columnName.ColumnName.ToString())
        //    {
        //        case "CategoryName":
        //            {
        //                table.Columns[columnName.MappingColumnName].SetOrdinal(0);
        //                break;
        //            }
        //        case "SubCategoryName":
        //            {
        //                table.Columns[columnName.MappingColumnName].SetOrdinal(1);
        //                break;
        //            }
        //        case "LocationName":
        //            {
        //                table.Columns[columnName.MappingColumnName].SetOrdinal(2);
        //                break;
        //            }
        //        case "BuildingName":
        //            {
        //                table.Columns[columnName.MappingColumnName].SetOrdinal(3);
        //                break;
        //            }
        //        case "FloorName":
        //            {
        //                table.Columns[columnName.MappingColumnName].SetOrdinal(4);
        //                break;
        //            }
        //    }
        //}
    }

    private void ValidateExcelAndBindGrid(DataTable Exceldt, List<MappingInfo> ListMapping)
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
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - "+Category+" not Exists in the master" + "');", true);
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
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - "+SubCategory+" not available in  master" + "');", true);
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
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - "+Category+" not found for this "+SubCategory+" in  master" + "');", true);
                                        return;
                                    }
                                    string ExcelColumnName = CurrentMapping.Where(x => x.ColumnName == "SubCategoryName").FirstOrDefault().MappingColumnName;
                                    string FurnitureNameFromExcel = Exceldt.Rows[i][ExcelColumnName].ToString();
                                    DataRow[] drSubcat = dt_subcat.Select("SubCatName='" + FurnitureNameFromExcel.Trim() + "'" + " and CategoryId= '" + CategoryId.Trim() + "'");
                                    if (drSubcat.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - "+Category+" and "+SubCategory+" is not mattched in  master" + "');", true);
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
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - "+Floor+" is not available in masters" + "');", true);
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
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - "+Building+"  is not available in masters" + "');", true);
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
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Location  is not available in masters" + "');", true);
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
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - "+Building+" is not mapped with existing "+Location+" master " + "');", true);
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
                                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + FloorTextFromExcel.Trim() + " -  "+Floor+" is not mapped with existing "+Building+" and "+Location+" in master" + "');", true);
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


                    }
                }
                else
                {
                    if (j <= 15)
                    {
                        MappingInfo objInfo = new MappingInfo();
                        string ExcelValue = dr[tblHeaderCoulmn].ToString();
                        string dbColumn = "Column" + j;
                        Exceldt.Rows[i]["Column" + j] = ExcelValue;
                        Exceldt.AcceptChanges();
                        if (ListMapping.Any(M => M.ColumnName == dbColumn) == false)
                        {
                            var maxID = ListMapping.Count();
                            switch (dbColumn)
                            {
                                case "Column1":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column2":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column3":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column4":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column5":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column6":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column7":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column8":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column9":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column10":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column11":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column12":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column13":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column14":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                case "Column15":
                                    {
                                        objInfo.id = j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                            }
                        }
                        j++;
                    }
                }

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


            if (Exceldt.Columns.Count != ListMapping.Count)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel file does not contain all the column you mapped.');", true);
                return false;
            }


            // Loop On Columns
            for (int i = 0; i < ListMapping.Count; i++)
            {
                if (ListMapping.Any(L => L.MappingColumnName == Exceldt.Columns[i].ColumnName) == false)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + Exceldt.Columns[i].ColumnName + " is Invalid.');", true);
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }


    }

    public int InsertImportedFile()
    {
        int MaxId = 0;
        try
        {

            MaxId = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "PinsertExcelTransaction", new SqlParameter[] {
                        new SqlParameter("@UserId",Session["userid"] .ToString() ),
                        new SqlParameter("@FileName","" ),
                         new SqlParameter("@CreatedDate",System.DateTime.Now ),
                          new SqlParameter("@Type","AssetIdentifiedMaster" ),
                           new SqlParameter("@IsDeleted","0" ),

                 }));


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
        }
        return MaxId;
    }

    // Import Excel Date To DataBase
    private void BindImportAssetToGrid(DataTable Exceldt, List<MappingInfo> ListMapping, bool IsMappingExist)
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

                foreach (DataRow dr in Exceldt.Rows)
                {


                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportAsset", new SqlParameter[] {
                             new SqlParameter("@TranID", MaxID),
                             new SqlParameter("@CategoryId", Convert.ToInt32(dr["CategoryId"])),
                             new SqlParameter("@CategoryCode", Convert.ToString(dr["ECatCode"])),
                             new SqlParameter("@SubCategoryId", Convert.ToString(dr["SubCatId"])),
                             new SqlParameter("@SubCategoryCode", Convert.ToString(dr["ESubCatCode"])),
                             new SqlParameter("@BuildingId", Convert.ToString(dr["BuildingId"])),
                             new SqlParameter("@FloorId", Convert.ToString(dr["FloorId"])),
                             new SqlParameter("@LocationId", Convert.ToString(dr["LocationId"])),
                             new SqlParameter("@DepartmentId", Convert.ToString(dr["DepartmentId"])),
                             new SqlParameter("@CustodianId", Convert.ToString(dr["CustodianId"])),
                             new SqlParameter("@SupplierId", Convert.ToString(dr["SupplierId"])),
                             new SqlParameter("@SerialNo", Convert.ToString(dr["NSerialNo"]).Trim()),
                             new SqlParameter("@Description", Convert.ToString(dr["NDescription"]).Trim()),
                             new SqlParameter("@Quantity", Convert.ToString(dr["NQuantity"]).Trim()),
                             new SqlParameter("@Price", Convert.ToString(dr["NPrice"]).Trim()),
                             new SqlParameter("@DeliveryDate", dr["NDeliveryDate"].ToString()),
                             new SqlParameter("@AssignDate", dr["NAssignDate"].ToString()),
                             new SqlParameter("@Active", Convert.ToString(dr["NActive"])),
                             new SqlParameter("@Column1", Convert.ToString(dr["Column1"])),
                             new SqlParameter("@Column2", Convert.ToString(dr["Column2"])),
                             new SqlParameter("@Column3", Convert.ToString(dr["Column3"])),
                             new SqlParameter("@Column4", Convert.ToString(dr["Column4"])),
                             new SqlParameter("@Column5", Convert.ToString(dr["Column5"])),
                             new SqlParameter("@Column6", Convert.ToString(dr["Column6"])),
                             new SqlParameter("@Column7", Convert.ToString(dr["Column7"])),
                             new SqlParameter("@Column8", Convert.ToString(dr["Column8"])),
                             new SqlParameter("@Column9", Convert.ToString(dr["Column9"])),
                             new SqlParameter("@Column10", Convert.ToString(dr["Column10"])),
                             new SqlParameter("@Column11", Convert.ToString(dr["Column11"])),
                             new SqlParameter("@Column12", Convert.ToString(dr["Column12"])),
                             new SqlParameter("@Column13", Convert.ToString(dr["Column13"])),
                             new SqlParameter("@Column14", Convert.ToString(dr["Column14"])),
                             new SqlParameter("@Column15", Convert.ToString(dr["Column15"])),
                             new SqlParameter("@ImportType", "Master"),

                 });
                    i++;
                }

                Trans.Commit();
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Imported Successfully.');", true);

                string Message = "Imported Successfully.";
                imgpopup.ImageUrl = "images/Success.png";
                lblpopupmsg.Text = Message;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
            }
            catch (Exception ex)
            {
                i = i + 1;
                Trans.Rollback();
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " error at line number in excel file- " + (i) + "');", true);

            }
        }

    }

    #endregion

    protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
    {
        this.UpdateToolTip(args.Value, args.UpdatePanel);
    }
    private void UpdateToolTip(string elementID, UpdatePanel panel)
    {
        Control ctrl = Page.LoadControl("ProductDetailsCS.ascx");
        ctrl.ID = "UcProductDetails1";
        panel.ContentTemplateContainer.Controls.Add(ctrl);
        usercontrol_ProductDetailsCS details = (usercontrol_ProductDetailsCS)ctrl;
        details.ImageName = elementID;
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Sort" || e.CommandName == "Page")
        {
            RadToolTipManager1.TargetControls.Clear();
        }

    }


    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        //Added By ponraj
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = e.Item as GridHeaderItem;
            item["AssetCode"].Text = Assets.ToUpper() + "CODE";
            item["Location"].Text = Location.ToUpper();
            item["Building"].Text = Building.ToUpper();
            item["Floor"].Text = Floor.ToUpper();
            item["Category"].Text = Category.ToUpper();
            item["SubCategory"].Text = SubCategory.ToUpper();
        }
    }
}