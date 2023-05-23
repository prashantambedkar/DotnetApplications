using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ECommerce.DataAccess;
/// <summary>
/// Summary description for LabelConfig
/// </summary>
namespace Serco
{
    public class LabelConfigBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public DataSet GetLabelConfigDetails(string TagType)
        {
            CreateDB();
            //DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getLabelConfigDetails");
            //return ds;

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getLabelConfigDetails", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(TagType))
                });
            return ds;
        }

        public void UpdateLabelConfiguration(int RowID, string PrintStatus, string Position,string Barcode, string Prefix,string Font,string FontSize,string Orientation,string Logo,string Company)
        {
            try
            {
                CreateDB();

                 DataAccessHelper1 help = new DataAccessHelper1(
                  StoredProcedures.PupdateLabelConfigDetails, new SqlParameter[] { 
                  new SqlParameter("@Id", Convert.ToInt32(RowID)), 
                  new SqlParameter("@PrintStatus", PrintStatus.Trim()),
                  new SqlParameter("@Position", Position.ToString()),
                   new SqlParameter("@Barcode",Convert.ToString(Barcode)),
                  new SqlParameter("@Prefix", Prefix.ToString()),
                  new SqlParameter("@Font", Font.ToString()),
                  new SqlParameter("@FontSize", FontSize.ToString()),
                  new SqlParameter("@Orientation", Orientation.ToString()),
                  new SqlParameter("@Company", Company.ToString()),
                  new SqlParameter("@Logo", Logo.ToString()),
                 }
                 );
                 help.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }


}