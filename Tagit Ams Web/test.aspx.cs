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
using Telerik.Web.UI;
using System.Globalization;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Session["Dashboard_Filtered_Location"] = null;
        HttpContext.Current.Session["Dashboard_Filtered_LocationV2LocationName"] = null;
        HttpContext.Current.Session["SessionofHealthDataColumn9"] = null;
        HttpContext.Current.Session["Dashboard_Filtered_CaseManagerName"] = null;
        Response.Redirect("Asset.aspx");
    }

}
