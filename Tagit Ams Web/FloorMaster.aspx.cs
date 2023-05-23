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
using Microsoft.ApplicationBlocks.Data;
using ECommerce.Common;
using Serco;
using Telerik.Web.UI;

public partial class FloorMaster : System.Web.UI.Page
{
    String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    public DataTable dt_floor
    {
        get
        {
            return ViewState["dt_Buliding"] as DataTable;
        }
        set
        {
            ViewState["dt_Buliding"] = value;

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "gvData_PageIndexChanged", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "gvData_Init", path);
        }
    }


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
                PgHeader.InnerText = Floor + " Master";
                lblcattype.Text = Floor + " Name";
                LabelLoc.Text = Location;
                LabelBuild.Text = Building;


                divSearch.Style.Add("display", "none");
                chkActive.Enabled = false;
                chkActive.Checked = true;
                if (Session["userid"] != null)
                {
                    if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                    {
                        dt_floor = GetActiveFloor();
                        Common.BindLocation(ddloc, Session["userid"].ToString());
                        ddlbld.Items.Insert(0, new ListItem("--Select--", "0", true));
                        grid_view();
                    }
                    else
                    {
                        Response.Redirect("AcceessError.aspx");
                        //ModalPopupExtender1.Show();
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "Page_Load", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "GetActiveFloor", path);
            return null;
        }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private bool userAuthorize(int PageID, string UserID)
    {
        bool IsValid = Common.ValidateUser(PageID, UserID);
        return IsValid;
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();


            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.Getbuilding, new SqlParameter[] {
                      new SqlParameter("@LocationId",  ddloc.SelectedValue),

                        });
            DataSet ds = help.ExecuteDataset();
            ddlbld.DataSource = ds;
            ddlbld.DataTextField = "BuildingName";
            ddlbld.DataValueField = "BuildingId";
            ddlbld.DataBind();
            ddlbld.Items.Insert(0, new ListItem("--Select--", "0", true));

            if (ddloc.SelectedItem.Value == "1")
            {

                txtFullName.Attributes.Add("disabled", "disabled");


            }
            else
            {
                txtFullName.Attributes.Add("disabled", "false");
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "OnSelectedIndexChanged", path);
        }
    }


    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text.ToLower() == "submit")
            {
                //FloorBL objfloor = new FloorBL();
                //bool FloorExist = objfloor.CheckFloorExists(txtFullName.Text.ToString().Trim());
                //if (FloorExist == true)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\"  is already exists.');", true);
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                //}
                //else

                //{
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.pInsertFloor, new SqlParameter[]
                        {
                             new SqlParameter("@FloorName", txtFullName.Text.ToString().Trim()),
                             new SqlParameter("@Active",1),
                             new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                             new SqlParameter("@BuildingId", ddlbld.SelectedValue),


                        });
                if (help.ExecuteNonQuery() < 1)
                {
                    ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);
                    //string Message = "The " + Floor + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                // }

            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                if (hdnoldFloor.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    FloorBL objFloor = new FloorBL();
                    if (chkActive.Checked == false)
                    {
                        bool FloorActive = objFloor.CheckFloorAssociatedToAnyAsset(hidflrid.Value);
                        if (FloorActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Floor');", true);

                            //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Floor.ToLower() + ".";
                            //imgpopup.ImageUrl = "images/info.jpg";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Floor.ToLower() + ".');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            var result = objFloor.UpdateFloorStatus(Convert.ToInt32(hidflrid.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }
                    }
                    else
                    {
                        var result = objFloor.UpdateFloorStatus(Convert.ToInt32(hidflrid.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                            //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            //imgpopup.ImageUrl = "images/CloseRed.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                    }
                }
                else
                {
                    FloorBL objfloor = new FloorBL();
                    bool FloorExist = objfloor.CheckFloorExists(txtFullName.Text.ToString().Trim());
                    if (FloorExist == true)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\"  is already Exists');", true);

                        //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\"  is already exists.";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\"  is already exists.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.pupdatefloor, new SqlParameter[] {
                           new SqlParameter("@FloorId", Convert.ToInt32(hidflrid.Value)),
                           new SqlParameter("@FloorName", txtFullName.Text.Trim()),
                           new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                           new SqlParameter("@CreatedDate", System.DateTime.Now),
                           new SqlParameter("@BuildingId", ddlbld.SelectedValue),

                           });

                        FloorBL objFloor = new FloorBL();
                        if (chkActive.Checked == false)
                        {
                            bool FloorActive = objFloor.CheckFloorAssociatedToAnyAsset(hidflrid.Value);
                            if (FloorActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Floor');", true);

                                //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Floor.ToLower() + ".";
                                //imgpopup.ImageUrl = "images/info.jpg";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Floor.ToLower() + ".');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                var result = objFloor.UpdateFloorStatus(Convert.ToInt32(hidflrid.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                    //imgpopup.ImageUrl = "images/Success.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                                else
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                    //imgpopup.ImageUrl = "images/CloseRed.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }

                            }
                        }
                        else
                        {
                            objFloor.UpdateFloorStatus(Convert.ToInt32(hidflrid.Value), chkActive.Checked == true ? 1 : 0);
                        }

                        if (help.ExecuteNonQuery() >= 1)
                        {
                            ddloc.Enabled = true;
                            ddlbld.Enabled = true;
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The floor  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                            //string Message = "The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Floor.ToLower() + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }

                        btnsubmit.Text = "Add";
                    }
                }
                //btnreset_Click(sender, e);
            }
            hidflrid.Value = "";
            hdnoldFloor.Value = "";
            grid_view();
            btnreset_Click(sender, e);
            dt_floor = GetActiveFloor();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "btnsubmit_Click", path);
            //string Message = ex.Message;
            //imgpopup.ImageUrl = "images/CloseRed.png";
            //lblpopupmsg.Text = Message;
            //trheader.BgColor = "#98CODA";
            //trfooter.BgColor = "#98CODA";
            //ModalPopupExtender2.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('" + ex.ToString() + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

        }
    }

    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            ddloc.Enabled = true;
            ddlbld.Enabled = true;
            ddlbld.Items.Clear();
            ddlbld.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddloc.SelectedIndex = 0;

            btnsubmit.Text = "Submit";

            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "btnreset_Click", path);
        }
    }
    protected void btnSearchInfo_Click(object sender, EventArgs e)
    {
        try
        {
            // this.AssetId = Convert.ToInt32(ddlAsstID.SelectedValue);
            grid_view();
            gvData.DataBind();
            ////ResetSearch();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "btnSearchInfo_Click", path);
        }
    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            // DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetFloorDetails");
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetFloorDetailsV2(SearchText, Session["userid"].ToString());

            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                gvData.DataSource = string.Empty;
            }
            else
            {
                DataTable dt = ds.Tables[0];

                DataView myView;
                myView = ds.Tables[0].DefaultView;
                lblcnt.Text = Convert.ToString(dt.Rows.Count);
                if (StrSort != "")
                {
                    myView.Sort = StrSort;
                }

                gvData.DataSource = myView;

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "grid_view", path);
        }
    }

    private void ShowSuccessMessage(string msg)
    {
        lblMessage.Text = msg;
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToString().ToLower());

            var FloorDetails = dt_floor.AsEnumerable().Where(c => c.Field<string>("FloorName").ToLower().Contains(text));
            if (FloorDetails.Count() > 0)
            {
                DataTable dtFloorDetails = FloorDetails.CopyToDataTable<DataRow>();
                gvData.DataSource = dtFloorDetails;
                gvData.DataBind();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('No Record Found.!!');", true);

            }
            txtSearch.Text = "";
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "btnSearch_Click", path);
        }
    }
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        try
        {
            Label FloorId = e.Item.Cells[0].FindControl("FloorId") as Label;
            Label FloorName = e.Item.Cells[0].FindControl("FloorName") as Label;
            HiddenField hidbuildid = e.Item.Cells[0].FindControl("hidbuildid") as HiddenField;
            HiddenField hidlocid = e.Item.Cells[0].FindControl("hidlocid") as HiddenField;
            ddloc.SelectedValue = hidlocid.Value;
            OnSelectedIndexChanged(sender, e);
            hidflrid.Value = FloorId.Text;
            hdnoldFloor.Value = FloorName.Text;
            txtFullName.Text = FloorName.Text;
            ddlbld.SelectedValue = hidbuildid.Value;
            ddloc.Enabled = false;
            ddlbld.Enabled = false;
            btnsubmit.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
            Logging.WriteErrorLog(Ex.Message.ToString(), Ex.StackTrace.ToString(), "FloorMaster.aspx", "EditDataGrid", path);
        }
    }
    private bool CheckFloorBelongToAsset(string FloorName)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pFloorBelongToAsset", new SqlParameter[] {
                        new SqlParameter("@FloorName", FloorName),
                 }));
        if (exist == 1)
        {
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
            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "gvData_NeedDataSource", path);
        }
    }

    // For editing and deleting
    protected void GvData_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "dit")
            {
                CategoryBL objcat = new CategoryBL();
                GridDataItem item = (GridDataItem)e.Item;
                string FloorId = item["FloorId"].Text;
                string FloorName = item["FloorName"].Text;
                string hidbuildid = item["BuildingId"].Text;
                string hidlocid = item["LocationId"].Text;
                string Active = item["Status"].Text;

                ddloc.SelectedValue = hidlocid;
                OnSelectedIndexChanged(sender, e);
                hidflrid.Value = FloorId;
                hdnoldFloor.Value = FloorName;
                txtFullName.Text = FloorName;
                ddlbld.SelectedValue = hidbuildid;
                //ddloc.Enabled = false;
                //ddlbld.Enabled = false;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";
                chkActive.Enabled = true;


                //Label FloorId = e.Item.Cells[0].FindControl("FloorId") as Label;
                //Label FloorName = e.Item.Cells[0].FindControl("FloorName") as Label;
                //HiddenField hidbuildid = e.Item.Cells[0].FindControl("hidbuildid") as HiddenField;
                //HiddenField hidlocid = e.Item.Cells[0].FindControl("hidlocid") as HiddenField;
                //ddloc.SelectedValue = hidlocid.Value;
                //OnSelectedIndexChanged(sender, e);
                //hidflrid.Value = FloorId.Text;
                //hdnoldFloor.Value = FloorName.Text;
                //txtFullName.Text = FloorName.Text;
                //ddlbld.SelectedValue = hidbuildid.Value;
                //ddloc.Enabled = false;
                //ddlbld.Enabled = false;
                //btnsubmit.Text = "Update";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "GvData_ItemCommand", path);
        }
    }

    //Added by ponraj

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["FloorName"].Text = Floor.ToUpper() + " NAME";
                item["FloorCode"].Text = Floor.ToUpper() + " ID";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["FloorName"].Text = Floor.ToUpper() + " NAME";
                item["FloorCode"].Text = Floor.ToUpper() + " ID";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "FloorMaster.aspx", "gvData_ItemCreated", path);
        }
    }
}