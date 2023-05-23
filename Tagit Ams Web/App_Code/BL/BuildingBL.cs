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
/// Summary description for BuildingBL
/// </summary>
namespace Serco
{
    public class BuildingBL
    
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public BuildingBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet GetAllBuildingDetails()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetBuildingDetails");
            return ds;
        }

        public bool CheckBuildingBelongsToAnyFloor(string BuildingID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "pBuildingBelongToFloor", new SqlParameter[] { 
                        new SqlParameter("@BuildingID", BuildingID),
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

        public bool UpdateBuildingStatus(int BuildingID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateBuildingStatus", new SqlParameter[] { 
           
                            new SqlParameter("@BuildingID",BuildingID),
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

        public bool CheckBuidingExists(string BuldingName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkBuildingExists", new SqlParameter[] { 
                        new SqlParameter("@BuildingName", BuldingName),
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