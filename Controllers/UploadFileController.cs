using FingerCrew.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace FingerCrew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private static IWebHostEnvironment _webHostEnvironment;

        public UploadFileController (IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<string> upload([FromForm] UploadFile obj)
        {
            if (obj.files.Length >0)
            {
                try
                {
                    if(!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images\\");
                    }
                    
                    using (FileStream fileStream =System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Images\\"+ obj.files.FileName))
                    {
                        obj.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "\\Images\\" +obj.files.FileName;
                    }
                }
                catch (Exception ex) 
                {
                    return ex.ToString();
                
                }
            }
            else
            {
                return "Upload failed";

            }
        }
    }
}
