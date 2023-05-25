using FingerCrew.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mob_Surveyor_Login_API.Model;
using Response = FingerCrew.Model.Response;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResendOTPController : ControllerBase
    {
        ResendOTP ResendOTP = new ResendOTP();
        [HttpPost]
        [Route("resendOTP")]
        public IActionResult resendOTP(resendOTPMDetails param, [FromHeader] string authorization)
        {
            OResponse oresponse = new OResponse();
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
                        oresponse = ResendOTP.resend(param);
                    }
                    else
                    {
                        oresponse.status = "Failed";
                        oresponse.remarks = lstTokenValidation[1];
                        //lstResponse.Add(response);
                    }
                }
                else
                {
                    oresponse.status = "Failed";
                    oresponse.remarks = "Invalid Token";
                    // lstResponse.Add(response);
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "resendOTP_controller", ex.Message, ex.StackTrace);
                oresponse.status = "Failed";
                oresponse.remarks = "Something went wrong !";
                //lstResponse.Add(response);
            }
            return Ok(oresponse);
        }

    }
}
