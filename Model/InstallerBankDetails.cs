using System.Data.SqlClient;
using System.Data;

namespace FingerCrew.Model
{
    public class InstallerBankDetails
    {
        DBHelper dBHelper = new DBHelper();
        public List<Response> addBankDet(bankMDet param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                     

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddBankDetails"));
                paramList.Add(new SqlParameter("@bank_mast_id", param.bank_mast_id));
                paramList.Add(new SqlParameter("@routing_no", param.routing_no));
                paramList.Add(new SqlParameter("@account_no", param.account_no));
                paramList.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                paramList.Add(new SqlParameter("@isactive","1"));


                paramList.Add(new SqlParameter("@created_by", "1"));
                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
                foreach (DataRow dr in dtstatus.Rows)
                {
                    response.status = Convert.ToString(dr["Status"]);
                    response.remarks = Convert.ToString(dr["Remarks"]);
                    lstResponse.Add(response);
                    response = new Response();
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "addbankDet", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                lstResponse.Add(response);
            }
            return lstResponse;
        }

        public bankDetResponse getBankDet(bankDetParam bankDetails)
        {
            bankDetResponse bnkDetailsResponse = new bankDetResponse();
            displayBankDet disBankDetails = new displayBankDet();
            List<displayBankDet> lstDisplayData = new List<displayBankDet>();
            DataTable dtBankDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetBankDetail"));
                paramList.Add(new SqlParameter("@installer_master_id", bankDetails.installer_master_id));
                dtBankDetails = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
                if (dtBankDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtBankDetails.Rows[0]["Status"]) == "Success")
                    {
                        bnkDetailsResponse.status = Convert.ToString(dtBankDetails.Rows[0]["Status"]);

                        foreach (DataRow dr in dtBankDetails.Rows)
                        {
                            disBankDetails.bank_name = Convert.ToString(dr["bank_name"]);
                            disBankDetails.routing_no = Convert.ToString(dr["routing_no"]);
                            disBankDetails.account_no = Convert.ToString(dr["account_no"]);

                            lstDisplayData.Add(disBankDetails);
                            disBankDetails = new displayBankDet();
                        }
                        bnkDetailsResponse.getData = lstDisplayData;

                    }
                    else
                    {
                        bnkDetailsResponse.status = Convert.ToString(dtBankDetails.Rows[0]["Status"]);
                        bnkDetailsResponse.remarks = Convert.ToString(dtBankDetails.Rows[0]["Remarks"]);
                    }

                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getBankData", ex.Message, ex.StackTrace);
            }

            return bnkDetailsResponse;
        }
    }

    //public class Response
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //}

    public class bankMDet
    {
        public string bank_mast_id { get; set; }
        public string routing_no { get; set; }
        public string account_no { get; set; }
        public string installer_master_id { get; set; }

    }

    public class bankDetParam
    {
        public string? installer_master_id { get; set; }
    }

    public class bankDetResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayBankDet>? getData { get; set; }
    }

    public class displayBankDet
    {
        public string bank_name { get; set; }
        public string routing_no { get; set; }
        public string account_no { get; set; }

    }
}
