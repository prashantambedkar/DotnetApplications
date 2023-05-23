using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public enum ProcessingErrors
{
    ProcessedSuccessfully,
    DataBaseExceptionCaught,
    ExceptionCaught,
    MinimumDataNotProvided,

}

public enum DataExecutionMethod
{
    ExecuteDataSet,
    ExecuteReader,
    ExecuteXmlReader,
    ExecuteScalar,
    ExecuteNonQuery,

}

public enum StoredProcedures
{
    RGetEmtyLocation,
    GetEncodedLabelsV2_,
    GetAssetDetailsForRePrintV2_,
    GetVerificationHistoryv2,
    RGetEmtyLocationV2,
    PinsertCategory,
    PupdateCategory,
    ChartPrintVsTaggedV2_,
    pInsertsubcategory,
    Pupdatesubcat,
    PinsertBuilding,
    PupdateBuilding,
    pInsertFloor,
    pupdatefloor,
    Pinsertlocation,
    pupdatelocation,
    pupdatedepartment,
    Pinsertdepartment,
    Pinsertcustodian,
    pupdatecustodian,
    Pinsertsupplier,
    pupdatesupplier,
    PinsertAsset,
    pupdateasset,
    GetBuildingDetails,
    GetBuildingDetailsV2,
    getlocation,
    Psearchbuilding,
    plocationbelongtobuilding,
    Getbuilding,
    PsearchFloor,
    Psearchsubcategory,
    Psearchsubcustodian,
    getcategoryinsubcatasset,
    getfloorforasset,
    getcustodianforasset,
    PinsertUpdateWarrentyOrAMC,
    GetAssetWiseAMCDetails,
    getAssetinWarrenty,
    PinsertUser,
    GetModuleWisePermission,
    PInsertUpdateUserPermission,
    GetAssetDetails,
    PupdateLabelConfigDetails,
    GetAssetDetailsForPrint,
    GetAssetDetailsForPrintV2,
    GetAssetDetailsForRePrint,
    GetAssetDetailsForRePrintV2,
    getActiveAssetsForPrint,
    getActiveAssetsForRePrint,
    getActiveAssetsForEncode,
    GetAssetDetailsForEncode,
    GetAssetDetailsForTagging,
    GetAssetsAccordingToDateandType,
    GetAssetsAccordingToDateandTypeV2_,
    GetAssetsAccordingToDateandTypeV2_1,
    GetAssetsAccordingToDateandTypeV2,
    getSubCategory,
    ValidateUser,
    GetAssetDetailsForStockChecking,
    GetTransferAssetsAccordingToDate,
    getAssetTranferDetailsByTransferID,
    GetVerificationHistory,
    getAssetVerificationDetailsByID,
    getAssetVerificationDetailsByID_Summary,
    usp_GetPrintingLabelsHistory,
    usp_GetUnPrintedLabelHistory,
    usp_GetWarrentyAMCHistory,
    usp_GetTaggedItems,
    usp_GetTaggedItemsV2,
    SP_ASSET_BULKUPDATE,
    AssetVerificationreportSearch,
    GetEncodedLabels,
    GetEncodedLabelsV2,
    getMovementHistory,
    getMovementHistoryV2,
    ChartPrintVsTagged,
    usp_GetAllCategory,
    GetSubCategoryDetails,
    GetLocationDetails,
    GetLocationDetailsV2,
    GetFloorDetails,
    GetFloorDetailsV2,
    GetDepartmentDetails,
    GetCustodianDetails,
    GetCustodianDetailsV2,
    GetSupplierDetails,
    GetAssetsToApprove,
    GetAssetIdentifiedDetails,
    Sp_PrepareDatatoAddAssets,
    GetAssetDetailsForTSBEncode,
    GetAssetDetailsForTSBEncodeV2,
    GetAssetDetailsForTaging,
    GetAssetDetailsForTagingV2,
    GetAssetsAccordingToDateandType_1

}
public enum pages
{
    //CategoryMaster = 1,
    //Subcategory = 2,
    //LocationMaster = 3,
    //BuildingMaster = 4,
    //FloorMaster = 5,
    //DepartmentMaster = 6,
    //CustodianMaster = 7,
    //SupplierMaster = 8,
    //Assetmaster = 9,
    //LabelPrint = 10,
    //LabelRePrint = 11,
    //Transfer = 12,
    //AssetVerificatinReport = 13,
    //AssetTransferReport = 14,
    //AssetMovementReport = 15,
    //EncodedReport = 16,
    //WarrantyReport = 17,
    //TaggedReport = 18


    CategoryMaster = 1,
    CompanyMaster = 2,
    AssetMaster = 3,
    UserManagement = 4,
    LabelPrinting = 5,
    Transfer_Manual = 6,
    Statistics = 7,
    Approval=8,
    AssetIdentification = 9,
    Transfer_TSB=10,
    DocumentRequestt=11,
    ViewDocumentRequestt=12,
    SLA=13,
    PdfReportConfig=14
}