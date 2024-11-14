using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamReq
    {
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }
        public List<ContestantTeamReq> Contestants { get; set; }
    }
    public class TeamRegisterReq
    {
        public int? CompetitionId { get; set; }

        public int? AccountId { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public DateTime? RegisterTime { get; set; }

        public string? Email { get; set; }

        public List<ContestantTeamReq> Contestants { get; set; }
    }
    public class ContestantTeamReq
    {
        public int? ContestantId { get; set; }
    }
}
