using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Algorithm;
using OfficeOpenXml;
using STEM_ROBOT.Common.Req;
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
    public class SchoolSvc
    {
        private readonly SchoolRepo _schoolRepo;
        private readonly IMapper _mapper;
        private readonly DistrictRepo _districtRepo;
        public SchoolSvc(SchoolRepo schoolRepo, IMapper mapper, DistrictRepo districtRepo)
        {
            _schoolRepo = schoolRepo;
            _mapper = mapper;
            _districtRepo = districtRepo;
        }

        public async Task<MutipleRsp> ImportSchool(IFormFile file)
        {

            MutipleRsp res = new MutipleRsp();
            try
            {
                var school = new List<School>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        int rowCount = workSheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var schools = new School
                            {
                                SchoolName = string.IsNullOrEmpty(workSheet.Cells[row, 2].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 2].Value?.ToString().Trim(),
                                SchoolCode = string.IsNullOrEmpty(workSheet.Cells[row, 1].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 1].Value?.ToString().Trim(),
                                Address = string.IsNullOrEmpty(workSheet.Cells[row, 3].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 3].Value?.ToString().Trim(),
                                ProvinceCode = string.IsNullOrEmpty(workSheet.Cells[row, 5].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 5].Value?.ToString().Trim(),
                                DistrictCode = string.IsNullOrEmpty(workSheet.Cells[row, 4].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 4].Value?.ToString().Trim(),
                                DistrictId = GetIdDistrict(workSheet.Cells[row, 5].Value?.ToString().Trim(), workSheet.Cells[row, 4].Value?.ToString().Trim())
                            };
                            school.Add(schools);
                        }

                    }
                }
                await _schoolRepo.BulkInsertAsyncSchool(school);
                res.SetMessage("Import thành công");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public MutipleRsp ListSchool()
        {
            var res = new MutipleRsp();
            try
            {
                var school = _schoolRepo.All();
                var mapper = _mapper.Map<IEnumerable<SchoolRep>>(school);
                res.SetSuccess(mapper, "OK");

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp ListIDSchool(int id)
        {
            var res = new SingleRsp();
            try
            {
                var school = _schoolRepo.GetById(id);
                if (school == null)
                {
                    res.SetError($"NO School ID: {id}");
                }
                var mapper = _mapper.Map<SchoolRep>(school);
                res.setData("OK",mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp AddSchool(SchoolReq school)
        {
            var res = new SingleRsp();
            try
            {
              var mapper = _mapper.Map<School>(school);
                if(mapper != null)
                {
                    _schoolRepo.Add(mapper);

                    res.setData("OK", mapper);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateSchool(int Id,SchoolReq school)
        {
            var res = new SingleRsp();
            try
            {
                var schoolID = _schoolRepo.GetById(Id);
                if(schoolID == null)
                {
                    res.SetError($"No SchoolID : {Id}");
                }
                _mapper.Map(school, schoolID);
                _schoolRepo.Update(schoolID);
                res.setData("OK", schoolID);  
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteSchool(int Id)
        {
            var res = new SingleRsp();
            try
            {
                var schoolID = _schoolRepo.GetById(Id);
                if (schoolID == null)
                {
                    res.SetError($"No SchoolID : {Id}");
                }
                _schoolRepo.Delete(Id);
                res.setData("Ok",schoolID);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public int GetIdDistrict(string provinceCode, string districtCode)
        {
            var district = _districtRepo.All()
        .Where(d => d.ProvinceCode == provinceCode && d.DistrictCode == districtCode)
        .FirstOrDefault();

            if (district == null)
            {
                throw new Exception("Không tìm thấy quận/huyện với mã tỉnh và mã huyện này.");
            }

            return district.Id; // Trả về DistrictId
        }

    }
}
