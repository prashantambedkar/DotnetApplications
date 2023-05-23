using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace ECommerce.DataAccess
{
    public class DataAccessMain
    {
        private string procedureName;
        public string ConnectionString
        {
            get
            {
                string con = "";
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Application["ConnectionString"] != null)
                    con = (string)System.Web.HttpContext.Current.Application["ConnectionString"];
                else if (con == "")
                    con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                if (con == "" || con == null)
                {
                    throw new Exception("The Application variable named ConnectionString does not exist, or connectionString ConnectionString does not exist or invalid in current web.config file.\n Please try to add or edit it.\n");
                }
                return con;
            }
        }
        protected string ProcedureName
        {
            get { return procedureName; }
            set { procedureName = value; }
        }
    }

}
