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
/// Summary description for ReportBL
/// </summary>
public class ReportBL
{
    static SqlConnection con = null;
    private static void CreateDB()
    {
        con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    }




    public DataSet GetAssetHistry(string FromDate, string ToDate, string variance, string USERID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetVerificationHistoryv2, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@TYPE",  variance),
                           new SqlParameter("@USERID",  USERID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;

    }

    public DataSet GetAssetVerificationDetailsByID(string VID)
    {
        CreateDB();

        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getAssetVerificationDetailsByID, new SqlParameter[] {
                            new SqlParameter("@VID",  VID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetAssetsToApprove(string RID)
    {
        CreateDB();

        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetAssetsToApprove, new SqlParameter[] {
                            new SqlParameter("@REQUESTID",  RID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetAssetVerificationDetailsByID_Summary(string VID)
    {
        CreateDB();

        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.getAssetVerificationDetailsByID_Summary, new SqlParameter[] {
                            new SqlParameter("@VID",  VID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetPrintingLabelsHistory(string FromDate, string ToDate, string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.usp_GetPrintingLabelsHistory, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetTaggedItems(string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID, string AssetCode, string DeptId, string CustodianID, string SearchText, string FromDate, string ToDate)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.usp_GetTaggedItems, new SqlParameter[] {
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                          new SqlParameter("@DepartmentID",  DeptId),
                          new SqlParameter("@AssetCode",  AssetCode),
                           new SqlParameter("@CustodianId",  CustodianID),
                           new SqlParameter("SearchText",SearchText),
                            new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate)
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetTaggedItemsV2(string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID, string AssetCode, string DeptId, string CustodianID, string SearchText, string UserID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.usp_GetTaggedItemsV2, new SqlParameter[] {
                          new SqlParameter("@LocationId",Convert.ToInt32(LocationId)),
                          new SqlParameter("@BuildingId",  Convert.ToInt32(BuildingId)),
                          new SqlParameter("@FloorId", Convert.ToInt32(FloorId)),
                          new SqlParameter("@CategoryID", Convert.ToInt32(CatID)),
                          new SqlParameter("@SubCategoryID", Convert.ToInt32(SubCatID)),
                          new SqlParameter("@DepartmentID", Convert.ToInt32(DeptId)),
                          new SqlParameter("@AssetCode", AssetCode),
                           new SqlParameter("@CustodianId",  Convert.ToInt32(CustodianID)),
                           new SqlParameter("SearchText",SearchText),
            new SqlParameter("@UserID",  Convert.ToInt32(UserID)),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }


    public DataSet GetUnPrintedLabelHistory(string FromDate, string ToDate, string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.usp_GetUnPrintedLabelHistory, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetEncodedLabels(string FromDate, string ToDate, string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID, string AssetCode, string DepID, string CustodianID, string SearchText)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetEncodedLabels, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                          new SqlParameter("@DepartmentID",  DepID),
                          new SqlParameter("@AssetCode",  AssetCode),
                          new SqlParameter("@CustodianID",CustodianID),
                          new SqlParameter("@SearchText",SearchText),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetEncodedLabelsV2(string FromDate, string ToDate, string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID, string AssetCode, string DepID, string CustodianID, string SearchText, string UserID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetEncodedLabelsV2, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                          new SqlParameter("@DepartmentID",  DepID),
                          new SqlParameter("@AssetCode",  AssetCode),
                          new SqlParameter("@CustodianID",CustodianID),
                          new SqlParameter("@SearchText",SearchText),
                          new SqlParameter("@UserID",UserID),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetEncodedLabelsV2_(string FromDate, string ToDate, string LocationId, string BuildingId, string FloorId, string CatID, string SubCatID, string AssetCode, string DepID, string CustodianID, string SearchText, string UserID, string Column1FCNumber, string Column2AssigneeName, string Column3ClientName,string Column5CaseManager,string column7CaseWorker1)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.GetEncodedLabelsV2_, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@LocationId",  LocationId),
                          new SqlParameter("@BuildingId",  BuildingId),
                          new SqlParameter("@FloorId",  FloorId),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                          new SqlParameter("@DepartmentID",  DepID),
                          new SqlParameter("@AssetCode",  AssetCode),
                          new SqlParameter("@CustodianID",CustodianID),
                          new SqlParameter("@SearchText",SearchText),
                          new SqlParameter("@UserID",UserID),
                           new SqlParameter("@Column1FCNumber",Column1FCNumber),
                            new SqlParameter("@Column2AssigneeName",Column2AssigneeName),
                             new SqlParameter("@Column3ClientName",Column3ClientName),
                             new SqlParameter("@Column5CaseManager",Column5CaseManager),
                            new SqlParameter("@Column7CaseWorker1",column7CaseWorker1),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }

    public DataSet GetWarrentyAMCHistory(string FromDate, string ToDate, string CatID, string SubCatID, string ExpiryDate)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.usp_GetWarrentyAMCHistory, new SqlParameter[] {
                          new SqlParameter("@FromDate",  FromDate),
                          new SqlParameter("@Todate",  ToDate),
                          new SqlParameter("@CategoryID",  CatID),
                          new SqlParameter("@SubCategoryID",  SubCatID),
                          new SqlParameter("@ExpiryDate",  ExpiryDate),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }
    public DataSet GetEmptyLocation(string LocationId, string BuildingId, string FloorId, string SearchText)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.RGetEmtyLocation, new SqlParameter[] {
                          new SqlParameter("@SearchText",  SearchText),
                         new SqlParameter("@LocationId",  LocationId),
                         new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }
    public DataSet GetEmptyLocationV2(string LocationId, string BuildingId, string FloorId, string SearchText, string UserID)
    {
        CreateDB();
        DataAccessHelper1 help = new DataAccessHelper1(
        StoredProcedures.RGetEmtyLocationV2, new SqlParameter[] {
                          new SqlParameter("@SearchText",  SearchText),
                         new SqlParameter("@LocationId",  LocationId),
                         new SqlParameter("@BuildingId",  BuildingId),
                         new SqlParameter("@FloorId",  FloorId),
                         new SqlParameter("@UserID",  UserID),

                            });
        DataSet ds = help.ExecuteDataset();
        return ds;
    }


}