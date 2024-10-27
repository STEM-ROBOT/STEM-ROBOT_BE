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

            CreateMap<Tournament,TournamentReq>().ReverseMap();


        
            CreateMap<TournamentComeptition, Competition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.RegisterTime, opt => opt.MapFrom(src => src.RegisterTime))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Regulation, opt => opt.MapFrom(src => src.Regulation))
                .ForMember(dest => dest.NumberContestantTeam, opt => opt.MapFrom(src => src.NumberContestantTeam))
                .ForMember(dest => dest.IsTop, opt => opt.MapFrom(src => src.IsTop))
                .ForMember(dest => dest.NumberView, opt => opt.MapFrom(src => src.NumberView))
                .ForMember(dest => dest.FormatId, opt => opt.MapFrom(src => src.FormatId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Mode, opt => opt.MapFrom(src => src.Mode))
                .ForMember(dest => dest.NumberTeam, opt => opt.MapFrom(src => src.NumberTeam))
                .ForMember(dest => dest.NumberTeamNextRound, opt => opt.MapFrom(src => src.NumberTeamNextRound))
                .ForMember(dest => dest.NumberTable, opt => opt.MapFrom(src => src.NumberTable))
                .ForMember(dest => dest.WinScore, opt => opt.MapFrom(src => src.WinScore))
                .ForMember(dest => dest.LoseScore, opt => opt.MapFrom(src => src.LoseScore))
                .ForMember(dest => dest.TieScore, opt => opt.MapFrom(src => src.TieScore))
                .ForMember(dest => dest.NumberSubReferee, opt => opt.MapFrom(src => src.NumberSubReferee))
                .ForMember(dest => dest.NumberTeamReferee, opt => opt.MapFrom(src => src.NumberTeamReferee))
                .ForMember(dest => dest.TimeOfMatch, opt => opt.MapFrom(src => src.TimeOfMatch))
                .ForMember(dest => dest.TimeBreak, opt => opt.MapFrom(src => src.TimeBreak))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.TimeStartPlay, opt => opt.MapFrom(src => src.TimeStartPlay))
                .ForMember(dest => dest.TimeEndPlay, opt => opt.MapFrom(src => src.TimeEndPlay));
        

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

            CreateMap<Competition, CompetitionReq>().ReverseMap();
            CreateMap<Competition, CompetitionConfigReq>().ReverseMap();
            CreateMap<Competition, ListCompetiton>().ReverseMap();
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
        }
    }
}
