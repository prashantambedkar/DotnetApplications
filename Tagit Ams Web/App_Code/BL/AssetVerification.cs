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
/// Summary description for AssetVerification
/// </summary>
namespace Serco
{

    public class AssetVerification
    {
        static SqlConnection con = null;
        private static void CreateDB()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }
        public int GetIsAssetIdendification()
        {
            CreateDB();
            int ID = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.Text, "Select isnull(IsAssetIdendification,0) from CompanySetting"));
            return ID;
        }
        public int GetMaxVerificationID()
        {
            CreateDB();
            int MaxID = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "GetMaxVerificationID"));
            return MaxID;
        }

        public void SaveAssetVerification(string Asset_Verification_ID, string UserID, string LocationID, string BuildingID, string FloorID)
        {
            CreateDB();
            try
            {
                SqlHelper.ExecuteNonQuery(con, "SaveAssetVerification", new SqlParameter[] { 
           
                            new SqlParameter("@Verification_ID",Asset_Verification_ID),
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),   
                            new SqlParameter("@LocationID", Convert.ToInt32(LocationID)),                           
                            new SqlParameter("@BuildingID", Convert.ToInt32(BuildingID)),                           
                            new SqlParameter("@FloorID", Convert.ToInt32(FloorID)), 
                        }
                );
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SaveAssetVerificationDetails(string Asset_Verification_ID, string UserID, DataTable dt_result, string LocationID, string BuildingID, string FloorID)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int VID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetVerification", new SqlParameter[] { 
           
                            new SqlParameter("@Verification_ID",Asset_Verification_ID),
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)), 
                            new SqlParameter("@LocationID", Convert.ToInt32(LocationID)),                           
                            new SqlParameter("@BuildingID", Convert.ToInt32(BuildingID)),                           
                            new SqlParameter("@FloorID", Convert.ToInt32(FloorID)), 
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetVerificationDetails", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@VID", VID),
                             new SqlParameter("@Status", dr["Status"]),
                             new SqlParameter("@FromLocation", dr["FromLocation"]),
                             new SqlParameter("@ToLocation", dr["Tolocation"]),

                         });
                    }
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }

        }

        public void SaveAssetVerificationDetailsTHS(string Asset_Verification_ID, string UserID, DataTable dt_result, string LocationID, string BuildingID, string FloorID)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int VID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetVerification", new SqlParameter[] { 
           
                            new SqlParameter("@Verification_ID",Asset_Verification_ID),
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),                           
                            new SqlParameter("@LocationID", Convert.ToInt32(LocationID)),                           
                            new SqlParameter("@BuildingID", Convert.ToInt32(BuildingID)),                           
                            new SqlParameter("@FloorID", Convert.ToInt32(FloorID)),                           
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetVerificationDetails", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@VID", VID),
                             new SqlParameter("@Status", dr["Status"]),
                             new SqlParameter("@FromLocation", dr["FromLocation"]),
                             new SqlParameter("@ToLocation", dr["Tolocation"]),
                         });
                    }
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void UpdateMissingData(DataTable dt_result, String user, String Asset_Transfer_ID, string ToDestination)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {

                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID", Convert.ToInt32(user)),                           
                            new SqlParameter("@fromLocation", Convert.ToString("Manual")),                           
                            new SqlParameter("@ToLocation", Convert.ToString(ToDestination)),                        
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),                           
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        if (dr["Status"].ToString() == "Mismatch")
                        {
                            SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                        }
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        if (dr["Status"].ToString() == "Mismatch")
                        {
                            SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateMissingAssets", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@Location", Convert.ToInt32(dr["ToLocID"])),
                             new SqlParameter("@Building", Convert.ToInt32(dr["TobldgID"])),
                             new SqlParameter("@FloorID", Convert.ToInt32(dr["ToFloorID"])),                                                         
                         });
                        }

                    }
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }



        public bool CheckAssetIDsExistInAssetmaster(string AssetIds)
        {
            CreateDB();
            int exist = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "CheckAssetIdsExistInMaster", new SqlParameter[] { 
                        new SqlParameter("@AssetIDs", AssetIds),
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

        public DataSet GetActiveFloor()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveFloor");
            return ds;
        }

        public DataSet GetActiveBuilding()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveBuilding");
            return ds;
        }

        public DataSet GetActiveLocation()
        {
            CreateDB();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getlocation");
            return ds;
        }

        public void SaveAssetTransferDetails(DataTable dt_result, string UserID, string Asset_Transfer_ID, string Reason, string ToDestination)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),                           
                            new SqlParameter("@fromLocation", Convert.ToString(Reason)),                           
                            new SqlParameter("@ToLocation", Convert.ToString(ToDestination)),                        
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),                           
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {

                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateMissingAssets", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@Location", Convert.ToInt32(dr["ToLocationID"])),
                             new SqlParameter("@Building", Convert.ToInt32(dr["TobuildingID"].ToString()==""?"1":dr["TobuildingID"].ToString())),
                             new SqlParameter("@FloorID", Convert.ToInt32(dr["ToFloorID"].ToString()==""?"1":dr["ToFloorID"].ToString())),                                                         
                         });

                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }


        public void SaveAssetTransfer_Custodian(DataTable dt_result, string UserID, string Asset_Transfer_ID, string Reason,
    string ToDestination, string Custodian, string user, string FromCustodian, string ToCustodian)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {

                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetCustodianTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),                           
                            new SqlParameter("@fromLocation", Convert.ToString(Reason)),                           
                            new SqlParameter("@FromCustodian", Convert.ToString(FromCustodian)), 
                            new SqlParameter("@ToCustodian", Convert.ToString(ToCustodian)), 
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),                           
                        }
                                    ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetCustodianTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {

                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateCustodian", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@CustodianID", Convert.ToInt32(Custodian)),
                             new SqlParameter("@CreatedBy", user),                            
                         });

                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void SendAssets_For_Approval(DataTable dt_result, string UserID, string Asset_Transfer_ID, string Reason,
    string ToDestination, string LocId, string Bldid, string FlrID, string user, string Type, string FromCus, string ToCust, string ToCustId,string TransferType,string GPSLocation
            )
        {
            CreateDB();
            con.Open();

            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    string output = "";
                    for (int i = 0; i < dt_result.Rows.Count; i++)
                    {
                        output = output + dt_result.Rows[i]["ASSETCODE"].ToString();
                        output += (i < dt_result.Rows.Count) ? "," : string.Empty;
                    }


                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SP_SENTREQUEST", new SqlParameter[] {
                             new SqlParameter("@STOCK_CODE", output),
                             new SqlParameter("@TO_LOC", ToDestination),
                             new SqlParameter("@FROM_CUSTODIAN", FromCus),
                             new SqlParameter("@TO_CUSTODIAN", ToCust),
                              new SqlParameter("@REMARKS", Reason),
                              new SqlParameter("@USERNAME", UserID),
                              new SqlParameter("@TYPE", Type),
                              new SqlParameter("@LOCID", LocId),
                              new SqlParameter("@BLDID", Bldid),
                              new SqlParameter("@FLRID", FlrID),
                              new SqlParameter("@CUSTID", ToCustId),
                              new SqlParameter("@transferType", TransferType),
                              new SqlParameter("@Asset_Transfer_ID",Asset_Transfer_ID),
                              new SqlParameter("@GPS_Location",GPSLocation)

                        });

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveOrReject(string ID, string RequestStatus, string user, string Type)
        {
            CreateDB();
            con.Open();

            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    //DataTable dt_Result = GetAssetsToApprove(ID);
                    DataSet ds = SqlHelper.ExecuteDataset(Trans, CommandType.StoredProcedure, "GetAssetsToApprove", new SqlParameter[] {
                new SqlParameter("@REQUESTID", ID),  
            }
                        );
                    DataTable dt_Result = ds.Tables[0];

                    if (dt_Result.Rows.Count > 0)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SP_APPROVE_OR_REJECT", new SqlParameter[] {
                             new SqlParameter("@REQUESTID", ID),
                             new SqlParameter("@STATUS", RequestStatus),
                             new SqlParameter("@USERNAME", user),
                        });

                        if (RequestStatus == "Approved")
                        {
                            if (Type.Contains("Location"))
                            {

                                int MaxID = GetMaxTransferID("location");
                                String Asset_Transfer_ID = "";
                                if (MaxID == 0)
                                {
                                    //9 ZEROS
                                    Asset_Transfer_ID = "T000000001";
                                }
                                else
                                {
                                    //9 ZEROS
                                    var res = MaxID + 1;
                                    Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#000000000");
                                }
                                //ApproveAsset_LocationTransferDetails(dt_Result, Asset_Transfer_ID);

                                int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetTransfer", new SqlParameter[] {

                            new SqlParameter("@UserID",dt_Result.Rows[0]["Request_by"]),
                            new SqlParameter("@Reason", dt_Result.Rows[0]["Remarks"]),
                            new SqlParameter("@ToLocation", dt_Result.Rows[0]["To_Location"]),
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),
                            new SqlParameter("@trans_type",dt_Result.Rows[0]["TRANSFER_TYPE"]),
                            new SqlParameter("@GPS_location",dt_Result.Rows[0]["GPS_Location"])
                        }
                            ));

                                foreach (DataRow dr in dt_Result.Rows)
                                {
                                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                                }

                                foreach (DataRow dr in dt_Result.Rows)
                                {

                                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateMissingAssets", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@Location", dr["LocId"].ToString()),
                             new SqlParameter("@Building", Convert.ToInt32(dr["BldId"].ToString()==""?"1":dr["BldId"].ToString())),
                             new SqlParameter("@FloorID", Convert.ToInt32(dr["FlrId"].ToString()==""?"1":dr["FlrId"].ToString())),
                             new SqlParameter("@CreatedBy",  dr["Request_by"].ToString()),
                             new SqlParameter("@Reason",dr["Remarks"].ToString()),
                         });

                                }



                            }
                            else
                            {
                                //ApproveAsset_CustodianTransferDetails(dt_Result, Asset_Transfer_ID);

                                int MaxID = GetMaxTransferID("custodian");
                                String Asset_Transfer_ID = "";
                                if (MaxID == 0)
                                {
                                    Asset_Transfer_ID = "T0000000001";
                                }
                                else
                                {
                                    var res = MaxID + 1;
                                    Asset_Transfer_ID = "T" + Convert.ToInt32(res).ToString("#000000000");
                                }

                                int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetCustodianTransfer", new SqlParameter[] {

                            new SqlParameter("@UserID", dt_Result.Rows[0]["Request_by"]),
                            new SqlParameter("@Reason", dt_Result.Rows[0]["Remarks"]),
                            new SqlParameter("@FromCustodian", dt_Result.Rows[0]["From_Custodoan"]),
                            new SqlParameter("@ToCustodian", dt_Result.Rows[0]["To_Custodoan"]),
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),
                            new SqlParameter("@trans_type",dt_Result.Rows[0]["TRANSFER_TYPE"]),
                            new SqlParameter("@GPS_location",dt_Result.Rows[0]["GPS_Location"])


                        }
                                                ));

                                foreach (DataRow dr in dt_Result.Rows)
                                {
                                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetCustodianTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                                }

                                foreach (DataRow dr in dt_Result.Rows)
                                {

                                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateCustodian", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@CustodianID", dr["CustId"].ToString()),
                             new SqlParameter("@CreatedBy", dr["Request_by"].ToString()),
                              new SqlParameter("@Reason", dr["Remarks"].ToString()),
                         });

                                }
                            }
                        }
                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveAsset_LocationTransferDetails(DataTable dt_result, string Asset_Transfer_ID
            )
        {
            CreateDB();
            con.Open();

            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID",dt_result.Rows[0]["Request_by"]),                           
                            new SqlParameter("@Reason", dt_result.Rows[0]["Remarks"]),                           
                            new SqlParameter("@ToLocation", dt_result.Rows[0]["To_Location"]),                        
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),                           
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {

                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateMissingAssets", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@Location", dr["LocId"].ToString()),
                             new SqlParameter("@Building", Convert.ToInt32(dr["BldId"].ToString()==""?"1":dr["BldId"].ToString())),
                             new SqlParameter("@FloorID", Convert.ToInt32(dr["FlrId"].ToString()==""?"1":dr["FlrId"].ToString())),
                             new SqlParameter("@CreatedBy",  dr["Request_by"].ToString()),                            
                         });

                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void ApproveAsset_CustodianTransferDetails(DataTable dt_result, string Asset_Transfer_ID)
        {
            CreateDB();
            con.Open();
            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {

                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetCustodianTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID", dt_result.Rows[0]["Request_by"]),                           
                            new SqlParameter("@fromLocation", dt_result.Rows[0]["Remarks"]),                           
                            new SqlParameter("@FromCustodian", dt_result.Rows[0]["From_Custodian"]), 
                            new SqlParameter("@ToCustodian", dt_result.Rows[0]["To_Custodian"]), 
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),  
                         
 
                        }
                                    ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetCustodianTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {

                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateCustodian", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@CustodianID", dr["CustId"].ToString()),
                             new SqlParameter("@CreatedBy", dr["Request_by"].ToString()),                            
                         });

                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public void SaveAssetTransferDetails_Manual(DataTable dt_result, string UserID, string Asset_Transfer_ID, string Reason,
            string ToDestination, string Loc, string Build, string Floor, string user)
        {
            CreateDB();
            con.Open();

            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {
                    int TransferID = Convert.ToInt32(SqlHelper.ExecuteScalar(Trans, "SaveAssetTransfer", new SqlParameter[] {            
                            
                            new SqlParameter("@UserID", Convert.ToInt32(UserID)),                           
                            new SqlParameter("@Reason", Convert.ToString(Reason)),                           
                            new SqlParameter("@ToLocation", Convert.ToString(ToDestination)),                        
                            new SqlParameter("@TransfrCode", Convert.ToString(Asset_Transfer_ID)),                           
                        }
                ));

                    foreach (DataRow dr in dt_result.Rows)
                    {
                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "SaveAssetTransferDetails", new SqlParameter[] {
                             new SqlParameter("@AssetID", Convert.ToInt32(dr["AssetID"])),
                             new SqlParameter("@AssetCode", Convert.ToString(dr["AssetCode"])),
                             new SqlParameter("@TransferID", TransferID),
                        });
                    }

                    foreach (DataRow dr in dt_result.Rows)
                    {

                        SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "UpdateMissingAssets", new SqlParameter[] {
                             new SqlParameter("@AssetCode", dr["AssetCode"].ToString()),
                             new SqlParameter("@Location", Convert.ToInt32(Loc)),
                             new SqlParameter("@Building", Convert.ToInt32(Build.ToString()==""?"1":Build.ToString())),
                             new SqlParameter("@FloorID", Convert.ToInt32(Floor.ToString()==""?"1":Floor.ToString())),
                             new SqlParameter("@CreatedBy", user),                            
                         });

                    }

                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }

        public int GetMaxTransferID(string Type)
        {
            CreateDB();
            int MaxID = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "GetMaxTransferID", new SqlParameter[] {
                new SqlParameter("@Type", Type),  
            }
                ));
            return MaxID;
        }
       
        public DataSet GetAssetTransferDeatilsByTransferID(string TransferID, string Type)
        {
            CreateDB();

            DataAccessHelper1 help = new DataAccessHelper1(
            StoredProcedures.getAssetTranferDetailsByTransferID, new SqlParameter[] { 
                            new SqlParameter("@TransferID",  TransferID),  
                            new SqlParameter("@Type",  Type),                           
                            });
            DataSet ds = help.ExecuteDataset();
            return ds;
        }

        public DataSet GetActiveSubCategory()
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetActiveSubCat");
            con.Close();
            return ds;
        }

        public DataSet GetActiveCategory()
        {
            con.Open();
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "getCategory");
            con.Close();
            return ds;
        }

        public DataTable GetAssetsToApprove(string ID)
        {
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GetAssetsToApprove", new SqlParameter[] {
                new SqlParameter("@REQUESTID", ID),  
            }
                );
            return ds.Tables[0];
        }

        public bool ApproveOrRejectIdentifiedAssets(string ID)
        {
            CreateDB();
            con.Open();

            using (SqlTransaction Trans = con.BeginTransaction())
            {
                try
                {

                    SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "DeleteIdentifiedAsset", new SqlParameter[] {
                             new SqlParameter("@ID", ID),
                        });
                   
                    Trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    return false;                    
                }
            }
        }
        //added by ponraj
        public int GetCustordianID(string Code)
        {
            CreateDB();
            int ID = Convert.ToInt32(SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "GetCustordianID", new SqlParameter[] {
                new SqlParameter("@Code", Code),
            }
                ));
            return ID;
        }
        public int GetIsApprovalNeeded()
        {
            CreateDB();
            int ID = Convert.ToInt32(SqlHelper.ExecuteScalar(con,CommandType.Text, "Select isnull(IsApprovalNeeded,0) from CompanySetting"));
            return ID;
        }
    }
}