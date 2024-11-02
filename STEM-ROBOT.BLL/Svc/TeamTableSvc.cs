﻿using AutoMapper;
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

        public MutipleRsp GetDataToAssign(int competitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var teamTable = _teamTableRepo.All;
                var team = _teamRepo.All;
                var tableGroup = _tableGroupRepo.All;
                var competition = _competitionRepo.All;
                if (teamTable == null || team == null || tableGroup == null || competition == null)
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
    }
}
