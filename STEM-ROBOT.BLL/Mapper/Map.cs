using AutoMapper;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = STEM_ROBOT.DAL.Models.Action;

namespace STEM_ROBOT.BLL.Mapper
{
    public class Map : Profile
    {
        public Map()
        {

            CreateMap<Account, Account>().ReverseMap();
            CreateMap<Account, AccountRsp>()
             .ForMember(x => x.RoleName, op => op.MapFrom(x => x.Role.Name)).ReverseMap();
            CreateMap<Account, AccountReq>().ReverseMap();

          
            //tournament
            CreateMap<TournamentFormat, TournamentFormatReq>().ReverseMap();
            CreateMap<Tournament,TournamentReq>().ReverseMap();

            //location
            CreateMap<Location,LocationReq >().ReverseMap();
            CreateMap<Location, LocationRsp>().ReverseMap();

            //school
            CreateMap<School, SchoolReq>().ReverseMap();
            CreateMap<Referee, RefereeReq>().ReverseMap();
            CreateMap<Referee, RefereeRsp>().ReverseMap();
            CreateMap<ScoreCategory, ScoreCategoryReq>().ReverseMap();
            CreateMap<ScoreCategory, ScoreCategoryRsp>().ReverseMap();

            //contestant
            CreateMap<Contestant, ContestantRep>().ReverseMap();
            CreateMap<Contestant,ContestantReq>().ReverseMap();

            //competition
            CreateMap<Competition, CompetitionRep>()
                .ForMember(x => x.NameGenre , op => op.MapFrom( x=> x.Genre.Name))
                .ForMember(x=> x.TournamentName, op=> op.MapFrom(x=> x.Tournament.Name))
                .ForMember(x=> x.Address, op => op.MapFrom(x=> x.Locations.FirstOrDefault().Address))
                 .ForMember(x => x.ContactPerson, op => op.MapFrom(x => x.Locations.FirstOrDefault().ContactPerson))
                .ForMember(x=> x.Description, op => op.MapFrom(x => x.Genre.Description))
                .ForMember(x=> x.Image, op => op.MapFrom(x => x.Genre.Image))
                .ReverseMap();

            //team
            CreateMap<Team, TeamReq>().ReverseMap();
            CreateMap<Team, TeamRsp>().ReverseMap();

            //action
            CreateMap<Action, ActionReq>().ReverseMap();
        }
    }
}
