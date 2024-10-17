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
        private readonly IMapper _mapper;

        public ScoreCategorySvc(ScoreCategoryRepo scoreCategoryRepo, IMapper mapper)
        {
            _scoreCategoryRepo = scoreCategoryRepo;
            _mapper = mapper;
        }

        public MutipleRsp GetScoreCategories()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _scoreCategoryRepo.All();
                if (lst == null || !lst.Any())
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    res.SetSuccess(lst, "200");
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
                var scoreCategory = _scoreCategoryRepo.GetById(id);
                if (scoreCategory == null)
                {
                    res.SetError("404", "Score category not found");
                }
                else
                {
                    res.setData("200", scoreCategory);
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(ScoreCategoryReq req)
        {
            var res = new SingleRsp();
            try
            {
                var newScoreCategory = _mapper.Map<ScoreCategory>(req);
                _scoreCategoryRepo.Add(newScoreCategory);
                res.setData("200", newScoreCategory);
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
                    res.setData("200", scoreCategory);
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
    }
}
