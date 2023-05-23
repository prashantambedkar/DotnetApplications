using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                    if (userAuthorize((int)pages.DocumentRequestt, Session["userid"].ToString()))
                    {
                        Page.DataBind();
                        CompanyImg.Src = "images/" + _Logo;
                        fillData();
                        fillddlTagType();
                        if (Session["Status"].ToString() == "Approved" || Session["Status"].ToString() == "Rejected")
                        {
                            btnupdate.Visible = false;
                            gvData2.MasterTableView.GetColumn("Delete").Display = false;
                            gvData2.MasterTableView.GetColumn("Edit").Display = false;
                            ddlMinorLocation.Enabled = false; ddlMinorSubLocation.Enabled = false;
                            ddlMajorLocation.Enabled = false; ddlTagType.Enabled = false;
                        }
                    }
                    else
                    {
                        Response.Redirect("AcceessError.aspx");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "Page_Load", path);
        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    public void fillddlTagType()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("GetTagType", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    ddlTagType.DataSource = dt;
                    ddlTagType.DataTextField = "Name";
                    ddlTagType.DataValueField = "Id";
                    ddlTagType.DataBind();
                    ddlTagType.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlTagType.SelectedValue = "3";
                }
                else
                {
                    // Response.Redirect("ViewDocumentRequestt.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "fillddlTagType", path);
        }
    }
    public void fillData()
    {
        try
        {
            if (HttpContext.Current.Session["reqHdrid"] != null)
            {
                DataTable dttemp = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select trh.CaseID, cum.CustodianName, trh.AssigneeName,trh.OrganisationName, trh.LocationId,trh.Status,trh.BuildingId,trh.FloorId from tblDocumentRequestHdr as trh left join CustodianMaster as cum on cum.CustodianId = trh.CustodianId where id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", HttpContext.Current.Session["reqHdrid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dttemp);
                    }
                }
                if (dttemp.Rows.Count > 0)
                {
                    lblCaseId.Text = dttemp.Rows[0]["CaseID"].ToString();
                    txtCustodianName.Text = dttemp.Rows[0]["CustodianName"].ToString();
                    //txtAssigneeName.Text = dttemp.Rows[0]["AssigneeName"].ToString();
                    txtOrganisationName.Text = dttemp.Rows[0]["OrganisationName"].ToString();
                    txtStatus.Text = dttemp.Rows[0]["Status"].ToString();
                    if (dttemp.Rows[0]["BuildingId"].ToString().Trim() != null)
                    {
                        Session["BuildingId"] = dttemp.Rows[0]["BuildingId"].ToString();
                    }
                    if (dttemp.Rows[0]["FloorId"].ToString().Trim() != null)
                    {
                        Session["FloorId"] = dttemp.Rows[0]["FloorId"].ToString();
                    }
                    fillLocationData(Convert.ToInt32(dttemp.Rows[0]["LocationId"].ToString()));
                    //if(dttemp.Rows[0]["BuildingId"].ToString().Trim()!=null)
                    //{
                    //    ddlMinorSubLocation.SelectedValue = dttemp.Rows[0]["BuildingId"].ToString();
                    //}
                    //if (dttemp.Rows[0]["FloorId"].ToString().Trim() != null)
                    //{
                    //Session["BuildingId"]
                    //Session["FloorId"]
                    //}

                }
                else
                {
                    Response.Redirect("ViewDocumentRequestt.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "fillData", path);
        }
    }

    public void fillLocationData(int LocationId)
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
                //txtMajorLocation.Text = dttemp.Rows[0]["LocationName"].ToString();
                ddlMajorLocation.DataSource = dtLocation;
                ddlMajorLocation.DataTextField = "LocationName";
                ddlMajorLocation.DataValueField = "LocationId";
                ddlMajorLocation.DataBind();
                ddlMajorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                ddlMajorLocation.SelectedValue = LocationId.ToString();
                fillddlMinorLocation(Convert.ToInt32(LocationId));

            }
            else
            {
                Response.Redirect("ViewDocumentRequestt.aspx");
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "fillLocationData", path);
        }
    }

    public void fillddlMinorLocation(int LocationId)
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getbuilding", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationId", LocationId);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    ddlMinorLocation.DataSource = dt;
                    ddlMinorLocation.DataTextField = "BuildingName";
                    ddlMinorLocation.DataValueField = "BuildingId";
                    ddlMinorLocation.DataBind();
                    ddlMinorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    //Session["BuildingId"]
                    //Session["FloorId"]
                    if (Session["BuildingId"] != null)
                    {
                        ddlMinorLocation.SelectedValue = Session["BuildingId"].ToString();
                        fillfloor(Convert.ToInt32(Session["BuildingId"]));

                    }
                }
                else
                {
                    // Response.Redirect("ViewDocumentRequestt.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "fillddlMinorLocation", path);
        }
    }

    public void fillfloor(int BuildingId)
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getfloorforasset", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                {
                    ddlMinorSubLocation.DataSource = dt;
                    ddlMinorSubLocation.DataTextField = "FloorName";
                    ddlMinorSubLocation.DataValueField = "FloorId";
                    ddlMinorSubLocation.DataBind();
                    ddlMinorSubLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    if (Session["FloorId"] != null)
                    {
                        ddlMinorSubLocation.SelectedValue = Session["FloorId"].ToString();

                    }
                }
                else
                {
                    //Response.Redirect("ViewDocumentRequestt.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "fillfloor", path);
        }
    }
    protected void ddlMinorLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // ddlMinorSubLocation
            if (ddlMinorLocation.SelectedValue != "0")
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getfloorforasset", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BuildingId", ddlMinorLocation.SelectedValue);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ddlMinorSubLocation.DataSource = dt;
                        ddlMinorSubLocation.DataTextField = "FloorName";
                        ddlMinorSubLocation.DataValueField = "FloorId";
                        ddlMinorSubLocation.DataBind();
                        ddlMinorSubLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    }
                    else
                    {
                        //Response.Redirect("ViewDocumentRequestt.aspx");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "ddlMinorLocation_SelectedIndexChanged", path);
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlTagType.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Tag Type')", true);
                return;
            }
            if (ddlMajorLocation.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Major Location')", true);
                return;
            }
            if (ddlMinorLocation.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Minor Location')", true);
                return;
            }
            if (ddlMinorSubLocation.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select Minor Sub Location')", true);
                return;
            }
            if (ddlTagType.SelectedValue != "0" && ddlMajorLocation.SelectedValue != "0" && ddlMinorLocation.SelectedValue != "0" && ddlMinorSubLocation.SelectedValue != "0")
            {
                using (SqlCommand cmd = new SqlCommand("updateRecords", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tblDocumentRequestHdrid", HttpContext.Current.Session["reqHdrid"]);
                    cmd.Parameters.AddWithValue("@LocationId", ddlMajorLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@BuildingId", ddlMinorLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@FloorId", ddlMinorSubLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@TagtypeId", ddlTagType.SelectedValue);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ddlTagType.Enabled = ddlMajorLocation.Enabled = ddlMinorLocation.Enabled = ddlMinorSubLocation.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal2();", true);
                    btnupdate.Enabled = false;
                    //Response.Redirect("ViewDocumentRequest.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "btnupdate_Click", path);
        }
    }

    protected void gvData2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            grid_view2();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "gvData2_NeedDataSource", path);
        }
    }
    protected void gv_data2_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //edit operation
        try
        {
            if (e.CommandName == "dit")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                HttpContext.Current.Session["rowid"] = id;
                string idd = id.ToString();
                string CategoryName = item["CategoryName"].Text;
                string Qty = item["Qty"].Text;
                string Remark = item["Remark"].Text;
                string Name = item["Name"].Text;
                if (Remark.Contains("&nbsp;"))
                {
                    Remark = "";
                }
                if (Name.Contains("&nbsp;"))
                {
                    Name = "";
                }
                Session["Namemodal"] = Name;
                // lblcat.Text = CategoryName;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforCategory('" + CategoryName + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp2", "setvalueforid('" + idd + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp3", "setvalueforqty('" + Qty + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp4", "setvalueforRemark('" + Remark + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp5", "setvalueforName('" + Name + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            if (e.CommandName == "del")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                Session["itemidd"] = id;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal1();", true);



            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "gv_data2_ItemCommand", path);
        }
    }
    private void grid_view2()
    {
        try
        {
            if (HttpContext.Current.Session["reqHdrid"] != null)
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetails1", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@reqHdrid", HttpContext.Current.Session["reqHdrid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
                if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
                {
                    gvData2.DataSource = string.Empty;
                }
                else
                {
                    gvData2.DataSource = null;
                    DataTable dt = ds.Tables[0];
                    DataView myView = null;
                    myView = dt.DefaultView;
                    gvData2.DataSource = myView;
                    gvData2.DataBind();
                    //gvData.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "grid_view2", path);
        }
    }

    protected void ddlMajorLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMajorLocation.SelectedValue != "0")
            {
                fillddlMinorLocation(Convert.ToInt32(ddlMajorLocation.SelectedValue));
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "ddlMajorLocation_SelectedIndexChanged", path);
        }
    }

    protected void btnupdates_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("update tblDocumentRequestDetailsFInal set Qty=@qty,Remark=@Remark,Name=@Name where id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@qty", txtqty.Text);
                cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);
                cmd.Parameters.AddWithValue("@Name", Session["Namemodal"].ToString());
                cmd.Parameters.AddWithValue("@id", HttpContext.Current.Session["rowid"]);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Response.Redirect("ViewDocumentRequest.aspx");
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequest.aspx", "btnupdates_Click", path);
        }
    }

    protected void gvData2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    var item = (GridDataItem)e.Item;
        //    if (Session["Status"].ToString() == "Approved" || Session["Status"].ToString() == "Rejected")
        //    {
        //        item["Delete"].Visible = true;
        //        item["Edit"].Visible = true;
        //    }
        //}
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        using (SqlCommand cmd = new SqlCommand("delete from tblDocumentRequestDetailsFinal where id=@id;", conn))
        {
            cmd.Parameters.AddWithValue("@id", Session["itemidd"]);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            grid_view2();
        }
    }
}