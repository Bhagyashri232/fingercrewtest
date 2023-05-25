using FingerCrew.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallerUploadDocumentController : ControllerBase
    {
        InstallerUploadDocument installer = new InstallerUploadDocument();


        [Route("UploadDocDetails")]
        public IActionResult uploadDocDetails([FromForm] installerDocMDetails param, [FromHeader] string authorization)
        {
            Response response = new Response();
            List<Response> lstResponse = new List<Response>();

            List<string> lstTokenValidation = new List<string>();
            try
            {
                const string apiKeyName = "tokenKey";
                string tokenKey = "";

                if (Request.Headers.TryGetValue(apiKeyName, out var extractedApiKey))
                {
                    tokenKey = extractedApiKey;
                }

                lstTokenValidation = CommonUtilities.checkValidToken(authorization, tokenKey);
                if (lstTokenValidation.Count > 0)
                {
                    if (lstTokenValidation[0].ToLower() == "true" && string.IsNullOrEmpty(lstTokenValidation[1]))
                    {
                        response = installer.uploadDoc(param);
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = lstTokenValidation[1];
                        //lstResponse.Add(response);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Invalid Token";
                    // lstResponse.Add(response);
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "uploadDocDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                //lstResponse.Add(response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("getInstallerDocDetails")]
        public IActionResult getInstallerDocDetails(installerDocDetailParam param, [FromHeader] string authorization)
        {
            installerDocDetailResponse response = new installerDocDetailResponse();
            List<string> lstTokenValidation = new List<string>();
            try
            {
                const string apiKeyName = "tokenKey";
                string tokenKey = "";

                if (Request.Headers.TryGetValue(apiKeyName, out var extractedApiKey))
                {
                    tokenKey = extractedApiKey;
                }

                lstTokenValidation = CommonUtilities.checkValidToken(authorization, tokenKey);
                if (lstTokenValidation.Count > 0)
                {
                    if (lstTokenValidation[0].ToLower() == "true" && string.IsNullOrEmpty(lstTokenValidation[1]))
                    {
                        response = installer.getInstallerDocData(param);
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = lstTokenValidation[1];
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Invalid Token";
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getInstallerDocDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
            }
            return Ok(response);
        }

    }
}
