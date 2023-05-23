using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Serco;
using ECommerce.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

public partial class LabelConfig : System.Web.UI.Page
{
    public String _Ams = System.Configuration.ConfigurationManager.AppSettings["ApplicationType"];
    public String Assets = System.Configuration.ConfigurationManager.AppSettings["Asset"];
    public static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        path = Server.MapPath("~/ErrorLog.txt");
        try
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
                HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
                HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
                if (Session["userid"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

                if (userAuthorize((int)pages.LabelPrinting, Session["userid"].ToString()) == true)
                {
                    BinTagType();
                    BindFont();
                    BindOrientation();
                    BindGrid(ddlTagType.SelectedItem.ToString());
                    BindBarcode();
                    string TagType = ddlTagType.SelectedItem.ToString();
                    LoadLabelCofigDetails(TagType);
                }
                else
                {
                    Response.Redirect("AcceessError.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "Page_Load", path);

        }
    }

    private void LoadLabelCofigDetails(string TagType)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SP_LABEL_CONFIG_DETAILS", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(TagType))
                });
            ddlbarcode.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt_Details = ds.Tables[0];

                DataRow[] dr_barcode = dt_Details.Select("FieldName='Barcode'");
                if (dr_barcode.Length > 0)
                {
                    try
                    {
                        lblBarcode.Text = dr_barcode[0]["Id"].ToString();
                        txtPosbarcode.Text = dr_barcode[0]["Position"].ToString();
                        ddlOrientation_barcode.Text = dr_barcode[0]["orientation"].ToString();
                        ddlbarcode.Text = dr_barcode[0]["Barcode"].ToString();
                    }
                    catch
                    {

                    }
                }

                DataRow[] dr_logo = dt_Details.Select("FieldName='Logo'");
                if (dr_logo.Length > 0)
                {
                    try
                    {
                        lblLogo.Text = dr_logo[0]["Id"].ToString();
                        txtLogo.Text = dr_logo[0]["Logo"].ToString();
                        txtPosLogo.Text = dr_logo[0]["Position"].ToString();
                    }
                    catch
                    {

                    }
                }

