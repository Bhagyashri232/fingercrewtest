using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace FingerCrew.Model
{
    public class CommonUtilities
    {
        public string generateTokenNo()
        {
            string strToken = "";
            return strToken;
        }
        public static string createDirectory(string commonPath, string type ,string doctype,string mobileno)
        {
            List<string> lstFilePath = new List<string>();
            string newFilePath = "";
            string savenewFilePath = "";
            try
            {

                newFilePath = commonPath + "\\" ;
               // commonPath = "";
                savenewFilePath = " ";
                if (!Directory.Exists(newFilePath))
                {
                    Directory.CreateDirectory(newFilePath);
                }

                    newFilePath = commonPath + DateTime.Now.Year;
                   // savenewFilePath = DateTime.Now.Year + "\\";
                    if (!Directory.Exists(newFilePath))
                    {
                        Directory.CreateDirectory(newFilePath);
                    }

                    //savenewFilePath = savenewFilePath + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture) + "\\";
                    newFilePath = newFilePath + "/" + DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                    if (!Directory.Exists(newFilePath))
                    {
                        Directory.CreateDirectory(newFilePath);
                    }
                    //savenewFilePath = savenewFilePath + DateTime.Now.ToString("dd.MM.yyyy") + "\\";
                    newFilePath = newFilePath + "/" + DateTime.Now.ToString("dd.MM.yyyy");
                    if (!Directory.Exists(newFilePath))
                    {
                        Directory.CreateDirectory(newFilePath);
                    }
                    
                        //savenewFilePath = savenewFilePath  + "\\" + type + "\\" +doctype;
                        newFilePath = newFilePath + "/"+type + "/" + doctype;

                  
                    if (!Directory.Exists(newFilePath))
                    {
                        Directory.CreateDirectory(newFilePath);
                    }

                newFilePath = newFilePath + "/" + mobileno;
                if (!Directory.Exists(newFilePath))
                {
                    Directory.CreateDirectory(newFilePath);
                }

                //lstFilePath.Add(savenewFilePath);
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("AngelOne_BOT", "createDirectory", ex.Message, ex.StackTrace);
            }

            return newFilePath;
        }
        public static bool ValidateCurrentToken(string token, string tokenKey)
        {
            //var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
            var mySecret = tokenKey;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = ConfigurationManager.AppSetting["Jwt:Issuer"];
            var myAudience = ConfigurationManager.AppSetting["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "ValidateCurrentToken", ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }

        public static string GenerateRandomString(int Length)
        {
            char[] chars = new char[Length];
            try
            {
                string _allowedChars = "#@$&*abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
                Random randNum = new Random();

                for (int i = 0; i < Length; i++)
                {
                    chars[i] = _allowedChars[Convert.ToInt32((_allowedChars.Length - 1) * randNum.NextDouble())];
                }
            }
            catch (Exception ex)
            {
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "GenerateRandomString", ex.Message, ex.StackTrace);
            }

            return new string(chars);
        }

        public static void FnStoreErrorLog(string mode, string fnName, string errormessage, string errordescription)
        {
            try
            {
                ErroLogBO erroLogBO = new ErroLogBO();
                StoreErrorLog storeErrorLog = new StoreErrorLog();
                erroLogBO.mode = mode;
                erroLogBO.fnName = fnName;
                erroLogBO.error_message = errormessage;
                erroLogBO.error_description = errordescription;
                storeErrorLog.StoreError(erroLogBO);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }

        public static List<string> checkValidToken(string authorization, string tokenKey)
        {
            bool isValid = false;
            List<string> lstOutput = new List<string>();
            try
            {


                var tokenValue = "";
                var scheme = "";

                if (!string.IsNullOrEmpty(tokenKey))
                {
                    try
                    {
                        if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                        {
                            // we have a valid AuthenticationHeaderValue that has the following details:
                            // scheme will be "Bearer"                    
                            scheme = headerValue.Scheme;
                            // parmameter will be the token itself.
                            tokenValue = headerValue.Parameter;
                            if (!string.IsNullOrEmpty(tokenValue))
                            {
                               // isValid = true;
                                isValid = CommonUtilities.ValidateCurrentToken(tokenValue, tokenKey);
                                if (isValid == true)
                                {
                                    lstOutput.Add(Convert.ToString(isValid));
                                    lstOutput.Add("");
                                }
                                else
                                {
                                    lstOutput.Add(Convert.ToString(isValid));
                                    lstOutput.Add("Invalid Token");
                                }
                            }
                            else
                            {
                                lstOutput.Add(Convert.ToString(isValid));
                                lstOutput.Add("Please add generated token in Authorization.");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "checkValidToken_controller", ex.Message, ex.StackTrace);
                    }
                }
                else
                {
                    lstOutput.Add(Convert.ToString(isValid));
                    lstOutput.Add("Please add tokenKey in the header");
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                lstOutput.Add("Something Went Wrong!");
                CommonUtilities.FnStoreErrorLog("FingerCrewAPI", "checkValidToken_controller", ex.Message, ex.StackTrace);
            }

            return lstOutput;
        }

        internal static object createDirectory(string commonpath, string type, string doctype, object mobileno)
        {
            throw new NotImplementedException();
        }
    }
}
