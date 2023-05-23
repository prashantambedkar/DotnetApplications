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
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;

public partial class LabelReprint : System.Web.UI.Page
{
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    public static string path = "";
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static DataTable dtAssetDetails = new DataTable();
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
    //public DataTable dtAssetDetails
    //{
    //    get
    //    {
    //        return ViewState["dtAssetDetails"] as DataTable;
    //    }
    //    set
    //    {
    //        ViewState["dtAssetDetails"] = value;

    //    }
    //}

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_Init", path);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                BinTagType();
                if (userAuthorize((int)pages.LabelPrinting, Session["userid"].ToString()) == true)
                {
                    Common.Bindcategory((DropDownList)ddlproCategory);
                    Common.BindLocation((DropDownList)ddlloc, Session["userid"].ToString());
                    Common.BindDepartMent((DropDownList)ddldept);
                    // ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddldept.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                    RadYes.Checked = true;
                    bindCustodian();
                    // BinTagType();
                    grid_view();
                    SetGridOrder();
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "SetGridOrder", path);
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


            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCustodian");
            DataSet ds = new DataSet();
            //ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "bindCustodian", path);
        }
    }

    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {
        try
        {
            //Common.bindSubCategory((DropDownList)ddlsubcat, ddlproCategory.SelectedValue);
            //ddlsubcat.SelectedValue = "0";
            ddlloc.SelectedValue = "0";
            ddlbuild.SelectedValue = "0";
            ddlfloor.SelectedValue = "0";
            ddldept.SelectedValue = "0";
            // ddlAsstID.Items.Clear();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "OnSelectedIndexChangedCategory", path);
        }
    }
    protected void OnSelectedIndexChangedLcocation(object sender, EventArgs e)
    {
        try
        {
            Common.bindBuilding((DropDownList)ddlbuild, ddlloc.SelectedValue);
            ddlfloor.SelectedValue = "0";
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "OnSelectedIndexChangedLcocation", path);
        }
    }

    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        try
        {
            Common.BindFloor((DropDownList)ddlfloor, ddlbuild.SelectedValue);
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }
    protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
    {

        ////PrintBL objPrint = new PrintBL();        
        ////objPrint.BindActiveAssetsForRePrint((ListBox)ddlAsstID, ddlproCategory.SelectedValue, ddlsubcat.SelectedValue, ddlloc.SelectedValue, ddlbuild.SelectedValue, ddlfloor.SelectedValue, ddldept.SelectedValue);

    }

    protected void btnsearchsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
            grid_view();
            gvData.DataBind();
            ////ResetSearch();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "btnsearchsubmit_Click", path);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
            grid_view();
            gvData.DataBind();
            ////ResetSearch();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "btnSearch_Click", path);
        }
    }
    private void ResetSearch()
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
            //ddlAsstID.Items.Clear();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "ResetSearch", path);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            txtAssetCode.Text = "";
            this.AssetId = 0;
            ddlproCategory.SelectedIndex = 0;
            // ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            OnSelectedIndexChangedDepartment(sender, e);
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "btnRefresh_Click", path);

        }
    }
    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    grid_view();
    //}
    protected void btnConfirmPrint_Click(object sender, EventArgs e)
    {
        bool confirmValue = Convert.ToBoolean(this.hfConfirmValue.Value);

        if (confirmValue == false)
        {
            //lblMessage.Visible = true;
            //lblMessage.Text = "You have to register Activex to print items..";
            //lblMessage.ForeColor = System.Drawing.Color.Red;
            //return;
            // MessageBox.Show("You have to register Activex to print items..");

            string Message = "You have to register Activex to print items.";
            imgpopup.ImageUrl = "images/info.jpg";
            lblpopupmsg.Text = Message;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();
        }
        else
        {
            try
            {
                clsEncoder lobj_EncoderClass = new clsEncoder();

                string ls_AssetCode = "";
                string ls_EncodedAssetCode = "";
                string ls_Category = "";
                string ls_Sub_Category = "";

                bool ASSETCODE_Status;

                bool BARCODE_STATUS;
                bool CATEGORY_Status;
                bool SUBCATEGORY_Status;
                bool BUILDING_Status;
                bool FLOOR_Status;
                bool LOCATION_Status;
                bool DEPARTMENT_Status;
                bool CUSTODIAN_Status;
                bool SUPPLIER_Status;
                bool SERIALNO_Status;
                bool DESCRIPTION_Status;
                bool QUANTITY_Status;
                bool PRICE_Status;
                bool DELIVERYDATE_Status;
                bool ASSIGNDATE_Status;
                bool Column1_STATUS, Column2_STATUS, Column3_STATUS, Column4_STATUS, Column5_STATUS, Column6_STATUS, Column7_STATUS
                    , Column8_STATUS, Column9_STATUS, Column10_STATUS, Column11_STATUS, Column12_STATUS, Column13_STATUS, Column14_STATUS;


                string Pos_ASSETCODE;
                string Pos_BARCODE;
                string Pos_CATEGORY;
                string Pos_SUBCATEGORY;
                string Pos_BUILDING;
                string Pos_FLOOR;
                string Pos_LOCATION;
                string Pos_DEPARTMENT;
                string Pos_CUSTODIAN;
                string Pos_SUPPLIER;
                string Pos_SERIALNO;
                string Pos_DESCRIPTION;
                string Pos_QUANTITY;
                string Pos_PRICE;
                string Pos_DELIVERYDATE;
                string Pos_ASSIGNDATE;
                string Pos_Column1, Pos_Column2, Pos_Column3, Pos_Column4, Pos_Column5, Pos_Column6, Pos_Column7
                    , Pos_Column8, Pos_Column9, Pos_Column10, Pos_Column11, Pos_Column12, Pos_Column13, Pos_Column14;

                string Font_ASSETCODE;
                string Font_CATEGORY;
                string Font_SUBCATEGORY;
                string Font_BUILDING;
                string Font_FLOOR;
                string Font_LOCATION;
                string Font_DEPARTMENT;
                string Font_CUSTODIAN;
                string Font_SUPPLIER;
                string Font_SERIALNO;
                string Font_DESCRIPTION;
                string Font_QUANTITY;
                string Font_PRICE;
                string Font_DELIVERYDATE;
                string Font_ASSIGNDATE;
                string Font_BARCODE;
                string Font_Column1, Font_Column2, Font_Column3, Font_Column4, Font_Column5, Font_Column6, Font_Column7
                    , Font_Column8, Font_Column9, Font_Column10, Font_Column11, Font_Column12, Font_Column13, Font_Column14;

                string Size_ASSETCODE;
                string Size_CATEGORY;
                string Size_SUBCATEGORY;
                string Size_BUILDING;
                string Size_FLOOR;
                string Size_LOCATION;
                string Size_DEPARTMENT;
                string Size_CUSTODIAN;
                string Size_SUPPLIER;
                string Size_SERIALNO;
                string Size_DESCRIPTION;
                string Size_QUANTITY;
                string Size_PRICE;
                string Size_DELIVERYDATE;
                string Size_ASSIGNDATE;
                string Size_BARCODE;
                string Size_Column1, Size_Column2, Size_Column3, Size_Column4, Size_Column5, Size_Column6, Size_Column7
                                    , Size_Column8, Size_Column9, Size_Column10, Size_Column11, Size_Column12, Size_Column13, Size_Column14;


                string Orientation_ASSETCODE;
                string Orientation_CATEGORY;
                string Orientation_SUBCATEGORY;
                string Orientation_BUILDING;
                string Orientation_FLOOR;
                string Orientation_LOCATION;
                string Orientation_DEPARTMENT;
                string Orientation_CUSTODIAN;
                string Orientation_SUPPLIER;
                string Orientation_SERIALNO;
                string Orientation_DESCRIPTION;
                string Orientation_QUANTITY;
                string Orientation_PRICE;
                string Orientation_DELIVERYDATE;
                string Orientation_ASSIGNDATE;
                string Orientation_BARCODE;
                string Orientation_Column1, Orientation_Column2, Orientation_Column3, Orientation_Column4, Orientation_Column5, Orientation_Column6, Orientation_Column7
                                    , Orientation_Column8, Orientation_Column9, Orientation_Column10, Orientation_Column11, Orientation_Column12, Orientation_Column13, Orientation_Column14;




                DataTable dt_SelectedAsset = new DataTable();
                dt_SelectedAsset.Columns.Add("AssetId", typeof(int));


                foreach (GridDataItem item in gvData.Items)
                {
                    HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                    CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                    if (chkitem.Checked == true)
                    {
                        dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    }
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

                dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
                dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
                var results = (from table1 in dtAssetDetails.AsEnumerable()
                               join table2 in dt_SelectedAsset.AsEnumerable()
                               on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                               select table1).ToList();

                DataTable dt_PrintData = new DataTable();
                if (results.Count() > 0)
                {
                    dt_PrintData = results.CopyToDataTable();
                }
                if (dt_PrintData.Rows.Count == 0)
                {
                    //MessageBox.Show("No data available to print.");

                    string Message = "No data available to print.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    //lblMessage.Text = "No data available to print.";
                    //lblMessage.ForeColor = System.Drawing.Color.Red;
                    //lblMessage.Font.Size = 11;
                    return;
                }

                string ApplicationFolder = Server.MapPath("~/Printing/");
                string PrintedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string ls_Time = DateTime.Now.ToString().Replace(':', '-');
                ls_Time = ls_Time.Replace('/', '-');
                string ls_PrintingDirectory = ApplicationFolder;

                string ls_FilesToCreate = null;
                ls_FilesToCreate = ls_PrintingDirectory + "Forprinting" + ls_Time + " " + ".zpl";

                FileInfo printinfo = new FileInfo(ls_FilesToCreate);
                StreamWriter lsw_DataToPrint = printinfo.CreateText();

                //DataTable dt_lblConfig = Common.GetConfigDetails();
                DataTable dt_lblConfig = Common.GetLabelConfigDetails(ddlTagType.SelectedItem.ToString());

                ASSETCODE_Status = dt_lblConfig.Rows[0]["PrintStatus"].ToString() == "1" ? true : false;
                CATEGORY_Status = dt_lblConfig.Rows[1]["PrintStatus"].ToString() == "1" ? true : false;
                SUBCATEGORY_Status = dt_lblConfig.Rows[2]["PrintStatus"].ToString() == "1" ? true : false;
                BUILDING_Status = dt_lblConfig.Rows[3]["PrintStatus"].ToString() == "1" ? true : false;
                FLOOR_Status = dt_lblConfig.Rows[4]["PrintStatus"].ToString() == "1" ? true : false;
                LOCATION_Status = dt_lblConfig.Rows[5]["PrintStatus"].ToString() == "1" ? true : false;
                DEPARTMENT_Status = dt_lblConfig.Rows[6]["PrintStatus"].ToString() == "1" ? true : false;
                CUSTODIAN_Status = dt_lblConfig.Rows[7]["PrintStatus"].ToString() == "1" ? true : false;
                SUPPLIER_Status = dt_lblConfig.Rows[8]["PrintStatus"].ToString() == "1" ? true : false;
                SERIALNO_Status = dt_lblConfig.Rows[9]["PrintStatus"].ToString() == "1" ? true : false;
                DESCRIPTION_Status = dt_lblConfig.Rows[10]["PrintStatus"].ToString() == "1" ? true : false;
                QUANTITY_Status = dt_lblConfig.Rows[11]["PrintStatus"].ToString() == "1" ? true : false;
                PRICE_Status = dt_lblConfig.Rows[12]["PrintStatus"].ToString() == "1" ? true : false;
                DELIVERYDATE_Status = dt_lblConfig.Rows[13]["PrintStatus"].ToString() == "1" ? true : false;
                ASSIGNDATE_Status = dt_lblConfig.Rows[14]["PrintStatus"].ToString() == "1" ? true : false;
                BARCODE_STATUS = dt_lblConfig.Rows[15]["PrintStatus"].ToString() == "1" ? true : false;
                Column1_STATUS = dt_lblConfig.Rows[16]["PrintStatus"].ToString() == "1" ? true : false;
                Column2_STATUS = dt_lblConfig.Rows[17]["PrintStatus"].ToString() == "1" ? true : false;
                Column3_STATUS = dt_lblConfig.Rows[18]["PrintStatus"].ToString() == "1" ? true : false;
                Column4_STATUS = dt_lblConfig.Rows[19]["PrintStatus"].ToString() == "1" ? true : false;
                Column5_STATUS = dt_lblConfig.Rows[20]["PrintStatus"].ToString() == "1" ? true : false;
                Column6_STATUS = dt_lblConfig.Rows[21]["PrintStatus"].ToString() == "1" ? true : false;
                Column7_STATUS = dt_lblConfig.Rows[22]["PrintStatus"].ToString() == "1" ? true : false;
                Column8_STATUS = dt_lblConfig.Rows[23]["PrintStatus"].ToString() == "1" ? true : false;
                Column9_STATUS = dt_lblConfig.Rows[24]["PrintStatus"].ToString() == "1" ? true : false;
                Column10_STATUS = dt_lblConfig.Rows[25]["PrintStatus"].ToString() == "1" ? true : false;
                Column11_STATUS = dt_lblConfig.Rows[26]["PrintStatus"].ToString() == "1" ? true : false;
                Column12_STATUS = dt_lblConfig.Rows[27]["PrintStatus"].ToString() == "1" ? true : false;
                Column13_STATUS = dt_lblConfig.Rows[28]["PrintStatus"].ToString() == "1" ? true : false;
                Column14_STATUS = dt_lblConfig.Rows[29]["PrintStatus"].ToString() == "1" ? true : false;

                Pos_ASSETCODE = dt_lblConfig.Rows[0]["POSITION"].ToString();
                Pos_CATEGORY = dt_lblConfig.Rows[1]["POSITION"].ToString();
                Pos_SUBCATEGORY = dt_lblConfig.Rows[2]["POSITION"].ToString();
                Pos_BUILDING = dt_lblConfig.Rows[3]["POSITION"].ToString();
                Pos_FLOOR = dt_lblConfig.Rows[4]["POSITION"].ToString();
                Pos_LOCATION = dt_lblConfig.Rows[5]["POSITION"].ToString();
                Pos_DEPARTMENT = dt_lblConfig.Rows[6]["POSITION"].ToString();
                Pos_CUSTODIAN = dt_lblConfig.Rows[7]["POSITION"].ToString();
                Pos_SUPPLIER = dt_lblConfig.Rows[8]["POSITION"].ToString();
                Pos_SERIALNO = dt_lblConfig.Rows[9]["POSITION"].ToString();
                Pos_DESCRIPTION = dt_lblConfig.Rows[10]["POSITION"].ToString();
                Pos_QUANTITY = dt_lblConfig.Rows[11]["POSITION"].ToString();
                Pos_PRICE = dt_lblConfig.Rows[12]["POSITION"].ToString();
                Pos_DELIVERYDATE = dt_lblConfig.Rows[13]["POSITION"].ToString();
                Pos_ASSIGNDATE = dt_lblConfig.Rows[14]["POSITION"].ToString();
                Pos_BARCODE = dt_lblConfig.Rows[15]["POSITION"].ToString();
                Pos_Column1 = dt_lblConfig.Rows[16]["POSITION"].ToString();
                Pos_Column2 = dt_lblConfig.Rows[17]["POSITION"].ToString();
                Pos_Column3 = dt_lblConfig.Rows[18]["POSITION"].ToString();
                Pos_Column4 = dt_lblConfig.Rows[19]["POSITION"].ToString();
                Pos_Column5 = dt_lblConfig.Rows[20]["POSITION"].ToString();
                Pos_Column6 = dt_lblConfig.Rows[21]["POSITION"].ToString();
                Pos_Column7 = dt_lblConfig.Rows[22]["POSITION"].ToString();
                Pos_Column8 = dt_lblConfig.Rows[23]["POSITION"].ToString();
                Pos_Column9 = dt_lblConfig.Rows[24]["POSITION"].ToString();
                Pos_Column10 = dt_lblConfig.Rows[25]["POSITION"].ToString();
                Pos_Column11 = dt_lblConfig.Rows[26]["POSITION"].ToString();
                Pos_Column12 = dt_lblConfig.Rows[27]["POSITION"].ToString();
                Pos_Column13 = dt_lblConfig.Rows[28]["POSITION"].ToString();
                Pos_Column14 = dt_lblConfig.Rows[29]["POSITION"].ToString();

                Font_ASSETCODE = dt_lblConfig.Rows[0]["FONT"].ToString().Substring(dt_lblConfig.Rows[0]["FONT"].ToString().Length - 1);
                Font_CATEGORY = dt_lblConfig.Rows[1]["FONT"].ToString().Substring(dt_lblConfig.Rows[1]["FONT"].ToString().Length - 1);
                Font_SUBCATEGORY = dt_lblConfig.Rows[2]["FONT"].ToString().Substring(dt_lblConfig.Rows[2]["FONT"].ToString().Length - 1);
                Font_BUILDING = dt_lblConfig.Rows[3]["FONT"].ToString().Substring(dt_lblConfig.Rows[3]["FONT"].ToString().Length - 1);
                Font_FLOOR = dt_lblConfig.Rows[4]["FONT"].ToString().Substring(dt_lblConfig.Rows[4]["FONT"].ToString().Length - 1);
                Font_LOCATION = dt_lblConfig.Rows[5]["FONT"].ToString().Substring(dt_lblConfig.Rows[5]["FONT"].ToString().Length - 1);
                Font_DEPARTMENT = dt_lblConfig.Rows[6]["FONT"].ToString().Substring(dt_lblConfig.Rows[6]["FONT"].ToString().Length - 1);
                Font_CUSTODIAN = dt_lblConfig.Rows[7]["FONT"].ToString().Substring(dt_lblConfig.Rows[7]["FONT"].ToString().Length - 1);
                Font_SUPPLIER = dt_lblConfig.Rows[8]["FONT"].ToString().Substring(dt_lblConfig.Rows[8]["FONT"].ToString().Length - 1);
                Font_SERIALNO = dt_lblConfig.Rows[9]["FONT"].ToString().Substring(dt_lblConfig.Rows[9]["FONT"].ToString().Length - 1);
                Font_DESCRIPTION = dt_lblConfig.Rows[10]["FONT"].ToString().Substring(dt_lblConfig.Rows[10]["FONT"].ToString().Length - 1);
                Font_QUANTITY = dt_lblConfig.Rows[11]["FONT"].ToString().Substring(dt_lblConfig.Rows[11]["FONT"].ToString().Length - 1);
                Font_PRICE = dt_lblConfig.Rows[12]["FONT"].ToString().Substring(dt_lblConfig.Rows[12]["FONT"].ToString().Length - 1);
                Font_DELIVERYDATE = dt_lblConfig.Rows[13]["FONT"].ToString().Substring(dt_lblConfig.Rows[13]["FONT"].ToString().Length - 1);
                Font_ASSIGNDATE = dt_lblConfig.Rows[14]["FONT"].ToString().Substring(dt_lblConfig.Rows[14]["FONT"].ToString().Length - 1);
                Font_BARCODE = dt_lblConfig.Rows[15]["FONT"].ToString().Substring(dt_lblConfig.Rows[15]["FONT"].ToString().Length - 1);
                Font_Column1 = dt_lblConfig.Rows[16]["FONT"].ToString().Substring(dt_lblConfig.Rows[16]["FONT"].ToString().Length - 1);
                Font_Column2 = dt_lblConfig.Rows[17]["FONT"].ToString().Substring(dt_lblConfig.Rows[17]["FONT"].ToString().Length - 1);
                Font_Column3 = dt_lblConfig.Rows[18]["FONT"].ToString().Substring(dt_lblConfig.Rows[18]["FONT"].ToString().Length - 1);
                Font_Column4 = dt_lblConfig.Rows[19]["FONT"].ToString().Substring(dt_lblConfig.Rows[19]["FONT"].ToString().Length - 1);
                Font_Column5 = dt_lblConfig.Rows[20]["FONT"].ToString().Substring(dt_lblConfig.Rows[20]["FONT"].ToString().Length - 1);
                Font_Column6 = dt_lblConfig.Rows[21]["FONT"].ToString().Substring(dt_lblConfig.Rows[21]["FONT"].ToString().Length - 1);
                Font_Column7 = dt_lblConfig.Rows[22]["FONT"].ToString().Substring(dt_lblConfig.Rows[22]["FONT"].ToString().Length - 1);
                Font_Column8 = dt_lblConfig.Rows[23]["FONT"].ToString().Substring(dt_lblConfig.Rows[23]["FONT"].ToString().Length - 1);
                Font_Column9 = dt_lblConfig.Rows[24]["FONT"].ToString().Substring(dt_lblConfig.Rows[24]["FONT"].ToString().Length - 1);
                Font_Column10 = dt_lblConfig.Rows[25]["FONT"].ToString().Substring(dt_lblConfig.Rows[25]["FONT"].ToString().Length - 1);
                Font_Column11 = dt_lblConfig.Rows[26]["FONT"].ToString().Substring(dt_lblConfig.Rows[26]["FONT"].ToString().Length - 1);
                Font_Column12 = dt_lblConfig.Rows[27]["FONT"].ToString().Substring(dt_lblConfig.Rows[27]["FONT"].ToString().Length - 1);
                Font_Column13 = dt_lblConfig.Rows[28]["FONT"].ToString().Substring(dt_lblConfig.Rows[28]["FONT"].ToString().Length - 1);
                Font_Column14 = dt_lblConfig.Rows[29]["FONT"].ToString().Substring(dt_lblConfig.Rows[29]["FONT"].ToString().Length - 1);


                Size_ASSETCODE = dt_lblConfig.Rows[0]["FONTSIZE"].ToString();
                Size_CATEGORY = dt_lblConfig.Rows[1]["FONTSIZE"].ToString();
                Size_SUBCATEGORY = dt_lblConfig.Rows[2]["FONTSIZE"].ToString();
                Size_BUILDING = dt_lblConfig.Rows[3]["FONTSIZE"].ToString();
                Size_FLOOR = dt_lblConfig.Rows[4]["FONTSIZE"].ToString();
                Size_LOCATION = dt_lblConfig.Rows[5]["FONTSIZE"].ToString();
                Size_DEPARTMENT = dt_lblConfig.Rows[6]["FONTSIZE"].ToString();
                Size_CUSTODIAN = dt_lblConfig.Rows[7]["FONTSIZE"].ToString();
                Size_SUPPLIER = dt_lblConfig.Rows[8]["FONTSIZE"].ToString();
                Size_SERIALNO = dt_lblConfig.Rows[9]["FONTSIZE"].ToString();
                Size_DESCRIPTION = dt_lblConfig.Rows[10]["FONTSIZE"].ToString();
                Size_QUANTITY = dt_lblConfig.Rows[11]["FONTSIZE"].ToString();
                Size_PRICE = dt_lblConfig.Rows[12]["FONTSIZE"].ToString();
                Size_DELIVERYDATE = dt_lblConfig.Rows[13]["FONTSIZE"].ToString();
                Size_ASSIGNDATE = dt_lblConfig.Rows[14]["FONTSIZE"].ToString();
                Size_BARCODE = dt_lblConfig.Rows[15]["FONTSIZE"].ToString();
                Size_Column1 = dt_lblConfig.Rows[16]["FONTSIZE"].ToString();
                Size_Column2 = dt_lblConfig.Rows[17]["FONTSIZE"].ToString();
                Size_Column3 = dt_lblConfig.Rows[18]["FONTSIZE"].ToString();
                Size_Column4 = dt_lblConfig.Rows[19]["FONTSIZE"].ToString();
                Size_Column5 = dt_lblConfig.Rows[20]["FONTSIZE"].ToString();
                Size_Column6 = dt_lblConfig.Rows[21]["FONTSIZE"].ToString();
                Size_Column7 = dt_lblConfig.Rows[22]["FONTSIZE"].ToString();
                Size_Column8 = dt_lblConfig.Rows[23]["FONTSIZE"].ToString();
                Size_Column9 = dt_lblConfig.Rows[24]["FONTSIZE"].ToString();
                Size_Column10 = dt_lblConfig.Rows[25]["FONTSIZE"].ToString();
                Size_Column11 = dt_lblConfig.Rows[26]["FONTSIZE"].ToString();
                Size_Column12 = dt_lblConfig.Rows[27]["FONTSIZE"].ToString();
                Size_Column13 = dt_lblConfig.Rows[28]["FONTSIZE"].ToString();
                Size_Column14 = dt_lblConfig.Rows[29]["FONTSIZE"].ToString();

                ////Orientation_ASSETCODE = dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_CATEGORY = dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_SUBCATEGORY = dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_BUILDING = dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_FLOOR = dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_LOCATION = dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_DEPARTMENT = dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_CUSTODIAN = dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_SUPPLIER = dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_SERIALNO = dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_DESCRIPTION = dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_QUANTITY = dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_PRICE = dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_DELIVERYDATE = dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_ASSIGNDATE = dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_BARCODE = dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column1 = dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column2 = dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column3 = dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column4 = dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column5 = dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column6 = dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column7 = dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column8 = dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column9 = dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column10 = dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column11 = dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column12 = dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column13 = dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
                ////Orientation_Column14 = dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";


                //1
                if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_ASSETCODE = "N";
                }
                else if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_ASSETCODE = "R";
                }
                else if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_ASSETCODE = "I";
                }
                else
                {
                    Orientation_ASSETCODE = "B";
                }

                //2 
                if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_CATEGORY = "N";
                }
                else if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_CATEGORY = "R";
                }
                else if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_CATEGORY = "I";
                }
                else
                {
                    Orientation_CATEGORY = "B";
                }

                //3
                if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_SUBCATEGORY = "N";
                }
                else if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_SUBCATEGORY = "R";
                }
                else if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_SUBCATEGORY = "I";
                }
                else
                {
                    Orientation_SUBCATEGORY = "B";
                }

                //4
                if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_BUILDING = "N";
                }
                else if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_BUILDING = "R";
                }
                else if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_BUILDING = "I";
                }
                else
                {
                    Orientation_BUILDING = "B";
                }

                //5
                if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_FLOOR = "N";
                }
                else if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_FLOOR = "R";
                }
                else if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_FLOOR = "I";
                }
                else
                {
                    Orientation_FLOOR = "B";
                }

                //6
                if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_LOCATION = "N";
                }
                else if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_LOCATION = "R";
                }
                else if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_LOCATION = "I";
                }
                else
                {
                    Orientation_LOCATION = "B";
                }

                //7
                if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_DEPARTMENT = "N";
                }
                else if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_DEPARTMENT = "R";
                }
                else if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_DEPARTMENT = "I";
                }
                else
                {
                    Orientation_DEPARTMENT = "B";
                }

                //8
                if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_CUSTODIAN = "N";
                }
                else if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_CUSTODIAN = "R";
                }
                else if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_CUSTODIAN = "I";
                }
                else
                {
                    Orientation_CUSTODIAN = "B";
                }

                //9
                if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_SUPPLIER = "N";
                }
                else if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_SUPPLIER = "R";
                }
                else if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_SUPPLIER = "I";
                }
                else
                {
                    Orientation_SUPPLIER = "B";
                }

                //10
                if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_SERIALNO = "N";
                }
                else if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_SERIALNO = "R";
                }
                else if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_SERIALNO = "I";
                }
                else
                {
                    Orientation_SERIALNO = "B";
                }

                //11
                if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_DESCRIPTION = "N";
                }
                else if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_DESCRIPTION = "R";
                }
                else if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_DESCRIPTION = "I";
                }
                else
                {
                    Orientation_DESCRIPTION = "B";
                }

                //12
                if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_QUANTITY = "N";
                }
                else if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_QUANTITY = "R";
                }
                else if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_QUANTITY = "I";
                }
                else
                {
                    Orientation_QUANTITY = "B";
                }

                //13
                if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_PRICE = "N";
                }
                else if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_PRICE = "R";
                }
                else if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_PRICE = "I";
                }
                else
                {
                    Orientation_PRICE = "B";
                }

                //14
                if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_DELIVERYDATE = "N";
                }
                else if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_DELIVERYDATE = "R";
                }
                else if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_DELIVERYDATE = "I";
                }
                else
                {
                    Orientation_DELIVERYDATE = "B";
                }

                //15
                if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_ASSIGNDATE = "N";
                }
                else if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_ASSIGNDATE = "R";
                }
                else if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_ASSIGNDATE = "I";
                }
                else
                {
                    Orientation_ASSIGNDATE = "B";
                }

                //16
                if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_BARCODE = "N";
                }
                else if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_BARCODE = "R";
                }
                else if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_BARCODE = "I";
                }
                else
                {
                    Orientation_BARCODE = "B";
                }

                //17
                if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column1 = "N";
                }
                else if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column1 = "R";
                }
                else if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column1 = "I";
                }
                else
                {
                    Orientation_Column1 = "B";
                }


                //18
                if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column2 = "N";
                }
                else if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column2 = "R";
                }
                else if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column2 = "I";
                }
                else
                {
                    Orientation_Column2 = "B";
                }


                //19
                if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column3 = "N";
                }
                else if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column3 = "R";
                }
                else if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column3 = "I";
                }
                else
                {
                    Orientation_Column3 = "B";
                }


                //20
                if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column4 = "N";
                }
                else if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column4 = "R";
                }
                else if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column4 = "I";
                }
                else
                {
                    Orientation_Column4 = "B";
                }

                //21
                if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column5 = "N";
                }
                else if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column5 = "R";
                }
                else if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column5 = "I";
                }
                else
                {
                    Orientation_Column5 = "B";
                }

                //22
                if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column6 = "N";
                }
                else if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column6 = "R";
                }
                else if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column6 = "I";
                }
                else
                {
                    Orientation_Column6 = "B";
                }

                //23
                if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column7 = "N";
                }
                else if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column7 = "R";
                }
                else if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column7 = "I";
                }
                else
                {
                    Orientation_Column7 = "B";
                }

                //24
                if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column8 = "N";
                }
                else if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column8 = "R";
                }
                else if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column8 = "I";
                }
                else
                {
                    Orientation_Column8 = "B";
                }

                //25
                if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column9 = "N";
                }
                else if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column9 = "R";
                }
                else if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column9 = "I";
                }
                else
                {
                    Orientation_Column9 = "B";
                }

                //26
                if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column10 = "N";
                }
                else if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column10 = "R";
                }
                else if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column10 = "I";
                }
                else
                {
                    Orientation_Column10 = "B";
                }

                //27
                if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column11 = "N";
                }
                else if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column11 = "R";
                }
                else if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column11 = "I";
                }
                else
                {
                    Orientation_Column11 = "B";
                }

                //28
                if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column12 = "N";
                }
                else if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column12 = "R";
                }
                else if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column12 = "I";
                }
                else
                {
                    Orientation_Column12 = "B";
                }

                //29
                if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column13 = "N";
                }
                else if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column13 = "R";
                }
                else if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column13 = "I";
                }
                else
                {
                    Orientation_Column13 = "B";
                }

                //30
                if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Normal")
                {
                    Orientation_Column14 = "N";
                }
                else if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Rotated")
                {
                    Orientation_Column14 = "R";
                }
                else if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Inverted")
                {
                    Orientation_Column14 = "I";
                }
                else
                {
                    Orientation_Column14 = "B";
                }


                System.Text.StringBuilder strSbPrint = new System.Text.StringBuilder();

                foreach (DataRow dr in dt_PrintData.Rows)
                {


                    ls_AssetCode = "";
                    ls_EncodedAssetCode = "";
                    ls_Category = "";
                    ls_Sub_Category = "";

                    ls_AssetCode = dr["AssetCode"].ToString();
                    ls_AssetCode = ls_AssetCode.ToUpper();

                    ls_Category = dr["CategoryCode"].ToString();
                    ls_Category = ls_Category.ToUpper();
                    ls_Category = ls_Category.Substring(4, 3);

                    ls_Sub_Category = dr["SubCatCode"].ToString();
                    ls_Sub_Category = ls_Sub_Category.ToUpper();
                    ls_Sub_Category = ls_Sub_Category.Substring(4, 3);

                    string prefix = dt_lblConfig.Rows[0]["Prefix"].ToString();

                    if (prefix == "" || prefix == null)
                    {
                        MessageBox.Show("Please Configure client prefix.");
                        return;
                    }
                    if (ls_Category.Length != 3 || ls_Sub_Category.Length != 3)
                    {
                        MessageBox.Show("Please check " + Category + " and " + SubCategory + ".");
                        return;
                    }
                    //ls_EncodedAssetCode = lobj_EncoderClass.EncodeTheTag("S" + ls_Category + ls_Sub_Category + ls_AssetCode);
                    ls_EncodedAssetCode = lobj_EncoderClass.EncodeTheTag(prefix + ls_AssetCode);

                    if (ls_EncodedAssetCode == null || ls_EncodedAssetCode == "")
                    {
                        MessageBox.Show("The tag is not encoded properly, printing unsuccessful.");
                        return;
                    }

                    lsw_DataToPrint.WriteLine("^XA");//ZPL Code will start from here (for starting of New label ^XA IS REQUIRED)
                    lsw_DataToPrint.WriteLine(prefix + ls_AssetCode);
                    strSbPrint.Append("^XA");

                    if (RadYes.Checked == true)
                    {
                        lsw_DataToPrint.WriteLine("^RS8");//Set up parameter including tag type, read/write position of the transponder
                        lsw_DataToPrint.WriteLine("^RFW,H ");
                        lsw_DataToPrint.WriteLine("^FD" + ls_EncodedAssetCode);//Encode the data on RFID chip
                        lsw_DataToPrint.WriteLine("^FS");//   

                        strSbPrint.Append("^RS8");//Set up parameter including tag type, read/write position of the transponder
                        strSbPrint.Append("^RFW,H ");
                        strSbPrint.Append("^FD" + ls_EncodedAssetCode);//Encode the data on RFID chip
                        strSbPrint.Append("^FS");//  
                    }

                    if (ASSETCODE_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_ASSETCODE);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_ASSETCODE);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["AssetCode"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_ASSETCODE);
                        lsw_DataToPrint.WriteLine("^A" + Font_ASSETCODE + Orientation_ASSETCODE + Size_ASSETCODE);
                        lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_ASSETCODE);
                        strSbPrint.Append("^A" + Font_ASSETCODE + Orientation_ASSETCODE + Size_ASSETCODE);
                        strSbPrint.Append("^FD" + dr["AssetCode"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (BARCODE_STATUS == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_BARCODE);
                        ////ADDED
                        //lsw_DataToPrint.WriteLine("^BY1");
                        //lsw_DataToPrint.WriteLine("^BCN,30,N,N,50,N");
                        ////ENDED
                        //lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_BARCODE);
                        ////ADDED
                        //strSbPrint.Append("^BY1");
                        //strSbPrint.Append("^BCN,30,N,N,50,N");
                        ////ENDED
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["AssetCode"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_BARCODE);
                        //ADDED
                        lsw_DataToPrint.WriteLine("^BY1");
                        lsw_DataToPrint.WriteLine("^BC" + Orientation_BARCODE + ",30,N,N,50,N");
                        //ENDED
                        lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_BARCODE);
                        //ADDED
                        strSbPrint.Append("^BY1");
                        strSbPrint.Append("^BC" + Orientation_BARCODE + ",30,N,N,50,N");
                        //ENDED
                        strSbPrint.Append("^A0" + Orientation_BARCODE + Size_BARCODE);
                        strSbPrint.Append("^FD" + dr["AssetCode"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (CATEGORY_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_CATEGORY);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Category"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_CATEGORY);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Category"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_CATEGORY);
                        lsw_DataToPrint.WriteLine("^A" + Font_CATEGORY + Orientation_CATEGORY + Size_CATEGORY);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Category"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_CATEGORY);
                        strSbPrint.Append("^A" + Font_CATEGORY + Orientation_CATEGORY + Size_CATEGORY);
                        strSbPrint.Append("^FD" + dr["Category"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (SUBCATEGORY_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_SUBCATEGORY);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["SubCategory"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_SUBCATEGORY);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["SubCategory"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_SUBCATEGORY);
                        lsw_DataToPrint.WriteLine("^A" + Font_SUBCATEGORY + Orientation_SUBCATEGORY + Size_SUBCATEGORY);
                        lsw_DataToPrint.WriteLine("^FD" + dr["SubCategory"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_SUBCATEGORY);
                        strSbPrint.Append("^A" + Font_SUBCATEGORY + Orientation_SUBCATEGORY + Size_SUBCATEGORY);
                        strSbPrint.Append("^FD" + dr["SubCategory"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (BUILDING_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_BUILDING);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Building"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_BUILDING);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Building"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_BUILDING);
                        lsw_DataToPrint.WriteLine("^A" + Font_BUILDING + Orientation_BUILDING + Size_BUILDING);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Building"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_BUILDING);
                        strSbPrint.Append("^A" + Font_BUILDING + Orientation_BUILDING + Size_BUILDING);
                        strSbPrint.Append("^FD" + dr["Building"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (FLOOR_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_FLOOR);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Floor"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_FLOOR);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Floor"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_FLOOR);
                        lsw_DataToPrint.WriteLine("^A" + Font_FLOOR + Orientation_FLOOR + Size_FLOOR);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Floor"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_FLOOR);
                        strSbPrint.Append("^A" + Font_FLOOR + Orientation_FLOOR + Size_FLOOR);
                        strSbPrint.Append("^FD" + dr["Floor"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (LOCATION_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_LOCATION);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Location"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_LOCATION);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Location"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_LOCATION);
                        lsw_DataToPrint.WriteLine("^A" + Font_LOCATION + Orientation_LOCATION + Size_LOCATION);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Location"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_LOCATION);
                        strSbPrint.Append("^A" + Font_LOCATION + Orientation_LOCATION + Size_LOCATION);
                        strSbPrint.Append("^FD" + dr["Location"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (DEPARTMENT_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_DEPARTMENT);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Department"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_DEPARTMENT);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Department"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_DEPARTMENT);
                        lsw_DataToPrint.WriteLine("^A" + Font_DEPARTMENT + Orientation_DEPARTMENT + Size_DEPARTMENT);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Department"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_DEPARTMENT);
                        strSbPrint.Append("^A" + Font_DEPARTMENT + Orientation_DEPARTMENT + Size_DEPARTMENT);
                        strSbPrint.Append("^FD" + dr["Department"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (CUSTODIAN_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_CUSTODIAN);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Custodian"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_CUSTODIAN);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Custodian"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_CUSTODIAN);
                        lsw_DataToPrint.WriteLine("^A" + Font_CUSTODIAN + Orientation_CUSTODIAN + Size_CUSTODIAN);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Custodian"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_CUSTODIAN);
                        strSbPrint.Append("^A" + Font_CUSTODIAN + Orientation_CUSTODIAN + Size_CUSTODIAN);
                        strSbPrint.Append("^FD" + dr["Custodian"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (SUPPLIER_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_SUPPLIER);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["SupplierName"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_SUPPLIER);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["SupplierName"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_SUPPLIER);
                        lsw_DataToPrint.WriteLine("^A" + Font_SUPPLIER + Orientation_SUPPLIER + Size_SUPPLIER);
                        lsw_DataToPrint.WriteLine("^FD" + dr["SupplierName"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_SUPPLIER);
                        strSbPrint.Append("^A" + Font_SUPPLIER + Orientation_SUPPLIER + Size_SUPPLIER);
                        strSbPrint.Append("^FD" + dr["SupplierName"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (SERIALNO_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_SERIALNO);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["SerialNo"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_SERIALNO);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["SerialNo"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_SERIALNO);
                        lsw_DataToPrint.WriteLine("^A" + Font_SERIALNO + Orientation_SERIALNO + Size_SERIALNO);
                        lsw_DataToPrint.WriteLine("^FD" + dr["SerialNo"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_SERIALNO);
                        strSbPrint.Append("^A" + Font_SERIALNO + Orientation_SERIALNO + Size_SERIALNO);
                        strSbPrint.Append("^FD" + dr["SerialNo"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (DESCRIPTION_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_DESCRIPTION);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Description"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_DESCRIPTION);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Description"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_DESCRIPTION);
                        lsw_DataToPrint.WriteLine("^A" + Font_DESCRIPTION + Orientation_DESCRIPTION + Size_DESCRIPTION);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Description"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_DESCRIPTION);
                        strSbPrint.Append("^A" + Font_DESCRIPTION + Orientation_DESCRIPTION + Size_DESCRIPTION);
                        strSbPrint.Append("^FD" + dr["Description"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (QUANTITY_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_QUANTITY);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Quantity"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_QUANTITY);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Quantity"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_QUANTITY);
                        lsw_DataToPrint.WriteLine("^A" + Font_QUANTITY + Orientation_QUANTITY + Size_QUANTITY);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Quantity"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_QUANTITY);
                        strSbPrint.Append("^A" + Font_QUANTITY + Orientation_QUANTITY + Size_QUANTITY);
                        strSbPrint.Append("^FD" + dr["Quantity"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (PRICE_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_PRICE);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["Price"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_PRICE);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["Price"].ToString());
                        //strSbPrint.Append("^FS");


                        lsw_DataToPrint.WriteLine("^FO" + Pos_PRICE);
                        lsw_DataToPrint.WriteLine("^A" + Font_PRICE + Orientation_PRICE + Size_PRICE);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Price"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_PRICE);
                        strSbPrint.Append("^A" + Font_PRICE + Orientation_PRICE + Size_PRICE);
                        strSbPrint.Append("^FD" + dr["Price"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (DELIVERYDATE_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_DELIVERYDATE);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["DeliveryDate"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_DELIVERYDATE);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["DeliveryDate"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_DELIVERYDATE);
                        lsw_DataToPrint.WriteLine("^A" + Font_DELIVERYDATE + Orientation_DELIVERYDATE + Size_DELIVERYDATE);
                        lsw_DataToPrint.WriteLine("^FD" + dr["DeliveryDate"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_DELIVERYDATE);
                        strSbPrint.Append("^A" + Font_DELIVERYDATE + Orientation_DELIVERYDATE + Size_DELIVERYDATE);
                        strSbPrint.Append("^FD" + dr["DeliveryDate"].ToString());
                        strSbPrint.Append("^FS");
                    }
                    if (ASSIGNDATE_Status == true)
                    {
                        //lsw_DataToPrint.WriteLine("^FO" + Pos_ASSIGNDATE);
                        //lsw_DataToPrint.WriteLine("^A0,25,25");
                        //lsw_DataToPrint.WriteLine("^FD" + dr["AssignDate"].ToString());
                        //lsw_DataToPrint.WriteLine("^FS");

                        //strSbPrint.Append("^FO" + Pos_DELIVERYDATE);
                        //strSbPrint.Append("^A0,25,25");
                        //strSbPrint.Append("^FD" + dr["AssignDate"].ToString());
                        //strSbPrint.Append("^FS");

                        lsw_DataToPrint.WriteLine("^FO" + Pos_ASSIGNDATE);
                        lsw_DataToPrint.WriteLine("^A" + Font_ASSIGNDATE + Orientation_ASSIGNDATE + Size_ASSIGNDATE);
                        lsw_DataToPrint.WriteLine("^FD" + dr["AssignDate"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_DELIVERYDATE);
                        strSbPrint.Append("^A" + Font_ASSIGNDATE + Orientation_ASSIGNDATE + Size_ASSIGNDATE);
                        strSbPrint.Append("^FD" + dr["AssignDate"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column1_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column1);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column1 + Orientation_Column1 + Size_Column1);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column1"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column1);
                        strSbPrint.Append("^A" + Font_Column1 + Orientation_Column1 + Size_Column1);
                        strSbPrint.Append("^FD" + dr["Column1"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column2_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column2);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column2 + Orientation_Column2 + Size_Column2);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column2"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column2);
                        strSbPrint.Append("^A" + Font_Column2 + Orientation_Column2 + Size_Column2);
                        strSbPrint.Append("^FD" + dr["Column2"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column3_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column3);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column3 + Orientation_Column3 + Size_Column3);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column3"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column3);
                        strSbPrint.Append("^A" + Font_Column3 + Orientation_Column3 + Size_Column3);
                        strSbPrint.Append("^FD" + dr["Column3"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column4_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column4);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column4 + Orientation_Column4 + Size_Column4);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column4"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column4);
                        strSbPrint.Append("^A" + Font_Column4 + Orientation_Column4 + Size_Column4);
                        strSbPrint.Append("^FD" + dr["Column4"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column5_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column5);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column5 + Orientation_Column5 + Size_Column5);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column5"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column5);
                        strSbPrint.Append("^A" + Font_Column5 + Orientation_Column5 + Size_Column5);
                        strSbPrint.Append("^FD" + dr["Column5"].ToString());
                        strSbPrint.Append("^FS");
                    }


                    if (Column6_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column6);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column6 + Orientation_Column6 + Size_Column6);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column6"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column6);
                        strSbPrint.Append("^A" + Font_Column6 + Orientation_Column6 + Size_Column6);
                        strSbPrint.Append("^FD" + dr["Column6"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column7_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column7);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column7 + Orientation_Column7 + Size_Column7);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column7"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column7);
                        strSbPrint.Append("^A" + Font_Column7 + Orientation_Column7 + Size_Column7);
                        strSbPrint.Append("^FD" + dr["Column7"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column8_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column8);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column8 + Orientation_Column8 + Size_Column8);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column8"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column8);
                        strSbPrint.Append("^A" + Font_Column8 + Orientation_Column8 + Size_Column8);
                        strSbPrint.Append("^FD" + dr["Column8"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column9_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column9);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column9 + Orientation_Column9 + Size_Column9);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column9"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column9);
                        strSbPrint.Append("^A" + Font_Column9 + Orientation_Column9 + Size_Column9);
                        strSbPrint.Append("^FD" + dr["Column9"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column10_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column10);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column10 + Orientation_Column10 + Size_Column10);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column10"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column10);
                        strSbPrint.Append("^A" + Font_Column10 + Orientation_Column10 + Size_Column10);
                        strSbPrint.Append("^FD" + dr["Column10"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column11_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column11);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column11 + Orientation_Column11 + Size_Column11);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column11"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column11);
                        strSbPrint.Append("^A" + Font_Column11 + Orientation_Column11 + Size_Column11);
                        strSbPrint.Append("^FD" + dr["Column11"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column12_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column12);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column12 + Orientation_Column12 + Size_Column12);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column12"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column12);
                        strSbPrint.Append("^A" + Font_Column12 + Orientation_Column12 + Size_Column12);
                        strSbPrint.Append("^FD" + dr["Column12"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column13_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column13);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column13 + Orientation_Column13 + Size_Column13);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column13"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column13);
                        strSbPrint.Append("^A" + Font_Column13 + Orientation_Column13 + Size_Column13);
                        strSbPrint.Append("^FD" + dr["Column13"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    if (Column14_STATUS == true)
                    {

                        lsw_DataToPrint.WriteLine("^FO" + Pos_Column14);
                        lsw_DataToPrint.WriteLine("^A" + Font_Column14 + Orientation_Column14 + Size_Column14);
                        lsw_DataToPrint.WriteLine("^FD" + dr["Column14"].ToString());
                        lsw_DataToPrint.WriteLine("^FS");

                        strSbPrint.Append("^FO" + Pos_Column14);
                        strSbPrint.Append("^A" + Font_Column14 + Orientation_Column14 + Size_Column14);
                        strSbPrint.Append("^FD" + dr["Column14"].ToString());
                        strSbPrint.Append("^FS");
                    }

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                    SqlDataAdapter dpt = new SqlDataAdapter();

                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SP_LABEL_CONFIG_DETAILS", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(ddlTagType.SelectedItem.ToString()))
                });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt_Details = ds.Tables[0];

                        DataRow[] dr_barcode = dt_Details.Select("FieldName='Barcode'");
                        if (dr_barcode.Length > 0)
                        {
                            try
                            {

                                string orient = dr_barcode[0]["orientation"].ToString() == "Normal" ? "N" : "R";
                                lsw_DataToPrint.WriteLine("^FO" + dr_barcode[0]["Position"].ToString());
                                lsw_DataToPrint.WriteLine("^BY1");
                                lsw_DataToPrint.WriteLine("^BC" + orient + ",30,N,N,50,N");
                                lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                                lsw_DataToPrint.WriteLine("^FS");

                                strSbPrint.Append("^FO" + dr_barcode[0]["Position"].ToString());
                                strSbPrint.Append("^BY1");
                                strSbPrint.Append("^BC" + orient + ",30,N,N,50,N");
                                strSbPrint.Append("^FD" + dr["AssetCode"].ToString());
                                strSbPrint.Append("^FS");
                            }
                            catch
                            {

                            }
                        }

                        DataRow[] dr_logo = dt_Details.Select("FieldName='Logo'");
                        if (dr_logo.Length > 0)
                        {
                            try
                            {

                                lsw_DataToPrint.WriteLine("^FO" + dr_logo[0]["Position"].ToString());
                                lsw_DataToPrint.WriteLine("^XGE:" + dr_logo[0]["Logo"].ToString() + ".GRF,1,1");
                                lsw_DataToPrint.WriteLine("^FS");

                                strSbPrint.Append("^FO" + dr_logo[0]["Position"].ToString());
                                strSbPrint.Append("^XGE:" + dr_logo[0]["Logo"].ToString() + ".GRF,1,1");
                                strSbPrint.Append("^FS");
                            }
                            catch
                            {

                            }
                        }

                        DataRow[] dr_company = dt_Details.Select("FieldName='Company'");
                        if (dr_company.Length > 0)
                        {
                            try
                            {

                                //dt_lblConfig.Rows[29]["FONT"].ToString().Substring(dt_lblConfig.Rows[29]["FONT"].ToString().Length - 1);
                                string font = dr_company[0]["Font"].ToString().Substring(dr_company[0]["Font"].ToString().Length - 1);
                                string orient = dr_company[0]["orientation"].ToString() == "Normal" ? "N" : "R";

                                lsw_DataToPrint.WriteLine("^FO" + dr_company[0]["Position"].ToString());
                                lsw_DataToPrint.WriteLine("^A" + font + orient + dr_company[0]["FontSize"].ToString());
                                lsw_DataToPrint.WriteLine("^FD" + dr_company[0]["Company"].ToString());
                                lsw_DataToPrint.WriteLine("^FS");

                                strSbPrint.Append("^FO" + dr_company[0]["Position"].ToString());
                                strSbPrint.Append("^A" + font + orient + dr_company[0]["FontSize"].ToString());
                                strSbPrint.Append("^FD" + dr_company[0]["Company"].ToString());
                                strSbPrint.Append("^FS");
                            }
                            catch
                            {

                            }
                        }
                    }

                    lsw_DataToPrint.WriteLine("^XZ");
                    strSbPrint.Append("^XZ");

                }
                lsw_DataToPrint.Flush();
                lsw_DataToPrint.Close();

                string AssetID = string.Join(",", dt_PrintData.AsEnumerable().Select(s => s.Field<int>("AssetId")).ToArray<int>());

                PrintBL ObjPrint = new PrintBL();
                ObjPrint.UpdateAssetMasterPrintStatus(AssetID, Convert.ToInt32(Session["userid"]), "RePrint");

                grid_view();
                gvData.DataBind();

                string strprint = strSbPrint.ToString();
                if (!string.IsNullOrEmpty(strprint))
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CallMyFunction", "PrintTags('" + strprint + "');", true);

                    //string Message = "Item sent to printer successfully";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                }
                ////lblMessage.Text = "";
            }
            catch (Exception ex)
            {
                Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "btnConfirmPrint_Click", path);
                MessageBox.Show(ex.ToString());
            }

        }

    }

    private void grid_view()
    {
        try
        {
            string Asset = (Convert.ToString(this.AssetId) == "0") ? null : Convert.ToString(this.AssetId);
            string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
            string SubCatId = null;
            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
            string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            string CustodianId = (ddlCustodian.SelectedValue == "0") ? null : ddlCustodian.SelectedValue;
            string TagType = ddlTagType.SelectedItem.ToString();

            PrintBL objBL = new PrintBL();
            DataSet ds = objBL.GetAssetDetailsForRePrintV2(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode, SearchText, CustodianId, TagType, Session["userid"].ToString());
            dtAssetDetails = ds.Tables[0];

            lblcnt.Text = Convert.ToString(ds.Tables[0].Rows.Count);

            //gridlist.DataSource = ds;
            //gridlist.DataBind();

            gvData.DataSource = ds;

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "grid_view", path);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_NeedDataSource", path);
        }
    }

    protected void HeaderCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "HeaderCheckBox_CheckedChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_PageIndexChanged", path);
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            clsEncoder lobj_EncoderClass = new clsEncoder();

            string ls_AssetCode = "";
            string ls_EncodedAssetCode = "";
            string ls_Category = "";
            string ls_Sub_Category = "";

            bool BARCODE_STATUS;
            bool ASSETCODE_Status;
            bool CATEGORY_Status;
            bool SUBCATEGORY_Status;
            bool BUILDING_Status;
            bool FLOOR_Status;
            bool LOCATION_Status;
            bool DEPARTMENT_Status;
            bool CUSTODIAN_Status;
            bool SUPPLIER_Status;
            bool SERIALNO_Status;
            bool DESCRIPTION_Status;
            bool QUANTITY_Status;
            bool PRICE_Status;
            bool DELIVERYDATE_Status;
            bool ASSIGNDATE_Status;
            bool Column1_STATUS, Column2_STATUS, Column3_STATUS, Column4_STATUS, Column5_STATUS, Column6_STATUS, Column7_STATUS
                , Column8_STATUS, Column9_STATUS, Column10_STATUS, Column11_STATUS, Column12_STATUS, Column13_STATUS, Column14_STATUS;


            string Pos_ASSETCODE;
            string Pos_CATEGORY;
            string Pos_SUBCATEGORY;
            string Pos_BUILDING;
            string Pos_FLOOR;
            string Pos_LOCATION;
            string Pos_DEPARTMENT;
            string Pos_CUSTODIAN;
            string Pos_SUPPLIER;
            string Pos_SERIALNO;
            string Pos_DESCRIPTION;
            string Pos_QUANTITY;
            string Pos_PRICE;
            string Pos_DELIVERYDATE;
            string Pos_ASSIGNDATE;
            string Pos_BARCODE;
            string Pos_Column1, Pos_Column2, Pos_Column3, Pos_Column4, Pos_Column5, Pos_Column6, Pos_Column7
                , Pos_Column8, Pos_Column9, Pos_Column10, Pos_Column11, Pos_Column12, Pos_Column13, Pos_Column14;

            string Font_ASSETCODE;
            string Font_CATEGORY;
            string Font_SUBCATEGORY;
            string Font_BUILDING;
            string Font_FLOOR;
            string Font_LOCATION;
            string Font_DEPARTMENT;
            string Font_CUSTODIAN;
            string Font_SUPPLIER;
            string Font_SERIALNO;
            string Font_DESCRIPTION;
            string Font_QUANTITY;
            string Font_PRICE;
            string Font_DELIVERYDATE;
            string Font_ASSIGNDATE;
            string Font_BARCODE;
            string Font_Column1, Font_Column2, Font_Column3, Font_Column4, Font_Column5, Font_Column6, Font_Column7
                , Font_Column8, Font_Column9, Font_Column10, Font_Column11, Font_Column12, Font_Column13, Font_Column14;

            string Size_ASSETCODE;
            string Size_CATEGORY;
            string Size_SUBCATEGORY;
            string Size_BUILDING;
            string Size_FLOOR;
            string Size_LOCATION;
            string Size_DEPARTMENT;
            string Size_CUSTODIAN;
            string Size_SUPPLIER;
            string Size_SERIALNO;
            string Size_DESCRIPTION;
            string Size_QUANTITY;
            string Size_PRICE;
            string Size_DELIVERYDATE;
            string Size_ASSIGNDATE;
            string Size_BARCODE;
            string Size_Column1, Size_Column2, Size_Column3, Size_Column4, Size_Column5, Size_Column6, Size_Column7
                                , Size_Column8, Size_Column9, Size_Column10, Size_Column11, Size_Column12, Size_Column13, Size_Column14;


            string Orientation_ASSETCODE;
            string Orientation_CATEGORY;
            string Orientation_SUBCATEGORY;
            string Orientation_BUILDING;
            string Orientation_FLOOR;
            string Orientation_LOCATION;
            string Orientation_DEPARTMENT;
            string Orientation_CUSTODIAN;
            string Orientation_SUPPLIER;
            string Orientation_SERIALNO;
            string Orientation_DESCRIPTION;
            string Orientation_QUANTITY;
            string Orientation_PRICE;
            string Orientation_DELIVERYDATE;
            string Orientation_ASSIGNDATE;
            string Orientation_BARCODE;
            string Orientation_Column1, Orientation_Column2, Orientation_Column3, Orientation_Column4, Orientation_Column5, Orientation_Column6, Orientation_Column7
                                , Orientation_Column8, Orientation_Column9, Orientation_Column10, Orientation_Column11, Orientation_Column12, Orientation_Column13, Orientation_Column14;




            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (chkitem.Checked == true)
                {
                    dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    break;
                }
            }

            dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
            dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
            var results = (from table1 in dtAssetDetails.AsEnumerable()
                           join table2 in dt_SelectedAsset.AsEnumerable()
                           on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                           select table1).ToList();

            DataTable dt_PrintData = new DataTable();
            if (results.Count() > 0)
            {
                dt_PrintData = results.CopyToDataTable();
            }
            if (dt_PrintData.Rows.Count == 0)
            {
                //MessageBox.Show("No data available to print.");

                string Message = "No data available to show preview.";
                imgpopup.ImageUrl = "images/info.jpg";
                lblpopupmsg.Text = Message;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();

                //lblMessage.Text = "No data available to print.";
                //lblMessage.ForeColor = System.Drawing.Color.Red;
                //lblMessage.Font.Size = 11;
                return;
            }

            string ApplicationFolder = Server.MapPath("~/Printing/");

            string ls_Time = DateTime.Now.ToString().Replace(':', '-');
            ls_Time = ls_Time.Replace('/', '-');
            string ls_PrintingDirectory = ApplicationFolder;

            string ls_FilesToCreate = null;
            ls_FilesToCreate = ls_PrintingDirectory + "Label" + " " + ".txt";

            FileInfo printinfo = new FileInfo(ls_FilesToCreate);
            StreamWriter lsw_DataToPrint = printinfo.CreateText();

            DataTable dt_lblConfig = Common.GetLabelConfigDetails(ddlTagType.SelectedItem.ToString());

            ASSETCODE_Status = dt_lblConfig.Rows[0]["PrintStatus"].ToString() == "1" ? true : false;
            CATEGORY_Status = dt_lblConfig.Rows[1]["PrintStatus"].ToString() == "1" ? true : false;
            SUBCATEGORY_Status = dt_lblConfig.Rows[2]["PrintStatus"].ToString() == "1" ? true : false;
            BUILDING_Status = dt_lblConfig.Rows[3]["PrintStatus"].ToString() == "1" ? true : false;
            FLOOR_Status = dt_lblConfig.Rows[4]["PrintStatus"].ToString() == "1" ? true : false;
            LOCATION_Status = dt_lblConfig.Rows[5]["PrintStatus"].ToString() == "1" ? true : false;
            DEPARTMENT_Status = dt_lblConfig.Rows[6]["PrintStatus"].ToString() == "1" ? true : false;
            CUSTODIAN_Status = dt_lblConfig.Rows[7]["PrintStatus"].ToString() == "1" ? true : false;
            SUPPLIER_Status = dt_lblConfig.Rows[8]["PrintStatus"].ToString() == "1" ? true : false;
            SERIALNO_Status = dt_lblConfig.Rows[9]["PrintStatus"].ToString() == "1" ? true : false;
            DESCRIPTION_Status = dt_lblConfig.Rows[10]["PrintStatus"].ToString() == "1" ? true : false;
            QUANTITY_Status = dt_lblConfig.Rows[11]["PrintStatus"].ToString() == "1" ? true : false;
            PRICE_Status = dt_lblConfig.Rows[12]["PrintStatus"].ToString() == "1" ? true : false;
            DELIVERYDATE_Status = dt_lblConfig.Rows[13]["PrintStatus"].ToString() == "1" ? true : false;
            ASSIGNDATE_Status = dt_lblConfig.Rows[14]["PrintStatus"].ToString() == "1" ? true : false;
            BARCODE_STATUS = dt_lblConfig.Rows[15]["PrintStatus"].ToString() == "1" ? true : false;
            Column1_STATUS = dt_lblConfig.Rows[16]["PrintStatus"].ToString() == "1" ? true : false;
            Column2_STATUS = dt_lblConfig.Rows[17]["PrintStatus"].ToString() == "1" ? true : false;
            Column3_STATUS = dt_lblConfig.Rows[18]["PrintStatus"].ToString() == "1" ? true : false;
            Column4_STATUS = dt_lblConfig.Rows[19]["PrintStatus"].ToString() == "1" ? true : false;
            Column5_STATUS = dt_lblConfig.Rows[20]["PrintStatus"].ToString() == "1" ? true : false;
            Column6_STATUS = dt_lblConfig.Rows[21]["PrintStatus"].ToString() == "1" ? true : false;
            Column7_STATUS = dt_lblConfig.Rows[22]["PrintStatus"].ToString() == "1" ? true : false;
            Column8_STATUS = dt_lblConfig.Rows[23]["PrintStatus"].ToString() == "1" ? true : false;
            Column9_STATUS = dt_lblConfig.Rows[24]["PrintStatus"].ToString() == "1" ? true : false;
            Column10_STATUS = dt_lblConfig.Rows[25]["PrintStatus"].ToString() == "1" ? true : false;
            Column11_STATUS = dt_lblConfig.Rows[26]["PrintStatus"].ToString() == "1" ? true : false;
            Column12_STATUS = dt_lblConfig.Rows[27]["PrintStatus"].ToString() == "1" ? true : false;
            Column13_STATUS = dt_lblConfig.Rows[28]["PrintStatus"].ToString() == "1" ? true : false;
            Column14_STATUS = dt_lblConfig.Rows[29]["PrintStatus"].ToString() == "1" ? true : false;

            Pos_ASSETCODE = dt_lblConfig.Rows[0]["POSITION"].ToString();
            Pos_CATEGORY = dt_lblConfig.Rows[1]["POSITION"].ToString();
            Pos_SUBCATEGORY = dt_lblConfig.Rows[2]["POSITION"].ToString();
            Pos_BUILDING = dt_lblConfig.Rows[3]["POSITION"].ToString();
            Pos_FLOOR = dt_lblConfig.Rows[4]["POSITION"].ToString();
            Pos_LOCATION = dt_lblConfig.Rows[5]["POSITION"].ToString();
            Pos_DEPARTMENT = dt_lblConfig.Rows[6]["POSITION"].ToString();
            Pos_CUSTODIAN = dt_lblConfig.Rows[7]["POSITION"].ToString();
            Pos_SUPPLIER = dt_lblConfig.Rows[8]["POSITION"].ToString();
            Pos_SERIALNO = dt_lblConfig.Rows[9]["POSITION"].ToString();
            Pos_DESCRIPTION = dt_lblConfig.Rows[10]["POSITION"].ToString();
            Pos_QUANTITY = dt_lblConfig.Rows[11]["POSITION"].ToString();
            Pos_PRICE = dt_lblConfig.Rows[12]["POSITION"].ToString();
            Pos_DELIVERYDATE = dt_lblConfig.Rows[13]["POSITION"].ToString();
            Pos_ASSIGNDATE = dt_lblConfig.Rows[14]["POSITION"].ToString();
            Pos_BARCODE = dt_lblConfig.Rows[15]["POSITION"].ToString();
            Pos_Column1 = dt_lblConfig.Rows[16]["POSITION"].ToString();
            Pos_Column2 = dt_lblConfig.Rows[17]["POSITION"].ToString();
            Pos_Column3 = dt_lblConfig.Rows[18]["POSITION"].ToString();
            Pos_Column4 = dt_lblConfig.Rows[19]["POSITION"].ToString();
            Pos_Column5 = dt_lblConfig.Rows[20]["POSITION"].ToString();
            Pos_Column6 = dt_lblConfig.Rows[21]["POSITION"].ToString();
            Pos_Column7 = dt_lblConfig.Rows[22]["POSITION"].ToString();
            Pos_Column8 = dt_lblConfig.Rows[23]["POSITION"].ToString();
            Pos_Column9 = dt_lblConfig.Rows[24]["POSITION"].ToString();
            Pos_Column10 = dt_lblConfig.Rows[25]["POSITION"].ToString();
            Pos_Column11 = dt_lblConfig.Rows[26]["POSITION"].ToString();
            Pos_Column12 = dt_lblConfig.Rows[27]["POSITION"].ToString();
            Pos_Column13 = dt_lblConfig.Rows[28]["POSITION"].ToString();
            Pos_Column14 = dt_lblConfig.Rows[29]["POSITION"].ToString();

            Font_ASSETCODE = dt_lblConfig.Rows[0]["FONT"].ToString().Substring(dt_lblConfig.Rows[0]["FONT"].ToString().Length - 1);
            Font_CATEGORY = dt_lblConfig.Rows[1]["FONT"].ToString().Substring(dt_lblConfig.Rows[1]["FONT"].ToString().Length - 1);
            Font_SUBCATEGORY = dt_lblConfig.Rows[2]["FONT"].ToString().Substring(dt_lblConfig.Rows[2]["FONT"].ToString().Length - 1);
            Font_BUILDING = dt_lblConfig.Rows[3]["FONT"].ToString().Substring(dt_lblConfig.Rows[3]["FONT"].ToString().Length - 1);
            Font_FLOOR = dt_lblConfig.Rows[4]["FONT"].ToString().Substring(dt_lblConfig.Rows[4]["FONT"].ToString().Length - 1);
            Font_LOCATION = dt_lblConfig.Rows[5]["FONT"].ToString().Substring(dt_lblConfig.Rows[5]["FONT"].ToString().Length - 1);
            Font_DEPARTMENT = dt_lblConfig.Rows[6]["FONT"].ToString().Substring(dt_lblConfig.Rows[6]["FONT"].ToString().Length - 1);
            Font_CUSTODIAN = dt_lblConfig.Rows[7]["FONT"].ToString().Substring(dt_lblConfig.Rows[7]["FONT"].ToString().Length - 1);
            Font_SUPPLIER = dt_lblConfig.Rows[8]["FONT"].ToString().Substring(dt_lblConfig.Rows[8]["FONT"].ToString().Length - 1);
            Font_SERIALNO = dt_lblConfig.Rows[9]["FONT"].ToString().Substring(dt_lblConfig.Rows[9]["FONT"].ToString().Length - 1);
            Font_DESCRIPTION = dt_lblConfig.Rows[10]["FONT"].ToString().Substring(dt_lblConfig.Rows[10]["FONT"].ToString().Length - 1);
            Font_QUANTITY = dt_lblConfig.Rows[11]["FONT"].ToString().Substring(dt_lblConfig.Rows[11]["FONT"].ToString().Length - 1);
            Font_PRICE = dt_lblConfig.Rows[12]["FONT"].ToString().Substring(dt_lblConfig.Rows[12]["FONT"].ToString().Length - 1);
            Font_DELIVERYDATE = dt_lblConfig.Rows[13]["FONT"].ToString().Substring(dt_lblConfig.Rows[13]["FONT"].ToString().Length - 1);
            Font_ASSIGNDATE = dt_lblConfig.Rows[14]["FONT"].ToString().Substring(dt_lblConfig.Rows[14]["FONT"].ToString().Length - 1);
            Font_BARCODE = dt_lblConfig.Rows[15]["FONT"].ToString().Substring(dt_lblConfig.Rows[15]["FONT"].ToString().Length - 1);
            Font_Column1 = dt_lblConfig.Rows[16]["FONT"].ToString().Substring(dt_lblConfig.Rows[16]["FONT"].ToString().Length - 1);
            Font_Column2 = dt_lblConfig.Rows[17]["FONT"].ToString().Substring(dt_lblConfig.Rows[17]["FONT"].ToString().Length - 1);
            Font_Column3 = dt_lblConfig.Rows[18]["FONT"].ToString().Substring(dt_lblConfig.Rows[18]["FONT"].ToString().Length - 1);
            Font_Column4 = dt_lblConfig.Rows[19]["FONT"].ToString().Substring(dt_lblConfig.Rows[19]["FONT"].ToString().Length - 1);
            Font_Column5 = dt_lblConfig.Rows[20]["FONT"].ToString().Substring(dt_lblConfig.Rows[20]["FONT"].ToString().Length - 1);
            Font_Column6 = dt_lblConfig.Rows[21]["FONT"].ToString().Substring(dt_lblConfig.Rows[21]["FONT"].ToString().Length - 1);
            Font_Column7 = dt_lblConfig.Rows[22]["FONT"].ToString().Substring(dt_lblConfig.Rows[22]["FONT"].ToString().Length - 1);
            Font_Column8 = dt_lblConfig.Rows[23]["FONT"].ToString().Substring(dt_lblConfig.Rows[23]["FONT"].ToString().Length - 1);
            Font_Column9 = dt_lblConfig.Rows[24]["FONT"].ToString().Substring(dt_lblConfig.Rows[24]["FONT"].ToString().Length - 1);
            Font_Column10 = dt_lblConfig.Rows[25]["FONT"].ToString().Substring(dt_lblConfig.Rows[25]["FONT"].ToString().Length - 1);
            Font_Column11 = dt_lblConfig.Rows[26]["FONT"].ToString().Substring(dt_lblConfig.Rows[26]["FONT"].ToString().Length - 1);
            Font_Column12 = dt_lblConfig.Rows[27]["FONT"].ToString().Substring(dt_lblConfig.Rows[27]["FONT"].ToString().Length - 1);
            Font_Column13 = dt_lblConfig.Rows[28]["FONT"].ToString().Substring(dt_lblConfig.Rows[28]["FONT"].ToString().Length - 1);
            Font_Column14 = dt_lblConfig.Rows[29]["FONT"].ToString().Substring(dt_lblConfig.Rows[29]["FONT"].ToString().Length - 1);


            Size_ASSETCODE = dt_lblConfig.Rows[0]["FONTSIZE"].ToString();
            Size_CATEGORY = dt_lblConfig.Rows[1]["FONTSIZE"].ToString();
            Size_SUBCATEGORY = dt_lblConfig.Rows[2]["FONTSIZE"].ToString();
            Size_BUILDING = dt_lblConfig.Rows[3]["FONTSIZE"].ToString();
            Size_FLOOR = dt_lblConfig.Rows[4]["FONTSIZE"].ToString();
            Size_LOCATION = dt_lblConfig.Rows[5]["FONTSIZE"].ToString();
            Size_DEPARTMENT = dt_lblConfig.Rows[6]["FONTSIZE"].ToString();
            Size_CUSTODIAN = dt_lblConfig.Rows[7]["FONTSIZE"].ToString();
            Size_SUPPLIER = dt_lblConfig.Rows[8]["FONTSIZE"].ToString();
            Size_SERIALNO = dt_lblConfig.Rows[9]["FONTSIZE"].ToString();
            Size_DESCRIPTION = dt_lblConfig.Rows[10]["FONTSIZE"].ToString();
            Size_QUANTITY = dt_lblConfig.Rows[11]["FONTSIZE"].ToString();
            Size_PRICE = dt_lblConfig.Rows[12]["FONTSIZE"].ToString();
            Size_DELIVERYDATE = dt_lblConfig.Rows[13]["FONTSIZE"].ToString();
            Size_ASSIGNDATE = dt_lblConfig.Rows[14]["FONTSIZE"].ToString();
            Size_BARCODE = dt_lblConfig.Rows[15]["FONTSIZE"].ToString();
            Size_Column1 = dt_lblConfig.Rows[16]["FONTSIZE"].ToString();
            Size_Column2 = dt_lblConfig.Rows[17]["FONTSIZE"].ToString();
            Size_Column3 = dt_lblConfig.Rows[18]["FONTSIZE"].ToString();
            Size_Column4 = dt_lblConfig.Rows[19]["FONTSIZE"].ToString();
            Size_Column5 = dt_lblConfig.Rows[20]["FONTSIZE"].ToString();
            Size_Column6 = dt_lblConfig.Rows[21]["FONTSIZE"].ToString();
            Size_Column7 = dt_lblConfig.Rows[22]["FONTSIZE"].ToString();
            Size_Column8 = dt_lblConfig.Rows[23]["FONTSIZE"].ToString();
            Size_Column9 = dt_lblConfig.Rows[24]["FONTSIZE"].ToString();
            Size_Column10 = dt_lblConfig.Rows[25]["FONTSIZE"].ToString();
            Size_Column11 = dt_lblConfig.Rows[26]["FONTSIZE"].ToString();
            Size_Column12 = dt_lblConfig.Rows[27]["FONTSIZE"].ToString();
            Size_Column13 = dt_lblConfig.Rows[28]["FONTSIZE"].ToString();
            Size_Column14 = dt_lblConfig.Rows[29]["FONTSIZE"].ToString();


            ////Orientation_ASSETCODE = dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_CATEGORY = dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_SUBCATEGORY = dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_BUILDING = dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_FLOOR = dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_LOCATION = dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_DEPARTMENT = dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_CUSTODIAN = dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_SUPPLIER = dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_SERIALNO = dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_DESCRIPTION = dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_QUANTITY = dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_PRICE = dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_DELIVERYDATE = dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_ASSIGNDATE = dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_BARCODE = dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column1 = dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column2 = dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column3 = dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column4 = dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column5 = dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column6 = dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column7 = dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column8 = dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column9 = dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column10 = dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column11 = dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column12 = dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column13 = dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";
            ////Orientation_Column14 = dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Normal" ? "N" : "R";


            //1
            if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_ASSETCODE = "N";
            }
            else if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_ASSETCODE = "R";
            }
            else if (dt_lblConfig.Rows[0]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_ASSETCODE = "I";
            }
            else
            {
                Orientation_ASSETCODE = "B";
            }

            //2 
            if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_CATEGORY = "N";
            }
            else if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_CATEGORY = "R";
            }
            else if (dt_lblConfig.Rows[1]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_CATEGORY = "I";
            }
            else
            {
                Orientation_CATEGORY = "B";
            }

            //3
            if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_SUBCATEGORY = "N";
            }
            else if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_SUBCATEGORY = "R";
            }
            else if (dt_lblConfig.Rows[2]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_SUBCATEGORY = "I";
            }
            else
            {
                Orientation_SUBCATEGORY = "B";
            }

            //4
            if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_BUILDING = "N";
            }
            else if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_BUILDING = "R";
            }
            else if (dt_lblConfig.Rows[3]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_BUILDING = "I";
            }
            else
            {
                Orientation_BUILDING = "B";
            }

            //5
            if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_FLOOR = "N";
            }
            else if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_FLOOR = "R";
            }
            else if (dt_lblConfig.Rows[4]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_FLOOR = "I";
            }
            else
            {
                Orientation_FLOOR = "B";
            }

            //6
            if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_LOCATION = "N";
            }
            else if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_LOCATION = "R";
            }
            else if (dt_lblConfig.Rows[5]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_LOCATION = "I";
            }
            else
            {
                Orientation_LOCATION = "B";
            }

            //7
            if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_DEPARTMENT = "N";
            }
            else if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_DEPARTMENT = "R";
            }
            else if (dt_lblConfig.Rows[6]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_DEPARTMENT = "I";
            }
            else
            {
                Orientation_DEPARTMENT = "B";
            }

            //8
            if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_CUSTODIAN = "N";
            }
            else if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_CUSTODIAN = "R";
            }
            else if (dt_lblConfig.Rows[7]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_CUSTODIAN = "I";
            }
            else
            {
                Orientation_CUSTODIAN = "B";
            }

            //9
            if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_SUPPLIER = "N";
            }
            else if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_SUPPLIER = "R";
            }
            else if (dt_lblConfig.Rows[8]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_SUPPLIER = "I";
            }
            else
            {
                Orientation_SUPPLIER = "B";
            }

            //10
            if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_SERIALNO = "N";
            }
            else if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_SERIALNO = "R";
            }
            else if (dt_lblConfig.Rows[9]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_SERIALNO = "I";
            }
            else
            {
                Orientation_SERIALNO = "B";
            }

            //11
            if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_DESCRIPTION = "N";
            }
            else if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_DESCRIPTION = "R";
            }
            else if (dt_lblConfig.Rows[10]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_DESCRIPTION = "I";
            }
            else
            {
                Orientation_DESCRIPTION = "B";
            }

            //12
            if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_QUANTITY = "N";
            }
            else if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_QUANTITY = "R";
            }
            else if (dt_lblConfig.Rows[11]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_QUANTITY = "I";
            }
            else
            {
                Orientation_QUANTITY = "B";
            }

            //13
            if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_PRICE = "N";
            }
            else if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_PRICE = "R";
            }
            else if (dt_lblConfig.Rows[12]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_PRICE = "I";
            }
            else
            {
                Orientation_PRICE = "B";
            }

            //14
            if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_DELIVERYDATE = "N";
            }
            else if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_DELIVERYDATE = "R";
            }
            else if (dt_lblConfig.Rows[13]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_DELIVERYDATE = "I";
            }
            else
            {
                Orientation_DELIVERYDATE = "B";
            }

            //15
            if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_ASSIGNDATE = "N";
            }
            else if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_ASSIGNDATE = "R";
            }
            else if (dt_lblConfig.Rows[14]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_ASSIGNDATE = "I";
            }
            else
            {
                Orientation_ASSIGNDATE = "B";
            }

            //16
            if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_BARCODE = "N";
            }
            else if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_BARCODE = "R";
            }
            else if (dt_lblConfig.Rows[15]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_BARCODE = "I";
            }
            else
            {
                Orientation_BARCODE = "B";
            }

            //17
            if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column1 = "N";
            }
            else if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column1 = "R";
            }
            else if (dt_lblConfig.Rows[16]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column1 = "I";
            }
            else
            {
                Orientation_Column1 = "B";
            }


            //18
            if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column2 = "N";
            }
            else if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column2 = "R";
            }
            else if (dt_lblConfig.Rows[17]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column2 = "I";
            }
            else
            {
                Orientation_Column2 = "B";
            }


            //19
            if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column3 = "N";
            }
            else if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column3 = "R";
            }
            else if (dt_lblConfig.Rows[18]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column3 = "I";
            }
            else
            {
                Orientation_Column3 = "B";
            }


            //20
            if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column4 = "N";
            }
            else if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column4 = "R";
            }
            else if (dt_lblConfig.Rows[19]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column4 = "I";
            }
            else
            {
                Orientation_Column4 = "B";
            }

            //21
            if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column5 = "N";
            }
            else if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column5 = "R";
            }
            else if (dt_lblConfig.Rows[20]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column5 = "I";
            }
            else
            {
                Orientation_Column5 = "B";
            }

            //22
            if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column6 = "N";
            }
            else if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column6 = "R";
            }
            else if (dt_lblConfig.Rows[21]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column6 = "I";
            }
            else
            {
                Orientation_Column6 = "B";
            }

            //23
            if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column7 = "N";
            }
            else if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column7 = "R";
            }
            else if (dt_lblConfig.Rows[22]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column7 = "I";
            }
            else
            {
                Orientation_Column7 = "B";
            }

            //24
            if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column8 = "N";
            }
            else if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column8 = "R";
            }
            else if (dt_lblConfig.Rows[23]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column8 = "I";
            }
            else
            {
                Orientation_Column8 = "B";
            }

            //25
            if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column9 = "N";
            }
            else if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column9 = "R";
            }
            else if (dt_lblConfig.Rows[24]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column9 = "I";
            }
            else
            {
                Orientation_Column9 = "B";
            }

            //26
            if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column10 = "N";
            }
            else if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column10 = "R";
            }
            else if (dt_lblConfig.Rows[25]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column10 = "I";
            }
            else
            {
                Orientation_Column10 = "B";
            }

            //27
            if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column11 = "N";
            }
            else if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column11 = "R";
            }
            else if (dt_lblConfig.Rows[26]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column11 = "I";
            }
            else
            {
                Orientation_Column11 = "B";
            }

            //28
            if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column12 = "N";
            }
            else if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column12 = "R";
            }
            else if (dt_lblConfig.Rows[27]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column12 = "I";
            }
            else
            {
                Orientation_Column12 = "B";
            }

            //29
            if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column13 = "N";
            }
            else if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column13 = "R";
            }
            else if (dt_lblConfig.Rows[28]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column13 = "I";
            }
            else
            {
                Orientation_Column13 = "B";
            }

            //30
            if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Normal")
            {
                Orientation_Column14 = "N";
            }
            else if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Rotated")
            {
                Orientation_Column14 = "R";
            }
            else if (dt_lblConfig.Rows[29]["ORIENTATION"].ToString() == "Inverted")
            {
                Orientation_Column14 = "I";
            }
            else
            {
                Orientation_Column14 = "B";
            }


            System.Text.StringBuilder strSbPrint = new System.Text.StringBuilder();

            foreach (DataRow dr in dt_PrintData.Rows)
            {


                ls_AssetCode = "";
                ls_EncodedAssetCode = "";
                ls_Category = "";
                ls_Sub_Category = "";

                ls_AssetCode = dr["AssetCode"].ToString();
                ls_AssetCode = ls_AssetCode.ToUpper();

                ls_Category = dr["CategoryCode"].ToString();
                ls_Category = ls_Category.ToUpper();
                ls_Category = ls_Category.Substring(4, 3);

                ls_Sub_Category = dr["SubCatCode"].ToString();
                ls_Sub_Category = ls_Sub_Category.ToUpper();
                ls_Sub_Category = ls_Sub_Category.Substring(4, 3);

                string prefix = dt_lblConfig.Rows[0]["Prefix"].ToString();

                if (prefix == "" || prefix == null)
                {
                    //MessageBox.Show("Please Configure client prefix.");
                    //return;
                }
                if (ls_Category.Length != 3 || ls_Sub_Category.Length != 3)
                {
                    MessageBox.Show("Please check " + Category + " and " + SubCategory + ".");
                    return;
                }
                ls_EncodedAssetCode = lobj_EncoderClass.EncodeTheTag(prefix + ls_AssetCode);

                if (ls_EncodedAssetCode == null || ls_EncodedAssetCode == "")
                {
                    MessageBox.Show("The tag is not encoded properly, printing unsuccessful.");
                    return;
                }

                lsw_DataToPrint.WriteLine("^XA");//ZPL Code will start from here (for starting of New label ^XA IS REQUIRED)
                                                 //lsw_DataToPrint.WriteLine("A" + ls_Category + ls_Sub_Category + ls_AssetCode);

                if (ASSETCODE_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_ASSETCODE);
                    lsw_DataToPrint.WriteLine("^A" + Font_ASSETCODE + Orientation_ASSETCODE + Size_ASSETCODE);
                    lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (BARCODE_STATUS == true)
                {


                    lsw_DataToPrint.WriteLine("^FO" + Pos_BARCODE);
                    //ADDED
                    lsw_DataToPrint.WriteLine("^BY1");
                    lsw_DataToPrint.WriteLine("^BC" + Orientation_BARCODE + ",30,N,N,50,N");
                    //ENDED
                    lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (CATEGORY_Status == true)
                {
                    lsw_DataToPrint.WriteLine("^FO" + Pos_CATEGORY);
                    lsw_DataToPrint.WriteLine("^A" + Font_CATEGORY + Orientation_CATEGORY + Size_CATEGORY);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Category"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (SUBCATEGORY_Status == true)
                {
                    lsw_DataToPrint.WriteLine("^FO" + Pos_SUBCATEGORY);
                    lsw_DataToPrint.WriteLine("^A" + Font_SUBCATEGORY + Orientation_SUBCATEGORY + Size_SUBCATEGORY);
                    lsw_DataToPrint.WriteLine("^FD" + dr["SubCategory"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (BUILDING_Status == true)
                {
                    lsw_DataToPrint.WriteLine("^FO" + Pos_BUILDING);
                    lsw_DataToPrint.WriteLine("^A" + Font_BUILDING + Orientation_BUILDING + Size_BUILDING);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Building"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");
                }
                if (FLOOR_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_FLOOR);
                    lsw_DataToPrint.WriteLine("^A" + Font_FLOOR + Orientation_FLOOR + Size_FLOOR);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Floor"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (LOCATION_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_LOCATION);
                    lsw_DataToPrint.WriteLine("^A" + Font_LOCATION + Orientation_LOCATION + Size_LOCATION);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Location"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (DEPARTMENT_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_DEPARTMENT);
                    lsw_DataToPrint.WriteLine("^A" + Font_DEPARTMENT + Orientation_DEPARTMENT + Size_DEPARTMENT);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Department"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (CUSTODIAN_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_CUSTODIAN);
                    lsw_DataToPrint.WriteLine("^A" + Font_CUSTODIAN + Orientation_CUSTODIAN + Size_CUSTODIAN);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Custodian"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (SUPPLIER_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_SUPPLIER);
                    lsw_DataToPrint.WriteLine("^A" + Font_SUPPLIER + Orientation_SUPPLIER + Size_SUPPLIER);
                    lsw_DataToPrint.WriteLine("^FD" + dr["SupplierName"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");
                }

                if (SERIALNO_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_SERIALNO);
                    lsw_DataToPrint.WriteLine("^A" + Font_SERIALNO + Orientation_SERIALNO + Size_SERIALNO);
                    lsw_DataToPrint.WriteLine("^FD" + dr["SerialNo"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");
                }

                if (DESCRIPTION_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_DESCRIPTION);
                    lsw_DataToPrint.WriteLine("^A" + Font_DESCRIPTION + Orientation_DESCRIPTION + Size_DESCRIPTION);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Description"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");
                }
                if (QUANTITY_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_QUANTITY);
                    lsw_DataToPrint.WriteLine("^A" + Font_QUANTITY + Orientation_QUANTITY + Size_QUANTITY);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Quantity"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (PRICE_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_PRICE);
                    lsw_DataToPrint.WriteLine("^A" + Font_PRICE + Orientation_PRICE + Size_PRICE);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Price"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");
                }
                if (DELIVERYDATE_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_DELIVERYDATE);
                    lsw_DataToPrint.WriteLine("^A" + Font_DELIVERYDATE + Orientation_DELIVERYDATE + Size_DELIVERYDATE);
                    lsw_DataToPrint.WriteLine("^FD" + dr["DeliveryDate"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }
                if (ASSIGNDATE_Status == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_ASSIGNDATE);
                    lsw_DataToPrint.WriteLine("^A" + Font_ASSIGNDATE + Orientation_ASSIGNDATE + Size_ASSIGNDATE);
                    lsw_DataToPrint.WriteLine("^FD" + dr["AssignDate"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                }

                if (Column1_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column1);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column1 + Orientation_Column1 + Size_Column1);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column1"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column1);
                    strSbPrint.Append("^A" + Font_Column1 + Orientation_Column1 + Size_Column1);
                    strSbPrint.Append("^FD" + dr["Column1"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column2_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column2);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column2 + Orientation_Column2 + Size_Column2);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column2"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column2);
                    strSbPrint.Append("^A" + Font_Column2 + Orientation_Column2 + Size_Column2);
                    strSbPrint.Append("^FD" + dr["Column2"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column3_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column3);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column3 + Orientation_Column3 + Size_Column3);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column3"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column3);
                    strSbPrint.Append("^A" + Font_Column3 + Orientation_Column3 + Size_Column3);
                    strSbPrint.Append("^FD" + dr["Column3"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column4_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column4);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column4 + Orientation_Column4 + Size_Column4);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column4"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column4);
                    strSbPrint.Append("^A" + Font_Column4 + Orientation_Column4 + Size_Column4);
                    strSbPrint.Append("^FD" + dr["Column4"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column5_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column5);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column5 + Orientation_Column5 + Size_Column5);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column5"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column5);
                    strSbPrint.Append("^A" + Font_Column5 + Orientation_Column5 + Size_Column5);
                    strSbPrint.Append("^FD" + dr["Column5"].ToString());
                    strSbPrint.Append("^FS");
                }


                if (Column6_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column6);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column6 + Orientation_Column6 + Size_Column6);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column6"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column6);
                    strSbPrint.Append("^A" + Font_Column6 + Orientation_Column6 + Size_Column6);
                    strSbPrint.Append("^FD" + dr["Column6"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column7_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column7);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column7 + Orientation_Column7 + Size_Column7);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column7"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column7);
                    strSbPrint.Append("^A" + Font_Column7 + Orientation_Column7 + Size_Column7);
                    strSbPrint.Append("^FD" + dr["Column7"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column8_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column8);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column8 + Orientation_Column8 + Size_Column8);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column8"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column8);
                    strSbPrint.Append("^A" + Font_Column8 + Orientation_Column8 + Size_Column8);
                    strSbPrint.Append("^FD" + dr["Column8"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column9_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column9);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column9 + Orientation_Column9 + Size_Column9);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column9"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column9);
                    strSbPrint.Append("^A" + Font_Column9 + Orientation_Column9 + Size_Column9);
                    strSbPrint.Append("^FD" + dr["Column9"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column10_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column10);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column10 + Orientation_Column10 + Size_Column10);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column10"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column10);
                    strSbPrint.Append("^A" + Font_Column10 + Orientation_Column10 + Size_Column10);
                    strSbPrint.Append("^FD" + dr["Column10"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column11_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column11);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column11 + Orientation_Column11 + Size_Column11);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column11"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column11);
                    strSbPrint.Append("^A" + Font_Column11 + Orientation_Column11 + Size_Column11);
                    strSbPrint.Append("^FD" + dr["Column11"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column12_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column12);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column12 + Orientation_Column12 + Size_Column12);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column12"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column12);
                    strSbPrint.Append("^A" + Font_Column12 + Orientation_Column12 + Size_Column12);
                    strSbPrint.Append("^FD" + dr["Column12"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column13_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column13);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column13 + Orientation_Column13 + Size_Column13);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column13"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column13);
                    strSbPrint.Append("^A" + Font_Column13 + Orientation_Column13 + Size_Column13);
                    strSbPrint.Append("^FD" + dr["Column13"].ToString());
                    strSbPrint.Append("^FS");
                }

                if (Column14_STATUS == true)
                {

                    lsw_DataToPrint.WriteLine("^FO" + Pos_Column14);
                    lsw_DataToPrint.WriteLine("^A" + Font_Column14 + Orientation_Column14 + Size_Column14);
                    lsw_DataToPrint.WriteLine("^FD" + dr["Column14"].ToString());
                    lsw_DataToPrint.WriteLine("^FS");

                    strSbPrint.Append("^FO" + Pos_Column14);
                    strSbPrint.Append("^A" + Font_Column14 + Orientation_Column14 + Size_Column14);
                    strSbPrint.Append("^FD" + dr["Column14"].ToString());
                    strSbPrint.Append("^FS");
                }

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                SqlDataAdapter dpt = new SqlDataAdapter();

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SP_LABEL_CONFIG_DETAILS", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(ddlTagType.SelectedItem.ToString()))
                });

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt_Details = ds.Tables[0];

                    DataRow[] dr_barcode = dt_Details.Select("FieldName='Barcode'");
                    if (dr_barcode.Length > 0)
                    {
                        try
                        {
                            string orient = dr_barcode[0]["orientation"].ToString() == "Normal" ? "N" : "R";
                            lsw_DataToPrint.WriteLine("^FO" + dr_barcode[0]["Position"].ToString());
                            lsw_DataToPrint.WriteLine("^BY1");
                            lsw_DataToPrint.WriteLine("^BC" + orient + ",30,N,N,50,N");
                            lsw_DataToPrint.WriteLine("^FD" + dr["AssetCode"].ToString());
                            lsw_DataToPrint.WriteLine("^FS");
                        }
                        catch
                        {

                        }
                    }

                    DataRow[] dr_company = dt_Details.Select("FieldName='Company'");
                    if (dr_company.Length > 0)
                    {
                        try
                        {

                            //dt_lblConfig.Rows[29]["FONT"].ToString().Substring(dt_lblConfig.Rows[29]["FONT"].ToString().Length - 1);
                            string font = dr_company[0]["Font"].ToString().Substring(dr_company[0]["Font"].ToString().Length - 1);
                            string orient = dr_company[0]["orientation"].ToString() == "Normal" ? "N" : "R";

                            lsw_DataToPrint.WriteLine("^FO" + dr_company[0]["Position"].ToString());
                            lsw_DataToPrint.WriteLine("^A" + font + orient + dr_company[0]["FontSize"].ToString());
                            lsw_DataToPrint.WriteLine("^FD" + dr_company[0]["Company"].ToString());
                            lsw_DataToPrint.WriteLine("^FS");

                            strSbPrint.Append("^FO" + dr_company[0]["Position"].ToString());
                            strSbPrint.Append("^A" + font + orient + dr_company[0]["FontSize"].ToString());
                            strSbPrint.Append("^FD" + dr_company[0]["Company"].ToString());
                            strSbPrint.Append("^FS");
                        }
                        catch
                        {

                        }
                    }
                }

                lsw_DataToPrint.WriteLine("^XZ");
                strSbPrint.Append("^XZ");

            }
            lsw_DataToPrint.Flush();
            lsw_DataToPrint.Close();

            string lLabel = File.ReadAllText(ls_FilesToCreate);
            if (lLabel == "^XA\r\n^XZ\r\n")
            {
                return;
            }
            string lLabelSize = "3.2x1.7";

            string lUrl = @"http://api.labelary.com/v1/printers/8dpmm/labels/" + lLabelSize + @"/0/" + lLabel;
            trheaderPrview.BgColor = "#98CODA";
            trfooterPreview.BgColor = "#98CODA";
            Image1.ImageUrl = lUrl;

            ModalPopupExtender22.Show();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "btnPreview_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_DataBinding", path);
        }
    }

    private void BinTagType()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTagType");
            ddlTagType.DataSource = ds;
            ddlTagType.DataTextField = "Name";
            ddlTagType.DataValueField = "Id";
            ddlTagType.DataBind();
            ddlTagType.SelectedIndex = 1;
            //ddlTagType.Items.Insert(0, new ListItem("--Select Tag--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "BinTagType", path);
        }
    }

    protected void ddlTagType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "ddlTagType_SelectedIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelReprint.aspx", "gvData_ItemCreated", path);

        }
    }
}