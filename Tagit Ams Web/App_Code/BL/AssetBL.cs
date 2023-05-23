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
/// Summary description for AssetBL
/// </summary>
namespace Serco
{
    public class AssetBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public bool CheckMappingExistsForTheClient()
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckClientMappingExists"));
                
            if (exist == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetAssetMovement()
        {
            CreateDB();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getMovementHistory");
            return ds;
        }

        public List<MappingInfo> GetMappingListFromDB()
        {
            List<MappingInfo> ListInfo = new List<MappingInfo>();
         
            CreateDB();
            SqlDataReader dr = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "GetMappingLists");
            while (dr.Read())
            {
                MappingInfo objinfo = new MappingInfo();
                objinfo.id = (Int32)dr["id"];
                objinfo.ColumnName = (string)dr["ColumnName"];
                objinfo.MappingColumnName = (string)dr["MappingColumnName"];
                ListInfo.Add(objinfo);
           }
            return ListInfo;
        }
        public bool UpdateAssetEncodedTSB(int AssetID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateAssetEncoded", new SqlParameter[] {

                            new SqlParameter("@AssetID",AssetID),
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
        public bool UpdateAssetTaggedTSB(string AssetID, int Status)
        {
            CreateDB();
            try
            {
                string str = "";
                str = "Update AssetMaster Set IsTagged = " + Status + " where AssetId in (" + AssetID + ")";
                SqlHelper.ExecuteNonQuery(con,CommandType.Text,str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}