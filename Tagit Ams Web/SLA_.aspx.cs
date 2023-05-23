using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    public static String pdfconfigid = "";
    public static String LocationName = "";
    public static String imgPath = ""; public static string path = "";
    public static int mark = 0;
    public static int categorycount = 0;
    SqlConnection conn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            if (!IsPostBack)
            {
                mark = 0;
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                if (userAuthorize((int)pages.SLA, Session["userid"].ToString()))
                {
                    Page.DataBind();
                    HdnLocation.Value = Location;
                    CompanyImg.Src = "images/" + _Logo;
                    txtnoofdays.Text = "1";
                    fillLocations();
                    fillCategory();
                    fillfields();
                    // grid_view();
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA_.aspx", "Page_Load", path);
            Response.Redirect("SLA.aspx");
        }

    }
    public void fillfields()
    {
        //Session["SLAid"]
        DataTable dt = new DataTable();
        using (SqlCommand cmd = new SqlCommand("select * from SLAMaster where id=@id", conn))
        {
            cmd.Parameters.AddWithValue("@id", Session["SLAid"].ToString());
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dt);
            }
        }
        ddlLocation.SelectedValue = dt.Rows[0]["LocationId"].ToString();
        txtnoofdays.Text = dt.Rows[0]["noOfDays"].ToString();
        txttoemailid.Text = dt.Rows[0]["toEmailId"].ToString();
        txtccemailid.Text = dt.Rows[0]["ccEmailId"].ToString();
        string casemanageremailstat = dt.Rows[0]["casemanageremailstat"].ToString();
        string caseworkeremailstat = dt.Rows[0]["caseworkeremailstat"].ToString();
        string casemanageremailstat2 = dt.Rows[0]["casemanageremailstat2"].ToString();
        string caseworkeremailstat2 = dt.Rows[0]["caseworkeremailstat2"].ToString();
        string CategoryIds = dt.Rows[0]["CategoryId"].ToString();
        string[] catsplit = CategoryIds.Split(',');
        int c = 0;
        string categorynames = "";
        for (int j = 0; j < catsplit.Count(); j++)
        {
            categorynames += catname(catsplit[j]) + ",";
            foreach (RepeaterItem item in rptdocumentCategory.Items)
            {
                HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                for (int i = 1; i <= categorycount; i++)
                {
                    if (chkDisplayTitle.Value == catsplit[j])
                    {
                        c++;
                        chkDisplayTitle.Checked = true;
                    }
                }

            }
            if (c == categorycount)
            {
                btnmark.Text = "UNMARK ALL";
            }
            else
            {
                btnmark.Text = "MARK ALL";
            }
        }
        txtcategoryitems.Text = categorynames.Remove(categorynames.Length - 1); ;

        if (casemanageremailstat == "1")
        {
            chkcaseManager.Checked = true;
            chkcaseManager.Enabled = true;
            chkcaseManager2.Checked = false;
            chkcaseManager2.Enabled = false;
        }

        if (casemanageremailstat2 == "1")
        {
            chkcaseManager2.Checked = true;
            chkcaseManager2.Enabled = true;
            chkcaseManager.Checked = false;
            chkcaseManager.Enabled = false;
        }

        if (caseworkeremailstat == "1")
        {
            chkcaseWorker.Checked = true;
            chkcaseWorker.Enabled = true;
            chkcaseWorker2.Checked = false;
            chkcaseWorker2.Enabled = false;
        }

        if (caseworkeremailstat2 == "1")
        {
            chkcaseWorker2.Checked = true;
            chkcaseWorker2.Enabled = true;
            chkcaseWorker.Checked = false;
            chkcaseWorker.Enabled = false;
        }

    }
    protected void btnmark_Click(object sender, EventArgs e)
    {
        if (btnmark.Text == "UNMARK ALL")
        {
            mark = 2; txtcategoryname.Text = "";
        }
        else
        {
            mark = 1; txtcategoryname.Text = "";
        }
        fillCategory();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    public string fetchcatname(string id)
    {
        try
        {
            DataTable dtname = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getCategory_withfilterid", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(id));
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtname);
                }
            }
            if (dtname.Rows.Count > 0)
            {
                return dtname.Rows[0].ItemArray[1].ToString();
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "fetchcatname", path);
            return "";
        }
    }
    protected void btnsavecategoryitems_Click(object sender, EventArgs e)
    {
        string ab = "";
        foreach (RepeaterItem item in rptdocumentCategory.Items)
        {
            HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
            if (chkDisplayTitle.Checked)
            {

                ab += fetchcatname(chkDisplayTitle.Value) + ",";

                //HERE IS YOUR VALUE: chkAddressSelected.Value
            }
            else
            {

            }
        }
        if (ab.Length > 0)
        {
            ab = ab.Remove(ab.Length - 1);
        }
        txtcategoryitems.Text = ab;
    }
    public string catname(string catid)
    {
        DataTable dt = new DataTable();
        using (SqlCommand cmd = new SqlCommand("select CategoryName from CategoryMaster where CategoryId=@CategoryId", conn))
        {
            cmd.Parameters.AddWithValue("@CategoryId", catid);
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dt);
            }
        }
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["CategoryName"].ToString();
        }
        return "";
    }

    protected void rptdocumentCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
    private void fillLocations()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct lm.LocationName,lm.LocationId from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where Active=1 and lp.UserID=@UserID order by lm.LocationName asc", con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    ddlLocation.DataSource = dt;
                    ddlLocation.DataTextField = "LocationName";
                    ddlLocation.DataValueField = "LocationId";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlLocation.SelectedIndex = 1;
                }
                else
                {
                    ddlLocation.DataSource = null;
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillMajorLocations", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillMajorLocations", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "fillLocations", path);
        }
    }

    public void fillCategory()
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
                rptdocumentCategory.DataSource = dtddlCategory;
                rptdocumentCategory.DataBind();
                categorycount = Convert.ToInt32(dtddlCategory.Rows.Count);
                if (mark == 1)
                {
                    foreach (RepeaterItem item in rptdocumentCategory.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        for (int i = 1; i <= dtddlCategory.Rows.Count; i++)
                        {
                            if (chkDisplayTitle.Value == i.ToString())
                            {
                                chkDisplayTitle.Checked = true;
                            }
                        }

                    }
                    btnmark.Text = "UNMARK ALL";
                }
                if (mark == 2)
                {
                    foreach (RepeaterItem item in rptdocumentCategory.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        for (int i = 1; i <= categorycount; i++)
                        {
                            if (chkDisplayTitle.Value == i.ToString())
                            {
                                chkDisplayTitle.Checked = false;
                            }
                        }

                    }
                    btnmark.Text = "MARK ALL";
                }
            }
            else
            {
                rptdocumentCategory.DataSource = null;
                rptdocumentCategory.DataBind();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "fillddlCategory", path);
        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    protected void txtcategoryname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtddlCategory = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getCategory_withfilter", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryName", txtcategoryname.Text);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtddlCategory);
                }
            }
            if (dtddlCategory.Rows.Count > 0)
            {
                rptdocumentCategory.DataSource = dtddlCategory;
                rptdocumentCategory.DataBind();
                if (mark == 1)
                {
                    foreach (RepeaterItem item in rptdocumentCategory.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        for (int i = 1; i <= categorycount; i++)
                        {
                            if (chkDisplayTitle.Value == i.ToString())
                            {
                                chkDisplayTitle.Checked = true;
                            }
                        }

                    }
                    btnmark.Text = "UNMARK ALL";
                    txtcategoryname.Text = "";
                }
                if (mark == 2)
                {
                    foreach (RepeaterItem item in rptdocumentCategory.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        for (int i = 1; i <= dtddlCategory.Rows.Count; i++)
                        {
                            if (chkDisplayTitle.Value == i.ToString())
                            {
                                chkDisplayTitle.Checked = false;
                            }
                        }

                    }
                    btnmark.Text = "MARK ALL";
                    txtcategoryname.Text = "";
                }
            }
            else
            {
                rptdocumentCategory.DataSource = null;
                rptdocumentCategory.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "txtcategoryname_TextChanged", path);
        }
    }
    protected void chkcaseManager_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcaseManager.Checked == true)
        {
            chkcaseManager2.Enabled = false;
        }
        else
        {
            chkcaseManager2.Enabled = true;
        }
    }
    protected void chkcaseWorker_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcaseWorker.Checked == true)
        {
            chkcaseWorker2.Enabled = false;
        }
        else
        {
            chkcaseWorker2.Enabled = true;
        }
    }
    protected void chkcaseManager2_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcaseManager2.Checked == true)
        {
            chkcaseManager.Enabled = false;
        }
        else
        {
            chkcaseManager.Enabled = true;
        }
    }
    protected void chkcaseWorker2_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcaseWorker2.Checked == true)
        {
            chkcaseWorker.Enabled = false;
        }
        else
        {
            chkcaseWorker.Enabled = true;
        }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Select Location');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                ddlLocation.Focus();
                return;
            }
            if (txttoemailid.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Enter TO Email ID');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                txttoemailid.Focus();
                return;
            }
            if (txtccemailid.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Enter CC Email ID');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                txtccemailid.Focus();
                return;
            }
            if (txtcategoryitems.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Select Document Categories');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                //txtccemailid.Focus();
                return;
            }
            if (checktoemailid())
            {
                if (checkccemailid())
                {
                    string ab = "";
                    foreach (RepeaterItem item in rptdocumentCategory.Items)
                    {
                        HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");
                        if (chkDisplayTitle.Checked)
                        {

                            ab += chkDisplayTitle.Value + ",";

                            //HERE IS YOUR VALUE: chkAddressSelected.Value
                        }
                    }
                    ab = ab.Remove(ab.Length - 1);
                    string CategoryId = ab;

                    using (SqlCommand cmd = new SqlCommand("updateSLAMaster", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Session["SLAid"].ToString());
                        cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@noOfDays", txtnoofdays.Text);
                        cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                        cmd.Parameters.AddWithValue("@toEmailId", txttoemailid.Text);
                        cmd.Parameters.AddWithValue("@ccEmailId", txtccemailid.Text);
                        cmd.Parameters.AddWithValue("@casemanageremailstat", chkcaseManager.Checked == true ? 1 : 0);
                        cmd.Parameters.AddWithValue("@caseworkeremailstat", chkcaseWorker.Checked == true ? 1 : 0);
                        cmd.Parameters.AddWithValue("@casemanageremailstat2", chkcaseManager2.Checked == true ? 1 : 0);
                        cmd.Parameters.AddWithValue("@caseworkeremailstat2", chkcaseWorker2.Checked == true ? 1 : 0);
                        cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp11", "setvalueforlocation1('SLA Details Updated Successfully!');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal1();", true);
                        //ddlLocation.SelectedValue = "0";
                        Session["SLAid"] = null;



                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Invalid Email format in CC-EMAIL ID!');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Invalid Email format in TO-EMAIL ID!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "fetchcatname", path);
            Response.Redirect("SLA.aspx");
        }
    }
    public bool checktoemailid()
    {
        string[] emailid = txttoemailid.Text.Split(',');
        for (int i = 0; i < emailid.Count(); i++)
        {
            bool isEmail = Regex.IsMatch(emailid[i].ToString(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail == false)
            {
                return false;
            }
        }
        return true;
    }
    public bool checkccemailid()
    {
        string[] emailid = txtccemailid.Text.Split(',');
        for (int i = 0; i < emailid.Count(); i++)
        {
            bool isEmail = Regex.IsMatch(emailid[i].ToString(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail == false)
            {
                return false;
            }
        }
        return true;
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        Response.Redirect("SLA.aspx");
    }
}