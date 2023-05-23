using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Net.Mail;

/// <summary>
/// Summary description for Utilities
/// </summary>
namespace ECommerce.Utilities
{
    public class Utilities
    {
        public Utilities()
        {
            //
            // TODO: Add constructor logic here
            //
        }

       
       
    }
    public class MessageBox
    {
        public static void Show(string _message)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Error", "alert('" + _message + "');", true);
            }
        }


    }


}