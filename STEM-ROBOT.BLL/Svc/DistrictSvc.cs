using AutoMapper;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class DistrictSvc
    {
        private readonly DistrictRepo _districtRepo;
        private readonly ProvinceRepo _provinceRepo;
        private readonly IMapper _mapper;

        public DistrictSvc(DistrictRepo districtRepo, IMapper mapper, ProvinceRepo provinceRepo)
        {
            _districtRepo = districtRepo;
            _mapper = mapper;
            _provinceRepo = provinceRepo;
        }

        public async Task<MutipleRsp> ImportDistrictExcel(IFormFile file)
        {
            MutipleRsp res = new MutipleRsp();
            try
            {
                var districts = new List<District>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set license context
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        int rowCount = workSheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var provinceCode = workSheet.Cells[row, 3].Value?.ToString().Trim();
                            var District = new District
                            {
                                DistrictCode = string.IsNullOrEmpty(workSheet.Cells[row, 1].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 1].Value?.ToString().Trim(),
                                Name = string.IsNullOrEmpty(workSheet.Cells[row, 2].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 2].Value?.ToString().Trim(),
                                ProvinceCode = string.IsNullOrEmpty(workSheet.Cells[row, 3].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 3].Value?.ToString().Trim(),
                                ProvinceId = provinceCode != null ? GetIdProvince(provinceCode) : 0
                            };
                            districts.Add(District);
                        }
                    }
                }
                await _districtRepo.BulkInsertAsyncDistrict(districts);
                res.SetMessage("Import thành công");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public int GetIdProvince(string provinceCode)
        {
            var province = _provinceRepo.All(x => x.ProvinceCode == provinceCode).FirstOrDefault();
            if (province == null)
            {
                throw new Exception("Không tìm thấy tỉnh với mã này.");
            }
            return province.Id; // Trả về ProvinceId
        }


    }
}
