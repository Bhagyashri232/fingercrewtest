using FingerCrew.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace FingerCrew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BankMasterController : ControllerBase
    {
        BankMaster bankMaster = new BankMaster();

        [HttpPost]
        [Route("AddBankMaster")]
        public IActionResult addBankDetails(bankMDetails param, [FromHeader] string authorization)
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
                        lstResponse = bankMaster.addBankMaster(param);
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = lstTokenValidation[1];
                        lstResponse.Add(response);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Invalid Token";
                    lstResponse.Add(response);
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "addbankDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
                lstResponse.Add(response);
            }
            return Ok(lstResponse);
        }

        [HttpPost]
        [Route("getBankDetails")]
        public IActionResult getBankWiseDetails(bankDetailsParam param, [FromHeader] string authorization)
        {
            bankDetailResponse response = new bankDetailResponse();
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
                        response = bankMaster.getBankData(param);
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
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "getBankDetails_controller", ex.Message, ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong !";
            }
            return Ok(response);
        }


    }
}