                DataRow[] dr_company = dt_Details.Select("FieldName='Company'");
                if (dr_company.Length > 0)
                {
                    try
                    {
                        lblCompany.Text = dr_company[0]["Id"].ToString();
                        txtCompany.Text = dr_company[0]["Company"].ToString();
                        txtPosCompany.Text = dr_company[0]["Position"].ToString();
                        txtCompanyFontSize.Text = dr_company[0]["FontSize"].ToString();
                        ddlCompayOrient.Text = dr_company[0]["orientation"].ToString();
                        ddlFontCompany.Text = dr_company[0]["Position"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "LoadLabelCofigDetails", path);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "LoadLabelCofigDetails", path);
        }
    }
    private void BindBarcode()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SP_GET_FIELD");
            ddlbarcode.DataSource = ds;
            ddlbarcode.DataTextField = "MappingColumnName";
            ddlbarcode.DataValueField = "ColumnName";
            ddlbarcode.DataBind();
            foreach (ListItem ls in ddlbarcode.Items)
            {
                if (ls.Text.Contains("Asset"))
                {
                    ls.Text = ls.Text.Replace("Asset", Assets);
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "BindBarcode", path);

        }
    }
    private void BinTagType()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlDataAdapter dpt = new SqlDataAdapter();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetTagType");
            ddlTagType.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[4].Delete();
            }
            ddlTagType.DataTextField = "Name";
            ddlTagType.DataValueField = "Id";
            ddlTagType.DataBind();
            //ddlTagType.Items.Insert(0, new ListItem("--Select Tag--", "0", true));
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "BinTagType", path);
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

    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //////e.Row.Cells[7].Visible = false;
            //////e.Row.Cells[6].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row
                DropDownList ddlFont = (e.Row.FindControl("ddlFont") as DropDownList);
                DataTable dt = new DataTable();
                dt.Columns.Add("Font");
                dt.Rows.Add("FONT-D");
                dt.Rows.Add("FONT-E");
                dt.Rows.Add("FONT-F");
                dt.Rows.Add("FONT-G");
                dt.Rows.Add("FONT-O");
                dt.Rows.Add("FONT-P");
                dt.Rows.Add("FONT-Q");

                ddlFont.DataSource = dt;
                ddlFont.DataTextField = "Font";
                ddlFont.DataValueField = "Font";
                ddlFont.DataBind();

                //Add Default Item in the DropDownList
                ddlFont.Items.Insert(0, new ListItem("Please select"));

                //Select the Country of Customer in DropDownList
                string Font = (e.Row.FindControl("lblFont") as Label).Text;
                ddlFont.Items.FindByValue(Font).Selected = true;


                DropDownList ddlOrientation = (e.Row.FindControl("ddlOrientation") as DropDownList);
                DataTable dtOrientation = new DataTable();
                dtOrientation.Columns.Add("Orientation");
                dtOrientation.Rows.Add("Normal");
                dtOrientation.Rows.Add("Rotated");
                dtOrientation.Rows.Add("Inverted");
                dtOrientation.Rows.Add("Bottom up");

                ddlOrientation.DataSource = dtOrientation;
                ddlOrientation.DataTextField = "Orientation";
                ddlOrientation.DataValueField = "Orientation";
                ddlOrientation.DataBind();

                //Add Default Item in the DropDownList
                ddlOrientation.Items.Insert(0, new ListItem("Please select"));

                //Select the Country of Customer in DropDownList
                string Orientation = (e.Row.FindControl("lblOrientation") as Label).Text;
                ddlOrientation.Items.FindByValue(Orientation).Selected = true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "GridView_RowDataBound", path);
        }
    }

    private void BindGrid(string TagType)
    {
        try
        {
            LabelConfigBL objLB = new LabelConfigBL();
            DataSet ds = objLB.GetLabelConfigDetails(TagType);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[15].Delete();
            }
            GridView1.DataSource = string.Empty;
            GridView1.DataSource = ds;
            GridView1.DataBind();
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "BindGrid", path);
        }
    }
    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.BindGrid(ddlTagType.SelectedItem.ToString());
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "OnRowEditing", path);
        }
    }
    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        try
        {
            GridView1.EditIndex = -1;
            this.BindGrid(ddlTagType.SelectedItem.ToString());
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "OnRowCancelingEdit", path);
        }
    }

    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int RowID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string PrintStatus = (row.Cells[4].Controls[0] as TextBox).Text;
            string Barcode = (row.Cells[6].Controls[0] as TextBox).Text;
            var testPrintStatus = new Regex(@"^\d{0,1}$");
            var resultStatus = testPrintStatus.IsMatch(PrintStatus);
            bool isSelected = (row.FindControl("chkSelect") as CheckBox).Checked;
            if (isSelected == true)
            {
                PrintStatus = "1";
            }
            else
            {
                PrintStatus = "0";
            }
            string font = (GridView1.Rows[e.RowIndex].FindControl("ddlFont") as DropDownList).SelectedItem.Value;
            string fontsize = (row.Cells[9].Controls[0] as TextBox).Text;
            string Orient = (GridView1.Rows[e.RowIndex].FindControl("ddlOrientation") as DropDownList).SelectedItem.Value;


            if (resultStatus == false)
            {
                (row.Cells[3].Controls[0] as TextBox).Text = "Error";
                (row.Cells[3].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                return;
            }
            if (PrintStatus != "" && PrintStatus != "0" && PrintStatus != "1")
            {
                (row.Cells[3].Controls[0] as TextBox).Text = "Error";
                (row.Cells[3].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                return;


            }

            string Position = (row.Cells[5].Controls[0] as TextBox).Text;
            var test = new Regex(@"^\d+([\,\,]?\d+)?$");
            if (Position != "")
            {
                if (Position.Contains(","))
                {
                    var commas = Position.Split(',').Count();
                    if (Position.Split(',').Count() > 2)
                    {
                        (row.Cells[5].Controls[0] as TextBox).Text = "Error";
                        (row.Cells[5].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                        return;
                    }
                }
                var result = test.IsMatch(Position);
                if (result == false)
                {
                    (row.Cells[5].Controls[0] as TextBox).Text = "Error";
                    (row.Cells[5].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                    return;
                }

            }

            if (Barcode != "")
            {
                if (Barcode.Contains(","))
                {
                    var commas = Barcode.Split(',').Count();
                    if (Barcode.Split(',').Count() > 2)
                    {
                        (row.Cells[5].Controls[0] as TextBox).Text = "Error";
                        (row.Cells[5].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                        return;
                    }
                }
                var result = test.IsMatch(Barcode);
                if (result == false)
                {
                    (row.Cells[5].Controls[0] as TextBox).Text = "Error";
                    (row.Cells[5].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                    return;
                }

            }
            if (PrintStatus != "" && Position == "")
            {
                (row.Cells[4].Controls[0] as TextBox).Text = "Error";
                (row.Cells[4].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                return;
            }
            if (PrintStatus == "" && Position != "")
            {
                (row.Cells[3].Controls[0] as TextBox).Text = "Error";
                (row.Cells[3].Controls[0] as TextBox).BorderColor = System.Drawing.Color.Red;
                return;
            }

            string Prefix = (row.Cells[7].Controls[0] as TextBox).Text;

            LabelConfigBL objLB = new LabelConfigBL();
            objLB.UpdateLabelConfiguration(RowID, PrintStatus, Position, Barcode, Prefix, font, fontsize, Orient, "", "");
            GridView1.EditIndex = -1;
            this.BindGrid(ddlTagType.SelectedItem.ToString());
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "OnRowUpdating", path);
        }
    }


    protected void ddlTagType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.BindGrid(ddlTagType.SelectedItem.ToString());
            string TagType = ddlTagType.SelectedItem.ToString();
            LoadLabelCofigDetails(TagType);
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "ddlTagType_SelectedIndexChanged", path);
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        try
        {
            LabelConfigBL objLB = new LabelConfigBL();
            DataSet ds = objLB.GetLabelConfigDetails(ddlTagType.SelectedItem.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable DtPrintPreview = new DataTable();
                DtPrintPreview = ds.Tables[0];
                //DataRow[] dr = DtPrintPreview.Select("PrintStatus=1");
                if (DtPrintPreview.Rows.Count > 0)
                {

                    string ApplicationFolder = Server.MapPath("~/Printing/");
                    string PrintedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string ls_Time = DateTime.Now.ToString().Replace(':', '-');
                    ls_Time = ls_Time.Replace('/', '-');
                    string ls_PrintingDirectory = ApplicationFolder;

                    string ls_FilesToCreate = null;
                    ls_FilesToCreate = ls_PrintingDirectory + "Label" + " " + ".txt";

                    FileInfo printinfo = new FileInfo(ls_FilesToCreate);
                    StreamWriter lsw_DataToPrint = printinfo.CreateText();
                    lsw_DataToPrint.WriteLine("^XA");
                    foreach (DataRow dr in DtPrintPreview.Rows)
                    {
                        if (dr["PrintStatus"].ToString() == "1")
                        {
                            //ZPL Code will start from here (for starting of New label ^XA IS REQUIRED)
                            //lsw_DataToPrint.WriteLine("A" + ls_Category + ls_Sub_Category + ls_AssetCode); 
                            ////if (dr["FieldName"].ToString() == "BARCODE")
                            ////{
                            ////    lsw_DataToPrint.WriteLine("^FO" + dr["Position"].ToString());
                            ////    lsw_DataToPrint.WriteLine("^BY1");
                            ////    string Orient = "N";
                            ////    if(dr["orientation"].ToString()=="Normal")
                            ////    {
                            ////        Orient = "^BCN, 30,N,N,50,N";
                            ////    }
                            ////    else if (dr["orientation"].ToString() == "Rotated")
                            ////    {
                            ////        Orient = "^BCR, 30,N,N,50,N";
                            ////    }
                            ////    else if (dr["orientation"].ToString() == "Inverted")
                            ////    {
                            ////        Orient = "^BCI, 30,N,N,50,N";
                            ////    }
                            ////    else
                            ////    {
                            ////        Orient = "^BCB, 30,N,N,50,N";
                            ////    }

                            ////    //lsw_DataToPrint.WriteLine(dr["orientation"].ToString() == "Normal" ? "^BCN, 30,N,N,50,N" : "^BCR, 30,N,N,50,N");
                            ////    lsw_DataToPrint.WriteLine(Orient);
                            ////    lsw_DataToPrint.WriteLine("^A0," + dr["FontSize"].ToString());
                            ////    lsw_DataToPrint.WriteLine("^FDBARCODE");
                            ////    lsw_DataToPrint.WriteLine("^FS");
                            ////}
                            ////else
                            ////{


                            string Orient = "N";
                            if (dr["orientation"].ToString() == "Normal")
                            {
                                Orient = "N";
                            }
                            else if (dr["orientation"].ToString() == "Rotated")
                            {
                                Orient = "R";
                            }
                            else if (dr["orientation"].ToString() == "Inverted")
                            {
                                Orient = "I";
                            }
                            else
                            {
                                Orient = "B";
                            }


                            lsw_DataToPrint.WriteLine("^FO" + dr["Position"].ToString());
                            //lsw_DataToPrint.WriteLine(dr["orientation"].ToString() == "Normal" ? "^A0N" : "^A0R" + dr["FontSize"].ToString());
                            lsw_DataToPrint.WriteLine(("^A") + (dr["FONT"].ToString().Substring(dr["FONT"].ToString().Length - 1)) + Orient + (dr["FontSize"].ToString()));
                            lsw_DataToPrint.WriteLine("^FD" + dr["FieldName"].ToString());
                            lsw_DataToPrint.WriteLine("^FS");



                            ////}

                        }
                    }

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                    SqlDataAdapter dpt = new SqlDataAdapter();

                    DataSet dst = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SP_LABEL_CONFIG_DETAILS", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(ddlTagType.SelectedItem.ToString()))
                });

                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt_Details = dst.Tables[0];

                        DataRow[] dr_barcode = dt_Details.Select("FieldName='Barcode'");
                        if (dr_barcode.Length > 0)
                        {
                            try
                            {


                                string orient = dr_barcode[0]["orientation"].ToString() == "Normal" ? "N" : "R";
                                lsw_DataToPrint.WriteLine("^FO" + dr_barcode[0]["Position"].ToString());
                                lsw_DataToPrint.WriteLine("^BY1");
                                lsw_DataToPrint.WriteLine("^BC" + orient + ",30,N,N,50,N");
                                lsw_DataToPrint.WriteLine("^FD" + dr_barcode[0]["barcode"].ToString());
                                lsw_DataToPrint.WriteLine("^FS");
                            }
                            catch
                            {

                            }
                        }

                        DataRow[] dr_company = dt_Details.Select("FieldName='Company'");
                        if (dr_company.Length > 0)
                        {
                            try
                            {


                                //dt_lblConfig.Rows[29]["FONT"].ToString().Substring(dt_lblConfig.Rows[29]["FONT"].ToString().Length - 1);
                                string font = dr_company[0]["Font"].ToString().Substring(dr_company[0]["Font"].ToString().Length - 1);
                                string orient = dr_company[0]["orientation"].ToString() == "Normal" ? "N" : "R";

                                lsw_DataToPrint.WriteLine("^FO" + dr_company[0]["Position"].ToString());
                                lsw_DataToPrint.WriteLine("^A" + font + orient + dr_company[0]["FontSize"].ToString());
                                lsw_DataToPrint.WriteLine("^FD" + dr_company[0]["Company"].ToString());
                                lsw_DataToPrint.WriteLine("^FS");

                            }
                            catch
                            {

                            }
                        }
                    }

                    lsw_DataToPrint.WriteLine("^XZ");
                    lsw_DataToPrint.Flush();
                    lsw_DataToPrint.Close();

                    string lLabel = File.ReadAllText(ls_FilesToCreate);
                    if (lLabel == "^XA\r\n^XZ\r\n")
                    {
                        string Message = "No data available to show preview.";
                        imgpopup.ImageUrl = "images/info.jpg";
                        lblpopupmsg.Text = Message;
                        trheader.BgColor = "#98CODA";
                        trfooter.BgColor = "#98CODA";
                        ModalPopupExtender2.Show();
                    }
                    string lLabelSize = "3.2x1.7";
                    ////if (ddlTagType.SelectedIndex == 0)
                    ////{
                    ////    lLabelSize = "4.2x0.6";
                    ////}
                    ////if (ddlTagType.SelectedIndex == 1)
                    ////{
                    ////    lLabelSize = "2.5x2.5";
                    ////}

                    string lUrl = @"http://api.labelary.com/v1/printers/8dpmm/labels/" + lLabelSize + @"/0/" + lLabel;
                    ////Session["PrintPreviewUrl"] = lUrl;

                    ////string pageurl = "LabelPreview.aspx";
                    ////Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");

                    trheaderPrview.BgColor = "#98CODA";
                    //trfooterPreview.BgColor = "#98CODA";
                    Image1.ImageUrl = lUrl;
                    ModalPopupExtender22.Show();




                }
                else
                {
                    trheader.BgColor = "#98CODA";
                    trfooter.BgColor = "#98CODA";
                    ModalPopupExtender2.Show();
                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "btnPreview_Click", path);
        }
    }

    private void BindFont()
    {
        try
        {
            DataTable dt_Font = new DataTable();
            dt_Font.Columns.Add("Font");
            dt_Font.Rows.Add("FONT-D");
            dt_Font.Rows.Add("FONT-E");
            dt_Font.Rows.Add("FONT-F");
            dt_Font.Rows.Add("FONT-G");
            dt_Font.Rows.Add("FONT-O");
            dt_Font.Rows.Add("FONT-P");
            dt_Font.Rows.Add("FONT-Q");

            ddlFontCompany.DataSource = dt_Font;
            ddlFontCompany.DataTextField = "Font";
            ddlFontCompany.DataValueField = "Font";
            ddlFontCompany.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "BindFont", path);

        }

        //ddlTagType.Items.Insert(0, new ListItem("--Select Tag--", "0", true));

    }

    private void BindOrientation()
    {
        try
        {
            DataTable dt_Orientation = new DataTable();
            dt_Orientation.Columns.Add("Orientation");
            dt_Orientation.Rows.Add("Normal");
            dt_Orientation.Rows.Add("Rotated");
            dt_Orientation.Rows.Add("Inverted");
            dt_Orientation.Rows.Add("Bottom up");

            ddlCompayOrient.DataSource = dt_Orientation;
            ddlCompayOrient.DataTextField = "Orientation";
            ddlCompayOrient.DataValueField = "Orientation";
            ddlCompayOrient.DataBind();

            ddlOrientation_barcode.DataSource = dt_Orientation;
            ddlOrientation_barcode.DataTextField = "Orientation";
            ddlOrientation_barcode.DataValueField = "Orientation";
            ddlOrientation_barcode.DataBind();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "BindOrientation", path);
        }

        //ddlTagType.Items.Insert(0, new ListItem("--Select Tag--", "0", true));

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            LabelConfigBL objLB = new LabelConfigBL();
            //Barcode
            objLB.UpdateLabelConfiguration(Convert.ToInt32(lblBarcode.Text), "0", txtPosbarcode.Text, ddlbarcode.Text, "", "FONT-D", "", ddlOrientation_barcode.Text, "", "");
            //Logo
            objLB.UpdateLabelConfiguration(Convert.ToInt32(lblLogo.Text), "0", txtPosLogo.Text, "", "", "", "", "", txtLogo.Text, "");
            //Company
            objLB.UpdateLabelConfiguration(Convert.ToInt32(lblCompany.Text), "0", txtPosCompany.Text, "", "", ddlFontCompany.Text, txtCompanyFontSize.Text, ddlCompayOrient.Text, "", txtCompany.Text);

            string Message = "Set tag position changed successfully";
            imgpopup.ImageUrl = "images/info.jpg";
            lblpopupmsg.Text = Message;
            trheader.BgColor = "#98CODA";
            trfooter.BgColor = "#98CODA";
            ModalPopupExtender2.Show();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "LabelConfig.aspx", "btnSave_Click", path);
        }
    }
}