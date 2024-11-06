using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TeamRsp
    {
        public int? Id { get; set; }

        public int? CompetitionId { get; set; }

        public int? ContestantInTeam { get; set;}

        public string? Name { get; set; }

        public string? Status { get; set; }

        public int? TableId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        public List<Constestant> member { get; set; }
    }

    public class Constestant
    {
        public int? ContestantId { get; set; }

        public string? ContestantName { get; set; }
    }

    public class ListTeamRspByTournament
    {
        public int? Id { get; set; }

        public int? CompetitionId { get; set; }


        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }
    }
}
