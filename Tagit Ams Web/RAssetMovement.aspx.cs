using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ECommerce.DataAccess;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.Utilities;
using ECommerce.Common;
using Serco;
using System.Drawing;
using Telerik.Web.UI;
using System.Globalization;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
//using DocumentFormat.OpenXml.Spreadsheet;

public partial class RAssetMovement : System.Web.UI.Page
{

    public static DataTable _dt_rt = new DataTable();
    public static DataTable _dt_rt1 = new DataTable();
    public static DataTable dtmodaldata = new DataTable();
    public static string path = "";
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    CompanyBL objcomp = new CompanyBL();
    SqlConnection con = new SqlConnection();

    public String _Order = System.Configuration.ConfigurationManager.AppSettings["ChangeGridOrder"];
    // Check User is Authorize to view this page
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }


    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    protected void gvData_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        try
        {
            gvData.ClientSettings.Scrolling.ScrollTop = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_PageIndexChanged", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_Init", path);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {


            string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
            FromDate = FromDate == null ? ToDate = null : FromDate;
            ToDate = ToDate == null ? FromDate = null : ToDate;
            DateTime dateValueFrom;
            DateTime dateValueTo;

            string DateFormat = "MM/dd/yyyy";
            if (DateTime.TryParseExact(FromDate, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out dateValueFrom)) { }

            if (DateTime.TryParseExact(ToDate, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out dateValueTo)) { }


            if (dateValueTo < dateValueFrom)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('ToDate always greater than from date');", true);
                txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                return;
            }

            grid_view();
            gvData.DataBind();
            grid_view_Custodian();
            gvData_Custodian.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "btnSearch_Click", path);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
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
                if (userAuthorize((int)pages.Statistics, Session["userid"].ToString()) == true)
                {
                    divSearch.Style.Add("display", "none");
                    // divbyCustodian.Visible = false;
                    //divbyCustodian.Style.Add("display", "none");
                    txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");

                    lblTotHeader.Visible = false;
                }
                else
                {
                    divSearch.Style.Add("display", "none");

                    txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    lblTotHeader.Visible = false;
                    //divbyCustodian.Visible = false;
                    Response.Redirect("AcceessError.aspx");
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "Page_Load", path);
        }
    }

    private void grid_view()
    {
        try
        {
            //objcomp.Insertlogmaster("AssetMovement:Grid Loading started");
            string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
            FromDate = FromDate == null ? ToDate = null : FromDate;
            ToDate = ToDate == null ? FromDate = null : ToDate;
            //DateTime dateValueFrom;
            //DateTime dateValueTo;

            //string DateFormat ="MM/dd/yyyy";
            //if (DateTime.TryParseExact(FromDate, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out dateValueFrom)) { }

            //    if (DateTime.TryParseExact(ToDate, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out dateValueTo)) { }


            //        if (dateValueTo < dateValueFrom)
            //        {
            //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('ToDate always greater than from date');", true);
            //            return;
            //        }
            //    DataAccessHelper1 help = new DataAccessHelper1(
            //StoredProcedures.GetTransferAssetsAccordingToDate, new SqlParameter[] {
            //                  new SqlParameter("@FromDate",  FromDate),
            //                  new SqlParameter("@Todate",  ToDate),
            //                   new SqlParameter("@Type",  "location"),
            //                        });
            DataSet ds = objcomp.GetTransferAssetsAccordingToDatedt(FromDate, ToDate, "location");


            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _dt_rt = ds.Tables[0];
                    DataTable dtlocation = new DataTable();

                    using (SqlCommand cmdloc = new SqlCommand("select lm.LocationName from LocationPermission as lp left join LocationMaster as lm on lm.LocationId = lp.LocationID where lp.UserID = @UserID", con))
                    {
                        cmdloc.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmdloc))
                        {
                            adp.Fill(dtlocation);
                        }
                    }
                    DataTable dtnew = new DataTable();
                    dtnew = _dt_rt.Clone();
                    for (int j = 0; j < dtlocation.Rows.Count; j++)
                    {
                        string locname = dtlocation.Rows[j]["LocationName"].ToString();
                        foreach (DataRow dr in _dt_rt.Rows)
                        {

                            string toloc = dr["ToLocation"].ToString();
                            if (toloc.Contains(locname))
                            {
                                dtnew.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    DataView dv = dtnew.DefaultView;
                    dv.Sort = "TransferCode desc";
                    DataTable sortedtable1 = dv.ToTable();
                    gvData.DataSource = sortedtable1;
                    lblTotHeader.Visible = true;

                    Session["gvdatatable"] = dtnew;
                }
                else
                {
                    lblTotHeader.Visible = false;
                    gvData.DataSource = string.Empty;
                }
            }
            else
            {
                lblTotHeader.Visible = false;
                gvData.DataSource = string.Empty;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
            }
            objcomp.Insertlogmaster("AssetMovement:Grid Loading Ended");
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "grid_view", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_NeedDataSource", path);
        }
    }
    public static DataTable dtExportToPdf = new DataTable();
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {//main
            if (e.CommandName == "TransferCode")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string HdnVerID = item["TransferID"].Text;
                string tsc = item["tsc"].Text;

                string REASON = item["REASON"].Text;
                string GPS_Location = item["GPS_Location"].Text;


                if (GPS_Location == "&nbsp;")
                {
                    GPS_Location = "-";
                }
                if (REASON == "&nbsp;")
                {
                    REASON = "-";
                }
                // Session["TransferCode"] = item["tsc"].Text;

                Session["REASON"] = REASON;
                Session["GPS_Location"] = GPS_Location;
                Session["TransferCode"] = tsc;
                ReportBL objReport = new ReportBL();
                AssetVerification objVer = new AssetVerification();
                DataSet ds = objVer.GetAssetTransferDeatilsByTransferID(HdnVerID, "location");
                DataTable dt_rt = new DataTable();
                dt_rt = ds.Tables[0];
                dtmodaldata = dt_rt;
                dtExportToPdf = dt_rt;
                if (dt_rt.Rows.Count > 0)
                {
                    string ToLocation = dt_rt.Rows[0]["ExistingLocation"].ToString();
                    string[] locName = ToLocation.Split('>');

                    Session["locName1"] = locName[0].ToString().Remove(locName[0].ToString().Length - 1, 1).ToString();
                    if (item["ToLocation"].Text.Contains("Document Returned"))
                    {
                        string tolocdata = item["ToLocation"].Text.ToString();
                        string[] locNamedata = tolocdata.Split('>');

                        Session["ToLocation"] = locNamedata[0].ToString().Remove(locNamedata[0].ToString().Length - 1, 1).ToString();

                        btnpdfDownload.Enabled = true;
                    }
                    else
                    {
                        btnpdfDownload.Enabled = false;
                    }
                }
                else
                {
                    Session["locName1"] = "";
                }
                DataTable dtExportToPdf1 = dtExportToPdf;
                Session["dtExportToPdf"] = dtExportToPdf;
                Session["dtExportToPdf1"] = dtExportToPdf1;
                string fromlocdata = dt_rt.Rows[0]["ExistingLocation"].ToString();
                string fromlocdata = dt_rt.Rows[0][""].ToString();
                string[] locNamedatafrom = fromlocdata.Split('>');
                Session["FromLocation"] = locNamedatafrom[0].ToString().Remove(locNamedatafrom[0].ToString().Length - 1, 1).ToString();
                gridDetails.DataSource = dt_rt;
                gridDetails.DataBind();
                LblDetails.Text = dt_rt.Rows.Count.ToString();
                ImgSignAsset.Visible = dt_rt.Rows[0]["Signimg"].ToString() != "";
                if (dt_rt.Rows[0]["Signimg"].ToString() != "")
                {
                    byte[] bytes = (byte[])dt_rt.Rows[0]["Signimg"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    ImgSignAsset.ImageUrl = "data:image/png;base64," + base64String;
                    SaveImage(base64String);
                }
                else
                {
                    FileInfo file = new FileInfo(Server.MapPath("image.jpg"));
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }
                }
                GriddetailsPopup.Show();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gv_data_ItemCommand", path);
        }
        finally
        {

        }
    }
    protected void gvData_Custodian_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "TransferCode")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string HdnVerID = item["TransferID"].Text;

                ReportBL objReport = new ReportBL();
                AssetVerification objVer = new AssetVerification();
                DataSet ds = objVer.GetAssetTransferDeatilsByTransferID(HdnVerID, "Custodian");
                DataTable dt_rt = new DataTable();
                dt_rt = ds.Tables[0];
                gridDetailsCustodian.DataSource = dt_rt;
                gridDetailsCustodian.DataBind();
                LblDetailsCustodian.Text = dt_rt.Rows.Count.ToString();
                Session["FromCustodian"] = item["FromCustodian"].Text;
                ImgSignCust.Visible = dt_rt.Rows[0]["Signimg"].ToString() != "";
                if (dt_rt.Rows[0]["Signimg"].ToString() != "")
                {
                    byte[] bytes = (byte[])dt_rt.Rows[0]["Signimg"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    ImgSignCust.ImageUrl = "data:image/png;base64," + base64String;

                }
                GriddetailsPopupCustodian.Show();
                //divbyCustodian.Style.Add("display", "inline");

                // divbyLocation.Style.Add("display", "none");
                //divbyCustodian.Visible = true; ;
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_Custodian_ItemCommand", path);
        }
        finally
        {

        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ////PrepareForExport(gridlist);
            ExportToExcel();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "BtnExportExcel_Click", path);
        }
    }

    private void ExportToExcel()
    {
        try
        {
            DataTable dtnewtransfer = new DataTable();
            DataTable rpt_DT = new DataTable();
            DataTable dtsubsheet = new DataTable();
            DataSet dssubsheet = new DataSet();
            AssetVerification objVer = new AssetVerification();
            string pathexcel = "", filename = "";
            if (divbyLocation.Visible == true)
            {
                if (_dt_rt != null)
                    rpt_DT = _dt_rt;
                pathexcel = Server.MapPath("~/LocationReport.xls");
                filename = "LocationReport.xls";
                dtnewtransfer.Columns.Add("TransferId");
                dtnewtransfer.Columns.Add("TransferCode");
                dtnewtransfer.Columns.Add("tsc");
                dtnewtransfer.Columns.Add("ToLocation");
                dtnewtransfer.Columns.Add("Trans_Type");
                dtnewtransfer.Columns.Add("GPS_Location");
                dtnewtransfer.Columns.Add("TransferredBy");
                dtnewtransfer.Columns.Add("CreateDate");
                dtnewtransfer.Columns.Add("REASON");
                foreach (GridDataItem item in gvData.MasterTableView.Items)
                {
                    DataRow newRow = dtnewtransfer.NewRow();
                    foreach (GridColumn col in gvData.MasterTableView.RenderColumns)
                    {
                        if (col.ColumnType == "GridBoundColumn")
                        {
                            string unique = col.UniqueName;
                            newRow[unique] = item[unique].Text;

                        }


                    }
                    dtnewtransfer.Rows.Add(newRow);
                }
                //newcode
                dtsubsheet.Columns.Add("TransferID");
                dtsubsheet.Columns.Add("AssetID");
                dtsubsheet.Columns.Add("AssetCode");
                dtsubsheet.Columns.Add("SerialNo");
                dtsubsheet.Columns.Add("Price");
                dtsubsheet.Columns.Add("CategoryName");
                dtsubsheet.Columns.Add("SubCatName");
                dtsubsheet.Columns.Add("ExistingLocation");
                dtsubsheet.Columns.Add("NewLocation");
                dtsubsheet.Columns.Add("CustodianName");
                dtsubsheet.Columns.Add("FCNumber");
                dtsubsheet.Columns.Add("CaseAssigneeName");
                dtsubsheet.Columns.Add("ClientName");
                dtsubsheet.Columns.Add("Signimg");
                for (int i = 0; i < dtnewtransfer.Rows.Count; i++)
                {
                    DataTable dtg = new DataTable();
                    dssubsheet = objVer.GetAssetTransferDeatilsByTransferID(dtnewtransfer.Rows[i]["TransferID"].ToString(), "location");
                    dtg = dssubsheet.Tables[0];
                    for (int j = 0; j < dtg.Rows.Count; j++)
                    {
                        DataRow drNew = dtsubsheet.NewRow();
                        drNew.ItemArray = dtg.Rows[j].ItemArray;
                        dtsubsheet.Rows.Add(drNew);
                    }

                }
                for (int x = 0; x < dtnewtransfer.Rows.Count; x++)
                {
                    dtnewtransfer.Rows[x]["GPS_Location"] = dtnewtransfer.Rows[x]["GPS_Location"].ToString().Replace("&nbsp;", " ");
                }
                dtsubsheet.Columns["AssetCode"].ColumnName = "Document Code";
                dtsubsheet.Columns["CategoryName"].ColumnName = "Document Category";
                dtsubsheet.Columns["CustodianName"].ColumnName = "From Custodian";

                dtsubsheet.Columns.Remove("SerialNo");
                dtsubsheet.Columns.Remove("Price");
                dtsubsheet.Columns.Remove("SubCatName");
                dtsubsheet.Columns.Remove("AssetID");
                dtsubsheet.Columns.Remove("From Custodian");
                // dtnewtransfer.Columns.Remove("TransferID");
                dtnewtransfer.Columns.Remove("TransferCode");
                dtnewtransfer.Columns["tsc"].ColumnName = "TransferCode";
            }
            else if (divbyCustodian.Visible == true)
            {
                if (_dt_rt1 != null)
                    rpt_DT = _dt_rt1;
                pathexcel = Server.MapPath("~/CustodianReport.xls");
                filename = "CustodianReport.xls";
                dtnewtransfer.Columns.Add("TransferId");
                dtnewtransfer.Columns.Add("TransferCode");
                dtnewtransfer.Columns.Add("FromCustodian");
                dtnewtransfer.Columns.Add("ToCustodian");
                dtnewtransfer.Columns.Add("Trans_Type");
                dtnewtransfer.Columns.Add("GPS_Location");
                dtnewtransfer.Columns.Add("TransferredBy");
                dtnewtransfer.Columns.Add("CreateDate");
                dtnewtransfer.Columns.Add("Reason");
                foreach (GridDataItem item in gvData_Custodian.MasterTableView.Items)
                {
                    DataRow newRow = dtnewtransfer.NewRow();
                    foreach (GridColumn col in gvData_Custodian.MasterTableView.RenderColumns)
                    {
                        if (col.ColumnType == "GridBoundColumn")
                        {
                            string unique = col.UniqueName;
                            newRow[unique] = item[unique].Text;
                        }
                    }
                    dtnewtransfer.Rows.Add(newRow);
                }
                for (int x = 0; x < dtnewtransfer.Rows.Count; x++)
                {
                    dtnewtransfer.Rows[x]["GPS_Location"] = dtnewtransfer.Rows[x]["GPS_Location"].ToString().Replace("&nbsp;", " ");
                }
                dtsubsheet.Columns.Add("TransferID");
                dtsubsheet.Columns.Add("TransferCode");
                dtsubsheet.Columns.Add("tsc");
                dtsubsheet.Columns.Add("FromCustodian");
                dtsubsheet.Columns.Add("ToCustodian");
                dtsubsheet.Columns.Add("CreatedDate");
                dtsubsheet.Columns.Add("TransferredBY");
                dtsubsheet.Columns.Add("Reason");
                dtsubsheet.Columns.Add("Trans_Type");
                dtsubsheet.Columns.Add("GPS_Location");
                for (int i = 0; i < dtnewtransfer.Rows.Count; i++)
                {
                    DataTable dtg = new DataTable();
                    string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
                    string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
                    FromDate = FromDate == null ? ToDate = null : FromDate;
                    ToDate = ToDate == null ? FromDate = null : ToDate;
                    dssubsheet = objcomp.GetTransferAssetsAccordingToDatedt(FromDate, ToDate, "Custodian");
                    dtg = dssubsheet.Tables[0];
                    for (int j = 0; j < dtg.Rows.Count; j++)
                    {
                        DataRow drNew = dtsubsheet.NewRow();
                        drNew.ItemArray = dtg.Rows[j].ItemArray;
                        dtsubsheet.Rows.Add(drNew);
                    }

                }

                dtsubsheet.Columns["TransferCode"].ColumnName = "Document Code";
                dtsubsheet.Columns["ToCustodian"].ColumnName = "To Custodian";
                dtsubsheet.Columns["FromCustodian"].ColumnName = "From Custodian";

                //dtsubsheet.Columns.Remove("SerialNo");
                // dtsubsheet.Columns.Remove("Price");
                //dtsubsheet.Columns.Remove("SubCatName");
                // dtsubsheet.Columns.Remove("AssetID");
                dtnewtransfer.Columns.Remove("TransferID");
                dtnewtransfer.Columns.Remove("TransferCode");
                dtsubsheet.Columns["tsc"].ColumnName = "TransferCode";
            }

            if (rpt_DT != null)
            {



                DataSet datasetexp = new DataSet();
                datasetexp.Tables.Add(dtnewtransfer);
                datasetexp.Tables.Add(dtsubsheet);
                ExportDataSet(datasetexp, Server.MapPath("~/" + filename + "").ToString());


                WebClient req = new WebClient();
                HttpResponse Response = HttpContext.Current.Response;
                string filePath = pathexcel;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + "");
                byte[] data = req.DownloadData(Server.MapPath("~/" + filename + ""));
                Response.BinaryWrite(data);
                Response.Flush();
                Response.End(); ;

            }
            else
            {
                // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "ExportToExcel", path);
        }
    }
    private void ExportDataSet(DataSet ds, string destination)
    {
        try
        {
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                foreach (System.Data.DataTable table in ds.Tables)
                {

                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }


                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }

                }
            }
            //Response.ContentType = "Application/x-msexcel";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=TransferReport.xls");
            //Response.TransmitFile(destination);
            //Response.End();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "ExportDataSet", path);
            Response.Redirect("RAssetMovement.aspx");
        }
    }

    protected void gvData_Custodian_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            grid_view_Custodian();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_Custodian_NeedDataSource", path);
        }
    }



    private void grid_view_Custodian()
    {
        try
        {
            string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
            string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
            FromDate = FromDate == null ? ToDate = null : FromDate;
            ToDate = ToDate == null ? FromDate = null : ToDate;

            DataSet ds = objcomp.GetTransferAssetsAccordingToDatedt(FromDate, ToDate, "Custodian");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //gvData_Custodian.DataSource = ds;
                    //lblTotHeader.Visible = true;
                    //_dt_rt1 = ds.Tables[0];
                    //Session["gvdatatable"] = ds.Tables[0];

                    //new code
                    _dt_rt1 = ds.Tables[0];
                    DataTable dtcustodian = new DataTable();

                    using (SqlCommand cmdloc = new SqlCommand("select cm.CustodianName from CustodianPermission as cp left join CustodianMaster as cm on cm.CustodianId = cp.CustodianId where cp.UserID = @UserID", con))
                    {
                        cmdloc.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmdloc))
                        {
                            adp.Fill(dtcustodian);
                        }
                    }
                    DataTable dtnew = new DataTable();
                    dtnew = _dt_rt1.Clone();
                    for (int j = 0; j < dtcustodian.Rows.Count; j++)
                    {
                        string cusname = dtcustodian.Rows[j]["CustodianName"].ToString();
                        foreach (DataRow dr in _dt_rt1.Rows)
                        {

                            string tocus = dr["ToCustodian"].ToString();
                            if (tocus.Contains(cusname))
                            {
                                dtnew.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    DataView dv = dtnew.DefaultView;
                    dv.Sort = "TransferCode desc";
                    DataTable sortedtable1 = dv.ToTable();
                    gvData_Custodian.DataSource = sortedtable1;
                    lblTotHeader.Visible = true;

                    Session["gvdatatable"] = dtnew;

                    //new code
                }
                else
                {
                    lblTotHeader.Visible = false;
                    gvData_Custodian.DataSource = string.Empty;
                }
            }
            else
            {
                lblTotHeader.Visible = false;
                gvData_Custodian.DataSource = string.Empty;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "grid_view_Custodian", path);
        }
    }
    protected void BtnByLocation_Click(object sender, EventArgs e)
    {
        try
        {
            divbyCustodian.Visible = false;
            divbyLocation.Visible = true;
            grid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "BtnByLocation_Click", path);
        }
    }
    protected void BtnByCustodian_Click(object sender, EventArgs e)
    {
        try
        {
            divbyCustodian.Visible = true;
            hdnCustodianGrid.Value = "show";

            divbyLocation.Visible = false;
            divbyCustodian.Style.Add("display", "block");
            grid_view_Custodian();
            gvData_Custodian.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "BtnByCustodian_Click", path);
        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;
                dataBoundItem["TransferCode"].ForeColor = Color.BlueViolet;
            }
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_Custodian_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;
                dataBoundItem["TransferCode"].ForeColor = Color.BlueViolet;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_Custodian_ItemDataBound", path);
        }
    }

    protected void gridDetails_PreRender(object sender, EventArgs e)
    {
        try
        {
            gridDetails.Columns[1].HeaderText = Assets.ToUpper() + " CODE";
            gridDetails.Columns[4].HeaderText = Category.ToUpper();
            gridDetails.Columns[5].HeaderText = SubCategory.ToUpper();
            AssetBL objAsset = new AssetBL();
            LabelConfigBL objLB = new LabelConfigBL();
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            ListMapping = objAsset.GetMappingListFromDB();
            DataSet ds = objLB.GetLabelConfigDetails("T6");
            DataTable dt_Col = new DataTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt_Col = ds.Tables[0];
            }

            int i = 1;
            foreach (var ColMap in ListMapping)
            {
                if (ColMap.ColumnName.Contains("Column"))
                {
                    if (ListMapping.Any(L => L.ColumnName == "Column" + i.ToString()) == true)
                    {
                        string MapSerialName = ListMapping.Where(x => x.ColumnName == "Column" + i.ToString()).SingleOrDefault().MappingColumnName;
                        foreach (DataGridColumn column1 in gridDetails.Columns)
                        {
                            if (column1.HeaderText.ToString().Contains("Column" + i.ToString()))
                            {
                                column1.HeaderText = MapSerialName.ToUpper();
                            }
                        }
                        i = i + 1;
                    }
                }
            }
            foreach (DataGridColumn column in gridDetails.Columns)
            {
                if (column.HeaderText.ToString().Contains("Column"))
                {
                    column.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gridDetails_PreRender", path);
        }

    }

    protected void gridDetailsCustodian_PreRender(object sender, EventArgs e)
    {
        try
        {
            gridDetailsCustodian.Columns[1].HeaderText = Assets.ToUpper() + " CODE";
            //gridDetailsCustodian.Columns[4].HeaderText = Category.ToUpper();
            //gridDetailsCustodian.Columns[5].HeaderText = SubCategory.ToUpper();


            AssetBL objAsset = new AssetBL();
            LabelConfigBL objLB = new LabelConfigBL();
            List<MappingInfo> ListMapping = new List<MappingInfo>();
            ListMapping = objAsset.GetMappingListFromDB();
            DataSet ds = objLB.GetLabelConfigDetails("T6");
            DataTable dt_Col = new DataTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt_Col = ds.Tables[0];
            }

            int i = 1;
            foreach (var ColMap in ListMapping)
            {
                if (ColMap.ColumnName.Contains("Column"))
                {
                    if (ListMapping.Any(L => L.ColumnName == "Column" + i.ToString()) == true)
                    {
                        string MapSerialName = ListMapping.Where(x => x.ColumnName == "Column" + i.ToString()).SingleOrDefault().MappingColumnName;
                        foreach (DataGridColumn column1 in gridDetailsCustodian.Columns)
                        {
                            if (column1.HeaderText.ToString().Contains("Column" + i.ToString()))
                            {
                                column1.HeaderText = MapSerialName.ToUpper();
                            }
                        }
                        i = i + 1;
                    }
                }
            }
            foreach (DataGridColumn column in gridDetailsCustodian.Columns)
            {
                if (column.HeaderText.ToString().Contains("Column"))
                {
                    column.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gridDetailsCustodian_PreRender", path);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string TransferCode = Session["TransferCode"].ToString();
            // Response.Write("<script>alert('"+ TransferCode + "');</script>");
            if (gridDetails.Items.Count > 0)
            {//Signimg
             //-----------------------------
             //dtExportToPdf.Columns["Signimg"].ColumnName = "SIGN IMG";
                GridView GridView2 = new GridView();
                GridView2.AllowPaging = false;
                DataTable dtxcel = dtExportToPdf;
                dtxcel.Columns["AssetCode"].ColumnName = "Document Code";
                dtxcel.Columns["CategoryName"].ColumnName = "Document Category";
                dtxcel.Columns["CustodianName"].ColumnName = "From Custodian";

                dtxcel.Columns.Remove("SerialNo");
                dtxcel.Columns.Remove("Price");
                dtxcel.Columns.Remove("SubCatName");
                dtxcel.Columns.Remove("TransferID");
                dtxcel.Columns.Remove("AssetID");
                dtxcel.Columns.Remove("From Custodian");


                GridView2.DataSource = dtxcel;
                GridView2.DataBind();


                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + TransferCode + "_" + DateTime.Now + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);


                    GridView2.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in GridView2.HeaderRow.Cells)
                    {
                        cell.BackColor = GridView2.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in GridView2.Rows)
                    {
                        row.BackColor = Color.White;
                        row.HorizontalAlign = HorizontalAlign.Center;
                        row.VerticalAlign = VerticalAlign.Middle;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = GridView2.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = GridView2.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    GridView2.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                //-----------------------------

                //using (XLWorkbook wb = new XLWorkbook())
                //{

                //    dtmodaldata.Columns["Signimg"].ColumnName = "SIGN IMG";
                //    wb.Worksheets.Add(dtmodaldata, "Sheet1");


                //    Response.Clear();
                //    Response.Buffer = true;
                //    Response.Charset = "";
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.AddHeader("content-disposition", "attachment;filename=" + TransferCode + "_" + DateTime.Now + ".xlsx");
                //    using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //        wb.SaveAs(MyMemoryStream);
                //        MyMemoryStream.WriteTo(Response.OutputStream);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "btnExport_Click", path);
        }
    }
    private string GetAbsoluteUrl(string relativeUrl)
    {
        try
        {
            relativeUrl = relativeUrl.Replace("~/", string.Empty);
            string[] splits = Request.Url.AbsoluteUri.Split('/');
            if (splits.Length >= 2)
            {
                string url = splits[0] + "//";
                for (int i = 2; i < splits.Length - 1; i++)
                {

                    url += splits[i];
                    url += "/";
                }

                return url + relativeUrl;
            }
            return relativeUrl;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "GetAbsoluteUrl", path);
            return null;
        }
    }

    protected void gvData_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "TransferCode")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string TransferId = item["TransferId"].Text;
                string ToLocation = item["ToLocation"].Text;
                //        DataAccessHelper1 help = new DataAccessHelper1(
                //StoredProcedures.GetTransferAssetsAccordingToDate, new SqlParameter[] {
                //                  new SqlParameter("@FromDate",  txtFrmDate.Text),
                //                  new SqlParameter("@Todate",  txtToDate.Text),
                //                   new SqlParameter("@Type",  "location"),
                //                        });
                DataSet ds = objcomp.GetTransferAssetsAccordingToDatedt(txtFrmDate.Text, txtToDate.Text, "location");

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        //GeneratePDF(dt);
                        //pdfTempData
                    }
                    else
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "gvData_ItemCommand", path);
        }
    }

    private void GeneratePDF(DataTable dataTable)
    {
        try
        {

            StringBuilder html = new StringBuilder();
            //html.Append("<html><head><style> .tbl{ border: 1px solid black; } .tablerow{ text-align:center; font - size:11px; } #trheader{ text-align:center; background-color:#d8d8d8!important;font-size:12px;}</style>");
            html.Append("<html><head>");
            html.Append("</head>");
            html.Append("<body>");




            html.Append("<table style='width: 100%'>");
            html.Append("<tr style='' class='bgcol'> ");
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>DOCUMENT CODE</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>DOCUMENT CATEGORY</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>FROM LOCATION</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>TO LOCATION</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>FROM CUSTODIAN</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>ASSIGNEE/DEPENDENT</th>");//-----------------Static
            html.Append("<th style='text-align:center;font-size:9px;font-weight: bold;'>NO.OF DOCUMENTS</th>");//-----------------Static
            html.Append("</tr>");
            //----------------------------Dynamic Binding Start
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                html.Append("<tr>");
                html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["AssetCode"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["CategoryName"].ToString() + "</td>");
                //html.Append("<td style='text-align:center;font-size:11px;'>" + Session["REASON"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["ExistingLocation"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["NewLocation"].ToString() + "</td>");
                if (dataTable.Columns.Contains("From Custodian"))
                {
                    html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["From Custodian"].ToString() + " </td>");
                }
                else
                {
                    html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["CustodianName"].ToString() + " </td>");
                }
                html.Append("<td style='text-align:center;font-size:9px;'>" + dataTable.Rows[i]["CaseAssigneeName"].ToString() + "</td>");
                html.Append("<td style='text-align:center;font-size:9px;'>" + "1" + "</td>");
                html.Append("</tr>");
            }
            //html.Append("<tr>");
            //html.Append("<td style='text-align:center;font-size:11px;'>039000002995</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Reason</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Document Returned</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Document Returned</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Afzal Amin</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>MASEKI, PRUDENCE KAVINYA</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>1</td>");
            //html.Append("</tr>");
            //html.Append("<tr>");
            //html.Append("<td style='text-align:center;font-size:11px;'>039000002995</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Reason</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Document Returned</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Document Returned</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>Afzal Amin</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>MASEKI, PRUDENCE KAVINYA</td>");
            //html.Append("<td style='text-align:center;font-size:11px;'>1</td>");
            //html.Append("</tr>");

            //----------------------------Dynamic Binding End
            html.Append("</table><br>");
            html.Append("</body></html>");

            //string imagePath = Server.MapPath("logo_ams.png");
            //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            //image.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            //image.ScaleToFit(180f, 250f);
            //Session["addresshtml"]
            string path = "";
            if (File.Exists(Server.MapPath("image.jpg")))
            {
                path = Server.MapPath("image.jpg");
            }
            else
            {
                path = Server.MapPath("images/bi.jpg");
            }
            string pathz = "";
            if (File.Exists(Server.MapPath(Session["imgPathhtml"].ToString())))
            {
                pathz = Server.MapPath(Session["imgPathhtml"].ToString());
            }
            else
            {
                pathz = Server.MapPath("images/bi.jpg");
            }
            string imagePath = pathz;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            image.ScaleToFit(130f, 100f);
            image.SetAbsolutePosition(460, 720);
            //SaveImage(ImgSignAsset.ImageUrl);
            string imagePathsign = path;
            iTextSharp.text.Image imagesign = iTextSharp.text.Image.GetInstance(imagePathsign);
            imagesign.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
            imagesign.ScaleToFit(100, 70);
            //imagesign.SetAbsolutePosition(460, 440);

            FontFactory.RegisterDirectories();

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.BORDER, "1");
            //styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FONTFAMILY, "Verdana");
            styles.LoadTagStyle(iTextSharp.text.html.HtmlTags.TABLE, iTextSharp.text.html.HtmlTags.FACE, "Calibri");
            styles.LoadTagStyle("bgcol", "color", "#d8d8d8");
            styles.LoadTagStyle(HtmlTags.TH, HtmlTags.BGCOLOR, "#d8d8d8");
            // string path = AppDomain.CurrentDomain.BaseDirectory + "/AAF/";
            System.IO.MemoryStream mStream = new System.IO.MemoryStream();
            using (iTextSharp.text.Document document1 = new iTextSharp.text.Document())
            {
                document1.SetPageSize(iTextSharp.text.PageSize.A4);
                document1.SetMargins(50, 50, 10, 40);

                iTextSharp.text.pdf.PdfWriter writer1 = iTextSharp.text.pdf.PdfWriter.GetInstance(document1, mStream);
                document1.Open();

                var value = Session["addresshtml"].ToString().Trim();
                if (value.Length < 50)
                {
                    string addr = "";
                    if (Session["addresshtml"].ToString().Length == 0)
                    {
                        addr = "";
                    }
                    else
                    {
                        addr = "Address : " + Session["addresshtml"].ToString();
                    }
                    iTextSharp.text.Paragraph Pr_Date1 = new iTextSharp.text.Paragraph(addr, iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 11.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                    iTextSharp.text.Paragraph Pr_Date2 = new iTextSharp.text.Paragraph("", iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 11.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                    Pr_Date1.Alignment = iTextSharp.text.Element.ALIGN_TOP;
                    Pr_Date2.Alignment = iTextSharp.text.Element.ALIGN_TOP;
                    StringBuilder html1 = new StringBuilder();
                    html1.Append("<div style='float: left;'><p>");
                    html1.Append(document1.Add(Pr_Date1));
                    html1.Append("</p><p>" + document1.Add(Pr_Date2));
                    html1.Append("</p></div>");
                    html1.Append("<div style='float: right; width: 200px;'><p>");
                    html1.Append(document1.Add(image));
                    html1.Append("</p></div>");

                    List<iTextSharp.text.IElement> objects1 = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
                    new StringReader(html1.ToString()), styles
                    );
                }
                else
                {
                    var firstHalfLength = (int)(value.Length / 2);
                    var secondHalfLength = value.Length - firstHalfLength;
                    var splitPhone = new[]
                        {
        value.Substring(0, firstHalfLength),
        value.Substring(firstHalfLength, secondHalfLength)
    };

                    iTextSharp.text.Paragraph Pr_Date1 = new iTextSharp.text.Paragraph("Address : " + splitPhone[0].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 11.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                    iTextSharp.text.Paragraph Pr_Date2 = new iTextSharp.text.Paragraph(splitPhone[1].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 11.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                    Pr_Date1.Alignment = iTextSharp.text.Element.ALIGN_TOP;
                    Pr_Date2.Alignment = iTextSharp.text.Element.ALIGN_TOP;
                    StringBuilder html1 = new StringBuilder();
                    html1.Append("<div style='float: left;'><p>");
                    html1.Append(document1.Add(Pr_Date1));
                    html1.Append("</p><p>" + document1.Add(Pr_Date2));
                    html1.Append("</p></div>");
                    html1.Append("<div style='float: right; width: 200px;'><p>");
                    html1.Append(document1.Add(image));
                    html1.Append("</p></div>");

                    List<iTextSharp.text.IElement> objects1 = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
                    new StringReader(html1.ToString()), styles
                    );
                }

                //document1.Add(image);
                document1.Add(new iTextSharp.text.Paragraph("\n"));
                iTextSharp.text.Paragraph Pr = new iTextSharp.text.Paragraph("\n TRANSFER REPORT", iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 16.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                Pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                iTextSharp.text.pdf.draw.LineSeparator underline = new iTextSharp.text.pdf.draw.LineSeparator(1, 35, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, -3);
                Pr.Add(underline);
                document1.Add(Pr);
                List<iTextSharp.text.IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
               new StringReader(html.ToString()), styles
               );
                iTextSharp.text.Paragraph Pr_Date = new iTextSharp.text.Paragraph("Date : " + DateTime.Now.ToShortDateString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Paragraph Pr_Time = new iTextSharp.text.Paragraph("Time : " + DateTime.Now.ToShortTimeString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Paragraph Pr_GPSLocation = new iTextSharp.text.Paragraph("GPS Location : " + Session["GPS_Location"].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                //iTextSharp.text.Paragraph Pr_Address = new iTextSharp.text.Paragraph("Address : " + Session["locName1"].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                iTextSharp.text.Paragraph Pr_Address = new iTextSharp.text.Paragraph("REASON : " + Session["REASON"].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                //iTextSharp.text.Paragraph Pr_TransferCode = new iTextSharp.text.Paragraph("TRANSFER CODE : " + Session["TransferCode"].ToString(), iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));


                document1.Add(new iTextSharp.text.Paragraph("\n"));

                document1.Add(Pr_Date);
                document1.Add(Pr_Time);
                document1.Add(Pr_GPSLocation);
                document1.Add(Pr_Address);

                document1.Add(new iTextSharp.text.Paragraph("\n"));
                Pr = new iTextSharp.text.Paragraph(" ");

                foreach (iTextSharp.text.IElement element in objects)
                {
                    document1.Add(element);
                }
                document1.Add(new iTextSharp.text.Paragraph("\n"));
                iTextSharp.text.Paragraph Prsign = new iTextSharp.text.Paragraph("Signature      ", iTextSharp.text.FontFactory.GetFont("Palatino Linotype", 12.0f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK));
                Prsign.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                //iTextSharp.text.pdf.draw.LineSeparator underlinesign = new iTextSharp.text.pdf.draw.LineSeparator(6, 30, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.Element.ALIGN_RIGHT, -3);
                //Prsign.Add(underlinesign);
                document1.Add(Prsign);
                document1.Add(imagesign);




            }
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=TransferReport.pdf");
            Response.Clear();
            Response.BinaryWrite(mStream.ToArray());

            Response.End();



        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "RAssetMovement.aspx", "GeneratePDF", path);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.ToString() + "')", true);
            //return;
            //Response.Redirect("RAssetMovement.aspx");
            //GriddetailsPopup.Hide();
        }

    }

    public void SaveImage(string base64img)
    {
        string outputImgFilename = Server.MapPath("image.jpg");
        //var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "imgs");
        //if (!System.IO.Directory.Exists(folderPath))
        //{
        //    System.IO.Directory.CreateDirectory(folderPath);
        //}
        System.IO.File.WriteAllBytes(outputImgFilename, Convert.FromBase64String(base64img));
    }
    protected void btnpdfDownload_Click(object sender, EventArgs e)
    {

        DataTable dt = objcomp.pdflogonAddressData(Session["FromLocation"].ToString());
        if (dt.Rows.Count > 0)
        {
            Session["addresshtml"] = dt.Rows[0]["address"].ToString();
            Session["imgPathhtml"] = dt.Rows[0]["imgPath"].ToString();
        }
        else
        {
            Session["addresshtml"] = "";
            Session["imgPathhtml"] = "";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No Location Details Found!! Please Add Details in PDF Configuration')", true);
            //return;
        }
        GeneratePDF((DataTable)Session["dtExportToPdf1"]);

    }

}