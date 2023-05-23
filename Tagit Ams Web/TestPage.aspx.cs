using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;
using ECommerce.Common;

public partial class TestPage : System.Web.UI.Page
{

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

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
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
    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Sort" || e.CommandName == "Page")
        {
            RadToolTipManager1.TargetControls.Clear();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        grid_view();
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        grid_view();
    }

    private void grid_view()
    {
        try
        {

            string TranId = "T5";
            string Asset =  null;
            string CategoryId = null;
            string SubCatId = null;
            string LocationId = null;
            string BuildingId = null;
            string FloorId = null;
            string DepartmentId = null;
            string AssetCode = null;
            string SearchText = null;
            string CustodianId = null;
            string TagType = "T6";// ddlTagType.SelectedItem.ToString();


            DataSet ds = Common.GetIdentifiedAssetDetails(TranId, Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode, CustodianId, SearchText, TagType);
            ////this.dtAssetDetails = ds.Tables[0];
            DataTable dtAssetDetails = new DataTable();
            dtAssetDetails = ds.Tables[0];
            RadGrid1.DataSource = dtAssetDetails;
            
           
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
        }
    }
}