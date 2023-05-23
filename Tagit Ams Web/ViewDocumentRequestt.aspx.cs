using ECommerce.Common;
using Serco;
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
    CompanyBL objcomp = new CompanyBL();
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
                    if (userAuthorize((int)pages.ViewDocumentRequestt, Session["userid"].ToString()))
                    {
                        Page.DataBind();
                        CompanyImg.Src = "images/" + _Logo;
                        HttpContext.Current.Session["reqHdrid"] = null;

                        grid_view();

                        // gvData_Init(sender, e);
                    }
                    else
                    {
                        //ModalPopupExtender1.Show();
                        Response.Redirect("AcceessError.aspx");
                    }
                    //panel3.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "Page_Load", path);

        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gvData_Init", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gvData_NeedDataSource", path);
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
                HttpContext.Current.Session["reqHdrid"] = id;
                string BuildingName = item["BuildingName"].Text;
                string FloorName = item["FloorName"].Text;
                if (BuildingName == "&nbsp;")
                {
                    BuildingName = "";
                }
                if (FloorName == "&nbsp;")
                {
                    FloorName = "";
                }
                string Status = item["Status"].Text;
                Session["Status"] = Status;
                HttpContext.Current.Session["BuildingName"] = BuildingName;
                HttpContext.Current.Session["FloorName"] = FloorName;
                //fillMinorLocation(id);
                Response.Redirect("ViewDocumentRequest.aspx");
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            if (e.CommandName == "tup")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                InsertAssetData(id);
                // Response.Write("<script>alert('Request Approved..');window.location = 'ViewDocumentRequestt.aspx';</script>");
                //  Response.Redirect("ViewDocumentRequestt.aspx");
                grid_view();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal1();", true);
            }
            if (e.CommandName == "tdn")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = Convert.ToInt32(item["id"].Text);
                using (SqlCommand cmd = new SqlCommand("updateRequests", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentRequestDetailsID", 0);
                    cmd.Parameters.AddWithValue("@DocumentRequestHdrID", id);
                    cmd.Parameters.AddWithValue("@type", 2);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //Response.Write("<script>alert('Request Rejected..');window.location = 'ViewDocumentRequestt.aspx';</script>");
                    Response.Redirect("ViewDocumentRequestt.aspx");
                    //Response.Write("<script>Rejectedmsgpopup();</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gv_data_ItemCommand", path);
        }
    }
    public void InsertAssetData(int id)
    {
        try
        {
            DataTable dttblDocumentRequestHdr = new DataTable();
            using (SqlCommand cmd = new SqlCommand("fetchdataforasset_1", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dttblDocumentRequestHdr);
                }
            }
            for (int i = 0; i < dttblDocumentRequestHdr.Rows.Count; i++)
            {
                DataTable dttblDocumentRequestDetailsFinal = new DataTable();
                using (SqlCommand cmd1 = new SqlCommand("fetchdataforasset_2", conn))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@ReqHdrId", id);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd1))
                    {
                        adp.Fill(dttblDocumentRequestDetailsFinal);
                    }
                }
                for (int j = 0; j < dttblDocumentRequestDetailsFinal.Rows.Count; j++)
                {//
                    for (int z = 0; z < Convert.ToInt32(dttblDocumentRequestDetailsFinal.Rows[j]["Qty"]); z++)
                    {
                        int checkcatecode = catcodeverify(Convert.ToInt32(dttblDocumentRequestDetailsFinal.Rows[j]["CategoryId"]));
                        string leftPrefix = "0";
                        string rightPrefix = "000000";
                        if (checkcatecode == 1)
                        {
                            leftPrefix = "00";
                            rightPrefix = "000000";
                        }
                        else if (checkcatecode == 2)
                        {
                            leftPrefix = "0";
                            rightPrefix = "000000";
                        }
                        else if (checkcatecode == 3)
                        {
                            leftPrefix = "0";
                            rightPrefix = "0000";
                        }
                        else if (checkcatecode == 4)
                        {
                            leftPrefix = "0";
                            rightPrefix = "000";
                        }
                        DataTable dtAssetCode = new DataTable();
                        using (SqlCommand cmd2 = new SqlCommand("select ('" + leftPrefix + "'+Convert(varchar(100),(@CategoryId))+RIGHT('" + rightPrefix + "' + CONVERT(VARCHAR(9), (SELECT max(AssetId) + 1 FROM AssetMaster)), 9))", conn))
                        {
                            cmd2.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(dttblDocumentRequestDetailsFinal.Rows[j]["CategoryId"]));
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd2))
                            {
                                adp.Fill(dtAssetCode);
                            }
                        }
                        if (dtAssetCode.Rows.Count > 0)
                        {
                            string AssetCode = dtAssetCode.Rows[0].ItemArray[0].ToString();
                            DataTable doccontrolerName = new DataTable();
                            string USER_NAME = "";
                            using (SqlCommand cmd = new SqlCommand("select USER_NAME from TBL_USERMST where USER_ID=@USER_ID", conn))
                            {
                                cmd.Parameters.AddWithValue("@USER_ID", dttblDocumentRequestHdr.Rows[i]["USER_ID"]);
                                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                                {
                                    adp.Fill(doccontrolerName);
                                }
                            }
                            if (doccontrolerName.Rows.Count > 0)
                            {
                                USER_NAME = doccontrolerName.Rows[0]["USER_NAME"].ToString();
                            }
                            else
                            {
                                USER_NAME = "ADMIN";
                            }

                            using (SqlCommand cmd3 = new SqlCommand("newAssetInsertData", conn))
                            {
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@DocumentRequestHdrID", id);
                                cmd3.Parameters.AddWithValue("@AssetCode", AssetCode);//CasePersonAssociation
                                cmd3.Parameters.AddWithValue("@CasePersonAssociation", dttblDocumentRequestDetailsFinal.Rows[j]["CasePersonAssociation"]);
                                cmd3.Parameters.AddWithValue("@CategoryId", Convert.ToInt32(dttblDocumentRequestDetailsFinal.Rows[j]["CategoryId"]));
                                cmd3.Parameters.AddWithValue("@BuildingId", Convert.ToInt32(dttblDocumentRequestHdr.Rows[i]["BuildingId"]));
                                cmd3.Parameters.AddWithValue("@FloorId", Convert.ToInt32(dttblDocumentRequestHdr.Rows[i]["FloorId"]));
                                cmd3.Parameters.AddWithValue("@LocationId", Convert.ToInt32(dttblDocumentRequestHdr.Rows[i]["LocationId"]));
                                cmd3.Parameters.AddWithValue("@CustodianId", Convert.ToInt32(dttblDocumentRequestHdr.Rows[i]["CustodianId"]));
                                cmd3.Parameters.AddWithValue("@Quantity", 1);
                                cmd3.Parameters.AddWithValue("@AssignDate", dttblDocumentRequestDetailsFinal.Rows[j]["CaseStampDate"]);
                                cmd3.Parameters.AddWithValue("@CaseID", dttblDocumentRequestHdr.Rows[i]["CaseID"]);
                                cmd3.Parameters.AddWithValue("@AssigneeName", dttblDocumentRequestHdr.Rows[i]["AssigneeName"]);
                                cmd3.Parameters.AddWithValue("@OrganisationName", dttblDocumentRequestHdr.Rows[i]["OrganisationName"]);
                                cmd3.Parameters.AddWithValue("@USER_ID", USER_NAME);
                                cmd3.Parameters.AddWithValue("@CaseManagerFullName", dttblDocumentRequestDetailsFinal.Rows[j]["CaseManagerFullName"]);
                                cmd3.Parameters.AddWithValue("@CaseManagerEmailAddress", dttblDocumentRequestDetailsFinal.Rows[j]["CaseManagerEmailAddress"]);
                                cmd3.Parameters.AddWithValue("@CaseWorker1Name", dttblDocumentRequestDetailsFinal.Rows[j]["CaseWorker1Name"]);
                                cmd3.Parameters.AddWithValue("@CaseWorker1EmailAddress", dttblDocumentRequestDetailsFinal.Rows[j]["CaseWorker1EmailAddress"]);
                                cmd3.Parameters.AddWithValue("@CaseStatus", dttblDocumentRequestDetailsFinal.Rows[j]["CaseStatus"]);
                                cmd3.Parameters.AddWithValue("@ApplicantNames", dttblDocumentRequestDetailsFinal.Rows[j]["ApplicantNames"]);
                                cmd3.Parameters.AddWithValue("@TagName", dttblDocumentRequestHdr.Rows[i]["Name"]);
                                conn.Open();
                                cmd3.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "InsertAssetData", path);
        }
    }
    public int catcodeverify(int Cateid)
    {
        try
        {
            int count = 0;
            while (Cateid > 0)
            {
                Cateid = Cateid / 10;
                count++;
            }
            return count;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "catcodeverify", path);
            return 0;
        }
    }
    private void grid_view()
    {
        try
        {
            DataSet ds = new DataSet();
            if (Session["locnamenewdata"].ToString() != "" && Session["typenewdata"].ToString() != "")
            {
                string FROMDATE = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                string TODATE = System.DateTime.Now.ToString("MM/dd/yyyy");
                using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetailsnewdata", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocationName", Session["locnamenewdata"].ToString());
                    cmd.Parameters.AddWithValue("@Status", Session["typenewdata"]);
                    cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                    cmd.Parameters.AddWithValue("@TODATE", TODATE);
                    cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
                Session["locnamenewdata"] = Session["typenewdata"] = "";
            }
            else if (Session["locnamenewdata"].ToString() != "" && Session["typenewdata"].ToString() == "")
            {
                string FROMDATE = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                string TODATE = System.DateTime.Now.ToString("MM/dd/yyyy");
                using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetailsnewdata1", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocationName", Session["locnamenewdata"].ToString());
                    cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                    cmd.Parameters.AddWithValue("@TODATE", TODATE);
                    cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
                Session["locnamenewdata"] = Session["typenewdata"] = "";
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "grid_view", path);
        }
    }



    protected void btnsubmit_Click(object sender, EventArgs e)
    {


    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                var item = (GridDataItem)e.Item;
                if (item["Status"].Text == "Waiting for Approval")
                {
                    item["Status"].ForeColor = System.Drawing.Color.CornflowerBlue;
                    item["Status"].Font.Bold = true;
                    //item["Approve"].Visible = true;
                    //item["Reject"].Visible = true;
                    item["Approve"].Enabled = true;
                    item["Reject"].Enabled = true;
                    item["Edit"].Visible = true;
                    item["Status"].HorizontalAlign = HorizontalAlign.Center;
                    item["Status"].VerticalAlign = VerticalAlign.Middle;
                    item["Status"].Text = "PENDING";

                }
                else if (item["Status"].Text == "Rejected")
                {
                    item["Status"].ForeColor = System.Drawing.Color.Red;
                    item["Status"].Font.Bold = true;
                    item["Approve"].Enabled = false;
                    //item["Edit"].Enabled = false;
                    item["Reject"].Enabled = false;
                    item["Approve"].Text = " ";
                    item["Reject"].Text = " ";
                    //item["Approve"].Visible = false;
                    //item["Reject"].Visible = false;
                }
                else
                {
                    item["Status"].ForeColor = System.Drawing.Color.Green;
                    item["Status"].Font.Bold = true;
                    item["Approve"].Enabled = false;
                    item["Reject"].Enabled = false;
                    item["Approve"].Text = " ";
                    item["Reject"].Text = " ";
                    //item["Approve"].Visible = false;
                    //item["Reject"].Visible = false;
                }
                if (item["BuildingName"].Text == "&nbsp;")
                {
                    item["Approve"].Enabled = false;
                    item["Reject"].Enabled = false;
                    item["Approve"].Text = " ";
                    item["Reject"].Text = " ";
                    //item["Approve"].Visible = false;
                    //item["Reject"].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "ViewDocumentRequestt.aspx", "gvData_ItemDataBound", path);
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
}
