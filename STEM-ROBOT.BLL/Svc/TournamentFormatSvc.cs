using AutoMapper;
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
    public class TournamentFormatSvc
    {
        private readonly TournamentFormatRepo _tournamentFormatSvc;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TournamentFormatSvc(TournamentFormatRepo tournamentFormat, IMapper mapper, IConfiguration configuration)
        {
            _tournamentFormatSvc = tournamentFormat;
            _configuration = configuration;
            _mapper = mapper;
        }

        public MutipleRsp GetAll()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _tournamentFormatSvc.All();
                if(lst == null)
                {
                    res.SetError("404", "No data found");
                }
                res.SetSuccess(lst, "200");

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

                var format = _tournamentFormatSvc.getID(id);
                if(format == null)

                {
                    res.SetError("404", "No data found");
                }
                res.setData("200", format);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(TournamentFormatReq tournamentFormat)
        {
            var res = new SingleRsp();
            try
            {
                var newFormat = _mapper.Map<TournamentFormat>(tournamentFormat);
                _tournamentFormatSvc.Add(newFormat);
                res.setData("200", newFormat);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(TournamentFormatReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var updFormat = _tournamentFormatSvc.getID(id);
                if (updFormat == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    updFormat = _mapper.Map<TournamentFormat>(req);
                    _tournamentFormatSvc.Update(updFormat);
                    res.setData("200", updFormat);
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
                var delFormat = _tournamentFormatSvc.getID(id);
                if (delFormat == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _tournamentFormatSvc.Delete(id);
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
