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

        public SingleRsp GetGenres()
        {
            var res = new SingleRsp();
            try
            {
                var lst = _genreRepo.All();
                if (lst != null)
                {
                    var lstRsp = _mapper.Map<List<GenreRsp>>(lst);
                    res.setData("data",lstRsp);
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
                    res.setData("data", genreRes);
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
                res.setData("data", newGenre);
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
                    res.setData("data", updGenre);
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

     
    }
}
