using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Serco;
using Newtonsoft.Json;
using ECommerce.Common;
using ECommerce.DataAccess;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Drawing;

public partial class Assettransfer : System.Web.UI.Page
{
    public static DataTable dt_rt = new DataTable();
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
    public DataTable dt_resultTHS
    {
        get
        {
            return ViewState["dt_resultTHS"] as DataTable;
        }
        set
        {
            ViewState["dt_resultTHS"] = value;

        }
    }
    public DataTable dt_grdDetails
    {
        get
        {
            return ViewState["dt_grdDetails"] as DataTable;
        }
        set
        {
            ViewState["dt_grdDetails"] = value;

        }
    }
    //public DataTable dtAssetDetails
    //{
    //    get
    //    {
    //        return ViewState["dtAssetDetails"] as DataTable;
    //    }
    //    set
    //    {
    //        ViewState["dtAssetDetails"] = value;

    //    }
    //}
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
    public DataTable dt_Category
    {
        get
        {
            return ViewState["dt_Category"] as DataTable;
        }
        set
        {
            ViewState["dt_Category"] = value;

        }
    }
    public DataTable dt_SubCategory
    {
        get
        {
            return ViewState["dt_SubCategory"] as DataTable;
        }
        set
        {
            ViewState["dt_SubCategory"] = value;

        }
    }
    public string FromLocation
    {
        get
        {
            return ViewState["FromLocation"].ToString();
        }
        set
        {
            ViewState["FromLocation"] = value;
        }
    }
    public string ToLocation
    {
        get
        {
            return ViewState["ToLocation"].ToString();
        }
        set
        {
            ViewState["ToLocation"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblTotHeader.Visible = false;
            txtFrmDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            CreateTables();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grid_view();
    }

    public string StrSort;
    private void grid_view()
    {
        string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
        string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
        FromDate = FromDate == null ? ToDate = null : FromDate;
        ToDate = ToDate == null ? FromDate = null : ToDate;

        try
        {
            DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetTransferAssetsAccordingToDate, new SqlParameter[] { 
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                            });
            DataSet ds = help.ExecuteDataset();
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                lblMessage.Text = "Problem occured while retrieving Product records. Please try again.";
            }
            else
            {
                lblTotHeader.Visible = true;
                DataTable dt = ds.Tables[0];
                DataView myView;
                myView = ds.Tables[0].DefaultView;
                lblcnt.Text = Convert.ToString(dt.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }
                gridlist.DataSource = myView;
                gridlist.DataBind();

            }

        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }
    protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    {
        gridlist.CurrentPageIndex = e.NewPageIndex;
        grid_view();
    }
    protected void OpenTransferDetails(object sender, EventArgs e)
    {
        this.dt_grdDetails = null;
        DataGridItem item = (DataGridItem)((LinkButton)sender).Parent.Parent;
        HiddenField HdnTrnID = (HiddenField)item.FindControl("HdnTrnID");
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetAssetTransferDeatilsByTransferID(HdnTrnID.Value,"location");
        dt_rt = new DataTable();
        dt_rt = ds.Tables[0];
        gridDetails.DataSource = dt_rt;
        gridDetails.DataBind();
        this.dt_grdDetails = dt_rt;
        LblDetails.Text =dt_rt.Rows.Count.ToString();
        GriddetailsPopup.Show();
    }
    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        ////PrepareForExport(gridlist);
        ExportToExcel();
    }
    private void ExportToExcel()
    {
        if (gridDetails.Items.Count > 0)
        {
            ////Response.Clear();
            ////Response.Clear();
            ////Response.AddHeader("content-disposition",
            ////                      "attachment;filename=Transfer.xls");
            ////Response.Charset = String.Empty;
            ////Response.ContentType = "application/ms-excel";
            ////StringWriter stringWriter = new StringWriter();
            ////HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            ////gridDetails.AllowPaging = false;
            ////gridDetails.DataSource = this.dt_grdDetails;
            ////gridDetails.DataBind();
            ////gridDetails.RenderControl(HtmlTextWriter);
            ////Response.Write(stringWriter.ToString());
            ////Response.End();

            GridView GridView2 = new GridView();
            GridView2.AllowPaging = false;
            GridView2.DataSource = dt_rt;
            GridView2.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Transfer.xls");
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
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No records found.');", true);
        }

    }
    private void PrepareForExport(Control ctrl)
    {
        //iterate through all the grid controls
        foreach (Control childControl in ctrl.Controls)
        {
            //if the control type is link button, remove it
            //from the collection
            if (childControl.GetType() == typeof(LinkButton))
            {
                ctrl.Controls.Remove(childControl);
            }

            //if the child control is not empty, repeat the process
            // for all its controls
            else if (childControl.HasControls())
            {
                PrepareForExport(childControl);
            }
        }
    }
    protected void BtnGetTHS_Click(object sender, EventArgs e)
    {
        try
        {
            this.dt_result = null;
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            string JsonString = string.Empty;
            if (validateTXTFile(strFileType, strFileName) == true)
            {
                DataSet dsStockTransfer = new DataSet();
                var stream = File.OpenText(Server.MapPath("~/Transfer/" + strFileName + strFileType));
                JsonString = stream.ReadToEnd();
                dt_resultTHS = (DataTable)JsonConvert.DeserializeObject(JsonString, (typeof(DataTable)));
                if (dt_resultTHS.Columns.Count == 9 && dt_resultTHS.Columns.Contains("AssetCode"))
                {
                    string AssetIDs = ""; ;
                    for (int i = 0; i < dt_resultTHS.Rows.Count; i++)
                    {
                        AssetIDs = AssetIDs + dt_resultTHS.Rows[i]["AssetCode"].ToString();
                        AssetIDs += (i < dt_resultTHS.Rows.Count) ? "," : string.Empty;
                    }

                    try
                    {
                        //var AssetIDs1 = string.Join(",", dt_resultTHS.AsEnumerable().Select(s => s."AssetID").ToArray<string>());
                        object var = string.Join(",", dt_resultTHS.AsEnumerable().Where(s => s.Field<object>("AssetID") != null).Select(s => s.Field<object>("AssetID")).ToArray<object>());
                    }
                    catch
                    {

                    }

                    if (exist_AssetIDsInAssetMaster(AssetIDs.ToString()) == true)
                    {
                        BindColumntoExcelTHS(dt_resultTHS);
                        ValidateTHSData(dt_resultTHS);

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
                        string FromDestination = "";
                        string ToDestination = "";
                        for (int i = 0; i < dt_resultTHS.Rows.Count; i++)
                        {
                            if (dt_resultTHS.Rows[i]["From_Location"].ToString() != "")
                            {
                                FromDestination = dt_resultTHS.Rows[i]["From_Location"].ToString();

                                if (dt_resultTHS.Rows[i]["From_Building"].ToString() != "")
                                {
                                    FromDestination = FromDestination + "->" + dt_resultTHS.Rows[i]["From_Building"].ToString();

                                    if (dt_resultTHS.Rows[i]["From_Floor"].ToString() != "")
                                    {
                                        FromDestination = FromDestination + "->" + dt_resultTHS.Rows[i]["From_Floor"].ToString();
                                    }
                                }
                            }

                            if (dt_resultTHS.Rows[i]["To_Location"].ToString() != "")
                            {
                                ToDestination = dt_resultTHS.Rows[i]["To_Location"].ToString();

                                if (dt_resultTHS.Rows[i]["To_Building"].ToString() != "")
                                {
                                    ToDestination = ToDestination + "->" + dt_resultTHS.Rows[i]["To_Building"].ToString();
                                    if (dt_resultTHS.Rows[i]["To_Floor"].ToString() != "")
                                    {
                                        ToDestination = ToDestination + "->" + dt_resultTHS.Rows[i]["To_Floor"].ToString();
                                    }
                                }
                            }
                            break;
                        }
                        string Reason = dt_resultTHS.Rows[0]["Reason"].ToString();
                        objVer.SaveAssetTransferDetails(dt_resultTHS, Session["userid"].ToString(), Asset_Transfer_ID, Reason, ToDestination);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Some AssetID does not exist in the master!!');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
                    return;
                }

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
        }
    }

    private void ValidateTHSData(DataTable dt_resultTHS)
    {
        int i = 0;
        foreach (DataRow dr in dt_resultTHS.Rows)
        {
            foreach (var Coulmn in dt_resultTHS.Columns)
            {
                string tblHeaderCoulmn = Coulmn.ToString();
                string ExcelValue = dr[tblHeaderCoulmn].ToString();
                switch (tblHeaderCoulmn)
                {
                    //case "From_Location":
                    //    {
                    //        //if (ExcelValue != "1")
                    //        //{
                    //        DataRow[] Exist_Location = dt_Loc.Select("LocationName='" + ExcelValue + "'");
                    //        if (Exist_Location.Length == 0)
                    //        {
                    //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Location  is not available in masters" + "');", true);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            //if(Exist_BUILDING[0].ItemArray[1].ToString()!="1")
                    //            //{
                    //            dt_resultTHS.Rows[i]["FromLocationID"] = Convert.ToString(Exist_Location[0].ItemArray[0]);
                    //            dt_resultTHS.AcceptChanges();
                    //            //}
                    //            // }
                    //        }
                    //        break;

                    //    }
                    case "To_Location":
                        {
                            //if (ExcelValue != "1")
                            //{
                            DataRow[] Exist_Location = dt_Loc.Select("LocationName='" + ExcelValue + "'");
                            if (Exist_Location.Length == 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Location  is not available in masters" + "');", true);
                                return;
                            }
                            else
                            {
                                //if (Exist_BUILDING[0].ItemArray[0].ToString() != "1")
                                //{
                                dt_resultTHS.Rows[i]["ToLocationId"] = Convert.ToString(Exist_Location[0].ItemArray[0]);
                                dt_resultTHS.AcceptChanges();
                                //}
                                //}
                            }
                            break;
                        }
                    //case "From_Building":
                    //    {
                    //        if (ExcelValue != "No Building")
                    //        {
                    //            DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + ExcelValue + "'");
                    //            if (Exist_BUILDING.Length == 0)
                    //            {
                    //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Building  is not available in masters" + "');", true);
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                dt_resultTHS.Rows[i]["FromBuildingId"] = Convert.ToString(Exist_BUILDING[0].ItemArray[0]);
                    //                dt_resultTHS.AcceptChanges();

                    //            }
                    //        }
                    //        break;
                    //    }
                    case "To_Building":
                        {
                            if (ExcelValue != "No Building")
                            {
                                DataRow[] Exist_BUILDING = dt_Build.Select("BuildingName='" + ExcelValue + "'");
                                if (Exist_BUILDING.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Building  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_resultTHS.Rows[i]["ToBuildingId"] = Convert.ToString(Exist_BUILDING[0].ItemArray[0]);
                                    dt_resultTHS.AcceptChanges();
                                }
                            }
                            break;
                        }
                    //case "From_Floor":
                    //    {
                    //        if (ExcelValue != "No Floor")
                    //        {
                    //            DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue + "'");
                    //            if (Exist_Floor.Length == 0)
                    //            {
                    //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Floor  is not available in masters" + "');", true);
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                dt_resultTHS.Rows[i]["ToFloorId"] = Convert.ToString(Exist_Floor[0].ItemArray[0]);
                    //                dt_resultTHS.AcceptChanges();
                    //            }
                    //        }
                    //        break;
                    //    }
                    case "To_Floor":
                        {
                            if (ExcelValue != "No Floor")
                            {
                                DataRow[] Exist_Floor = dt_floor.Select("FloorName='" + ExcelValue + "'");
                                if (Exist_Floor.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Floor  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_resultTHS.Rows[i]["ToFloorId"] = Convert.ToString(Exist_Floor[0].ItemArray[0]);
                                    dt_resultTHS.AcceptChanges();
                                }
                            }
                            break;
                        }
                }
            }
            i++;
        }
    }

    private void ValidateData(DataTable dt_result)
    {
        int i = 0;
        foreach (DataRow dr in dt_result.Rows)
        {
            foreach (var Coulmn in dt_result.Columns)
            {
                string tblHeaderCoulmn = Coulmn.ToString();
                string ExcelValue = dr[tblHeaderCoulmn].ToString();
                switch (tblHeaderCoulmn)
                {
                    case "FromLocationID":
                        {
                            //if (ExcelValue != "1")
                            //{
                            DataRow[] Exist_Location = dt_Loc.Select("LocationId='" + ExcelValue + "'");
                            if (Exist_Location.Length == 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Location  is not available in masters" + "');", true);
                                return;
                            }
                            else
                            {
                                //if(Exist_BUILDING[0].ItemArray[1].ToString()!="1")
                                //{
                                dt_result.Rows[i]["FromLocation"] = Convert.ToString(Exist_Location[0].ItemArray[1]);
                                dt_result.AcceptChanges();
                                //}
                                // }
                            }
                            break;

                        }
                    case "ToLocationID":
                        {
                            //if (ExcelValue != "1")
                            //{
                            DataRow[] Exist_Location = dt_Loc.Select("LocationId='" + ExcelValue + "'");
                            if (Exist_Location.Length == 0)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Location  is not available in masters" + "');", true);
                                return;
                            }
                            else
                            {
                                //if (Exist_BUILDING[0].ItemArray[0].ToString() != "1")
                                //{
                                dt_result.Rows[i]["ToLocation"] = Convert.ToString(Exist_Location[0].ItemArray[1]);
                                dt_result.AcceptChanges();
                                //}
                                //}
                            }
                            break;
                        }
                    case "FromBuildingID":
                        {
                            if (ExcelValue != "1")
                            {
                                DataRow[] Exist_BUILDING = dt_Build.Select("BuildingId='" + ExcelValue + "'");
                                if (Exist_BUILDING.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Building  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_result.Rows[i]["FromBuilding"] = Convert.ToString(Exist_BUILDING[0].ItemArray[1]);
                                    dt_result.AcceptChanges();

                                }
                            }
                            break;
                        }
                    case "ToBuildingID":
                        {
                            if (ExcelValue != "1")
                            {
                                DataRow[] Exist_BUILDING = dt_Build.Select("BuildingId='" + ExcelValue + "'");
                                if (Exist_BUILDING.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Building  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_result.Rows[i]["ToBuilding"] = Convert.ToString(Exist_BUILDING[0].ItemArray[1]);
                                    dt_result.AcceptChanges();
                                }
                            }
                            break;
                        }
                    case "FromFloorID":
                        {
                            if (ExcelValue != "1")
                            {
                                DataRow[] Exist_Floor = dt_floor.Select("FloorId='" + ExcelValue + "'");
                                if (Exist_Floor.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Floor  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_result.Rows[i]["ToFloor"] = Convert.ToString(Exist_Floor[0].ItemArray[1]);
                                    dt_result.AcceptChanges();
                                }
                            }
                            break;
                        }
                    case "ToFloorID":
                        {
                            if (ExcelValue != "1")
                            {
                                DataRow[] Exist_Floor = dt_floor.Select("FloorId='" + ExcelValue + "'");
                                if (Exist_Floor.Length == 0)
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + "Floor  is not available in masters" + "');", true);
                                    return;
                                }
                                else
                                {
                                    dt_result.Rows[i]["ToFloor"] = Convert.ToString(Exist_Floor[0].ItemArray[1]);
                                    dt_result.AcceptChanges();
                                }
                            }
                            break;
                        }
                }
            }
            i++;
        }
    }
    private void BindColumntoExcelTHS(DataTable dt_resultTHS)
    {
        dt_resultTHS.Columns.Add(new DataColumn("FromBuildingID", Type.GetType("System.String")));
        dt_resultTHS.Columns.Add(new DataColumn("ToBuildingID", Type.GetType("System.String")));
        dt_resultTHS.Columns.Add(new DataColumn("FromFloorID", Type.GetType("System.String")));
        dt_resultTHS.Columns.Add(new DataColumn("ToFloorID", Type.GetType("System.String")));
        dt_resultTHS.Columns.Add(new DataColumn("FromLocationID", Type.GetType("System.String")));
        dt_resultTHS.Columns.Add(new DataColumn("ToLocationID", Type.GetType("System.String")));
    }
    protected void BtnGetTHR_Click(object sender, EventArgs e)
    {
        try
        {
            this.dt_result = null;
            //this.dtAssetDetails = null;
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            if (validateXMLFile(strFileType, strFileName) == true)
            {
                string FilePath = Server.MapPath("~/Transfer/" + strFileName + strFileType);
                DataSet dsStockTransfer = new DataSet();
                // DataTable gdt_StockData = new DataTable();
                dsStockTransfer.ReadXml(FilePath);
                dt_result = dsStockTransfer.Tables[1];
                if (dt_result.Columns.Count == 9 && dt_result.Columns.Contains("Asset_Code"))
                {
                    object AssetIDs = string.Join(",", dt_result.AsEnumerable().Select(s => s.Field<object>("AssetID")).ToArray<object>());
                    if (exist_AssetIDsInAssetMaster(AssetIDs.ToString()) == true)
                    {
                        BindColumntoExcel(dt_result);
                        ValidateData(dt_result);
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
                        string FromDestination = "";
                        string ToDestination = "";
                        for (int i = 0; i < dt_result.Rows.Count; i++)
                        {
                            if (dt_result.Rows[i]["FromLocation"].ToString() != "")
                            {
                                FromDestination = dt_result.Rows[i]["FromLocation"].ToString();
                            }
                            else if (dt_result.Rows[i]["FromBuilding"].ToString() != "")
                            {
                                FromDestination = FromDestination + "->" + dt_result.Rows[i]["FromBuilding"].ToString();
                            }
                            else if (dt_result.Rows[i]["FromFloor"].ToString() != "")
                            {
                                FromDestination = FromDestination + "->" + dt_result.Rows[i]["FromFloor"].ToString();
                            }

                            if (dt_result.Rows[i]["ToLocation"].ToString() != "")
                            {
                                ToDestination = dt_result.Rows[i]["ToLocation"].ToString();
                            }
                            else if (dt_result.Rows[i]["ToBuilding"].ToString() != "")
                            {
                                ToDestination = ToDestination + "->" + dt_result.Rows[i]["ToBuilding"].ToString();
                            }
                            else if (dt_result.Rows[i]["ToFloor"].ToString() != "")
                            {
                                ToDestination = ToDestination + "->" + dt_result.Rows[i]["ToFloor"].ToString();
                            }
                            break;
                        }

                        objVer.SaveAssetTransferDetails(dt_result, Session["userid"].ToString(), Asset_Transfer_ID, FromDestination, ToDestination);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Asset Transfer Data Save Successfully!!');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Some AssetID does not exist in the master!!');", true);
                    }



                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
                    return;
                }



            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }
    private string GetFromDatatableforFound(DataTable dt_result)
    {
        string ToLocation = "";
        foreach (DataRow dr in dt_result.Rows)
        {
            DataRow[] chkext = dt_result.Select("Status = 'Found'");
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



    private void BindColumntoExcel(DataTable dt_result)
    {
        dt_result.Columns.Add(new DataColumn("FromBuilding", Type.GetType("System.String")));
        dt_result.Columns.Add(new DataColumn("ToBuilding", Type.GetType("System.String")));
        dt_result.Columns.Add(new DataColumn("FromFloor", Type.GetType("System.String")));
        dt_result.Columns.Add(new DataColumn("ToFloor", Type.GetType("System.String")));
        dt_result.Columns.Add(new DataColumn("FromLocation", Type.GetType("System.String")));
        dt_result.Columns.Add(new DataColumn("ToLocation", Type.GetType("System.String")));
    }

    private void CreateTables()
    {
        this.dt_Loc = GetActiveLocatio();
        this.dt_Build = GetActiveBuilding();
        this.dt_floor = GetActiveFloor();
        this.dt_Category = GetActiveCategory();
        this.dt_SubCategory = GetActiveSubCategory();
    }

    private DataTable GetActiveSubCategory()
    {
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetActiveSubCategory();
        return ds.Tables[0];
    }

    private DataTable GetActiveCategory()
    {
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetActiveCategory();
        return ds.Tables[0];
    }
    private DataTable GetActiveFloor()
    {
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetActiveFloor();
        return ds.Tables[0];
    }

    private DataTable GetActiveBuilding()
    {
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetActiveBuilding();
        return ds.Tables[0];


    }

    private DataTable GetActiveLocatio()
    {
        AssetVerification objVer = new AssetVerification();
        DataSet ds = objVer.GetActiveLocation();
        return ds.Tables[0];
    }
    private bool exist_AssetIDsInAssetMaster(string AssetIds)
    {
        AssetVerification objVer = new AssetVerification();
        bool Exist = objVer.CheckAssetIDsExistInAssetmaster(AssetIds);
        return Exist;
    }
    private bool validateXMLFile(string strFileType, string strFileName)
    {
        if (strFileType == ".xml")
        {
            productimguploder.SaveAs(Server.MapPath("~/Transfer/" + strFileName + strFileType));
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
            productimguploder.SaveAs(Server.MapPath("~/Transfer/" + strFileName + strFileType));
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void BtnDownloadMaster_Click(object sender, EventArgs e)
    {
        OperationBL objBL = new OperationBL();
        DataTable dtMaster = objBL.GetMasterFile();
        System.Text.StringBuilder sp = new System.Text.StringBuilder();
        sp.Append("<Tagit>");
        sp.Append("<LineItems>");
        foreach (DataRow dr in dtMaster.Rows)
        {
            sp.Append("<LineItem ID =" + '"' + dr["ID"].ToString() + '"' + " Name =" + '"' + dr["Name"].ToString() + '"' + " Type =" + '"' + dr["Type"].ToString() + '"' + " />");
        }
        sp.Append("</LineItems>");
        sp.Append("</Tagit>");
        Session["MasterFile"] = sp.ToString();
        string strPopup = "<script language='javascript' ID='script1'>"

               // Passing intId to popup window.
               + "window.open('DownloadMaster.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

               + "</script>";

        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);

    }
}