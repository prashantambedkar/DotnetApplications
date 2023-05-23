using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    public static string path = "";
    public static DataTable dt = new DataTable();
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
                Page.DataBind();
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                grid_view();
                //Thread.Sleep(1000);
                if (HttpContext.Current.Session["VAL"] != null)
                {
                    fillGridwithVal(HttpContext.Current.Session["VAL"].ToString());
                    HttpContext.Current.Session["VAL"] = null;
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "Page_Load", path);

        }
    }
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "dit")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string clientName = item["clientName"].Text;
                //int id = Convert.ToInt32(item["id"].Text);
                //GridDataItem item = (GridDataItem)e.Item;
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = clientName;
                HttpContext.Current.Session["Dashboard_Filtered_Location_statpage"] = "1";
                HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"] = "";
                Response.Redirect("Asset.aspx");
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gv_data_ItemCommand", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Asset.aspx", "gvData_NeedDataSource", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "gvData_Init", path);
        }
    }

    public void grid_view()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataSet ds = new DataSet();
            dt = new DataTable();
            //SqlCommand cmd = new SqlCommand("select top 10 AM.column3 as clientName, Count(*) as countClientData from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where CP.UserID = @UserID and LP.LocationID !=8 and LP.UserID = @UserID group by   AM.column3 order by countClientData desc", con);
            //cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
            SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_top10Clients2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
            cmd.Parameters.AddWithValue("@LocationId", null);
            cmd.Parameters.AddWithValue("@BuildingId", null);
            cmd.Parameters.AddWithValue("@FloorId", null);
            cmd.Parameters.AddWithValue("@Column1", null);
            cmd.Parameters.AddWithValue("@Column2", null);
            cmd.Parameters.AddWithValue("@Column3", null);
            cmd.Parameters.AddWithValue("@Column5", null);
            cmd.Parameters.AddWithValue("@CustodianId", null);
            // cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            //DataTable dt = new DataTable();
            da.SelectCommand.CommandTimeout = 600;
            da.Fill(dt);
            da.Fill(ds);
            //using (SqlCommand cmd = new SqlCommand("Graphtop10ClientsData", con))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@UserID", Session["userid"]);
            //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            //    {
            //        adp.Fill(ds);
            //        adp.Fill(dt);
            //    }
            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvData.DataSource = ds;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "grid_view", path);
        }
    }
    public void fillGridwithVal(string clientName)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataSet ds = new DataSet();
            dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select AM.column3 as clientName, Count(*) as countClientData from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where CP.UserID = @UserID and LP.LocationID !=8 and LP.UserID = @UserID and AM.column3=" + clientName + " group by AM.column3 order by countClientData desc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                    adp.Fill(dt);
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvData.DataSource = ds;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "fillGridwithVal", path);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvData.Items.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "ClientsReport");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + Session["userid"] + "_" + DateTime.Now + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "topClients.aspx", "Button3_Click", path);
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