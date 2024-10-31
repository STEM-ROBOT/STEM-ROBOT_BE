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
    public class FormatSvc
    {
        private readonly FormatRepo _formatSvc;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public FormatSvc(FormatRepo tournamentFormat, IMapper mapper, IConfiguration configuration)
        {
            _formatSvc = tournamentFormat;
            _configuration = configuration;
            _mapper = mapper;
        }

        public MutipleRsp GetFormats()
        {
            var res = new MutipleRsp();
            try
            {
                var lst = _formatSvc.All();
                if (lst == null)
                {
                    res.SetError("404", "No data found");
                }
                var lstRes = _mapper.Map<List<FormatRsp>>(lst);
                res.SetSuccess(lstRes, "200");

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

                var format = _formatSvc.GetById(id);
                if (format == null)

                {
                    res.SetError("404", "No data found");
                }
                var formatRes = _mapper.Map<FormatRsp>(format);
                res.setData("200", formatRes);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Create(FormatReq tournamentFormat)
        {
            var res = new SingleRsp();
            try
            {
                var newFormat = _mapper.Map<CompetitionFormat>(tournamentFormat);
                _formatSvc.Add(newFormat);
                res.setData("200", newFormat);
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }
            return res;
        }

        public SingleRsp Update(FormatReq req, int id)
        {
            var res = new SingleRsp();
            try
            {
                var updFormat = _formatSvc.GetById(id);
                if (updFormat == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _mapper.Map(req, updFormat);
                    _formatSvc.Update(updFormat);
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
                var delFormat = _formatSvc.GetById(id);
                if (delFormat == null)
                {
                    res.SetError("404", "No data found");
                }
                else
                {
                    _formatSvc.Delete(id);
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
