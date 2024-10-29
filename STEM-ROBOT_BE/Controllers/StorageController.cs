using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.Common.Req;



namespace STEM_ROBOT.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly StorageSvc _storageSvc;


        public StorageController(ILogger<StorageController> logger, StorageSvc storageSvc )
        {
            _storageSvc = storageSvc;
            _logger = logger;
        }
        private readonly string _googleCredentialsFilePath = Path.Combine(AppContext.BaseDirectory, "stem-system-d01be9098931.json");


        [HttpPost]
        public async Task<IActionResult> AddFile([FromForm] FileUpload fileUpload)
        {
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(_googleCredentialsFilePath);
            var client = StorageClient.Create(credential);
            using (var stream = new MemoryStream())
            {
                await fileUpload.File.CopyToAsync(stream);
                stream.Position = 0; // Đặt lại vị trí stream trước khi upload

                var obj = await client.UploadObjectAsync(
                    "stem-system-storage",
                    fileUpload.Name,
                    fileUpload.File.ContentType,
                    stream);
                string fileUrl = _storageSvc.GetFileUrl(fileUpload.Name);

                return Ok(new { Url = fileUrl, Message = "File uploaded successfully!" });
            }

        }
      
      

    }
}
