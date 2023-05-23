using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using ECommerce.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for SupplierBL
/// </summary>
namespace Serco
{
    public class SupplierBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public SupplierBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool checkSupplierExists(string SupplierName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkSupplierExists", new SqlParameter[] { 
                        new SqlParameter("@SupplierName", SupplierName),
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

        public bool UpdateSupplierStatus(int SupplierID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateSupplierStatus", new SqlParameter[] { 
           
                            new SqlParameter("@SupplierID",SupplierID),
                            new SqlParameter("@Status", Status),
                        }
                );
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool CheckSupplierAssociatedwithAnyAsset(string SupplierID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pSupplierBelongToAsset", new SqlParameter[] { 
                        new SqlParameter("@SupplierID", SupplierID),
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
    }
}