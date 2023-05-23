using ECommerce.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    static String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    SqlConnection conn = new SqlConnection(strConnString);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    if (userAuthorize((int)pages.UserManagement, Session["userid"].ToString()))
                    {
                        Page.DataBind();
                    CompanyImg.Src = "images/" + _Logo;
                    fillDropDowns();
                    grid_view();
                    container1.Visible = true;
                    container2.Visible = true;
                    container3.Visible = false;
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

        }
    }
    public void fillDropDowns()
    {
        try
        {
            //fillddlCustodian();
            //fillddlLocation();
            fillddlCaseId();
        }
        catch (Exception ex)
        {

        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    public void fillddlCustodian()
    {
        try
        {
            DataTable dtCustodian = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getUserSpecificCustodian", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
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

        }

    }
    protected void ddlCaseId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCaseId.SelectedValue != "0")
            {
                fillddlAssigneeName(Convert.ToInt32(ddlCaseId.SelectedValue));
                fillddlOrganisationName(Convert.ToInt32(ddlCaseId.SelectedValue));
                grid_view();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }
    public void fillddlAssigneeName(int CaseID)
    {
        try
        {
            DataTable dtAssigneeName = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getAssigneeFullName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CaseID", CaseID);
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
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void fillddlOrganisationName(int CaseID)
    {
        try
        {
            DataTable dtOrganisationName = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getOrganizationFullName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CaseID", CaseID);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtOrganisationName);
                }
            }
            if (dtOrganisationName.Rows.Count > 0)
            {
                ddlOrganisationName.DataSource = dtOrganisationName;
                ddlOrganisationName.DataTextField = "OrganizationFullName";
                ddlOrganisationName.DataValueField = "OrganizationFullName";
                ddlOrganisationName.DataBind();
                ddlOrganisationName.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlOrganisationName.DataSource = null;
                ddlOrganisationName.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //edit operation
        try
        {
            if (e.CommandName == "dit")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                string CaseID = item["CaseID"].Text;
                string CaseAssigneeFullName = item["CaseAssigneeFullName"].Text;
                string ParentOrganizationName = item["ParentOrganizationName"].Text;
                string CaseWorker1Name = item["CaseWorker1Name"].Text;
                string CaseManagerFullName = item["CaseManagerFullName"].Text;
                string ApplicantNames = item["ApplicantNames"].Text;
                string CaseStatus = item["CaseStatus"].Text;
                txtid.Text = id.ToString();
                fillddlCustodian();
                fillddlLocation();
                fillddlTagtypeId();
                fillddlDocumentControllerUSER_ID();
                txtAssigneeName.Text = CaseAssigneeFullName;
                txtCaseID.Text = CaseID;
                txtOrganisationName.Text = ParentOrganizationName;
                txtStatus.Text = CaseStatus;
                container1.Visible = false;
                container2.Visible = false;
                container3.Visible = true;

            }
            if (e.CommandName == "del")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string CaseID = item["CaseID"].Text;
                string CaseAssigneeFullName = item["CaseAssigneeFullName"].Text;
                string ParentOrganizationName = item["ParentOrganizationName"].Text;
                string CaseWorker1Name = item["CaseWorker1Name"].Text;
                string CaseManagerFullName = item["CaseManagerFullName"].Text;
                string ApplicantNames = item["ApplicantNames"].Text;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void fillddlTagtypeId()
    {
        try
        {
            DataTable dtddlTagtypeId = new DataTable();
            using (SqlCommand cmd = new SqlCommand("GetTagType", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtddlTagtypeId);
                }
            }
            if (dtddlTagtypeId.Rows.Count > 0)
            {
                ddlTagtypeId.DataSource = dtddlTagtypeId;
                ddlTagtypeId.DataTextField = "Name";
                ddlTagtypeId.DataValueField = "Id";
                ddlTagtypeId.DataBind();
                ddlTagtypeId.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlTagtypeId.DataSource = null;
                ddlTagtypeId.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void fillddlDocumentControllerUSER_ID()
    {
        try
        {
            DataTable dtDocumentControllerUSER_ID = new DataTable();
            using (SqlCommand cmd = new SqlCommand("getDocumentController", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtDocumentControllerUSER_ID);
                }
            }
            if (dtDocumentControllerUSER_ID.Rows.Count > 0)
            {
                ddlDocumentControllerUSER_ID.DataSource = dtDocumentControllerUSER_ID;
                ddlDocumentControllerUSER_ID.DataTextField = "USER_NAME";
                ddlDocumentControllerUSER_ID.DataValueField = "USER_ID";
                ddlDocumentControllerUSER_ID.DataBind();
                ddlDocumentControllerUSER_ID.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlDocumentControllerUSER_ID.DataSource = null;
                ddlDocumentControllerUSER_ID.DataBind();
            }
        }
        catch (Exception ex)
        {

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

        }
    }
    private void grid_view()
    {
        try
        {
            DataSet ds = new DataSet();
            string qry = "select * from ExcelDataTemp where 1=1";
            if (ddlCaseId.SelectedValue != "0")
            {
                qry += " and CaseID=" + Convert.ToInt32(ddlCaseId.SelectedValue) + "";
            }
            if (ddlAssigneeName.SelectedValue != "0")
            {
                qry += " and CaseAssigneeFullName='" + ddlAssigneeName.SelectedValue + "'";
            }
            if (ddlOrganisationName.SelectedValue != "0")
            {
                qry += " and OrganizationFullName=" + ddlOrganisationName.SelectedValue;
            }
            using (SqlCommand cmd = new SqlCommand(qry, conn))
            {
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
                gvData.DataSource = null;
                DataTable dt = ds.Tables[0];
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
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // gvData.Rebind();
            grid_view();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedValue != "0")
            {
                DataTable dtddlBuildingId = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getSublocationlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocationId", Convert.ToInt32(ddlLocation.SelectedValue));
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtddlBuildingId);
                    }
                }
                if (dtddlBuildingId.Rows.Count > 0)
                {
                    ddlBuildingId.DataSource = dtddlBuildingId;
                    ddlBuildingId.DataTextField = "BuildingName";
                    ddlBuildingId.DataValueField = "BuildingId";
                    ddlBuildingId.DataBind();
                    ddlBuildingId.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
                else
                {
                    ddlBuildingId.DataSource = null;
                    ddlBuildingId.DataBind();
                }
            }
            else
            {
                ddlBuildingId.DataSource = null;
                ddlBuildingId.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlBuildingId_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBuildingId.SelectedValue != "0")
            {
                DataTable dtddlFloorId = new DataTable();
                using (SqlCommand cmd = new SqlCommand("getMinorSublocationlist", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BuildingId", Convert.ToInt32(ddlBuildingId.SelectedValue));
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtddlFloorId);
                    }
                }
                if (dtddlFloorId.Rows.Count > 0)
                {
                    ddlFloorId.DataSource = dtddlFloorId;
                    ddlFloorId.DataTextField = "FloorName";
                    ddlFloorId.DataValueField = "FloorId";
                    ddlFloorId.DataBind();
                    ddlFloorId.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
                else
                {
                    ddlFloorId.DataSource = null;
                    ddlFloorId.DataBind();
                }
            }
            else
            {
                ddlFloorId.DataSource = null;
                ddlFloorId.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustodian.SelectedValue != "0" && ddlLocation.SelectedValue != "0" && ddlDocumentControllerUSER_ID.SelectedValue != "0" && ddlBuildingId.SelectedValue != "0" && ddlFloorId.SelectedValue != "0" && ddlTagtypeId.SelectedValue != "0")
            {
                int check = checkPreviousData();
                if (check == 0)
                {
                    using (SqlCommand cmd = new SqlCommand("pInsertDocumentRequestHdr", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", txtid.Text);
                        cmd.Parameters.AddWithValue("@CustodianId", ddlCustodian.SelectedValue);
                        cmd.Parameters.AddWithValue("@LocationId", ddlLocation.SelectedValue);
                        cmd.Parameters.AddWithValue("@AssigneeName", txtAssigneeName.Text);
                        cmd.Parameters.AddWithValue("@CaseID", txtCaseID.Text);
                        cmd.Parameters.AddWithValue("@OrganisationName", txtOrganisationName.Text);
                        cmd.Parameters.AddWithValue("@USER_ID", ddlDocumentControllerUSER_ID.SelectedValue);
                        cmd.Parameters.AddWithValue("@BuildingId", ddlBuildingId.SelectedValue);
                        cmd.Parameters.AddWithValue("@FloorId", ddlFloorId.SelectedValue);
                        cmd.Parameters.AddWithValue("@TagtypeId", ddlTagtypeId.SelectedValue);
                        //cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                        cmd.Parameters.AddWithValue("@Status", "");
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        Response.Write("<script>alert('Inserted..');window.location = 'DocumentRequest.aspx';</script>");
                    }
                }
                else if (check == 1)
                {
                    Response.Write("<script>alert('Data Already Present!');</script>");
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }
    public int checkPreviousData()
    {
        try
        {
            DataTable dtDocumentRequestHdrData = new DataTable();

            using (SqlCommand cmd = new SqlCommand("checkPreviousDataDocumentRequestHdr", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustodianId", Convert.ToInt32(ddlCustodian.SelectedValue));
                cmd.Parameters.AddWithValue("@LocationId", Convert.ToInt32(ddlLocation.SelectedValue));
                cmd.Parameters.AddWithValue("@AssigneeName", txtAssigneeName.Text);
                cmd.Parameters.AddWithValue("@CaseID", Convert.ToInt32(txtCaseID.Text));
                cmd.Parameters.AddWithValue("@OrganisationName", txtOrganisationName.Text);
                cmd.Parameters.AddWithValue("@USER_ID", Convert.ToInt32(ddlDocumentControllerUSER_ID.SelectedValue));
                cmd.Parameters.AddWithValue("@BuildingId", Convert.ToInt32(ddlBuildingId.SelectedValue));
                cmd.Parameters.AddWithValue("@FloorId", Convert.ToInt32(ddlFloorId.SelectedValue));
                cmd.Parameters.AddWithValue("@TagtypeId", Convert.ToInt32(ddlTagtypeId.SelectedValue));
                //cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dtDocumentRequestHdrData);
                }
            }
            if (dtDocumentRequestHdrData.Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 2;
        }
    }
}