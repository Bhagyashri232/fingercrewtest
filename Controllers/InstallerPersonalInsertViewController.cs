using FingerCrew.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallerPersonalInsertViewController : ControllerBase
    {
        InstallerPersonalInsertView installer = new InstallerPersonalInsertView();


        [HttpPost]
        [Route("insertInstaller")]
        public IActionResult insertInstallerDetails(installerMDetails param, [FromHeader] string authorization)
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
                        response = installer.insertInstaller(param);
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "insertInstallerDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                //lstResponse.Add(response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("getInstallerPersonalDetails")]
        public IActionResult getInstallerDetails([FromHeader] string authorization)
        {
            installerPerDetailResponse response = new installerPerDetailResponse();
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
                        response = installer.getInstallerPersonalData();
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", " getInstallerDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
            }
            return Ok(response);
        }

    }
}
