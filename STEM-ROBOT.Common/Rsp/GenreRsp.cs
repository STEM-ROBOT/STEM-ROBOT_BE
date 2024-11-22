using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class GenreRsp
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsTop { get; set; }

        public string HintRule { get; set; }

        public string HintScore { get; set; }
    }

    public class GenerCompetitonID
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public DateTime? registerTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int FormatId { get; set; }
        public int? numberContestantTeam { get; set; }
        public string? Introduce { get; set; }

        public string? ProvinceCode { get; set; }

        public string? AreaCode { get; set; }
        public string? TournamentLevel { get; set; }

    }
}
