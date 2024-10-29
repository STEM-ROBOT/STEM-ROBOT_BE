using AutoMapper;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class StorageSvc
    {
      
        private readonly string _googleCredentialsFilePath = Path.Combine(AppContext.BaseDirectory, "stem-system-d01be9098931.json");
        private readonly string _bucketName = "stem-system-storage";
        public string GetFileUrl(string fileName)
        {
            UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(_googleCredentialsFilePath);

            // Tạo URL có thời hạn (ví dụ: 1 ngày)
            var url = urlSigner.Sign(
               _bucketName,
                fileName,
                TimeSpan.FromDays(1), // Thời gian hiệu lực của URL
                HttpMethod.Get); // HTTP method (GET)

            return url;
        }
    }
}
