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
    public class CompanyBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public void SaveConfiguration(int ImportType, int IsQuantitybase)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "SaveCompanyConfig", new SqlParameter[] {

                            new SqlParameter("@ImportType",ImportType),
                            new SqlParameter("@IsQuantiybase", IsQuantitybase),
                        }
                );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet GetTransferAssetsAccordingToDatedt(string FromDate, string ToDate,string Type)
        {
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand("GetTransferAssetsAccordingToDate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@Todate", ToDate);
                cmd.Parameters.AddWithValue("@Type", Type);
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            return ds;
        }
        public string LocName(string LocationName)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select * from LocationMaster where LocationName='" + LocationName + "'", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["LocationCode"].ToString();
            }
            else
            {
                return "";
            }
        }
        public DataTable pdflogonAddressData(string LocationName)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select pc.imgPath,pc.address from tblPdfReportConfiguration as pc left join LocationMaster as lm on lm.LocationId=pc.LocationId where lm.LocationName='" + LocationName + "'", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable pdflogonAddressDatafromCustodian(string FromCustodian)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select pc.imgPath,pc.address from tblPdfReportConfiguration as pc left join CustodianMaster as cm on lm.LocationId=pc.LocationId where lm.LocationName='" + FromCustodian + "'", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(dt);
                }
            }
            return dt;
        }
        public bool CheckConfiguration()
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckConfigurationExist"));
            if (exist == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet getUserSetting()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetuserSettings");
            return ds;
        }
        public DataSet getUserDetails(string userid)
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetUserDetailByID", new SqlParameter[] {
                            new SqlParameter("@UserID",userid),
            });
            return ds;
        }
        public void Insertlogmaster(string log)
        {
            CreateDB();
            using (SqlCommand cmd = new SqlCommand("insert into logmaster(progress,datetime_) values('" + log + "','" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "')", con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}