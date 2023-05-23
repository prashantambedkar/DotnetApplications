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

namespace ECommerce.Common
{
    /// <summary>
    /// Contains common functionalities required in Virtual Cart
    /// </summary>
    public class Common
    {
        static SqlConnection con = null;

        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public static void Bindcategory(DropDownList ddlproCategory)
        {
            CreateDB();

            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");

            ddlproCategory.DataSource = ds;
            ddlproCategory.DataTextField = "CategoryName";
            ddlproCategory.DataValueField = "CategoryId";
            ddlproCategory.DataBind();
            ddlproCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        public static void BindLocation(DropDownList ddlloc, string userid)
        {
            CreateDB();
            DataSet ds = new DataSet();
           // DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            using (SqlCommand cmd = new SqlCommand("select lm.* from LocationMaster as lm left join LocationPermission as lp on lp.LocationID=lm.LocationId where lp.UserID=" + userid + " and Active = 1 order by LocationName asc", con))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.Fill(ds);
                }
            }
            ddlloc.DataSource = ds;
            ddlloc.DataTextField = "LocationName";
            ddlloc.DataValueField = "LocationId";
            ddlloc.DataBind();
            ddlloc.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        public static void BindDepartMent(DropDownList ddldept)
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getDepartment");

            ddldept.DataSource = ds;
            ddldept.DataTextField = "DepartmentName";
            ddldept.DataValueField = "DepartmentId";
            ddldept.DataBind();
        }

