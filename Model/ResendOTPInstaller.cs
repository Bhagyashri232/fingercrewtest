using Mob_Surveyor_Login_API.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace FingerCrew.Model
{
    public class ResendOTPInstaller
    {
        DBHelper dBHelper = new DBHelper();
        public OResponse resendInstallerOTP(resendOTPInstallerMDetails param)
        {
            OResponse oresponse = new OResponse();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "InstallerLogin"));
                paramList.Add(new SqlParameter("@installer_contact_number", param.installer_contact_number));

                var mob = param.installer_contact_number;
                string pattern = @"^(\+?\d{1,3}[-. ]?)?\(?\d{3}\)?[-. ]?\d{3}[-. ]?\d{4}$";
                if (Regex.IsMatch(mob, pattern))
                {
                    dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_login_installer]", paramList.ToArray());

                    foreach (DataRow dr in dtstatus.Rows)
                    {
                        oresponse.status = Convert.ToString(dr["Status"]);
                        oresponse.remarks = Convert.ToString(dr["Remarks"]);
                        //lstResponse.Add(response);
                        //response = new Response();

                    }

                    if (oresponse.status == "Success")
                    {

                        String otp = Generate_otp();
                        List<SqlParameter> paramList2 = new List<SqlParameter>();
                        paramList2.Add(new SqlParameter("@type", "Otp"));
                        paramList2.Add(new SqlParameter("@otp", otp));
                        paramList2.Add(new SqlParameter("@installer_contact_number", param.installer_contact_number));

                        dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_login_installer]", paramList2.ToArray());
                        foreach (DataRow dr in dtstatus.Rows)
                        {
                            oresponse.status = Convert.ToString(dr["Status"]);
                            oresponse.remarks = Convert.ToString(dr["Remarks"]);
                            oresponse.otpid = Convert.ToString(dr["OTPId"]);
                            //lstResponse.Add(response);
                            //response = new Response();

                        }
                    }
                }
                else
                {
                    oresponse.status = "Failed";
                    oresponse.remarks = "Invalid Mobile Number Format";
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "resendInstallerOTP", ex.Message, ex.StackTrace);
                oresponse.status = "Failed";
                oresponse.remarks = "Something went wrong !";
                // lstResponse.Add(response);
            }
            return oresponse;
        }
        protected string Generate_otp()
        {
            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 4; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                else i--;
            }
            return strrandom;
        }
    }

    //public class Response
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //}
    //public class OResponse
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //    public int? otpid { get; set; }
    //}
    public class resendOTPInstallerMDetails
    {
        public string? installer_contact_number { get; set; }


    }

}
