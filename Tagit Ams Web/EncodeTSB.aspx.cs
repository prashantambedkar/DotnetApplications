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

public partial class EncodeTSB : System.Web.UI.Page
{
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_Init", path);

        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            if (!IsPostBack)
            {
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                Page.DataBind();
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.LabelPrinting, Session["userid"].ToString()) == true)
                {

                    Common.Bindcategory((DropDownList)ddlproCategory);
                    Common.BindLocation((DropDownList)ddlloc, Session["userid"].ToString());
                    Common.BindDepartMent((DropDownList)ddldept);
                    //////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddldept.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                    // RadYes.Checked = true;
                    bindCustodian();
                    //BinTagType();
                    grid_view();
                    SetGridOrder();
                    string ClientCode = (SqlHelper.ExecuteScalar(con, CommandType.Text, "select Max(Prefix) from [Label_Config]")).ToString();
                    hdnClientCode.Value = ClientCode;
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "SetGridOrder", path);
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

            string ClientCode = (SqlHelper.ExecuteScalar(con, CommandType.Text, "select Max(Prefix) from [Label_Config]")).ToString();
            hdnClientCode.Value = ClientCode;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "bindCustodian", path);
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
            //ddlAsstID.Items.Clear();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "OnSelectedIndexChangedCategory", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "OnSelectedIndexChangedLcocation", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "OnSelectedIndexChangedBuilding", path);
        }
    }

    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    grid_view();
    //}
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "btnsearchsubmit_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "btnSearch_Click", path);
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
            //ddlAsstID.Items.Clear();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "ResetSearch", path);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            txtAssetCode.Text = "";
            txtSearch.Text = "";
            this.AssetId = 0;
            ddlproCategory.SelectedIndex = 0;
            //ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            ddlCustodian.SelectedIndex = 0;
            //ddlCustodian.SelectedIndex = 0;
            // OnSelectedIndexChangedDepartment(sender, e);
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "btnRefresh_Click", path);

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

            DataSet ds = Common.GetAssetDetailsForTSBEncodeV2(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode, CustodianId, SearchText, Session["userid"].ToString());
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "grid_view", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_NeedDataSource", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_DataBinding", path);
        }
    }


    //Added By ponraj
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "gvData_ItemCreated", path);
        }
    }

    protected void btnEncode_Click(object sender, EventArgs e)
    {
        try
        {
            AssetBL objAst = new AssetBL();
            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                if (chkitem.Checked == true)
                {
                    if ((objAst.UpdateAssetEncodedTSB(Convert.ToInt32(hdnAstID.Value), 1)) == true)
                    {
                        grid_view();
                        gvData.DataBind();
                        //string Message = "The " + Assets + " was successfully encoded.";
                        //imgpopup.ImageUrl = "images/Success.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Successfully Encoded');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        //string Message = "The " + Assets + " was not encoded.";
                        //imgpopup.ImageUrl = "images/CloseRed.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Asset Not Encoded');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "EncodeTSB.aspx", "btnEncode_Click", path);
        }
    }
}