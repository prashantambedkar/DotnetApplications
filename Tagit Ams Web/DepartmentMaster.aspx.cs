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

public partial class DepartmentMaster : System.Web.UI.Page
{
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
    public DataTable dt_Department
    {
        get
        {
            return ViewState["dt_Department"] as DataTable;
        }
        set
        {
            ViewState["dt_Department"] = value;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "gvData_PageIndexChanged", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "gvData_Init", path);
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
                chkActive.Enabled = false;
                chkActive.Checked = true;
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                divSearch.Style.Add("display", "none");
                if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                {
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "Page_Load", path);
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

            btnsubmit.Text = "Submit";
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "btnreset_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "btnSearchInfo_Click", path);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text == "Submit")
            {
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.Pinsertdepartment, new SqlParameter[]
                        {
                     new SqlParameter("@DepartmentName", txtFullName.Text.Trim()),
                     new SqlParameter("@Active", 1 ),
                     new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                     new SqlParameter("@CreatedDate", System.DateTime.Now),


                        }
                        );

                if (help.ExecuteNonQuery() < 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);

                    string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    imgpopup.ImageUrl = "images/Success.png";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                }
                grid_view();
                btnreset_Click(sender, e);

            }
            else if (btnsubmit.Text == "Update")
            {
                if (hdnOldDept.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    DepartmentBL objDet = new DepartmentBL();
                    if (chkActive.Checked == false)
                    {
                        bool DepartmentActive = objDet.CheckDepartmentAssociatedwithAnyCustodian(hdncatidId.Value);
                        if (DepartmentActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" is already mapped with asset,\\n you can’t deactivate this Department');", true);

                            string Message = "The department  \"" + txtFullName.Text.Trim() + "\" is already mapped with asset,\\n you can’t deactivate this Department.";
                            imgpopup.ImageUrl = "images/info.jpg";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            var result = objDet.UpdateDepartmentIdStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);

                                string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                string Message = "The department \"" + txtFullName.Text.Trim() + "\" was not Updated.";
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
                        var result = objDet.UpdateDepartmentIdStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                            string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            imgpopup.ImageUrl = "images/Success.png";
                            lblpopupmsg.Text = Message;
                            trheader.BgColor = "#98CODA";
                            trfooter.BgColor = "#98CODA";
                            ModalPopupExtender2.Show();
                        }
                        else
                        {
                            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                            string Message = "The department \"" + txtFullName.Text.Trim() + "\" was not updated.";
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
                    DepartmentBL objDept = new DepartmentBL();
                    bool DepartmentExist = objDept.checkDepartmentExists(txtFullName.Text.Trim());
                    //bool DepartmentExist = CheckDepartmentBelongToCustodian(txtFullName.Text.Trim());
                    if (DepartmentExist == true)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" is already exists.');", true);


                        string Message = "The department  \"" + txtFullName.Text.Trim() + "\" is already exists.";
                        imgpopup.ImageUrl = "images/info.jpg";
                        lblpopupmsg.Text = Message;
                        trheader.BgColor = "#98CODA";
                        trfooter.BgColor = "#98CODA";
                        ModalPopupExtender2.Show();
                    }
                    else
                    {

                        DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.pupdatedepartment, new SqlParameter[] {
                     new SqlParameter("@DepartmentId", Convert.ToInt32(hdncatidId.Value)),
                     new SqlParameter("@DepartmentName", txtFullName.Text.Trim()),
                     new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                     new SqlParameter("@CreatedDate", System.DateTime.Now),

                }
                          );

                        DepartmentBL objDet = new DepartmentBL();
                        if (chkActive.Checked == false)
                        {
                            bool DepartmentActive = objDet.CheckDepartmentAssociatedwithAnyCustodian(hdncatidId.Value);
                            if (DepartmentActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" is already mapped with any Custodian,\\n you can’t deactivate this Department');", true);


                                string Message = "The department  \"" + txtFullName.Text.Trim() + "\" is already mapped with any Custodian,\\n you can’t deactivate this Department.";
                                imgpopup.ImageUrl = "images/info.jpg";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                var result = objDet.UpdateDepartmentIdStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                    imgpopup.ImageUrl = "images/Success.png";
                                    lblpopupmsg.Text = Message;
                                    trheader.BgColor = "#98CODA";
                                    trfooter.BgColor = "#98CODA";
                                    ModalPopupExtender2.Show();
                                }
                                else
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Not Updated.');", true);

                                    string Message = "The department \"" + txtFullName.Text.Trim() + "\" was not updated.";
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
                            var result = objDet.UpdateDepartmentIdStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                imgpopup.ImageUrl = "images/Success.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                string Message = "The department \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                imgpopup.ImageUrl = "images/CloseRed.png";
                                lblpopupmsg.Text = Message;
                                trheader.BgColor = "#98CODA";
                                trfooter.BgColor = "#98CODA";
                                ModalPopupExtender2.Show();
                            }
                        }

                        if (help.ExecuteNonQuery() <= 1)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The department  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                            string Message = "The department  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
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
            hdnOldDept.Value = "";
            //grid_view();
            btnreset_Click(sender, e);
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "btnsubmit_Click", path);
            string Message = ex.Message.ToString();
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

            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetDepartmentDetails");
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetDepartmentDetails(SearchText);

            this.dt_Department = ds.Tables[0];
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "grid_view", path);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToString().ToLower());

            var DeptDetails = dt_Department.AsEnumerable().Where(c => c.Field<string>("DepartmentName").ToLower().Contains(text));
            if (DeptDetails.Count() > 0)
            {
                DataTable dtDeptDetails = DeptDetails.CopyToDataTable<DataRow>();

                gvData.DataSource = dtDeptDetails;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "btnSearch_Click", path);
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
            Label DepartmentName = e.Item.Cells[0].FindControl("DepartmentName") as Label;
            Label DepartmentId = e.Item.Cells[0].FindControl("DepartmentId") as Label;
            hdncatidId.Value = DepartmentId.Text;
            txtFullName.Text = DepartmentName.Text;
            hdnOldDept.Value = DepartmentName.Text;
            btnsubmit.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
            Logging.WriteErrorLog(Ex.Message.ToString(), Ex.StackTrace.ToString(), "DepartmentMaster.aspx", "EditDataGrid", path);
        }
    }

    private bool CheckDepartmentBelongToCustodian(string DeapatmentName)
    {
        //DataAccessHelper1 help = new DataAccessHelper1(

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pdepartmentbelongtocustodian", new SqlParameter[] {
                        new SqlParameter("@DepartmentName", DeapatmentName),
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
    protected void UpdateDepartmentStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            DepartmentBL objDept = new DepartmentBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label DepartmentId = (Label)item.FindControl("DepartmentId");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool DepartmentActive = objDept.CheckDepartmentAssociatedwithAnyCustodian(DepartmentId.Text);
            if (DepartmentActive == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This Department already mapped with any Custodian,\\n you can’t deactivate this Department');", true);
            }
            else
            {

                objDept.UpdateDepartmentIdStatus(Convert.ToInt32(DepartmentId.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "UpdateDepartmentStatusOnChange", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "gridlist_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "gvData_NeedDataSource", path);
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
                string DepartmentName = item["DepartmentName"].Text;
                string DepartmentId = item["DepartmentId"].Text;
                string Active = item["Status"].Text;

                hdncatidId.Value = DepartmentId;
                txtFullName.Text = DepartmentName;
                hdnOldDept.Value = DepartmentName;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";
                chkActive.Enabled = true;

                //Label DepartmentName = e.Item.Cells[0].FindControl("DepartmentName") as Label;
                //Label DepartmentId = e.Item.Cells[0].FindControl("DepartmentId") as Label;
                //hdncatidId.Value = DepartmentId.Text;
                //txtFullName.Text = DepartmentName.Text;
                //hdnOldDept.Value = DepartmentName.Text;
                //btnsubmit.Text = "Update";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DepartmentMaster.aspx", "GvData_ItemCommand", path);
        }
    }
}