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

public partial class CategoryMaster : System.Web.UI.Page
{
    String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
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
                PgHeader.InnerText = Category + " Master";
                lblcattype.Text = Category + " Name";

                chkActive.Checked = true;
                chkActive.Enabled = false;
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CategoryMaster, Session["userid"].ToString()) == true)
                {
                    grid_view();
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }
                // txtFullName.Text = "Category Name!";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "Page_Load", path);
        }

    }

    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gvData_Init", path);
        }
    }


    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFullName.Text == "")
            {
                //string Message = "Please enter " + Category + " name.";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender2.Show();
                //return;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Already mapped with items,you cannot deactive');", true);
                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            if (btnsubmit.Text.ToLower() == "submit")
            {

                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.PinsertCategory, new SqlParameter[]
                        {
                            new SqlParameter("@category", txtFullName.Text.ToString().Trim()),

                    new SqlParameter("@Active",  1),
                    new SqlParameter("@USER_NAME", Session["UserName"].ToString()),
                    new SqlParameter("@CreatedDate", System.DateTime.Now),
                    new SqlParameter("@CreatedBY", Session["userid"].ToString()),

                        }
                        );

                if (help.ExecuteNonQuery() < 1)
                {

                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The category " + txtFullName.Text + "was successfully added.');", true);
                    //string Message = "The " + Category.ToLower() + " \"" + txtFullName.Text + "\" was successfully added.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Added');", true);
                    ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                //grid_view();
                //btnreset_Click(sender, e);

            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                if (hdnOldCategory.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    if (chkActive.Checked == false)
                    {
                        CategoryBL objcat = new CategoryBL();
                        bool CategoryExstn = objcat.CheckCategoryBelongToSubCategory(hdncatidId.Value);
                        if (CategoryExstn == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This category already mapped with items,\\n you can’t deactivate this category');", true);

                            //string Message = "Already mapped with items, you can’t deactivate this.";
                            //imgpopup.ImageUrl = "images/info.jpg";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Already mapped with items,you cannot deactive');", true);
                            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            CategoryBL objCatBL = new CategoryBL();
                            bool result = objCatBL.UpdateCategoryStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category  \"" + txtFullName.Text + "\"was successfully Updated.');", true);

                                //string Message = "The " + Category.ToLower() + "  \"" + txtFullName.Text + "\"was successfully updated.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Updated');", true);
                                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\" not Updated.');", true);
                                //string Message = "The " + Category.ToLower() + " \"" + txtFullName.Text + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Not Updated');", true);
                                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                            }
                        }
                    }
                    else
                    {
                        CategoryBL objCatBL = new CategoryBL();
                        bool result = objCatBL.UpdateCategoryStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\" was successfully Updated.');", true);
                            //string Message = "The " + Category.ToLower() + " \"" + txtFullName.Text + "\" was successfully updated.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Updated');", true);
                            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\" not Updated.');", true);

                            //string Message = "The " + Category.ToLower() + " \"" + txtFullName.Text + "\" was not updated.";
                            //imgpopup.ImageUrl = "images/CloseRed.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Not Updated');", true);
                            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                        }
                    }
                }
                else
                {

                    bool CategoryExist = CheckCategoryExists(txtFullName.Text.Trim());
                    if (CategoryExist == true)
                    {

                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\"  already Exists');", true);
                        //string Message = "The " + Category.ToLower() + " \"" + txtFullName.Text + "\"  already exists";
                        //imgpopup.ImageUrl = "images/CloseRed.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Already Exists');", true);
                        ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.PupdateCategory, new SqlParameter[] {
                    new SqlParameter("@CategoryId", Convert.ToInt32(hdncatidId.Value)),
                    new SqlParameter("@category", txtFullName.Text.Trim()),
                    new SqlParameter("@CreatedDate", System.DateTime.Now),
                    new SqlParameter("@CreatedBY", Session["userid"].ToString()),
                }
                           );

                        if (chkActive.Checked == false)
                        {
                            CategoryBL objcat = new CategoryBL();
                            bool CategoryExst = objcat.CheckCategoryBelongToSubCategory(hdncatidId.Value);
                            if (CategoryExist == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\" already mapped with items,\\n you can’t deactivate this category');", true);

                                //string Message = "Already mapped with items, you can’t deactivate this.";
                                //imgpopup.ImageUrl = "images/info.jpg";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Already mapped with items,you cannot deactive');", true);
                                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                CategoryBL objCatBL = new CategoryBL();
                                objCatBL.UpdateCategoryStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            }
                        }
                        else
                        {
                            CategoryBL objCatBL = new CategoryBL();
                            objCatBL.UpdateCategoryStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        }

                        if (help.ExecuteNonQuery() < 1)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The Category \"" + txtFullName.Text + "\" was updated successfully..!!');", true);

                            //string Message = "The " + Category + " \"" + txtFullName.Text + "\" was successfully updated.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Category Updated');", true);
                            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }

                        btnsubmit.Text = "Add";
                    }

                }
            }
            hdncatidId.Value = "";
            hdnOldCategory.Value = "";
            //grid_view();
           btnreset_Click(sender, e);
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "btnsubmit_Click", path);
            string Message = ex.Message;
            imgpopup.ImageUrl = "images/CloseRed.png";
            lblpopupmsg.Text = Message;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();
        }


    }

    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            // chkstatus.Checked = false;

            btnsubmit.Text = "SUBMIT";
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "btnreset_Click", path);
        }
    }


    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            CategoryBL objCatBL = new CategoryBL();
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetAllCategoryDetails(SearchText);

            this.dt_cat = ds.Tables[0];
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                lblMessage.Text = "Problem occured while retrieving Product records. Please try again.";
            }
            else
            {
                DataView myView;
                myView = dt_cat.DefaultView;
                lblcnt.Text = Convert.ToString(dt_cat.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                //gridlist.DataSource = myView;
                //gridlist.DataBind();

                gvData.DataSource = myView;

                // txtFullName.Text = "Category Name!";

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "grid_view", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "btnSearchInfo_Click", path);
        }
    }
    //btnSearch_Click
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //string AssetID = string.Join(",", dt_PrintData.AsEnumerable().Select(s => s.Field<int>("AssetId")).ToArray<int>());

            string text = Convert.ToString(txtSearch.Text.ToLower());

            var CategoryDetails = dt_cat.AsEnumerable().Where(c => c.Field<string>("CategoryName").ToLower().Contains(text));
            if (CategoryDetails.Count() > 0)
            {
                DataTable dtCategoryDetails = CategoryDetails.CopyToDataTable<DataRow>();
                //gridlist.DataSource = dtCategoryDetails;
                //gridlist.DataBind();
                gvData.DataSource = dtCategoryDetails;
                gvData.DataBind();
                lblcnt.Text = Convert.ToString(dtCategoryDetails.Rows.Count);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record Found.!!');", true);

            }
            txtSearch.Text = "";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "btnSearch_Click", path);

        }
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

            Label CategoryName = e.Item.Cells[0].FindControl("CategoryName") as Label;
            Label CategoryId = e.Item.Cells[0].FindControl("CategoryId") as Label;
            Label Active = e.Item.Cells[0].FindControl("Active") as Label;
            Label catcode = e.Item.Cells[0].FindControl("catcode") as Label;
            hidcatcode.Value = catcode.Text;
            hdncatidId.Value = CategoryId.Text;
            txtFullName.Text = CategoryName.Text;
            hdnOldCategory.Value = CategoryName.Text;
            btnsubmit.Text = "UPDATE";
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
            Logging.WriteErrorLog(Ex.Message.ToString(), Ex.StackTrace.ToString(), "CategoryMaster.aspx", "EditDataGrid", path);
        }
    }

    private bool CheckCategoryExists(string CategoryName)
    {
        //DataAccessHelper1 help = new DataAccessHelper1(

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkCategoryExists", new SqlParameter[] {
                        new SqlParameter("@CategoryName", CategoryName),
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

    protected void UpdateCategoryStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            CategoryBL objcat = new CategoryBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label CategoryId = (Label)item.FindControl("CategoryId");
            Label CategoryName = (Label)item.FindControl("CategoryName");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool CategoryExist = objcat.CheckCategoryBelongToSubCategory(CategoryId.Text);
            if (CategoryExist == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This " + Category.ToLower() + " already mapped with items,\\n you can’t deactivate this " + Category.ToLower() + "');", true);
            }
            else
            {
                CategoryBL objCatBL = new CategoryBL();
                objCatBL.UpdateCategoryStatus(Convert.ToInt32(CategoryId.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "UpdateCategoryStatusOnChange", path);
        }

    }

    protected void gridlist_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlStatus = (e.Item.FindControl("ddlStatus") as DropDownList);
                string Active = (e.Item.FindControl("Active") as Label).Text;
                ddlStatus.Items.FindByValue(Active).Selected = true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gridlist_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gvData_NeedDataSource", path);
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
                string CategoryId = item["CategoryId"].Text;
                string CategoryName = item["CategoryName"].Text;
                string catcode = item["CategoryCode"].Text;
                string Active = item["Status"].Text;

                hidcatcode.Value = catcode;
                hdncatidId.Value = CategoryId;
                txtFullName.Text = CategoryName;
                hdnOldCategory.Value = CategoryName;
                chkActive.Checked = Active == "Active" ? true : false;
                chkActive.Enabled = true;

                //chkActive.Enabled = true;
                //chkActive.Enabled = false;

                btnsubmit.Text = "Update";



                //Label CategoryName = e.Item.Cells[0].FindControl("CategoryName") as Label;
                //Label CategoryId = e.Item.Cells[0].FindControl("CategoryId") as Label;
                //Label Active = e.Item.Cells[0].FindControl("Active") as Label;
                //Label catcode = e.Item.Cells[0].FindControl("catcode") as Label;
                //hidcatcode.Value = catcode.Text;
                //hdncatidId.Value = CategoryId.Text;
                //txtFullName.Text = CategoryName.Text;
                //hdnOldCategory.Value = CategoryName.Text;
                //btnsubmit.Text = "Update";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "GvData_ItemCommand", path);

        }
    }

    public void btnOk_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    //Added by ponraj
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["CategoryCode"].Text = Category.ToUpper() + " ID";
                item["CategoryName"].Text = Category.ToUpper() + " NAME";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["CategoryCode"].Text = Category.ToUpper() + " ID";
                item["CategoryName"].Text = Category.ToUpper() + " NAME";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CategoryMaster.aspx", "gvData_ItemCreated", path);
        }
    }
}