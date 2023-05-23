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
using System.Text;
using Telerik.Web.UI;

public partial class Sendasset : System.Web.UI.Page
{
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
    public DataTable dtAssetDetails
    {
        get
        {
            return ViewState["dtAssetDetails"] as DataTable;
        }
        set
        {
            ViewState["dtAssetDetails"] = value;

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ////if (userAuthorize((int)pages.SendAsset, Session["userid"].ToString()) == true)
            ////{
                Common.Bindcategory((DropDownList)ddlproCategory);
                Common.BindLocation((DropDownList)ddlloc,Session["userid"].ToString());
                Common.BindDepartMent((DropDownList)ddldept);
                ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
                ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
                ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
                ddldept.Items.Insert(0, new ListItem("--Select Department--", "0", true));
                BindGrid_view();
            ////}
            ////else
            ////{
            ////    Response.Redirect("AcceessError.aspx");
            ////}
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
    private void BindGrid_view()
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

            OperationBL objBL = new OperationBL();
            DataSet ds = objBL.GetAssetDetailsForStockChecking(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId);
            this.dtAssetDetails = ds.Tables[0];

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
    protected void btnsearchsubmit_Click(object sender, EventArgs e)
    {
        // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
        BindGrid_view();
        gvData.DataBind();
        //ResetSearch();
    }

    private void ResetSearch()
    {
        ddlproCategory.SelectedValue = "0";
        ddlsubcat.Items.Clear();
        ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
        ddlloc.SelectedValue = "0";
        ddlbuild.Items.Clear();
        ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
        ddlfloor.Items.Clear();
        ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
        ddldept.SelectedValue = "0";
        BindGrid_view();
        gvData.DataBind();
        // ddlAsstID.Items.Clear();
    }

    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {
        Common.bindSubCategory((DropDownList)ddlsubcat, ddlproCategory.SelectedValue);
        ddlsubcat.SelectedValue = "0";
        ////ddlloc.SelectedValue = "0";
        ////ddlbuild.SelectedValue = "0";
        ////ddlfloor.SelectedValue = "0";
        ////ddldept.SelectedValue = "0";
        //ddlAsstID.Items.Clear();
    }
    protected void OnSelectedIndexChangedLcocation(object sender, EventArgs e)
    {
        Common.bindBuilding((DropDownList)ddlbuild, ddlloc.SelectedValue);
        ddlfloor.SelectedValue = "0";
        ddldept.SelectedValue = "0";
    }

    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        Common.BindFloor((DropDownList)ddlfloor, ddlbuild.SelectedValue);
        ddldept.SelectedValue = "0";
    }
    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    BindGrid_view();
    //}
    //protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
    //{
    //    if (ddldept.SelectedIndex == 0)
    //    {
    //        ddlAsstID.Items.Clear();
    //    }
    //    else
    //    {
    //        PrintBL objBL = new PrintBL();

    //        objBL.BindActiveAssetsForEncode((ListBox)ddlAsstID, ddlproCategory.SelectedValue, ddlsubcat.SelectedValue, ddlloc.SelectedValue, ddlbuild.SelectedValue, ddlfloor.SelectedValue, ddldept.SelectedValue);
    //    }
    //}

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.AssetId = 0;
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        ddlbuild.SelectedIndex = 0;
        ddlfloor.SelectedIndex = 0;
        ddlloc.SelectedIndex = 0;
        ddldept.SelectedIndex = 0;
        // OnSelectedIndexChangedDepartment(sender, e);
        BindGrid_view();
        gvData.DataBind();
    }
    protected void BtnSendTHR_Click(object sender, EventArgs e)
    {
        Session["THRStock"] = "";
        DataTable dt_SelectedAsset = new DataTable();
        dt_SelectedAsset.Columns.Add("AssetId", typeof(int));




        //for (int i = 0; i < gvData.MasterTableView.Items.Count; i++)
        //{
        //    GridDataItem dataItem = (GridDataItem)gvData.MasterTableView.Items[i];
        //    string AssetID = dataItem.GetDataKeyValue("AssetId").ToString();
        
        //    dt_SelectedAsset.Rows.Add(AssetID);
        //}


        //dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
        //dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
        //var results = (from table1 in dtAssetDetails.AsEnumerable()
        //               join table2 in dt_SelectedAsset.AsEnumerable()
        //               on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
        //               select table1).ToList();

        dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };

