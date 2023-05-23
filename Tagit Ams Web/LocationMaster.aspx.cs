using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ECommerce.DataAccess;
using System.Configuration;
using System.Text;
using Serco;
using ECommerce.Common;

using Microsoft.ApplicationBlocks.Data;
using Telerik.Web.UI;

public partial class LocationMaster : System.Web.UI.Page
{
    String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Dispose = System.Configuration.ConfigurationManager.AppSettings["Dispose"];
    public static string path = "";
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];

    public DataTable dt_location
    {
        get
        {
            return ViewState["dt_location"] as DataTable;
        }
        set
        {
            ViewState["dt_location"] = value;

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gvData_PageIndexChanged", path);
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gvData_Init", path);

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
                PgHeader.InnerText = Location + " Master";
                lblcattype.Text = Location + " Name";

                chkActive.Enabled = false;
                chkActive.Checked = true;
                divSearch.Style.Add("display", "none");
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (userAuthorize((int)pages.CompanyMaster, Session["userid"].ToString()) == true)
                {
                    grid_view();
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "Page_Load", path);

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
    public void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            txtFullName.Text = string.Empty;
            txtserchbox.Text = "";
            txtSearch.Text = "";
            btnsubmit.Text = "Submit";
            grid_view();
            gvData.DataBind();
            chkActive.Enabled = false;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "btnreset_Click", path);

        }
    }
    public void InsertLocationPermission(int UserID, int LocationID, int CreatedBY)
    {
        try
        {
            //UserID, LocationID, IsPermission, CreatedBY, CreatedDate
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand("sp_insertLocationPermission", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@CreatedBY", CreatedBY);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "TestMaster.aspx", "InsertLocationPermission", path);
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text.ToLower() == "submit")
            {
                DataAccessHelper1 help = new DataAccessHelper1(
                        StoredProcedures.Pinsertlocation, new SqlParameter[]
                        {
                            new SqlParameter("@LocationName", txtFullName.Text.Trim()),

                    new SqlParameter("@Active",  1 ),
                    new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                    new SqlParameter("@CreatedDate", System.DateTime.Now),


                        }
                        );

                if (help.ExecuteNonQuery() < 1)
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("select LocationId from LocationMaster where LocationName=@LocationName", conn))
                    {
                        cmd.Parameters.AddWithValue("@LocationName", txtFullName.Text.Trim());
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(dt);
                        }
                    }
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was inserted Successfully.');", true);
                    if (dt.Rows.Count > 0)
                    {
                        InsertLocationPermission(Convert.ToInt32(Session["userid"]), Convert.ToInt32(dt.Rows[0]["LocationId"]), Convert.ToInt32(Session["userid"]));
                    }
                    //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                    //imgpopup.ImageUrl = "images/Success.png";
                    //trheader.BgColor = "#98CODA";
                    //trfooter.BgColor = "#98CODA";
                    //lblpopupmsg.Text = Message;
                    //ModalPopupExtender2.Show();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The Location " + txtFullName.Text.Trim() + " as inserted successfully');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
                grid_view();
                btnreset_Click(sender, e);

            }
            else if (btnsubmit.Text.ToLower() == "update")
            {
                if (hdnOldLoc1.Value.ToString().Trim() == txtFullName.Text.ToString().Trim())
                {
                    LocationBL objLocationBL = new LocationBL();
                    if (chkActive.Checked == false)
                    {
                        bool LocationActive = objLocationBL.CheckLocatonBelongToanyBuilding(hdncatidId.Value);
                        if (LocationActive == true)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" already mapped with items, \\n you can’t deactivate this Location');", true);

                            //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" already mapped with items, \n you can’t deactivate this " + Location + ".";
                            //imgpopup.ImageUrl = "images/info.jpg";
                            //lblpopupmsg.Text = Message;
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //ModalPopupExtender2.Show();
                            //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Already Mapped,Cant Deactive');  ", true);
                            //Response.Write("<script>alert('already mapped with items, you can't deactive this Location');</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Already mapped with items,you cannot deactive');", true);
                            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + " " + txtFullName.Text.Trim() + " already mapped with items, you can't deactive this " + Location + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            var result = objLocationBL.UpdateLocationStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //lblpopupmsg.Text = Message;
                                //ModalPopupExtender2.Show();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was not Updated.');", true);
                                //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //lblpopupmsg.Text = Message;
                                //ModalPopupExtender2.Show();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }

                        }
                    }
                    else
                    {
                        var result = objLocationBL.UpdateLocationStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                        if (result)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //lblpopupmsg.Text = Message;
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                            //imgpopup.ImageUrl = "images/CloseRed.png";
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //lblpopupmsg.Text = Message;
                            //ModalPopupExtender2.Show();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }

                    }
                }
                else
                {
                    LocationBL objLoc = new LocationBL();
                    bool LocExist = objLoc.CheckLocationExists(txtFullName.Text.Trim());
                    if (LocExist == true)
                    {
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\"  is Already Exists');", true);

                        //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\"  is already Exists.";
                        //imgpopup.ImageUrl = "images/info.jpg";
                        //lblpopupmsg.Text = Message;
                        //trheader.BgColor = "#98CODA";
                        //trfooter.BgColor = "#98CODA";
                        //ModalPopupExtender2.Show();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\"  is already Exists.');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                    else
                    {
                        DataAccessHelper1 help = new DataAccessHelper1(
                           StoredProcedures.pupdatelocation, new SqlParameter[] {
                          new SqlParameter("@LocationId", Convert.ToInt32(hdncatidId.Value)),
                     new SqlParameter("@LocationName", txtFullName.Text.Trim()),
                     new SqlParameter("@UserId", Convert.ToInt32( Session["userid"])),
                     new SqlParameter("@CreatedDate", System.DateTime.Now),

                }

                           );

                        LocationBL objLocationBL = new LocationBL();
                        if (chkActive.Checked == false)
                        {
                            bool LocationActive = objLocationBL.CheckLocatonBelongToanyBuilding(hdncatidId.Value);
                            if (LocationActive == true)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" is already mapped with items, \\n you can’t deactivate this Location');", true);


                                //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items, \\n you can’t deactivate this " + Location + ".";
                                //imgpopup.ImageUrl = "images/info.jpg";
                                //lblpopupmsg.Text = Message;
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //ModalPopupExtender2.Show();
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" is already mapped with items, \\n you can’t deactivate this " + Location + ".');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('Already mapped with items,cant deactive');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true); return;
                            }
                            else
                            {
                                var result = objLocationBL.UpdateLocationStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                                if (result)
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                                    //imgpopup.ImageUrl = "images/Success.png";
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //lblpopupmsg.Text = Message;
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                                else
                                {
                                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);

                                    //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                    //imgpopup.ImageUrl = "images/CloseRed.png";
                                    //trheader.BgColor = "#98CODA";
                                    //trfooter.BgColor = "#98CODA";
                                    //lblpopupmsg.Text = Message;
                                    //ModalPopupExtender2.Show();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                                }
                            }
                        }
                        else
                        {
                            var result = objLocationBL.UpdateLocationStatus(Convert.ToInt32(hdncatidId.Value), chkActive.Checked == true ? 1 : 0);
                            if (result)
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.";
                                //imgpopup.ImageUrl = "images/Success.png";
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //lblpopupmsg.Text = Message;
                                //ModalPopupExtender2.Show();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.";
                                //imgpopup.ImageUrl = "images/CloseRed.png";
                                //trheader.BgColor = "#98CODA";
                                //trfooter.BgColor = "#98CODA";
                                //lblpopupmsg.Text = Message;
                                //ModalPopupExtender2.Show();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was not updated.');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                        }

                        if (help.ExecuteNonQuery() <= 1)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('The location  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            //string Message = "The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was inserted successfully.";
                            //imgpopup.ImageUrl = "images/Success.png";
                            //trheader.BgColor = "#98CODA";
                            //trfooter.BgColor = "#98CODA";
                            //lblpopupmsg.Text = Message;
                            //ModalPopupExtender2.Show();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pozp1", "setvalueforlocation('The " + Location + "  \"" + txtFullName.Text.Trim() + "\" was updated successfully.');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                        }
                        btnsubmit.Text = "Add";
                    }



                }
            }
            btnreset_Click(sender, e);
            hdncatidId.Value = "";
            hdnOldLoc1.Value = "";
            //grid_view();
            chkActive.Enabled = false;
            chkActive.Checked = true;

        }
        catch (Exception ex)
        {
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " .');", true);
            //Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "btnsubmit_Click", path);
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
            LocationBL objLoc = new LocationBL();
            //DataSet ds = objLoc.GetAllLocationDetails();
            string SearchText = (txtSearch.Text.ToString().ToLower() == "") ? null : txtSearch.Text.ToString().ToLower();
            DataSet ds = Common.GetAllLocationDetailsV2(SearchText, Session["userid"].ToString());

            this.dt_location = ds.Tables[0];
            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                lblMessage.Text = "Problem occured while retrieving " + Location + " records. Please try again.";
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
                //gridlist.DataSource = myView;
                //gridlist.DataBind();

                gvData.DataSource = myView;

            }
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "grid_view", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "btnSearchInfo_Click", path);

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string text = Convert.ToString(txtSearch.Text.ToLower());
            var LocationDetails = dt_location.AsEnumerable().Where(c => c.Field<string>("LocationName").ToLower().Contains(text));
            if (LocationDetails.Count() > 0)
            {
                DataTable dtLocationDetails = LocationDetails.CopyToDataTable<DataRow>();

                gvData.DataSource = dtLocationDetails;
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "btnSearch_Click", path);

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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "ShowSuccessMessage", path);

        }
    }
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        try
        {
            Label LocationName = e.Item.Cells[0].FindControl("LocationName") as Label;
            Label LocationId = e.Item.Cells[0].FindControl("LocationId") as Label;
            hdncatidId.Value = LocationId.Text;
            txtFullName.Text = LocationName.Text;
            hdnOldLoc1.Value = LocationName.Text;
            btnsubmit.Text = "Update";
        }
        catch (Exception Ex)
        {
            lblMessage.Text = Ex.Message;
            Logging.WriteErrorLog(Ex.Message.ToString(), Ex.StackTrace.ToString(), "LocationMaster.aspx", "EditDataGrid", path);

        }
    }

    protected void UpdateLocationStatusOnChange(object sender, EventArgs e)
    {
        try
        {
            LocationBL objLocationBL = new LocationBL();
            DataGridItem item = (DataGridItem)((DropDownList)sender).Parent.Parent;
            Label LocationID = (Label)item.FindControl("Locationid");
            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            bool LocationActive = objLocationBL.CheckLocatonBelongToanyBuilding(LocationID.Text);
            if (LocationActive == true)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This " + Location + " already mapped with items, \\n you can’t deactivate this " + Location + "');", true);

            }
            else
            {

                objLocationBL.UpdateLocationStatus(Convert.ToInt32(LocationID.Text), Convert.ToInt32(ddlStatus.SelectedValue));
            }

            grid_view();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "UpdateLocationStatusOnChange", path);

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

                //Label LocationName = e.Item.FindControl("LocationName") as Label;
                //Label LocationId = e.Item.FindControl("LocationId") as Label;

                //LocationId.Text = "sasdasd";
            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gridlist_ItemDataBound", path);


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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gvData_NeedDataSource", path);


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
                string LocationName = item["LocationName"].Text;
                string LocationId = item["LocationId"].Text;
                string Active = item["Status"].Text;
                if (LocationName == Dispose)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('You can not modify " + Dispose + " " + Location + "');", true);
                    return;
                }

                hdncatidId.Value = LocationId;
                txtFullName.Text = LocationName;
                hdnOldLoc1.Value = LocationName;
                chkActive.Checked = Active == "Active" ? true : false;
                btnsubmit.Text = "Update";
                chkActive.Enabled = true;

                //Label LocationName = e.Item.Cells[0].FindControl("LocationName") as Label;
                //Label LocationId = e.Item.Cells[0].FindControl("LocationId") as Label;
                //hdncatidId.Value = LocationId.Text;
                //txtFullName.Text = LocationName.Text;
                //hdnOldLoc1.Value = LocationName.Text;
                //btnsubmit.Text = "Update";

            }

        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "GvData_ItemCommand", path);


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
                item["LocationCode"].Text = Location.ToUpper() + " ID";
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
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gvData_ItemDataBound", path);

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
                item["LocationCode"].Text = Location.ToUpper() + " ID";
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LocationMaster.aspx", "gvData_ItemCreated", path);

        }
    }
}