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
/// Summary description for Category
/// </summary>
namespace Serco
{
    public class CategoryBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public DataSet GetAllCategoryDetails()
        {
            CreateDB();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_GetAllCategory");
            return ds;
        }

        public bool UpdateCategoryStatus(int CatID, int Status)
        {
            bool success = false;
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateCategoryStatus", new SqlParameter[] { 
           
                            new SqlParameter("@CatID",CatID),
                            new SqlParameter("@Status", Status),
                        }
                );
                success = true;
                return success;
            }
            catch (Exception)
            {
                return false;
                
            }
        }

        public DataSet GetAllSubCategoryDetails()
        {
            CreateDB();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetSubCategoryDetails");
            return ds;
        }

        public bool CheckSubCategoryBelongsToAnyAsset(string SubCategoryID)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "chkSubcategoryUsedinAssets", new SqlParameter[] { 
                        new SqlParameter("@SubCategoryID", SubCategoryID),
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

        public bool UpdateSubCategoryStatus(int SubCatID, int Status)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateSubCategoryStatus", new SqlParameter[] { 
           
                            new SqlParameter("@SubCatID",SubCatID),
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



        public bool CheckCategoryBelongToSubCategory(string Category)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "chkCategoryBelongToSubCategory", new SqlParameter[] { 
                        new SqlParameter("@CategoryID", Category),
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

        public bool CheckSubCategoryExists(string SubCategoryName)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "ChkSubCategoryExists", new SqlParameter[] { 
                        new SqlParameter("@SubCategoryName", SubCategoryName),
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