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
/// Summary description for LocationBL
/// </summary>
namespace Serco
{
    public class LocationBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public LocationBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet GetAllLocationDetails()
        {
            CreateDB();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetLocationDetails");
            return ds;
        }

        public bool CheckLocatonBelongToanyBuilding(string LocationID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "plocationbelongtobuilding", new SqlParameter[] { 
                        new SqlParameter("@LocationID", LocationID),
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

        public bool UpdateLocationStatus(int LocationID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateLocationStatus", new SqlParameter[] { 
           
                            new SqlParameter("@LocationID",LocationID),
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

        public bool CheckLocationExists(string LocationName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkLocationExists", new SqlParameter[] { 
                        new SqlParameter("@LocationName", LocationName),
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