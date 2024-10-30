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

            CreateMap<Account, AccountRsp>() .ReverseMap();

            CreateMap<Account, AccountRsp>().ReverseMap();

            CreateMap<Account, AccountReq>().ReverseMap();

          
            //tournament

           
            CreateMap<TournamentReq, Tournament>()
            .ForMember(dest => dest.Competitions, opt => opt.MapFrom(src => src.competition)) // Ánh xạ danh sách competition
            .ForMember(dest => dest.TournamentLevel, opt => opt.MapFrom(src => src.TournamentLevel))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

            CreateMap<Format, FormatReq>().ReverseMap();

            CreateMap<Tournament,TournamentReq>() .ReverseMap();

            CreateMap<Tournament,TournamentInforRsp>().ReverseMap();

            CreateMap<TournamentComeptition, Competition>()  ;
               
        

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
            CreateMap<Contestant, ContestantInTournament>().ReverseMap();


            //competition
            CreateMap<Competition, CompetitionRep>()
                .ForMember(x => x.NameGenre , op => op.MapFrom( x=> x.Genre.Name))
                .ForMember(x=> x.TournamentName, op=> op.MapFrom(x=> x.Tournament.Name))
                 .ForMember(x => x.ContactPerson, op => op.MapFrom(x => x.Locations.FirstOrDefault().ContactPerson))
                .ForMember(x=> x.Description, op => op.MapFrom(x => x.Genre.Description))
                .ForMember(x => x.Address, op => op.MapFrom(x => x.Tournament.Location))
                .ForMember(x => x.FormatName, op => op.MapFrom(x => x.Format.Name))
                .ReverseMap();

            CreateMap<Competition, CompetitionReq>().ReverseMap();
            CreateMap<Competition, CompetitionConfigReq>().ReverseMap();
            CreateMap<Competition, ListCompetiton>()
                .ForMember(x => x.Name, op => op.MapFrom(x => x.Genre.Name))
                .ForMember(x => x.Image, op => op.MapFrom(x => x.Genre.Image))
                
            //CreateMap<Competition, CompetionCore>()
            //    .ForMember(x => x.Type, op => op.MapFrom(x => x.ScoreCategories.FirstOrDefault().Type))
            //    .ForMember(x => x.ListCore, op => op.MapFrom(x => x.ScoreCategories));



                .ForMember(x => x.Name, op => op.MapFrom(x => x.Genre.Name))
                .ForMember(x => x.Image, op => op.MapFrom(x => x.Genre.Image))
                .ReverseMap();
            //CreateMap<Competition, CompetionCore>()
            //    .ForMember(x => x.Type, op => op.MapFrom(x => x.ScoreCategories.FirstOrDefault().Type))
            //    .ForMember(x => x.ListCore, op => op.MapFrom(x => x.ScoreCategories));

            CreateMap<Competition, ListCompetiton>()

                .ForMember(x => x.Name, op => op.MapFrom(x => x.Genre.Name))
                .ForMember(x => x.Image, op => op.MapFrom(x => x.Genre.Image))
                .ReverseMap();
            //CreateMap<Competition, CompetionCore>()
            //    .ForMember(x => x.Type, op => op.MapFrom(x => x.ScoreCategories.FirstOrDefault().Type))
            //    .ForMember(x => x.ListCore, op => op.MapFrom(x => x.ScoreCategories));

            CreateMap<Competition, CompetitionInforRsp>()
       .ForMember(x => x.TournamentName, op => op.MapFrom(x => x.Tournament != null ? x.Tournament.Name : ""))
       .ForMember(x => x.Location, op => op.MapFrom(x => x.Tournament != null ? x.Tournament.Location : ""))
       .ForMember(x => x.Name, op => op.MapFrom(x => x.Genre != null ? x.Genre.Name : ""))
       .ForMember(x => x.Image, op => op.MapFrom(x => x.Genre != null ? x.Genre.Image : ""))
       .ReverseMap();


           
            //score
            //CreateMap<ScoreCategory, Score>().ReverseMap();
            //stage
            CreateMap<Stage,StageReq>().ReverseMap();
            CreateMap<Stage,StageRep>().ReverseMap();
            //match
            CreateMap<Match, MatchReq>().ReverseMap(); 
            CreateMap<Match,MatchRep>().ReverseMap();
            //tablegroup
            CreateMap<TableGroup, TableGroupReq>().ReverseMap();
            CreateMap<TableGroup,TableGroupRep>().ReverseMap();


            //team
            CreateMap<Team, TeamReq>().ReverseMap();
            CreateMap<Team, TeamRsp>().ReverseMap();

            //action
            CreateMap<Action, ActionReq>().ReverseMap();
            //teammatch
            CreateMap<TeamMatch, TeamMatchReq>().ReverseMap();

            //package
            CreateMap<Package, PackageReq>().ReverseMap();
            CreateMap<Package, PackageRsp>().ReverseMap();

            //action
            CreateMap<Action, ActionReq>().ReverseMap();
            CreateMap<Action, ActionRsp>().ReverseMap();

            //teamtable
            CreateMap<TeamTable, TeamTableReq>().ReverseMap();

        }
    }
}
