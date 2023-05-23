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
using System.Web.Services;
using System.IO;
using Serco;
using System.Drawing;
using Telerik.Web.UI;


using System.Text;
using System.Data.OleDb;
using ECommerce.Utilities;
using ECommerce.Common;


public partial class AssetIdentification : System.Web.UI.Page
{
    public static DataTable dt = new DataTable();
    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    public static string path = "";
    public static DataTable Exceldt = new DataTable();
    bool _MappingExist = false;
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];

    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];

    public bool MappingExist
    {
        get
        {
            return _MappingExist;
        }
        set
        {
            _MappingExist = value;
        }
    }

    public Boolean IsQuantitybase
    {
        get
        {
            return Convert.ToBoolean(ViewState["IsQuantitybase"]);
        }
        set
        {
            ViewState["IsQuantitybase"] = value;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "gvData_Init", path);

        }
    }

    public DataTable dt_cat
    {
        get
        {
            return ViewState["dt_cat"] as DataTable;
        }
        set
        {
            ViewState["dt_cat"] = value;

        }
    }
    public DataTable dt_subcat
    {
        get
        {
            return ViewState["dt_subcat"] as DataTable;
        }
        set
        {
            ViewState["dt_subcat"] = value;

        }
    }
    public DataTable dt_Loc
    {
        get
        {
            return ViewState["dt_Loc"] as DataTable;
        }
        set
        {
            ViewState["dt_Loc"] = value;

        }
    }
    public DataTable dt_Build
    {
        get
        {
            return ViewState["dt_Build"] as DataTable;
        }
        set
        {
            ViewState["dt_Build"] = value;

        }
    }
    public DataTable dt_floor
    {
        get
        {
            return ViewState["dt_floor"] as DataTable;
        }
        set
        {
            ViewState["dt_floor"] = value;

        }
    }
    public DataTable dt_department
    {
        get
        {
            return ViewState["dt_department"] as DataTable;
        }
        set
        {
            ViewState["dt_department"] = value;

        }
    }
    public DataTable dt_custodian
    {
        get
        {
            return ViewState["dt_custodian"] as DataTable;
        }
        set
        {
            ViewState["dt_custodian"] = value;

        }
    }
    public DataTable dt_supplier
    {
        get
        {
            return ViewState["dt_supplier"] as DataTable;
        }
        set
        {
            ViewState["dt_supplier"] = value;

        }
    }

    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                if (userAuthorize((int)pages.AssetIdentification, Session["userid"].ToString()))
                {
                    grid_view();
                    CompanyBL objcomp = new CompanyBL();
                    DataSet ds = objcomp.getUserSetting();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.IsQuantitybase = ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1" ? true : false;
                    }
                    else
                    {
                        this.IsQuantitybase = true;
                    }
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "gvData_NeedDataSource", path);
        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;
                dataBoundItem["VIEW"].ForeColor = Color.BlueViolet;
                dataBoundItem["Download1"].ForeColor = Color.BlueViolet;

                if (dataBoundItem["STATUS"].Text == "Approved")
                {
                    dataBoundItem["STATUS"].ForeColor = Color.Green;
                }

                if (dataBoundItem["STATUS"].Text == "Rejected")
                {
                    dataBoundItem["STATUS"].ForeColor = Color.Red; // chanmge particuler cell
                }

            }

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                // Get ColumnValue -- disable image from second row
                string strName = item["Status"].Text;
                if (strName.ToLower() == "approved")
                {
                    (item["APPROVE"].Controls[0] as ImageButton).Enabled = false;
                    (item["REJECT"].Controls[0] as ImageButton).Enabled = false;
                    (item["APPROVE"].Controls[0] as ImageButton).ImageUrl = "~/images/Approve Blus.png";
                    (item["REJECT"].Controls[0] as ImageButton).ImageUrl = "~/images/Reject Blur.png";
                }

                if (strName.ToLower() == "rejected")
                {
                    (item["APPROVE"].Controls[0] as ImageButton).Enabled = false;
                    (item["REJECT"].Controls[0] as ImageButton).Enabled = false;
                    (item["APPROVE"].Controls[0] as ImageButton).ImageUrl = "~/images/Approve Blus.png";
                    (item["REJECT"].Controls[0] as ImageButton).ImageUrl = "~/images/Reject Blur.png";
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "gvData_ItemDataBound", path);
        }
    }

    public void AddMasters(string TranId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PInsertMasterIdentifiedAndroid", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TranId", SqlDbType.VarChar).Value = TranId;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "AddMasters", path);
        }
    }

    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DataTable DT2 = new DataTable();
            if (e.CommandName == "APPROVE")
            {

                string ty = Session["Approve"].ToString();
                if (Session["Approve"].ToString() != "1")
                {
                    // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Only Approver can approve or reject the request.');", true);

                    string Message = "Only Approver can approve or reject the request.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }

                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["Trans_Id"].Text;
                string Status = item["STATUS"].Text;


                if (Status.ToLower() == "approved" || Status.ToLower() == "rejected")
                {
                    string Message = "Request already approved.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }

                //Update_request("Approved", ID, Type);
                AddMasters(ID);
                ImportAssets(ID);

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request approved successfully.');", true);


            }


            if (e.CommandName == "REJECT")
            {

                if (Session["Approve"].ToString() != "1")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Only Approver can approve or reject the request.');", true);

                    string Message = "Only Approver can approve or reject the request.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }

                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["Trans_Id"].Text;
                string Status = item["STATUS"].Text;


                if (Status.ToLower() == "approved" || Status.ToLower() == "rejected")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request already rejected.');", true);
                    string Message = Assets + " already rejected.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }
                //Update_request("Rejected", ID, Type);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request rejected successfully.');", true);

                string query = "Update AssetMasterIdentifiedAndroidHeader set Status='Rejected'  Where Trans_Id='" + ID + "'";
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, query);

                grid_view();
                gvData.DataBind();

                string Messages = Assets + " rejected successfully.";
                imgpopup.ImageUrl = "images/Success.png";
                lblpopupmsg.Text = Messages;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
            }

            if (e.CommandName == "VIEW")
            {
                Session["Identification_ID"] = "";
                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["Trans_Id"].Text;
                Session["Identification_ID"] = ID;

                Response.Redirect("AssetIdentificationDetails.aspx");
            }

            if (e.CommandName == "Download")
            {
                //lblMessage.Text = "";
                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["Trans_Id"].Text;

                DataTable dt_CheckExist = new DataTable();
                con.Open();
                using (SqlDataAdapter dad = new SqlDataAdapter())
                {
                    dad.SelectCommand = new SqlCommand("GetAssetIdentifiedDetails", con);
                    dad.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dad.SelectCommand.Parameters.Add("@TRANID", DbType.String).Value = ID;
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "dt_Check");
                    dt_CheckExist = ds.Tables["dt_Check"];
                }
                con.Close();

                DataTable Dt_Export = new DataTable();

                if (dt_CheckExist.Rows.Count == 0)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
                    return;
                }

                if (dt_CheckExist.Rows.Count > 0)
                {
                    Dt_Export = dt_CheckExist.Copy();
                }

                if (Dt_Export.Rows.Count > 0)
                {

                    try { Dt_Export.Columns.Remove("LocationId"); } catch { }
                    try { Dt_Export.Columns.Remove("BuildingId"); } catch { }
                    try { Dt_Export.Columns.Remove("FloorId"); } catch { }
                    try { Dt_Export.Columns.Remove("CategoryId"); } catch { }
                    try { Dt_Export.Columns.Remove("CategoryCode"); } catch { }
                    try { Dt_Export.Columns.Remove("SubCatId"); } catch { }
                    try { Dt_Export.Columns.Remove("SubCatCode"); } catch { }
                    try { Dt_Export.Columns.Remove("CustodianId"); } catch { }
                    try { Dt_Export.Columns.Remove("DepartmentId"); } catch { }
                    try { Dt_Export.Columns.Remove("AssetID"); } catch { }
                    try { Dt_Export.Columns.Remove("TranID"); } catch { }
                    try { Dt_Export.Columns.Remove("AssetId1"); } catch { }


                    //added by ponraj
                    try { Dt_Export.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets); } catch { }
                    try { Dt_Export.Columns["Location"].ColumnName = Location; } catch { }
                    try { Dt_Export.Columns["Building"].ColumnName = Building; } catch { }
                    try { Dt_Export.Columns["Floor"].ColumnName = Floor; } catch { }
                    try { Dt_Export.Columns["Category"].ColumnName = Category; } catch { }
                    try { Dt_Export.Columns["SubCategory"].ColumnName = SubCategory; } catch { }

                    AssetBL objAsset = new AssetBL();
                    List<MappingInfo> clientColumns = new List<MappingInfo>();
                    clientColumns = objAsset.GetMappingListFromDB();

                    foreach (DataColumn col in Dt_Export.Columns)
                    {
                        if (col.ColumnName.Contains("Column"))
                        {
                            var clientValue = clientColumns.Where(c => c.ColumnName == col.ColumnName.ToString().Trim());
                            foreach (var a in clientValue)
                            {
                                if (a.MappingColumnName != null && a.MappingColumnName != "")
                                {
                                    Dt_Export.Columns[a.ColumnName].ColumnName = a.MappingColumnName + "#";
                                }
                            }
                        }
                    }

                    DT2 = Dt_Export.Copy();
                    foreach (DataColumn column in DT2.Columns)
                    {
                        if (column.ColumnName.ToString().Contains("Column"))
                        {
                            Dt_Export.Columns.Remove(column.ColumnName);
                            // rpt_DT.AcceptChanges();
                        }
                    }

                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = Dt_Export;
                    GridView1.DataBind();


                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + ID + "-RequestDetails.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }

                    GridView1.RenderControl(hw);

                    //style to format numbers to string

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.Close();
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    grid_view();
                    gvData.DataBind();
                }

            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "gv_data_ItemCommand", path);
        }
        finally
        {

        }
    }

    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            try
            {
                SqlDataAdapter dpt = new SqlDataAdapter();
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTransaction_AssetIdentified");
                if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
                {
                }
                else
                {
                    dt = ds.Tables[0];
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    DataView myView;
                    myView = ds.Tables[0].DefaultView;
                    gvData.DataSource = myView;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "grid_view", path);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "grid_view", path);
        }
    }
    #region ApproveAssets

    public void ImportAssets(string ID)
    {
        try
        {
            //DataSet ds = new DataSet();
            DataSet ds = Common.GetIdentifiedAssets(ID);

            if (ds == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No data found.');", true);
                return;
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                ////this.Exceldt = ds.Tables[0];
                Exceldt = ds.Tables[0];

                AssetBL objAsset = new AssetBL();
                this.MappingExist = objAsset.CheckMappingExistsForTheClient();
                if (MappingExist == false)
                {

                }
                else
                {
                    List<MappingInfo> ListMapping = new List<MappingInfo>();
                    ListMapping = objAsset.GetMappingListFromDB();
                    bool ChkExistTagType = ListMapping.Any(cus => cus.ColumnName == "TagType");
                    if (ChkExistTagType == false)
                    {
                        MappingInfo mc = new MappingInfo();
                        mc.id = 100;
                        mc.ColumnName = "TagType";
                        mc.MappingColumnName = "TagType";
                        ListMapping.Add(mc);
                    }
                    //Added By Ponraj
                    bool ChkExistImageName = ListMapping.Any(cus => cus.ColumnName == "Image");
                    if (ChkExistImageName == false)
                    {
                        MappingInfo mc = new MappingInfo();
                        mc.id = 99;
                        mc.ColumnName = "Image";
                        mc.MappingColumnName = "Image";
                        ListMapping.Add(mc);
                    }
                    CreateTables();
                    if (ValidateData(Exceldt, ListMapping) == true)
                    {
                        if (ListMapping.Any(L => L.ColumnName == "SerialNo") == true)
                        {
                            string MapSerialName = ListMapping.Where(x => x.ColumnName == "SerialNo").SingleOrDefault().MappingColumnName;
                            object SerialNo = string.Join(",", Exceldt.AsEnumerable().Where(s => s.Field<object>(MapSerialName) != null).Select(s => s.Field<object>(MapSerialName)).ToArray<object>());

                            string SerialNo_ColumnNameinExel = MapSerialName;
                            /* Check duplicate serial number in Excel */
                            foreach (DataRow row in Exceldt.Rows)
                            {

                                string s = row[SerialNo_ColumnNameinExel].ToString();
                                string qry = "[" + SerialNo_ColumnNameinExel + "]= '" + s + "'";
                                DataRow[] drx = Exceldt.Select(qry);

                                if (drx.Length > 1)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Serial number should be unique..');", true);
                                    return;
                                }
                            }

                            DataTable Dt_Exists = new DataTable();
                            Dt_Exists = exist_SerialNumber_chk(SerialNo.ToString());
                            if (Dt_Exists.Rows.Count > 0)
                            {
                                if (this.IsQuantitybase == true)
                                {
                                    string Sno = string.Join(",", Dt_Exists.AsEnumerable().Select(s => s["SerialNo"].ToString()).ToArray<string>());
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The below given " + Assets + " with Serial No already exists,cannot import " + Assets + ". - " + Sno.ToString() + "');", true);
                                }
                                else
                                {
                                    ValidateExcelAndBindGrid(Exceldt, ListMapping, ID);
                                }
                            }
                            else
                            {
                                ValidateExcelAndBindGrid(Exceldt, ListMapping, ID);
                            }

                        }
                        else
                        {
                            ValidateExcelAndBindGrid(Exceldt, ListMapping, ID);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "ImportAssets", path);
        }
    }

    // Check ExcelHeader column Exists in the List of Mapping
    private bool CheckColumnExists(string tblHeaderCoulmn, List<CurrentMapping> ListMapping)
    {
        if (ListMapping.Any(M => M.MappingColumnName == tblHeaderCoulmn))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Add Cuurent Mapping to the list
    private List<CurrentMapping> GetCurrentMapping(List<MappingInfo> ListMapping)
    {
        try
        {
            List<CurrentMapping> objMap = new List<CurrentMapping>();
            foreach (var item in ListMapping)
            {
                CurrentMapping CurrMap = new CurrentMapping();
                CurrMap.id = item.id;
                CurrMap.ColumnName = item.ColumnName;
                CurrMap.MappingColumnName = item.MappingColumnName;
                objMap.Add(CurrMap);
            }
            return objMap;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetCurrentMapping", path);
            return null;
        }
    }

    // Add coumn to DataTable
    private void BindColumntoExcel(DataTable Exceldt)
    {
        try
        {
            Exceldt.Columns.Add(new DataColumn("CategoryId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("ECatCode", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("SubCatId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("ESubCatCode", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("LocationId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("BuildingId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("FloorId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("DepartmentId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("CustodianId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("SupplierId", Type.GetType("System.Int32")));
            Exceldt.Columns.Add(new DataColumn("Column1", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column2", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column3", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column4", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column5", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column6", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column7", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column8", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column9", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column10", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column11", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column12", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column13", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column14", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("Column15", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NSerialNo", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NDescription", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NQuantity", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NPrice", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NDeliveryDate", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NAssignDate", Type.GetType("System.String")));
            Exceldt.Columns.Add(new DataColumn("NActive", Type.GetType("System.String")));
            //Exceldt.Columns.Add(new DataColumn("Image", Type.GetType("System.String")));

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "BindColumntoExcel", path);
        }

    }

    public static void SetColumnsOrder(DataTable table, List<MappingInfo> ListMapping)
    {
        try
        {
            String[] _Columns = new String[5];
            _Columns[0] = "CategoryName";
            _Columns[1] = "SubCategoryName";
            _Columns[2] = "LocationName";
            _Columns[3] = "BuildingName";
            _Columns[4] = "FloorName";
            int i = 0;
            foreach (var columnName in ListMapping)
            {
                if (columnName.ColumnName.ToString() == _Columns[i])
                {
                    table.Columns[columnName.MappingColumnName].SetOrdinal(i);
                    i++;
                    break;
                }

            }
            //foreach (var columnName in ListMapping)
            //{
            //    switch (columnName.ColumnName.ToString())
            //    {
            //        case "CategoryName":
            //            {
            //                table.Columns[columnName.MappingColumnName].SetOrdinal(0);
            //                break;
            //            }
            //        case "SubCategoryName":
            //            {
            //                table.Columns[columnName.MappingColumnName].SetOrdinal(1);
            //                break;
            //            }
            //        case "LocationName":
            //            {
            //                table.Columns[columnName.MappingColumnName].SetOrdinal(2);
            //                break;
            //            }
            //        case "BuildingName":
            //            {
            //                table.Columns[columnName.MappingColumnName].SetOrdinal(3);
            //                break;
            //            }
            //        case "FloorName":
            //            {
            //                table.Columns[columnName.MappingColumnName].SetOrdinal(4);
            //                break;
            //            }
            //    }
            //}
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "SetColumnsOrder", path);
        }
    }

    private void ValidateExcelAndBindGrid(DataTable Exceldt, List<MappingInfo> ListMapping, string ID)
    {
        try
        {
            SetColumnsOrder(Exceldt, ListMapping);
            StringBuilder sb = new StringBuilder();
            int MaxID = 0;
            int TotalColumn = Exceldt.Columns.Count;
            BindColumntoExcel(Exceldt);
            int i = 0;
            int j = 1;
            int colCount = 0;
            List<CurrentMapping> CurrentMapping = GetCurrentMapping(ListMapping);//To keep the current Map

            foreach (DataRow dr in Exceldt.Rows)
            {
                foreach (var Coulmn in Exceldt.Columns)
                {
                    if (++colCount > TotalColumn) break;
                    string tblHeaderCoulmn = Coulmn.ToString();

                    bool ColumnExists = CheckColumnExists(tblHeaderCoulmn, CurrentMapping);

                    if (ColumnExists)
                    {
                        var MappingItem = ListMapping.Where(M => M.MappingColumnName == tblHeaderCoulmn).ToList();
                        string DBColumnname = MappingItem[0].ColumnName.ToString();
                        string ExcelValue = dr[tblHeaderCoulmn].ToString();
                        ExcelValue = ExcelValue.ToString().Replace("'", "''");
                        switch (DBColumnname)
                        {
                            case "CategoryName":
                                {
                                    DataRow[] Exist_CatName = dt_cat.Select("CategoryName='" + ExcelValue.Trim() + "'");

                                    if (Exist_CatName.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " not Exists in the master" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["CategoryId"] = Convert.ToInt32(Exist_CatName[0].ItemArray[0]);
                                        Exceldt.Rows[i]["ECatCode"] = Exist_CatName[0].ItemArray[2].ToString();

                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }

                            case "SubCategoryName":
                                {
                                    DataRow[] Exist_SubCatName = dt_subcat.Select("SubCatName='" + ExcelValue.Trim() + "'");

                                    if (Exist_SubCatName.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + SubCategory + " not available in  master" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["SubCatId"] = Convert.ToInt32(Exist_SubCatName[0].ItemArray[0]);
                                        Exceldt.Rows[i]["ESubCatCode"] = Exist_SubCatName[0].ItemArray[2].ToString();

                                        Exceldt.AcceptChanges();

                                        string CategoryId = Convert.ToString(Exceldt.Rows[i]["CategoryId"]);
                                        DataRow[] drData = dt_cat.Select("CategoryId='" + CategoryId + "'");
                                        if (drData != null && drData.Length == 0)
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " not found for this " + SubCategory + " in  master" + "');", true);
                                            return;
                                        }
                                        string ExcelColumnName = CurrentMapping.Where(x => x.ColumnName == "SubCategoryName").FirstOrDefault().MappingColumnName;
                                        string FurnitureNameFromExcel = Exceldt.Rows[i][ExcelColumnName].ToString();
                                        DataRow[] drSubcat = dt_subcat.Select("SubCatName='" + FurnitureNameFromExcel.Trim() + "'" + " and CategoryId= '" + CategoryId.Trim() + "'");
                                        if (drSubcat.Length == 0)
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " - " + Category + " and " + SubCategory + " is not mattched in  master" + "');", true);
                                            return;
                                        }

                                    }
                                    break;
                                }

                            case "FloorName":
                                {
                                    string _Building = Exceldt.Rows[i]["BuildingId"].ToString();
                                    string _Location = Exceldt.Rows[i]["LocationId"].ToString();
                                    DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue.Trim() + "' and BuildingID ='" + _Building + "' and LocationId ='" + _Location + "'");

                                    //DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue.Trim() + "'");
                                    if (Exist_Floor.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Floor + " is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["FloorId"] = Convert.ToInt32(Exist_Floor[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "BuildingName":
                                {
                                    string _Location = Exceldt.Rows[i]["LocationId"].ToString();
                                    DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + ExcelValue.Trim() + "' and LocationId ='" + _Location + "'");
                                    if (Exist_BUILDING.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Building + "  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["BuildingId"] = Convert.ToInt32(Exist_BUILDING[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "LocationName":
                                {
                                    DataRow[] Exist_LOCATION = dt_Loc.Select("LocationName='" + ExcelValue.Trim() + "'");

                                    if (Exist_LOCATION.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Location + "  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["LocationId"] = Convert.ToInt32(Exist_LOCATION[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                        string LocationId = "", BuildingId = "", FloorId = "", LocationNmfrmExcel = "", LocationTextFromExcel = "";

                                        var LocInfo = CurrentMapping.Where(x => x.ColumnName == "LocationName").ToList();
                                        if (LocInfo.Count > 0)
                                        {
                                            LocationNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "LocationName").FirstOrDefault().MappingColumnName;
                                            LocationTextFromExcel = Exceldt.Rows[i][LocationNmfrmExcel].ToString();
                                        }

                                        var BldInfo = CurrentMapping.Where(x => x.ColumnName == "BuildingName").ToList();
                                        string BuildingNmfrmExcel = "", BuildingTextFromExcel = "";
                                        if (BldInfo.Count > 0)
                                        {
                                            BuildingNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "BuildingName").FirstOrDefault().MappingColumnName; ;
                                            BuildingTextFromExcel = Exceldt.Rows[i][BuildingNmfrmExcel].ToString();
                                        }


                                        var FlrInfo = CurrentMapping.Where(x => x.ColumnName == "FloorName").ToList();
                                        string FloorNmfrmExcel = "", FloorTextFromExcel = "", FloorNmfromExcel = "";
                                        if (FlrInfo.Count > 0)
                                        {
                                            FloorNmfrmExcel = CurrentMapping.Where(x => x.ColumnName == "FloorName").FirstOrDefault().MappingColumnName;
                                            FloorTextFromExcel = Convert.ToString(Exceldt.Rows[i][FloorNmfrmExcel]);
                                            FloorNmfromExcel = CurrentMapping.Where(x => x.ColumnName == "FloorName").FirstOrDefault().MappingColumnName;
                                        }

                                        DataRow[] drLoc = dt_Loc.Select("LocationName='" + LocationTextFromExcel.Trim() + "'");
                                        if (drLoc != null && drLoc.Length > 0)
                                        {
                                            LocationId = drLoc[0].ItemArray[0].ToString();

                                        }



                                        if (BldInfo.Count > 0)
                                        {
                                            DataRow[] drBuilding = dt_Build.Select("BuildingName='" + BuildingTextFromExcel.Trim() + "'" + " and LocationId= '" + LocationId.Trim() + "'");
                                            if (drBuilding.Length == 0)
                                            {
                                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - " + Building + " is not mapped with existing location master " + "');", true);
                                                return;
                                            }
                                            if (drBuilding.Length > 0)
                                            {
                                                BuildingId = Convert.ToString(drBuilding[0].ItemArray[0]);
                                                DataRow[] drFloor = dt_floor.Select("FloorName='" + FloorTextFromExcel.Trim() + "'" + " and BuildingId='" + BuildingId.Trim() + "'");

                                                if (FlrInfo.Count > 0)
                                                {
                                                    if (drLoc.Length == 0 || drBuilding.Length == 0 || drFloor.Length == 0)
                                                    {
                                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + FloorTextFromExcel.Trim() + " -  " + Floor + " is not mapped with existing Building and location in master" + "');", true);
                                                        return;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    break;
                                }
                            case "DepartmentName":
                                {
                                    DataRow[] Exist_DEPARTMENT = dt_department.Select("DepartmentName='" + ExcelValue.Trim() + "'");
                                    if (Exist_DEPARTMENT.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Department  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["DepartmentId"] = Convert.ToInt32(Exist_DEPARTMENT[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "CustodianName":
                                {
                                    DataRow[] Exist_CUSTODIAN = dt_custodian.Select("CustodianName='" + ExcelValue.Trim() + "'");
                                    if (Exist_CUSTODIAN.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Custodian  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["CustodianId"] = Convert.ToInt32(Exist_CUSTODIAN[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }
                            case "SupplierName":
                                {
                                    DataRow[] Exist_SUPPLIERNAME = dt_supplier.Select("SupplierName='" + ExcelValue.Trim() + "'");
                                    if (Exist_SUPPLIERNAME.Length == 0)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.Trim() + " - Supplier  is not available in masters" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        Exceldt.Rows[i]["SupplierId"] = Convert.ToInt32(Exist_SUPPLIERNAME[0].ItemArray[0]);
                                        Exceldt.AcceptChanges();
                                    }
                                    break;
                                }

                            case "SerialNo":
                                {

                                    if (ExcelValue.ToString() == "")
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue.ToString() + " - Serial number should not be empty" + "');", true);
                                        return;
                                    }

                                    Exceldt.Rows[i]["NSerialNo"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Quantity":
                                {
                                    if (this.IsQuantitybase == false && Convert.ToInt32(ExcelValue) > 1)
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('While Import Quantity Should not be greater than 1');", true);
                                        return;
                                    }
                                    else
                                    {
                                        int value;
                                        if (int.TryParse(ExcelValue, out value))
                                        {
                                            Exceldt.Rows[i]["NQuantity"] = ExcelValue.ToString();
                                            Exceldt.AcceptChanges();
                                        }
                                        else
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ExcelValue + " -Quantity should be a integer value" + "');", true);
                                            return;
                                        }
                                    }

                                    break;
                                }
                            case "Description":
                                {
                                    Exceldt.Rows[i]["NDescription"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Price":
                                {
                                    Exceldt.Rows[i]["NPrice"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "DeliveryDate":
                                {
                                    Exceldt.Rows[i]["NDeliveryDate"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "AssignDate":
                                {
                                    Exceldt.Rows[i]["NAssignDate"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Active":
                                {
                                    Exceldt.Rows[i]["NActive"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();

                                    break;
                                }
                            case "Column1":
                                {
                                    Exceldt.Rows[i]["Column1"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column2":
                                {
                                    Exceldt.Rows[i]["Column2"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column3":
                                {
                                    Exceldt.Rows[i]["Column3"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column4":
                                {
                                    Exceldt.Rows[i]["Column4"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column5":
                                {
                                    Exceldt.Rows[i]["Column5"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column6":
                                {
                                    Exceldt.Rows[i]["Column6"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column7":
                                {
                                    Exceldt.Rows[i]["Column7"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column8":
                                {
                                    Exceldt.Rows[i]["Column8"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column9":
                                {
                                    Exceldt.Rows[i]["Column9"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column10":
                                {
                                    Exceldt.Rows[i]["Column10"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column11":
                                {
                                    Exceldt.Rows[i]["Column11"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column12":
                                {
                                    Exceldt.Rows[i]["Column12"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column13":
                                {
                                    Exceldt.Rows[i]["Column13"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column14":
                                {
                                    Exceldt.Rows[i]["Column14"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Column15":
                                {
                                    Exceldt.Rows[i]["Column15"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }
                            case "Image":
                                {
                                    Exceldt.Rows[i]["Image"] = ExcelValue.ToString();
                                    Exceldt.AcceptChanges();
                                    break;
                                }


                        }
                    }
                    else
                    {
                        if (j <= 15)
                        {
                            MappingInfo objInfo = new MappingInfo();
                            string ExcelValue = dr[tblHeaderCoulmn].ToString();
                            string dbColumn = "Column" + j;
                            Exceldt.Rows[i]["Column" + j] = ExcelValue;
                            Exceldt.AcceptChanges();
                            if (ListMapping.Any(M => M.ColumnName == dbColumn) == false)
                            {
                                var maxID = ListMapping.Count();
                                switch (dbColumn)
                                {
                                    case "Column1":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column2":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column3":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column4":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column5":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column6":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column7":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column8":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column9":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column10":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column11":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column12":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column13":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column14":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                    case "Column15":
                                        {
                                            objInfo.id = j;
                                            objInfo.ColumnName = dbColumn;
                                            objInfo.MappingColumnName = tblHeaderCoulmn;
                                            ListMapping.Add(objInfo);
                                            break;
                                        }
                                }
                            }
                            j++;
                        }
                    }

                }
                i++;
                colCount = 0;
                j = 1;
            }


            //MaxID = InsertImportedFile();
            //if (MaxID > 0)
            //{
            BindImportAssetToGrid(Exceldt, ListMapping, this.MappingExist);


            string query = "Update AssetMasterIdentifiedAndroidHeader set Status='Approved'  Where Trans_Id='" + ID + "'";
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, query);

            grid_view();
            gvData.DataBind();

            string Messages = Assets + " approved successfully.";
            imgpopup.ImageUrl = "images/Success.png";
            lblpopupmsg.Text = Messages;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();

            //}
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "ValidateExcelAndBindGrid", path);
        }
    }

    private DataTable exist_SerialNumber_chk(string SerialNo)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DataSet ds_exist = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "CheckSerialNos", new SqlParameter[] {
                        new SqlParameter("@SerialNo", SerialNo),
                 });
            if (ds_exist.Tables[0].Rows.Count > 0)
            {
                return ds_exist.Tables[0];
            }
            else
            {
                return ds_exist.Tables[0];
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "exist_SerialNumber_chk", path);
            return null;
        }
    }

    // compare matching column name between excelsheet and List of Mapping
    private bool ValidateData(DataTable Exceldt, List<MappingInfo> ListMapping)
    {


        if (MappingExist == true)
        {
            ////Check Columns Count
            //if (Exceldt.Columns.Count.ToString() != ListMapping.Count().ToString())
            //{

            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Excel File Dont have all columns which are required..');", true);
            //    return false;
            //}


            if (Exceldt.Rows.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No dtat found..');", true);
                return false;
            }
            // Exceldt.Columns.Remove("Image");
            if (Exceldt.Columns.Count != ListMapping.Count)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Data does not contain all the column you mapped.');", true);
                return false;
            }


            // Loop On Columns
            for (int i = 0; i < ListMapping.Count; i++)
            {
                if (ListMapping.Any(L => L.MappingColumnName == Exceldt.Columns[i].ColumnName) == false)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Column " + Exceldt.Columns[i].ColumnName + " is Invalid.');", true);
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }


    }

    public int InsertImportedFile()
    {
        try
        {
            int MaxId = 0;
            try
            {

                MaxId = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "PinsertExcelTransaction", new SqlParameter[] {
                        new SqlParameter("@UserId",Session["userid"] .ToString() ),
                        new SqlParameter("@FileName","" ),
                         new SqlParameter("@CreatedDate",System.DateTime.Now ),
                          new SqlParameter("@Type","AssetIdentifiedMaster" ),
                           new SqlParameter("@IsDeleted","0" ),

                 }));


            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            }
            return MaxId;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "InsertImportedFile", path);
            return 0;
        }
    }

    // Import Excel Date To DataBase
    private void BindImportAssetToGrid(DataTable Exceldt, List<MappingInfo> ListMapping, bool IsMappingExist)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        con.Open();
        int i = 0;
        using (SqlTransaction Trans = con.BeginTransaction())
        {
            try
            {
                int MaxID = 0;
                MaxID = InsertImportedFile();
                if (IsMappingExist == false)
                {
                    foreach (var item in ListMapping)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportMapping", new SqlParameter[] {
                          new SqlParameter("@columnName", item.ColumnName.ToString()),
                             new SqlParameter("@MappingColumnName", item.MappingColumnName.ToString()),

                     });
                    }
                }

                foreach (DataRow dr in Exceldt.Rows)
                {


                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PImportAsset", new SqlParameter[] {
                             new SqlParameter("@TranID", MaxID),
                             new SqlParameter("@CategoryId", Convert.ToInt32(dr["CategoryId"])),
                             new SqlParameter("@CategoryCode", Convert.ToString(dr["ECatCode"])),
                             new SqlParameter("@SubCategoryId", Convert.ToString(dr["SubCatId"])),
                             new SqlParameter("@SubCategoryCode", Convert.ToString(dr["ESubCatCode"])),
                             new SqlParameter("@BuildingId", Convert.ToString(dr["BuildingId"])),
                             new SqlParameter("@FloorId", Convert.ToString(dr["FloorId"])),
                             new SqlParameter("@LocationId", Convert.ToString(dr["LocationId"])),
                             new SqlParameter("@DepartmentId", Convert.ToString(dr["DepartmentId"])),
                             new SqlParameter("@CustodianId", Convert.ToString(dr["CustodianId"])),
                             new SqlParameter("@SupplierId", Convert.ToString(dr["SupplierId"])),
                             new SqlParameter("@SerialNo", Convert.ToString(dr["NSerialNo"]).Trim()),
                             new SqlParameter("@Description", Convert.ToString(dr["NDescription"]).Trim()),
                             new SqlParameter("@Quantity", Convert.ToString(dr["NQuantity"]).Trim()),
                             new SqlParameter("@Price", Convert.ToString(dr["NPrice"]).Trim()),
                             new SqlParameter("@DeliveryDate", dr["NDeliveryDate"].ToString()),
                             new SqlParameter("@AssignDate", dr["NAssignDate"].ToString()),
                             new SqlParameter("@Active", Convert.ToString(dr["NActive"])),
                             new SqlParameter("@Column1", Convert.ToString(dr["Column1"])),
                             new SqlParameter("@Column2", Convert.ToString(dr["Column2"])),
                             new SqlParameter("@Column3", Convert.ToString(dr["Column3"])),
                             new SqlParameter("@Column4", Convert.ToString(dr["Column4"])),
                             new SqlParameter("@Column5", Convert.ToString(dr["Column5"])),
                             new SqlParameter("@Column6", Convert.ToString(dr["Column6"])),
                             new SqlParameter("@Column7", Convert.ToString(dr["Column7"])),
                             new SqlParameter("@Column8", Convert.ToString(dr["Column8"])),
                             new SqlParameter("@Column9", Convert.ToString(dr["Column9"])),
                             new SqlParameter("@Column10", Convert.ToString(dr["Column10"])),
                             new SqlParameter("@Column11", Convert.ToString(dr["Column11"])),
                             new SqlParameter("@Column12", Convert.ToString(dr["Column12"])),
                             new SqlParameter("@Column13", Convert.ToString(dr["Column13"])),
                             new SqlParameter("@Column14", Convert.ToString(dr["Column14"])),
                             new SqlParameter("@Column15", Convert.ToString(dr["Column15"])),
                             new SqlParameter("@ImportType", "Master"),
                             new SqlParameter("@Image", Convert.ToString(dr["Image"])),
                             new SqlParameter("@TAGTYPE", Convert.ToString(dr["TAGTYPE"])),

                 });
                    i++;
                }

                Trans.Commit();
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Imported Successfully.');", true);

                string Message = "Imported Successfully.";
                imgpopup.ImageUrl = "images/Success.png";
                lblpopupmsg.Text = Message;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
            }
            catch (Exception ex)
            {
                i = i + 1;
                Trans.Rollback();
                Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "BindImportAssetToGrid", path);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " error at line number in excel file- " + (i) + "');", true);

            }
        }

    }

    private void CreateTables()
    {
        try
        {
            this.dt_cat = GetActiveCategory();
            this.dt_subcat = GetActiveSubCategory();
            this.dt_Loc = GetActiveLocatio();
            this.dt_Build = GetActiveBuilding();
            this.dt_floor = GetActiveFloor();
            this.dt_department = GetActiveDepartment();
            this.dt_custodian = GetCustodian();
            this.dt_supplier = GetActiveSupplier();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "CreateTables", path);
        }
    }

    private DataTable GetActiveSupplier()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getsupplier");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveSupplier", path);
            return null;
        }
    }

    private DataTable GetCustodian()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveCustodian");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetCustodian", path);
            return null;
        }
    }

    private DataTable GetActiveDepartment()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveDepartment", path);
            return null;
        }
    }

    private DataTable GetActiveFloor()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveFloor");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveFloor", path);
            return null;
        }
    }

    private DataTable GetActiveBuilding()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveBuilding");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveBuilding", path);
            return null;
        }

    }

    private DataTable GetActiveLocatio()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveLocatio", path);
            return null;
        }
    }

    private DataTable GetActiveSubCategory()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveSubCat");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveSubCategory", path);
            return null;
        }
    }

    private DataTable GetActiveCategory()
    {
        try
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");
            con.Close();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "AssetIdentification.aspx", "GetActiveCategory", path);
            return null;
        }
    }

    #endregion
}