using ECommerce.Common;
using ECommerce.DataAccess;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
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
    public static String imgPath = "";
    public static string path = "";

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
                if (userAuthorize((int)pages.PdfReportConfig, Session["userid"].ToString()))
                {
                    Page.DataBind();
                    HdnLocation.Value = Location;
                    CompanyImg.Src = "images/" + _Logo;
                    fillddlLocation();
                    fillGrid();
                    lblmsg.Text = "";
                    btnSubmit.Text = "SUBMIT";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "Page_Load", path);

        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    public string StrSort;

    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "gvData_Init", path);
        }
    }

    private void fillGrid()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select tprc.pdfconfigid,lm.LocationName,tprc.imgPath,tprc.address,lm.LocationCode, case when lm.Active=1 then 'Active' else 'Inactive' end as ActiveStatus from tblPdfReportConfiguration as tprc left join LocationMaster as lm on lm.LocationId=tprc.LocationId where lm.Active=1", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            string html = "";
            if (ds.Tables.Count > 0)
            {
                gvData.DataSource = ds.Tables[0];
                gvData.DataBind();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "fillGrid", path);
        }
    }
    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            fillGrid();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "gvData_NeedDataSource", path);
        }
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "dit")
            //{
            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //    GridDataItem item = (GridDataItem)e.Item;
            //    int pdfconfigid = Convert.ToInt32(item["pdfconfigid"].Text);
            //    string LocationName = item["LocationName"].Text;
            //    DataTable dt = new DataTable();
            //    using (SqlCommand cmd1 = new SqlCommand("select lm.LocationId,tprc.pdfconfigid,lm.LocationName,tprc.imgPath,lm.LocationCode,tprc.address from tblPdfReportConfiguration as tprc left join LocationMaster as lm on lm.LocationId=tprc.LocationId where lm.Active=1 and lm.LocationName=@LocationName", con))
            //    {
            //        cmd1.Parameters.AddWithValue("@LocationName", LocationName);
            //        using (SqlDataAdapter ad = new SqlDataAdapter(cmd1))
            //        {
            //            ad.Fill(dt);
            //        }
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlLocation.SelectedValue = dt.Rows[0].ItemArray[0].ToString();
            //        lblimagepath.Text = dt.Rows[0].ItemArray[3].ToString();
            //        Imgcompanylogo.ImageUrl = dt.Rows[0].ItemArray[3].ToString();
            //        Session["pdfconfigid"] = dt.Rows[0].ItemArray[1].ToString();
            //        txtAddress.Text = dt.Rows[0].ItemArray[5].ToString();
            //    }
            //}
            if (e.CommandName == "dels")
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                DataTable dt = new DataTable();
                GridDataItem item = (GridDataItem)e.Item;
                string LocationName = item["LocationName"].Text;
                using (SqlCommand cmd1 = new SqlCommand("select lm.LocationId,tprc.pdfconfigid,lm.LocationName,tprc.imgPath,lm.LocationCode, case when lm.Active=1 then 'Active' else 'Inactive' end as ActiveStatus from tblPdfReportConfiguration as tprc left join LocationMaster as lm on lm.LocationId=tprc.LocationId where lm.Active=1 and lm.LocationName=@LocationName", con))
                {
                    cmd1.Parameters.AddWithValue("@LocationName", LocationName);
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd1))
                    {
                        ad.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    Session["locID"] = dt.Rows[0].ItemArray[0].ToString();
                    Session["LocationName"] = dt.Rows[0].ItemArray[2].ToString();
                    //lbllocname.Text = dt.Rows[0].ItemArray[2].ToString();
                    //string confirmValue = "";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + dt.Rows[0].ItemArray[2].ToString() + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    //if (confirmValue == "Yes")
                    //{


                    //}
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "gv_data_ItemCommand", path);
        }
    }
    private void fillddlLocation()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataTable dt = new DataTable();
            //using (SqlCommand cmd = new SqlCommand("select LocationId,LocationName from LocationMaster where Active=1 order by LocationName asc", con))
            //{
            //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            //    {
            //        adp.Fill(dt);
            //    }
            //}
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + Session["userid"].ToString() + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            ddlLocation.Items.Clear();
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
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "fillddlLocation", path);
        }
    }

    private void fillddlLocationV2(string LocationId)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select LocationId,LocationName from LocationMaster where Active=1", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            ddlLocation.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlLocation.DataSource = dt;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationId";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlLocation.SelectedValue = LocationId;
            }
            else
            {
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "fillddlLocationV2", path);
        }
    }

    [WebMethod]
    public static void Dashboard_Filtered_Location(string name)
    {
        try
        {
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = name;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "Dashboard_Filtered_Location", path);
        }
    }

    [System.Web.Services.WebMethod()]
    public static string SendParameters(string name)
    {
        return string.Format("Name: {0}", name);
    }

    [WebMethod]
    public static int LogoutCheck()
    {
        if (HttpContext.Current.Session["userid"] == null)
        {
            return 0;
        }
        return 1;
    }

    public class LocationStock
    {
        public string LocationName { get; set; }
        public double Stock { get; set; }

    }
    public class PrintVsTagged
    {
        public double? PrintStatus { get; set; }
        public double? IsTagged { get; set; }
        public double? LocationId { get; set; }
    }
    public class PrintVsTaggedCount
    {
        public double? Printed { get; set; }
        public double? Tagged { get; set; }

    }
    public class ChartData
    {
        public string StockDate { get; set; }
        public int Found { get; set; }
        public int MissMatch { get; set; }
        public int Missing { get; set; }
        public int Extra { get; set; }
    }
    public class PrintVsTaggedInfo
    {
        public string StringColumn { get; set; }
        public double dataCount { get; set; }
    }
    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            HttpContext.Current.Session["VAL"] = e.PostBackValue;
            Response.Redirect("Default.aspx");
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "Chart1_Click", path);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string ext = fileuploadCompanyLogo.FileName.ToString().Split('.').Last();

            if (ext == "jpg" || ext == "png" || ext == "jpeg")
            {
                int newbtnSubmit_ClickID = fetchtoppdfconfigID();
                string folderPath = Server.MapPath("~/PdfReportCompanyLogo/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);

                }

                fileuploadCompanyLogo.SaveAs(folderPath + Path.GetFileName(ddlLocation.SelectedValue + fileuploadCompanyLogo.FileName));
                Imgcompanylogo.ImageUrl = "~/PdfReportCompanyLogo/" + Path.GetFileName(ddlLocation.SelectedValue + fileuploadCompanyLogo.FileName);
                lblimagepath.Text = "~/PdfReportCompanyLogo/" + Path.GetFileName(ddlLocation.SelectedValue + fileuploadCompanyLogo.FileName);
                //lblmsg.Text = "Image Uploaded Successfully!";
                //lblmsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only Image Files Allowed!!')", true); return;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "btnUpload_Click", path);
            lblmsg.Text = "";
            btnUpload.Enabled = true;
            btnSubmit.Enabled = false;
        }

    }
    byte[] ReadFile(string sPath)
    {
        try
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes to read from file.
            //In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //tblPdfReportConfiguration
        try
        {
            if (txtAddress.Text.Length <= 500)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

                DataTable dt = new DataTable();
                using (SqlCommand cmd1 = new SqlCommand("select * from tblPdfReportConfiguration where LocationId=@LocationId ", con))
                {
                    cmd1.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd1))
                    {
                        ad.Fill(dt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    // byte[] imageData = ReadFile(lblimagepath.Text);
                    byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(lblimagepath.Text));
                    using (SqlCommand cmd = new SqlCommand("insert into tblPdfReportConfiguration(LocationId,imgPath,imgBinaryData,address) values(@LocationId,@imgPath,@imgBinaryData,@address)", con))
                    {
                        cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@imgPath", lblimagepath.Text);
                        cmd.Parameters.AddWithValue("@imgBinaryData", (object)imgdata);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    lblmsg.Text = "";
                    //Response.Redirect("PdfReportConfig.aspx");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PDF Configuration Added!!')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocationz1('PDF Configuration Added!!');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalz1();", true);
                    fillGrid();
                }
                else
                {
                    byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(lblimagepath.Text));
                    using (SqlCommand cmd = new SqlCommand("update tblPdfReportConfiguration set LocationId=@LocationId , imgPath=@imgPath , imgBinaryData=@imgBinaryData , address=@address where pdfconfigID=@pdfconfigID", con))
                    {
                        cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@imgPath", lblimagepath.Text);
                        cmd.Parameters.AddWithValue("@imgBinaryData", (object)imgdata);
                        cmd.Parameters.AddWithValue("@pdfconfigID", dt.Rows[0].ItemArray[0]);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    lblmsg.Text = "";
                    Session["pdfconfigid"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PDF Configuration Updated!!')", true);
                    fillGrid();
                    //Response.Redirect("PdfReportConfig.aspx");

                }
                ddlLocation.SelectedIndex = 0;
                txtAddress.Text = "";
                Imgcompanylogo.ImageUrl = "";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Address exceeds limit of 500 words')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "btnSubmit_Click", path);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Something Went Wrong')", true);
            return;
        }
    }

    private int fetchtoppdfconfigID()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select Count(*) from tblPdfReportConfiguration", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return (Convert.ToInt32(dt.Rows[0].ItemArray[0]) + 1);
            }
            else
            {
                return 1;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "fetchtoppdfconfigID", path);
            return 0;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("PdfReportConfig.aspx");
    }
    protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btnupdates_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("delete from tblPdfReportConfiguration where LocationId=@LocationId", con))
            {
                cmd.Parameters.AddWithValue("@LocationId", Session["locID"]);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            Response.Redirect("PdfReportConfig.aspx");
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "PdfReportConfig.aspx", "btnupdates_Click", path);
        }

    }



    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
    }
}