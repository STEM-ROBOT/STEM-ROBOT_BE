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
        public SchoolSvc(SchoolRepo schoolRepo, IMapper mapper)
        {
            _schoolRepo = schoolRepo;
            _mapper = mapper;
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
                        for (int row = 3; row < rowCount; row++)
                        {

                            var schools = new School
                            {

                                SchoolName = string.IsNullOrEmpty(workSheet.Cells[row, 8].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 8].Value?.ToString().Trim(),
                                SchoolCode = string.IsNullOrEmpty(workSheet.Cells[row, 7].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 7].Value?.ToString().Trim(),
                                Address = string.IsNullOrEmpty(workSheet.Cells[row, 9].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 9].Value?.ToString().Trim(),
                                Province = string.IsNullOrEmpty(workSheet.Cells[row, 4].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 4].Value?.ToString().Trim(),
                                District = string.IsNullOrEmpty(workSheet.Cells[row, 6].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 6].Value?.ToString().Trim(),
                                ProvinceCode = string.IsNullOrEmpty(workSheet.Cells[row, 3].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 3].Value?.ToString().Trim(),
                                DistrictCode = string.IsNullOrEmpty(workSheet.Cells[row, 5].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 5].Value?.ToString().Trim(),
                                Area = string.IsNullOrEmpty(workSheet.Cells[row, 10].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 10].Value?.ToString().Trim(),

                            };
                            school.Add(schools);
                        }

                    }
                }
                await _schoolRepo.BulkInsertAsyncSchool(school);


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
                var school = _schoolRepo.getID(id);
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
                var schoolID = _schoolRepo.getID(Id);
                if(schoolID == null)
                {
                    res.SetError($"No SchoolID : {Id}");
                }
                var mapper = _mapper.Map<School>(school);
                _schoolRepo.Update(mapper);
                res.setData("OK", mapper);  
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
                var schoolID = _schoolRepo.getID(Id);
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
    }
}
