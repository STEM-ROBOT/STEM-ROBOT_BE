using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class GenreSvc
    {
        private readonly GenreRepo _genreRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public GenreSvc(GenreRepo repo, IConfiguration configuration, IMapper mapper)
        {
            _genreRepo = repo;
            _mapper = mapper;
            _configuration = configuration;
        }

        public MutipleRsp GetGenres()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _genreRepo.All();
                if (lst != null)
                {
                    var lstRes = _mapper.Map<List<GenreRsp>>(lst);
                    res.SetData("200",lst);
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
                var getGenre = _genreRepo.GetById(id);
                if (getGenre == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    var genreRes = _mapper.Map<GenreRsp>(getGenre);
                    res.setData("200", genreRes);
                }
                
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create([FromBody] GenreReq genre)
        {
            var res = new SingleRsp();
            try
            {
                var newGenre = _mapper.Map<Genre>(genre);
                _genreRepo.Add(newGenre);
                res.setData("200", newGenre);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }
        public SingleRsp Update( GenreReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var updGenre = _genreRepo.GetById(id);
                if (updGenre == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _mapper.Map(res, updGenre);
                    _genreRepo.Update(updGenre);
                    res.setData("200", updGenre);
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
                var genre = _genreRepo.GetById(id);
                if (genre == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _genreRepo.Delete(id);
                    res.SetMessage("Delete successfully");

                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;

        }

        public async Task<SingleRsp> getGenerCompetitionID(int CompetitionID)
        {
            var res = new SingleRsp();
            try
            {
                var genre = await _genreRepo.getGenerCompetitionID(CompetitionID);
                if (genre == null)
                {
                    throw new Exception("No data");
                }
                res.setData("data", genre);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail getGenerCompetitionID ");
            }
            return res;
        }
    }
}
