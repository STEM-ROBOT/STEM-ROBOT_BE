using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Algorithm;
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
    public class ProvinceSvc
    {
        private readonly ProvinceRepo _provinceRepo;
        private readonly IMapper _mapper;
        public ProvinceSvc(ProvinceRepo provinceRepo, IMapper mapper)
        {
            _provinceRepo = provinceRepo;
            _mapper = mapper;
        }
        public async Task<MutipleRsp> ImportProvinceExcel(IFormFile file)
        {
            MutipleRsp res = new MutipleRsp();
            try
            {
                var provinces = new List<Province>(); // Corrected variable name
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        int rowCount = workSheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var province = new Province
                            {
                                Name = string.IsNullOrEmpty(workSheet.Cells[row, 2].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 2].Value?.ToString().Trim(),
                                ProvinceCode = string.IsNullOrEmpty(workSheet.Cells[row, 1].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 1].Value?.ToString().Trim(),
                                AreaId = int.TryParse(workSheet.Cells[row, 3].Value?.ToString().Trim(), out int areaId) ? areaId : 0

                            };
                            provinces.Add(province); // Corrected variable name
                        }
                    }
                }
                await _provinceRepo.BulkInsertAsyncProvince(provinces); // Corrected variable name
                res.SetMessage("Import thành công");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
