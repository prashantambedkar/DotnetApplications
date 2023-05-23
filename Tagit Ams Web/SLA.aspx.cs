using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "Page_Load", path);
        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
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
    protected void rptdocumentCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
    public void grid_view()
    {
        try
        {
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("fetchSLAData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                gvData.DataSource = string.Empty;
            }
            else
            {
                DataTable dtx = ds.Tables[0];
                dtx.Columns.Add(new DataColumn("CategoryName", typeof(string)));
                DataTable dt = datacolumnModify(dtx);
                gvData.DataSource = null;
                // DataTable dt = ds.Tables[0];
                DataView myView = null;
                myView = dt.DefaultView;
                gvData.DataSource = myView;
                gvData.DataBind();
                //gvData.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public DataTable datacolumnModify(DataTable dt)
    {

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string categoryname = "";
            string catid = dt.Rows[i]["CategoryId"].ToString();
            string[] catidlist = catid.Split(',');
            for (int j = 0; j < catidlist.Count(); j++)
            {
                categoryname += fetchcatname(catidlist[j].ToString()) + ",";
            }
            categoryname = categoryname.Remove(categoryname.Length - 1);
            dt.Rows[i]["CategoryName"] = categoryname;
            dt.AcceptChanges();
        }
        return dt;
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
                    if (btnadd.Text == "UPDATE") { }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("insertSLAMaster", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                            cmd.Parameters.AddWithValue("@noOfDays", txtnoofdays.Text);
                            cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                            cmd.Parameters.AddWithValue("@toEmailId", txttoemailid.Text);
                            cmd.Parameters.AddWithValue("@ccEmailId", txtccemailid.Text);
                            cmd.Parameters.AddWithValue("@casemanageremailstat", chkcaseManager.Checked == true ? 1 : 0);
                            cmd.Parameters.AddWithValue("@caseworkeremailstat", chkcaseWorker.Checked == true ? 1 : 0);
                            cmd.Parameters.AddWithValue("@casemanageremailstat2", chkcaseManager2.Checked == true ? 1 : 0);
                            cmd.Parameters.AddWithValue("@caseworkeremailstat2", chkcaseWorker2.Checked == true ? 1 : 0);
                            cmd.Parameters.AddWithValue("@createdBy", Convert.ToInt32(Session["userid"].ToString()));
                            cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('SLA Details Added Successfully!');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openalertModal();", true);
                            //ddlLocation.SelectedValue = "0";
                            txtnoofdays.Text = "1";
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
                            txtcategoryitems.Text = txttoemailid.Text = txtccemailid.Text = txtcategoryname.Text = "";
                            chkcaseManager.Checked = chkcaseWorker.Checked = chkcaseManager2.Checked = chkcaseWorker2.Checked = false;
                            chkcaseManager.Enabled = chkcaseWorker.Enabled = chkcaseManager2.Enabled = chkcaseWorker2.Enabled = true;
                            //ddlLocation.SelectedValue = "0";
                            grid_view();
                            fillLocations();
                            fillCategory();
                        }
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
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    Label hl = (e.Item as GridDataItem)["CategoryNamez"].Controls[0] as Label;
        //    hl.Text = hl.Text.Substring(0, 28) + "...";
        //}
    }
    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "gvData_NeedDataSource", path);
        }
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //edit operations
        try
        {
            if (e.CommandName == "dit")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                Session["itemidd1"] = id;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal1();", true);


            }
            if (e.CommandName == "edview")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                string LocationId = item["LocationId"].Text;
                string LocationName = item["LocationName"].Text;
                Session["SLAid"] = id;
                Response.Redirect("SLA_.aspx");
                //if (ddlLocation.Items.FindByValue(LocationId) != null)
                //{
                //    ddlLocation.Items.FindByValue(LocationId).Selected = true;
                //}
                //ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByText(LocationName));
                // ddlLocation.SelectedValue = LocationId;
                //ddlLocation.Items.FindByText(LocationName).Selected = true;
                //ddlLocation.SelectedItem.Text = LocationName;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "gv_data_ItemCommand", path);
        }
    }
    public void markcategories(string categoriesID)
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
        string[] catid = categoriesID.Split(',');
        for (int j = 0; j < catid.Count(); j++)
        {
            foreach (RepeaterItem item in rptdocumentCategory.Items)
            {
                HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("chkDisplayTitle");

                for (int i = 1; i <= categorycount; i++)
                {
                    if (chkDisplayTitle.Value == catid[j].ToString())
                    {
                        chkDisplayTitle.Checked = true;
                    }
                }

            }
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "SLA.aspx", "gvData_Init", path);
        }
    }
    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                //item["CategoryCode"].Text = Category.ToUpper() + " ID";
                //item["CategoryName"].Text = Category.ToUpper() + " NAME";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gvData_ItemCreated", path);
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
    protected void Button2_Click(object sender, EventArgs e)
    {
        using (SqlCommand cmd = new SqlCommand("deleteSLAMasterdata", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", Session["itemidd1"].ToString());
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        grid_view();
    }
}