using AutoMapper;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Mapper
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<Account, Account>().ReverseMap();
            CreateMap<Account, AccountRes>().ReverseMap();
            CreateMap<TournamentFormat, TournamentFormatReq>().ReverseMap();
            CreateMap<Tournament,TournamentReq>().ReverseMap();
            CreateMap<School, SchoolRep>().ReverseMap();
        }
    }
}
