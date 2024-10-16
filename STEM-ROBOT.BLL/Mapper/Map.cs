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
            CreateMap<Account, AccountReq>().ReverseMap();
            CreateMap<Account, AccountRes>()
                .ForMember(x => x.RoleName, y => y.MapFrom(z => z.Role.Name)).ReverseMap();
            CreateMap<TournamentFormat, TournamentFormatReq>().ReverseMap();
            CreateMap<Tournament,TournamentReq>().ReverseMap();
            CreateMap<Location,LocationReq >().ReverseMap();
            CreateMap<Location, LocationRes>().ReverseMap();
        }
    }
}