        public static void bindSubCategory(DropDownList ddlsubcat, string CategoryValue)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getcategoryinsubcatasset, new SqlParameter[] {
                      new SqlParameter("@CategoryId",  CategoryValue),

                    });
            DataSet ds = help.ExecuteDataset();
            ddlsubcat.DataSource = ds;
            ddlsubcat.DataTextField = "SubCatName";
            ddlsubcat.DataValueField = "SubCatId";
            ddlsubcat.DataBind();
            ////////ddlsubcat.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        public static void bindBuilding(DropDownList ddlbuild, string LocationValue)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.Getbuilding, new SqlParameter[] {
                      new SqlParameter("@LocationId",  LocationValue),

                    });
            DataSet ds = help.ExecuteDataset();
            ddlbuild.DataSource = ds;
            ddlbuild.DataTextField = "BuildingName";
            ddlbuild.DataValueField = "BuildingId";
            ddlbuild.DataBind();
            ddlbuild.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

        public static void BindFloor(DropDownList ddlfloor, string BuldingValue)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getfloorforasset, new SqlParameter[] {
                      new SqlParameter("@BuildingId",  BuldingValue),

                    });
            DataSet ds = help.ExecuteDataset();
            ddlfloor.DataSource = ds;
            ddlfloor.DataTextField = "FloorName";
            ddlfloor.DataValueField = "FloorId";
            ddlfloor.DataBind();
            ddlfloor.Items.Insert(0, new ListItem("--Select--", "0", true));
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

        public static DataSet GetAssetDetailsForPrint(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search, string TagType)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForPrint, new SqlParameter[] {
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
        public static DataSet GetAssetDetailsForPrintV2(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search, string TagType, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForPrintV2, new SqlParameter[] {
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
        public static DataSet GetAssetDetailsForTSBEncode(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForTSBEncode, new SqlParameter[] {
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

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetAssetDetailsForTSBEncodeV2(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForTSBEncodeV2, new SqlParameter[] {
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
                             new SqlParameter("@UserID",  UserID),

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetAssetDetailsForTaging(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForTaging, new SqlParameter[] {
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

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetAssetDetailsForTagingV2(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId, string AssetCode, string Custodian, string Search, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetailsForTagingV2, new SqlParameter[] {
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
                             new SqlParameter("@UserID",  UserID),

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetIdentifiedAssets(string TranId)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.Sp_PrepareDatatoAddAssets, new SqlParameter[] {
                 new SqlParameter("@TranId",  TranId),
                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }


        public static DataSet GetIdentifiedAssetDetails(string TranId, string Asset, string Category, string SubCat, string Location, string Building, string Floor, string Department, string AssetCode, string Custodian, string Search, string TagType)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetIdentifiedDetails, new SqlParameter[] {
                 new SqlParameter("@TranId",  TranId),
                      new SqlParameter("@Category",  Category),
                      new SqlParameter("@SubCat", SubCat),
                       new SqlParameter("@Location",  Location),
                        new SqlParameter("@Building",  Building),
                         new SqlParameter("@Floor",  Floor),
                          new SqlParameter("@Department",  Department),
                           new SqlParameter("@Asset",  Asset),
                           new SqlParameter("@AssetCode",  AssetCode),
                           new SqlParameter("@SearchText",  Search),
                            new SqlParameter("@CustodianId",  Custodian),
                            new SqlParameter("@TagType",  TagType),

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetAllActiveAssetsDetails(string Asset, string CategoryId, string SubCatId, string LocationId, string BuildingId, string FloorId, string DepartmentId)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetAssetDetails, new SqlParameter[] {
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

        public static DataTable GetConfigDetails()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getLabelConfigDetails");
            return ds.Tables[0];
        }

        public static DataTable GetLabelConfigDetails(string TagType)
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getLabelConfigDetails", new SqlParameter[] {
                new SqlParameter("@TagType", Convert.ToString(TagType))
                });
            return ds.Tables[0];
        }

        public static bool ValidateUser(int PageID, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.ValidateUser, new SqlParameter[] {
                      new SqlParameter("@PageID",  PageID),
                      new SqlParameter("@UserID", UserID),
                    });
            int result = Convert.ToInt32(help.ExecuteScalar());
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static DataSet GetAllCategoryDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.usp_GetAllCategory, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetAllSubCategoryDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetSubCategoryDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetAllLocationDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetLocationDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetAllLocationDetailsV2(string Search, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetLocationDetailsV2, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),
                           new SqlParameter("@UserID",  UserID),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetFloorDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetFloorDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }


        public static DataSet GetFloorDetailsV2(string Search, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetFloorDetailsV2, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),
                           new SqlParameter("@UserID",  UserID),

                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetDepartmentDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetDepartmentDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetCustodianDetails(string Search, string dept, string custid, string custname, string design, string phone, string mail, string encoded)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetCustodianDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),
                                   //Added By Ponraj
                            new SqlParameter("@DepartmentId",  dept),
                             new SqlParameter("@CustodianName",  custname),
                              new SqlParameter("@Encoded",  encoded),
                               new SqlParameter("@CustodianCode",  custid),
                               new SqlParameter("@Designation",  design),
                                new SqlParameter("@PhoneNo",  phone),
                                 new SqlParameter("@EmailID",  mail),




                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetCustodianDetailsV2(string Search, string dept, string custid, string custname, string design, string phone, string mail, string encoded, string UserID)
        {
            try
            {
                CreateDB();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("GetCustodianDetailsV2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchText", Search);
                    cmd.Parameters.AddWithValue("@DepartmentId", dept);
                    cmd.Parameters.AddWithValue("@CustodianName", custname);
                    cmd.Parameters.AddWithValue("@Encoded", encoded);
                    cmd.Parameters.AddWithValue("@CustodianCode", custid);
                    cmd.Parameters.AddWithValue("@Designation", design);
                    cmd.Parameters.AddWithValue("@PhoneNo", phone);
                    cmd.Parameters.AddWithValue("@EmailID", mail);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
                // DataSet ds = help.ExecuteDataset();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataSet GetSupplierDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetSupplierDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public static DataSet GetAllBuildingDetails(string Search)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetBuildingDetails, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
        public static DataSet GetAllBuildingDetailsV2(string Search, string UserID)
        {
            CreateDB();
            DataAccessHelper1 help = new DataAccessHelper1(
             StoredProcedures.GetBuildingDetailsV2, new SqlParameter[] {

                           new SqlParameter("@SearchText",  Search),
                 new SqlParameter("@UserID",  UserID),


                    });

            DataSet ds = help.ExecuteDataset();
            return ds;
        }
    }
}
