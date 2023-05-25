using System.Data.SqlClient;
using System.Data;

namespace FingerCrew.Model
{
    public class InstallerUploadDocument
    {
        DBHelper dBHelper = new DBHelper();

        public Response uploadDoc(installerDocMDetails param)
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
                var doctype = "Documents";

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
                mobileno = dtResponse.mobileno;

                var path = CommonUtilities.createDirectory(commonpath,type,doctype,mobileno);
                var finalpath = path + "/" + param.file;
                string input = param.file.FileName;
                string[] parts = input.Split('.');

                var filename = parts[0];
                var extension = "." + parts[1];
                string fileName = Path.GetFileName(param.file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    param.file.CopyTo(stream);

                }

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "UploadDocument"));
                paramList.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                paramList.Add(new SqlParameter("@document_master_id", param.document_master_id));
                paramList.Add(new SqlParameter("@document_no", param.document_no));
                paramList.Add(new SqlParameter("@document_file_path", finalpath));
                paramList.Add(new SqlParameter("@document_file_name", filename));
                paramList.Add(new SqlParameter("@document_file_extension",extension));
                paramList.Add(new SqlParameter("@created_by", "1")); 
                paramList.Add(new SqlParameter("@isactive", "1"));

                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
                foreach (DataRow dr in dtstatus.Rows)
                {
                    response.status = Convert.ToString(dr["Status"]);
                    response.remarks = Convert.ToString(dr["Remarks"]);
                   
                }

            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "uploadDoc", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
             
            }
            return response;
        }
        public installerDocDetailResponse getInstallerDocData(installerDocDetailParam param)
        {
            installerDocDetailResponse DetailResponse = new installerDocDetailResponse();
            displayInstallerDocDetails disInstallerDetails = new displayInstallerDocDetails();
            List<displayInstallerDocDetails> lstDisplayData = new List<displayInstallerDocDetails>();
            DataTable dtInstallerDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetInstallerDocDetails"));
                paramList.Add(new SqlParameter("@installer_master_id", param.installer_master_id));
                dtInstallerDetails = dBHelper.GetTableFromSP("[dbo].[mob_installer_per]", paramList.ToArray());
                if (dtInstallerDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtInstallerDetails.Rows[0]["Status"]) == "Success")
                    {
                        DetailResponse.status = Convert.ToString(dtInstallerDetails.Rows[0]["Status"]);

                        foreach (DataRow dr in dtInstallerDetails.Rows)
                        {

                            disInstallerDetails.installer_document_mast_id = Convert.ToString(dr["installer_document_mast_id"]);
                            disInstallerDetails.installer_master_id = Convert.ToString(dr["installer_master_id"]);
                            disInstallerDetails.document_master_id = Convert.ToString(dr["document_master_id"]);
                            disInstallerDetails.document_no = Convert.ToString(dr["document_no"]);
                            disInstallerDetails.document_file_path = Convert.ToString(dr["document_file_path"]);
                            disInstallerDetails.document_file_name = Convert.ToString(dr["document_file_name"]);
                            disInstallerDetails.document_file_extension = Convert.ToString(dr["document_file_extension"]);
                            lstDisplayData.Add(disInstallerDetails);
                            disInstallerDetails = new displayInstallerDocDetails();
                        }
                        DetailResponse.remarks = " ";
                        DetailResponse.getData = lstDisplayData;
                    }
                    else
                    {
                        DetailResponse.status = Convert.ToString(dtInstallerDetails.Rows[0]["Status"]);
                        DetailResponse.remarks = Convert.ToString(dtInstallerDetails.Rows[0]["Remarks"]);
                    }

                    // surveyorKYCDetailResponse.getData = lstDisplayData;
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getInstallerDocData", ex.Message, ex.StackTrace);
            }

            return DetailResponse;
        }

    }
    public class installerDocMDetails
    {
        public string? installer_master_id { get; set; }
        public string? document_master_id { get; set; }
        public string? document_no { get; set; }
        public IFormFile? file { get; set; }
          

    }
    //public class mobResponse
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //    public string? mobileno { get; set; }
    //}
    public class installerDocDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displayInstallerDocDetails>? getData { get; set; }
    }

    public class displayInstallerDocDetails
    {
        
        public string? installer_document_mast_id { get; set; }
        public string? installer_master_id { get; set; }
        public string? document_master_id { get; set; }
        public string? document_no { get; set; }
        public string? document_file_path { get; set; }
        public string? document_file_name { get; set; }
        public string? document_file_extension { get; set; }

    }
    public class installerDocDetailParam
    {
        public string? installer_master_id { get; set; }
    }
}