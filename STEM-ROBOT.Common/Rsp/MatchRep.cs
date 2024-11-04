using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class MatchRep
    {
        public int Id { get; set; }

        public int? RoundId { get; set; }

        public int? TableId { get; set; }

        public DateTime? StartDate { get; set; }

        public string? Status { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }
    }


    //round game
    public class roundTableParentConfig
    {
        public DateTime? startTime { get; set; }
        public bool isMatch { get; set; }
        public GroupRound group { get; set; } = new GroupRound();
        public GroupRound knockout { get; set; } = new GroupRound();
        public ICollection<locationCompetitionConfig> locations { get; set; } = new List<locationCompetitionConfig>();


    }
    public class roundKnocOutParentConfig
    {
        public DateTime? startTime { get; set; }
        public bool isMatch { get; set; }
        public GroupRound knockout { get; set; } = new GroupRound();
        public ICollection<locationCompetitionConfig> locations { get; set; } = new List<locationCompetitionConfig>();
    }
    public class locationCompetitionConfig
    {
        public int? locationId { get; set; }
        public string? locationName { get; set; }
    }
    public class GroupRound
    {
        public ICollection<RoundGroupGame> rounds { get; set; } = new List<RoundGroupGame>();
    }
    public class RoundGroupGame
    {
        public int roundId { get; set; }

        public string? round { get; set; }

        public ICollection<RoundGroupGameMatch> matchrounds { get; set; } = new List<RoundGroupGameMatch>();

    }

    public class RoundGroupGameMatch
    {
        public string? tableName { get; set; }
        public ICollection<TeamMatchRound> matches { get; set; } = new List<TeamMatchRound>();

    }

    public class TeamMatchRound
    {
        public int? matchId { get; set; }
        public string? teamA { get; set; }
        public string? teamB { get; set; }
        public DateTime? date { get; set; }
        public int? locationId { get; set; }
        public TimeSpan? time { get; set; }
    }


    // round loại trực tiếp


    // roundknockout
    public class RoundGameKnockoutParent
    {
        public bool? isTeamMatch { get; set; }
        public List<RoundGameTeamBye> teams { get; set; } = new List<RoundGameTeamBye>();
        public List<RoundGameKnockout> rounds { get; set; } = new List<RoundGameKnockout>();

    }
    public class RoundGameKnockout
    {
        public int? roundId { get; set; }
        public string? roundName { get; set; }

        public List<RoundGameMatch> matches { get; set; } = new List<RoundGameMatch>();

    }
    public class RoundGameTeamBye
    {
        public int? teamId { get; set; }
        public string? name { get; set; }


    }




    // get listround table
    public class RoundParentTable
    {
        public List<tableGroup> tableGroup { get; set; } = new List<tableGroup>();
        public List<RoundGameTable> rounds { get; set; } = new List<RoundGameTable>();
        public bool? isTeamMatch { get; set; }
    }

    //table 
    public class tableGroup
    {
        public int? team_tableId { get; set; }
        public List<RoundTableTeam> team_table { get; set; } = new List<RoundTableTeam>();
    }
    // list table

    public class RoundGameTable
    {
        public int? roundId { get; set; }
        public string? roundName { get; set; }
        public List<RoundTable> tables { get; set; } = new List<RoundTable>();

    }
    //
    public class RoundTableTeam
    {
        public int? teamId { get; set; }
        public string? teamName { get; set; }

    }
    public class RoundTable
    {
        public int? tableId { get; set; }
        public string? tableName { get; set; }

        public List<RoundGameMatch> matches { get; set; } = new List<RoundGameMatch>();
    }

    public class RoundGameMatch
    {
        public int? matchId { get; set; }

        public List<RoundGameTeamMatch> teamMatches { get; set; } = new List<RoundGameTeamMatch>();
    }


    public class RoundGameTeamMatch
    {
        public int? teamMatchId { get; set; }
        public int? teamId { get; set; }
        public string? teamName { get; set; }

    }

}
