using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
/// <summary>
/// Summary description for FloorBL
/// </summary>
namespace Serco
{
    public class FloorBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public FloorBL()
        {
        }

        public bool CheckFloorAssociatedToAnyAsset(string FloorID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pFloorBelongToAsset", new SqlParameter[] { 
                        new SqlParameter("@FloorID", FloorID),
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

        public bool UpdateFloorStatus(int FloorID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateFloorStatus", new SqlParameter[] { 
           
                            new SqlParameter("@FloorId",FloorID),
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

        public bool CheckFloorExists(string FloorName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkFloorExists", new SqlParameter[] { 
                        new SqlParameter("@FloorName", FloorName),
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