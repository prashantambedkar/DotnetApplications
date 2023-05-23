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

public partial class SuplierMaster : System.Web.UI.Page
{
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
    public DataTable dt_Supplier
    {
        get
        {
            return ViewState["dt_Supplier"] as DataTable;
        }
        set
        {
            ViewState["dt_Supplier"] = value;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "gvData_PageIndexChanged", path);

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

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "gvData_Init", path);
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
                divSearch.Style.Add("display", "none");
                chkActive.Enabled = false;
                chkActive.Checked = true;
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                {
                    grid_view();
                }
                else
                {
                    // ModalPopupExtender1.Show();
                    Response.Redirect("AcceessError.aspx");
                }
            }
        }
        catch (Exception ex)
        {

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "Page_Load", path);
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

    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            txtcnname.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphn.Text = string.Empty;
            txtadd.Text = string.Empty;
            txtrmk.Text = string.Empty;
            btnsubmit.Text = "Submit";
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "btnreset_Click", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "btnSearchInfo_Click", path);

        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text == "Submit")
            {
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.Pinsertsupplier, new SqlParameter[]
                        {
                            new SqlParameter("@SupplierName", txtFullName.Text.Trim()),
                             new SqlParameter("@ContactPersonName", txtcnname.Text.Trim()),
                             new SqlParameter("@EmailId", txtemail.Text.Trim()),
                                   new SqlParameter("@PhoneNo", txtphn.Text.Trim()),

                                   new SqlParameter("@Address", txtadd.Text.Trim()),
                                   new SqlParameter("@Remark", txtrmk.Text.Trim()),
                             new SqlParameter("@Active",1 ),
                               new SqlParameter("@UserId",Convert.ToInt32(Session["userid"])),


                        }
                        );

                if (help.ExecuteNonQuery() < 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);

                    string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
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
                if (hdnOldSuppName.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    DataAccessHelper1 help = new DataAccessHelper1(
                          StoredProcedures.pupdatesupplier, new SqlParameter[] {
                          new SqlParameter("@SupplierId", Convert.ToInt32(hdncatidId.Value)),
                            new SqlParameter("@SupplierName", txtFullName.Text.Trim()),
                             new SqlParameter("@ContactPersonName", txtcnname.Text.Trim()),
                             new SqlParameter("@EmailId", txtemail.Text.Trim()),
                                   new SqlParameter("@PhoneNo", txtphn.Text.Trim()),

                                   new SqlParameter("@Address", txtadd.Text.Trim()),
                                   new SqlParameter("@Remarks", txtrmk.Text.Trim()),


                }
                          );

                    SupplierBL objSupp = new SupplierBL();
                    if (chkActive.Checked == false)
                    {
                        bool SupplierActive = objSupp.CheckSupplierAssociatedwithAnyAsset(hdncatidId.Value);
                        if (SupplierActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" is already mapped with Asset,\\n you can’t deactivate this supplier');", true);

                            string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" is already mapped with Asset,\\n you can’t deactivate this supplier.";
                            imgpopup.ImageUrl = "images/info.jpg";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            var result = objSupp.UpdateSupplierStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                                string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                imgpopup.ImageUrl = "images/CloseRed.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#EA6658";
                                trfooter.BgColor = "#EA6658";
                                ModalPopupExtender2.Show();
                            }
                        }
                    }
                    else
                    {
                        var result = objSupp.UpdateSupplierStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            imgpopup.ImageUrl = "images/Success.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            imgpopup.ImageUrl = "images/CloseRed.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#EA6658";
                            trfooter.BgColor = "#EA6658";
                            ModalPopupExtender2.Show();
                        }
                    }

                    if (help.ExecuteNonQuery() <= 1)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                        string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                        imgpopup.ImageUrl = "images/Success.png";
                        lblpopupmsg.Text = Message;
                        trheader.BgColor = "#98CODA";
                        trfooter.BgColor = "#98CODA";
                        ModalPopupExtender2.Show();
                    }

                    btnsubmit.Text = "Add";
                }
                else
                {
                    SupplierBL objSup = new SupplierBL();
                    bool SupplierExist = objSup.checkSupplierExists(txtFullName.Text.Trim());
                    //bool DepartmentExist = CheckDepartmentBelongToCustodian(txtFullName.Text.Trim());
                    if (SupplierExist == true)
                    {
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" is already exists.');", true);


                        string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" is already exists.";
                        imgpopup.ImageUrl = "images/info.jpg";
                        lblpopupmsg.Text = Message;
                        trheader.BgColor = "#98CODA";
                        trfooter.BgColor = "#98CODA";
                        ModalPopupExtender2.Show();
                    }
                    else
                    {
                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.pupdatesupplier, new SqlParameter[] {
                          new SqlParameter("@SupplierId", Convert.ToInt32(hdncatidId.Value)),
                            new SqlParameter("@SupplierName", txtFullName.Text.Trim()),
                             new SqlParameter("@ContactPersonName", txtcnname.Text.Trim()),
                             new SqlParameter("@EmailId", txtemail.Text.Trim()),
                                   new SqlParameter("@PhoneNo", txtphn.Text.Trim()),

                                   new SqlParameter("@Address", txtadd.Text.Trim()),
                                   new SqlParameter("@Remarks", txtrmk.Text.Trim()),


                }
                           );

                        SupplierBL objSupp = new SupplierBL();
                        if (chkActive.Checked == false)
                        {
                            bool SupplierActive = objSupp.CheckSupplierAssociatedwithAnyAsset(hdncatidId.Value);
                            if (SupplierActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" is already mapped with any Asset,\\n you can’t deactivate this Custodian');", true);

                                string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" is already mapped with any Asset,\\n you can’t deactivate this Custodian.";
                                imgpopup.ImageUrl = "images/info.jpg";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                var result = objSupp.UpdateSupplierStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                                    string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                    imgpopup.ImageUrl = "images/Success.png";
                                    lblpopupmsg.Text = Message;
                                    trheader.BgColor = "#98CODA";
                                    trfooter.BgColor = "#98CODA";
                                    ModalPopupExtender2.Show();
                                }
                                else
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                    imgpopup.ImageUrl = "images/CloseRed.png";
                                    lblpopupmsg.Text = Message;
                                    trheader.BgColor = "#EA6658";
                                    trfooter.BgColor = "#EA6658";
                                    ModalPopupExtender2.Show();
                                }
                            }
                        }
                        else
                        {
                            var result = objSupp.UpdateSupplierStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                                string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                imgpopup.ImageUrl = "images/CloseRed.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#EA6658";
                                trfooter.BgColor = "#EA6658";
                                ModalPopupExtender2.Show();
                            }
                        }


                        if (help.ExecuteNonQuery() <= 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            string Message = "The supplier  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
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
            hdncatidId.Value = "";
            hdnOldSuppName.Value = "";
            // grid_view();
            btnreset_Click(sender, e);
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            string Message = ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "btnsubmit_Click", path);
            imgpopup.ImageUrl = "images/CloseRed.png";
            lblpopupmsg.Text = Message;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();
        }


    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            // DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetSupplierDetails");
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetSupplierDetails(SearchText);

            this.dt_Supplier = ds.Tables[0];
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                lblMessage.Text = "Problem occured while retrieving Department records. Please try again.";
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


                gvData.DataSource = myView;

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "grid_view", path);
        }
    }
    protected void gridlist_SortCommand(Object sender, DataGridSortCommandEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "gridlist_SortCommand", path);

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToString().ToLower());

            var SuppDetails = dt_Supplier.AsEnumerable().Where(c => c.Field<string>("SupplierName").ToLower().Contains(text));
            if (SuppDetails.Count() > 0)
            {
                DataTable dtSuppDetails = SuppDetails.CopyToDataTable<DataRow>();
                gvData.DataSource = dtSuppDetails;
                gvData.DataBind();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record Found.!!');", true);

            }
            txtSearch.Text = "";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "btnSearch_Click", path);

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

            Label SupplierName = e.Item.Cells[0].FindControl("SupplierName") as Label;
            Label ContactPersonName = e.Item.Cells[0].FindControl("ContactPersonName") as Label;

            Label Remarks = e.Item.Cells[0].FindControl("Remarks") as Label;
            Label Address = e.Item.Cells[0].FindControl("Address") as Label;
            Label PhoneNo = e.Item.Cells[0].FindControl("PhoneNo") as Label;
            Label SupplierId = e.Item.Cells[0].FindControl("SupplierId") as Label;
            Label EmailId = e.Item.Cells[0].FindControl("EmailId") as Label;
            Label Active = e.Item.Cells[0].FindControl("Active") as Label;
            Label supcode = e.Item.Cells[0].FindControl("supcode") as Label;
            hidcatcode.Value = supcode.Text;
            hdncatidId.Value = SupplierId.Text;
            txtFullName.Text = SupplierName.Text;
            hdnOldSuppName.Value = SupplierName.Text;
            txtphn.Text = PhoneNo.Text;
            txtemail.Text = EmailId.Text;
            txtcnname.Text = ContactPersonName.Text;
            txtrmk.Text = Remarks.Text;
            txtadd.Text = Address.Text;

            btnsubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "EditDataGrid", path);
        }
    }
    private bool CheckSupplierBelongToAsset(string SupplierName)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pSupplierBelongToAsset", new SqlParameter[] {
                        new SqlParameter("@SupplierName", SupplierName),
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
    protected void UpdateSupplierStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            SupplierBL objSupp = new SupplierBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label SupplierId = (Label)item.FindControl("SupplierId");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool SupplierActive = objSupp.CheckSupplierAssociatedwithAnyAsset(SupplierId.Text);
            if (SupplierActive == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This Custodian already mapped with any Asset,\\n you can’t deactivate this Custodian');", true);
            }
            else
            {

                objSupp.UpdateSupplierStatus(Convert.ToInt32(SupplierId.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "UpdateSupplierStatusOnChange", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "gridlist_ItemDataBound", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "gvData_NeedDataSource", path);

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
                string SupplierName = item["SupplierName"].Text;
                string ContactPersonName = item["ContactPersonName"].Text;
                string Remarks = item["Remarks"].Text == "&nbsp;" ? "" : item["Remarks"].Text;
                string Address = item["Address"].Text == "&nbsp;" ? "" : item["Address"].Text;
                string PhoneNo = item["PhoneNo"].Text;
                string SupplierId = item["SupplierId"].Text;
                string EmailId = item["EmailId"].Text;
                string Active = item["Status"].Text;
                string supcode = item["SupplierCode"].Text;


                hidcatcode.Value = supcode;
                hdncatidId.Value = SupplierId;
                txtFullName.Text = SupplierName;
                hdnOldSuppName.Value = SupplierName;
                txtphn.Text = PhoneNo;
                txtemail.Text = EmailId;
                txtcnname.Text = ContactPersonName;
                txtrmk.Text = Remarks;
                txtadd.Text = Address;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";

                chkActive.Enabled = true;
                //chkActive.Checked = true;

                //Label SupplierName = e.Item.Cells[0].FindControl("SupplierName") as Label;
                //Label ContactPersonName = e.Item.Cells[0].FindControl("ContactPersonName") as Label;

                //Label Remarks = e.Item.Cells[0].FindControl("Remarks") as Label;
                //Label Address = e.Item.Cells[0].FindControl("Address") as Label;
                //Label PhoneNo = e.Item.Cells[0].FindControl("PhoneNo") as Label;
                //Label SupplierId = e.Item.Cells[0].FindControl("SupplierId") as Label;
                //Label EmailId = e.Item.Cells[0].FindControl("EmailId") as Label;
                //Label Active = e.Item.Cells[0].FindControl("Active") as Label;
                //Label supcode = e.Item.Cells[0].FindControl("supcode") as Label;
                //hidcatcode.Value = supcode.Text;
                //hdncatidId.Value = SupplierId.Text;
                //txtFullName.Text = SupplierName.Text;
                //hdnOldSuppName.Value = SupplierName.Text;
                //txtphn.Text = PhoneNo.Text;
                //txtemail.Text = EmailId.Text;
                //txtcnname.Text = ContactPersonName.Text;
                //txtrmk.Text = Remarks.Text;
                //txtadd.Text = Address.Text;

                //btnsubmit.Text = "Update";


            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SuplierMaster.aspx", "GvData_ItemCommand", path);

        }
    }

}