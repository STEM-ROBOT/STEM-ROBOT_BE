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

        public MutipleRsp GetAll()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _genreRepo.All();
                if (lst != null)
                {
                    res.SetSuccess(lst, "200");
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
                var getGenre = _genreRepo.getID(id);
                if (getGenre == null)
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
                var updGenre = _genreRepo.getID(id);
                if (updGenre == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    updGenre = _mapper.Map<Genre>(req);
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
                var genre = _genreRepo.getID(id);
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
