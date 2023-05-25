using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace FingerCrew.Model
{
    public class BankMaster
    {
        DBHelper dBHelper = new DBHelper();
        public List<Response> addBankMaster(bankMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddBankM"));
                paramList.Add(new SqlParameter("@bankName", param.bankname));
                paramList.Add(new SqlParameter("@bankbranchcode", param.branchcode));
                paramList.Add(new SqlParameter("@ifsccode", param.ifsccode));
                paramList.Add(new SqlParameter("@insertedby", "1"));
                dtstatus = dBHelper.GetTableFromSP("SP_BankMaster", paramList.ToArray());
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "addbankMaster", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                lstResponse.Add(response);
            }
            return lstResponse;
        }

        public bankDetailResponse getBankData(bankDetailsParam bankDetails)
        {
            bankDetailResponse bnkDetailsResponse = new bankDetailResponse();
            displayBankDetails disBankDetails = new displayBankDetails();
            List<displayBankDetails> lstDisplayData = new List<displayBankDetails>();
            DataTable dtBankDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetBankDetails"));
                paramList.Add(new SqlParameter("@bankId", bankDetails.bankId));
                dtBankDetails = dBHelper.GetTableFromSP("SP_BankMaster", paramList.ToArray());
                if (dtBankDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtBankDetails.Rows[0]["Status"]) == "Success")
                    {
                        bnkDetailsResponse.status = Convert.ToString(dtBankDetails.Rows[0]["Status"]);

                        foreach (DataRow dr in dtBankDetails.Rows)
                        {
                            disBankDetails.bankname = Convert.ToString(dr["bank_name"]);
                            disBankDetails.branchcode = Convert.ToString(dr["bank_name"]);
                            disBankDetails.ifsccode = Convert.ToString(dr["ifsc_code"]);
                            lstDisplayData.Add(disBankDetails);
                            disBankDetails = new displayBankDetails();
                        }
                    }
                    else
                    {
                        bnkDetailsResponse.status = Convert.ToString(dtBankDetails.Rows[0]["Status"]);
                        bnkDetailsResponse.remarks = Convert.ToString(dtBankDetails.Rows[0]["Remarks"]);
                    }

                    bnkDetailsResponse.getData = lstDisplayData;
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getBankData", ex.Message, ex.StackTrace);
            }

            return bnkDetailsResponse;
        }
    }

    public class Response
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
    }

    public class bankMDetails
    {
        public string bankname { get; set; }
        public string branchcode { get; set; }
        public string ifsccode { get; set; }
        public string userid { get; set; }
    }

    public class bankDetailsParam
    {
        public string? bankId { get; set; }
    }

    public class bankDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayBankDetails>? getData { get; set; }
    }

    public class displayBankDetails
    {
        public string? bankname { get; set; }
        public string? branchcode { get; set; }
        public string? ifsccode { get; set; }
    }
}
