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
    public class MatchSvc
    {
        private readonly MatchRepo _matchRepo;
        private readonly IMapper _mapper;
        private readonly TeamTableRepo _teamTableRepo;
        private readonly TableGroupRepo _tableGroupRepo;
        private readonly TeamRepo _teamRepo;
        private readonly StageRepo _stageRepo;
        private readonly StageSvc _stageSvc;
        public MatchSvc(MatchRepo repo, IMapper mapper, TeamTableRepo teamTableRepo, TableGroupRepo tableGroupRepo, TeamRepo teamRepo, StageRepo stageRepo, StageSvc stageSvc)
        {
            _matchRepo = repo;
            _teamTableRepo = teamTableRepo;
            _mapper = mapper;
            _tableGroupRepo = tableGroupRepo;
            _teamRepo = teamRepo;
            _stageRepo = stageRepo;
            _stageSvc = stageSvc;
        }
        public MutipleRsp GetListMatch()
        {
            var res = new MutipleRsp();
            try
            {
                var list = _matchRepo.All();
                if (list == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<IEnumerable<MatchRep>>(list);
                res.SetData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp getByIDMatch(int id)

        {
            var res = new SingleRsp();
            try
            {
                var list = _matchRepo.GetById(id);
                if (list == null)
                {
                    res.SetError("No data");
                }
                var mapper = _mapper.Map<MatchRep>(list);
                res.setData("OK", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }

        public SingleRsp AddMatch(MatchReq req)
        {
            var res = new SingleRsp();
            try
            {
               var mapper = _mapper.Map<Match>(req);
                if(mapper == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Add(mapper);
                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp UpdateMatch(int id,MatchReq req)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Match>(req);
                if (mapper == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Add(mapper);
                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }
        public SingleRsp DeleteMatch(int id)
        {
            var res = new SingleRsp();
            try
            {
                var match = _matchRepo.GetById(id);
                if (match == null)
                {
                    res.SetError("Please check data");
                }
                _matchRepo.Delete(match.Id);
                res.setData("Ok", match);
            }
            catch (Exception ex)
            {
                res.SetError(ex.Message);
            }
            return res;
        }

        public async Task<MutipleRsp> getListRound(int competitionID)

        {
            var res = new MutipleRsp();
            try
            {
                var list = await _matchRepo.getRoundGame(competitionID);
                if (list == null) throw new Exception("No data");
                res.SetData("data", list);

            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }
        public async Task<MutipleRsp> getListKnockOut(int CompetitionId)
        {
            var res = new MutipleRsp();
            try
            {
                var list = await _matchRepo.getKnockOut(CompetitionId);
                if (list == null) throw new Exception("No data");
                res.SetData("data", list);

            }
            catch
            {
                throw new Exception("Fail");
            }
            return res;
        }
    }
}
