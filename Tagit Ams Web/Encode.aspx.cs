using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECommerce.Common;
using System.Data;
using System.IO;
using Tagit;
using ECommerce.Utilities;
using Serco;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telerik.Web.UI;
using System.Drawing;

public partial class Encode : System.Web.UI.Page
{
    public static DataTable dtAssetDetails = new DataTable();
    public static string path = "";
    public int AssetId
    {
        get
        {
            return Convert.ToInt32(ViewState["AssetId"]);
        }
        set
        {
            ViewState["AssetId"] = value;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                if (userAuthorize((int)pages.Statistics, Session["userid"].ToString()) == true)
                {
                    lblTotHeader.Visible = false;
                    Common.Bindcategory((DropDownList)ddlproCategory);
                    Common.BindLocation((DropDownList)ddlloc, Session["userid"].ToString());
                    Common.BindDepartMent((DropDownList)ddldept);
                    ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
                    ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
                    ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
                    ddldept.Items.Insert(0, new ListItem("--Select Department--", "0", true));
                    BindGrid_view();
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "Page_Load", path);

        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }
    protected void btnYesErr_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    private void BindGrid_view()
    {
        try
        {
            //string Asset = (Convert.ToString(this.AssetId) == "0") ? null : Convert.ToString(this.AssetId);
            string CategoryId = (ddlproCategory.SelectedValue == "0") ? null : ddlproCategory.SelectedValue;
            string SubCatId = null;
            string LocationId = (ddlloc.SelectedValue == "0") ? null : ddlloc.SelectedValue;
            string BuildingId = (ddlbuild.SelectedValue == "0") ? null : ddlbuild.SelectedValue;
            string FloorId = (ddlfloor.SelectedValue == "0") ? null : ddlfloor.SelectedValue;
            string DepartmentId = (ddldept.SelectedValue == "0") ? null : ddldept.SelectedValue;
            string AssetCode = txtAssetCode.Text == "" ? null : txtAssetCode.Text;

            PrintBL objBL = new PrintBL();
            //DataSet ds = objBL.GetAssetDetailsForEncode(Asset, CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId);
            DataSet ds = objBL.GetAssetDetailsForEncode(CategoryId, SubCatId, LocationId, BuildingId, FloorId, DepartmentId, AssetCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtAssetDetails = new DataTable();
                dtAssetDetails = ds.Tables[0];
                ///this.dtAssetDetails = ds.Tables[0];
                lblTotHeader.Visible = true;
                lblcnt.Text = Convert.ToString(ds.Tables[0].Rows.Count);

                //gridlist.DataSource = ds;
                //gridlist.DataBind();

                gvData.DataSource = ds;
            }
            else
            {
                //gridlist.DataSource = null;
                //gridlist.DataBind();

                gvData.DataSource = ds;
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "BindGrid_view", path);
        }
    }
    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    BindGrid_view();
    //}
    protected void btnsearchsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
            BindGrid_view();
            gvData.DataBind();
            //ResetSearch();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "btnsearchsubmit_Click", path);
        }
    }

    private void ResetSearch()
    {
        try
        {
            ddlproCategory.SelectedValue = "0";
            ddlsubcat.Items.Clear();
            ddlsubcat.Items.Insert(0, new ListItem("--Select Sub Category--", "0", true));
            ddlloc.SelectedValue = "0";
            ddlbuild.Items.Clear();
            ddlbuild.Items.Insert(0, new ListItem("--Select Building--", "0", true));
            ddlfloor.Items.Clear();
            ddlfloor.Items.Insert(0, new ListItem("--Select Floor--", "0", true));
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "ResetSearch", path);

        }
        // ddlAsstID.Items.Clear();
    }

    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {
        try
        {
            Common.bindSubCategory((DropDownList)ddlsubcat, ddlproCategory.SelectedValue);
            ddlsubcat.SelectedValue = "0";
            ddlloc.SelectedValue = "0";
            ddlbuild.SelectedValue = "0";
            ddlfloor.SelectedValue = "0";
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "OnSelectedIndexChangedCategory", path);

        }
        // ddlAsstID.Items.Clear();
    }
    protected void OnSelectedIndexChangedLcocation(object sender, EventArgs e)
    {
        try
        {
            Common.bindBuilding((DropDownList)ddlbuild, ddlloc.SelectedValue);
            ddlfloor.SelectedValue = "0";
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "OnSelectedIndexChangedLcocation", path);

        }
    }

    protected void OnSelectedIndexChangedBuilding(object sender, EventArgs e)
    {
        try
        {
            Common.BindFloor((DropDownList)ddlfloor, ddlbuild.SelectedValue);
            ddldept.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "OnSelectedIndexChangedBuilding", path);

        }
    }
    //protected void OnSelectedIndexChangedDepartment(object sender, EventArgs e)
    //{
    //    //if (ddldept.SelectedIndex == 0)
    //    //{
    //    //    ddlAsstID.Items.Clear();
    //    //}
    //    //else
    //    //{
    //        PrintBL objBL = new PrintBL();

    //       // objBL.BindActiveAssetsForEncode(ddlproCategory.SelectedValue, ddlsubcat.SelectedValue, ddlloc.SelectedValue, ddlbuild.SelectedValue, ddlfloor.SelectedValue, ddldept.SelectedValue);
    //   // }
    //}

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            // this.AssetId = 0;
            txtAssetCode.Text = "";
            ddlproCategory.SelectedIndex = 0;
            ddlsubcat.SelectedIndex = 0;
            ddlbuild.SelectedIndex = 0;
            ddlfloor.SelectedIndex = 0;
            ddlloc.SelectedIndex = 0;
            ddldept.SelectedIndex = 0;
            // OnSelectedIndexChangedDepartment(sender, e);
            BindGrid_view();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "btnRefresh_Click", path);

        }
    }
    protected void BtnSendTHR_Click(object sender, EventArgs e)
    {
        try
        {
            //if (this.dtAssetDetails != null)
            if (dtAssetDetails.Rows.Count > 0)
            {
                Session["THREncode"] = "";
                DataTable dt_SelectedAsset = new DataTable();
                dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

                foreach (GridDataItem item in gvData.Items)
                {
                    HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                    CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                    if (chkitem.Checked == true)
                    {
                        dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    }
                }

                //foreach (DataGridItem item in gridlist.Items)
                //{
                //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                //    {
                //        HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                //        CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("chkitem");
                //        if (chkitem.Checked == true)
                //        {
                //            dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                //        }

                //    }

                //}


                dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
                dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
                var results = (from table1 in dtAssetDetails.AsEnumerable()
                               join table2 in dt_SelectedAsset.AsEnumerable()
                               on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                               select table1).ToList();

                DataTable dt_PrintData = new DataTable();
                if (results.Count() > 0)
                {
                    dt_PrintData = results.CopyToDataTable();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Select Assets for Encode..!!');", true);
                    return;
                }

                System.Text.StringBuilder sp = new System.Text.StringBuilder();
                sp.Append("<Tagit>");
                sp.Append("<LineItems>");
                string output = "";
                foreach (DataRow dr in dt_PrintData.Rows)
                {
                    sp.Append("<LineItem AssetId =" + '"' + dr["AssetId"].ToString() + '"' + " Asset_Code =" + '"' + dr["AssetCode"].ToString() + '"' + " Category =" + '"' + dr["Category"].ToString() + '"' + " CategoryCode =" + '"' + dr["CategoryCode"].ToString() + '"' + " SubCategory =" + '"' + dr["SubCategory"].ToString() + '"' + " SubCategoryCode =" + '"' + dr["SubCatCode"].ToString() + '"' + " />");
                    output = output + dr["AssetId"].ToString() + ",";
                }
                sp.Append("</LineItems>");
                sp.Append("</Tagit>");

                PrintBL objEncode = new PrintBL();
                objEncode.UpdateEncodeStatusAssetMaster(output, Convert.ToInt32(Session["userid"]), "ExportTHR");
                Session["THREncode"] = sp.ToString();
                MessageBox.Show("Selected assets sent to THR for encoding.");
                string strPopup = "<script language='javascript' ID='script1'>"

                   // Passing intId to popup window.
                   + "window.open('DownloadTHR.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

                   + "</script>";

                ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                btnRefresh_Click(sender, e);
                //Download the Text file.
                //Response.Clear();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=Encode_List.xml");
                //Response.Charset = "";
                //Response.ContentType = "application/text";
                //Response.Output.Write(sp);
                ////Response.Write(sp);
                //Response.Flush();
                //Response.End();

            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record found..!!');", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "BtnSendTHR_Click", path);
            throw;
        }



    }
    protected void BtnSendTHS_Click(object sender, EventArgs e)
    {
        try
        {
            //if (this.dtAssetDetails != null)
            if (dtAssetDetails.Rows.Count > 0)
            {

                Session["THSEncode"] = "";
                Session["THSTagging"] = "";

                DataTable dt_SelectedAsset = new DataTable();
                dt_SelectedAsset.Columns.Add("AssetId", typeof(int));

                foreach (GridDataItem item in gvData.Items)
                {
                    HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                    CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("cboxSelect");
                    if (chkitem.Checked == true)
                    {
                        dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                    }
                }

                //foreach (DataGridItem item in gridlist.Items)
                //{
                //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                //    {
                //        HiddenField hdnAstID = (HiddenField)item.Cells[1].FindControl("hdnAstID");
                //        CheckBox chkitem = (CheckBox)item.Cells[1].FindControl("chkitem");
                //        if (chkitem.Checked == true)
                //        {
                //            dt_SelectedAsset.Rows.Add(hdnAstID.Value);
                //        }
                //    }
                //}
                dtAssetDetails.PrimaryKey = new DataColumn[] { dtAssetDetails.Columns["AssetId"] };
                dt_SelectedAsset.PrimaryKey = new DataColumn[] { dt_SelectedAsset.Columns["AssetId"] };
                var results = (from table1 in dtAssetDetails.AsEnumerable()
                               join table2 in dt_SelectedAsset.AsEnumerable()
                               on table1.Field<int>("AssetId") equals table2.Field<int>("AssetId")
                               select table1).ToList();
                DataTable dt_PrintData = new DataTable();
                var JSONString = new StringBuilder();
                string output = "";
                if (results.Count() > 0)
                {
                    dt_PrintData = results.CopyToDataTable();
                    dt_PrintData = RemoveColumnsFromTable(dt_PrintData);

                    JSONString.Append("[");
                    for (int i = 0; i < dt_PrintData.Rows.Count; i++)
                    {
                        output = output + dt_PrintData.Rows[i]["AssetId"].ToString() + ",";
                        JSONString.Append("{");
                        for (int j = 0; j < dt_PrintData.Columns.Count; j++)
                        {
                            if (j < dt_PrintData.Columns.Count - 3)
                            {
                                if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetCode" || dt_PrintData.Columns[j].ColumnName.ToString() == "Category" || dt_PrintData.Columns[j].ColumnName.ToString() == "SubCategory"
                                    || dt_PrintData.Columns[j].ColumnName.ToString() == "DeliveryDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "AssignDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "Description" ||
                                    dt_PrintData.Columns[j].ColumnName.ToString() == "Floor" || dt_PrintData.Columns[j].ColumnName.ToString() == "Building" || dt_PrintData.Columns[j].ColumnName.ToString() == "SerialNo"
                                    || dt_PrintData.Columns[j].ColumnName.ToString() == "Location" || dt_PrintData.Columns[j].ColumnName.ToString() == "Custodian" || dt_PrintData.Columns[j].ColumnName.ToString() == "SupplierName")
                                {
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt_PrintData.Rows[i][j].ToString().Replace("\"", "'") + "\",");
                                }

                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetId")
                                {
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + dt_PrintData.Rows[i][j].ToString() + ",");
                                }
                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Quantity")
                                {
                                    string Quantity = dt_PrintData.Rows[i][j].ToString() == "" ? "1" : dt_PrintData.Rows[i][j].ToString();
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Quantity + ",");
                                }
                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Price")
                                {
                                    string Price = dt_PrintData.Rows[i][j].ToString() == "" ? "0" : dt_PrintData.Rows[i][j].ToString();
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Price + ",");
                                }

                            }
                            else if (j == dt_PrintData.Columns.Count - 3)
                            {
                                if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetCode" || dt_PrintData.Columns[j].ColumnName.ToString() == "Category" || dt_PrintData.Columns[j].ColumnName.ToString() == "SubCategory"
                                    || dt_PrintData.Columns[j].ColumnName.ToString() == "DeliveryDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "AssignDate" || dt_PrintData.Columns[j].ColumnName.ToString() == "Description" ||
                                    dt_PrintData.Columns[j].ColumnName.ToString() == "Floor" || dt_PrintData.Columns[j].ColumnName.ToString() == "Building" || dt_PrintData.Columns[j].ColumnName.ToString() == "SerialNo"
                                    || dt_PrintData.Columns[j].ColumnName.ToString() == "Location" || dt_PrintData.Columns[j].ColumnName.ToString() == "Custodian" || dt_PrintData.Columns[j].ColumnName.ToString() == "SupplierName")
                                {
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt_PrintData.Rows[i][j].ToString().Replace("\"", "'") + "\"");
                                }
                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "AssetId")
                                {
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + dt_PrintData.Rows[i][j].ToString());
                                }
                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Quantity")
                                {
                                    string Quantity = dt_PrintData.Rows[i][j].ToString() == "" ? "1" : dt_PrintData.Rows[i][j].ToString();
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Quantity);
                                }
                                else if (dt_PrintData.Columns[j].ColumnName.ToString() == "Price")
                                {
                                    string Price = dt_PrintData.Rows[i][j].ToString() == "" ? "0" : dt_PrintData.Rows[i][j].ToString();
                                    JSONString.Append("\"" + dt_PrintData.Columns[j].ColumnName.ToString() + "\":" + Price);
                                }

                            }

                        }

                        if (i == dt_PrintData.Rows.Count - 1)
                        {
                            JSONString.Append("}");
                        }
                        else
                        {
                            JSONString.Append("},");
                        }

                    }
                    JSONString.Append("]");

                    Session["THSEncode"] = JSONString.ToString();
                    PrintBL objEncode = new PrintBL();
                    objEncode.UpdateEncodeStatusAssetMaster(output, Convert.ToInt32(Session["userid"]), "ExportTHS");
                    MessageBox.Show("Selected assets sent to THS for encoding.");
                    string strPopup = "<script language='javascript' ID='script1'>"

                                 // Passing intId to popup window.
                                 + "window.open('DownloadTHS.aspx','new window', 'top=90, left=200, width=300, height=100, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')"

                                 + "</script>";

                    ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Select Assets for Encode!!');", true);
                    return;
                }

                //Response.ClearHeaders();
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Encode_List.txt");
                //Response.AppendHeader("Content-Length", (JSONString.Length + 2).ToString());
                //Response.ContentType = "xml";
                //Response.Write(JSONString.ToString());
                //btnRefresh_Click(sender, e);

            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record found..!!');", true);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "BtnSendTHS_Click", path);
        }

    }

    private DataTable RemoveColumnsFromTable(DataTable dt_PrintData)
    {
        try
        {
            dt_PrintData.Columns.Remove("BuildingId");
            dt_PrintData.Columns.Remove("CategoryId");
            dt_PrintData.Columns.Remove("CategoryCode");
            dt_PrintData.Columns.Remove("CustodianId");
            dt_PrintData.Columns.Remove("DepartmentId");
            dt_PrintData.Columns.Remove("LocationId");
            dt_PrintData.Columns.Remove("FloorId");
            dt_PrintData.Columns.Remove("SubCatId");
            dt_PrintData.Columns.Remove("SubCatCode");
            dt_PrintData.Columns.Remove("Active");
            // dt_PrintData.Columns.Remove("");

            return dt_PrintData;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "RemoveColumnsFromTable", path);
            return null;
        }
    }

    protected void BtnTsbEncode_Click(object sender, EventArgs e)
    {

    }
    protected void BtnSyncTHS_Click(object sender, EventArgs e)
    {
        try
        {
            if ((productimguploder.HasFile))
            {
                DataTable dtTHS = new DataTable();
                string JsonString = string.Empty;
                string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
                if (validateTxtFile(strFileType, strFileName) == true)
                {
                    var stream = File.OpenText(Server.MapPath("~/SyncFolder/" + strFileName + strFileType));
                    JsonString = stream.ReadToEnd();
                    //if (isValidJSON(JsonString) == false)
                    //{
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);
                    //    return;
                    //}
                    dtTHS = (DataTable)JsonConvert.DeserializeObject(JsonString, (typeof(DataTable)));
                    if (dtTHS.Columns.Count == 16 && dtTHS.Columns.Contains("IsEncoded") && dtTHS.Columns.Contains("AssetCode"))
                    {
                        OperationBL objOperation = new OperationBL();
                        objOperation.UpdateAssetsEncodedByTHS(dtTHS);



                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Encoded by THS imported Successfully..!!');", true);
                        BindGrid_view();
                        gvData.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);

                    }

                }
                else
                {
                    lblMessage.Text = "Only txt files allowed";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    return;
                }
            }
            else
            {
                lblMessage.Text = "Please select an excel file first";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }


        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "BtnSyncTHS_Click", path);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('"+ ex.ToString() +"');", true);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "isValidJSON", path);
            return false;
        }
    }
    private bool validateTxtFile(string strFileType, string strFileName)
    {
        if (strFileType == ".txt" || strFileType == ".Txt")
        {
            productimguploder.SaveAs(Server.MapPath("~/SyncFolder/" + strFileName + strFileType));
            return true;

        }
        else
        {

            return false;
        }
    }
    protected void BtnSyncTHR_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTHR = new DataTable();
            DataSet ds = new DataSet();
            string JsonString = string.Empty;
            string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string strFileType = System.IO.Path.GetExtension(productimguploder.FileName).ToString().ToLower();
            if (validateXMLFile(strFileType, strFileName) == true)
            {
                string FilePath = Server.MapPath("~/SyncFolder/" + strFileName + strFileType);
                ds.ReadXml(FilePath);
                dtTHR = ds.Tables["LineItem"];
                //if (dtTHR.Columns.Count == 8 && dtTHR.Columns.Contains("IsEncoded") && dtTHR.Columns.Contains("Asset_Code"))
                if (dtTHR.Columns.Count == 5 && dtTHR.Columns.Contains("Status") && dtTHR.Columns.Contains("Asset_Code"))
                {
                    OperationBL objOperation = new OperationBL();
                    objOperation.UpdateAssetsEncodedByTHR(dtTHR);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Encoded by THR imported Successfully..!!');", true);
                    BindGrid_view();
                    gvData.DataBind();
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Invalid File..!!');", true);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "BtnSyncTHR_Click", path);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('"+ ex.ToString() +"');", true);

        }

    }

    private bool validateXMLFile(string strFileType, string strFileName)
    {
        if (strFileType == ".xml")
        {
            productimguploder.SaveAs(Server.MapPath("~/SyncFolder/" + strFileName + strFileType));
            return true;
        }
        else
        {
            return false;
        }
    }


    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindGrid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "gvData_NeedDataSource", path);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('"+ ex.ToString() +"');", true);

        }
    }

    protected void HeaderCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox headercheckbox = (CheckBox)sender;
            if (headercheckbox.Checked)
            {
                foreach (GridDataItem item in gvData.Items)
                {
                    item.Selected = headercheckbox.Checked;
                    CheckBox checkbox = (CheckBox)item.FindControl("CheckBox1");
                    checkbox.Checked = headercheckbox.Checked;
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "HeaderCheckBox_CheckedChanged", path);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('"+ ex.ToString() +"');", true);

        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            //Is it a GridDataItem
            if (e.Item is GridDataItem)
            {

                GridDataItem dataBoundItem = e.Item as GridDataItem;


                if (dataBoundItem["IsEncodedTHR"].Text == "1")
                {
                    Color clr = ColorTranslator.FromHtml("#d0f2bc");
                    e.Item.BackColor = clr; // for whole row #66FFC2
                }

                if (dataBoundItem["IsEncodedTHS"].Text == "1")
                {
                    Color clr = ColorTranslator.FromHtml("#f2f1bc");
                    e.Item.BackColor = clr; // for whole row
                }


            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Encode.aspx", "gvData_ItemDataBound", path);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('"+ ex.ToString() +"');", true);

        }
    }
}