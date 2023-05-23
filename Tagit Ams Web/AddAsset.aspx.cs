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
using System.Web.UI.WebControls;


public partial class AddAsset : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
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

    public DataTable dt_asset { get; set; }
    public DataTable Exceldt
    {
        get
        {
            return ViewState["Exceldt"] as DataTable;
        }
        set
        {
            ViewState["Exceldt"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlproCategory.Attributes.Add("onchange", "myFunction();");
        if (!IsPostBack)
        {
            Page.DataBind();
            if (userAuthorize((int)pages.AssetMaster, Session["userid"].ToString()) == true)
            {
                CompanyBL objcomp = new CompanyBL();
                DataSet ds = objcomp.getUserSetting();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.IsImport = ds.Tables[0].Rows[0]["ImportType"].ToString() == "1" ? true : false;
                    this.IsQuantitybase = ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1" ? true : false;
                }
                else
                {
                    this.IsImport = true;
                    this.IsQuantitybase = true;
                }
                if (IsImport == true)//When import required
                {

                    SearchDiv.Visible = false;
                    divImport.Visible = true;
                    btnsubmit.Visible = false;
                    btnreset.Visible = false;
                    btnImport.Visible = true;
                }

                if (IsImport == false)//When import not required
                {
                    SearchDiv.Visible = true;
                    divImport.Visible = false;
                    btnsubmit.Visible = true;
                    btnreset.Visible = true;
                    btnImport.Visible = false;
                }

                Bincategory();
                ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
                bindlocation();
                ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
                ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
                ddlcust.Items.Insert(0, new ListItem("--Select Custodian--", "0", true));
                BindDepartment();
                BindSupplier();
                txtdeldate.Text = System.DateTime.Now.ToString("MM-dd-yyyy");
                txtass.Text = System.DateTime.Now.ToString("MM-dd-yyyy");

                grid_view();
            }
            else
            {
                Response.Redirect("AcceessError.aspx");               
            }

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
    private void Bincategory()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

        ddlproCategory.DataSource = ds;
        ddlproCategory.DataTextField = "CategoryName";
        ddlproCategory.DataValueField = "CategoryCode";
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
    private void BindSupplier()
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");

        DropDownList1.DataSource = ds;
        DropDownList1.DataTextField = "SupplierName";
        DropDownList1.DataValueField = "SupplierId";
        DropDownList1.DataBind();
        DropDownList1.Items.Insert(0, new ListItem("--Select Supplier--", "0", true));




    }
    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();



        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getSubCategory, new SqlParameter[] { 
                      new SqlParameter("@CategoryCode",  ddlproCategory.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        ddlsubcat.DataSource = ds;
        ddlsubcat.DataTextField = "SubCatName";
        ddlsubcat.DataValueField = "SubCatCode";
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
    protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
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
        ddlcust.Items.Insert(0, new ListItem("--Select Custodian--", "0", true));
    }

    public void btnreset_Click(object sender, EventArgs e)
    {
        txtdeldate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtass.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        txtprice.Text = string.Empty;
        txtdesc.Text = string.Empty;
        txtquant.Text = string.Empty;
        txtserail.Text = string.Empty;
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        ddlcust.SelectedIndex = 0;
        DropDownList1.SelectedIndex = 0;
        chkstatus.Checked = false;

        btnsubmit.Text = "Submit";
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {

        if (btnsubmit.Text == "Submit")
        {
            if (exist_SerialNumber(txtserail.Text.Trim()) == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial No. Already Exist, \\n Please Insert Different Serial No..!!');", true);
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
                             new SqlParameter("@SubCategoryId", ddlsubcat.SelectedValue),
                             new SqlParameter("@SubCategoryName", ddlsubcat.SelectedItem.Text),
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
                             new SqlParameter("@DeliveryDate",txtdeldate.Text.Trim().ToString()),
                             new SqlParameter("@AssignDate", txtass.Text.Trim().ToString()),
                             new SqlParameter("@Active", chkstatus.Checked == true ? 1 : 0),
                             new SqlParameter("@ImportType", "Manual"),
                    
                        }
                                );

                        Trans.Commit();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Inserted Successfully..!!');", true);
                    }
                    catch (Exception ex)
                    {
                        Trans.Rollback();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);

                    }
                }
            }


        }
        else if (btnsubmit.Text == "Update")
        {
            // if (exist_SerialNumber(txtserail.Text.Trim()) == true)
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial No. Already Exist, \\n Please Insert Different Serial No..!!');", true);
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
           
                            new SqlParameter("@AssetId", Convert.ToInt32(hdncatidId.Value)), 
                           //new SqlParameter("@AssetCode",hidcatcode.Value.ToString()),
                   new SqlParameter("@CategoryId", ddlproCategory.SelectedValue),
                   new SqlParameter("@CategoryName", ddlproCategory.SelectedItem.Text),
                             new SqlParameter("@SubCategoryId", ddlsubcat.SelectedValue),
                             new SqlParameter("@SubCategoryName", ddlsubcat.SelectedItem.Text),
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
                                   new SqlParameter("@DeliveryDate", txtdeldate.Text.Trim().ToString()),
                                   new SqlParameter("@AssignDate", txtass.Text.Trim().ToString()),
                    new SqlParameter("@Active", chkstatus.Checked == true ? 1 : 0),                     
                    
                    
                }
               );


                    Trans.Commit();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully..!!');", true);
                    // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully..!!');", true);
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);

                }

                btnsubmit.Text = "Add";

            }

            // }

        }
        hdncatidId.Value = "";
        grid_view();
        btnreset_Click(sender, e);



    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Getassetdetail");
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {

                lblMessage.Text = "Problem occured while retrieving Product records. Please try again.";
            }
            else
            {
                DataTable dt = ds.Tables[0];

                DataView myView;
                myView = ds.Tables[0].DefaultView;
                lblcnt.Text = Convert.ToString(dt.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                gridlist.DataSource = myView;

                gridlist.DataBind();

            }

        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }
    protected void gridlist_SortCommand(Object sender, DataGridSortCommandEventArgs e)
    {
        if (lblSort.Text == "asc")
        {
            lblSort.Text = "desc";
        }
        else
        {
            lblSort.Text = "asc";
        }
        StrSort = e.SortExpression + " " + lblSort.Text;
        grid_view();
    }
    protected void myDataGrid_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    {
        gridlist.CurrentPageIndex = e.NewPageIndex;
        grid_view();
    }

    private void ShowSuccessMessage(string msg)
    {
        lblMessage.Text = msg;
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        try
        {

            HiddenField hidcatid = e.Item.Cells[0].FindControl("hidcatid") as HiddenField;

            HiddenField SubCatId = e.Item.Cells[0].FindControl("SubCatId") as HiddenField;
            HiddenField hidlocid = e.Item.Cells[0].FindControl("hidlocid") as HiddenField;

            HiddenField hidBldid = e.Item.Cells[0].FindControl("hidBldid") as HiddenField;


            HiddenField hidflrid = e.Item.Cells[0].FindControl("hidflrid") as HiddenField;
            HiddenField hiddptid = e.Item.Cells[0].FindControl("hiddptid") as HiddenField;

            HiddenField hidcstid = e.Item.Cells[0].FindControl("hidcstid") as HiddenField;
            HiddenField hidsplrid = e.Item.Cells[0].FindControl("hidsplrid") as HiddenField;
            Label SerialNo = e.Item.Cells[0].FindControl("SerialNo") as Label;
            Label Description = e.Item.Cells[0].FindControl("Description") as Label;
            Label Quantity = e.Item.Cells[0].FindControl("Quantity") as Label;
            Label Price = e.Item.Cells[0].FindControl("Price") as Label;
            Label DeliveryDate = e.Item.Cells[0].FindControl("DeliveryDate") as Label;
            Label AssignDate = e.Item.Cells[0].FindControl("AssignDate") as Label;




            Label AssetId = e.Item.Cells[0].FindControl("AssetId") as Label;

            Label Active = e.Item.Cells[0].FindControl("Active") as Label;
            Label AssetCode = e.Item.Cells[0].FindControl("AssetCode") as Label;

            hidcatcode.Value = AssetCode.Text;
            hdncatidId.Value = AssetId.Text;

            ddlproCategory.SelectedValue = hidcatid.Value;
            OnSelectedIndexChangedCategory(sender, e);
            ddlsubcat.SelectedValue = SubCatId.Value;
            ddlloc.SelectedValue = hidlocid.Value;
            OnSelectedIndexChangedLocation(sender, e);
            ddlbuild.SelectedValue = hidBldid.Value;
            OnSelectedIndexChangedBuilding(sender, e);
            ddlfloor.SelectedValue = hidflrid.Value;

            ddldept.SelectedValue = hiddptid.Value;
            OnSelectedIndexChangedDepartment(sender, e);
            ddlcust.SelectedValue = hidcstid.Value;
            DropDownList1.SelectedValue = hidsplrid.Value;
            txtdeldate.Text = DeliveryDate.Text;
            txtass.Text = AssignDate.Text;
            txtprice.Text = Price.Text;
            txtdesc.Text = Description.Text;
            txtquant.Text = Quantity.Text;
            txtserail.Text = SerialNo.Text;
            if (Active.Text == "Active")
            {
                chkstatus.Checked = true;
            }
            else
            {
                chkstatus.Checked = false;
            }





            btnsubmit.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
        }
    }
    // Import Excel Sheet Asset
    protected void btnImport_Click(object sender, EventArgs e)
    {

        if ((productimguploder.HasFile))
        {
            DataSet ds = GetDateFromExcelSheet();

            if (ds.Tables[0].Rows.Count > 0)
            {
                this.Exceldt = ds.Tables[0];
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
                            if (this.IsQuantitybase == false)
                            {
                                if (ListMapping.Any(L => L.ColumnName == "SerialNo") == true)
                                {
                                    string MapSerialName = ListMapping.Where(x => x.ColumnName == "SerialNo").SingleOrDefault().MappingColumnName;
                                    object SerialNo = string.Join(",", Exceldt.AsEnumerable().Select(s => s.Field<object>(MapSerialName)).ToArray<object>());
                                    if (exist_SerialNumber(SerialNo.ToString()) == true)
                                    {
                                        lblmodmsg.Text = "Assets already uploaded, Do you want to update new information against existing assets";
                                        ModalPopupExtender1.Show();
                                    }
                                }
                                else
                                {
                                    ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
                                }
                            }
                        }
                        else
                        {
                            ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
                        }                   

                }

            }
        }

        else
        {
            lblMessage.Text = "Please select an excel file first";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }


        //     CreateTables();

        //    if (ValidateData(Exceldt) == true)
        //{

        //    string SerialNo = string.Join(",", Exceldt.AsEnumerable().Select(s => s.Field<string>("SerialNumber")).ToArray<string>());
        //    if (exist_SerialNumber(SerialNo) == true)
        //    {
        //        lblmodmsg.Text = "Assets already uploaded, Do you want to update new information against existing assets";
        //        ModalPopupExtender1.Show();

        //    }
        //    else
        //    {
        //        ValidateExcelAndBindGrid(this.Exceldt);
        //    }

        //}
        //else
        //{
        //    lblMessage.Text = "Please select an excel file first";
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    lblMessage.Visible = true;
        //}

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
                if (this.IsQuantitybase == false)
                {
                    if (ListMapping.Any(L => L.ColumnName == "SerialNo") == true)
                    {
                        string MapSerialName = ListMapping.Where(x => x.ColumnName == "SerialNo").SingleOrDefault().MappingColumnName;
                        string SerialNo = string.Join(",", Exceldt.AsEnumerable().Select(s => s.Field<string>(MapSerialName)).ToArray<string>());
                        if (exist_SerialNumber(SerialNo) == true)
                        {
                            lblmodmsg.Text = "Assets already uploaded, Do you want to update new information against existing assets";
                            ModalPopupExtender1.Show();
                        }
                    }
                    else
                    {
                        ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
                    }
                }
            }
            else
            {
                ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    // Add Data to Datatable on the basis of new mapping or existing Mapping.
    private void ValidateExcelAndBindGrid(DataTable dataTable, List<MappingInfo> ListMapping)
    {
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
                    switch (DBColumnname)
                    {
                        case "CategoryName":
                            {
                                DataRow[] Exist_CatName = dt_cat.Select("CategoryName='" + ExcelValue + "'");
                                if (Exist_CatName.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Category not Exists in the master" + "');", true);
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
                                DataRow[] Exist_SubCatName = dt_subcat.Select("SubCatName='" + ExcelValue + "'");
                                if (Exist_SubCatName.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Sub_Category not available in  master" + "');", true);
                                    return;
                                }
                                else
                                {
                                    Exceldt.Rows[i]["SubCatId"] = Convert.ToInt32(Exist_SubCatName[0].ItemArray[0]);
                                    Exceldt.Rows[i]["ESubCatCode"] = Exist_SubCatName[0].ItemArray[2].ToString();

                                    Exceldt.AcceptChanges();
                                }
                                break;
                            }

                        case "FloorName":
                            {
                                DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue + "'");
                                if (Exist_Floor.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Floor is not available in masters" + "');", true);
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
                                DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + ExcelValue + "'");
                                if (Exist_BUILDING.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Building  is not available in masters" + "');", true);
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
                                DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + ExcelValue + "'");
                                if (Exist_LOCATION.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Location  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    Exceldt.Rows[i]["LocationId"] = Convert.ToInt32(Exist_LOCATION[0].ItemArray[0]);
                                    Exceldt.AcceptChanges();
                                }
                                break;
                            }
                        case "DepartmentName":
                            {
                                DataRow[] Exist_DEPARTMENT = dt_department.Select("DepartmentName='" + ExcelValue + "'");
                                if (Exist_DEPARTMENT.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Department  is not available in masters" + "');", true);
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
                                DataRow[] Exist_CUSTODIAN = dt_custodian.Select("CustodianName='" + ExcelValue + "'");
                                if (Exist_CUSTODIAN.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Custodian  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    Exceldt.Rows[i]["DepartmentId"] = Convert.ToInt32(Exist_CUSTODIAN[0].ItemArray[0]);
                                    Exceldt.AcceptChanges();
                                }
                                break;
                            }
                        case "SupplierName":
                            {
                                DataRow[] Exist_SUPPLIERNAME = dt_supplier.Select("SupplierName='" + ExcelValue + "'");
                                if (Exist_SUPPLIERNAME.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Supplier  is not available in masters" + "');", true);
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
                                    Exceldt.Rows[i]["NQuantity"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
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
                                Exceldt.Rows[i]["Column"] = ExcelValue.ToString();
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
                                        objInfo.id =j;
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
                                        objInfo.id =j;
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
                                        objInfo.id =j;
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
                                        objInfo.id =j;
                                        objInfo.ColumnName = dbColumn;
                                        objInfo.MappingColumnName = tblHeaderCoulmn;
                                        ListMapping.Add(objInfo);
                                        break;
                                    }
                                //case "Column16":
                                //    {
                                //        objInfo.id = Convert.ToInt32(maxID) + 1;
                                //        objInfo.ColumnName = dbColumn;
                                //        objInfo.MappingColumnName = tblHeaderCoulmn;
                                //        ListMapping.Add(objInfo);
                                //        break;
                                //    }

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
            BindImportAssetToGrid(Exceldt,ListMapping,this.MappingExist);
            grid_view();
        //}
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
    //Import ExcelData to DataBase
    private void BindImportAssetToGrid(DataTable Exceldt, List<MappingInfo> ListMapping, bool IsMappingExist)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        con.Open();
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
                             new SqlParameter("@DeliveryDate", Convert.ToString(dr["NDeliveryDate"])),
                             new SqlParameter("@AssignDate", Convert.ToString(dr["NAssignDate"])),
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
                }

                Trans.Commit();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Imported Successfully..!!');", true);
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);


            }
        }

    }

    private void BindAssetToGrid(DataTable Exceldt, int MaxId)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        con.Open();
        using (SqlTransaction Trans = con.BeginTransaction())
        {
            try
            {

                foreach (DataRow dr in Exceldt.Rows)
                {

                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportAsset", new SqlParameter[] { 
                             new SqlParameter("@TranID", MaxId),
                             new SqlParameter("@CategoryId", Convert.ToInt32(dr["CategoryId"])),
                             new SqlParameter("@CategoryName", Convert.ToString(dr["CATEGORY"])),
                             new SqlParameter("@SubCategoryId", Convert.ToInt32(dr["SubCatId"])),
                             new SqlParameter("@SubCategoryName", Convert.ToString(dr["SUB-CATEGORY"])),
                             new SqlParameter("@BuildingId", Convert.ToInt32(dr["BuildingId"])),
                             new SqlParameter("@FloorId", Convert.ToInt32(dr["FloorId"])),                            
                             new SqlParameter("@LocationId", Convert.ToInt32(dr["LocationId"])),
                             new SqlParameter("@DepartmentId", Convert.ToInt32(dr["DepartmentId"])),
                             new SqlParameter("@CustodianId", Convert.ToInt32(dr["CustodianId"])),
                             new SqlParameter("@SupplierId", Convert.ToInt32(dr["SupplierId"])),                             
                             new SqlParameter("@SerialNo", Convert.ToString(dr["SerialNumber"]).Trim()),
                             new SqlParameter("@Description", Convert.ToString(dr["AssetDescription"]).Trim()),
                             new SqlParameter("@Quantity", Convert.ToString(dr["Quantity"]).Trim()),
                             new SqlParameter("@Price", Convert.ToString(dr["Price"]).Trim()),
                             new SqlParameter("@DeliveryDate", Convert.ToDateTime(dr["DeliveryDate"])),
                             new SqlParameter("@AssignDate", Convert.ToDateTime(dr["AssignDate"])),
                             new SqlParameter("@Active", Convert.ToInt32(dr["Status"])),
                             new SqlParameter("@ImportType", "Master"),

                 });

                }
                Trans.Commit();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Imported Successfully..!!');", true);
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
            }
        }




        //gridlist.DataSource = Exceldt;
        //gridlist.DataBind();


        //lblMessage.Text = "Data retrieved successfully! Total Recodes:" + ds.Tables[0].Rows.Count;
        //lblMessage.ForeColor = System.Drawing.Color.Green;
        //lblMessage.Visible = true;
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

    private void ValidateExcelAndBindGrid(DataTable Exceldt)
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
                sb.Append("Category not Exists in the master \\n");


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
                sb.Append("Sub_Category not available in  master \\n ");


            }
            else
            {
                Exceldt.Rows[i]["SubCatId"] = Convert.ToInt32(Exist_SubCatName[0].ItemArray[0]);
                Exceldt.AcceptChanges();
            }


            string Floor = dr["FLOOR"].ToString();
            DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + Floor + "'");
            if (Exist_Floor.Length == 0)
            {
                sb.Append("Floor is not available in masters \\n ");


            }
            else
            {
                Exceldt.Rows[i]["FloorId"] = Convert.ToInt32(Exist_Floor[0].ItemArray[0]);
                Exceldt.AcceptChanges();
            }
            string BUILDING = dr["BUILDING"].ToString();
            DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + BUILDING + "'");
            if (Exist_BUILDING.Length == 0)
            {
                sb.Append("Building  is not available in masters \\n ");


            }

            else
            {
                Exceldt.Rows[i]["BuildingId"] = Convert.ToInt32(Exist_BUILDING[0].ItemArray[0]);
                Exceldt.AcceptChanges();
            }
            string LOCATION = dr["LOCATION"].ToString();
            DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + LOCATION + "'");
            if (Exist_LOCATION.Length == 0)
            {
                sb.Append("Location  is not available in masters \\n ");

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
                sb.Append("Department  is not available in masters \\n ");

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
                sb.Append("Custodian  is not available in masters \\n ");


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
                sb.Append("Supplier  is not available in masters \\n ");


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
        //MaxID = InsertImportedFile();
        //if (MaxID > 0)
        //{
        //    BindAssetToGrid(Exceldt, MaxID);
        //    grid_view();
        //}
    }

    private bool ValidateData(DataTable Exceldt, List<MappingInfo> ListMapping)
    {
        if (MappingExist == true)
        {
            ////Check Columns Count
            //if (Exceldt.Columns.Count.ToString() != ListMapping.Count().ToString())
            //{

            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel File Dont have all columns which are required...!!');", true);
            //    return false;
            //}
            if (Exceldt.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel fies does not contain any Row...!!');", true);
                return false;
            }

            // Loop On Columns
            for (int i = 0; i < ListMapping.Count; i++)
            {
                if (ListMapping.Any(L => L.MappingColumnName == Exceldt.Columns[i].ColumnName) == false)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + Exceldt.Columns[i].ColumnName + "is Invalid..!!');", true);
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

    private List<MappingInfo> GetSelectedMapping()
    {
        List<MappingInfo> ListMapping = new List<MappingInfo>();
        if (ddl1.SelectedValue != "-Select-")
        {
            MappingInfo objInfo = new MappingInfo();
            objInfo.ColumnName = "CategoryName";
            objInfo.MappingColumnName = ddl1.SelectedValue;
            ListMapping.Add(objInfo);
           
        }
        if (ddl7.SelectedValue != "-Select-")
        {
            MappingInfo objInfo = new MappingInfo();
            objInfo.ColumnName = "SubCategoryName";
            objInfo.MappingColumnName = ddl7.SelectedValue;
            ListMapping.Add(objInfo);
        }
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
                return ListMapping;
    }
    //Get Dataset from Excelsheet
    private DataSet GetDateFromExcelSheet()
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
            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        }
        else if (strFileType.Trim() == ".xlsx")
        {
            connString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        }


        query = "SELECT * FROM [Sheet1$]";


        //Create the connection object
        conn = new OleDbConnection(connString);
        //Open connection
        if (conn.State == ConnectionState.Closed) conn.Open();
        //Create the command object
        cmd = new OleDbCommand(query, conn);
        da = new OleDbDataAdapter(cmd);
        ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    //Validate import excel file
    private void validateExcelFile(string strFileType, string strFileName)
    {
        if (strFileType == ".xls" || strFileType == ".xlsx")
        {
            productimguploder.SaveAs(Server.MapPath("~/UploadedExcel/" + strFileName + strFileType));

        }
        else
        {
            lblMessage.Text = "Only excel files allowed";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
            return;
        }
    }
    // Get column data from excel
    private void GetColumnNamesFromTheExcelSheet(DataTable Exceldt)
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
        BindMapingColumnsDropDown(ddl7, lstColumnNames, "");
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
            dropDownList.SelectedValue = "-Select-";
        }
    }

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

    public int InsertImportedFile()
    {
        int MaxId = 0;
        try
        {

            MaxId = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "PinsertExcelTransaction", new SqlParameter[] { 
                        new SqlParameter("@UserId",Session["userid"] .ToString() ),
                        new SqlParameter("@FileName",productimguploder.FileName.ToString() ),
                         new SqlParameter("@CreatedDate",System.DateTime.Now ),
                          new SqlParameter("@Type","Master" ),
                           new SqlParameter("@IsDeleted","0" ),
                          
                 }));


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }
        return MaxId;
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

    public bool ValidateData(DataTable objDT)
    {

        //string Columns = "CLASS,TYPE,BRAND,MODEL,COMPANY,SITE,LOCATION,ROOM,DEPARTMENT,CUSTODIAN,SUPPLIER,ASSETSTATUS,SERIALNO,PRICE,NOOFASSETS";
        // string Columns = "CATEGORY,SUB-CATEGORY,FLOOR,BUILDING,LOCATION,DEPARTMENT,CUSTODIAN,SUPPLIERNAME,SerialNumber,AssetDescription,Quantity,Price,DeliveryDate,AssignDate,Status";
        string Columns = "CATEGORY,SUB-CATEGORY,FLOOR,BUILDING,LOCATION,DEPARTMENT,CUSTODIAN,SUPPLIERNAME,SerialNumber,AssetDescription,Quantity,Price,DeliveryDate,AssignDate,Status";

        string[] ColumnsList = Columns.Split(',');

        //Check Columns Count
        if (objDT.Columns.Count.ToString() != ColumnsList.Count().ToString())
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel File Dont have all columns which are required...!!');", true);
            return false;
        }


        // Check File has rows
        if (objDT.Rows.Count == 0)
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel fies does not contain any Row...!!');", true);
            return false;

        }



        // Loop On Columns
        for (int i = 0; i < objDT.Columns.Count; i++)
        {

            // Check all columns name are correct or not
            if (ColumnsList.Contains(objDT.Columns[i].ColumnName) == false)
            {


                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + objDT.Columns[i].ColumnName + "is Invalid..!!');", true);
                return false;

            }

            // check for empty
            if (i != 2)
            {
                DataRow[] dr1 = objDT.Select("[" + objDT.Columns[i].ColumnName + "] is null");
                if (dr1.Count() > 0 && objDT.Columns[i].ColumnName.ToString().ToLower() != "serialnumber")
                {

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + objDT.Columns[i].ColumnName + "has empty values..!!');", true);
                    return false;
                }
            }
        }
        return true;
    }
    public void btnYes_Click(object sender, EventArgs e)
    {

        if (this.IsImport==true)
        {
            AssetBL objAsset = new AssetBL();
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            ListMapping = objAsset.GetMappingListFromDB();
            ValidateExcelAndBindGrid(this.Exceldt, ListMapping);
        }
        else
        {
            ValidateExcelAndBindGrid(this.Exceldt);
        }
    }
    public void btnNo_Click(object sender, EventArgs e)
    {
        string filename = this.FileName;


        string path = Server.MapPath("~/UploadedExcel/" + filename);
        FileInfo file = new FileInfo(path);
        if (file.Exists)//check file exsit or not
        {
            file.Delete();

        }

    }
    protected void gridlist_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
       e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblBalance = (Label)e.Item.FindControl("Type");
            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            if (lblBalance.Text == "Master")
            {
                btnEdit.Visible = false;
            }

        }
    }
    protected void exptxl_Click(object sender, EventArgs e)
    {
        PrepareForExport(gridlist);
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
        if (gridlist.Items.Count > 0)
        {
            Response.Clear();
            Response.AddHeader("content-disposition",
                                  "attachment;filename=Asset.xls");
            Response.Charset = String.Empty;
            Response.ContentType = "application/ms-excel";
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            gridlist.AllowPaging = false;
            gridlist.Columns[19].Visible = false;
            this.grid_view();
            gridlist.RenderControl(HtmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        else
        {
            exptxl.Enabled = false;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
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
}