using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class CompetitionRep
    {
        public int Id { get; set; }

        public int? TournamentId { get; set; }

        public string TournamentName { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public int? GenreId { get; set; }

        public string? NameGenre { get; set; }

        public string? Address { get; set; }

        public string? ContactPerson { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? FormatId { get; set; }

        public string? FormatName { get; set; }

        public int? NumberView { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Mode { get; set; }

        public int? NumberTeam { get; set; }

        public int? NumberTeamNextRound { get; set; }

        public int? NumberTable { get; set; }

        public int? WinScore { get; set; }

        public int? LoseScore { get; set; }

        public int? TieScore { get; set; }

        public int? NumberSubReferee { get; set; }

        public int? NumberTeamReferee { get; set; }

        public TimeSpan? TimeOfMatch { get; set; }

        public TimeSpan? TimeBreak { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? TimeStartPlay { get; set; }

        public DateTime? TimeEndPlay { get; set; }
    }

    public class ListCompetiton
    {
        public int Id { get; set; }

        public string? Image { get; set; }

        public string? Name { get; set; }

        public DateTime? RegisterTime { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Mode { get; set; }

        public bool? IsActive { get; set; }

    }

    public class CompetionCore
    {
        public string? Regulation { get; set; }

        public ICollection<ScoreCompetition> scoreCompetition { get; set; } = new List<ScoreCompetition>();

    }
    public class ScoreCompetition
    {
        public string? Type { get; set; }
        public ICollection<ScoreList> score { get; set; } = new List<ScoreList>();
    }

    public class ScoreList
    {

        public int Id { get; set; }

        public string? Description { get; set; }

        public int? Point { get; set; }

    }
    public class ListPlayer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public int played { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lost { get; set; }

        public ICollection<MemeberPlayer> members { get; set; } = new List<MemeberPlayer>();

    }
    public class MemeberPlayer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? avatar { get; set; }
    }

    public class CompetitionInforRsp
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public int? NumberTeam { get; set; }
        public string? TournamentName { get; set; }
        public string? Status { get; set; }
        public bool? isActive { get; set; }
    }

    //model reponse view "vonfdaubang"
    public class MatchGroupStageCompetition
    {
        public string? groupName { get; set; }
        public ICollection<MatchGroupStageRound> round { get; set; } = new List<MatchGroupStageRound>();
    }
    public class MatchGroupStageRound
    {
        public string? roundNumber { get; set; }
        public ICollection<MatchRoundViewRsp> matches { get; set; } = new List<MatchRoundViewRsp>();

    }
    //model reponse view "lichthidau"
    public class MatchScheduleCompetition
    {
        public string? round { get; set; }
        public ICollection<MatchRoundViewRsp> matches { get; set; } = new List<MatchRoundViewRsp>();
    }

    public class MatchRoundViewRsp
    {
        public int? matchId { get; set; }
        //team tham gia tran dau
        public string? homeTeam { get; set; }
        public string? homeTeamLogo { get; set; }
        public string? awayTeamLogo { get; set; }
        public string? awayTeam { get; set; }
        //ti so tran dau
        public int? homeScore { get; set; }
        public int? awayScore { get; set; }
        //thoi gian, dia diem   
        public DateTime? startTime { get; set; }
        public string? locationName { get; set; }
    }

}
