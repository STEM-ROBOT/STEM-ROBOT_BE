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
    public class ScoreCategorySvc
    {
        private readonly ScoreCategoryRepo _scoreCategoryRepo;
        private readonly CompetitionRepo _competitionRepo;
        private readonly IMapper _mapper;

        public ScoreCategorySvc(ScoreCategoryRepo scoreCategoryRepo, IMapper mapper,CompetitionRepo competitionRepo)
        {
            _scoreCategoryRepo = scoreCategoryRepo;
            _mapper = mapper;
            _competitionRepo = competitionRepo;
        }

        public SingleRsp GetScoreCategories(int competitonId)
        {
            var res = new SingleRsp();
            try
            {
                var lst = _scoreCategoryRepo.All(x=>x.CompetitionId == competitonId);             
                    var lstRes = _mapper.Map<List<ScoreCategoryRsp>>(lst);
                    res.setData("data", lstRes);              
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
                var scoreCategory = _scoreCategoryRepo.GetById(id);
                if (scoreCategory == null)
                {
                    res.SetError("404", "Score category not found");
                }
                else
                {
                    var scoreCategoryRes = _mapper.Map<ScoreCategoryRsp>(scoreCategory);
                    res.setData("data", scoreCategoryRes);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(List<ScoreCategoryReq> reqList, int competitionId)
        {
            var res = new SingleRsp();
            try
            {        
                var competition = _competitionRepo.All(x => x.Id == competitionId).FirstOrDefault();
                if (competition == null)
                {
                    throw new Exception("Competition not found");
                }

                var newScoreCategories = _mapper.Map<List<ScoreCategory>>(reqList);
                foreach (var scoreCategory in newScoreCategories)
                {
                    scoreCategory.CompetitionId = competitionId;
                    _scoreCategoryRepo.Add(scoreCategory);
                }

                res.setData("data", "add success");
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(ScoreCategoryReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var scoreCategory = _scoreCategoryRepo.GetById(id);
                if (scoreCategory == null)
                {
                    res.SetError("404", "Score category not found");
                }
                else
                {
                    _mapper.Map(req, scoreCategory);
                    _scoreCategoryRepo.Update(scoreCategory);
                    res.setData("data", scoreCategory);
                }
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
                var scoreCategory = _scoreCategoryRepo.GetById(id);
                if (scoreCategory == null)
                {
                    res.SetError("404", "Score category not found");
                }
                else
                {
                    _scoreCategoryRepo.Delete(id);
                    res.SetMessage("Delete successfully");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        //public SingleRsp getScoreByCompetition(int competitionId)
        //{
        //    var res = new SingleRsp();
        //    try
        //    {
        //        var score = _scoreCategoryRepo.All(x=>x.CompetitionId == competitionId);
        //        if (score == null)
        //        {
        //            res.SetError("404", "Score category not found");
        //        }
        //        else
        //        {
        //            var lstRes = _mapper.Map<List<ScoreCategoryRsp>>(score);
        //            res.setData("data", lstRes);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.SetError("500", ex.Message);
        //    }
        //    return res;
        //}
       
    }
}
