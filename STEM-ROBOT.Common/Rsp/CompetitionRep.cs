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

        public string TournamentName {  get; set; }
 
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Name { get; set; }

        public string? Status { get; set; }


        public string? Address { get; set; }

        public string? ContactPerson { get; set; }

       
        public string? NameGenre { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}