        DataTable dt_StockData = new DataTable();
        if (dtAssetDetails.Rows.Count == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
            return;
        }
        if (dtAssetDetails.Rows.Count > 0)
        {
            dt_StockData = dtAssetDetails;
        }
        System.Text.StringBuilder sp = new System.Text.StringBuilder();
        sp.Append("<Tagit>");
        sp.Append("<LineItems>");
        foreach (DataRow dr in dt_StockData.Rows)
        {
            sp.Append("<LineItem AssetId =" + '"' + dr["AssetId"].ToString() + '"' + " Asset_Code =" + '"' + dr["AssetCode"].ToString() + '"' + " Buildingid =" + '"' + dr["Buildingid"].ToString() + '"' + " Floorid =" + '"' + dr["Floorid"].ToString() + '"' + " Locationid =" + '"' + dr["Locationid"].ToString() + '"' + " />");
        }
        sp.Append("</LineItems>");
        sp.Append("</Tagit>");
        Session["THRStock"] = sp.ToString();
        string strPopup = "<script language='javascript' ID='script1'>"

               // Passing intId to popup window.
               + "window.open('DownloadTHRStock.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

               + "</script>";

        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
    }
    protected void BtnSendTHS_Click(object sender, EventArgs e)
    {
        try
        {
            Session["THSStock"] = "";
            DataTable dt_SelectedAsset = new DataTable();
            dt_SelectedAsset.Columns.Add("AssetId", typeof(int));



            for (int i = 0; i < gvData.MasterTableView.Items.Count; i++)
            {
                GridDataItem dataItem = (GridDataItem)gvData.MasterTableView.Items[i];
                string AssetID = dataItem.GetDataKeyValue("AssetId").ToString();

                dt_SelectedAsset.Rows.Add(AssetID);
            }


            //dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
            //dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
            //var results = (from table1 in dtAssetDetails.AsEnumerable()
            //               join table2 in dt_SelectedAsset.AsEnumerable()
            //               on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
            //               select table1).ToList();

            dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };


            DataTable dt_PrintData = new DataTable();
            var JSONString = new StringBuilder();
            string output = "";

            if (dtAssetDetails.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                return;
            }
            if (dtAssetDetails.Rows.Count > 0)
            {
                dt_PrintData = dtAssetDetails;
                dt_PrintData = RemoveColumnsFromTable(dt_PrintData);

                JSONString.Append("[");
                for (int i = 0; i < dt_PrintData.Rows.Count; i++)
                {

                    JSONString.Append("{");
                    string Imgname = "Image";
                    JSONString.Append("\"" + Imgname + "\":\"\",");
                    for (int j = 0; j < dt_PrintData.Columns.Count; j++)
                    {
                        output = output + dt_PrintData.Rows[0]["AssetId"].ToString() + ",";
                        //JSONString.Append("Image:");

                        if (j < dt_PrintData.Columns.Count - 1)
                        {
                            if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetCode" || dt_PrintData.Columns[j].ColumnName.ToString() == "Category" || dt_PrintData.Columns[j].ColumnName.ToString() == "SubCategory"
                                || dt_PrintData.Columns[j].ColumnName.ToString() == "DeliveryDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "AssignDate" ||
                                dt_PrintData.Columns[j].ColumnName.ToString() == "Floor" || dt_PrintData.Columns[j].ColumnName.ToString() == "Building" || dt_PrintData.Columns[j].ColumnName.ToString() == "SerialNo"
                                || dt_PrintData.Columns[j].ColumnName.ToString() == "Location" || dt_PrintData.Columns[j].ColumnName.ToString() == "Custodian" || dt_PrintData.Columns[j].ColumnName.ToString() == "SupplierName" || dt_PrintData.Columns[j].ColumnName.ToString() == "Description")
                            {
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt_PrintData.Rows[i][j].ToString().Replace("\"", "'") + "\",");
                            }

                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetId")
                            {
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + dt_PrintData.Rows[i][j].ToString() + ",");
                            }
                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Quantity")
                            {
                                string Quantity = dt_PrintData.Rows[i][j].ToString() == "" ? "1" : dt_PrintData.Rows[i][j].ToString();
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Quantity + ",");
                            }
                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Price")
                            {
                                string Price = dt_PrintData.Rows[i][j].ToString() == "" ? "0" : dt_PrintData.Rows[i][j].ToString();
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Price + ",");
                            }

                        }
                        else if (j == dt_PrintData.Columns.Count - 1)
                        {
                            if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetCode" || dt_PrintData.Columns[j].ColumnName.ToString() == "Category" || dt_PrintData.Columns[j].ColumnName.ToString() == "SubCategory"
                                || dt_PrintData.Columns[j].ColumnName.ToString() == "DeliveryDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "AssignDate" ||
                                dt_PrintData.Columns[j].ColumnName.ToString() == "Floor" || dt_PrintData.Columns[j].ColumnName.ToString() == "Building" || dt_PrintData.Columns[j].ColumnName.ToString() == "SerialNo"
                                || dt_PrintData.Columns[j].ColumnName.ToString() == "Location" || dt_PrintData.Columns[j].ColumnName.ToString() == "Custodian" || dt_PrintData.Columns[j].ColumnName.ToString() == "SupplierName")
                            {
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt_PrintData.Rows[i][j].ToString().Replace("\"", "'") + "\"");
                            }
                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetId")
                            {
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + dt_PrintData.Rows[i][j].ToString());
                            }
                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Quantity")
                            {
                                string Quantity = dt_PrintData.Rows[i][j].ToString() == "" ? "1" : dt_PrintData.Rows[i][j].ToString();
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Quantity);
                            }
                            else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Price")
                            {
                                string Price = dt_PrintData.Rows[i][j].ToString() == "" ? "0" : dt_PrintData.Rows[i][j].ToString();
                                JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Price);
                            }

                        }
                    }
                    if (i == dt_PrintData.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }

                }
                JSONString.Append("]");
            }
            Session["THSStock"] = JSONString.ToString();
            string strPopup = "<script language='javascript' ID='script1'>"

                     // Passing intId to popup window.
                     + "window.open('DownloadTHSStock.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

                     + "</script>";

            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
        catch (Exception)
        {

            throw;
        }
    }
    private DataTable RemoveColumnsFromTable(DataTable dt_PrintData)
    {
        dt_PrintData.Columns.Remove("BuildingId");
        dt_PrintData.Columns.Remove("CategoryId");
        dt_PrintData.Columns.Remove("CategoryCode");
        dt_PrintData.Columns.Remove("CustodianId");
        dt_PrintData.Columns.Remove("DepartmentId");
        dt_PrintData.Columns.Remove("LocationId");
        dt_PrintData.Columns.Remove("FloorId");
        dt_PrintData.Columns.Remove("SubCatId");
        dt_PrintData.Columns.Remove("SubCatCode");
        // dt_PrintData.Columns.Remove("Active");
        // dt_PrintData.Columns.Remove("");

        return dt_PrintData;
    }
    protected void BtnMasterDownload_Click(object sender, EventArgs e)
    {
        OperationBL objBL = new OperationBL();
        DataTable dtMaster = objBL.GetMasterFile();
        System.Text.StringBuilder sp = new System.Text.StringBuilder();
        sp.Append("<Tagit>");
        sp.Append("<LineItems>");
        foreach (DataRow dr in dtMaster.Rows)
        {
            sp.Append("<LineItem ID =" + '"' + dr["ID"].ToString() + '"' + " Name =" + '"' + dr["Name"].ToString() + '"' + " Type =" + '"' + dr["Type"].ToString() + '"' + " />");
        }
        sp.Append("</LineItems>");
        sp.Append("</Tagit>");
        Session["MasterFile"] = sp.ToString();
        string strPopup = "<script language='javascript' ID='script1'>"

               // Passing intId to popup window.
               + "window.open('DownloadMaster.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

               + "</script>";

        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid_view();
    }

    protected void showmodal_Click(object sender, EventArgs e)
    {

    }
}