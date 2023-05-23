using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Caching;
using ECommerce.DataAccess;
using ECommerce.Common;
using Microsoft.ApplicationBlocks.Data;

/// <summary>
/// Summary description for PrintBL
/// </summary>
namespace Serco
{
    public class PrintBL
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public void UpdateAssetMasterPrintStatus(string AssetID, int UserID, string Source)

        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateAssetMasterPrintStatus", new SqlParameter[] {

                            new SqlParameter("@AssetId",AssetID),
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),
                            new SqlParameter("@Source",Source ),
                        }
                );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void BindAssets(ListBox ddlAsstID, string Category, string SubCategory, string Location, string Building, string Floor, string Department)
        {
            CreateDB();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getAssetinWarrenty, new SqlParameter[] {
                            new SqlParameter("@CategoryId",  Category),
                            new SqlParameter("@SubCatId",  SubCategory),
                            new SqlParameter("@LocationId",  Location),
                            new SqlParameter("@BuildingId",  Building),
                            new SqlParameter("@FloorId",  Floor),
                            new SqlParameter("@DepartmentId",  Department),

                            });
            DataSet ds = help.ExecuteDataset();

            ddlAsstID.DataSource = ds;
            ddlAsstID.DataTextField = "AssetCode";
            ddlAsstID.DataValueField = "AssetId";
            ddlAsstID.DataBind();
        }

        public void BindActiveAssetsForPrint(ListBox ddlAsstID, string Category, string SubCategory, string Location, string Building, string Floor, string Department)
        {
            CreateDB();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getActiveAssetsForPrint, new SqlParameter[] {
                            new SqlParameter("@CategoryId",  Category),
                            new SqlParameter("@SubCatId",  SubCategory),
                            new SqlParameter("@LocationId",  Location),
                            new SqlParameter("@BuildingId",  Building),
                            new SqlParameter("@FloorId",  Floor),
                            new SqlParameter("@DepartmentId",  Department),

                            });
            DataSet ds = help.ExecuteDataset();

            ddlAsstID.DataSource = ds;
            ddlAsstID.DataTextField = "AssetCode";
            ddlAsstID.DataValueField = "AssetId";
            ddlAsstID.DataBind();
        }
        public void BindActiveAssetsForRePrint(ListBox ddlAsstID, string Category, string SubCategory, string Location, string Building, string Floor, string Department)
        {
            CreateDB();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getActiveAssetsForRePrint, new SqlParameter[] {
                            new SqlParameter("@CategoryId",  Category),
                            new SqlParameter("@SubCatId",  SubCategory),
                            new SqlParameter("@LocationId",  Location),
                            new SqlParameter("@BuildingId",  Building),
                            new SqlParameter("@FloorId",  Floor),
                            new SqlParameter("@DepartmentId",  Department),

                            });
            DataSet ds = help.ExecuteDataset();

            ddlAsstID.DataSource = ds;
            ddlAsstID.DataTextField = "AssetCode";
            ddlAsstID.DataValueField = "AssetId";
            ddlAsstID.DataBind();
        }

        public DataSet GetAssetDetailsForRePrint(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Search, string Custodian, string TagType)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForRePrint, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                           new SqlParameter("@AssetId",  Asset),
                           new SqlParameter("@AssetCode",  AssetCode),
                             new SqlParameter("@SearchText",  Search),
                            new SqlParameter("@CustodianId",  Custodian),
                            new SqlParameter("@TagType",  TagType),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public DataSet GetAssetDetailsForRePrintV2(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Search, string Custodian, string TagType, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForRePrintV2, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                           new SqlParameter("@AssetId",  Asset),
                           new SqlParameter("@AssetCode",  AssetCode),
                             new SqlParameter("@SearchText",  Search),
                            new SqlParameter("@CustodianId",  Custodian),
                            new SqlParameter("@TagType",  TagType),
                             new SqlParameter("@UserID",  UserID),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public DataSet GetAssetDetailsForRePrintV2_(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Search, string Custodian, string TagType, string UserID, string Column1FCNumber, string Column2AssigneeName, string Column3ClientName,string Column5CaseManagerName,string column7CaseWorker1)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForRePrintV2_, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                           new SqlParameter("@AssetId",  Asset),
                           new SqlParameter("@AssetCode",  AssetCode),
                             new SqlParameter("@SearchText",  Search),
                            new SqlParameter("@CustodianId",  Custodian),
                            new SqlParameter("@TagType",  TagType),
                             new SqlParameter("@UserID",  UserID),
                              new SqlParameter("@Column1FCNumber",  Column1FCNumber),
                               new SqlParameter("@Column2AssigneeName",  Column2AssigneeName),
                                new SqlParameter("@Column3ClientName",  Column3ClientName),
                                new SqlParameter("@Column5CaseManagerName",Column5CaseManagerName),
                                new SqlParameter("@Column7CaseWorker1Name",column7CaseWorker1),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public void BindActiveAssetsForEncode(ListBox ddlAsstID, string Category, string SubCategory, string Location, string Building, string Floor, string Department)
        {
            CreateDB();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getActiveAssetsForEncode, new SqlParameter[] {
                            new SqlParameter("@CategoryId",  Category),
                            new SqlParameter("@SubCatId",  SubCategory),
                            new SqlParameter("@LocationId",  Location),
                            new SqlParameter("@BuildingId",  Building),
                            new SqlParameter("@FloorId",  Floor),
                            new SqlParameter("@DepartmentId",  Department),

                            });
            DataSet ds = help.ExecuteDataset();

            ddlAsstID.DataSource = ds;
            ddlAsstID.DataTextField = "AssetCode";
            ddlAsstID.DataValueField = "AssetId";
            ddlAsstID.DataBind();
        }

        public DataSet GetAssetDetailsForEncode(string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForEncode, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                          new SqlParameter("@AssetCode",  AssetCode),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public DataSet GetAssetDetailsForTagging(string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForTagging, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryId),
                      new SqlParameter("@SubCatId", SubCatId),
                       new SqlParameter("@LocationId",  LocationId),
                        new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@DepartmentId",  DepartmentId),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public void UpdateEncodeStatusAssetMaster(string output, int UserID, string Type)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "UpdateStatusInAssetMaster", new SqlParameter[] {

                            new SqlParameter("@Assets",output),
                            new SqlParameter("@UserID", UserID),
                            new SqlParameter("@Type",Type ),
                        }
                );
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}