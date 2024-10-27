using AutoMapper;
using Azure.Core;
using STEM_ROBOT.BLL.Mapper;
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
    public class CompetitionSvc

    {
        private readonly CompetitionRepo _competitionRepo;
        private readonly IMapper _mapper;

        private readonly StageSvc _stageSvc;
        private readonly StageRepo _stageRepo;
        private readonly TeamRepo _teamRepo;
        private readonly MatchRepo _matchRepo;
        private readonly TeamMatchRepo _teamMatchRepo;
        private readonly TeamSvc _teamSvc;
        private readonly TableGroupSvc _tableGroupSvc;

        
        public CompetitionSvc(CompetitionRepo competitionRepo, IMapper mapper, StageSvc stageSvc, StageRepo stageRepo,TeamRepo teamRepo,MatchRepo matchRepo,TeamMatchRepo teamMatchRepo, TeamSvc teamSvc, TableGroupSvc tableGroupSvc)
        {
            _competitionRepo = competitionRepo;
            _mapper = mapper;
            _stageSvc = stageSvc;
            _stageRepo = stageRepo;
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _teamMatchRepo = teamMatchRepo;
            _teamSvc = teamSvc;
            _stageSvc = stageSvc;
            _tableGroupSvc = tableGroupSvc;
        }

        public async Task<MutipleRsp> GetListCompetitions()
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetition();
                if (list == null)
                {
                    res.SetError("No Data");
                }
                var mapper = _mapper.Map<IEnumerable<CompetitionRep>>(list);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> GetIDCompetitions(int id)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _competitionRepo.getListCompetitionbyID(id);
                if (list == null)
                {
                    res.SetError("No Data");
                }
                var mapper = _mapper.Map<CompetitionRep>(list);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<MutipleRsp> getCompetitionWithIDTournament(int IdTournament)
        {
            var res = new MutipleRsp();
            try
            {
                var id = _competitionRepo.GetById(IdTournament);
                if(id == null) throw new Exception("No data");
                var list =  _competitionRepo.All(x => x.TournamentId == IdTournament);
              
                var mapper = _mapper.Map<List<ListCompetiton>>(list);
                res.SetData("data", mapper);
            }
            catch (Exception ex)
            {
                throw new Exception("No data");
            }
            return res;
        }
        public SingleRsp AddCompetion(CompetitionReq request)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Competition>(request);
                if (mapper == null)
                {
                    res.SetError("Please check again!");
                }
                _competitionRepo.Add(mapper);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateCompetition(int id, CompetitionReq request)
        {
            var res = new SingleRsp();
            try
            {
                var competion = _competitionRepo.GetById(id);


                if (competion == null)
                {
                    res.SetError("Please check ID again!");
                }
                var mapper = _mapper.Map(request, competion);
                _competitionRepo.Update(mapper);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteCompetition(int id)
        {
            var res = new SingleRsp();
            try
            {
                var competion = _competitionRepo.GetById(id);


                if (competion == null)
                {
                    res.SetError("Please check ID again!");
                }
                _competitionRepo.Delete(competion.Id);
                res.setData("OK", competion);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        //update competitionformatconfig
        public async Task<SingleRsp> UpdateCompetitionConfig(CompetitionConfigReq request)
        {
            var res = new SingleRsp();
            try
            {

                var id = _competitionRepo.GetById(request.Id);

                if (id == null)
                {
                    res.SetError("No ID");
                }
                var map = _mapper.Map(request, id);
                _competitionRepo.Update(map);
                if (request.FormatId == 2)
                {
                    int teamCount = (int)request.NumberTeam;
                    var checkBool = await StateSetup(teamCount, request.Id);

                    if (checkBool == false)
                    {
                        throw new Exception("Tạo vòng đấu thất bại !!");
                    }

                }

                res.Setmessage("Ok");


            }
            catch (Exception ex)
            {
                throw new Exception("Fail Loại trực tiếp");
            }
            return res;

        }

        public SingleRsp CreateCompetionFormatTable(CompetitionReq request)

        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Competition>(request);
                if (mapper == null)
                {
                    res.SetError("Please check again!");
                }
                _competitionRepo.Add(mapper);
                if (request.FormatId == 1)
                {
                    _teamSvc.CreateTeams(mapper.Id, request.NumberTeam);
                    int numberStage = CalculateNumberStage(request.NumberTeam, (int)request.NumberTable);
                    _stageSvc.CreateStages(mapper.Id, numberStage);

                    // Add tables to the first stage created
                    var stages = _stageSvc.GetFirstStageByCompetitionId(mapper.Id);
                    _tableGroupSvc.CreateTables(stages.Id, (int)request.NumberTable);
                }
                res.setData("OK", mapper);

            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }


        private async Task<bool> StateSetup(int number, int competitionId)
        {
            if (number <= 0)
            {
                throw new Exception("Number must be greater than 0");
            }

            // Thêm đội vào giải đấu
            for (int i = 1; i <= number; i++)
            {
                var team = new Team()
                {
                    CompetitionId = competitionId,
                    Name = $"Đội #{i}",
                    Image = "https://www.pngmart.com/files/22/Manchester-United-Transparent-Images-PNG.png",
                };
                _teamRepo.Add(team);
            }

            List<Team> teams = await _competitionRepo.GetTeamsByCompetitionId(competitionId);
            if (teams == null || teams.Count == 0) throw new Exception("No team found for competition.");

            teams = teams.OrderBy(t => t.Id).ToList();

           
            bool isPowerOf2 = (number & (number - 1)) == 0;
            int round = (int)Math.Ceiling(Math.Log2(number));
            int closestPowerOf2 = (int)Math.Pow(2, round);

            int extraTeams = number - (closestPowerOf2 / 2);

            List<Team> winningTeamsFromExtraRound = new List<Team>();

           
            if (!isPowerOf2 && extraTeams > 0)
            {
                string roundName = $"Vòng phụ cho vòng 1/{Math.Pow(2, round)}";
                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(2),
                    Status = "Vòng phụ"
                };
                _stageRepo.Add(stage);

           
                var teamsForExtraRound = teams.Take(extraTeams * 2).ToList();

               
                for (int i = 0; i < teamsForExtraRound.Count; i += 2)
                {
                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Đấu phụ",
                        TimeIn = DateTime.Now,
                        TimeOut = DateTime.Now.AddHours(1)
                    };

                    _matchRepo.Add(match);

                    
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teamsForExtraRound[i].Id, NameDefault = teamsForExtraRound[i].Name });
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teamsForExtraRound[i + 1].Id, NameDefault = teamsForExtraRound[i + 1].Name });

                  
                    winningTeamsFromExtraRound.Add(new Team
                    {
                        Id = teamsForExtraRound[i].Id,
                        Name = $"W#{i / 2 + 1} {roundName}" 
                    });
                }

             
                teams = teams.Except(teamsForExtraRound).ToList();
            }

          
            teams.AddRange(winningTeamsFromExtraRound);

        
            for (int i = round; i > 0; i--)
            {
                string roundName;
                if (i == 1)
                    roundName = "Chung Kết";
                else if (i == 2)
                    roundName = "Bán Kết";
                else if (i == 3)
                    roundName = "Tứ Kết";
                else
                    roundName = $"Vòng 1/{Math.Pow(2, i)}";

                var stage = new Stage
                {
                    CompetitionId = competitionId,
                    Name = roundName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    Status = "Vòng chính"
                };
                _stageRepo.Add(stage);

             
                List<Team> teamsInCurrentRound = new List<Team>();
                for (int j = 0; j < teams.Count - 1; j += 2)
                {
                    var match = new Match
                    {
                        StageId = stage.Id,
                        TableId = null,
                        StartDate = DateTime.Now,
                        Status = "Loại trực tiếp",
                        TimeIn = DateTime.Now,
                        TimeOut = DateTime.Now.AddHours(1)
                    };

                    _matchRepo.Add(match);

                  
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teams[j].Id, NameDefault = teams[j].Name });
                    _teamMatchRepo.Add(new TeamMatch { MatchId = match.Id, TeamId = teams[j + 1].Id, NameDefault = teams[j + 1].Name });

                   
                    teamsInCurrentRound.Add(new Team
                    {
                        Id = teams[j].Id,
                        Name = $"W#{j / 2 + 1} {roundName}" 
                    });
                }

               
                teams = teamsInCurrentRound.ToList();
            }

            return true;
        }




        public int CalculateNumberStage(int numberTeam, int numberTable)
        {
            return (int)Math.Log2(numberTeam / numberTable) + 2;
        }

    }
}
