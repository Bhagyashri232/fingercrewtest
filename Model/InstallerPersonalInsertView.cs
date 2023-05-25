using System.Data.SqlClient;
using System.Data;

namespace FingerCrew.Model
{
    public class InstallerPersonalInsertView
    {

        DBHelper dBHelper = new DBHelper();
        public Response insertInstaller(installerMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {


                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "InsertInstallerPersonalDetails"));
                paramList.Add(new SqlParameter("@salutation", param.salutation));
                paramList.Add(new SqlParameter("@installer_name", param.installer_name));
                paramList.Add(new SqlParameter("@installer_company", param.installer_company));
                paramList.Add(new SqlParameter("@installer_email_id", param.installer_email_id));
                paramList.Add(new SqlParameter("@installer_contact_number", param.installer_contact_number));
                paramList.Add(new SqlParameter("@installer_dob",param.installer_dob));
                paramList.Add(new SqlParameter("@installer_active_status", "1"));
                paramList.Add(new SqlParameter("@isactive", "1"));
                paramList.Add(new SqlParameter("@created_by", "1"));



                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "insertInstaller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                // lstResponse.Add(response);
            }
            return response;
        }
        public installerPerDetailResponse getInstallerPersonalData()
        {
            installerPerDetailResponse installerDetailsPersonalResponse = new installerPerDetailResponse();
            displayPerInstallerDetails disInstallerDetails = new displayPerInstallerDetails();
            List<displayPerInstallerDetails> lstDisplayData = new List<displayPerInstallerDetails>();
            DataTable dtInstallerDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetInstallerPersonalDetails"));
                //paramList.Add(new SqlParameter("@surveyor_master_id", surveyorDetails.surveyor_master_id));

                dtInstallerDetails = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
                if (dtInstallerDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtInstallerDetails.Rows[0]["Status"]) == "Success")
                    {
                        installerDetailsPersonalResponse.status = Convert.ToString(dtInstallerDetails.Rows[0]["Status"]);

                        foreach (DataRow dr in dtInstallerDetails.Rows)
                        {

                            disInstallerDetails.installer_master_id = Convert.ToString(dr["installer_master_id"]);
                            disInstallerDetails.salutation = Convert.ToString(dr["salutation"]);
                            disInstallerDetails.installer_name = Convert.ToString(dr["installer_name"]);
                            disInstallerDetails.installer_company = Convert.ToString(dr["installer_company"]);
                            disInstallerDetails.installer_email_id = Convert.ToString(dr["installer_email_id"]);
                            disInstallerDetails.installer_dob = Convert.ToString(dr["installer_dob"]);
                            disInstallerDetails.installer_contact_number = Convert.ToString(dr["installer_contact_number"]);


                            lstDisplayData.Add(disInstallerDetails);
                            disInstallerDetails = new displayPerInstallerDetails();

                        }
                        installerDetailsPersonalResponse.remarks = " ";
                        installerDetailsPersonalResponse.getData = lstDisplayData;



                    }
                    else
                    {
                        installerDetailsPersonalResponse.status = Convert.ToString(dtInstallerDetails.Rows[0]["Status"]);
                        installerDetailsPersonalResponse.remarks = Convert.ToString(dtInstallerDetails.Rows[0]["Remarks"]);
                    }


                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getInstallerPersonalData", ex.Message, ex.StackTrace);
            }

            return installerDetailsPersonalResponse;
        }


    }

    //public class Response
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //}

    public class installerPerMDetails
    {
        public string? installer_master_id { get; set; }
        public string? salutation { get; set; }
        public string? installer_name { get; set; }
        public string? installer_company { get; set; }
        public string? installer_email_id { get; set; }
        public string? installer_contact_number { get; set; }
        public string? installer_dob { get; set; }

    }

    public class installerPerDetailsParam
    {
        public string? installer_master_id { get; set; }
    }

    public class installerPerDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayPerInstallerDetails>? getData { get; set; }
    }

    public class displayPerInstallerDetails
    {
        public string? installer_master_id { get; set; }
        public string? salutation { get; set; }
        public string? installer_name { get; set; }
        public string? installer_company { get; set; }
        public string? installer_email_id { get; set; }
        public string? installer_contact_number { get; set; }
        public string? installer_dob { get; set; }


    }
}
