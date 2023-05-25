using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FingerCrew.Model;

namespace FingerCrew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("generateToken")]
        public IActionResult GenerateNewToken()
        {
            try
            {
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Key"]));
                string uniquekey = CommonUtilities.GenerateRandomString(20);
                ConfigurationManager.AppSetting["JWT:Key"] = uniquekey;
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: ConfigurationManager.AppSetting["JWT:Issuer"],
                    audience: ConfigurationManager.AppSetting["JWT:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                if (!string.IsNullOrEmpty(tokenString))
                {
                    return Ok(new JWTTokenResponse { status = "Success", remarks = "Token Generated Successfully", tokenKey = uniquekey, Token = tokenString, ExpireTime = DateTime.Now.AddMinutes(6).ToString("mm") });
                }
                else
                {
                    return Ok(new JWTTokenResponse { status = "Failed", remarks = "Token Generation Failed", tokenKey = "", Token = "", ExpireTime = "0" });
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "GenerateToken", ex.Message, ex.StackTrace);
                return Ok(new JWTTokenResponse { status = "Failed", remarks = "Something Went Wrong.", tokenKey = "", Token = "", ExpireTime = "0" });
            }
        }
    }
}
