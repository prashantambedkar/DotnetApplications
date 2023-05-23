using AjaxControlToolkit;
using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TagitEncrypt;
using Telerik.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    static String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    SqlConnection conn = new SqlConnection(strConnString);
    public static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                    HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                    HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                    HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                    if (userAuthorize((int)pages.DocumentRequestt, Session["userid"].ToString()))
                    {
                        Page.DataBind();
                        CompanyImg.Src = "images/" + _Logo;
                        fillDropDowns();
                        // divRemark.Visible = divName.Visible = false;
                        // txtRemark.ReadOnly = txtName.ReadOnly = true;
                        gvData.Visible = container3.Visible = btnSubmit.Visible = false;
                        delOldData();
                        txtQty.Text = "1";
                        //AssigneeNameDiv.Visible = false;
                        //OrganisationNameDiv.Visible = false;
                        //PersonNameDiv.Visible = false;
                        //txtAssigneeName.Visible = false;
                        //ddlAssigneeName.Visible = false;
                        //txtOrganisationName.Visible = false;
                        //ddlApplicantNames.Visible = false;
                    }
                    else
                    {
                        //ModalPopupExtender1.Show();
                        Response.Redirect("AcceessError.aspx");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "Page_Load", path);

        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    public void delOldData()
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("truncate table tblDocumentRequestDetails", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "delOldData", path);
        }
    }
    public void fillDropDowns()
    {
        try
        {
            fillddlCustodian();
            fillddlLocation();
            fillddlCaseId();
            // fillddlDocumentControllerUSER_ID();
            fillddlCategory();

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillDropDowns", path);
        }
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //del
        try
        {
            if (e.CommandName == "del")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                int ReqHdrId = Convert.ToInt32(item["ReqHdrId"].Text);
                Session["itemidd"] = id.ToString();
                Session["itemReqHdrId"] = ReqHdrId.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);


            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "gv_data_ItemCommand", path);
        }
    }
    public void fillddlCustodian()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataTable dtCustodian = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select cm.CustodianId ,cm.CustodianName from CustodianMaster as cm left join CustodianPermission as cp on cp.CustodianId=cm.CustodianId where cp.UserID=@UserID and cm.Active=1", con))
            {
                cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtCustodian);
                }
            }
            if (dtCustodian.Rows.Count > 0)
            {
                ddlCustodian.DataSource = dtCustodian;
                ddlCustodian.DataTextField = "CustodianName";
                ddlCustodian.DataValueField = "CustodianId";
                ddlCustodian.DataBind();
                ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCustodian.DataSource = null;
                ddlCustodian.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlCustodian", path);
        }
    }
    public void fillddlLocation()
    {
        try
        {
            DataTable dtLocation = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getUserSpecificLocation", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtLocation);
                }
            }
            if (dtLocation.Rows.Count > 0)
            {
                ddlLocation.DataSource = dtLocation;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationId";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlLocation", path);
        }
    }
    public void fillddlCaseId()
    {
        try
        {
            DataTable dtCaseId = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getCaseID", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtCaseId);
                }
            }
            if (dtCaseId.Rows.Count > 0)
            {
                ddlCaseId.DataSource = dtCaseId;
                ddlCaseId.DataTextField = "CaseID";
                ddlCaseId.DataValueField = "CaseID";
                ddlCaseId.DataBind();
                ddlCaseId.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCaseId.DataSource = null;
                ddlCaseId.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlCaseId", path);
        }

    }
    //public void fillddlDocumentControllerUSER_ID()
    //{
    //    try
    //    {
    //        DataTable dtDocumentControllerUSER_ID = new DataTable();
    //        using (SqlCommand cmd = new SqlCommand("getDocumentController", conn))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
    //            {
    //                adp.Fill(dtDocumentControllerUSER_ID);
    //            }
    //        }
    //        if (dtDocumentControllerUSER_ID.Rows.Count > 0)
    //        {
    //            ddlDocumentControllerUSER_ID.DataSource = dtDocumentControllerUSER_ID;
    //            ddlDocumentControllerUSER_ID.DataTextField = "USER_NAME";
    //            ddlDocumentControllerUSER_ID.DataValueField = "USER_ID";
    //            ddlDocumentControllerUSER_ID.DataBind();
    //            ddlDocumentControllerUSER_ID.Items.Insert(0, new ListItem("--Select--", "0", true));
    //        }
    //        else
    //        {
    //            ddlDocumentControllerUSER_ID.DataSource = null;
    //            ddlDocumentControllerUSER_ID.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlDocumentControllerUSER_ID", path);
    //    }
    //}
    public void fillddlCategory()
    {
        try
        {
            DataTable dtddlCategory = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getCategory", conn))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtddlCategory);
                }
            }
            if (dtddlCategory.Rows.Count > 0)
            {
                ddlCategory.DataSource = dtddlCategory;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryId";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCategory.DataSource = null;
                ddlCategory.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlCategory", path);
        }
    }
    protected void ddlCaseId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCaseId.SelectedValue != "0")
            {
                //fillAssigneeName(Convert.ToInt32(ddlCaseId.SelectedValue));
                //fillOrganisationName(Convert.ToInt32(ddlCaseId.SelectedValue));
                fillddlCasePersonAssociation(Convert.ToInt32(ddlCaseId.SelectedValue));
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "ddlCaseId_SelectedIndexChanged", path);
        }
    }
    public void fillddlCasePersonAssociation(int CaseID)
    {
        try
        {
            DataTable dtddlCasePersonAssociation = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select distinct CasePersonAssociation from ExcelDataTemp where CaseID=@CaseID", conn))
            {
                // cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CaseID", CaseID);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtddlCasePersonAssociation);
                }
            }
            if (dtddlCasePersonAssociation.Rows.Count > 0)
            {
                ddlCasePersonAssociation.DataSource = dtddlCasePersonAssociation;
                ddlCasePersonAssociation.DataTextField = "CasePersonAssociation";
                ddlCasePersonAssociation.DataValueField = "CasePersonAssociation";
                ddlCasePersonAssociation.DataBind();
                ddlCasePersonAssociation.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCasePersonAssociation.DataSource = null;
                ddlCasePersonAssociation.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillddlCasePersonAssociation", path);
        }
    }
    public void fillAssigneeName(string CasePersonAssociation, int CaseId)
    {
        try
        {
            DataTable dtAssigneeName = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getAssigneeFullName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CasePersonAssociation", CasePersonAssociation);
                cmd.Parameters.AddWithValue("@CaseID", CaseId);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtAssigneeName);
                }
            }
            if (dtAssigneeName.Rows.Count > 0)
            {

                ddlAssigneeName.DataSource = dtAssigneeName;
                ddlAssigneeName.DataTextField = "CaseAssigneeFullName";
                ddlAssigneeName.DataValueField = "CaseAssigneeFullName";
                ddlAssigneeName.DataBind();
                ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));

            }
            else
            {
                ddlAssigneeName.DataSource = null;
                ddlAssigneeName.DataBind();
                //txtAssigneeName.Text = "";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillAssigneeName", path);
        }
    }
    public void fillOrganisationName(string CasePersonAssociation, int CaseId)
    {
        try
        {
            DataTable dtOrganisationName = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getOrganizationFullName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CasePersonAssociation", CasePersonAssociation);
                cmd.Parameters.AddWithValue("@CaseID", CaseId);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtOrganisationName);
                }
            }
            if (dtOrganisationName.Rows.Count > 0)
            {
                txtOrganisationName.Text = dtOrganisationName.Rows[0]["OrganizationFullName"].ToString();
            }
            else
            {
                txtOrganisationName.Text = "";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "fillOrganisationName", path);
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustodian.SelectedValue == "0")
            {
                //string Message = "Select Custodian";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender1.Show();
                ddlCustodian.Focus();
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Custodian')", true);
                return;
            }
            if (ddlLocation.SelectedValue == "0")
            {
                //string Message = "Select Location";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender1.Show();
                ddlLocation.Focus();
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Location')", true);
                return;
            }
            if (ddlCaseId.SelectedValue == "0")
            {
                //string Message = "Select Case ID";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender1.Show();
                ddlCaseId.Focus();
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Case ID')", true);
                return;
            }
            if (ddlCasePersonAssociation.SelectedValue == "0")
            {
                //string Message = "Select Case Association";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender1.Show();
                ddlCasePersonAssociation.Focus();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Custodian')", true);
                return;
            }
            if (ddlAssigneeName.SelectedValue == "0")
            {
                ddlAssigneeName.Focus();
                return;

            }

            //if (ddlDocumentControllerUSER_ID.SelectedValue == "0")
            //{
            //    //string Message = "Select Document Controller";
            //    //imgpopup.ImageUrl = "images/info.jpg";
            //    //lblpopupmsg.Text = Message;
            //    //trheader.BgColor = "#98CODA";
            //    //trfooter.BgColor = "#98CODA";
            //    //ModalPopupExtender1.Show();
            //    ddlDocumentControllerUSER_ID.Focus();
            //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Document Controller')", true);
            //    return;
            //}
            if (ddlCategory.SelectedValue == "0")
            {
                //string Message = "Select Category";
                //imgpopup.ImageUrl = "images/info.jpg";
                //lblpopupmsg.Text = Message;
                //trheader.BgColor = "#98CODA";
                //trfooter.BgColor = "#98CODA";
                //ModalPopupExtender1.Show();
                ddlCategory.Focus();
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Category')", true);
                return;
            }
            if (ddlCustodian.SelectedValue != "0" && ddlCasePersonAssociation.SelectedValue != "0" && ddlLocation.SelectedValue != "0" && ddlCaseId.SelectedValue != "0" && ddlCategory.SelectedValue != "0")
            {
                int result = CheckData();
                if (result == 1 && txtQty.Text.Trim().Length > 0)
                {
                    DataTable dttblDocumentRequestHdr = new DataTable();
                    DataTable tdttblDocumentRequestHdr = new DataTable();
                    dttblDocumentRequestHdr = checktblDocumentRequestHdr();
                    ddlCustodian.Enabled = ddlLocation.Enabled = ddlCaseId.Enabled = false;
                    tdttblDocumentRequestHdr = checktblDocumentRequestDetails(dttblDocumentRequestHdr);
                    grid_view(tdttblDocumentRequestHdr);
                    if (tdttblDocumentRequestHdr.Rows.Count > 0)
                    {
                        btnSubmit.Visible = true;
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                    }
                    ddlCategory.SelectedValue = "0";
                    txtQty.Text = "1";
                    txtRemark.Text = "";
                }
                else if (result == 2)
                {
                    txtName.Focus();

                }
                else if (result == 3)
                {
                    txtRemark.Focus();
                }
                if (txtQty.Text.Trim().Length == 0)
                {
                    txtQty.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "btnadd_Click", path);
        }
    }
    public int CheckData()
    {
        try
        {

            string Category = ddlCategory.SelectedItem.ToString();
            if (Category == "PASSPORT DEPENDENT" || Category == "BIRTH CERTIFICATE")
            {
                if (txtName.Text.Trim().Length == 0)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else if (Category.Contains("OTHER"))
            {
                if (txtRemark.Text.Trim().Length == 0)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "CheckData", path);
            return 4;
        }

    }
    public void grid_view(DataTable tdttblDocumentRequestHdr)
    {
        try
        {
            gvData.Visible = true;
            DataView myView = null;
            myView = tdttblDocumentRequestHdr.DefaultView;
            gvData.DataSource = myView;
            gvData.DataBind();
            container3.Visible = true;
            btnSubmit.Visible = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "grid_view", path);
        }
    }
    public DataTable checktblDocumentRequestHdr()
    {
        try
        {
            DataTable dttblDocumentRequestHdr = new DataTable();
            using (SqlCommand cmd = new SqlCommand("checktblDocumentRequestHdr", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustodianId", ddlCustodian.SelectedValue);
                cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                cmd.Parameters.AddWithValue("@AssigneeName", ddlAssigneeName.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@CaseID", ddlCaseId.SelectedValue);
                cmd.Parameters.AddWithValue("@OrganisationName", txtOrganisationName.Text);

                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userid"]);
                //cmd.Parameters.AddWithValue("@excelid", txtexcelid.Text);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dttblDocumentRequestHdr);
                }
            }
            if (dttblDocumentRequestHdr.Rows.Count > 0)
            {
                return dttblDocumentRequestHdr;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "checktblDocumentRequestHdr", path);
            return null;
        }
    }

    public DataTable checktblDocumentRequestDetails(DataTable dttblDocumentRequestHdr)
    {
        try
        {
            DataTable tdttblDocumentRequestHdr = new DataTable();
            Session["ReqHdrIddformail"] = dttblDocumentRequestHdr.Rows[0]["id"].ToString();
            using (SqlCommand cmd = new SqlCommand("checktblDocumentRequestDetails", conn))
            {
                //@CasePersonAssociation nvarchar(500),
                //@ApplicantNames nvarchar(500)

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqHdrId", dttblDocumentRequestHdr.Rows[0]["id"]);
                cmd.Parameters.AddWithValue("@CategoryId", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Qty", txtQty.Text);
                cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userid"]);
                cmd.Parameters.AddWithValue("@excelid", dttblDocumentRequestHdr.Rows[0]["excelid"]);
                cmd.Parameters.AddWithValue("@CasePersonAssociation", ddlCasePersonAssociation.SelectedValue);
                cmd.Parameters.AddWithValue("@ApplicantNames", ddlAssigneeName.SelectedItem.ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(tdttblDocumentRequestHdr);
                }
            }
            if (tdttblDocumentRequestHdr.Rows.Count > 0)
            {
                return tdttblDocumentRequestHdr;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "checktblDocumentRequestDetails", path);
            return null;
        }
    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("insertDocumentRequestDetailsFInal", conn))
            {
                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                ddlCustodian.SelectedIndex = ddlLocation.SelectedIndex = ddlCaseId.SelectedIndex = ddlCasePersonAssociation.SelectedIndex = ddlAssigneeName.SelectedIndex = ddlCategory.SelectedIndex = 0;
                txtOrganisationName.Text = txtRemark.Text = txtName.Text = "";
                txtQty.Text = "1";
                delOldData();
                DataTable td = null;
                grid_view(dt);
                gvData.Visible = false;
                gvData.Visible = container3.Visible = btnSubmit.Visible = false;
                ddlCustodian.Enabled = ddlLocation.Enabled = ddlCaseId.Enabled = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal1();", true);
                //using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                //{
                //    adp.Fill(dt);
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    SendEmail(dt.Rows[0]["EmailID"].ToString(), dt.Rows[0]["USER_NAME"].ToString());
                //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Request Added!!')", true);
                //    //return;//
                //    ddlCustodian.SelectedIndex = ddlLocation.SelectedIndex = ddlCaseId.SelectedIndex = ddlCasePersonAssociation.SelectedIndex = ddlAssigneeName.SelectedIndex = ddlDocumentControllerUSER_ID.SelectedIndex = ddlCategory.SelectedIndex = 0;
                //    txtOrganisationName.Text = txtRemark.Text = txtName.Text = "";
                //    txtQty.Text = "1";
                //    delOldData();
                //    DataTable td = null;
                //    grid_view(dt);
                //    gvData.Visible = false;
                //    gvData.Visible = container3.Visible = btnSubmit.Visible = false;
                //    ddlCustodian.Enabled = ddlLocation.Enabled = ddlCaseId.Enabled = true;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal1();", true);
                //}

            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "btnSubmit_Click", path);
            //return null;
        }
        //Page_Load(sender, e);
        //Response.Redirect("DocumentRequestt.aspx");
        //Server.Transfer("DocumentRequestt.aspx");
    }
    public void SendEmail(string emailid, string username)
    {
        try
        {
            EncryptManager em = new EncryptManager();
            string WebApiUrlApprove = "";
            SqlCommand select = new SqlCommand();
            select.CommandText = "select * from TagitEmailConfig where Application='tagit'";
            select.CommandType = CommandType.Text;
            select.Connection = conn;
            SqlDataAdapter daEmail = new SqlDataAdapter(select);
            DataTable dt_MailConfig = new DataTable();
            daEmail.Fill(dt_MailConfig);
            System.Net.Mail.Attachment attachment;
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(dt_MailConfig.Rows[0][1].ToString());
            message.Subject = "TAGIT DMS DOCUMENT REQUEST DETAILS";
            message.To.Add(new MailAddress(emailid));
            message.IsBodyHtml = true;
            string bcc = "ragesh@tagitglobal.com";
            string[] bccid = bcc.Split(',');

            foreach (string bccEmailId in bccid)
            {
                message.Bcc.Add(new MailAddress(bccEmailId));
            }
            string MessageBody = "";
            message.Body = @"
                <html>
                    <head>    
                    <title> Tagit DMS </title>
                    <meta xmlns = ""http://www.w3.org/1999/xhtml"" content = ""text /html; charset=utf-8"" />
                    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
                    <!-- jQuery library -->
                    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js""></script>
                    <!-- Latest compiled JavaScript -->
                    <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
                    </head>      
                    <body>
                        <div id = ""stylized"" class=""myform"">
                            <form
                                id = ""form""
                                action = ""#""
                                name =""form"" >                                                                                                                           
                            </form>
                <table>
				    <tr>
				    <td align=""center"" colspan=""3"">                          
                    </td>
       
                    </tr>
       
                    <tr>
                        <td style=""width: 85%""><b>Dear " + username + @",	</b>
                        </td>
                        <td></td>
                        <td style=""width: 15%"" rowspan=""3"">                            
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan=""4"">
                            You have received a Document Request :- " + Environment.NewLine + @"Request ID :" + Session["ReqHdrIddformail"].ToString() + @". Kindly login to the application and check for Document Request Details.
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>&nbsp;</td>
                    </tr>  
                        <tr><td>** Please note that this email is auto-generated from Tagit DMS application.**</td><td></td></tr>
                       <tr>
                           <td>   
                               <br>   
                               <b> Thank You,</b><br>      
                               <br>
                                  Tagit Dms <br>      
                               <a href = ""https://www.tagitglobal.com/"" target =""_blank"" > www.tagitglobal.com</a>
                        </td>
                    </tr>
                </table>
                        </div>
                    </body>
                </html> ";


            smtp.Port = Convert.ToInt32(dt_MailConfig.Rows[0][4].ToString());
            smtp.Host = dt_MailConfig.Rows[0][3].ToString();
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(dt_MailConfig.Rows[0][1].ToString(), em.Decode(dt_MailConfig.Rows[0][2].ToString()));
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "SendEmail", path);
        }
    }

    protected void ddlCasePersonAssociation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCasePersonAssociation.SelectedValue != "0")
            {
                txtName.Text = "";
                fillAssigneeName(ddlCasePersonAssociation.SelectedValue, Convert.ToInt32(ddlCaseId.SelectedValue));
                fillOrganisationName(ddlCasePersonAssociation.SelectedValue, Convert.ToInt32(ddlCaseId.SelectedValue));
                //if (ddlCasePersonAssociation.SelectedItem.ToString() == "Case Assignee")
                //{
                //    //txtAssigneeName.Visible = true;
                //    ddlAssigneeName.Visible = true;
                //    //txtOrganisationName.Visible = true;
                //    //ddlApplicantNames.Visible = false;
                //    AssigneeNameDiv.Visible = true;
                //    //OrganisationNameDiv.Visible = true;
                //   // PersonNameDiv.Visible = false;

                //}
                //else if (ddlCasePersonAssociation.SelectedItem.ToString() == "Case Dependent")
                //{
                //    //txtAssigneeName.Visible = false;

                //    //ddlAssigneeName.Visible = false;
                //    //txtOrganisationName.Visible = false;
                //   // ddlApplicantNames.Visible = true;
                //    //AssigneeNameDiv.Visible = false;
                //    //OrganisationNameDiv.Visible = false;
                //    //PersonNameDiv.Visible = true;
                //    fillAssigneeName(ddlCasePersonAssociation.SelectedValue, Convert.ToInt32(ddlCaseId.SelectedValue));
                //    //fillddlApplicantNames(ddlCasePersonAssociation.SelectedValue, Convert.ToInt32(ddlCaseId.SelectedValue));
                //}
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "ddlCasePersonAssociation_SelectedIndexChanged", path);
        }
    }

    protected void ddlAssigneeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssigneeName.SelectedValue != "0")
            {
                txtName.Text = ddlAssigneeName.SelectedItem.ToString();
                txtexcelid.Text = ddlAssigneeName.SelectedValue;
                // txtexcelid.Text = dtAssigneeName.Rows[0]["id"].ToString();
                //txtName.Text = dtAssigneeName.Rows[0]["CaseAssigneeFullName"].ToString();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "DocumentRequestt.aspx", "ddlAssigneeName_SelectedIndexChanged", path);
        }
    }



    protected void btnupdates_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        using (SqlCommand cmd = new SqlCommand("deleteDocumentRequestDetails", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", Session["itemidd"]);
            cmd.Parameters.AddWithValue("@ReqHdrId", Session["itemReqHdrId"]);
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dt);
            }
        }
        if (dt.Rows.Count > 0)
        {
            grid_view(dt);
        }
        else
        {
            dt = null;
            grid_view(dt);
        }
    }
}