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

public partial class CustodianMaster : System.Web.UI.Page
{
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    bool setsearch = false;
    public static string path = "";
    public DataTable dt_Custodian
    {
        get
        {
            return ViewState["dt_Custodian"] as DataTable;
        }
        set
        {
            ViewState["dt_Custodian"] = value;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "gvData_PageIndexChanged", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "gvData_Init", path);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
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
                divSearch.Style.Add("display", "none");
                chkActive.Enabled = false;
                chkActive.Checked = true;
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                {
                    // BindDepartment();
                    grid_view();
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "Page_Load", path);
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
    //private void BindDepartment()
    //{
    //    try
    //    {
    //        SqlConnection con = new SqlConnection();
    //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    //        SqlDataAdapter dpt = new SqlDataAdapter();


    //        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

    //        ddldepartment.DataSource = ds;
    //        ddldepartment.DataTextField = "DepartmentName";
    //        ddldepartment.DataValueField = "DepartmentId";
    //        ddldepartment.DataBind();
    //        ddldepartment.Items.Insert(0, new ListItem("--Select--", "0", true));


    //        //Added By Ponraj
    //        ddlCustDepartment.DataSource = ds;
    //        ddlCustDepartment.DataTextField = "DepartmentName";
    //        ddlCustDepartment.DataValueField = "DepartmentId";
    //        ddlCustDepartment.DataBind();
    //        ddlCustDepartment.Items.Insert(0, new ListItem("--Select--", "0", true));

    //        string ClientCode = (SqlHelper.ExecuteScalar(con, CommandType.Text, "select Max(Prefix) from [Label_Config]")).ToString();
    //        hdnClientCode.Value = ClientCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "BindDepartment", path);
    //    }
    //}
    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            txtdesg.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphn.Text = string.Empty;
            // ddldepartment.SelectedIndex = 0;
            btnsubmit.Text = "Submit";
            setsearch = false;
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "btnreset_Click", path);
        }
    }
    public void InsertCustodianPermission(int UserID, int CustodianId, int CreatedBY)
    {
        try
        {
            //UserID, LocationID, IsPermission, CreatedBY, CreatedDate
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("sp_insertCustodianPermission", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                cmd.Parameters.AddWithValue("@CreatedBY", CreatedBY);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "InsertCustodianPermission", path);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if(txtFullName.Text.Trim().Length==0)
            {
                txtFullName.Focus();
                return;
            }
            if(txtdesg.Text.Trim().Length==0)
            {
                txtdesg.Focus();
                return;
            }
            if(txtphn.Text.Trim().Length==0)
            {
                txtphn.Focus();
                return;
            }
            if(txtemail.Text.Trim().Length==0)
            {
                txtemail.Focus();
                return;
            }
            if (btnsubmit.Text.ToLower() == "submit")
            {
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.Pinsertcustodian, new SqlParameter[]
                        {
                            new SqlParameter("@CustodianName", txtFullName.Text.Trim()),
                             new SqlParameter("@Designation", txtdesg.Text.Trim()),
                              new SqlParameter("@Phone", txtphn.Text.Trim()),
                              new SqlParameter("@EmailId", txtemail.Text.Trim()),
                               new SqlParameter("@Active", 1),

                      new SqlParameter("@DepartmentId", 0),
                        new SqlParameter("@UserId", Convert.ToInt32(Session["userid"])),

                        }
                        );

                if (help.ExecuteNonQuery() < 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);
                    DataTable dtcust = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                    using (SqlCommand cmd = new SqlCommand("select CustodianId from CustodianMaster where CustodianName=@CustodianName and Designation=@Designation and Phone=@Phone and EmailId=@EmailId ", con))
                    {
                        cmd.Parameters.AddWithValue("@CustodianName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Designation", txtdesg.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtphn.Text.Trim());
                        cmd.Parameters.AddWithValue("@EmailId", txtemail.Text.Trim());
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(dtcust);
                        }
                    }
                    if (dtcust.Rows.Count > 0)
                    {
                        InsertCustodianPermission(Convert.ToInt32(Session["userid"]), Convert.ToInt32(dtcust.Rows[0]["CustodianId"]), Convert.ToInt32(Session["userid"]));
                    }

                    //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                //grid_view();
                //btnreset_Click(sender, e);

            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                if (hdnOldCust.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    DataAccessHelper1 help = new DataAccessHelper1(
                         StoredProcedures.pupdatecustodian, new SqlParameter[] {
                          new SqlParameter("@CustodianId", Convert.ToInt32(hdncatidId.Value)),
                          new SqlParameter("@CustodianName", txtFullName.Text.Trim()),
                          new SqlParameter("@Designation", txtdesg.Text.Trim()),
                          new SqlParameter("@Phone", txtphn.Text.Trim()),
                          new SqlParameter("@EmailId", txtemail.Text.Trim()),
                          new SqlParameter("@DepartmentId",0),
                }
                     );

                    CustodianBL objCust = new CustodianBL();
                    if (chkActive.Checked == false)
                    {
                        bool DepartmentActive = objCust.CheckCustomerAssociatedwithAnyAssets(hdncatidId.Value);
                        if (DepartmentActive == true)
                        {
                            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian');", true);

                            //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian.";
                            //imgpopup.ImageUrl = "images/info.jpg";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            var result = objCust.UpdateCustodianStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }

                        }
                    }
                    else
                    {
                        var result = objCust.UpdateCustodianStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                            //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                            //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            //imgpopup.ImageUrl = "images/CloseRed.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                    }

                    if (help.ExecuteNonQuery() <= 1)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was update successfully.');", true);

                        //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                        //imgpopup.ImageUrl = "images/Success.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }

                    btnsubmit.Text = "Add";
                }
                else
                {
                    CustodianBL objCust = new CustodianBL();
                    bool CustExist = objCust.CheckCustodianExists(txtFullName.Text.Trim());
                    if (CustExist == true)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already exists.');", true);

                        //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\"  is already exists.";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already exists.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {

                        DataAccessHelper1 help = new DataAccessHelper1(
                          StoredProcedures.pupdatecustodian, new SqlParameter[] {
                          new SqlParameter("@CustodianId", Convert.ToInt32(hdncatidId.Value)),
                          new SqlParameter("@CustodianName", txtFullName.Text.Trim()),
                          new SqlParameter("@Designation", txtdesg.Text.Trim()),
                          new SqlParameter("@Phone", txtphn.Text.Trim()),
                          new SqlParameter("@EmailId", txtemail.Text.Trim()),
                          new SqlParameter("@DepartmentId",0),
                }
                      );

                        CustodianBL objCus = new CustodianBL();
                        if (chkActive.Checked == false)
                        {
                            bool DepartmentActive = objCus.CheckCustomerAssociatedwithAnyAssets(hdncatidId.Value);
                            if (DepartmentActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian');", true);

                                //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian.";
                                //imgpopup.ImageUrl = "images/info.jpg";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\"  is already mapped with Assets,\\n you can’t deactivate this Custodian.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                var result = objCust.UpdateCustodianStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                    //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                                    //imgpopup.ImageUrl = "images/Success.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                                else
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                    //imgpopup.ImageUrl = "images/CloseRed.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                            }
                        }
                        else
                        {
                            var result = objCus.UpdateCustodianStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);

                                //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }

                        if (help.ExecuteNonQuery() <= 1)
                        {
                            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The custodian  \"" + txtFullName.Text.Trim() + "\" was update successfully.');", true);

                            //string Message = "The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian  \"" + txtFullName.Text.Trim() + "\" was successfully updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }

                        btnsubmit.Text = "Add";
                    }
                }
            }
            hdnOldCust.Value = "";
            hdncatidId.Value = "";
            setsearch = false;
            grid_view();
            btnreset_Click(sender, e);
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "btnsubmit_Click", path);
            //string Message = ex.Message.ToString();
            //imgpopup.ImageUrl = "images/CloseRed.png";
            //lblpopupmsg.Text = Message;
            //trheader.BgColor = "#98CODA";
            //trfooter.BgColor = "#98CODA";
            //ModalPopupExtender2.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + ex.ToString() + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "btnSearchInfo_Click", path);
        }
    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            lblMsg.Visible = false;
            lblMsg.Text = "";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetCustodianDetails");
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            string DepartmentId = (ddlCustDepartment.SelectedValue == "0") ? null : ddlCustDepartment.SelectedValue;
            string CustCode = (txtCustCode.Text.ToString() == "") ? null : txtCustCode.Text.ToString();
            string CustName = (txtCustName.Text.ToString() == "") ? null : txtCustName.Text.ToString();
            string Designation = (txtDesignation.Text.ToString() == "") ? null : txtDesignation.Text.ToString();
            string phoneno = (txtphoneno.Text.ToString() == "") ? null : txtphoneno.Text.ToString();
            string email = (txtEmailId.Text.ToString() == "") ? null : txtEmailId.Text.ToString();
            string Encoded = (chkEncoded.Checked == false) ? null : "1";

            DataSet ds = null;
            if (setsearch == true)
            {
                ds = Common.GetCustodianDetailsV2(SearchText, null, CustCode, CustName, Designation, phoneno, email, Encoded, Session["userid"].ToString());
            }
            else
            {
                ds = Common.GetCustodianDetailsV2(SearchText, null, CustCode, CustName, Designation, phoneno, email, null, Session["userid"].ToString());
            }

            this.dt_Custodian = ds.Tables[0];
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
                lblMessage.Text = "";

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "grid_view", path);
        }
    }

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{

    //    string text = Convert.ToString(txtSearch.Text.ToString().ToLower());

    //    var CustDetails = dt_Custodian.AsEnumerable().Where(c => c.Field<string>("CustodianName").ToLower().Contains(text));
    //    if (CustDetails.Count() > 0)
    //    {
    //        DataTable dtCustDetailsDetails = CustDetails.CopyToDataTable<DataRow>();
    //        gvData.DataSource = dtCustDetailsDetails;
    //        gvData.DataBind();
    //    }
    //    else
    //    {
    //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record Found.!!');", true);

    //    }
    //    txtSearch.Text = "";

    //}
    private void ShowSuccessMessage(string msg)
    {
        lblMessage.Text = msg;
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        try
        {

            Label CustodianName = e.Item.Cells[0].FindControl("CustodianName") as Label;
            Label Designation = e.Item.Cells[0].FindControl("Designation") as Label;
            Label Phone = e.Item.Cells[0].FindControl("Phone") as Label;
            Label CustodianId = e.Item.Cells[0].FindControl("CustodianId") as Label;
            Label EmailId = e.Item.Cells[0].FindControl("EmailId") as Label;
            Label custcode = e.Item.Cells[0].FindControl("custcode") as Label;
            HiddenField hiddeptid = e.Item.Cells[0].FindControl("hiddeptid") as HiddenField;
            hidcatcode.Value = custcode.Text;
            hdncatidId.Value = CustodianId.Text;
            txtFullName.Text = CustodianName.Text;
            hdnOldCust.Value = CustodianName.Text; ;
            txtphn.Text = Phone.Text;
            txtemail.Text = EmailId.Text;
            txtdesg.Text = Designation.Text;
            //ddldepartment.SelectedValue = hiddeptid.Value;
            btnsubmit.Text = "Update";
        }
        catch (Exception Ex)
        {
            Logging.WriteErrorLog(Ex.Message.ToString(), Ex.StackTrace.ToString(), "CustodianMaster.aspx", "EditDataGrid", path);
            lblMessage.Text = Ex.Message;
        }
    }
    private bool CheckCustodianBelongToAsset(string CustodianName)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pCustodianBelongToAsset", new SqlParameter[] {
                        new SqlParameter("@CustodianName", CustodianName),
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

    protected void UpdateCustodianStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            CustodianBL objCust = new CustodianBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label CustodianId = (Label)item.FindControl("CustodianId");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool DepartmentActive = objCust.CheckCustomerAssociatedwithAnyAssets(CustodianId.Text);
            if (DepartmentActive == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This Custodian is already mapped with Assets,\\n you can’t deactivate this Custodian');", true);
            }
            else
            {

                objCust.UpdateCustodianStatus(Convert.ToInt32(CustodianId.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "UpdateCustodianStatusOnChange", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "gridlist_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "gvData_NeedDataSource", path);
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
                string CustodianName = item["CustodianName"].Text;
                string Designation = item["Designation"].Text;
                string Phone = item["Phone"].Text;
                if (Phone == "&nbsp;")
                {
                    Phone = "";
                }
                string CustodianId = item["CustodianId"].Text;
                string EmailId = item["EmailId"].Text;
                if (EmailId == "&nbsp;")
                {
                    EmailId = "";
                }
                string custcode = item["custodiancode"].Text;
                string hiddeptid = item["DepartmentId"].Text;
                string Active = item["Status"].Text;

                hidcatcode.Value = custcode;
                hdncatidId.Value = CustodianId;
                txtFullName.Text = CustodianName;
                hdnOldCust.Value = CustodianName; ;
                txtphn.Text = Phone;
                txtemail.Text = EmailId;
                txtdesg.Text = Designation;
                // ddldepartment.SelectedValue = hiddeptid;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";
                chkActive.Enabled = true;


                //Label CustodianName = e.Item.Cells[0].FindControl("CustodianName") as Label;
                //Label Designation = e.Item.Cells[0].FindControl("Designation") as Label;
                //Label Phone = e.Item.Cells[0].FindControl("Phone") as Label;
                //Label CustodianId = e.Item.Cells[0].FindControl("CustodianId") as Label;
                //Label EmailId = e.Item.Cells[0].FindControl("EmailId") as Label;
                //Label custcode = e.Item.Cells[0].FindControl("custcode") as Label;
                //HiddenField hiddeptid = e.Item.Cells[0].FindControl("hiddeptid") as HiddenField;
                //hidcatcode.Value = custcode.Text;
                //hdncatidId.Value = CustodianId.Text;
                //txtFullName.Text = CustodianName.Text;
                //hdnOldCust.Value = CustodianName.Text; ;
                //txtphn.Text = Phone.Text;
                //txtemail.Text = EmailId.Text;
                //txtdesg.Text = Designation.Text;
                //ddldepartment.SelectedValue = hiddeptid.Value;
                //btnsubmit.Text = "Update";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "GvData_ItemCommand", path);
        }
    }
    //added by ponraj
    protected void btnEncode_Click(object sender, EventArgs e)
    {
        try
        {
            CustodianBL objCust = new CustodianBL();
            foreach (GridDataItem item in gvData.Items)
            {
                HiddenField hdnCustID = (HiddenField)item.Cells[1].FindControl("hdbCustId");
                CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("ChkEncode");
                if (chkitem.Checked == true)
                {
                    if ((objCust.UpdateCustodianEncoded(Convert.ToInt32(hdnCustID.Value), 1)) == true)
                    {
                        grid_view();
                        gvData.DataBind();
                        //string Message = "The custodian was successfully encoded.";
                        //imgpopup.ImageUrl = "images/Success.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian was successfully encoded.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        //string Message = "The custodian was not encoded.";
                        //imgpopup.ImageUrl = "images/CloseRed.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The custodian was not encoded.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "btnEncode_Click", path);
        }
    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCustDepartment.SelectedValue = "0";
            txtCustCode.Text = "";
            txtCustName.Text = "";
            txtDesignation.Text = "";
            chkEncoded.Checked = false;
            setsearch = false;
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "BtnClear_Click", path);
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            setsearch = true;
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "CustodianMaster.aspx", "BtnSearch_Click", path);
        }
    }

    //protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    //{

    //    if (e.Item is GridDataItem)
    //    {
    //        GridDataItem dataItem = e.Item as GridDataItem;
    //        CheckBox chkbox = dataItem.FindControl("ChkEncode") as CheckBox;
    //        if (dataItem["IsEncoded"].Text == "Yes")
    //        {
    //            //dataItem.Enabled = false;
    //            chkbox.Enabled = false;
    //        }
    //        else
    //        {
    //            chkbox.Enabled = true;
    //        }
    //    }
    //}

}