using FingerCrew.Model;
using System.Data;
using System.Data.SqlClient;

namespace FingerCrew.Model
{
    public class SurveyorMaster
    {
        DBHelper dBHelper = new DBHelper();
        public Response addSurveyorMaster(surveyorMDetails param)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();
            DataTable dtstatus = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "InsertSurveyorPersonalDetails"));
                paramList.Add(new SqlParameter("@salutation", param.salutation));
                paramList.Add(new SqlParameter("@surveyor_name", param.surveyor_name));
                paramList.Add(new SqlParameter("@dob",param.dob));
                paramList.Add(new SqlParameter("@email_id", param.email_id));
                paramList.Add(new SqlParameter("@mobile_no", param.mobile_no));
                paramList.Add(new SqlParameter("@alt_mobile_no", param.alt_mobile_no));
                paramList.Add(new SqlParameter("@is_verified", "1"));
                paramList.Add(new SqlParameter("@is_approved", "1"));
                paramList.Add(new SqlParameter("@rejection_remarks", "No"));
                paramList.Add(new SqlParameter("@surveyor_active_status", "1"));
                paramList.Add(new SqlParameter("@intimation_sent", "1"));

                paramList.Add(new SqlParameter("@createby", "1"));
                dtstatus = dBHelper.GetTableFromSP("[dbo].[mob_surveyor]", paramList.ToArray());
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "addSurveyorMaster", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                // lstResponse.Add(response);
            }
            return response;
        }

        public surveyorDetailResponse getSurveyorData(surveyorDetailsParam surveyorDetails)
        {
            surveyorDetailResponse surveyorDetailsResponse = new surveyorDetailResponse();
            displaySurveyorDetails disSurveyorDetails = new displaySurveyorDetails();
            List<displaySurveyorDetails> lstDisplayData = new List<displaySurveyorDetails>();
            DataTable dtSurveyorDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetSurveyorDetails"));
                paramList.Add(new SqlParameter("@surveyor_master_id", surveyorDetails.surveyor_master_id));

                if (Convert.ToInt32(surveyorDetails.surveyor_master_id) <= 0)
                {
                    surveyorDetailsResponse.status = "Failed";
                    surveyorDetailsResponse.remarks = "Invalid Surveyor master id";
                }
                else
                {
                    dtSurveyorDetails = dBHelper.GetTableFromSP("[dbo].[mob_surveyor]", paramList.ToArray());
                    if (dtSurveyorDetails.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtSurveyorDetails.Rows[0]["Status"]) == "Success")
                        {
                            surveyorDetailsResponse.status = Convert.ToString(dtSurveyorDetails.Rows[0]["Status"]);

                            foreach (DataRow dr in dtSurveyorDetails.Rows)
                            {
                                disSurveyorDetails.surveyor_master_id = Convert.ToString(dr["surveyor_master_id"]);
                                disSurveyorDetails.salutation = Convert.ToString(dr["salutation"]);
                                disSurveyorDetails.surveyor_name = Convert.ToString(dr["surveyor_name"]);
                                disSurveyorDetails.dob = Convert.ToString(dr["dob"]);
                                disSurveyorDetails.email_id = Convert.ToString(dr["email_id"]);
                                disSurveyorDetails.mobile_no = Convert.ToString(dr["mobile_no"]);
                                disSurveyorDetails.alt_mobile_no = Convert.ToString(dr["alt_mobile_no"]);

                                lstDisplayData.Add(disSurveyorDetails);
                                disSurveyorDetails = new displaySurveyorDetails();

                            }
                            surveyorDetailsResponse.remarks = " ";
                            surveyorDetailsResponse.getData = lstDisplayData;

                        }
                        else
                        {
                            surveyorDetailsResponse.status = Convert.ToString(dtSurveyorDetails.Rows[0]["Status"]);
                            surveyorDetailsResponse.remarks = Convert.ToString(dtSurveyorDetails.Rows[0]["Remarks"]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getSurveyorData", ex.Message, ex.StackTrace);
            }

            return surveyorDetailsResponse;
        }
    }

    //public class Response
    //{
    //    public string? status { get; set; }
    //    public string? remarks { get; set; }
    //}

    public class surveyorMDetails
    {

        public string? surveyor_master_id { get; set; }
        public string? salutation { get; set; }
        public string? surveyor_name { get; set; }
        public string? dob { get; set; }
        public string? email_id { get; set; }
        public string? mobile_no { get; set; }

        public string? alt_mobile_no { get; set; }
    }

    public class surveyorDetailsParam
    {
        public string? surveyor_master_id { get; set; }
    }

    public class surveyorDetailResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public List<displaySurveyorDetails>? getData { get; set; }
    }

    public class displaySurveyorDetails
    {
        public string? surveyor_master_id { get; set; }
        public string? salutation { get; set; }
        public string? surveyor_name { get; set; }
        public string? dob { get; set; }
        public string? email_id { get; set; }

        public string? mobile_no { get; set; }

        public string? alt_mobile_no { get; set; }

    }
}
