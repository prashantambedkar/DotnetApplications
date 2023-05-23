using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.DataAccess;
using Telerik.Web.UI;
using ECommerce.Common;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class TestMaster : System.Web.UI.Page
{
    public static string path = "";
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Po22p", "ChangeColor();", true);
        try
        {
            if (!IsPostBack)
            {
               // PopulateDropDownList();
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                Page.DataBind();
                if (Session["userid"] != null)
                {

                    if (userAuthorize((int)pages.UserManagement, Session["userid"].ToString()))
                    {
                        BindType();
                        BindLocation("");
                        BindCustodian();
                        grid_view();
                        BindPer("0");

                    }
                    else
                    {
                        //ModalPopupExtender1.Show();
                        Response.Redirect("AcceessError.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }


            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "Page_Load", path);


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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "gvData_Init", path);
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

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void BindType()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            string query = "select * from [dbo].[UserType] ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            ddtype.DataSource = ds;
            ddtype.DataTextField = "Type";
            ddtype.DataValueField = "id";
            ddtype.DataBind();
            ddtype.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "BindType", path);
        }
    }
    private void BindLocation(string SearchText)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            //string query = "select * from [dbo].[UserType] ";
            //SqlCommand cmd = new SqlCommand(query, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("GetLocationDetails", con))
            {
                cmdz.CommandType = CommandType.StoredProcedure;
                cmdz.Parameters.AddWithValue("@SearchText", SearchText);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ddllocation.DataSource = dt;
                ddllocation.DataTextField = "LocationName";
                ddllocation.DataValueField = "LocationId";
                ddllocation.DataBind();
                rptPerson.DataSource = dt;
                rptPerson.DataBind();
                for (int i = 0; i <= ddllocation.Items.Count - 1; i++)
                {
                    if (dt.Rows[i]["LocationName"].ToString() == ddllocation.Items[i].Text.ToString())
                    {
                        if (dt.Rows[i]["Status"].ToString() == "Active")
                        {
                            ddllocation.Items[i].Visible = true;
                        }
                        else
                        {
                            ddllocation.Items[i].Visible = false;
                        }
                    }
                    // AddCheckboxes(dt.Rows[i]["LocationName"].ToString(), dt.Rows[i]["LocationId"].ToString(), ddllocation.Items.Count);
                }

                //ListBox1.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(ListBox1, "event 1"));
                //ddllocation.Items.Insert(0, new ListItem("--Select--", "0", true));

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "BindLocation", path);
        }
    }
    CheckBox chkList1;




    private void BindCustodian()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            //string query = "select * from [dbo].[UserType] ";
            //SqlCommand cmd = new SqlCommand(query, con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("getCustodian", con))
            {
                cmdz.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ddlCustodian.DataSource = dt;
                ddlCustodian.DataTextField = "CustodianName";
                ddlCustodian.DataValueField = "CustodianId";
                ddlCustodian.DataBind();
                custodianrepeater.DataSource = dt;
                custodianrepeater.DataBind();
                //ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "BindCustodian", path);
        }
    }
    private void fetchLocation(string UserID)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataTable dtpermissionDetails = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("fetchUserLocationPermissionDetails", con))
            {
                cmdz.CommandType = CommandType.StoredProcedure;
                cmdz.Parameters.AddWithValue("@UserID", UserID);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dtpermissionDetails);
                }
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("select * from LocationMaster where Active=1 order by LocationId asc", con))
            {
                //    cmdz.CommandType = CommandType.StoredProcedure;
                //    cmdz.Parameters.AddWithValue("@SearchText", "");
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ddllocation.DataSource = dt;
                ddllocation.DataTextField = "LocationName";
                ddllocation.DataValueField = "LocationId";
                ddllocation.DataBind();
                for (int i = 0; i <= ddllocation.Items.Count - 1; i++)
                {
                    if (dt.Rows[i]["LocationName"].ToString() == ddllocation.Items[i].Text.ToString())
                    {
                        //if (dt.Rows[i]["Status"].ToString() == "Active")
                        //{
                        ddllocation.Items[i].Visible = true;
                    }
                    else
                    {
                        ddllocation.Items[i].Visible = false;
                    }
                }
                //ddllocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                for (int j = 0; j < ddllocation.Items.Count; j++)
                {
                    for (int k = 0; k < dtpermissionDetails.Rows.Count; k++)
                    {
                        if (dtpermissionDetails.Rows[k]["LocationId"].ToString() == dt.Rows[j]["LocationId"].ToString())
                        {
                            ddllocation.Items[j].Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "fetchLocation", path);
        }
    }
    private void fetchCustodian(string UserID)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataTable dtpermissionDetails = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("fetchUserCustodianPermissionDetails", con))
            {
                cmdz.CommandType = CommandType.StoredProcedure;
                cmdz.Parameters.AddWithValue("@UserID", UserID);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dtpermissionDetails);
                }
            }
            DataTable dt = new DataTable();
            using (SqlCommand cmdz = new SqlCommand("select * from CustodianMaster where Active = 1 order by CustodianId asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                ddlCustodian.DataSource = dt;
                ddlCustodian.DataTextField = "CustodianName";
                ddlCustodian.DataValueField = "CustodianId";
                ddlCustodian.DataBind();
                for (int i = 0; i <= ddlCustodian.Items.Count - 1; i++)
                {
                    if (dt.Rows[i]["CustodianName"].ToString() == ddlCustodian.Items[i].Text.ToString())
                    {
                        if (dt.Rows[i]["Active"].ToString() == "1")
                        {
                            ddlCustodian.Items[i].Visible = true;
                        }
                        else
                        {
                            ddlCustodian.Items[i].Visible = false;
                        }
                    }
                }
                //ddllocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                for (int j = 0; j < ddlCustodian.Items.Count; j++)
                {
                    for (int k = 0; k < dtpermissionDetails.Rows.Count; k++)
                    {
                        if (dtpermissionDetails.Rows[k]["CustodianId"].ToString() == dt.Rows[j]["CustodianId"].ToString())
                        {
                            ddlCustodian.Items[j].Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "fetchCustodian", path);
        }
        //    try
        //    {
        //        SqlConnection con = new SqlConnection();
        //        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        //        SqlDataAdapter dpt = new SqlDataAdapter();

        //        //string query = "select * from [dbo].[UserType] ";
        //        //SqlCommand cmd = new SqlCommand(query, con);
        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        DataTable dt = new DataTable();
        //        using (SqlCommand cmdz = new SqlCommand("getCustodian", con))
        //        {
        //            cmdz.CommandType = CommandType.StoredProcedure;
        //            using (SqlDataAdapter adp = new SqlDataAdapter(cmdz))
        //            {
        //                adp.Fill(dt);
        //            }
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlCustodian.DataSource = dt;
        //            ddlCustodian.DataTextField = "CustodianName";
        //            ddlCustodian.DataValueField = "CustodianName";
        //            ddlCustodian.DataBind();
        //            //ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
        //        }
        //        //ddllocation.Items.Insert(0, new ListItem("--Select--", "0", true));
        //        string[] CustodianList = data.Split(',');
        //        for (int i = 0; i < ddlCustodian.Items.Count; i++)
        //        {
        //            if (ddlCustodian.Items.Count() == CustodianList.Count())
        //            {
        //                ddlCustodian.Items[0].Checked = true;
        //            }
        //            for (int j = 0; j < CustodianList.Count(); j++)
        //            {
        //                if (CustodianList[j].ToString() == ddlCustodian.Items[i].Text.ToString())
        //                {
        //                    ddlCustodian.Items[i].Checked = true;
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetUserMaster");

            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                //gridlist.DataSource = null;
                //gridlist.DataBind();
                gvData.DataSource = string.Empty;

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
                //gridlist.DataSource = myView;
                //gridlist.DataBind();
                gvData.DataSource = myView;



            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "grid_view", path);
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text.ToLower() == "submit")
            {
                if (txtboxname.Text.ToString().Trim() == "" || txtboxpas.Text.ToString().Trim() == "")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Username and Password should not be empty..');", true);
                    //string Message = "Username and Password should not be empty..";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Username & Password should not be empty');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                bool UserExist = CheckUserExist(txtboxname.Text.Trim());
                if (UserExist == true)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The user  \"" + txtboxname.Text.Trim() + "\" is Already Exist..');", true);
                    //string Message = "The user  \"" + txtboxname.Text.Trim() + "\" is Already Exist.";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The user '" + txtboxname.Text.Trim() + " already exists');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                else
                {
                    int UserNewID = CheckUserCount();
                    int locCount = ddllocation.Items.Count();
                    string data = "";
                    int loccheckedcount = 0, custcheckcount = 0;
                    for (int k = 0; k < locCount; k++)
                    {
                        if (ddllocation.Items[k].Checked == true)
                        {
                            loccheckedcount++;
                            //data += ddllocation.Items[k].Value + ",";
                            //Session["userid"] = vid;string UserID,string LocationID,string CreatedBY)
                            InsertLocationPermission(UserNewID, Convert.ToInt32(ddllocation.Items[k].Value), Convert.ToInt32(Session["userid"]));
                        }

                    }
                    foreach (RepeaterItem item in rptPerson.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        if (chkDisplayTitle.Checked)
                        {
                            string ab = chkDisplayTitle.Value;
                            // InsertLocationPermission(UserNewID, Convert.ToInt32(chkDisplayTitle.Value), Convert.ToInt32(Session["userid"]));

                        }
                    }
                    int custodianCount = ddlCustodian.Items.Count();
                    string custodianData = "";
                    for (int l = 0; l < custodianCount; l++)
                    {
                        if (ddlCustodian.Items[l].Checked == true)
                        {
                            custcheckcount++;
                            //custodianData += ddlCustodian.Items[l].Value + ",";
                            InsertCustodianPermission(UserNewID, Convert.ToInt32(ddlCustodian.Items[l].Value), Convert.ToInt32(Session["userid"]));
                        }
                    }//CustodianId
                     //custodianData = custodianData.Remove(custodianData.Length - 1, 1);
                    if (loccheckedcount == 0)
                    {
                        //string Message = "Please Select Location";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select Location');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        return;
                    }
                    if (custcheckcount == 0)
                    {
                        //string Message = "Please Select Custodian";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please Select Custodian');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        return;
                    }
                    if (txtEmailID.Text.Trim().Length == 0)
                    {
                        //string Message = "Email id Mandatory";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Email Id Mandatory');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        return;
                    }
                    string email = txtEmailID.Text.Trim();
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
                    if (match.Success)
                    {

                    }
                    else
                    {
                        //string Message = "Email Id format Invalid";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Invalid Email Id Format');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        return;
                    }
                    DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.PinsertUser, new SqlParameter[]
                    {
                            new SqlParameter("@PASSWORD",txtboxpas.Text.Trim()),
                            new SqlParameter("@Type", ddtype.SelectedValue),
                            new SqlParameter("@Status", chkstatus.Checked == true ? 1 : 0),
                            new SqlParameter("@CREATED_BY",Convert.ToInt32( Session["userid"]) ),
                            new SqlParameter("@UserName", txtboxname.Text.Trim()),
                            new SqlParameter("@Approve", chkApprove.Checked == true ? 1 : 0),
                            new SqlParameter("@CanDelete", chkDelete.Checked == true ? 1 : 0),
                            new SqlParameter("@IsDirector",chkIsDirector.Checked==true?1:0),
                            new SqlParameter("@EmailID",txtEmailID.Text.Trim()),

                    }
                        );

                    int UserID = Convert.ToInt32(help.ExecuteScalar());
                    Session["UIDD"] = UserID;
                    if (UserID > 1)
                    {
                        hiduserid.Value = UserID.ToString();
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The user  \"" + txtboxname.Text.Trim() + "\" was inserted successfully.');", true);
                        UpdatePermission();
                        //string Message = "The user  \"" + txtboxname.Text.Trim() + "\" was inserted successfully.";
                        //imgpopup.ImageUrl = "images/Success.png";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The user  \"" + txtboxname.Text.Trim() + "\" was inserted successfully.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                        for (int k = 0; k < ddllocation.Items.Count(); k++)
                        {
                            if (ddllocation.Items[k].Checked == true)
                            {
                                ddllocation.Items[k].Checked = false;
                            }
                        }
                        for (int l = 0; l < custodianCount; l++)
                        {
                            if (ddlCustodian.Items[l].Checked == true)
                            {
                                ddlCustodian.Items[l].Checked = false;
                            }
                        }
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The User '" + txtboxname.Text.Trim() + " inserted Successfully!');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        //return;
                    }
                    // grid_view();
                }

            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                int loccheckedcount = 0, custcheckcount = 0;
                if (txtboxname.Text.ToString().Trim() == "" || txtboxpas.Text.ToString().Trim() == "")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Username and Password should not be empty..');", true);
                    //string Message = "Username and Password should not be empty.";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Username & Password should not be empty!');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                if (txtEmailID.Text.Trim().Length == 0)
                {
                    //string Message = "Email id Mandatory";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Email Id mandatory');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                string email = txtEmailID.Text.Trim();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {

                }
                else
                {
                    //string Message = "Email Id format Invalid";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    txtEmailID.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Invalid Email Id Format');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;

                }
                for (int k = 0; k < ddllocation.Items.Count; k++)
                {
                    if (ddllocation.Items[k].Checked == true)
                    {
                        loccheckedcount++;
                        //data += ddllocation.Items[k].Value + ",";
                        //Session["userid"] = vid;string UserID,string LocationID,string CreatedBY)
                        //InsertLocationPermission(UserNewID, Convert.ToInt32(ddllocation.Items[k].Value), Convert.ToInt32(Session["userid"]));
                    }

                }

                int custodianCount = ddlCustodian.Items.Count();
                string custodianData = "";
                for (int l = 0; l < custodianCount; l++)
                {
                    if (ddlCustodian.Items[l].Checked == true)
                    {
                        custcheckcount++;
                        //custodianData += ddlCustodian.Items[l].Value + ",";
                        //InsertCustodianPermission(UserNewID, Convert.ToInt32(ddlCustodian.Items[l].Value), Convert.ToInt32(Session["userid"]));
                    }
                }
                if (loccheckedcount == 0)
                {
                    //string Message = "Please Select Location";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please select location');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }
                if (custcheckcount == 0)
                {
                    //string Message = "Please Select Custodian";
                    //imgpopup.ImageUrl = "images/info.jpg";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Please select Custodian');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }

                //custodianData = custodianData.Remove(custodianData.Length - 1, 1);
                DataAccessHelper1 help = new DataAccessHelper1(
                   StoredProcedures.PinsertUser, new SqlParameter[] {
                            new SqlParameter("@PASSWORD",txtboxpas.Text.Trim()),
                            new SqlParameter("@Type", ddtype.SelectedValue),
                            new SqlParameter("@Status", chkstatus.Checked == true ? 1 : 0),
                            new SqlParameter("@CREATED_BY",Convert.ToInt32( Session["userid"]) ),
                            new SqlParameter("@UserName", txtboxname.Text.Trim()),
                        //new SqlParameter("@UserId", Convert.ToInt32(hiduserid.Value)),  
                            new SqlParameter("@UserId", Convert.ToInt32(Session["UIDD"])),
                            new SqlParameter("@Approve", chkApprove.Checked == true ? 1 : 0),
                            new SqlParameter("@CanDelete", chkDelete.Checked == true ? 1 : 0),
                            new SqlParameter("@IsDirector",chkIsDirector.Checked==true?1:0),
                            new SqlParameter("@EmailID",txtEmailID.Text.Trim()),
                }
                   );
                int locCount = ddllocation.Items.Count();
                DeleteOldData(Convert.ToInt32(Session["UIDD"]));
                for (int k = 0; k < ddllocation.Items.Count(); k++)
                {
                    if (ddllocation.Items[k].Checked == true)
                    {
                        UpdateLocationPermission(Convert.ToInt32(Session["UIDD"]), Convert.ToInt32(ddllocation.Items[k].Value), Convert.ToInt32(Session["userid"]));
                    }

                }
                //int custodianCount = ddlCustodian.Items.Count();
                for (int l = 0; l < ddlCustodian.Items.Count(); l++)
                {
                    if (ddlCustodian.Items[l].Checked == true)
                    {
                        UpdateCustodianPermission(Convert.ToInt32(Session["UIDD"]), Convert.ToInt32(ddlCustodian.Items[l].Value), Convert.ToInt32(Session["userid"]));
                    }
                }

                if (help.ExecuteNonQuery() == 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The user  \"" + txtboxname.Text.Trim() + "\" was updated successfully.');", true);
                    //string Message = "The user  \"" + txtboxname.Text.Trim() + "\" was updated successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The user  \"" + txtboxname.Text.Trim() + "\" was updated successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The user '" + txtboxname.Text.Trim() + "updated successfully!!');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    for (int k = 0; k < ddllocation.Items.Count(); k++)
                    {
                        if (ddllocation.Items[k].Checked == true)
                        {
                            ddllocation.Items[k].Checked = false;
                        }
                    }
                    for (int l = 0; l < custodianCount; l++)
                    {
                        if (ddlCustodian.Items[l].Checked == true)
                        {
                            ddlCustodian.Items[l].Checked = false;
                        }
                    }
                    if (ddtype.SelectedItem.Text != "Admin")
                    {
                        UpdatePermission();
                    }
                    else
                    {
                        ClearDataGrid1();
                    }



                }

                btnsubmit.Text = "Submit";


            }
            hiduserid.Value = txtboxname.Text = txtboxpas.Text = txtEmailID.Text = "";
            grid_view();
            gvData.DataBind();
            txtboxname.Text = string.Empty;
            txtboxpas.Text = string.Empty;
            chkstatus.Checked = false;
            chkIsDirector.Checked = false;
            chkApprove.Checked = false;
            chkDelete.Checked = false;
            ddlCustodian.SelectedIndex = 0;
            ddllocation.SelectedIndex = 0;
            ddtype.SelectedIndex = 0;
            for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            {
                lstPermission.Items[i].Selected = false;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "changebuttonname('Submit');", true);
            // Response.Redirect("TestMaster.aspx");

        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            string Message = ex.Message;
            //Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "btnsubmit_Click", path);
            //imgpopup.ImageUrl = "images/CloseRed.png";
            //lblpopupmsg.Text = Message;
            //trheader.BgColor = "#98CODA";
            //trfooter.BgColor = "#98CODA";
            //ModalPopupExtender2.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + ex.ToString() + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
    }
    private void DeleteOldData(int UserID)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("delete from LocationPermission where UserID=@UserID;delete from CustodianPermission where UserID = @UserID;", con))
            {
                cmd.Parameters.AddWithValue("@UserID", UserID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "DeleteOldData", path);
        }
    }
    private void UpdatePermission()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    //foreach (DataGridItem item in DataGrid1.Items)
                    //{

                    //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    //    {
                    for (int i = 0; i < lstPermission.Items.Count; i++)
                    {
                        string Pageid = lstPermission.Items[i].Value;
                        int Permission = lstPermission.Items[i].Selected == true ? 1 : 0;
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PInsertUpdateUserPermission", new SqlParameter[] {
                            new SqlParameter("@userID", Session["UIDD"]),
                            new SqlParameter("@PageID", Convert.ToInt32(Pageid)),
                            new SqlParameter("@Permission", Permission),
                            new SqlParameter("@CreatedBy", Convert.ToInt32(Session["userid"].ToString())),
                    });
                    }

                    //HiddenField hdnpageID = (HiddenField)item.Cells[1].FindControl("hdnpageID");
                    //CheckBox checkPermission = (CheckBox)item.Cells[2].FindControl("checkPermission");
                    //int Permission = checkPermission.Checked == true ? 1 : 0;



                    //    }

                    //}
                    Trans.Commit();
                    BindPer("0");
                    //ClearDataGrid1();
                }
                catch (Exception ex)
                {
                    Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "UpdatePermission", path);
                    Trans.Rollback();
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "UpdatePermission", path);
        }
    }

    private void ClearDataGrid1()
    {
        ////foreach (DataGridItem item in DataGrid1.Items)
        ////{

        ////    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        ////    {
        ////        CheckBox checkPermission = (CheckBox)item.Cells[2].FindControl("checkPermission");
        ////        if (checkPermission.Checked)
        ////        {
        ////            checkPermission.Checked = false;
        ////        }

        ////    }
        ////}
    }

    private bool CheckUserExist(string UserName)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckUserExist", new SqlParameter[] {
                        new SqlParameter("@UserName", UserName),
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

    private int CheckUserCount()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select Count(*) from TBL_USERMST", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                int CountVal = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                return CountVal + 1;
            }
            else
            {
                return 1;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "CheckUserCount", path);
            return 0;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "InsertCustodianPermission", path);
        }
    }
    public void UpdateCustodianPermission(int UserID, int CustodianId, int CreatedBY)
    {
        try
        {
            //UserID, LocationID, IsPermission, CreatedBY, CreatedDate
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("sp_updateCustodianPermission", con))
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "UpdateCustodianPermission", path);
        }
    }
    public void InsertLocationPermission(int UserID, int LocationID, int CreatedBY)
    {
        try
        {
            //UserID, LocationID, IsPermission, CreatedBY, CreatedDate
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("sp_insertLocationPermission", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CreatedBY", CreatedBY);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "InsertLocationPermission", path);
        }
    }
    public void UpdateLocationPermission(int UserID, int LocationID, int CreatedBY)
    {
        try
        {
            //UserID, LocationID, IsPermission, CreatedBY, CreatedDate
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("sp_updateLocationPermission", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CreatedBY", CreatedBY);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "UpdateLocationPermission", path);
        }
    }
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        try
        {
            Label UserName = e.Item.Cells[0].FindControl("UserName") as Label;
            Label UserId = e.Item.Cells[0].FindControl("UserId") as Label;
            hiduserid.Value = UserId.Text;
            HiddenField hidtype = (HiddenField)e.Item.Cells[0].FindControl("hidtype");
            Label PASSWORD = e.Item.Cells[0].FindControl("PASSWORD") as Label;
            Label Active = e.Item.Cells[0].FindControl("Active") as Label;
            Label IsDirector = e.Item.Cells[0].FindControl("IsDirector") as Label;
            ddtype.SelectedValue = hidtype.Value;
            txtboxname.Text = UserName.Text;
            txtboxpas.Text = PASSWORD.Text;
            if (Active.Text == "Active")
            {
                chkstatus.Checked = true;
            }
            else
            {
                chkstatus.Checked = false;
            }
            if (IsDirector.Text == "Yes")
            {
                chkIsDirector.Checked = true;
            }
            else
            {
                chkIsDirector.Checked = false;
            }
            BindPer(UserId.Text);
            btnsubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "EditDataGrid", path);
        }
    }

    private void BindPer(string UserID)
    {

        try
        {
            DataSet ds = new DataSet(); ;

            DataAccessHelper1 help = new DataAccessHelper1(
           StoredProcedures.GetModuleWisePermission, new SqlParameter[] {
                      new SqlParameter("@userID",  Convert.ToInt32(UserID)),

                    });
            ds = help.ExecuteDataset();
            lstPermission.DataSource = ds;
            //lstPermission.DataTextField = "ModuleName";
            //lstPermission.DataValueField = "ModuleID";
            lstPermission.DataTextField = "PageName";
            lstPermission.DataValueField = "PageID";
            lstPermission.DataBind();



            for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            {
                if (ds.Tables[0].Rows[i]["PageName"].ToString() == lstPermission.Items[i].Text.ToString())
                {
                    if (ds.Tables[0].Rows[i]["IsPermission"].ToString() == "1")
                    {
                        lstPermission.Items[i].Selected = true;
                    }
                    else
                    {
                        lstPermission.Items[i].Selected = false;
                    }
                }
            }


            //for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            //{
            //    if (ds.Tables[0].Rows[i]["PageName"].ToString() == lstPermission.Items[i].Text.ToString())
            //    {
            //        if (ds.Tables[0].Rows[i]["IsPermission"].ToString() == "1")
            //        {
            //            lstPermission.Items[i].Selected = true;
            //        }
            //        else
            //        {
            //            lstPermission.Items[i].Selected = false;
            //        }
            //    }
            //}





            //if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            //{
            //    DataGrid1.DataSource = null;

            //    DataGrid1.DataBind();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];

            //    DataView myView;
            //    myView = ds.Tables[0].DefaultView;

            //    DataGrid1.DataSource = myView;

            //    DataGrid1.DataBind();

            //}
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "BindPer", path);
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox checkPermission = (CheckBox)e.Item.FindControl("checkPermission");
                checkPermission.Checked = (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "IsPermission")).ToString() == "1") ? true : false;

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "DataGrid1_ItemDataBound", path);
        }
    }

    // For editing and deleting
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "dit")
            {
                lblMessage.Text = "";

                GridDataItem item = (GridDataItem)e.Item;
                string UserId = item["UserId"].Text;
                string UserName = item["UserName"].Text;
                string Password = item["PASSWORD"].Text;
                string Active = item["Status"].Text;
                string Type = item["Type"].Text;
                string Approve = item["Approve"].Text;
                string CanDelete = item["CanDelete"].Text;
                string IsDirector = item["IsDirector"].Text;
                string EmailID = item["EmailID"].Text;
                Session["UIDD"] = UserId;

                ddtype.SelectedValue = Type;
                // ddllocation.SelectedValue = Location;
                fetchLocation(UserId);
                fetchCustodian(UserId);
                //ddlCustodian.SelectedValue = CustodianName;
                txtboxname.Text = UserName;
                txtboxpas.Text = Password;
                if (EmailID == "&nbsp;")
                {
                    txtEmailID.Text = "";
                }
                else
                {
                    txtEmailID.Text = EmailID;
                }
                if (IsDirector == "Yes")
                {
                    chkIsDirector.Checked = true;
                }
                else
                {
                    chkIsDirector.Checked = false;
                }
                if (Active == "Active")
                {
                    chkstatus.Checked = true;
                }
                else
                {
                    chkstatus.Checked = false;
                }

                if (Approve == "Yes")
                {
                    chkApprove.Checked = true;
                }
                else
                {
                    chkApprove.Checked = false;
                }

                if (CanDelete == "Yes")
                {
                    chkDelete.Checked = true;
                }
                else
                {
                    chkDelete.Checked = false;
                }

                BindPer(UserId);
                btnsubmit.Text = "Update";


                //Label UserName = e.Item.Cells[0].FindControl("UserName") as Label;
                //Label UserId = e.Item.Cells[0].FindControl("UserId") as Label;
                //hiduserid.Value = UserId.Text;
                //HiddenField hidtype = (HiddenField)e.Item.Cells[0].FindControl("hidtype");
                //Label PASSWORD = e.Item.Cells[0].FindControl("PASSWORD") as Label;
                //Label Active = e.Item.Cells[0].FindControl("Active") as Label;
                //ddtype.SelectedValue = hidtype.Value;
                //txtboxname.Text = UserName.Text;
                //txtboxpas.Text = PASSWORD.Text;
                //if (Active.Text == "Active")
                //{
                //    chkstatus.Checked = true;
                //}
                //else
                //{
                //    chkstatus.Checked = false;
                //}
                //BindPer(UserId.Text);
                btnsubmit.Text = "Update";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "changebuttonname('Update');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);

            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "gv_data_ItemCommand", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "gvData_NeedDataSource", path);
        }
    }

    protected void ddtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddtype.SelectedValue == "1")
            {
                for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
                {
                    lstPermission.Items[i].Selected = true;
                }
                //ddtype.d = false;
            }
            else
            {
                for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
                {
                    lstPermission.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "ddtype_SelectedIndexChanged", path);
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptPerson.Items)
        {
            HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
            if (chkDisplayTitle.Checked)
            {
                string ab = chkDisplayTitle.Value;
                //HERE IS YOUR VALUE: chkAddressSelected.Value
            }
        }
    }


    protected void rptPerson_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType.Equals(ListItemType.AlternatingItem) || e.Item.ItemType.Equals(ListItemType.Item))
        {
            CheckBox cbox = e.Item.FindControl("chkDisplayTitle") as CheckBox;
            if (cbox != null)
            {
                cbox.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "discontinued"));
            }
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptPerson.Items)
        {
            HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
            string ab = "3";
            if (chkDisplayTitle.Value == ab)
            {
                chkDisplayTitle.Checked = true;
            }
        }
    }

    protected void custodianrepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType.Equals(ListItemType.AlternatingItem) || e.Item.ItemType.Equals(ListItemType.Item))
        {
            CheckBox cbox = e.Item.FindControl("custodianrepeater") as CheckBox;
            if (cbox != null)
            {
                cbox.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "discontinued"));
            }
        }
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in custodianrepeater.Items)
        {

            HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkcustodianID");
            string ab = "3";
            if (chkDisplayTitle.Value == ab)
            {
                chkDisplayTitle.Checked = true;
            }
            //chkDisplayTitle.Checked = "3";
            //if (chkDisplayTitle.Checked)
            //{
            //    string ab = chkDisplayTitle.Value;
            //    //HERE IS YOUR VALUE: chkAddressSelected.Value
            //}
        }
    }
}