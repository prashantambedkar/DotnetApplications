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
using System.Text;

public partial class DownloadMasters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnDownloadMasters_THR_Click(object sender, EventArgs e)
    {
        OperationBL objBL = new OperationBL();
        DataTable dtMaster = objBL.GetMasterFile();
        System.Text.StringBuilder sp = new System.Text.StringBuilder();
        sp.Append("<Tagit>");
        sp.Append("<LineItems>");
        foreach (DataRow dr in dtMaster.Rows)
        {
            sp.Append("<LineItem ID =" + '"' + dr["ID"].ToString() + '"' + " Name =" + '"' + dr["Name"].ToString() + '"' + " Type =" + '"' + dr["Type"].ToString() + '"' + " Code =" + '"' + dr["Code"].ToString() + '"' + " />");
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
    protected void btnDownloadMasters_THS_Click(object sender, EventArgs e)
    {

        OperationBL objBL = new OperationBL();
        DataTable dtMaster = objBL.GetMasterFile();
        string Json = DataTableToJSONWithStringBuilder(dtMaster);

        Response.ClearHeaders();
        Response.AppendHeader("Content-Disposition", "attachment; filename=MasterFile.txt");
        Response.AppendHeader("Content-Length", (Json.Length + 2).ToString());
        Response.ContentType = "xml";
        Response.Write(Json.ToString());

    }


    public string DataTableToJSONWithStringBuilder(DataTable table)
    {
        var JSONString = new StringBuilder();
        if (table.Rows.Count > 0)
        {

            DataView view = new DataView(table);
            DataTable distinctValues = view.ToTable(true, "Type");

            foreach (DataRow dr in distinctValues.Rows)
            {
                string Type = dr["Type"].ToString();
                JSONString.Append("\"" + Type + "\":[");

                DataTable selectedTable = table.AsEnumerable()
                            .Where(r => r.Field<string>("Type") == Type)
                            .CopyToDataTable();

                for (int i = 0; i < selectedTable.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < selectedTable.Columns.Count-1; j++)
                    {
                        if (j < selectedTable.Columns.Count - 2)
                        {
                            JSONString.Append("\"" + selectedTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + selectedTable.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 2)
                        {
                            JSONString.Append("\"" + selectedTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + selectedTable.Rows[i][j].ToString() + "\"");
                        }
                    }


                    if (i == selectedTable.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("],");
            }

        }

        if (JSONString.Length > 0)
        {
            int len = JSONString.Length;
            string Json = JSONString.ToString().Substring(0, len - 1);
            Json = "{" + Json + "}";
            return Json.ToString();
        }
        else
        {
            return "";
        }

    }


}