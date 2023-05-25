using System.Data.SqlClient;
using System.Data;

namespace FingerCrew.Model
{
    public class InstallerRegistration
    {

        DBHelper dBHelper = new DBHelper();
        public Response addInstaller(installerMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "RegisterInstaller"));
                paramList.Add(new SqlParameter("@salutation", param.salutation));
                paramList.Add(new SqlParameter("@installer_name", param.installer_name));
                paramList.Add(new SqlParameter("@installer_company", param.installer_company));
                paramList.Add(new SqlParameter("@installer_email_id", param.installer_email_id));
                paramList.Add(new SqlParameter("@installer_contact_number", param.installer_contact_number));
                 paramList.Add(new SqlParameter("@installer_dob",param.installer_dob)); 
                paramList.Add(new SqlParameter("@installer_active_status", "1"));
                paramList.Add(new SqlParameter("@isactive", "1"));
                paramList.Add(new SqlParameter("@created_by", "1"));
              

                
                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_installer]", paramList.ToArray());
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "addInstaller", ex.Message, ex.StackTrace);
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

    public class installerMDetails
    {
        
        public string? installer_master_id { get; set; }
        public string? salutation { get; set; }
        public string? installer_name { get; set; }
        public string? installer_company { get; set; }
        public string? installer_email_id { get; set; }
        public string? installer_contact_number { get; set; }
        public string? installer_dob { get; set; }
        
    }

    public class installerDetailsParam
    {
        public string? installer_master_id { get; set; }
    }

    public class installerDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayInstallerDetails>? getData { get; set; }
    }

    public class displayInstallerDetails
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
