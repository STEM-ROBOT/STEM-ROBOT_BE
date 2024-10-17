using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class ContestantRep
    {
        public int Id { get; set; }

        public int? SchoolId { get; set; }

        public int? TournamentId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Status { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Image { get; set; }
    }
}
