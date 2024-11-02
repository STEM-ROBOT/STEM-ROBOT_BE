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
    public class roundParent
    {
        public List<RoundGame> groups { get; set; } = new List<RoundGame>();
       // public RoundGame knockout { get; set; }

        public bool? IsAsign { get; set; }
    }
    public class RoundGame
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }

        public ICollection<Table> matchrounds { get; set; } = new List<Table>();

    }


    public class Table
    {
        public int Id { get; set; }

        public string? tableName { get; set; }



        public ICollection<TeamMatchRound> matches { get; set; } = new List<TeamMatchRound>();

    }

    public class TeamMatchRound
    {
        public int? Id { get; set; }
        public int? IdMatch { get; set; }
        public string? TeamNameA { get; set; }
        public string? TeamNameB { get; set; }
        public DateTime? date { get; set; }
        public string? filed { get; set; }
        public TimeSpan? time { get; set; }
    }


    // round loại trực tiếp


    // roundknockout
    public class RoundGameKnockoutParent
    {
        public List<RoundGameTeamBye> teams { get; set; } = new List<RoundGameTeamBye>();
        public List<RoundGameKnockout> rounds { get; set; } = new List<RoundGameKnockout>();

    }
    public class RoundGameKnockout
    {
        public int roundId { get; set; }
        public string roundName { get; set; }

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
        public List<RoundGameTable> rounds { get; set; } = new List<RoundGameTable>();
        public List<tableGroup> tableGroup { get; set; } = new List<tableGroup>();
    }

    //table 
    public class tableGroup
    {
        public int team_tableId { get; set; }
        public List<RoundTableTeam> team_table { get; set; } = new List<RoundTableTeam>();
    }
    // list table

    public class RoundGameTable
    {
        public int roundId { get; set; }
        public string roundName { get; set; }
        public List<RoundTable> tables { get; set; } = new List<RoundTable>();

    }
    //
    public class RoundTable
    {
        public int tableId { get; set; }
        public string tableName { get; set; }
       
        public List<RoundGameMatch> matches { get; set; } = new List<RoundGameMatch>();
    }
    public class RoundTableTeam
    {
        public int? teamId { get; set; }
        public string teamName { get; set; }

    }
    public class RoundGameMatch
    {
        public int? matchId { get; set; }

        public List<RoundGameTeamMatch> teamsmatch { get; set; } = new List<RoundGameTeamMatch>();
    }


    public class RoundGameTeamMatch
    {
        public int? teamMatchId { get; set; }
        public int? teamId { get; set; }
        public string? teamName { get; set; }
    }

}
