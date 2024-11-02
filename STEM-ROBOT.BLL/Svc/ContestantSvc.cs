using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ContestantSvc
    {
        private readonly IMapper _mapper;
        private readonly ContestantRepo _contestantRepo;
        private readonly TournamentRepo _tournamentRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContestantSvc(ContestantRepo contestantRepo, IMapper mapper, TournamentRepo tournamentRepo, IHttpContextAccessor httpContextAccessor)
        {
            _tournamentRepo = tournamentRepo;
            _contestantRepo = contestantRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<MutipleRsp> AddContestant(IFormFile file)
        {

            MutipleRsp res = new MutipleRsp();
            try
            {
                var contestant = new List<Contestant>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        int rowCount = workSheet.Dimension.Rows;
                        for (int row = 3; row < rowCount; row++)
                        {

                            var contestants = new Contestant
                            {

                            //    SchoolId = int.TryParse(workSheet.Cells[row, 1].Value?.ToString().Trim(), out var schoolId)? schoolId: 0,  
                                TournamentId = int.TryParse(workSheet.Cells[row, 2].Value?.ToString().Trim(), out var tournamentId) ? tournamentId: 0,
                                Name = string.IsNullOrEmpty(workSheet.Cells[row, 3].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 3].Value?.ToString().Trim(),
                                Email = string.IsNullOrEmpty(workSheet.Cells[row, 4].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 4].Value?.ToString().Trim(),
                                Status = string.IsNullOrEmpty(workSheet.Cells[row, 5].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 5].Value?.ToString().Trim(),
                                Gender = string.IsNullOrEmpty(workSheet.Cells[row, 6].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 6].Value?.ToString().Trim(),
                                Phone = string.IsNullOrEmpty(workSheet.Cells[row, 7].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 7].Value?.ToString().Trim(),
                                Image = string.IsNullOrEmpty(workSheet.Cells[row, 8].Value?.ToString().Trim())
                                              ? "Không có dữ liệu"
                                              : workSheet.Cells[row, 8].Value?.ToString().Trim(),

                            };
                            contestant.Add(contestants);
                        }

                    }
                }
                await _contestantRepo.BulkInsertAsyncSchool(contestant);


            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        
        public MutipleRsp AddListContestant(List<ContestantReq> contestants, int accountId, int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var contestantList = new List<Contestant>();

                foreach (var item in contestants)
                {
                    var contestant = new Contestant
                    {
                        TournamentId = tournamentId,
                        AccountId = accountId,
                        Name = string.IsNullOrEmpty(item.Name) ? "Không có dữ liệu" : item.Name,
                        Email = string.IsNullOrEmpty(item.Email) ? "Không có dữ liệu" : item.Email,
                        Status = "active",
                        Gender = string.IsNullOrEmpty(item.Gender) ? "Không có dữ liệu" : item.Gender,
                        Phone = string.IsNullOrEmpty(item.Phone) ? "Không có dữ liệu" : item.Phone,
                        Image = string.IsNullOrEmpty(item.Image) ? "Không có dữ liệu" : item.Image,

                    };

                    contestantList.Add(contestant);
                }

                _contestantRepo.BulkInsertAsyncSchool(contestantList);
                res.SetData("data", contestantList);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        public SingleRsp GetListContestants()
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.All();
                if (contestant != null)
                {
                    var mapper = _mapper.Map<IEnumerable<Contestant>>(contestant);
                    res.setData("200", mapper);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetListContestantByTournament(int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var contestants = _contestantRepo.All(
                    filter: x => x.TournamentId == tournamentId,
                    includeProperties: "Tournament"
                ).ToList();

                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }
                var contestantRsp = contestants.Select(c => new ContestantInTournament
                {
                    Id = c.Id,
                    Image = c.Image,
                    SchoolName = c.Account?.School?.SchoolName,
                    Name = c.Name,
                    Email = c.Email,
                    Gender = c.Gender,
                    Phone = c.Phone
                }).ToList();

                res.SetData("data", contestantRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        public SingleRsp GetContestantID(int ID)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(ID);
                if (contestant != null)
                {
                    var mapper = _mapper.Map<Contestant>(contestant);
                    res.setData("200", mapper);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateContestant(int contestantID, ContestantReq contestantReq)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(contestantID);
                if (contestant != null)
                {
                    _mapper.Map(contestantReq, contestant);
                    _contestantRepo.Update(contestant);
                    res.setData("200", contestant);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteContestant(int contestantID)
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.GetById(contestantID);
                if (contestant != null)
                {
                    _contestantRepo.Delete(contestant.Id);
                    res.setData("200", contestant);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
    }
}
