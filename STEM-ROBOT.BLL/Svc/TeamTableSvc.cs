using AutoMapper;
using NetTopologySuite.Utilities;
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
    public class TeamTableSvc
    {
        private readonly TeamTableRepo _teamTableRepo;
        private readonly IMapper _mapper;
        private readonly TeamRepo _teamRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly StageRepo _stageRepo;
        private readonly CompetitionRepo _competitionRepo;
        public TeamTableSvc(TeamTableRepo teamTableRepo, IMapper mapper, TeamRepo teamRepo, TableGroupRepo tableGroupRepo, StageRepo stageRepo, CompetitionRepo competitionRepo)
        {
            _teamTableRepo = teamTableRepo;
            _mapper = mapper;
            _teamRepo = teamRepo;
            _tableGroupRepo = tableGroupRepo;
            _stageRepo = stageRepo;
            _competitionRepo = competitionRepo;
        }
        public MutipleRsp GetListTeamTable()
        {
            var res = new MutipleRsp();
            try
            {
                var teamTable = _teamTableRepo.All();
                if (teamTable == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<TeamTable>>(teamTable);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp GetIdTeamTable(int id)
        {
            var res = new SingleRsp();
            try
            {
                var teamTable = _teamTableRepo.GetById(id);
                if (teamTable == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<TeamTable>(teamTable);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }

        public SingleRsp AssignTeamsToTables(int competitionId, List<TableAssignmentReq> tableAssignments)
        {
            var res = new SingleRsp();

            try
            {
                // Validate that table assignments are provided
                if (tableAssignments == null || !tableAssignments.Any())
                {
                    throw new Exception("Không có thông tin bảng đấu hoặc đội.");
                }

                // Iterate through each table assignment provided by the frontend
                foreach (var assignment in tableAssignments)
                {
                    // Validate table ID and team list
                    if (assignment.TableGroupId <= 0 || assignment.Teams == null || !assignment.Teams.Any())
                    {
                        throw new Exception("Thông tin bảng hoặc danh sách đội không hợp lệ.");
                    }

                    // Add each team in the team list to the specified table
                    foreach (var teamId in assignment.Teams)
                    {
                        AddTeamToTable(teamId, assignment.TableGroupId);
                    }
                }

                res.setData("200", "Success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }

            return res;
        }

        // Method to add a team to a specific table in the TeamTable database
        public void AddTeamToTable(int teamId, int tableGroupId)
        {
            var teamTable = new TeamTable
            {
                TeamId = teamId,
                TableGroupId = tableGroupId,
                IsSetup = true // Assuming this should be true when teams are assigned
            };
            _teamTableRepo.Add(teamTable);
        }




    }
}
