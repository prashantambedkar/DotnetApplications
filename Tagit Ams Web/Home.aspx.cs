using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.DataAccess;
using System.Web.UI.DataVisualization.Charting;
using Serco;
using log4net;
using log4net.Config;
using ECommerce.Common;
using System.Text;
//using ECommerce.DataAccess;

public partial class adminx_demodashboard : System.Web.UI.Page
{
    String strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public String _Logo = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"];
    public static string path = "";
    public static int LocationID = 0;
    public static int BuildingID = 0;
    public static int FloorId = 0;
    public static string Column1FCNumber = "";
    public static string CaseManagerName = "";
    public static string Column2AssigneeName = "";
    public static string Column3ClientName = "";
    public static int CustodianId = 0;
    public static int loadstatusGetGeoLocationWiseStock = 0;
    public static int loadstatusGetBarchartData = 0;
    public static int loadstatusGetTagVsPrint = 0;
    public static int loadstatusbindlocation = 0;
    CompanyBL objcomp = new CompanyBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {

            if (HttpContext.Current.Session["userid"] != null)
            {
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                if (!IsPostBack)
                {
                    Logging.WriteLog(Logging.LogLevelL4N.INFO, "Home Page Started", "Page_Load", "Home_adminx_demodashboard");
                    // objcomp.Insertlogmaster("Home:Page Loading Started");
                    LocationID = BuildingID = FloorId = CustodianId = loadstatusGetGeoLocationWiseStock = loadstatusGetBarchartData = loadstatusGetTagVsPrint = loadstatusbindlocation = 0;
                    Column1FCNumber = Column2AssigneeName = Column3ClientName = "";
                    loadstatusGetGeoLocationWiseStock = 0;
                    loadstatusGetBarchartData = 0;
                    loadstatusGetTagVsPrint = 0;
                    loadstatusbindlocation = 0;
                    Page.DataBind();
                    HdnLocation.Value = Location;
                    CompanyImg.Src = "images/" + _Logo;
                    //fillGraphData();
                    fillCollapse();
                    HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                    HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                    HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                    HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                    HttpContext.Current.Session["LocationID"] = "0";
                    HttpContext.Current.Session["BuildingID"] = "0";
                    HttpContext.Current.Session["FloorId"] = "0";
                    HttpContext.Current.Session["Column1FCNumber"] = "0";
                    HttpContext.Current.Session["CustodianId"] = "0";
                    HttpContext.Current.Session["Column3ClientName"] = "0";
                    HttpContext.Current.Session["Column2AssigneeName"] = "0";
                    HttpContext.Current.Session["Column5CaseManager"] = "0";
                    HttpContext.Current.Session["Column7CaseWorker1"] = "0";
                    // BindChart4();
                    fillMonthWiseReport();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Javascript", "javascript:contentHide(); ", true);
                    //objcomp.Insertlogmaster("Home:Page Loading finished");
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "Page_Load", "Home_adminx_demodashboard");

            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "Page_Load", "Home_adminx_demodashboard");

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "Page_Load", path);
        }
    }
    StringBuilder sv1 = new StringBuilder();
    public void fillMonthWiseReport()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        DataTable dtUserLocations = new DataTable();
        DataTable dtfinal = new DataTable();
        string FROMDATE = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
        string TODATE = System.DateTime.Now.ToString("MM/dd/yyyy");
        using (SqlCommand cmd = new SqlCommand("select lm.LocationId,lm.LocationName from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=@UserID", con))
        {
            cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                adp.Fill(dtUserLocations);
            }
        }
        if (dtUserLocations.Rows.Count > 0)
        {
            dtfinal.Columns.Add("Location Name");
            dtfinal.Columns.Add("New Request");
            dtfinal.Columns.Add("Approved");
            dtfinal.Columns.Add("Rejected");
            dtfinal.Columns.Add("Waiting");
            DataRow dr = null;
            int Totalnewrequest = 0, totalapproved = 0, totalrejected = 0, totalwaiting = 0;
            string[] status = { "ALL", "Waiting for Approval", "Approved", "Rejected" };
            string locname = "", newreqcount = "0", approvedcount = "0", rejectedcount = "0", waitingcount = "0";
            for (int i = 0; i < dtUserLocations.Rows.Count; i++)
            {
                DataTable fnldt = new DataTable();
                //new code
                for (int i1 = 0; i1 < status.Count(); i1++)
                {
                    DataTable newfnldt = new DataTable();
                    if (status[i1] == "ALL")
                    {
                        string FROMDATE_ = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                        string TODATE_ = System.DateTime.Now.ToString("MM/dd/yyyy");
                        using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetailsnewdata1", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@LocationName", dtUserLocations.Rows[i]["LocationName"].ToString());
                            cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE_);
                            cmd.Parameters.AddWithValue("@TODATE", TODATE_);
                            cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.Fill(newfnldt);
                            }
                        }
                    }
                    else
                    {
                        string FROMDATE_ = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                        string TODATE_ = System.DateTime.Now.ToString("MM/dd/yyyy");
                        using (SqlCommand cmd = new SqlCommand("ViewDocumentRequestHdrDetailsnewdata", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@LocationName", dtUserLocations.Rows[i]["LocationName"].ToString());
                            cmd.Parameters.AddWithValue("@Status", status[i1].ToString());
                            cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE_);
                            cmd.Parameters.AddWithValue("@TODATE", TODATE_);
                            cmd.Parameters.AddWithValue("@USER_ID", HttpContext.Current.Session["userid"]);
                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.Fill(newfnldt);
                            }
                        }
                    }
                    if (status[i1] == "ALL")
                    {
                        newreqcount = newfnldt.Rows.Count.ToString();
                        Totalnewrequest = Totalnewrequest + Convert.ToInt32(newreqcount);
                    }
                    if (status[i1] == "Approved")
                    {
                        approvedcount = newfnldt.Rows.Count.ToString();
                        totalapproved = totalapproved + Convert.ToInt32(approvedcount);
                    }
                    if (status[i1] == "Rejected")
                    {
                        rejectedcount = newfnldt.Rows.Count.ToString();
                        totalrejected = totalrejected + Convert.ToInt32(rejectedcount);
                    }
                    if (status[i1] == "Waiting for Approval")
                    {
                        waitingcount = newfnldt.Rows.Count.ToString();
                        totalwaiting = totalwaiting + Convert.ToInt32(waitingcount);
                    }

                }
                if (newreqcount != "0")
                {
                    dr = dtfinal.NewRow();
                    dr["Location Name"] = dtUserLocations.Rows[i]["LocationName"].ToString();
                    dr["New Request"] = newreqcount;
                    dr["Approved"] = approvedcount;
                    dr["Rejected"] = rejectedcount;
                    dr["Waiting"] = waitingcount;
                    dtfinal.Rows.Add(dr);
                }
                locname = ""; newreqcount = "0"; approvedcount = "0"; rejectedcount = "0"; waitingcount = "0";

                //new code






                //using (SqlCommand cmd1 = new SqlCommand("monthWiseReportforDashboard", con))
                //{
                //    cmd1.CommandType = CommandType.StoredProcedure;
                //    cmd1.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                //    cmd1.Parameters.AddWithValue("@TODATE", TODATE);
                //    cmd1.Parameters.AddWithValue("@LocationId", Convert.ToInt32(dtUserLocations.Rows[i]["LocationId"]));
                //    cmd1.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd1))
                //    {
                //        adp.Fill(fnldt);
                //    }
                //}

                //for (int j = 0; j < fnldt.Rows.Count; j++)
                //{
                //    dr = dtfinal.NewRow();

                //    if (fnldt.Rows[j]["statustype"].ToString() == "ALL")
                //    {
                //        newreqcount = fnldt.Rows[j]["count"].ToString();
                //        Totalnewrequest = Totalnewrequest + Convert.ToInt32(fnldt.Rows[j]["count"]);
                //    }


                //    if (fnldt.Rows[j]["statustype"].ToString() == "Waiting for Approval")
                //    {
                //        waitingcount = fnldt.Rows[j]["count"].ToString();
                //        totalwaiting = totalwaiting + Convert.ToInt32(fnldt.Rows[j]["count"]);
                //    }

                //    if (fnldt.Rows[j]["statustype"].ToString() == "Approved")
                //    {
                //        approvedcount = fnldt.Rows[j]["count"].ToString();
                //        totalapproved = totalapproved + Convert.ToInt32(fnldt.Rows[j]["count"]);
                //    }

                //    if (fnldt.Rows[j]["statustype"].ToString() == "Rejected")
                //    {
                //        rejectedcount = fnldt.Rows[j]["count"].ToString();
                //        totalrejected = totalrejected + Convert.ToInt32(fnldt.Rows[j]["count"]);
                //    }
                //    if ((fnldt.Rows.Count - 1) == j)
                //    {
                //        dr["Location Name"] = fnldt.Rows[j]["LocationName"].ToString();
                //        dr["New Request"] = newreqcount;
                //        dr["Approved"] = approvedcount;
                //        dr["Rejected"] = rejectedcount;
                //        dr["Waiting"] = waitingcount;
                //        dtfinal.Rows.Add(dr);
                //        locname = ""; newreqcount = "0"; approvedcount = "0"; rejectedcount = "0"; waitingcount = "0";
                //    }
                //}

            }
            dr = dtfinal.NewRow();
            dr["Location Name"] = "TOTAL";
            dr["New Request"] = Totalnewrequest.ToString();
            dr["Approved"] = totalapproved.ToString();
            dr["Rejected"] = totalrejected.ToString();
            dr["Waiting"] = totalwaiting.ToString();
            dtfinal.Rows.Add(dr);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table'>");
            sb.Append("<thead class='thead - light' align='center'><tr align='center'><th scope='col' align='center'>#</th>");
            sb.Append("<th scope='col' style='font-family: Calibri; font-size: 10pt; text-align: center' align='center'>&nbsp;&nbsp;&nbsp;&nbsp;LocationName</th>");
            sb.Append("<th scope='col' style='font-family: Calibri; font-size: 10pt; text-align: center' align='center'>&nbsp;&nbsp;&nbsp;&nbsp;NewRequest</th>");
            sb.Append("<th scope='col' style='font-family: Calibri; font-size: 10pt; text-align: center' align='center'>&nbsp;&nbsp;&nbsp;&nbsp;Approved</th>");
            sb.Append("<th scope='col' style='font-family: Calibri; font-size: 10pt; text-align: center' align='center'>&nbsp;&nbsp;&nbsp;&nbsp;Rejected</th>");
            sb.Append("</tr></thead><tbody>");
            //logical code from here
            //foreach (DataRow row in dtfinal.Rows)
            //{
            //    sb.Append("<tr>");
            //    foreach (DataColumn column in dtfinal.Columns)
            //    {
            //        sb.Append("<td style='width:100px;border: 1px solid #ccc'>" + row[column.ColumnName].ToString() + "</td>");
            //    }
            //    sb.Append("</tr>");
            //}

            for (int l = 0; l < dtfinal.Rows.Count; l++)
            {

                if (l == dtfinal.Rows.Count - 1)
                {
                    sb.Append("<tr align='center'>");
                    sb.Append("<th scope='row' style='font-family: Calibri; font-size: 10pt; text-align: center'></th>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><b>" + dtfinal.Rows[l]["Location Name"].ToString() + "</b></td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><b>" + dtfinal.Rows[l]["New Request"].ToString() + "</b></td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><b>" + dtfinal.Rows[l]["Approved"].ToString() + "</b></td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><b>" + dtfinal.Rows[l]["Rejected"].ToString() + "</b></td>");
                    sb.Append("</tr>");
                }
                else
                {

                    sb.Append("<tr align='center'>");
                    sb.Append("<th scope='row' style='font-family: Calibri; font-size: 10pt; text-align: center'>" + (l + 1).ToString() + "</th>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'>" + dtfinal.Rows[l]["Location Name"].ToString() + "</td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><a id='lnkBtnEdit' runat='server' href='testvalue.aspx?locationname=" + dtfinal.Rows[l]["Location Name"].ToString() + "&type=ALL'>" + dtfinal.Rows[l]["New Request"].ToString() + "</a></td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><a id='lnkBtnEdit' runat='server' href='testvalue.aspx?locationname=" + dtfinal.Rows[l]["Location Name"].ToString() + "&type=Approved'>" + dtfinal.Rows[l]["Approved"].ToString() + "</a></td>");
                    sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'><a id='lnkBtnEdit' runat='server' href='testvalue.aspx?locationname=" + dtfinal.Rows[l]["Location Name"].ToString() + "&type=Rejected'>" + dtfinal.Rows[l]["Rejected"].ToString() + "</a></td>");
                    sb.Append("</tr>");
                }
            }


            //sb.Append("<tr align='center'>");
            //sb.Append("<th scope='row' style='font-family: Calibri; font-size: 10pt; text-align: center'>1</th>");
            //sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'>DIC-DC</td>");
            //sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'>20</td>");
            //sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'>15</td>");
            //sb.Append("<td align='center' style='font-family: Calibri; font-size: 10pt; text-align: center'>5</td>");
            //sb.Append("</tr>");
            //end logical code
            sb.Append("</tbody>");
            sb.Append("</table>");
            ltTable.Text = sb.ToString();
        }
    }

    public void MyFuncion_Click(string name)
    {
        try
        {
            //HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = name;

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "MyFuncion_Click", path);
        }
    }
    [WebMethod]
    public static void Dashboard_Filtered_Location(string name)
    {
        try
        {
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
            HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = name;
            string Column7CaseWorker = "";
            if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
            {
                Column7CaseWorker = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
            }
            HttpContext.Current.Session["Dashboard_Filtered_Location_statpage"] = "0";
            HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"] = Column7CaseWorker;
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "Dashboard_Filtered_Location", path);
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
        }
    }
    [WebMethod]
    public static void Dashboard_Filtered_LocationV2(string name)
    {
        try
        {
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
            HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = name;
            string Column7CaseWorker = "";
            if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
            {
                Column7CaseWorker = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
            }
            HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"] = Column7CaseWorker;
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "Dashboard_Filtered_LocationV2", path);
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
        }

    }
    [WebMethod]
    public static void createSessionofHealthData(string name)
    {
        try
        {
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
            HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
            HttpContext.Current.Session["SessionofHealthDataColumn9"] = name;

            HttpContext.Current.Session["SessionofCaseManager"] = HttpContext.Current.Session["Column5CaseManager"];
            string Column7CaseWorker1 = "";
            if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
            {
                Column7CaseWorker1 = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
            }
            HttpContext.Current.Session["SessionofCaseWorker1"] = Column7CaseWorker1;

        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "createSessionofHealthData", "Home_adminx_demodashboard");

            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "createSessionofHealthData", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "createSessionofHealthData", path);
        }
    }
    private void fillCollapse()
    {
        try
        {
            // objcomp.Insertlogmaster("Home:ddl Loading Started");
            fillMajorLocations();
            fillFCNumberonload();
            fillCustodianonload();
            fillCaseManageronload();
            fillCaseWorker1onload();
            //fillClientNameonload();
            //fillAssigneeNameonload();
            // objcomp.Insertlogmaster("Home:ddl Loading finished");
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillCollapse", "Home_adminx_demodashboard");

            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillCollapse", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillCollapse", path);

        }
    }
    public void fillCaseManageronload()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_forcasemanagerddl", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {

                ddlCaseManager.DataSource = dt;
                ddlCaseManager.DataTextField = "CaseManager";
                ddlCaseManager.DataValueField = "CaseManager";
                ddlCaseManager.DataBind();
                ddlCaseManager.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCaseManager.DataSource = null;
                ddlCaseManager.DataBind();
                ddlCaseManager.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
    }
    public void fillCaseWorker1onload()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_forcaseworker1", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {

                ddlCaseWorker1.DataSource = dt;
                ddlCaseWorker1.DataTextField = "CaseWorker1";
                ddlCaseWorker1.DataValueField = "CaseWorker1";
                ddlCaseWorker1.DataBind();
                ddlCaseWorker1.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                ddlCaseWorker1.DataSource = null;
                ddlCaseWorker1.DataBind();
                ddlCaseWorker1.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
        }
    }
    private void fillFCNumberonload()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column1 from AssetMaster order by Column1 asc", con))
                {
                    //cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlFCNumber.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlFCNumber.DataSource = dt;
                    ddlFCNumber.DataTextField = "Column1";
                    ddlFCNumber.DataValueField = "Column1";
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlFCNumber.SelectedIndex = 0;
                }
                else
                {
                    ddlFCNumber.DataSource = null;
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillFCNumberonload", "Home_adminx_demodashboard");

            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillFCNumberonload", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillFCNumberonload", path);
        }
    }
    private void fillCustodianonload()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataSet ds = new DataSet();
                //ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
                using (SqlCommand cmd = new SqlCommand("select cm.CustodianId ,cm.CustodianName from CustodianMaster as cm left join CustodianPermission as cp on cp.CustodianId=cm.CustodianId where cp.UserID=@UserID and cm.Active=1", con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
                DataTable dt = ds.Tables[0];
                //using (SqlCommand cmd = new SqlCommand("select distinct CM.CustodianName as CustodianName,CM.CustodianId from CustodianMaster as CM left join AssetMaster as AM on CM.CustodianId=AM.CustodianID order by CustodianName asc", con))
                //{
                //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                //    {
                //        adp.Fill(dt);
                //    }
                //}
                ddlCustodian.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlCustodian.DataSource = dt;
                    ddlCustodian.DataTextField = "CustodianName";
                    ddlCustodian.DataValueField = "CustodianId";
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.SelectedIndex = 0;
                }
                else
                {
                    ddlCustodian.DataSource = null;
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //    Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillCustodianonload", "Home_adminx_demodashboard");

            //    Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillCustodianonload", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillCustodianonload", path);

        }
    }
    private void fillClientNameonload()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column3 as Column3 from AssetMaster order by Column3 asc", con))
                {

                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlClientName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlClientName.DataSource = dt;
                    ddlClientName.DataTextField = "Column3";
                    ddlClientName.DataValueField = "Column3";
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlClientName.SelectedIndex = 0;
                }
                else
                {
                    ddlClientName.DataSource = null;
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillClientNameonload", path);
        }
    }
    private void fillAssigneeNameonload()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column2 as Column2 from AssetMaster order by Column2 asc", con))
                {
                    // cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlAssigneeName.DataSource = dt;
                    ddlAssigneeName.DataTextField = "Column2";
                    ddlAssigneeName.DataValueField = "Column2";
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlAssigneeName.SelectedIndex = 0;
                }
                else
                {
                    ddlAssigneeName.DataSource = null;
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillAssigneeNameonload", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillAssigneeNameonload", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillAssigneeNameonload", path);
        }

    }
    private void fillMajorLocations()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct lm.LocationName,lm.LocationId from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId and lm.LocationId!=8 where Active=1 and lp.UserID=@UserID order by lm.LocationName asc", con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlMajorLocation.Items.Clear();
                ddlMinorLocation.Items.Clear();
                ddlMinorSubLocation.Items.Clear();
                ddlClientName.Items.Clear();
                ddlCustodian.Items.Clear();
                ddlFCNumber.Items.Clear();
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlMajorLocation.DataSource = dt;
                    ddlMajorLocation.DataTextField = "LocationName";
                    ddlMajorLocation.DataValueField = "LocationId";
                    ddlMajorLocation.DataBind();
                    ddlMajorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
                else
                {
                    ddlMajorLocation.DataSource = null;
                    ddlMajorLocation.DataBind();
                    ddlMajorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillMajorLocations", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillMajorLocations", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillMajorLocations", path);
        }
    }
    private void fillMinorLocation(string LocationId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct BuildingName, BuildingId from BuildingMaster where LocationId=@LocationId order by BuildingName asc", con))
                {
                    //cmd.Parameters.AddWithValue("@CreatedBy", Session["userid"]);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlMinorLocation.Items.Clear();
                ddlMinorSubLocation.Items.Clear();
                // ddlClientName.Items.Clear();
                // ddlCustodian.Items.Clear();
                ddlFCNumber.Items.Clear();
                // ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlMinorLocation.DataSource = dt;
                    ddlMinorLocation.DataTextField = "BuildingName";
                    ddlMinorLocation.DataValueField = "BuildingId";
                    ddlMinorLocation.DataBind();
                    ddlMinorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlMinorLocation.SelectedIndex = 0;

                }
                else
                {
                    ddlMinorLocation.DataSource = null;
                    ddlMinorLocation.DataBind();
                    ddlMinorLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillMinorLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillMinorLocation", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillMinorLocation", path);
        }
    }
    private void fillMinorSubLocation(string BuildingId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct FloorName,FloorId from tblfloor where BuildingId=@BuildingId order by FloorName asc", con))
                {
                    // cmd.Parameters.AddWithValue("@CreatedBy", Session["userid"]);
                    cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlMinorSubLocation.Items.Clear();
                //ddlClientName.Items.Clear();
                //ddlCustodian.Items.Clear();
                ddlFCNumber.Items.Clear();
                //ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlMinorSubLocation.DataSource = dt;
                    ddlMinorSubLocation.DataTextField = "FloorName";
                    ddlMinorSubLocation.DataValueField = "FloorId";
                    ddlMinorSubLocation.DataBind();
                    ddlMinorSubLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlMinorSubLocation.SelectedIndex = 0;
                }
                else
                {
                    ddlMinorSubLocation.DataSource = null;
                    ddlMinorSubLocation.DataBind();
                    ddlMinorSubLocation.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillMinorSubLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillMinorSubLocation", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillMinorSubLocation", path);
        }
    }
    private void fillClientName(string CustodianId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column3 from AssetMaster where CustodianId=@CustodianId order by Column3 asc", con))
                {
                    cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlClientName.Items.Clear();

                //ddlFCNumber.Items.Clear();
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlClientName.DataSource = dt;
                    ddlClientName.DataTextField = "Column3";
                    ddlClientName.DataValueField = "Column3";
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlClientName.SelectedIndex = 0;
                }
                else
                {
                    ddlClientName.DataSource = null;
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillClientName", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillClientName", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillClientName", path);
        }
    }
    private void fillClientNamefromLocation(string LocationId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column3 from AssetMaster where LocationId=@LocationId order by Column3 asc", con))
                {
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlClientName.Items.Clear();

                //ddlFCNumber.Items.Clear();
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlClientName.DataSource = dt;
                    ddlClientName.DataTextField = "Column3";
                    ddlClientName.DataValueField = "Column3";
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlClientName.SelectedIndex = 0;
                }
                else
                {
                    ddlClientName.DataSource = null;
                    ddlClientName.DataBind();
                    ddlClientName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillClientNamefromLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillClientNamefromLocation", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillClientNamefromLocation", path);
        }
    }
    private void fillCustodian(string Column1)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                //DataTable dt = new DataTable();
                //using (SqlCommand cmd = new SqlCommand("select distinct CM.CustodianName,CM.CustodianId from CustodianMaster as CM left join AssetMaster as AM on CM.CustodianId=AM.CustodianID where AM.Column1=@Column1 order by CustodianName asc", con))
                //{
                //    cmd.Parameters.AddWithValue("@Column1", Column1);
                //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                //    {
                //        adp.Fill(dt);
                //    }
                //}
                DataSet ds = new DataSet();
                ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
                DataTable dt = ds.Tables[0];
                //ddlCustodian.Items.Clear();
                // ddlFCNumber.Items.Clear();
                //ddlClientName.Items.Clear();
                //ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlCustodian.DataSource = dt;
                    ddlCustodian.DataTextField = "CustodianName";
                    ddlCustodian.DataValueField = "CustodianId";
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.SelectedIndex = 0;
                }
                else
                {
                    ddlCustodian.DataSource = null;
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillCustodian", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillCustodian", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillCustodian", path);
        }
    }
    private void fillCustodianfromLocation(string LocationId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                //DataTable dt = new DataTable();
                //using (SqlCommand cmd = new SqlCommand("select distinct CM.CustodianName,CM.CustodianId from CustodianMaster as CM left join AssetMaster as AM on CM.CustodianId=AM.CustodianID where AM.LocationId=@LocationId order by CustodianName asc", con))
                //{
                //    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                //    {
                //        adp.Fill(dt);
                //    }
                //}
                DataSet ds = new DataSet();
                ds = Common.GetCustodianDetailsV2(null, null, null, null, null, null, null, null, Session["userid"].ToString());
                DataTable dt = ds.Tables[0];
                ddlCustodian.Items.Clear();
                // ddlFCNumber.Items.Clear();
                ddlClientName.Items.Clear();
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlCustodian.DataSource = dt;
                    ddlCustodian.DataTextField = "CustodianName";
                    ddlCustodian.DataValueField = "CustodianId";
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlCustodian.SelectedIndex = 0;
                }
                else
                {
                    ddlCustodian.DataSource = null;
                    ddlCustodian.DataBind();
                    ddlCustodian.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillCustodianfromLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillCustodianfromLocation", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillCustodianfromLocation", path);
        }
    }
    private void fillFCNumber(string FloorId, string LocationId, string BuildingId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column1 from AssetMaster where FloorId=@FloorId and LocationId=@LocationId and BuildingId=@BuildingId order by Column1 asc", con))
                {
                    //LocationId,BuildingId,
                    //cmd.Parameters.AddWithValue("@CreatedBy", Session["userid"]);
                    cmd.Parameters.AddWithValue("@FloorId", FloorId);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                //ddlCustodian.Items.Clear();
                ddlFCNumber.Items.Clear();
                //ddlClientName.Items.Clear();
                //ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlFCNumber.DataSource = dt;
                    ddlFCNumber.DataTextField = "Column1";
                    ddlFCNumber.DataValueField = "Column1";
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlFCNumber.SelectedIndex = 0;
                }
                else
                {
                    ddlFCNumber.DataSource = null;
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillFCNumber", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillFCNumber", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillFCNumber", path);
        }
    }
    private void fillFCNumberFromLocation(string LocationId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column1 from AssetMaster where LocationId=@LocationId order by Column1 asc", con))
                {
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                // ddlCustodian.Items.Clear();
                ddlFCNumber.Items.Clear();
                //ddlClientName.Items.Clear();
                //ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlFCNumber.DataSource = dt;
                    ddlFCNumber.DataTextField = "Column1";
                    ddlFCNumber.DataValueField = "Column1";
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlFCNumber.SelectedIndex = 0;
                }
                else
                {
                    ddlFCNumber.DataSource = null;
                    ddlFCNumber.DataBind();
                    ddlFCNumber.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillFCNumberFromLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillFCNumberFromLocation", "Home_adminx_demodashboard");

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillFCNumberFromLocation", path);
        }
    }
    private void fillAssigneeName(string Column3)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column2 from AssetMaster where Column3=@Column3 order by Column2 asc", con))
                {
                    cmd.Parameters.AddWithValue("@Column3", Column3);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlAssigneeName.DataSource = dt;
                    ddlAssigneeName.DataTextField = "Column2";
                    ddlAssigneeName.DataValueField = "Column2";
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlAssigneeName.SelectedIndex = 0;
                }
                else
                {
                    ddlAssigneeName.DataSource = null;
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillAssigneeName", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillAssigneeName", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillAssigneeName", path);
        }
    }
    private void fillAssigneeNamefromLocation(string LocationId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("select distinct Column2 from AssetMaster where LocationId=@LocationId order by Column2 asc", con))
                {
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
                ddlAssigneeName.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlAssigneeName.DataSource = dt;
                    ddlAssigneeName.DataTextField = "Column2";
                    ddlAssigneeName.DataValueField = "Column2";
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                    ddlAssigneeName.SelectedIndex = 0;
                }
                else
                {
                    ddlAssigneeName.DataSource = null;
                    ddlAssigneeName.DataBind();
                    ddlAssigneeName.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillAssigneeNamefromLocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillAssigneeNamefromLocation", "Home_adminx_demodashboard");

            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillAssigneeNamefromLocation", path);
        }
    }

    [System.Web.Services.WebMethod()]
    public static string SendParameters(string name)
    {
        return string.Format("Name: {0}", name);
    }
    public void fillGraphData()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                using (SqlCommand cmd1 = new SqlCommand("truncate table graphTempData", con))
                {
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlCommand cmdzz = new SqlCommand("select L.LocationName,Count(*) Stock from [AssetMaster] A inner join LocationMaster L on A.LocationId = L.LocationId and LocationName not in ('Document Returned') left join LocationPermission as LP on LP.LocationID = A.LocationId Where isnull(A.Active, 0) = 1 and LP.UserID = @UserID group by L.LocationName", con))
                {
                    cmdzz.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["userid"]));
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmdzz))
                    {
                        adp.Fill(dt);
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into graphTempData(LocationName,Stock) values(@LocationName,@Stock)", con))
                    {
                        cmd.Parameters.AddWithValue("@LocationName", dt.Rows[i]["LocationName"].ToString());
                        cmd.Parameters.AddWithValue("@Stock", dt.Rows[i]["Stock"]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "fillGraphData", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "fillGraphData", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "fillGraphData", path);
        }
    }

    [WebMethod]
    public static List<ChartDataHealthAnalysis> GetTodayHealthAnalysis()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<ChartDataHealthAnalysis> chartData = new List<ChartDataHealthAnalysis>();
                string qry = "";
                if (Convert.ToInt32(HttpContext.Current.Session["LocationID"]) != 0)
                {
                    qry += " and AM.LocationId =" + Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
                }
                //if (BuildingID != 0)
                if (Convert.ToInt32(HttpContext.Current.Session["BuildingID"]) != 0)
                {
                    qry += " and AM.BuildingId=" + Convert.ToInt32(HttpContext.Current.Session["BuildingID"]);
                }
                if (Convert.ToInt32(HttpContext.Current.Session["FloorId"]) != 0)
                //if (FloorId != 0)
                {
                    qry += " and AM.FloorId=" + Convert.ToInt32(HttpContext.Current.Session["FloorId"]);
                }
                if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
                //if (Column1FCNumber != "")
                {
                    qry += " and AM.Column1='" + HttpContext.Current.Session["Column1FCNumber"].ToString() + "'";
                }
                if (Convert.ToInt32(HttpContext.Current.Session["CustodianId"]) != 0)
                //if (CustodianId != 0)
                {
                    qry += " and AM.CustodianId=" + Convert.ToInt32(HttpContext.Current.Session["CustodianId"]);
                }
                if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
                //if (Column2AssigneeName != "")
                {
                    qry += " and AM.Column2='" + HttpContext.Current.Session["Column2AssigneeName"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    qry += " and AM.Column3='" + HttpContext.Current.Session["Column3ClientName"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    qry += " and AM.Column5='" + HttpContext.Current.Session["Column5CaseManager"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
                {
                    qry += " and AM.Column7='" + HttpContext.Current.Session["Column7CaseWorker1"].ToString() + "'";
                }
                insertData(qry);
                SqlCommand cmd = new SqlCommand("select * from TodayHealthAnalysistempData", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {

                    chartData.Add(new ChartDataHealthAnalysis { Data = Convert.ToString(dr["Data"]), counthealthData = Convert.ToInt32(dr["counthealthData"]) });

                }

                chartData.OrderBy(x => x.counthealthData);

                return chartData;
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "GetTodayHealthAnalysis", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "GetTodayHealthAnalysis", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetTodayHealthAnalysis", path);
            return null;
        }
    }
    public static void insertData(string qry)
    {
        try
        {
            string CaseWorker1 = null, LocationIdd = null, BuildingIdd = null, FloorIdd = null, CaseManager = null, Column1FCNumberr = null, Column3ClientNamee = null, Column2AssigneeNamee = null, CustodianIdd = null;
            if (HttpContext.Current.Session["LocationID"].ToString() != "0")
            {
                LocationIdd = HttpContext.Current.Session["LocationID"].ToString();
            }
            if (HttpContext.Current.Session["BuildingID"].ToString() != "0")
            {
                BuildingIdd = HttpContext.Current.Session["BuildingID"].ToString();
            }
            if (HttpContext.Current.Session["FloorId"].ToString() != "0")
            {
                FloorIdd = HttpContext.Current.Session["FloorId"].ToString();
            }
            if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
            {
                CaseManager = HttpContext.Current.Session["Column5CaseManager"].ToString();
            }
            if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
            {
                CaseWorker1 = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
            }
            if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
            {
                Column1FCNumberr = HttpContext.Current.Session["Column1FCNumber"].ToString();
            }
            if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
            {
                Column3ClientNamee = HttpContext.Current.Session["Column3ClientName"].ToString();
            }
            if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
            {
                Column2AssigneeNamee = HttpContext.Current.Session["Column2AssigneeName"].ToString();
            }
            if (HttpContext.Current.Session["CustodianId"].ToString() != "0")
            {
                CustodianIdd = HttpContext.Current.Session["CustodianId"].ToString();
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                using (SqlCommand cmd0 = new SqlCommand("truncate table TodayHealthAnalysistempData", con))
                {
                    con.Open();
                    cmd0.ExecuteNonQuery();
                    con.Close();
                }

                string[] datavalues = { "Closed", "Inactive", "Active", "Inprogress" };
                for (int i = 0; i < datavalues.Count(); i++)
                {

                    //new
                    DataTable dttemp = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_1_27082022", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CategoryId", null);
                        cmd.Parameters.AddWithValue("@SubCatId", null);
                        cmd.Parameters.AddWithValue("@LocationId", LocationIdd);
                        cmd.Parameters.AddWithValue("@BuildingId", BuildingIdd);
                        cmd.Parameters.AddWithValue("@FloorId", FloorIdd);
                        cmd.Parameters.AddWithValue("@DepartmentId", null);
                        cmd.Parameters.AddWithValue("@FromDate", null);
                        cmd.Parameters.AddWithValue("@Todate", null);
                        cmd.Parameters.AddWithValue("@AssetCode", null);
                        cmd.Parameters.AddWithValue("@caseManager", CaseManager);
                        cmd.Parameters.AddWithValue("@caseWorker1", CaseWorker1);
                        cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                        cmd.Parameters.AddWithValue("@SearchText", datavalues[i]);
                        cmd.Parameters.AddWithValue("@Column1FCNumber", Column1FCNumberr);
                        cmd.Parameters.AddWithValue("@CustodianId", CustodianIdd);
                        cmd.Parameters.AddWithValue("@Column3ClientName", Column3ClientNamee);
                        cmd.Parameters.AddWithValue("@Column2AssigneeName", Column2AssigneeNamee);

                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.SelectCommand.CommandTimeout = 600;
                            adp.Fill(dttemp);
                        }
                    }

                    //new 

                    //DataTable dt1 = new DataTable();
                    //using (SqlCommand cmd1 = new SqlCommand("select count(*) as '" + datavalues[i] + "'  from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where LP.LocationID !=8 and AM.Column9 = '" + datavalues[i] + "' and AM.Active=1 and LP.UserID = @UserID and CP.UserID = @UserID " + qry + "", con))
                    //{
                    //    cmd1.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                    //    using (SqlDataAdapter adp = new SqlDataAdapter(cmd1))
                    //    {
                    //        adp.Fill(dt1);
                    //    }
                    //}
                    if (datavalues[i] == "Active")
                    {
                        datavalues[i] = "Not Started";
                    }
                    using (SqlCommand cmd4 = new SqlCommand("insert into TodayHealthAnalysistempData(Data,counthealthData) values('" + datavalues[i] + "','" + dttemp.Rows.Count + "')", con))
                    {
                        con.Open();
                        cmd4.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "insertData", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "insertData", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "insertData", path);
        }
    }

    [WebMethod]
    public static List<ChartData> GetBarchartDataV2_()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<ChartData> chartData = new List<ChartData>();
                //if (loadstatusGetBarchartData == 0)
                //{
                loadstatusGetBarchartData = 1;
                SqlCommand cmd = new SqlCommand("Chart_AssetVerificationReportV2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    chartData.Add(new ChartData { StockDate = Convert.ToString(dr["CreatedDate"]).Substring(0, 9), Found = Convert.ToInt32(dr["Found"]), Missing = Convert.ToInt32(dr["Missing"]), MissMatch = Convert.ToInt32(dr["Mismatch"]), Extra = Convert.ToInt32(dr["Extra"]) });
                }
                chartData.OrderBy(x => x.Found);
                // }
                return chartData;

            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "GetBarchartDataV2_", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "GetBarchartDataV2_", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetBarchartDataV2_", path);
            return null;
        }
    }

    [WebMethod]
    public static List<PrintVsTaggedInfo> GetTagVsPrintV2_()
    {
        try
        {
            List<PrintVsTagged> chartData = new List<PrintVsTagged>();
            List<PrintVsTaggedInfo> PrintInfo = new List<PrintVsTaggedInfo>();
            List<PrintVsTaggedCount> chartFinalData = new List<PrintVsTaggedCount>();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {

                //if (loadstatusGetTagVsPrint == 0)
                //{
                loadstatusGetTagVsPrint = 1;
                try
                {
                    string Asset = null, column7CaseWorker1 = null, Column5CaseManagerName = null, CategoryId = null, SubCatId = null, LocationId = null, BuildingId = null, FloorId = null, DepartmentId = null, AssetCode = null, SearchText = null, CustodianId = null, Column1FCNumber = null, Column3ClientName = null, Column2AssigneeName = null;
                    //newcode
                    int reprintCount = 0, encodedCount = 0, printedValue = 0, taggedValue = 0;
                    if (HttpContext.Current.Session["LocationID"] != "0")
                    {
                        LocationId = HttpContext.Current.Session["LocationID"].ToString();
                    }
                    if (HttpContext.Current.Session["BuildingID"] != "0")
                    {
                        BuildingId = HttpContext.Current.Session["BuildingID"].ToString();
                    }
                    if (HttpContext.Current.Session["FloorId"] != "0")
                    {
                        FloorId = HttpContext.Current.Session["FloorId"].ToString();
                    }
                    if (HttpContext.Current.Session["CustodianId"] != "0")
                    {
                        CustodianId = HttpContext.Current.Session["CustodianId"].ToString();
                    }
                    if (HttpContext.Current.Session["Column1FCNumber"] != "0")
                    {
                        Column1FCNumber = HttpContext.Current.Session["Column1FCNumber"].ToString();
                    }
                    if (HttpContext.Current.Session["Column3ClientName"] != "0")
                    {
                        Column3ClientName = HttpContext.Current.Session["Column3ClientName"].ToString();
                    }
                    if (HttpContext.Current.Session["Column2AssigneeName"] != "0")
                    {
                        Column2AssigneeName = HttpContext.Current.Session["Column2AssigneeName"].ToString();
                    }
                    if (HttpContext.Current.Session["Column5CaseManager"] != "0")
                    {
                        Column5CaseManagerName = HttpContext.Current.Session["Column5CaseManager"].ToString();
                    }

                    if (Column5CaseManagerName == "")
                    {
                        Column5CaseManagerName = null;
                    }
                    if (HttpContext.Current.Session["Column7CaseWorker1"] != "0")
                    {
                        column7CaseWorker1 = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
                    }
                    if (column7CaseWorker1 == "")
                    {
                        column7CaseWorker1 = null;
                    }


                    PrintBL objBLnew = new PrintBL();
                    DataSet dsnew1 = objBLnew.GetAssetDetailsForRePrintV2_(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode, SearchText, CustodianId, "T18", HttpContext.Current.Session["userid"].ToString(), Column1FCNumber, Column2AssigneeName, Column3ClientName, Column5CaseManagerName, column7CaseWorker1);
                    reprintCount = dsnew1.Tables[0].Rows.Count;
                    ReportBL objReport = new ReportBL();
                    if (Column1FCNumber == null)
                    {
                        Column1FCNumber = "";
                    }
                    if (Column2AssigneeName == null)
                    {
                        Column2AssigneeName = "";
                    }
                    if (Column3ClientName == null)
                    {
                        Column3ClientName = "";
                    }
                    if (Column5CaseManagerName == null)
                    {
                        Column5CaseManagerName = "";
                    }
                    if (column7CaseWorker1 == null)
                    {
                        column7CaseWorker1 = "";
                    }
                    DataSet dsnew2 = objReport.GetEncodedLabelsV2_(null, null, LocationId, BuildingId, FloorId, null, null, AssetCode, null, CustodianId, SearchText, HttpContext.Current.Session["userid"].ToString(), Column1FCNumber, Column2AssigneeName, Column3ClientName, Column5CaseManagerName, column7CaseWorker1);
                    encodedCount = dsnew2.Tables[0].Rows.Count;

                    printedValue = reprintCount + encodedCount;
                    DataSet dsnew3 = new DataSet();
                    using (SqlCommand cmd = new SqlCommand("usp_GetTaggedItemsV2_", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LocationID", LocationId);
                        cmd.Parameters.AddWithValue("@BuildingID", BuildingId);
                        cmd.Parameters.AddWithValue("@FloorID", FloorId);
                        cmd.Parameters.AddWithValue("@CategoryID", null);
                        cmd.Parameters.AddWithValue("@SubCategoryID", null);
                        cmd.Parameters.AddWithValue("@DepartmentID", null);
                        cmd.Parameters.AddWithValue("@AssetCode", AssetCode);
                        cmd.Parameters.AddWithValue("@CustodianID", CustodianId);
                        cmd.Parameters.AddWithValue("@SearchText", SearchText);
                        cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@Column1FCNumber", Column1FCNumber);
                        cmd.Parameters.AddWithValue("@Column2AssigneeName", Column2AssigneeName);
                        cmd.Parameters.AddWithValue("@Column3ClientName", Column3ClientName);
                        cmd.Parameters.AddWithValue("@Column5CaseManager", Column5CaseManagerName);
                        cmd.Parameters.AddWithValue("@column7CaseWorker1", column7CaseWorker1);
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(dsnew3);
                        }
                    }
                    taggedValue = dsnew3.Tables[0].Rows.Count;
                    // zzzzz
                    //new code
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PrintStatus");
                    dt.Columns.Add("IsTagged");
                    DataRow drz = dt.NewRow();

                    drz["PrintStatus"] = printedValue.ToString();
                    drz["IsTagged"] = taggedValue.ToString();
                    dt.Rows.Add(drz);

                    if (dt.Rows.Count > 0)
                    {

                        chartData.Add(new PrintVsTagged { PrintStatus = Convert.ToInt32(dt.Rows[0]["PrintStatus"].ToString() == "" ? 0 : dt.Rows[0]["PrintStatus"]), IsTagged = Convert.ToDouble(dt.Rows[0]["IsTagged"].ToString() == "" ? 0 : dt.Rows[0]["IsTagged"]), LocationId = Convert.ToDouble(dt.Rows[0]["IsTagged"]) });

                        if (chartData != null)
                        {

                            chartFinalData.Add(new PrintVsTaggedCount { Printed = Convert.ToInt32(dt.Rows[0]["PrintStatus"].ToString()), Tagged = Convert.ToInt32(dt.Rows[0]["IsTagged"].ToString()) });

                            PrintInfo.Add(new PrintVsTaggedInfo { StringColumn = "Printed", dataCount = chartFinalData[0].Printed.Value });
                            PrintInfo.Add(new PrintVsTaggedInfo { StringColumn = "Tagged", dataCount = chartFinalData[0].Tagged.Value });


                        }
                    }


                    //System.Threading.Thread.Sleep(1500);

                }
                catch (Exception ex)
                {

                    con.Close();

                    //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "GetTagVsPrintV2_", "Home_adminx_demodashboard");
                    //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "GetTagVsPrintV2_", "Home_adminx_demodashboard");
                    Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetTagVsPrintV2_", path);
                }
                finally
                {
                    con.Close();
                }
            }
            return PrintInfo;
            //}
        }
        catch (Exception ex)
        {

            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "GetTagVsPrintV2_", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "GetTagVsPrintV2_", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetTagVsPrintV2_", path);
            return null;

        }
    }


    [WebMethod]
    public static List<ListItem> bindlocation()
    {
        try
        {
            List<ListItem> locations = new List<ListItem>();
            //if (loadstatusbindlocation == 0)
            //{
            loadstatusbindlocation = 1;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataTable dt = new DataTable();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                locations.Add(new ListItem { Value = dr["LocationId"].ToString(), Text = dr["LocationName"].ToString() });
            }
            //}
            return locations;
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "bindlocation", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "bindlocation", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "bindlocation", path);
            return null;
        }
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
    public class ChartDataHealthAnalysis
    {
        public string Data { get; set; }
        public int counthealthData { get; set; }
    }
    public class PrintVsTaggedInfo
    {
        public string StringColumn { get; set; }
        public double dataCount { get; set; }
    }

    public class top10ClientsData
    {
        public string clientName { get; set; }
        public int countClientData { get; set; }
    }
    public class CaseManagerwiseDatalist
    {
        public string CaseManager { get; set; }
        public int documentCount { get; set; }
    }
    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        HttpContext.Current.Session["VAL"] = e.PostBackValue;
        Response.Redirect("topClients.aspx");
    }
    protected void ddlMajorLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMajorLocation.SelectedValue != "0")
            {
                fillMinorLocation(ddlMajorLocation.SelectedValue);
                LocationID = Convert.ToInt32(ddlMajorLocation.SelectedValue);
                HttpContext.Current.Session["LocationID"] = Convert.ToInt32(ddlMajorLocation.SelectedValue);
                ddlMinorLocation_SelectedIndexChanged(sender, e);
                fillFCNumberFromLocation(ddlMajorLocation.SelectedValue);

                // fillCustodianfromLocation(ddlMajorLocation.SelectedValue);
                //fillClientNamefromLocation(ddlMajorLocation.SelectedValue);
                //fillAssigneeNamefromLocation(ddlMajorLocation.SelectedValue);
            }
            else
            {
                // Response.Redirect("Home.aspx");
                HttpContext.Current.Session["LocationID"] = Convert.ToInt32(ddlMajorLocation.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
                fillFCNumberonload();
                // fillCustodianonload();
                //fillClientNameonload();
                //fillAssigneeNameonload();
                ddlMinorLocation.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlMajorLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlMajorLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlMajorLocation_SelectedIndexChanged", path);
        }
    }

    protected void ddlMinorLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMinorLocation.SelectedValue != "0")
            {
                fillMinorSubLocation(ddlMinorLocation.SelectedValue);
                BuildingID = Convert.ToInt32(ddlMinorLocation.SelectedValue);
                HttpContext.Current.Session["BuildingID"] = Convert.ToInt32(ddlMinorLocation.SelectedValue);
                ddlMinorSubLocation_SelectedIndexChanged(sender, e);
            }
            else
            {
                HttpContext.Current.Session["BuildingID"] = Convert.ToInt32(ddlMinorLocation.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlMinorLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlMinorLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlMinorLocation_SelectedIndexChanged", path);
        }
    }

    protected void ddlMinorSubLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMinorSubLocation.SelectedValue != "0")
            {
                fillFCNumber(ddlMinorSubLocation.SelectedValue, ddlMajorLocation.SelectedValue, ddlMinorLocation.SelectedValue);
                FloorId = Convert.ToInt32(ddlMinorSubLocation.SelectedValue);
                HttpContext.Current.Session["FloorId"] = Convert.ToInt32(ddlMinorSubLocation.SelectedValue);
                ddlFCNumber_SelectedIndexChanged(sender, e);
            }
            else
            {
                HttpContext.Current.Session["FloorId"] = Convert.ToInt32(ddlMinorSubLocation.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlMinorSubLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlMinorSubLocation_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlMinorSubLocation_SelectedIndexChanged", path);
        }
    }
    protected void ddlFCNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFCNumber.SelectedValue != "0")
            {
                //fillCustodian(ddlFCNumber.SelectedValue);
                //Column1FCNumber = ddlFCNumber.SelectedValue;
                HttpContext.Current.Session["Column1FCNumber"] = ddlFCNumber.SelectedValue;
                //ddlCustodian_SelectedIndexChanged(sender, e);
            }
            else
            {
                HttpContext.Current.Session["Column1FCNumber"] = ddlFCNumber.SelectedValue;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlFCNumber_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlFCNumber_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlFCNumber_SelectedIndexChanged", path);
        }
    }
    protected void ddlCustodian_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustodian.SelectedValue != "0")
            {
                fillClientName(ddlCustodian.SelectedValue);
                CustodianId = Convert.ToInt32(ddlCustodian.SelectedValue);
                HttpContext.Current.Session["CustodianId"] = Convert.ToInt32(ddlCustodian.SelectedValue);
                ddlClientName_SelectedIndexChanged(sender, e);
            }
            else
            {
                HttpContext.Current.Session["CustodianId"] = Convert.ToInt32(ddlCustodian.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlCustodian_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlCustodian_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlCustodian_SelectedIndexChanged", path);
        }
    }
    protected void ddlClientName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlClientName.SelectedValue != "0")
            {
                fillAssigneeName(ddlClientName.SelectedValue);
                Column3ClientName = ddlClientName.SelectedValue;
                HttpContext.Current.Session["Column3ClientName"] = ddlClientName.SelectedValue;
                ddlAssigneeName_SelectedIndexChanged(sender, e);
            }
            else
            {
                HttpContext.Current.Session["Column3ClientName"] = ddlClientName.SelectedValue;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "ddlCustodian_SelectedIndexChanged", "Home_adminx_demodashboard");
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "ddlCustodian_SelectedIndexChanged", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlClientName_SelectedIndexChanged", path);
        }
    }
    protected void ddlAssigneeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAssigneeName.SelectedValue != "0")
            {
                Column2AssigneeName = ddlAssigneeName.SelectedValue;
                HttpContext.Current.Session["Column2AssigneeName"] = ddlAssigneeName.SelectedValue;
            }
            else
            {
                HttpContext.Current.Session["Column2AssigneeName"] = ddlAssigneeName.SelectedValue;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "defaultContentsofPage()", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "ddlAssigneeName_SelectedIndexChanged", path);
        }
    }


    protected void btnclear_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    [WebMethod]
    public static List<ChartData> GetBarchartDataV2()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<ChartData> chartData = new List<ChartData>();

                loadstatusGetBarchartData = 1;
                SqlCommand cmd = new SqlCommand("Chart_AssetVerificationReport_V2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@locationID", LocationID);
                cmd.Parameters.AddWithValue("@BuildingID", BuildingID);
                cmd.Parameters.AddWithValue("@FloorID", FloorId);
                cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userid"]);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    chartData.Add(new ChartData { StockDate = Convert.ToString(dr["CreatedDate"]).Substring(0, 9), Found = Convert.ToInt32(dr["Found"]), Missing = Convert.ToInt32(dr["Missing"]), MissMatch = Convert.ToInt32(dr["Mismatch"]), Extra = Convert.ToInt32(dr["Extra"]) });
                    //chartData.Add(new ChartData { StockDate = "2017-01-05", Found = 9, Missing = 2, MissMatch = 1, Extra = 3 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-06", Found = 7, Missing = 3, MissMatch = 1, Extra = 3 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-07", Found = 6, Missing = 1, MissMatch = 2, Extra = 3 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-08", Found = 8, Missing = 3, MissMatch = 5, Extra = 1 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-09", Found = 9, Missing = 2, MissMatch = 1, Extra = 3 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-10", Found = 7, Missing = 3, MissMatch = 1, Extra = 4 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-11", Found = 6, Missing = 1, MissMatch = 2, Extra = 3 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-12", Found = 8, Missing = 3, MissMatch = 5, Extra = 6 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-13", Found = 6, Missing = 1, MissMatch = 2, Extra = 4 });
                    //chartData.Add(new ChartData { StockDate = "2017-01-14", Found = 8, Missing = 3, MissMatch = 5, Extra = 2 });

                }
                chartData.OrderBy(x => x.Found);

                return chartData;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetBarchartDataV2", path);
            return null;
        }
    }
    [WebMethod]
    public static List<PrintVsTaggedInfo> GetTagVsPrintV2(int? Location)
    {
        try
        {
            List<PrintVsTagged> chartData = new List<PrintVsTagged>();
            List<PrintVsTaggedInfo> PrintInfo = new List<PrintVsTaggedInfo>();
            List<PrintVsTaggedCount> chartFinalData = new List<PrintVsTaggedCount>();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                loadstatusGetTagVsPrint = 1;
                try
                {

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("SELECT a.AssetCode, CASE WHEN PrintStatus IS NULL THEN isnull(IsEncodedTSB, IsEncodedTHS) WHEN PrintStatus IS NULL THEN isnull(IsEncodedTHS, IsEncodedTSB) ELSE PrintStatus END AS PrintStatus, a.IsTagged, l.LocationId FROM dbo.AssetMaster AS a INNER JOIN dbo.LocationMaster AS l ON a.LocationId = l.LocationId WHERE(ISNULL(a.Active, 0) = 1) AND(a.LocationId = @LocationId) AND(a.BuildingId = @BuildingId) AND(a.FloorId = @FloorId) AND(a.CustodianId = @CustodianId) AND(a.Column1 = @Column1) AND(a.Column2 = @Column2) AND (a.Column3 = @Column3)", con))
                    {
                        cmd.Parameters.AddWithValue("@LocationId", LocationID);
                        cmd.Parameters.AddWithValue("@BuildingId", BuildingID);
                        cmd.Parameters.AddWithValue("@FloorId", FloorId);
                        cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                        cmd.Parameters.AddWithValue("@Column1", Column1FCNumber);
                        cmd.Parameters.AddWithValue("@Column2", Column2AssigneeName);
                        cmd.Parameters.AddWithValue("@Column3", Column3ClientName);
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(ds);
                        }
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];

                        foreach (DataRow dr in dt.Rows)
                        {

                            chartData.Add(new PrintVsTagged { PrintStatus = Convert.ToInt32(dr["PrintStatus"].ToString() == "" ? 0 : dr["PrintStatus"]), IsTagged = Convert.ToDouble(dr["IsTagged"].ToString() == "" ? 0 : dr["IsTagged"]), LocationId = Convert.ToDouble(dr["IsTagged"]) });
                        }
                        //&& chartData.Count > 0
                        if (chartData != null)
                        {
                            chartFinalData.Add(new PrintVsTaggedCount { Printed = chartData.Where(x => x.PrintStatus > 0).Count(), Tagged = chartData.Where(x => x.IsTagged > 0).Count() });
                            //if (chartFinalData[0].Printed.Value != 0 || chartFinalData[0].Tagged.Value != 0)
                            //{
                            PrintInfo.Add(new PrintVsTaggedInfo { StringColumn = "Printed", dataCount = chartFinalData[0].Printed.Value });
                            PrintInfo.Add(new PrintVsTaggedInfo { StringColumn = "Tagged", dataCount = chartFinalData[0].Tagged.Value });
                            // }

                        }
                    }


                    //System.Threading.Thread.Sleep(1500);

                }
                catch (Exception ex)
                {
                    Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetTagVsPrintV2", path);
                    con.Close();
                }
                finally
                {
                    con.Close();
                }
            }
            return PrintInfo;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetTagVsPrintV2", path);
            return null;
        }
    }
    [WebMethod]
    public static List<LocationStock> GetGeoLocationWiseStockV2()
    {
        try
        {
            List<LocationStock> chartData = new List<LocationStock>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                try
                {

                    string LocationId = null, BuildingId = null, FloorId = null, CustodianId = null;
                    string Column1FC = null, Column2assign = null, Column3client = null, Column5caseManager = null, Column7caseWorker1 = null;
                    if (Convert.ToInt32(HttpContext.Current.Session["LocationID"]) != 0)
                    {
                        LocationId = HttpContext.Current.Session["LocationID"].ToString();
                    }
                    if (Convert.ToInt32(HttpContext.Current.Session["BuildingID"]) != 0)
                    {
                        BuildingId = HttpContext.Current.Session["BuildingID"].ToString();
                    }
                    if (Convert.ToInt32(HttpContext.Current.Session["FloorId"]) != 0)
                    {
                        FloorId = HttpContext.Current.Session["FloorId"].ToString();
                    }
                    if (Convert.ToInt32(HttpContext.Current.Session["CustodianId"]) != 0)
                    {
                        CustodianId = HttpContext.Current.Session["CustodianId"].ToString();
                    }
                    if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
                    {
                        Column1FC = HttpContext.Current.Session["Column1FCNumber"].ToString();
                    }
                    if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
                    {
                        Column2assign = HttpContext.Current.Session["Column2AssigneeName"].ToString();
                    }
                    if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
                    {
                        Column3client = HttpContext.Current.Session["Column3ClientName"].ToString();
                    }
                    if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
                    {
                        Column5caseManager = HttpContext.Current.Session["Column5CaseManager"].ToString();
                    }
                    if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
                    {
                        Column7caseWorker1 = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
                    }
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd1 = new SqlCommand("GetAssetsAccordingToDateandTypeV2_fordashboard", con))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@LocationId", LocationId);
                        cmd1.Parameters.AddWithValue("@BuildingId", BuildingId);
                        cmd1.Parameters.AddWithValue("@FloorId", FloorId);
                        cmd1.Parameters.AddWithValue("@CustodianId", CustodianId);
                        cmd1.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                        cmd1.Parameters.AddWithValue("@SearchText", null);
                        cmd1.Parameters.AddWithValue("@Column1", Column1FC);
                        cmd1.Parameters.AddWithValue("@Column2", Column2assign);
                        cmd1.Parameters.AddWithValue("@Column3", Column3client);
                        cmd1.Parameters.AddWithValue("@Column5", Column5caseManager);
                        cmd1.Parameters.AddWithValue("@Column7", Column7caseWorker1);
                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd1))
                        {
                            ad.Fill(dt);
                        }

                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            chartData.Add(new LocationStock { LocationName = Convert.ToString(dr["LocationName"]), Stock = Convert.ToDouble(dr["Stock"]) });
                        }
                        chartData.OrderBy(x => x.Stock);


                    }
                    else
                    {
                        chartData.Clear();
                    }

                }
                catch (Exception ex)
                {
                    Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetGeoLocationWiseStockV2", path);
                    //Response.Write("<script>alert('"+ex.ToString()+"');</script>");
                    con.Close();
                }
                finally
                {
                    con.Close();
                }
            }

            // System.Threading.Thread.Sleep(1000);
            return chartData;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "GetGeoLocationWiseStockV2", path);
            return null;
        }
    }
    [WebMethod]
    public static List<top10ClientsData> LoadTop10Charts()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<top10ClientsData> chartData4 = new List<top10ClientsData>();
                string qry = "";
                string LocationId = null, BuildingId = null, FloorIdd = null, CustodianIdd = null, Column7 = null, Column1 = null, Column2 = null, Column3 = null, Column5 = null;
                if (Convert.ToInt32(HttpContext.Current.Session["LocationID"]) != 0)
                {
                    LocationId = HttpContext.Current.Session["LocationID"].ToString();
                    // qry += " and AM.LocationId =" + Convert.ToInt32(HttpContext.Current.Session["LocationID"]);
                }
                //if (BuildingID != 0)
                if (Convert.ToInt32(HttpContext.Current.Session["BuildingID"]) != 0)
                {
                    BuildingId = HttpContext.Current.Session["BuildingID"].ToString();
                    //   qry += " and AM.BuildingId=" + Convert.ToInt32(HttpContext.Current.Session["BuildingID"]);
                }
                if (Convert.ToInt32(HttpContext.Current.Session["FloorId"]) != 0)
                //if (FloorId != 0)
                {
                    FloorIdd = HttpContext.Current.Session["FloorId"].ToString();
                    // qry += " and AM.FloorId=" + Convert.ToInt32(HttpContext.Current.Session["FloorId"]);
                }
                if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
                //if (Column1FCNumber != "")
                {
                    Column1 = HttpContext.Current.Session["Column1FCNumber"].ToString();
                    //  qry += " and AM.Column1='" + HttpContext.Current.Session["Column1FCNumber"].ToString() + "'";
                }
                if (Convert.ToInt32(HttpContext.Current.Session["CustodianId"]) != 0)
                //if (CustodianId != 0)
                {
                    CustodianIdd = HttpContext.Current.Session["CustodianId"].ToString();
                    // qry += " and AM.CustodianId=" + Convert.ToInt32(HttpContext.Current.Session["CustodianId"]);
                }
                if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
                //if (Column2AssigneeName != "")
                {
                    Column2 = HttpContext.Current.Session["Column2AssigneeName"].ToString();
                    // qry += " and AM.Column2='" + HttpContext.Current.Session["Column2AssigneeName"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    Column3 = HttpContext.Current.Session["Column3ClientName"].ToString();
                    // qry += " and AM.Column3='" + HttpContext.Current.Session["Column3ClientName"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    Column5 = HttpContext.Current.Session["Column5CaseManager"].ToString();
                    // qry += " and AM.Column5='" + HttpContext.Current.Session["Column5CaseManager"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
                {
                    Column7 = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
                }
                string qry1 = "";
                // SqlCommand cmd = new SqlCommand("select top 10 AM.column3 as clientName, Count(*) as countClientData from AssetMaster as AM left join LocationPermission as LP on LP.LocationID = AM.LocationId left join CustodianPermission as CP on CP.CustodianId = AM.CustodianId where CP.UserID = @UserID and LP.LocationID !=8 and LP.UserID = @UserID " + qry + "group by   AM.column3 order by countClientData desc", con);
                SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_top10Clients", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                cmd.Parameters.AddWithValue("@LocationId", LocationId);
                cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                cmd.Parameters.AddWithValue("@FloorId", FloorIdd);
                cmd.Parameters.AddWithValue("@Column1", Column1);
                cmd.Parameters.AddWithValue("@Column2", Column2);
                cmd.Parameters.AddWithValue("@Column3", Column3);
                cmd.Parameters.AddWithValue("@Column5", Column5);
                cmd.Parameters.AddWithValue("@Column7", Column7);
                cmd.Parameters.AddWithValue("@CustodianId", CustodianIdd);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.SelectCommand.CommandTimeout = 600;
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    chartData4.Add(new top10ClientsData { clientName = Convert.ToString(dr["clientName"]), countClientData = Convert.ToInt32(dr["countClientData"]) });
                }
                chartData4.OrderBy(x => x.countClientData);
                return chartData4;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "LoadTop10Charts", path);
            return null;
        }
    }

    [WebMethod]
    public static List<CaseManagerwiseDatalist> CaseManagerwiseData()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                List<CaseManagerwiseDatalist> chartData4 = new List<CaseManagerwiseDatalist>();
                string qry = "";
                string LocationId = null, BuildingId = null, FloorId = null, CustodianId = null, Column7CaseWorker = null, Column1FCNumber = null, Column2AssigneeName = null, Column3ClientName = null, Column5CaseManager = null;
                if (Convert.ToInt32(HttpContext.Current.Session["LocationID"]) != 0)
                {
                    LocationId = HttpContext.Current.Session["LocationID"].ToString();
                }
                //if (BuildingID != 0)
                if (Convert.ToInt32(HttpContext.Current.Session["BuildingID"]) != 0)
                {
                    BuildingId = HttpContext.Current.Session["BuildingID"].ToString();
                }
                if (Convert.ToInt32(HttpContext.Current.Session["FloorId"]) != 0)
                //if (FloorId != 0)
                {
                    FloorId = HttpContext.Current.Session["FloorId"].ToString();
                }
                if (HttpContext.Current.Session["Column1FCNumber"].ToString() != "0")
                //if (Column1FCNumber != "")
                {
                    Column1FCNumber = HttpContext.Current.Session["Column1FCNumber"].ToString();
                }
                if (Convert.ToInt32(HttpContext.Current.Session["CustodianId"]) != 0)
                //if (CustodianId != 0)
                {
                    CustodianId = HttpContext.Current.Session["CustodianId"].ToString();
                }
                if (HttpContext.Current.Session["Column2AssigneeName"].ToString() != "0")
                //if (Column2AssigneeName != "")
                {
                    Column2AssigneeName = HttpContext.Current.Session["Column2AssigneeName"].ToString();
                }
                if (HttpContext.Current.Session["Column3ClientName"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    Column3ClientName = HttpContext.Current.Session["Column3ClientName"].ToString();
                }
                if (HttpContext.Current.Session["Column5CaseManager"].ToString() != "0")
                //if (Column3ClientName != "")
                {
                    Column5CaseManager = HttpContext.Current.Session["Column5CaseManager"].ToString();
                }
                if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
                {
                    Column7CaseWorker = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
                }
                DataTable dtcase = new DataTable();
                using (SqlCommand cmd = new SqlCommand("GetAssetsAccordingToDateandTypeV2_forcasemanager_WithoutDocReturned", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryId", null);
                    cmd.Parameters.AddWithValue("@SubCatId", null);
                    cmd.Parameters.AddWithValue("@LocationId", LocationId);
                    cmd.Parameters.AddWithValue("@BuildingId", BuildingId);
                    cmd.Parameters.AddWithValue("@FloorId", FloorId);
                    cmd.Parameters.AddWithValue("@DepartmentId", null);
                    cmd.Parameters.AddWithValue("@FromDate", null);
                    cmd.Parameters.AddWithValue("@Todate", null);
                    cmd.Parameters.AddWithValue("@AssetCode", null);
                    cmd.Parameters.AddWithValue("@CustodianId", CustodianId);
                    cmd.Parameters.AddWithValue("@SearchText", null);
                    cmd.Parameters.AddWithValue("@Column1FCNumber", Column1FCNumber);
                    cmd.Parameters.AddWithValue("@Column2AssigneeName", Column2AssigneeName);
                    cmd.Parameters.AddWithValue("@Column3ClientName", Column3ClientName);
                    cmd.Parameters.AddWithValue("@Column5CaseManager", Column5CaseManager);
                    cmd.Parameters.AddWithValue("@Column7CaseWorker", Column7CaseWorker);
                    cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtcase);
                    }

                }
                //using (SqlCommand cmd1 = new SqlCommand("truncate table tempcasemanagerdata;", con))
                //{
                //    con.Open();
                //    cmd1.ExecuteNonQuery();
                //    con.Close();
                //}
                //for (int i = 0; i < dtcase.Rows.Count; i++)
                //{
                //    using (SqlCommand cmd2 = new SqlCommand("insert into tempcasemanagerdata(AssetCode,casemanagernames) values(@AssetCode,@casemanagernames)", con))
                //    {
                //        cmd2.Parameters.AddWithValue("@AssetCode", dtcase.Rows[i]["AssetCode"].ToString());
                //        cmd2.Parameters.AddWithValue("@casemanagernames", dtcase.Rows[i]["Column5"].ToString());
                //        con.Open();
                //        cmd2.ExecuteNonQuery();
                //        con.Close();
                //    }
                //}
                //DataTable dt = new DataTable();
                //using (SqlCommand cmd3 = new SqlCommand("select distinct top 5 casemanagernames as CaseManager,count(*) as documentCount from tempcasemanagerdata group by casemanagernames order by documentCount", con))
                //{
                //    using (SqlDataAdapter ap = new SqlDataAdapter(cmd3))
                //    {
                //        ap.Fill(dt);
                //    }
                //}

                foreach (DataRow dr in dtcase.Rows)
                {
                    chartData4.Add(new CaseManagerwiseDatalist { CaseManager = Convert.ToString(dr["CaseManager"]), documentCount = Convert.ToInt32(dr["documentCount"]) });
                }
                chartData4.OrderBy(x => x.documentCount);
                return chartData4;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "CaseManagerwiseData", path);
            return null;
        }
    }
    [WebMethod]
    public static void Dashboard_Filtered_CaseManager(string name)
    {
        try
        {
            HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
            HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
            HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = name;
            string Column7CaseWorker = "";
            if (HttpContext.Current.Session["Column7CaseWorker1"].ToString() != "0")
            {
                Column7CaseWorker = HttpContext.Current.Session["Column7CaseWorker1"].ToString();
            }
            HttpContext.Current.Session["Dashboard_Filtered_CaseWorker1Name"] = Column7CaseWorker;
        }
        catch (Exception ex)
        {
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.Message.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "Dashboard_Filtered_CaseManager", path);
            //Logging.WriteLog(Logging.LogLevelL4N.ERROR, ex.StackTrace.ToString(), "Dashboard_Filtered_LocationV2", "Home_adminx_demodashboard");
        }

    }

    public static void setvalueofmonthdata(string locname)
    {
        try
        {


        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Home.aspx", "setvalueofmonthdata", path);

        }
    }



    protected void ddlCaseManager_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpContext.Current.Session["Column5CaseManager"] = ddlCaseManager.SelectedValue.ToString();
    }

    protected void ddlCaseWorker1_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpContext.Current.Session["Column7CaseWorker1"] = ddlCaseWorker1.SelectedValue.ToString();
    }
}