using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECommerce.DataAccess;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using Serco;
using ECommerce.Common;
using Telerik.Web.UI;

public partial class BuildingMaster : System.Web.UI.Page
{
    String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public static string path = "";
    public DataTable dt_Buliding
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gvData_Init", path);
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
                PgHeader.InnerText = Building + " Master";
                lblcattype.Text = Building + " Name";
                LabelLoc.Text = Location;

                chkActive.Enabled = false;
                chkActive.Checked = true;
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                {
                    bindlocation();
                    grid_view();
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "Page_Load", path);
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
    private void bindlocation()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataSet ds = new DataSet();
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + Session["userid"].ToString() + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            ddlloc.DataSource = ds;
            ddlloc.DataTextField = "LocationName";
            ddlloc.DataValueField = "LocationId";
            ddlloc.DataBind();
            ddlloc.Items.Insert(0, new ListItem("--Select--", "0", true));
            //ddllocationRadbox.DataSource = ds;
            //ddllocationRadbox.DataTextField = "LocationName";
            //ddllocationRadbox.DataValueField = "LocationId";
            //ddllocationRadbox.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "bindlocation", path);
        }

    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text.ToLower() == "submit")
            {

                //BuildingBL objBld = new BuildingBL();
                //bool BldExist = objBld.CheckBuidingExists(txtFullName.Text.ToString().Trim());
                //if (BldExist == true)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\"  is already Exists.');", true);
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                //}
                //else
                //{
                DataAccessHelper1 help = new DataAccessHelper1(
                   StoredProcedures.PinsertBuilding, new SqlParameter[]
                   {
                            new SqlParameter("@BuildingName", txtFullName.Text.ToString().Trim()),

                    new SqlParameter("@Active", 1),
                    new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                    new SqlParameter("@CreatedDate", System.DateTime.Now),
                      new SqlParameter("@LocationId", ddlloc.SelectedValue),
                 }
                   );


                if (help.ExecuteNonQuery() < 1)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);

                    //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //lblpopupmsg.Text = Message;
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //ModalPopupExtender2.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                // grid_view();
                //}
            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                if (hdnOldBldgName.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    BuildingBL objBldg = new BuildingBL();
                    if (chkActive.Checked == false)
                    {
                        bool BuildingActive = objBldg.CheckBuildingBelongsToAnyFloor(hdncatidId.Value);
                        if (BuildingActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Building');", true);

                            //string Message = "The " + Building + " \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Building.";
                            //imgpopup.ImageUrl = "images/info.jpg";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + " \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Building.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            var result = objBldg.UpdateBuildingStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                                //string Message = "The " + Building + " \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + " \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {

                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                //string Message = "The " + Building + " \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + " \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }
                    }
                    else
                    {
                        var result = objBldg.UpdateBuildingStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);

                            //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {

                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            //imgpopup.ImageUrl = "images/CloseRed.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                    }
                }
                else
                {
                    BuildingBL objBld = new BuildingBL();
                    bool BldExist = objBld.CheckBuidingExists(txtFullName.Text.ToString().Trim());
                    if (BldExist == true)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\"  is Already Exists');", true);
                        //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\"  is already Exists.";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\"  is already Exists.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {

                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.PupdateBuilding, new SqlParameter[] {
                          new SqlParameter("@BuildingId", Convert.ToInt32(hdncatidId.Value)),
                          new SqlParameter("@BuildingName", txtFullName.Text.ToString().Trim()),
                          new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                          new SqlParameter("@CreatedDate", System.DateTime.Now),
                          new SqlParameter("@LocationId", ddlloc.SelectedValue),

                }
                           );

                        BuildingBL objBldg = new BuildingBL();
                        if (chkActive.Checked == false)
                        {
                            bool BuildingActive = objBldg.CheckBuildingBelongsToAnyFloor(hdncatidId.Value);
                            if (BuildingActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this Building');", true);
                                //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Building + ".";
                                //imgpopup.ImageUrl = "images/info.jpg";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items,\\n you can’t deactivate this " + Building + ".');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                var result = objBldg.UpdateBuildingStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\"  was updated successfully.');", true);
                                    //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                    //imgpopup.ImageUrl = "images/Success.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                                else
                                {

                                    // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                    //imgpopup.ImageUrl = "images/CloseRed.png";
                                    //lblpopupmsg.Text = Message;
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                            }
                        }
                        else
                        {
                            var result = objBldg.UpdateBuildingStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {

                                ////ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }

                        if (help.ExecuteNonQuery() < 1)
                        {
                            ddlloc.Enabled = true;
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The building  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            //string Message = "The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Building + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }

                        btnsubmit.Text = "Add";
                    }
                    //btnreset_Click(sender, e);
                }
            }
            hdncatidId.Value = "";
            hdnOldBldgName.Value = "";
            //grid_view();
            btnreset_Click(sender, e);

            chkActive.Enabled = false;
            chkActive.Checked = true;

        }
        catch (Exception ex)
        {
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "btnsubmit_Click", path);
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
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {
            BuildingBL objBuild = new BuildingBL();
            //DataSet ds = objBuild.GetAllBuildingDetails();
            //  GetAllBuildingDetails
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetAllBuildingDetailsV2(SearchText, Session["userid"].ToString());
            this.dt_Buliding = ds.Tables[0];

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "grid_view", path);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToLower());

            var BuildingDetails = dt_Buliding.AsEnumerable().Where(c => c.Field<string>("BuildingName").ToLower().Contains(text));
            if (BuildingDetails.Count() > 0)
            {
                DataTable dtBuildingDetails = BuildingDetails.CopyToDataTable<DataRow>();
                gvData.DataSource = dtBuildingDetails;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "btnSearch_Click", path);

        }
    }
    private void ShowSuccessMessage(string msg)
    {
        try
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "ShowSuccessMessage", path);
        }
    }


    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtSearch.Text = "";
            ddlloc.Enabled = true;
            ddlloc.SelectedIndex = 0;
            btnsubmit.Text = "SUBMIT";
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "btnreset_Click", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "btnSearchInfo_Click", path);
        }
    }

    protected void UpdateBuildingStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            BuildingBL objBldg = new BuildingBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label BuildingId = (Label)item.FindControl("BuildingId");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool BuildingActive = objBldg.CheckBuildingBelongsToAnyFloor(BuildingId.Text);
            if (BuildingActive == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This " + Building + " already mapped with items,\\n you can’t deactivate this " + Building + "');", true);
            }
            else
            {

                objBldg.UpdateBuildingStatus(Convert.ToInt32(BuildingId.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "UpdateBuildingStatusOnChange", path);

        }
    }
    protected void gridlist_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlStatus = (e.Item.FindControl("ddlStatus") as DropDownList);
                string Active = (e.Item.FindControl("Active") as Label).Text;
                ddlStatus.Items.FindByValue(Active).Selected = true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gridlist_ItemDataBound", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gvData_NeedDataSource", path);
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
                string BuildingName = item["BuildingName"].Text;
                string BuildingId = item["BuildingId"].Text;
                string LocationId = item["LocationId"].Text;
                string Active = item["Status"].Text;

                ddlloc.SelectedValue = LocationId;
                hdncatidId.Value = BuildingId;
                txtFullName.Text = BuildingName;
                hdnOldBldgName.Value = BuildingName;
                ddlloc.Enabled = false;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "UPDATE";
                chkActive.Enabled = true;

                //Label BuildingName = e.Item.Cells[0].FindControl("BuildingName") as Label;
                //Label BuildingId = e.Item.Cells[0].FindControl("BuildingId") as Label;
                //HiddenField hidlocid = e.Item.Cells[0].FindControl("hidlocid") as HiddenField;
                //ddlloc.SelectedValue = hidlocid.Value;
                //hdncatidId.Value = BuildingId.Text;
                //txtFullName.Text = BuildingName.Text;
                //hdnOldBldgName.Value = BuildingName.Text;
                //ddlloc.Enabled = false;
                //btnsubmit.Text = "Update";

            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "GvData_ItemCommand", path);
        }
    }

    protected void ddlloc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlloc.SelectedItem.Value == "1")
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "ddlloc_SelectedIndexChanged", path);
        }
    }
    //added by ponraj
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["LocationName"].Text = Location.ToUpper() + " NAME";
                item["BuildingCode"].Text = Building.ToUpper() + " ID";
                item["BuildingName"].Text = Building.ToUpper() + " NAME";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gvData_ItemDataBound", path);
        }
    }

    protected void gvData_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem item = e.Item as GridHeaderItem;
                item["LocationName"].Text = Location.ToUpper() + " NAME";
                item["BuildingCode"].Text = Building.ToUpper() + " ID";
                item["BuildingName"].Text = Building.ToUpper() + " NAME";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "BuildingMaster.aspx", "gvData_ItemCreated", path);
        }
    }


    protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
       // e.Item.Text = string.Concat(e.Item.Text.ToLower().Split(' ')[0]);
    }
}