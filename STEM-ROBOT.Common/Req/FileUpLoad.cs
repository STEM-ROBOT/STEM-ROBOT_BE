using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace STEM_ROBOT.Common.Req
{
    public class FileUpload
    {
        public string Name { get; set; } // Tên file sẽ được lưu trong Cloud Storage
        public IFormFile File { get; set; } // Dùng IFormFile để nhận file upload từ client
    }
}
