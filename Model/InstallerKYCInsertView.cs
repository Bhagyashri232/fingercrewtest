using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FingerCrew.Model
{
    public class InstallerKYCInsertView
    {
        DBHelper dBHelper = new DBHelper();

        public Response uploadKYC(installerKYCMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            DataTable dtinstaller = new DataTable();
            mobResponse dtResponse = new mobResponse();
            try
            {
                var mobileno = " ";
                var commonpath = "D://FingerCrew/";
                var type = "Installer";
                var doctype = "KYC";

                List<SqlParameter> paramList1 = new List<SqlParameter>();
                paramList1.Add(new SqlParameter("@type", "getMobile"));
                paramList1.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                dtinstaller = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList1.ToArray());

                if (dtinstaller.Rows.Count > 0)
                {
                    if (Convert.ToString(dtinstaller.Rows[0]["Status"]) == "Success")
                    {
                        dtResponse.mobileno = Convert.ToString(dtinstaller.Rows[0]["installer_contact_number"]);
                       // dtResponse.status = Convert.ToString(dtinstaller.Rows[1]["Status"]);
                                       
                    }
                }
                mobileno=dtResponse.mobileno;

                var path =CommonUtilities.createDirectory(commonpath,type,doctype,mobileno);
                string input = param.file.FileName;
                string[] parts = input.Split('.');


                var filename = parts[0];
                var extension = "." + parts[1];

                var finalpath = path + "/" + filename + extension;


                string fileName = Path.GetFileName(param.file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    param.file.CopyTo(stream);
                                       
                }

                List<SqlParameter> paramList = new List<SqlParameter>(); 
                paramList.Add(new SqlParameter("@type", "UploadKYC"));
                paramList.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                paramList.Add(new SqlParameter("@installer_photo_path", finalpath));
                paramList.Add(new SqlParameter("@installer_file_name", filename));
                paramList.Add(new SqlParameter("@installer_file_extension", extension));
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "uploadKYC", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                // lstResponse.Add(response);
            }
            return response;
        }
        public installerKYCDetailResponse getInstallerKYCData(installerKYCDetailParam param)
        {
            installerKYCDetailResponse installerKYCDetailResponse = new installerKYCDetailResponse();
            displayInstallerKYCDetails disInstallerKYCDetails = new displayInstallerKYCDetails();
            List<displayInstallerKYCDetails> lstDisplayData = new List<displayInstallerKYCDetails>();
            DataTable dtInstallerKYCDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetInstallerKYCDetails"));
                paramList.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                dtInstallerKYCDetails = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]",paramList.ToArray());
                if (dtInstallerKYCDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtInstallerKYCDetails.Rows[0]["Status"]) == "Success")
                    {
                        installerKYCDetailResponse.status = Convert.ToString(dtInstallerKYCDetails.Rows[0]["Status"]);

                        foreach (DataRow dr in dtInstallerKYCDetails.Rows)
                        {
                            disInstallerKYCDetails.installer_kyc_id = Convert.ToString(dr["installer_kyc_id"]);
                            disInstallerKYCDetails.installer_master_id = Convert.ToString(dr["insatller_master_id"]);
                            disInstallerKYCDetails.installer_photo_path = Convert.ToString(dr["installer_photo_path"]);
                            disInstallerKYCDetails.installer_file_name = Convert.ToString(dr["installer_file_name"]);
                            disInstallerKYCDetails.installer_file_extension = Convert.ToString(dr["installer_file_extension"]);

                            lstDisplayData.Add(disInstallerKYCDetails);
                            disInstallerKYCDetails = new displayInstallerKYCDetails();
                        }
                        installerKYCDetailResponse.remarks = " ";
                        installerKYCDetailResponse.getData = lstDisplayData;
                    }
                    else
                    {
                        installerKYCDetailResponse.status = Convert.ToString(dtInstallerKYCDetails.Rows[0]["Status"]);
                        installerKYCDetailResponse.remarks = Convert.ToString(dtInstallerKYCDetails.Rows[0]["Remarks"]);
                    }

                    // surveyorKYCDetailResponse.getData = lstDisplayData;
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getInstallerKYCData", ex.Message, ex.StackTrace);
            }

            return installerKYCDetailResponse;
        }

    }
    
    public class installerKYCMDetails
    {
        public string? installer_master_id { get; set; }

        public IFormFile? file { get; set; }
      

    }
    public class mobResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public string? mobileno { get; set; }
    }
    public class installerKYCDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayInstallerKYCDetails>? getData { get; set; }
    }

    public class displayInstallerKYCDetails
    {
        public string? installer_kyc_id { get; set; }
        public string? installer_master_id { get; set; }
        public string? installer_photo_path { get; set; }
        public string? installer_file_name { get; set; }
        public string? installer_file_extension { get; set; }

    }
    public class installerKYCDetailParam
    {
        public string? installer_master_id { get; set; }
    }
}