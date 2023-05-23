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
using Telerik.Web.UI;
using ECommerce.Common;

public partial class UserMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (userAuthorize((int)pages.UserManagement, Session["userid"].ToString()))
            {
                //if (Session["UserType"].ToString() == "1")
                //{
                BindType();
                grid_view();
                BindPer("0");
                //}
                //else
                //{
                //    ModalPopupExtender1.Show();
                //}
            }
            else
            {
                //ModalPopupExtender1.Show();
                Response.Redirect("AcceessError.aspx");
            }

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

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void BindType()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();

        string query = "select * from [dbo].[UserType] ";
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        da.Fill(ds);
        ddtype.DataSource = ds;
        ddtype.DataTextField = "Type";
        ddtype.DataValueField = "id";
        ddtype.DataBind();
        ddtype.Items.Insert(0, new ListItem("--Select Type--", "0", true));
    }
    public string StrSort;
    private void grid_view()
    {
        SqlConnection conn = null;
        try
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetUserMaster");

            if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            {
                //gridlist.DataSource = null;
                //gridlist.DataBind();
                gvData.DataSource = null;

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
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnsubmit.Text == "Submit")
            {
                if (txtboxname.Text.ToString().Trim() == "" || txtboxpas.Text.ToString().Trim() == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Username and Password should not be empty..');", true);
                    return;
                }
                bool UserExist = CheckUserExist(txtboxname.Text.Trim());
                if (UserExist == true)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('This User Name Already Exist..');", true);
                }
                else
                {
                    DataAccessHelper1 help = new DataAccessHelper1(
                            StoredProcedures.PinsertUser, new SqlParameter[] 
                        {
                            new SqlParameter("@PASSWORD",txtboxpas.Text.Trim()),
                            new SqlParameter("@Type", ddtype.SelectedValue),
                    new SqlParameter("@Status", chkstatus.Checked == true ? 1 : 0),                    
                    
                      new SqlParameter("@CREATED_BY",Convert.ToInt32( Session["userid"]) ),
                    
                    new SqlParameter("@UserName", txtboxname.Text.Trim()),
                        }
                            );

                    int UserID = Convert.ToInt32(help.ExecuteScalar());
                    if (UserID > 1)
                    {
                        hiduserid.Value = UserID.ToString();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Inserted Successfully..!!');", true);
                        UpdatePermission();
                    }
                    // grid_view();
                }

            }
            else if (btnsubmit.Text == "Update")
            {
                if (txtboxname.Text.ToString().Trim() == "" || txtboxpas.Text.ToString().Trim() == "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Username and Password should not be empty..');", true);
                    return;
                }
                DataAccessHelper1 help = new DataAccessHelper1(
                   StoredProcedures.PinsertUser, new SqlParameter[] { 
                           new SqlParameter("@PASSWORD",txtboxpas.Text.Trim()),
                            new SqlParameter("@Type", ddtype.SelectedValue),
                    new SqlParameter("@Status", chkstatus.Checked == true ? 1 : 0),                    
                   
                      new SqlParameter("@CREATED_BY",Convert.ToInt32( Session["userid"]) ),
                    
                    new SqlParameter("@UserName", txtboxname.Text.Trim()),
                        //new SqlParameter("@UserId", Convert.ToInt32(hiduserid.Value)),  
                        new SqlParameter("@UserId", Convert.ToInt32(Session["UIDD"])), 
                }
                   );

                if (help.ExecuteNonQuery() == 1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Updated Successfully..!!');", true);
                    if (ddtype.SelectedItem.Text != "Admin")
                    {
                        UpdatePermission();
                    }
                    else
                    {
                        ClearDataGrid1();
                    }



                }

                btnsubmit.Text = "Submit";


            }
            hiduserid.Value = "";
            grid_view();
            gvData.DataBind();
            txtboxname.Text = string.Empty;
            txtboxpas.Text = string.Empty;
            chkstatus.Checked = false;

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);

        }
    }

    private void UpdatePermission()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        con.Open();
        using (SqlTransaction Trans = con.BeginTransaction())
        {
            try
            {
                //foreach (DataGridItem item in DataGrid1.Items)
                //{

                //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                //    {
                for (int i = 0; i < lstPermission.Items.Count; i++)
                {
                    string Pageid = lstPermission.Items[i].Value;
                    int Permission = lstPermission.Items[i].Selected == true ? 1 : 0;
                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "PInsertUpdateUserPermission", new SqlParameter[] { 
                            new SqlParameter("@userID", Session["UIDD"]),
                            new SqlParameter("@PageID", Convert.ToInt32(Pageid)),
                            new SqlParameter("@Permission", Permission),
                            new SqlParameter("@CreatedBy", Convert.ToInt32(Session["userid"].ToString())),
                    });
                }

                //HiddenField hdnpageID = (HiddenField)item.Cells[1].FindControl("hdnpageID");
                //CheckBox checkPermission = (CheckBox)item.Cells[2].FindControl("checkPermission");
                //int Permission = checkPermission.Checked == true ? 1 : 0;



                //    }

                //}
                Trans.Commit();
                BindPer("0");
                //ClearDataGrid1();
            }
            catch (Exception ex)
            {

                Trans.Rollback();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + ex.Message + " ..!!');", true);
            }
        }

    }

    private void ClearDataGrid1()
    {
        ////foreach (DataGridItem item in DataGrid1.Items)
        ////{

        ////    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        ////    {
        ////        CheckBox checkPermission = (CheckBox)item.Cells[2].FindControl("checkPermission");
        ////        if (checkPermission.Checked)
        ////        {
        ////            checkPermission.Checked = false;
        ////        }

        ////    }
        ////}
    }

    private bool CheckUserExist(string UserName)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckUserExist", new SqlParameter[] { 
                        new SqlParameter("@UserName", UserName),
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
    protected void EditDataGrid(Object sender, DataGridCommandEventArgs e)
    {
        Label UserName = e.Item.Cells[0].FindControl("UserName") as Label;
        Label UserId = e.Item.Cells[0].FindControl("UserId") as Label;
        hiduserid.Value = UserId.Text;
        HiddenField hidtype = (HiddenField)e.Item.Cells[0].FindControl("hidtype");
        Label PASSWORD = e.Item.Cells[0].FindControl("PASSWORD") as Label;
        Label Active = e.Item.Cells[0].FindControl("Active") as Label;
        ddtype.SelectedValue = hidtype.Value;
        txtboxname.Text = UserName.Text;
        txtboxpas.Text = PASSWORD.Text;
        if (Active.Text == "Active")
        {
            chkstatus.Checked = true;
        }
        else
        {
            chkstatus.Checked = false;
        }
        BindPer(UserId.Text);
        btnsubmit.Text = "Update";
    }

    private void BindPer(string UserID)
    {

        try
        {
            DataSet ds = new DataSet(); ;

            DataAccessHelper1 help = new DataAccessHelper1(
           StoredProcedures.GetModuleWisePermission, new SqlParameter[] { 
                      new SqlParameter("@userID",  Convert.ToInt32(UserID)),

                    });
            ds = help.ExecuteDataset();
            lstPermission.DataSource = ds;
            //lstPermission.DataTextField = "ModuleName";
            //lstPermission.DataValueField = "ModuleID";
            lstPermission.DataTextField = "PageName";
            lstPermission.DataValueField = "PageID";
            lstPermission.DataBind();



            for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            {
                if (ds.Tables[0].Rows[i]["PageName"].ToString() == lstPermission.Items[i].Text.ToString())
                {
                    if (ds.Tables[0].Rows[i]["IsPermission"].ToString() == "1")
                    {
                        lstPermission.Items[i].Selected = true;
                    }
                    else
                    {
                        lstPermission.Items[i].Selected = false;
                    }
                }
            }


            //for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            //{
            //    if (ds.Tables[0].Rows[i]["PageName"].ToString() == lstPermission.Items[i].Text.ToString())
            //    {
            //        if (ds.Tables[0].Rows[i]["IsPermission"].ToString() == "1")
            //        {
            //            lstPermission.Items[i].Selected = true;
            //        }
            //        else
            //        {
            //            lstPermission.Items[i].Selected = false;
            //        }
            //    }
            //}





            //if (ds == null || ds.Tables == null || ds.Tables.Count < 1)
            //{
            //    DataGrid1.DataSource = null;

            //    DataGrid1.DataBind();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];

            //    DataView myView;
            //    myView = ds.Tables[0].DefaultView;

            //    DataGrid1.DataSource = myView;

            //    DataGrid1.DataBind();

            //}
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Problem occured while getting list.<br>" + ex.Message;
        }
    }
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox checkPermission = (CheckBox)e.Item.FindControl("checkPermission");
            checkPermission.Checked = (Convert.ToString(DataBinder.Eval(e.Item.DataItem, "IsPermission")).ToString() == "1") ? true : false;

        }

    }

    // For editing and deleting
    protected void gv_data_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "dit")
            {
                lblMessage.Text = "";

                GridDataItem item = (GridDataItem)e.Item;
                string UserId = item["UserId"].Text;
                string UserName = item["UserName"].Text;
                string Password = item["PASSWORD"].Text;
                string Active = item["Status"].Text;
                string Type = item["Type"].Text;

                Session["UIDD"] = UserId;

                ddtype.SelectedValue = Type;
                txtboxname.Text = UserName;
                txtboxpas.Text = Password;
                if (Active == "Active")
                {
                    chkstatus.Checked = true;
                }
                else
                {
                    chkstatus.Checked = false;
                }
                BindPer(UserId);
                btnsubmit.Text = "Update";

                //Label UserName = e.Item.Cells[0].FindControl("UserName") as Label;
                //Label UserId = e.Item.Cells[0].FindControl("UserId") as Label;
                //hiduserid.Value = UserId.Text;
                //HiddenField hidtype = (HiddenField)e.Item.Cells[0].FindControl("hidtype");
                //Label PASSWORD = e.Item.Cells[0].FindControl("PASSWORD") as Label;
                //Label Active = e.Item.Cells[0].FindControl("Active") as Label;
                //ddtype.SelectedValue = hidtype.Value;
                //txtboxname.Text = UserName.Text;
                //txtboxpas.Text = PASSWORD.Text;
                //if (Active.Text == "Active")
                //{
                //    chkstatus.Checked = true;
                //}
                //else
                //{
                //    chkstatus.Checked = false;
                //}
                //BindPer(UserId.Text);
                //btnsubmit.Text = "Update";

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        grid_view();
    }

    protected void ddtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddtype.SelectedValue == "1")
        {
            for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            {
                lstPermission.Items[i].Selected = true;
            }
            //ddtype.d = false;
        }
        else
        {
            for (int i = 0; i <= lstPermission.Items.Count - 1; i++)
            {
                lstPermission.Items[i].Selected = false;
            }
        }
    }
}