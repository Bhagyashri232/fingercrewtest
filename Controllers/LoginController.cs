
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FingerCrew.Model;
using Response = FingerCrew.Model.Response;
using Mob_Surveyor_Login_API.Model;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        LoginMaster loginMaster = new LoginMaster();
        [HttpPost]
        [Route("loginSurveyor")]
        public IActionResult login(loginMDetails param, [FromHeader] string authorization)
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
                        oresponse = loginMaster.login(param);
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "login_controller", ex.Message, ex.StackTrace);
                oresponse.status = "Failed";
                oresponse.remarks = "Something went wrong !";
                //lstResponse.Add(response);
            }
            return Ok(oresponse);
        }

    }
}
