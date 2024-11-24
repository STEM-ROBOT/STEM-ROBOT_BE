using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TeamRegisterRsp
    {
        public int Id { get; set; }

        public int? TeamId { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactInfo { get; set; }

        public string? Image { get; set; }

        public string? Status { get; set; }

        public DateTime? RegisterTime { get; set; }

        public string? Email { get; set; }

        public int ? Member { get; set; }
    }
    public class TeamRegisterTournamentRsp
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? contactPhone { get; set; }

        public string? contactPerson { get; set; }

        public string? competition { get; set; }

        public string? Status { get; set; }

        public DateTime? RegisterTime { get; set; }

        public int? Members { get; set; }
    }
}
