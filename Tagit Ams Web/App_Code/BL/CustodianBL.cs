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
/// Summary description for CustodianBL
/// </summary>
namespace Serco
{
    public class CustodianBL
    {

        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public CustodianBL()
        {

        }

        public bool CheckCustodianExists(string CustName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkCustodianExists", new SqlParameter[] { 
                        new SqlParameter("@CustName", CustName),
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

        public bool CheckCustomerAssociatedwithAnyAssets(string CustodianID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pCustodianBelongToAsset", new SqlParameter[] { 
                        new SqlParameter("@CustodianID", CustodianID),
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

        public bool UpdateCustodianStatus(int CustID , int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateCustodianStatus", new SqlParameter[] { 
           
                            new SqlParameter("@CustodianID",CustID),
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
        public bool UpdateCustodianEncoded(int CustID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateCustodianEncoded", new SqlParameter[] {

                            new SqlParameter("@CustodianID",CustID),
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
    }
}