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
/// Summary description for Department
/// </summary>
namespace Serco
{
    public class DepartmentBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public bool checkDepartmentExists(string DepartmentName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkDepartmentExists", new SqlParameter[] { 
                        new SqlParameter("@DepartmentName", DepartmentName),
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

        public bool CheckDepartmentAssociatedwithAnyCustodian(string DepartmentID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pdepartmentbelongtocustodian", new SqlParameter[] { 
                        new SqlParameter("@DepartmentID", DepartmentID),
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

        public bool UpdateDepartmentIdStatus(int DepartmentID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateDepartmentStatus", new SqlParameter[] { 
           
                            new SqlParameter("@DepartmentD",DepartmentID),
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