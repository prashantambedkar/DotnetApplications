using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ECommerce.Common;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Serco;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using Serco;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

public partial class Getasset : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    public static DataTable dt_Fileter_ToLoc = new DataTable();

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

    public DataTable dtAssetDetails
    {
        get
        {
            return ViewState["dtAssetDetails"] as DataTable;
        }
        set
        {
            ViewState["dtAssetDetails"] = value;

        }
    }
    public DataTable dt_Reports_THS
    {
        get
        {
            return ViewState["dt_Reports_THS"] as DataTable;
        }
        set
        {
            ViewState["dt_Reports_THS"] = value;

        }
    }
    public DataTable dt_Reports_THR
    {
        get
        {
            return ViewState["dt_Reports_THR"] as DataTable;
        }
        set
        {
            ViewState["dt_Reports_THR"] = value;

        }
    }
    public DataTable dt_result
    {
        get
        {
            return ViewState["dt_result"] as DataTable;
        }
        set
        {
            ViewState["dt_result"] = value;

        }
    }
    public string TLocationID
    {
        get
        {
            return ViewState["TLocationID"].ToString();
        }
        set
        {
            ViewState["TLocationID"] = value;
        }
    }
    public string TBuildingID
    {
        get
        {
            return ViewState["TBuildingID"].ToString();
        }
        set
        {
            ViewState["TBuildingID"] = value;
        }
    }
    public string TFloorID
    {
        get
        {
            return ViewState["TFloorID"].ToString();
        }
        set
        {
            ViewState["TFloorID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CreateTables();
            lblTotHeader.Visible = false;
            this.dtAssetDetails = null;
            bindstatus();
        }
    }

    private void bindstatus()
    {
        try
        {
            DataTable dt_status = new DataTable();
            dt_status.Columns.Add("Status");

            dt_status.Rows.Add("All");
            dt_status.Rows.Add("Missing");
            dt_status.Rows.Add("Found");
            dt_status.Rows.Add("Extra");
            dt_status.Rows.Add("Mismatch");

            ddlStatus.DataSource = dt_status;
            ddlStatus.DataTextField = "Status";
            ddlStatus.DataValueField = "Status";
            ddlStatus.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (dt_result != null)
            {
                lblTotHeader.Visible = true;
                if (ddlStatus.SelectedValue == "All")
                {
                    gridlist.DataSource = dt_result;
                    gridlist.DataBind();
                    lblcnt.Text = dt_result.Rows.Count.ToString();

                    BtnAccept.Visible = false;
                    BtnReject.Visible = false;
                    gridlist.Columns[11].Visible = false;
                    gridlist.Columns[10].Visible = false;
                }
                else
                {
                    DataView dv = new DataView(dt_result);
                    if (ddlStatus.SelectedValue.ToString() == "Mismatch")
                    {
                        dv.RowFilter = "Status='" + ddlStatus.SelectedValue.ToString() + "'";
                        gridlist.DataSource = dv;
                        gridlist.DataBind();

                        if (dv.Count > 0)
                        {
                            BtnAccept.Visible = true;
                            BtnReject.Visible = true;
                            gridlist.Columns[11].Visible = true;
                            gridlist.Columns[10].Visible = true;
                        }
                    }
                    else
                    {
                        dv.RowFilter = "Status='" + ddlStatus.SelectedValue.ToString() + "'";
                        gridlist.DataSource = dv;
                        gridlist.DataBind();
                        BtnAccept.Visible = false;
                        BtnReject.Visible = false;
                        gridlist.Columns[11].Visible = false;
                        gridlist.Columns[10].Visible = false;
                    }

                    lblcnt.Text = dv.Count.ToString();
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Import File First!');", true);
            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void BtnReject_Click(object sender, EventArgs e)
    {
        dt_result = null;
        gridlist.DataSource = dt_result;
        gridlist.DataBind();
        lblTotHeader.Visible = false;
        lblcnt.Text = "";

    }
    protected void BtnAccept_Click(object sender, EventArgs e)
    {
        if (dt_result != null)
        {
            lblTotHeader.Visible = true;
            AssetVerification objVer = new AssetVerification();

            int MaxID = objVer.GetMaxTransferID("location");
            String Asset_Transfer_ID = "";
            if (MaxID == 0)
            {
                Asset_Transfer_ID = "T0001";

            }
            else
            {
                var res = MaxID + 1;
                Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#0000");
            }

            DataRow[] dr_Loc = dt_Loc.Select("Locationname='" + dt_Fileter_ToLoc.Rows[0]["Location"].ToString() + "'");
            string Location = dr_Loc[0]["LocationName"].ToString();
            DataRow[] dr_Building = dt_Build.Select("BuildingName='" + dt_Fileter_ToLoc.Rows[0]["Building"].ToString() + "'");
            string Building = dr_Building[0]["BuildingName"].ToString();
            DataRow[] dr_Floor = dt_floor.Select("FloorName='" + dt_Fileter_ToLoc.Rows[0]["Floor"].ToString() + "'");
            string Flr = dr_Floor[0]["FloorName"].ToString();

            string final_Location = Location + ">" + Building + ">" + Flr;

            objVer.UpdateMissingData(dt_result, Session["UserId"].ToString(), Asset_Transfer_ID, final_Location);
            DataRow[] chkMismatch = dt_result.Select("Status = 'Mismatch'");

            for (int y = 0; y < chkMismatch.Length; y++)
            {
                chkMismatch[y]["Status"] = "Found";
            }

            //foreach (var drow in chkMismatch)
            //{               
            //    drow.Delete();
            //}

            dt_result.AcceptChanges();
            gridlist.DataSource = dt_result;
            gridlist.DataBind();
            gridlist.Columns[11].Visible = false;
            gridlist.Columns[10].Visible = false;
            lblcnt.Text = dt_result.Rows.Count.ToString();

        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Import File First!');", true);
        }
    }

    private DataTable GetActiveFloor()
    {
        con.Open();
        DataSet ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveFloor");
        con.Close();
        return ds.Tables[0];
    }

    private DataTable GetActiveBuilding()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveBuilding");
        con.Close();
        return ds.Tables[0];


    }

    private DataTable GetActiveLocatio()
    {
        con.Open();
        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
        con.Close();
        return ds.Tables[0];
    }


    private void CreateTables()
    {
        this.dt_Loc = GetActiveLocatio();
        this.dt_Build = GetActiveBuilding();
        this.dt_floor = GetActiveFloor();
    }

    protected void btmRefresh_Click(object sender, EventArgs e)
    {
        if (dt_result != null)
        {
            lblTotHeader.Visible = true;
            if (dt_result.Rows.Count > 0)
            {
                gridlist.DataSource = dt_result;
                gridlist.DataBind();
                gridlist.Columns[11].Visible = false;
                gridlist.Columns[10].Visible = false;
                lblcnt.Text = dt_result.Rows.Count.ToString();

                bindstatus();
            }
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Please Import File First!');", true);
        }
    }
    protected void BtnSaveTHS_Click(object sender, EventArgs e)
    {
        try
        {
            this.dt_Reports_THR = null;
            if (dt_Reports_THS == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Reports not found');", true);
                return;

            }
            else
            {
                AssetVerification objAsset = new AssetVerification();
                int MaxID = objAsset.GetMaxVerificationID();
                String Asset_Verification_ID = "";
                if (MaxID == 0)
                {
                    Asset_Verification_ID = "ASV000000001";

                }
                else
                {
                    var res = MaxID + 1;
                    Asset_Verification_ID = "ASV" + Convert.ToInt32(res).ToString("#000000000");
                }
                objAsset.SaveAssetVerificationDetailsTHS(Asset_Verification_ID, Session["userid"].ToString(), dt_result, this.TLocationID, this.TBuildingID, this.TFloorID);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset verification reports saved successfully.');", true);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    protected void btnSaveTHR_Click(object sender, EventArgs e)
    {
        try
        {
            this.dt_Reports_THS = null;
            if (dt_Reports_THR == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Reports not found');", true);
                return;

            }
            else
            {
                AssetVerification objAsset = new AssetVerification();
                int MaxID = objAsset.GetMaxVerificationID();
                String Asset_Verification_ID = "";
                if (MaxID == 0)
                {
                    Asset_Verification_ID = "ASV000000001";

                }
                else
                {
                    var res = MaxID + 1;
                    Asset_Verification_ID = "ASV" + Convert.ToInt32(res).ToString("#000000000");
                }
                objAsset.SaveAssetVerificationDetails(Asset_Verification_ID, Session["userid"].ToString(), dt_result, this.TLocationID, this.TBuildingID, this.TFloorID);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset verification reports saved successfully.');", true);
            }
        }
        catch (Exception ex)
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.ToString() + "');", true);
        }
    }
    protected void BtnGetTHS_Click(object sender, EventArgs e)
    {
        try
        {
            dt_Fileter_ToLoc.Rows.Clear();
            this.dtAssetDetails = null;
            this.dt_result = null;
            // this.dtAssetDetails = null;
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            string JsonString = string.Empty;
            if (validateTXTFile(strFileType, strFileName) == true)
            {

                DataSet dsStockChk = new DataSet();
                DataTable gdt_StockData = new DataTable();

                var stream = File.OpenText(Server.MapPath("~/Stock/" + strFileName + strFileType));
                JsonString = stream.ReadToEnd();

                dsStockChk = (DataSet)JsonConvert.DeserializeObject(JsonString, (typeof(DataSet)));


                DataColumn dtcol1 = new DataColumn();
                DataColumn dtcol2 = new DataColumn();
                DataColumn dtcol3 = new DataColumn();
                DataColumn dtcol4 = new DataColumn();

                dtcol1.ColumnName = "Status";
                dtcol1.DefaultValue = "Found";
                dsStockChk.Tables["Found"].Columns.Add(dtcol1);
                dsStockChk.Tables["Found"].AcceptChanges();

                dtcol2.ColumnName = "Status";
                dtcol2.DefaultValue = "Missing";
                dsStockChk.Tables["Missing"].Columns.Add(dtcol2);
                dsStockChk.Tables["Missing"].AcceptChanges();

                dtcol3.ColumnName = "Status";
                dtcol3.DefaultValue = "Extra";
                dsStockChk.Tables["Extra"].Columns.Add(dtcol3);
                dsStockChk.Tables["Extra"].AcceptChanges();

                dtcol4.ColumnName = "Status";
                dtcol4.DefaultValue = "Mismatch";
                dsStockChk.Tables["Mismatch"].Columns.Add(dtcol4);
                dsStockChk.Tables["Mismatch"].AcceptChanges();

                gdt_StockData.Merge(dsStockChk.Tables["Found"]);
                gdt_StockData.Merge(dsStockChk.Tables["Missing"]);
                gdt_StockData.Merge(dsStockChk.Tables["Extra"]);
                gdt_StockData.Merge(dsStockChk.Tables["Mismatch"]);
                gdt_StockData.AcceptChanges();

                dt_Fileter_ToLoc = new DataTable();
                dt_Fileter_ToLoc = (dsStockChk.Tables["Filters"]);

                if (gdt_StockData.Columns.Count == 18 && gdt_StockData.Columns.Contains("Status") && gdt_StockData.Columns.Contains("AssetCode"))
                {
                    if (this.dtAssetDetails == null)
                    {
                        DataSet ds = Common.GetAllActiveAssetsDetails(null, null, null, null, null, null, null);
                        this.dtAssetDetails = ds.Tables[0];
                    }

                    dt_Reports_THS = new DataTable();
                    if (dt_Reports_THS.Columns.Count == 0)
                    {
                        dt_Reports_THS.Columns.Add("AssetCode");
                        dt_Reports_THS.Columns.Add("AssetId");
                        dt_Reports_THS.Columns.Add("Status");
                    }
                    foreach (DataRow dr in gdt_StockData.Rows)
                    {
                        dt_Reports_THS.Rows.Add(dr["AssetCode"].ToString(), dr["AssetId"].ToString(), dr["Status"].ToString());
                    }

                    var results = (from table1 in dtAssetDetails.AsEnumerable()
                                   join table2 in dt_Reports_THS.AsEnumerable() on (string)table1["AssetCode"] equals (string)table2["AssetCode"]
                                   select new
                                   {
                                       Status = table2["Status"],
                                       AssetCode = table2["AssetCode"],
                                       AssetId = table1["AssetId"],
                                       Category = table1["Category"],
                                       SubCategory = table1["SubCategory"],
                                       Building = table1["Building"],
                                       Floor = table1["Floor"],
                                       Location = table1["Location"],
                                       Department = table1["Department"],
                                       Price = table1["Price"],
                                       Supplier = table1["SupplierName"],
                                       SerialNo = table1["SerialNo"],
                                       Quantity = table1["Quantity"],
                                       Custodian = table1["Custodian"],
                                       DeliveryDate = table1["DeliveryDate"],
                                       AssignDate = table1["AssignDate"],
                                       Description = table1["Description"],
                                       CatID = table1["CategoryId"],
                                       SubCatID = table1["SubCatId"],
                                       LocID = table1["LocationId"],
                                       BldgID = table1["BuildingId"],
                                       FloorID = table1["FloorId"]
                                   }).ToList();

                    dt_result = new DataTable();
                    dt_result = LINQToDataTable(results);
                    if (dt_result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_Reports_THS.Rows)
                        {
                            string AssetCode = dr["AssetCode"].ToString();
                            DataRow[] chkext = dt_result.Select("AssetCode = '" + AssetCode + "'");
                            if (chkext.Length == 0)
                            {
                                dt_result.Rows.Add("Extra", AssetCode, "", "", "", "", "", "", "", "", "", "", "", "", null, null, "", "", "", "", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt_Reports_THS.Rows)
                        {
                            string AssetCode = dr["AssetCode"].ToString();
                            dt_result.Rows.Add("Extra", AssetCode, "", "", "", "", "", "", "", "", "", "", "", "", null, null, "", "", "", "", "", "");
                        }
                    }

                    DataColumn dtFrom = new DataColumn();
                    dtFrom.ColumnName = "FromLocation";
                    dt_result.Columns.Add(dtFrom);
                    DataColumn dtTo = new DataColumn();
                    dtTo.ColumnName = "ToLocation";
                    dt_result.Columns.Add(dtTo);
                    DataColumn ToLocID = new DataColumn();
                    ToLocID.ColumnName = "ToLocID";
                    dt_result.Columns.Add(ToLocID);
                    DataColumn TobldgID = new DataColumn();
                    TobldgID.ColumnName = "TobldgID";
                    dt_result.Columns.Add(TobldgID);
                    DataColumn ToFloorID = new DataColumn();
                    ToFloorID.ColumnName = "ToFloorID";
                    dt_result.Columns.Add(ToFloorID);

                    //string ToLocationID1 = GetLocDataFromDataTable(dt_result);
                    //string ToBuildingID1 = GetBldgDataFromDataTable(dt_result);
                    //string ToFlrID1 = GetFloorDataFromDataTable(dt_result);
                    string ToLocationID = "", ToLocationName = "", ToBuildingID = "", ToBuildingName = "", ToFlrID = "", ToFlrName="";


                    DataRow[] dr_Loc = dt_Loc.Select("Locationname='" + dsStockChk.Tables["Filters"].Rows[0]["Location"].ToString() + "'");
                    if (dr_Loc.Length > 0)
                    {
                         ToLocationID = dr_Loc[0]["LocationId"].ToString();
                         ToLocationName = dr_Loc[0]["LocationName"].ToString();
                    }


                    if (dsStockChk.Tables["Filters"].Rows[0]["Building"].ToString() == "No Building")
                    {
                        ToBuildingID = "001";
                        ToBuildingName = "No Building";
                    }
                    else
                    {
                        DataRow[] dr_Building = dt_Build.Select("BuildingName='" + dsStockChk.Tables["Filters"].Rows[0]["Building"].ToString() + "'");
                        if (dr_Building.Length > 0)
                        {
                            ToBuildingID = dr_Building[0]["BuildingId"].ToString();
                            ToBuildingName = dr_Building[0]["BuildingName"].ToString();
                        }
                    }

                    if (dsStockChk.Tables["Filters"].Rows[0]["Floor"].ToString() == "No Floor")
                    {
                        ToFlrID = "001";
                        ToFlrName = "No Floor";
                    }
                    else
                    {
                        DataRow[] dr_Floor = dt_floor.Select("FloorName='" + dsStockChk.Tables["Filters"].Rows[0]["Floor"].ToString() + "'");
                        if (dr_Floor.Length > 0)
                        {
                            ToFlrID = dr_Floor[0]["FloorId"].ToString();
                            ToFlrName = dr_Floor[0]["FloorName"].ToString();
                        }
                    }


                    this.TLocationID = ToLocationID == "" ? "1" : ToLocationID;
                    this.TBuildingID = ToBuildingID == "" ? "1" : ToBuildingID;
                    this.TFloorID = ToFlrID == "" ? "1" : ToFlrID;

                    //string ToLocation = GetFromDatatableforFound(dt_result);
                    string ToLocation = ToLocationName + "-" + ToBuildingName + "-" + ToFlrName;
                    string FromLocation = GetFromDatatableforMisMatch(dt_result);

                    if (ToLocation == FromLocation)
                    {
                        DataRow[] HRow = dt_result.Select("[Status]='Mismatch'");
                        if (HRow.Length > 0)
                        {
                            for (int y = 0; y < HRow.Length; y++)
                            {
                                HRow[y]["Status"] = "Found";
                            }
                        }
                    }

                    if (ToLocation.Trim() == "")
                    {
                        ToLocation = GetFromDatatableforMissing(dt_result);
                    }
                    int i = 0;
                    for (i = 0; i < dt_result.Rows.Count; i++)
                    {
                        if (dt_result.Rows[i]["Status"].ToString() == "Mismatch")
                        {
                            dt_result.Rows[i]["FromLocation"] = FromLocation;
                            dt_result.Rows[i]["ToLocation"] = ToLocation;
                            dt_result.Rows[i]["ToLocID"] = ToLocationID;
                            dt_result.Rows[i]["TobldgID"] = ToBuildingID;
                            dt_result.Rows[i]["ToFloorID"] = ToFlrID;
                        }
                    }
                    lblTotHeader.Visible = true;
                    gridlist.DataSource = dt_result;
                    gridlist.DataBind();
                    lblcnt.Text = dt_result.Rows.Count.ToString();
                    gridlist.Columns[10].Visible = false;
                    gridlist.Columns[11].Visible = false;


                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
                    return;
                }
            }
            else
            {
                lblMessage.Text = "Only TXT file allowed";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
        }
    }
    public bool isValidJSON(String json)
    {
        try
        {
            Newtonsoft.Json.Linq.JToken token = JObject.Parse(json);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private string GetFloorDataFromDataTable(DataTable dt_result)
    {
        string ToFloorID = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Missing'");
            if (chkext.Length > 0)
            {
                ToFloorID = chkext[0].ItemArray[21].ToString();
                break;
            }
        }
        if (ToFloorID == "")
        {
            foreach (DataRow dr in dt_result.Rows)
            {
                DataRow[] chkext = dt_result.Select("Status = 'Found'");
                if (chkext.Length > 0)
                {
                    ToFloorID = chkext[0].ItemArray[21].ToString();
                    break;
                }
            }
        }
        return ToFloorID;
    }

    private string GetBldgDataFromDataTable(DataTable dt_result)
    {
        string ToBuildingID = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Missing'");
            if (chkext.Length > 0)
            {
                ToBuildingID = chkext[0].ItemArray[20].ToString();
                break;
            }
        }
        if (ToBuildingID == "")
        {
            foreach (DataRow dr in dt_result.Rows)
            {
                DataRow[] chkext = dt_result.Select("Status = 'Found'");
                if (chkext.Length > 0)
                {
                    ToBuildingID = chkext[0].ItemArray[20].ToString();
                    break;
                }
            }
        }
        return ToBuildingID;
    }

    private string GetLocDataFromDataTable(DataTable dt_result)
    {
        string ToLocationID = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Missing'");
            if (chkext.Length > 0)
            {
                ToLocationID = chkext[0].ItemArray[19].ToString();
                break;
            }
        }
        if (ToLocationID == "")
        {
            foreach (DataRow dr in dt_result.Rows)
            {
                DataRow[] chkext = dt_result.Select("Status = 'Found'");
                if (chkext.Length > 0)
                {
                    ToLocationID = chkext[0].ItemArray[19].ToString();
                    break;
                }
            }
        }
        return ToLocationID;
    }

    protected void BtnGetTHR_Click(object sender, EventArgs e)
    {
        try
        {
            dt_Fileter_ToLoc.Rows.Clear();
            this.dt_result = null;
            this.dtAssetDetails = null;
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            if (validateXMLFile(strFileType, strFileName) == true)
            {
                string FilePath = Server.MapPath("~/Stock/" + strFileName + strFileType);
                DataSet dsStockChk = new DataSet();
                DataTable gdt_StockData = new DataTable();
                dsStockChk.ReadXml(FilePath);
                gdt_StockData = dsStockChk.Tables[2];
                if (gdt_StockData.Columns.Count == 7 && gdt_StockData.Columns.Contains("Status") && gdt_StockData.Columns.Contains("Asset_Code"))
                {

                    if (this.dtAssetDetails == null)
                    {
                        DataSet ds = Common.GetAllActiveAssetsDetails(null, null, null, null, null, null, null);
                        this.dtAssetDetails = ds.Tables[0];
                    }
                    dt_Reports_THR = new DataTable();
                    if (dt_Reports_THR.Columns.Count == 0)
                    {
                        dt_Reports_THR.Columns.Add("AssetCode");
                        dt_Reports_THR.Columns.Add("AssetId");
                        dt_Reports_THR.Columns.Add("Status");
                    }

                    foreach (DataRow dr in gdt_StockData.Rows)
                    {
                        dt_Reports_THR.Rows.Add(dr["Asset_Code"].ToString(), dr["AssetId"].ToString(), dr["Status"].ToString());
                    }

                    var results = (from table1 in dtAssetDetails.AsEnumerable()
                                   join table2 in dt_Reports_THR.AsEnumerable() on (string)table1["AssetCode"] equals (string)table2["AssetCode"]
                                   select new
                                   {
                                       //STATUS1 = table2["Status"],
                                       STATUS = (table2["Status"].ToString() == "M" ? "Missing" :
                                                    table2["Status"].ToString() == "F" ? "Found" :
                                                    table2["Status"].ToString() == "E" ? "Mismatch" :
                                                   ""),
                                       AssetCode = table2["AssetCode"],
                                       AssetId = table1["AssetId"],
                                       Category = table1["Category"],
                                       SubCategory = table1["SubCategory"],
                                       Building = table1["Building"],
                                       Floor = table1["Floor"],
                                       Location = table1["Location"],
                                       Department = table1["Department"],
                                       Price = table1["Price"],
                                       Supplier = table1["SupplierName"],
                                       SerialNo = table1["SerialNo"],
                                       Quantity = table1["Quantity"],
                                       Custodian = table1["Custodian"],
                                       DeliveryDate = table1["DeliveryDate"],
                                       AssignDate = table1["AssignDate"],
                                       Description = table1["Description"],
                                       CatID = table1["CategoryId"],
                                       SubCatID = table1["SubCatId"],
                                       LocID = table1["LocationId"],
                                       BldgID = table1["BuildingId"],
                                       FloorID = table1["FloorId"]
                                   }).ToList();

                    dt_result = new DataTable();
                    dt_result = LINQToDataTable(results);
                    if (dt_result.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt_Reports_THR.Rows)
                        {
                            string AssetCode = dr["AssetCode"].ToString();
                            DataRow[] chkext = dt_result.Select("AssetCode = '" + AssetCode + "'");
                            if (chkext.Length == 0)
                            {
                                dt_result.Rows.Add("Extra", AssetCode, "", "", "", "", "", "", "", "", "", "", "", "", null, null, "", "", "", "", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt_Reports_THR.Rows)
                        {
                            string AssetCode = dr["AssetCode"].ToString();
                            dt_result.Rows.Add("Extra", AssetCode, "", "", "", "", "", "", "", "", "", "", "", "", null, null, "", "", "", "", "", "");
                        }
                    }

                    DataColumn dtFrom = new DataColumn();
                    dtFrom.ColumnName = "FromLocation";
                    dt_result.Columns.Add(dtFrom);
                    DataColumn dtTo = new DataColumn();
                    dtTo.ColumnName = "ToLocation";
                    dt_result.Columns.Add(dtTo);
                    DataColumn ToLocID = new DataColumn();
                    ToLocID.ColumnName = "ToLocID";
                    dt_result.Columns.Add(ToLocID);
                    DataColumn TobldgID = new DataColumn();
                    TobldgID.ColumnName = "TobldgID";
                    dt_result.Columns.Add(TobldgID);
                    DataColumn ToFloorID = new DataColumn();
                    ToFloorID.ColumnName = "ToFloorID";
                    dt_result.Columns.Add(ToFloorID);

                    string ToLocationID = GetLocDataFromDataTable(dt_result);
                    string ToBuildingID = GetBldgDataFromDataTable(dt_result);
                    string ToFlrID = GetFloorDataFromDataTable(dt_result);

                    this.TLocationID = ToLocationID == "" ? "1" : ToLocationID;
                    this.TBuildingID = ToBuildingID == "" ? "1" : ToBuildingID;
                    this.TFloorID = ToFlrID == "" ? "1" : ToFlrID;

                    string ToLocation = GetFromDatatableforFound(dt_result);
                    string FromLocation = GetFromDatatableforMisMatch(dt_result);
                    if (ToLocation.Trim() == "")
                    {
                        ToLocation = GetFromDatatableforMissing(dt_result);
                    }
                    int i = 0;
                    for (i = 0; i < dt_result.Rows.Count - 1; i++)
                    {
                        if (dt_result.Rows[i]["Status"].ToString() == "Mismatch")
                        {
                            dt_result.Rows[i]["FromLocation"] = FromLocation;
                            dt_result.Rows[i]["ToLocation"] = ToLocation;
                            dt_result.Rows[i]["ToLocID"] = ToLocationID;
                            dt_result.Rows[i]["TobldgID"] = ToBuildingID;
                            dt_result.Rows[i]["ToFloorID"] = ToFlrID;
                        }
                    }

                    gridlist.DataSource = dt_result;
                    gridlist.DataBind();
                    lblTotHeader.Visible = true;
                    lblcnt.Text = dt_result.Rows.Count.ToString();
                    gridlist.Columns[11].Visible = false;
                    gridlist.Columns[10].Visible = false;

                }
                else
                {
                    //lblMessage.Visible = true;
                    //lblMessage.Text = "Invalid file.";
                    //lblMessage.ForeColor = System.Drawing.Color.Red;
                    //lblMessage.Font.Size = 11;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
                    return;
                }


            }
            else
            {
                lblMessage.Text = "Only XML file allowed";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private string GetFromDatatableforMisMatch(DataTable dt_result)
    {
        string FromLocation = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Mismatch'");
            if (chkext.Length > 0)
            {
                string Location = chkext[0].ItemArray[7].ToString() == "No Location" ? "" : chkext[0].ItemArray[7].ToString();
                string Building = chkext[0].ItemArray[5].ToString() == "No Building" ? "" : chkext[0].ItemArray[5].ToString();
                string Floor = chkext[0].ItemArray[6].ToString() == "No Floor" ? "" : chkext[0].ItemArray[6].ToString();

                if (Location != "" || Building != "" || Floor != "")
                {
                    FromLocation = Location + "-" + Building + "-" + Floor;
                }
                //if (Location != "")
                //{
                //    FromLocation = Location;
                //}
                //else if (Building != "")
                //{
                //    FromLocation = Location + "->" + Building;
                //}
                //else if (Floor != "")
                //{
                //    FromLocation = Location + "->" + Building + "->" + Floor;
                //}
                break;
            }
        }
        return FromLocation;
    }

    private string GetFromDatatableforMissing(DataTable dt_result)
    {
        string ToLocation = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Missing'");
            if (chkext.Length == 1)
            {
                string Location = chkext[0].ItemArray[7].ToString() == "No Location" ? "" : chkext[0].ItemArray[7].ToString();
                string Building = chkext[0].ItemArray[5].ToString() == "No Building" ? "" : chkext[0].ItemArray[5].ToString();
                string Floor = chkext[0].ItemArray[6].ToString() == "No Floor" ? "" : chkext[0].ItemArray[6].ToString();
                if (Location != "")
                {
                    ToLocation = Location;
                }
                else if (Building != "")
                {
                    ToLocation = Location + "->" + Building;
                }
                else if (Floor != "")
                {
                    ToLocation = Location + "->" + Building + "->" + Floor;
                }
                break;
            }
        }
        return ToLocation;
    }

    private string GetFromDatatableforFound(DataTable dt_result)
    {
        string ToLocation = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Found'");
            if (chkext.Length > 0)
            {
                string Location = chkext[0].ItemArray[7].ToString() == "No Location" ? "" : chkext[0].ItemArray[7].ToString();
                string Building = chkext[0].ItemArray[5].ToString() == "No Building" ? "" : chkext[0].ItemArray[5].ToString();
                string Floor = chkext[0].ItemArray[6].ToString() == "No Floor" ? "" : chkext[0].ItemArray[6].ToString();

                if (Location != "" || Building != "" || Floor != "")
                {
                    ToLocation = Location + "-" + Building + "-" + Floor;
                }
                //if (Location != "")
                //{
                //    ToLocation = Location;
                //}
                //else if (Building != "")
                //{
                //    ToLocation = Location + "->" + Building;
                //}
                //else if (Floor != "")
                //{
                //    ToLocation = Location + "->" + Building + "->" + Floor;
                //}
                break;
            }
        }
        return ToLocation;
    }
    // Convert linq to datatable
    public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others  will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }
    private bool validateXMLFile(string strFileType, string strFileName)
    {
        if (strFileType == ".xml")
        {
            productimguploder.SaveAs(Server.MapPath("~/Stock/" + strFileName + strFileType));
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool validateTXTFile(string strFileType, string strFileName)
    {
        if (strFileType.ToString().ToLower() == ".txt")
        {
            productimguploder.SaveAs(Server.MapPath("~/Stock/" + strFileName + strFileType));
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void myDataGrid_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    {

        gridlist.CurrentPageIndex = e.NewPageIndex;
        gridlist.DataSource = this.dt_result;
        gridlist.DataBind();
    }
}