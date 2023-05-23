using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Caching;
using ECommerce.DataAccess;
using Microsoft.ApplicationBlocks.Data;

/// <summary>
/// Summary description for SearchBL
/// </summary>
namespace Serco
{
    public class SearchBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public void DeleteAssetsFromMaster(string AssetIDs, string TranIDs)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "DeleteAssetsFromMaster", new SqlParameter[] { 
           
                            new SqlParameter("@AssetIDs",AssetIDs),
                            new SqlParameter("@TranIDs",TranIDs),
                     }
                    );
                    Trans.Commit();
                }

                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw ex;
                }
            }
        }

        public bool ContainsInt(string str, int value)
        {
            return str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim()))
                .Contains(value);
        }

    }
}
