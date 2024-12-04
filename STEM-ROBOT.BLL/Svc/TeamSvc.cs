using AutoMapper;
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
    public class TeamSvc
    {
        private readonly TeamRepo _teamRepo;
        private readonly IMapper _mapper;
        private readonly CompetitionRepo _competitionRepo;
        private readonly TournamentRepo _tournamentRepo;
        private readonly ContestantTeamRepo _contestantTeamRepo;
        public TeamSvc(TeamRepo teamRepo, IMapper mapper, CompetitionRepo competitionRepo, TournamentRepo tournamentRepo, ContestantTeamRepo contestantTeamRepo)
        {
            _contestantTeamRepo = contestantTeamRepo;
            _tournamentRepo = tournamentRepo;
            _teamRepo = teamRepo;
            _mapper = mapper;
            _competitionRepo = competitionRepo;
        }

        public MutipleRsp GetTeams()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _teamRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<TeamRsp>>(lst);
                    res.SetSuccess(lstRes, "data");
                }
                else
                {
                    res.SetError("404", "No data found");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
      
        public SingleRsp GetById(int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    var teamRes = _mapper.Map<Common.Rsp.TeamRsp>(team);
                    res.setData("data", teamRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(TeamReq req)
        {
            var res = new SingleRsp();
            try
            {
                var newTeam = _mapper.Map<Team>(req);
                _teamRepo.Add(newTeam);
                res.setData("data", newTeam);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp UpdateTeam(TeamReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    _mapper.Map(req, team);
                    team.IsSetup = true;
                    _teamRepo.Update(team);
                    var contestantTeams = _contestantTeamRepo.All(filter: ct => ct.TeamId == id);
                    if (contestantTeams != null)
                    {
                        foreach (var item in contestantTeams)
                        {
                            _contestantTeamRepo.Delete(item.Id);
                        }
                    }

                    foreach (var item in req.Contestants)
                    {

                        var contestantTeam = new ContestantTeam
                        {
                            ContestantId = item.ContestantId,
                            TeamId = id
                        };
                        _contestantTeamRepo.Add(contestantTeam);
                    }
                }
                res.setData("data", req);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Delete(int id)
        {
            var res = new SingleRsp();
            try
            {
                var team = _teamRepo.GetById(id);
                if (team == null)
                {
                    res.SetError("404", "Team not found");
                }
                else
                {
                    _teamRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetTeamsByCompetition(int id)
        {
            var res = new MutipleRsp();
            try
            {
                var teams = _teamRepo.GetTeamsByCompetition(id);

                if (teams != null)
                {
                    var lstRes = _mapper.Map<List<TeamRsp>>(teams);
                    res.SetSuccess(lstRes, "data");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public MutipleRsp GetListTeamByTournament(int tournamentId)
        {
            var res = new MutipleRsp();
            try
            {
                var tournaments = _tournamentRepo.GetById(tournamentId);
                if (tournaments == null)
                {
                    res.SetError("404", "Tournament not found");
                }
                else
                {
                    var teams = _teamRepo.All(
                        filter: t => t.Competition.TournamentId == tournamentId,
                        includeProperties: "Competition"
                        ).ToList();

                    var lstTeamRsp = new List<ListTeamRspByTournament>();
                    foreach (var team in teams)
                    {
                        var teamRsp = new ListTeamRspByTournament
                        {
                            Id = team.Id,
                            CompetitionId = team.CompetitionId,
                            Name = team.Name,
                            PhoneNumber = team.PhoneNumber,
                            ContactInfo = team.ContactInfo,
                            Image = team.Image
                        };
                        lstTeamRsp.Add(teamRsp);
                    }
                    res.SetData("data", lstTeamRsp);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public async Task<SingleRsp> SchedulesTeamAdhesion(int teamId)
        {
            var res = new SingleRsp();
            try
            {
                var list = await _teamRepo.schedulesTeamAdhesion(teamId);
                if (list == null) throw new Exception("No data");
                var mapper = _mapper.Map<TeamScheduleRsp>(list);
                res.setData("data", mapper);

            }
            catch (Exception ex)
            {
                throw new Exception("Get List Referee Fail");
            }
            return res;

        }
    }
}
