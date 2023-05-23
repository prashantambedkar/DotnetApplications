using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;
using Serco;
using System.Drawing;
using ECommerce.Common;

public partial class Approve : System.Web.UI.Page
{

    SqlConnection cn;
    DataTable dt_Request = new DataTable();
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public static string path = "";
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];


    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            if (Session["userid"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                Page.DataBind();
                if (userAuthorize((int)pages.Approval, Session["userid"].ToString()))
                {
                    bindgrid();
                    gvData.DataBind(); // Newly added
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "Page_Load", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gvData_PageIndexChanged", path);
        }
    }


    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            bindgrid();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gvData_NeedDataSource", path);
        }
    }

    private void bindgrid()
    {
        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_SHOW_REQUEST";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;
            cmd.Parameters.Add("@SEARCH_FIELD", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@VALUE", SqlDbType.VarChar).Value = "";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt_Request);
            if (dt_Request.Rows.Count > 0)
            {
                gvData.DataSource = dt_Request;
            }
            else
            {
                gvData.DataSource = string.Empty;
            }
            cn.Close();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "bindgrid", path);
            ex.Message.ToString();
        }
        finally
        {
            cn.Close();
        }
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {


            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;



                if ((item["STATUS"].Text.ToLower() == "approved") || (item["STATUS"].Text.ToLower() == "rejected"))
                {
                    for (int i = 0; i < item.Controls.Count; i++)
                    {
                        if (i != 0)
                        {
                            //(item.Controls[i] as GridTableCell).Enabled = false;
                        }
                    }
                }

                //Is it a GridDataItem
                if (e.Item is GridDataItem)
                {
                    //Get the instance of the right type
                    GridDataItem dataBoundItem = e.Item as GridDataItem;
                    //if(dataBoundItem.GetDataKeyValue("ID").ToString() == "you Compared Text") // you can also use datakey also
                    if (dataBoundItem["STATUS"].Text == "Approved")
                    {
                        dataBoundItem["STATUS"].ForeColor = Color.Green; // chanmge particuler cell
                                                                         //e.Item.BackColor = System.Drawing.Color.LightGoldenrodYellow; // for whole row
                                                                         //dataItem.CssClass = "MyMexicoRowClass";
                    }

                    if (dataBoundItem["STATUS"].Text == "Rejected")
                    {
                        dataBoundItem["STATUS"].ForeColor = Color.Red; // chanmge particuler cell
                    }

                    dataBoundItem["VIEW"].ForeColor = Color.BlueViolet;
                    dataBoundItem["Download1"].ForeColor = Color.BlueViolet;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gvData_ItemDataBound", path);
        }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void Update_request(string RequestStatus, string ID, string Type)
    {
        try
        {
            AssetVerification objVer = new AssetVerification();
            objVer.ApproveOrReject(ID, RequestStatus, Session["userid"].ToString(), Type);
            bindgrid();
            gvData.DataBind();

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "Update_request", path);
        }
    }

    protected void gvData_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton button = (LinkButton)item["Download1"].Controls[0];
                //button.Attributes.Add("OnClick", "DoSomething(); return false;");
                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(button);

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gvData_ItemCreated", path);
        }
    }

    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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
                string ID = item["REQUESTID"].Text;
                string Status = item["STATUS"].Text;
                string Type = item["Type"].Text;

                if (Status.ToLower() == "approved" || Status.ToLower() == "rejected")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request already approved.');", true);

                    string Message = "Request already approved.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }

                Update_request("Approved", ID, Type);

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request approved successfully.');", true);

                string Messages = "Request approved successfully.";
                imgpopup.ImageUrl = "images/Success.png";
                lblpopupmsg.Text = Messages;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
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
                string ID = item["REQUESTID"].Text;
                string Status = item["STATUS"].Text;
                string Type = item["TYPE"].Text;

                if (Status.ToLower() == "approved" || Status.ToLower() == "rejected")
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request already rejected.');", true);
                    string Message = "Request already rejected.";
                    imgpopup.ImageUrl = "images/info.jpg";
                    lblpopupmsg.Text = Message;
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                    return;
                }
                Update_request("Rejected", ID, Type);
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Request rejected successfully.');", true);

                string Messages = "Request rejected successfully.";
                imgpopup.ImageUrl = "images/Success.png";
                lblpopupmsg.Text = Messages;
                trheader.BgColor = "#98CODA";
                trfooter.BgColor = "#98CODA";
                ModalPopupExtender2.Show();
            }

            if (e.CommandName == "VIEW")
            {

                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["REQUESTID"].Text;
                string Type = item["TYPE"].Text;


                ReportBL objReport = new ReportBL();
                DataSet ds = objReport.GetAssetsToApprove(ID);
                DataTable dt_rpt = new DataTable();
                dt_rpt = ds.Tables[0];



                if (Type.ToLower().Contains("location"))
                {

                    gv_Popup.MasterTableView.GetColumn("FROM_CUSTODOAN").Display = false;
                    gv_Popup.MasterTableView.GetColumn("TO_CUSTODOAN").Display = false;
                    gv_Popup.MasterTableView.GetColumn("TO_LOCATION").Display = true;
                    gv_Popup.MasterTableView.GetColumn("FROM_LOCATION").Display = true;

                }
                if (Type.ToLower().Contains("custodian"))
                {
                    gv_Popup.MasterTableView.GetColumn("TO_LOCATION").Display = false;
                    gv_Popup.MasterTableView.GetColumn("FROM_LOCATION").Display = false;
                    gv_Popup.MasterTableView.GetColumn("FROM_CUSTODOAN").Display = true;
                    gv_Popup.MasterTableView.GetColumn("TO_CUSTODOAN").Display = true;
                }


                gv_Popup.Visible = true;
                gv_Popup.DataSource = dt_rpt;
                gv_Popup.DataBind();

                GriddetailsPopup.Show();



            }
            if (e.CommandName == "Download")
            {
                //lblMessage.Text = "";
                GridDataItem item = (GridDataItem)e.Item;
                string ID = item["REQUESTID"].Text;
                string Type = item["Type"].Text;
                DataTable dt_CheckExist = new DataTable();
                cn.Open();
                using (SqlDataAdapter dad = new SqlDataAdapter())
                {
                    dad.SelectCommand = new SqlCommand("SP_GETREQUEST", cn);
                    dad.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dad.SelectCommand.Parameters.Add("@REQUESTID", DbType.String).Value = ID;
                    DataSet ds = new DataSet();
                    dad.Fill(ds, "dt_Check");
                    dt_CheckExist = ds.Tables["dt_Check"];
                }
                cn.Close();
                //Create a dummy GridView

                if (dt_CheckExist.Rows.Count > 0)
                {
                    if (Type.ToLower().Contains("location"))
                    {
                        if (dt_CheckExist.Columns.Contains("FROM_CUSTODIAN"))
                        {
                            dt_CheckExist.Columns.Remove("FROM_CUSTODIAN");
                            dt_CheckExist.Columns.Remove("TO_CUSTODIAN");
                            dt_CheckExist.Columns.Remove("CustodianName");
                        }
                    }
                    if (Type.ToLower().Contains("custodian"))
                    {
                        if (dt_CheckExist.Columns.Contains("TO_LOCATION"))
                        {
                            dt_CheckExist.Columns.Remove("TO_LOCATION");
                            dt_CheckExist.Columns.Remove("CustodianName");
                            //dt_CheckExist.Columns.Remove("FROM_LOCATION");                            
                        }
                    }
                    //added by ponraj
                    try { dt_CheckExist.Columns["AssetId"].ColumnName = "AssetId".Replace("Asset", Assets); } catch { }
                    try { dt_CheckExist.Columns["AssetCode"].ColumnName = "AssetCode".Replace("Asset", Assets); } catch { }
                    try { dt_CheckExist.Columns["LocationName"].ColumnName = Location; } catch { }
                    try { dt_CheckExist.Columns["BuildingName"].ColumnName = Building; } catch { }
                    try { dt_CheckExist.Columns["FloorName"].ColumnName = Floor; } catch { }
                    try { dt_CheckExist.Columns["CategoryName"].ColumnName = Category; } catch { }
                    try { dt_CheckExist.Columns["subcatName"].ColumnName = SubCategory; } catch { }

                }


                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt_CheckExist;
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


                bindgrid();
                gvData.DataBind();

            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gv_data_ItemCommand", path);

        }
        finally
        {

        }
    }

    protected void gvData_Init(object sender, EventArgs e)
    {
        try
        {
            Telerik.Web.UI.GridFilterMenu menu = gvData.FilterMenu;
            int i = 0;
            int ch = menu.Items.Count;

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gvData_Init", path);
        }
    }
    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }

    //added by ponraj
    protected void gv_Popup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["CategoryName"].Text = Category.ToUpper();
                item["SubCatName"].Text = SubCategory.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gv_Popup_ItemDataBound", path);

        }
    }

    protected void gv_Popup_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["AssetCode"].Text = Assets.ToUpper() + " CODE";
                item["CategoryName"].Text = Category.ToUpper();
                item["SubCatName"].Text = SubCategory.ToUpper();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gv_Popup_ItemCreated", path);
        }
    }

    protected void gv_Popup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gv_Popup.DataSource = string.Empty;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Approve.aspx", "gv_Popup_NeedDataSource", path);
        }
    }
}