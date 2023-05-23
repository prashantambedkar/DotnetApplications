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
using Microsoft.ApplicationBlocks.Data;
using Serco;
using ECommerce.Common;
using Telerik.Web.UI;

public partial class Subcategory : System.Web.UI.Page
{
    String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";

    public DataTable dt_Subcat
    {
        get
        {
            return ViewState["dt_Subcat"] as DataTable;
        }
        set
        {
            ViewState["dt_Subcat"] = value;

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "gvData_Init", path);
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
                PgHeader.InnerText = SubCategory + " Master";
                lblsubcattype.Text = SubCategory + " Name";
                lblcattype.Text = Category;

                chkActive.Checked = true;
                chkActive.Enabled = false;
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CategoryMaster, Session["userid"].ToString()) == true)
                {
                    Bincategory();
                    grid_view();
                }
                else
                {
                    //ModalPopupExtender1.Show();
                    Response.Redirect("AcceessError.aspx");
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "Page_Load", path);
        }

    }
    protected void btnSearchInfo_Click(object sender, EventArgs e)
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "btnSearchInfo_Click", path);
        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    protected void btnYes_Click(object sender, EventArgs e)
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
            ddlproCategory.DataSource = ds;
            ddlproCategory.DataTextField = "CategoryName";
            ddlproCategory.DataValueField = "CategoryId";
            ddlproCategory.DataBind();
            ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "Bincategory", path);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {

            //if (txtFullName.Text == "")
            //{
            //    string Message = "Please enter sub category name.";
            //    imgpopup.ImageUrl = "images/info.jpg";
            //    lblpopupmsg.Text = Message;
            //    trheader.BgColor = "#98CODA";
            //    trfooter.BgColor = "#98CODA";
            //    ModalPopupExtender2.Show();
            //    return;
            //}

            //if (ddlproCategory.SelectedIndex==0)
            //{
            //    string Message = "Please select subcategory.";
            //    imgpopup.ImageUrl = "images/info.jpg";
            //    lblpopupmsg.Text = Message;
            //    trheader.BgColor = "#98CODA";
            //    trfooter.BgColor = "#98CODA";
            //    ModalPopupExtender2.Show();
            //    return;
            //}
            if (btnsubmit.Text == "Submit")
            {
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.pInsertsubcategory, new SqlParameter[]
                        {
                             new SqlParameter("@Subcategory", txtFullName.Text.Trim()),
                             new SqlParameter("@Active", 1 ),
                             new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                             new SqlParameter("@CreatedDate", System.DateTime.Now),
                             new SqlParameter("@CategoryId", ddlproCategory.SelectedValue),

                        });
                if (help.ExecuteNonQuery() < 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);

                    string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    imgpopup.ImageUrl = "images/Success.png";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                }
                //grid_view();
                //btnreset_Click(sender, e);
            }
            else if (btnsubmit.Text == "Update")
            {
                if (hdnOldSubCat.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    CategoryBL objCatBL = new CategoryBL();
                    if (chkActive.Checked == false)
                    {
                        bool SubCategoryActive = objCatBL.CheckSubCategoryBelongsToAnyAsset(hdnsubcatidId.Value);
                        if (SubCategoryActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" already mapped with items,\\n you can’t deactivate this Sub category');", true);

                            string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" already mapped with items, you can’t deactivate this " + SubCategory + ".";
                            imgpopup.ImageUrl = "images/info.jpg";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            bool result = objCatBL.UpdateSubCategoryStatus(Convert.ToInt32(hdnsubcatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" was updated Successfully.');", true);

                                string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" was not Updated.');", true);
                                string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                imgpopup.ImageUrl = "images/CloseRed.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }

                        }
                    }
                    else
                    {
                        bool result = objCatBL.UpdateSubCategoryStatus(Convert.ToInt32(hdnsubcatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            imgpopup.ImageUrl = "images/Success.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\" was not Updated.');", true);
                            string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            imgpopup.ImageUrl = "images/CloseRed.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                    }

                }
                else
                {
                    CategoryBL objCat = new CategoryBL();
                    bool SubCategoryExist = objCat.CheckSubCategoryExists(txtFullName.Text.Trim());
                    if (SubCategoryExist == true)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\"  is already Exists');", true);

                        string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\"  is already Exists.";
                        imgpopup.ImageUrl = "images/info.jpg";
                        lblpopupmsg.Text = Message;
                        trheader.BgColor = "#98CODA";
                        trfooter.BgColor = "#98CODA";
                        ModalPopupExtender2.Show();
                    }
                    else
                    {
                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.Pupdatesubcat, new SqlParameter[] {
                           new SqlParameter("@SubCatId", Convert.ToInt32(hdnsubcatidId.Value)),
                           new SqlParameter("@SubCatName", txtFullName.Text.Trim()),
                           new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                           new SqlParameter("@CreatedDate", System.DateTime.Now),
                           new SqlParameter("@CategoryId", ddlproCategory.SelectedValue),
                           });

                        CategoryBL objcat = new CategoryBL();
                        if (chkActive.Checked == false)
                        {
                            bool SubCategoryActive = objcat.CheckSubCategoryBelongsToAnyAsset(hdnsubcatidId.Value);
                            if (SubCategoryActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The SubCategory \"" + txtFullName.Text.Trim() + "\"  already mapped with items,\\n you can’t deactivate this Sub category');", true);

                                string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\"  already mapped with items, you can’t deactivate this " + SubCategory + ".";
                                imgpopup.ImageUrl = "images/info.jpg";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                objcat.UpdateSubCategoryStatus(Convert.ToInt32(hdnsubcatidId.Value), chkActive.Checked == true ? 1 : 0);
                            }
                        }
                        else
                        {
                            objcat.UpdateSubCategoryStatus(Convert.ToInt32(hdnsubcatidId.Value), chkActive.Checked == true ? 1 : 0);
                        }

                        if (help.ExecuteNonQuery() <= 1)
                        {
                            ddlproCategory.Enabled = true;
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully.');", true);
                            string Message = "The " + SubCategory + " \"" + txtFullName.Text.Trim() + "\" was updated Successfully.";
                            imgpopup.ImageUrl = "images/Success.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }

                        btnsubmit.Text = "Add";

                    }
                }
            }
            hdnsubcatidId.Value = "";
            hdnOldSubCat.Value = "";
            //grid_view();
            btnreset_Click(sender, e);
            chkActive.Checked = true;
            chkActive.Enabled = false;
        }
        catch (Exception ex)
        {

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
            string Message = ex.Message.ToString();
            imgpopup.ImageUrl = "images/CloseRed.png";
            lblpopupmsg.Text = Message;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "btnsubmit_Click", path);
        }
    }
    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            ddlproCategory.Enabled = true;
            ddlproCategory.SelectedIndex = 0;
            btnsubmit.Text = "Submit";
            grid_view();
            gvData.DataBind();
            chkActive.Checked = true;
            chkActive.Enabled = false;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "btnreset_Click", path);
        }
    }
    public string StrSort;
    private void grid_view()
    {

        try
        {
            CategoryBL objCatBL = new CategoryBL();
            //DataSet ds = objCatBL.GetAllSubCategoryDetails();
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetAllSubCategoryDetails(SearchText);


            this.dt_Subcat = ds.Tables[0];
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
                if (myView.Count == 0)
                {
                    gvData.DataSource = string.Empty;
                }
                gvData.DataSource = myView;

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "grid_view", path);
        }
    }



    private void ShowSuccessMessage(string msg)
    {
        try
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "ShowSuccessMessage", path);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToLower());

            var SubCategoryDetails = dt_Subcat.AsEnumerable().Where(c => c.Field<string>("SubCatName").ToLower().Contains(text));
            if (SubCategoryDetails.Count() > 0)
            {
                DataTable dtSubCategoryDetails = SubCategoryDetails.CopyToDataTable<DataRow>();
                gvData.DataSource = dtSubCategoryDetails;
                gvData.DataBind();
                lblcnt.Text = Convert.ToString(dtSubCategoryDetails.Rows.Count);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record Found.!!');", true);

            }
            txtSearch.Text = "";

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "btnSearch_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "gvData_NeedDataSource", path);
        }
    }


    // For editing and deleting
    protected void GvData_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "dit")
            {
                CategoryBL objcat = new CategoryBL();
                GridDataItem item = (GridDataItem)e.Item;
                string subcatcode = item["subcatcode"].Text;
                string subCategoryId = item["SubCatId"].Text;
                string subCategoryName = item["SubCatName"].Text;
                string hidcatid = item["Categoryid"].Text;
                string Active = item["Status"].Text;

                hdnsubcatidId.Value = subCategoryId;
                hidsubcatcode.Value = subcatcode;
                txtFullName.Text = subCategoryName;
                hdnOldSubCat.Value = subCategoryName;
                ddlproCategory.SelectedValue = hidcatid;
                ddlproCategory.Enabled = false;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";

                chkActive.Enabled = true;
                //Label subcatcode = e.Item.Cells[0].FindControl("subcatcode") as Label;
                //Label subCategoryId = e.Item.Cells[0].FindControl("subCategoryId") as Label;
                //Label subCategoryName = e.Item.Cells[0].FindControl("subCategoryName") as Label;
                //HiddenField hidcatid = e.Item.Cells[0].FindControl("hidcatid") as HiddenField;
                //hdnsubcatidId.Value = subCategoryId.Text;
                //hidsubcatcode.Value = subcatcode.Text;
                //txtFullName.Text = subCategoryName.Text;
                //hdnOldSubCat.Value = subCategoryName.Text;
                //ddlproCategory.SelectedValue = hidcatid.Value;
                //ddlproCategory.Enabled = false;
                //btnsubmit.Text = "Update";


            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "GvData_ItemCommand", path);
        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["subcatcode"].Text = SubCategory.ToUpper() + " ID";
                item["CategoryName"].Text = Category.ToUpper() + " NAME";
                item["SubCatName"].Text = SubCategory.ToUpper() + " NAME";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["subcatcode"].Text = SubCategory.ToUpper() + " ID";
                item["CategoryName"].Text = Category.ToUpper() + " NAME";
                item["SubCatName"].Text = SubCategory.ToUpper() + " NAME";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Subcategory.aspx", "gvData_ItemCreated", path);
        }
    }
}