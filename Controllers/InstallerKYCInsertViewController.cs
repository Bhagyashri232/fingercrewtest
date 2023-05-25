using FingerCrew.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallerKYCInsertViewController : ControllerBase
    {
        InstallerKYCInsertView installer = new InstallerKYCInsertView();

        [HttpPost]
        [Route("UploadKYCDetails")]
        public IActionResult uploadKYCDetails([FromForm] installerKYCMDetails param, [FromHeader] string authorization)
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
                        response = installer.uploadKYC(param);
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "uploadKYCDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                //lstResponse.Add(response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("getInstallerKYCDetails")]
        public IActionResult getInstallerDetails(installerKYCDetailParam param, [FromHeader] string authorization)
        {
            installerKYCDetailResponse response = new installerKYCDetailResponse();
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
                        response = installer.getInstallerKYCData(param);
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getInstallerDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
            }
            return Ok(response);
        }

    }
}
