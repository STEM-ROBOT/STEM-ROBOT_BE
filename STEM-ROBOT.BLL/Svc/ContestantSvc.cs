using AutoMapper;
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
        public ContestantSvc(ContestantRepo contestantRepo, IMapper mapper, TournamentRepo tournamentRepo)
        {
            _tournamentRepo = tournamentRepo;
            _contestantRepo = contestantRepo;
            _mapper = mapper;
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
        public SingleRsp GetListContestants()
        {
            var res = new SingleRsp();
            try
            {
                var contestant = _contestantRepo.All();
                if (contestant != null)
                {
                    var mapper = _mapper.Map<IEnumerable<Contestant>>(contestant);
                    res.setData("Ok", mapper);
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
                // Lấy danh sách các thí sinh thuộc tournamentId
                var contestants = _contestantRepo.All(
                    filter: x => x.TournamentId == tournamentId,
                    includeProperties: "Tournament"
                ).ToList();

                if (contestants == null || !contestants.Any())
                {
                    res.SetError("No Data");
                    return res;
                }

                // Map từ danh sách contestants sang danh sách ContestantInTournament
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

                res.SetData("Ok", contestantRsp);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        // Response model để trả về danh sách các contestant
        public class ContestantResponse
        {
            public int Id { get; set; }
            public string Image { get; set; }
            public string SchoolName { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public string Phone { get; set; }
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
                    res.setData("Ok", mapper);
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
                    res.setData("OK", contestant);
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
                    res.setData("Ok", contestant);
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
