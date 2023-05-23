using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;

namespace Serco
{
    public class OperationBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }



        public DataSet GetAssetDetailsForStockChecking(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForStockChecking, new SqlParameter[] { 
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                           new SqlParameter("@AssetId",  Asset),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public void UpdateAssetsEncodedByTHS(DataTable dtTHS)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    foreach (DataRow dr in dtTHS.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateAssetsEncodedByTHS", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@IsEncodedTHS", Convert.ToInt32(dr["IsEncoded"]))
                         });

                        //if (Convert.ToInt32(dr["IsEncoded"]).ToString() == "1")
                        //{
                        //    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateAssetMasterPrintStatus", new SqlParameter[] {
                        //     new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                        //     new SqlParameter("@UserID", "1"),
                        //     new SqlParameter("@Source", "Encode")
                        // });
                        //}
                    }
                    Trans.Commit();
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public void UpdateAssetsTaggedByTHS(DataTable dtTHS)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    foreach (DataRow dr in dtTHS.Rows)
                    {
                        string isTag = dr["IsTagged"].ToString();


                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateAssetsTaggedByTHS", new SqlParameter[] {
                             new SqlParameter("@AssetID", dr["AssetID"].ToString()),
                             new SqlParameter("@IsTaggedTHS", "1")
                         });
                    }
                    Trans.Commit();
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public void UpdateAssetsEncodedByTHR(DataTable dtTHR)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    foreach (DataRow dr in dtTHR.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateAssetsEncodedByTHR", new SqlParameter[] {
                             new SqlParameter("@AssetID", dr["AssetID"]),
                             new SqlParameter("@IsEncodedTHR", Convert.ToInt32(dr["Status"]))
                         });
                    }
                    Trans.Commit();
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public DataTable GetMasterFile()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_GetMasterData");
            return ds.Tables[0];
        }
    }
}