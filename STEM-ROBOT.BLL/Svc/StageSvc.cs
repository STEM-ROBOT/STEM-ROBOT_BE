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
    public class StageSvc
    {
        private readonly StageRepo _stageRepo;
        private readonly IMapper _mapper;
        public StageSvc(StageRepo stageRepo, IMapper mapper)
        {
            _stageRepo = stageRepo;
            _mapper = mapper;
        }
        public MutipleRsp GetListStage()
        {
            var res = new MutipleRsp();
            try
            {
                var stage = _stageRepo.All();
                if (stage == null)
                {
                    res.SetError("Plage add data!");
                }
                var mapper = _mapper.Map<IEnumerable<Stage>>(stage);
                res.SetData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }

        public SingleRsp GetIDStage(int id)
        {
            var res = new SingleRsp();
            try
            {
                var stage = _stageRepo.GetById(id);
                if (stage == null)
                {
                    res.SetError("Plage add data!");
                }
                var mapper = _mapper.Map<Stage>(stage);
                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }
        public SingleRsp AddStage(StageReq request)
        {
            var res = new SingleRsp();
            try
            {
                var mapper = _mapper.Map<Stage>(request);
                if (mapper == null)
                {
                    res.SetError("Plage add data!");
                }

                res.setData("Ok", mapper);
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }
        public SingleRsp UpdateStage(int id, StageReq request)
        {
            var res = new SingleRsp();
            try
            {
                var stage = _stageRepo.GetById(id);

                if (stage == null)
                {
                    res.SetError("Plage add data!");
                }
                _mapper.Map(request, stage);

                res.setData("Ok", stage);
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }
        public SingleRsp DeleteStage(int id)
        {
            var res = new SingleRsp();
            try
            {
                var stage = _stageRepo.GetById(id);
                if (stage == null)
                {
                    res.SetError("No ID");
                }
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }
        public MutipleRsp CreateStages(int competitionId, int numberStage)
        {
            var res = new MutipleRsp();
            try
            {
                var competition = _stageRepo.GetById(competitionId);
                if (competition == null)
                {
                    res.SetError("No Competition found");
                    return res;
                }
                var createdStages = new List<Stage>();
                for (int i = 1; i <= numberStage; i++)
                {
                    var stage = new Stage
                    {
                        CompetitionId = competitionId,
                        Name = "Vòng " + i,
                    };
                    _stageRepo.Add(stage);
                    createdStages.Add(stage);
                }
                res.SetData("200", createdStages);
                return res;
            }
            catch (Exception ex)
            {
                res.SetError($"{ex.Message}");
            }
            return res;
        }
        public Stage GetFirstStageByCompetitionId(int competitionId)
        {
            return _stageRepo.All().FirstOrDefault(s => s.CompetitionId == competitionId);
        }

    
    }
}