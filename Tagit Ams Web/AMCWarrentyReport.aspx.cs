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

public partial class AMCWarrentyReport : System.Web.UI.Page
{
    public String Category = System.Configuration.ConfigurationManager.AppSettings["Category"];
    public String SubCategory = System.Configuration.ConfigurationManager.AppSettings["SubCategory"];
    public String Location = System.Configuration.ConfigurationManager.AppSettings["Location"];
    public String Building = System.Configuration.ConfigurationManager.AppSettings["Building"];
    public String Floor = System.Configuration.ConfigurationManager.AppSettings["Floor"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];

    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.DataBind();
            if (Session["userid"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (userAuthorize((int)pages.Statistics, Session["userid"].ToString()) == true)
            {
                divSearch.Style.Add("display", "none");
                txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                txtExpiry.Text = null; System.DateTime.Now.ToString("MM/dd/yyyy");
                Bincategory();
                ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                //lblTotHeader.Visible = false;
            }
            else
            {
                divSearch.Style.Add("display", "none");
                txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                txtExpiry.Text = null;//System.DateTime.Now.ToString("MM/dd/yyyy");
                Bincategory();
                ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
                Response.Redirect("AcceessError.aspx");
            }

        }
    }

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

    protected void OnSelectedIndexChangedCategory(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlDataAdapter dpt = new SqlDataAdapter();



        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getSubCategory, new SqlParameter[] { 
                      new SqlParameter("@CategoryID",  ddlproCategory.SelectedValue),

                    });
        DataSet ds = help.ExecuteDataset();
        ddlsubcat.DataSource = ds;
        ddlsubcat.DataTextField = "SubCatName";
        ddlsubcat.DataValueField = "SubCatID";
        ddlsubcat.DataBind();
        ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    private void Bincategory()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        SqlDataAdapter dpt = new SqlDataAdapter();


        DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

        ddlproCategory.DataSource = ds;
        ddlproCategory.DataTextField = "CategoryName";
        ddlproCategory.DataValueField = "CategoryID";
        ddlproCategory.DataBind();
        ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        txtFrmDate.Text = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
        txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
        txtExpiry.Text = "";
        ddlproCategory.SelectedIndex = 0;
        ddlsubcat.SelectedIndex = 0;
        grid_view();
        gvData.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grid_view();
        gvData.DataBind();
    }

    private void grid_view()
    {
        string FromDate = (txtFrmDate.Text.ToString().Trim() == "") ? null : txtFrmDate.Text;
        string ToDate = (txtToDate.Text.ToString().Trim() == "") ? null : txtToDate.Text;
        string ExpiryDate = (txtExpiry.Text.ToString().Trim() == "") ? null : txtExpiry.Text;
        FromDate = FromDate == null ? ToDate = null : FromDate;
        ToDate = ToDate == null ? FromDate = null : ToDate;
        string CatID = (ddlproCategory.SelectedValue == "0") ? "0" : ddlproCategory.SelectedValue;
        string SubCatID = (ddlsubcat.SelectedValue == "0") ? "1" : ddlsubcat.SelectedValue;
        ReportBL objReport = new ReportBL();
        DataSet ds = objReport.GetWarrentyAMCHistory(FromDate, ToDate, CatID, SubCatID, ExpiryDate);
        if (ds.Tables[0].Rows.Count > 0)
        {
            this.dt_result = ds.Tables[0];
            gvData.DataSource = ds;
            gvData.Visible = true;
        }
        else
        {
            gvData.DataSource = string.Empty;
            gvData.Visible = false;
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        //PrepareForExport(gridlist);
        ExportToExcel();
    }

    //protected void gridlist_PageChanger(Object sender, DataGridPageChangedEventArgs e)
    //{
    //    gridlist.CurrentPageIndex = e.NewPageIndex;
    //    grid_view();
    //}

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

    private void ExportToExcel()
    {
        if (gvData.Visible == true)
        {
            ////Response.Clear();
            ////Response.Clear();
            ////Response.AddHeader("content-disposition",
            ////                      "attachment;filename=PrintedLabelsHistory.xls");
            ////Response.Charset = String.Empty;
            ////Response.ContentType = "application/ms-excel";
            ////StringWriter stringWriter = new StringWriter();
            ////HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            ////gridlist.AllowPaging = false;
            ////gridlist.DataSource = this.dt_result;
            ////gridlist.DataBind();
            ////gridlist.RenderControl(HtmlTextWriter);
            ////Response.Write(stringWriter.ToString());
            ////Response.End();

            if (dt_result.Columns.Contains("Price1"))
            {
                dt_result.Columns.Remove("Price1");

            }

            GridView GridView2 = new GridView();
            GridView2.AllowPaging = false;
            GridView2.DataSource = dt_result;
            GridView2.DataBind();


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=AMC_Warranty_details.xls");
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

    protected void gvData_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        grid_view();
    }

}