using System.Data.SqlClient;
using System.Data;

namespace FingerCrew.Model
{
    public class VerifyOTPInstaller
    {
        DBHelper dBHelper = new DBHelper();
        public Response verifyOTPInstaller(verifyOTPInstallerMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "OtpVerification"));
                paramList.Add(new SqlParameter("@otp_id", param.otp_id));
                paramList.Add(new SqlParameter("@otp_api", param.otp_api));
                paramList.Add(new SqlParameter("@mobile_no_api", param.mobile_no_api));

                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_login_installer]", paramList.ToArray());

                foreach (DataRow dr in dtstatus.Rows)
                {
                    response.status = Convert.ToString(dr["Status"]);
                    response.remarks = Convert.ToString(dr["Remarks"]);
                    //lstResponse.Add(response);
                    //response = new Response();

                }

            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "verifyOTPInstaller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                // lstResponse.Add(response);
            }
            return response;
        }

    }

    //public class Response
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //}

    public class verifyOTPInstallerMDetails
    {
        public string? otp_id { get; set; }
        public string? otp_api { get; set; }
        public string? mobile_no_api { get; set; }

    }




}