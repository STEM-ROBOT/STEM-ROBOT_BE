using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class MatchReq
    {
      
        public int? RoundId { get; set; }

        public int? TableId { get; set; }

        public DateTime? StartDate { get; set; }

        public string? Status { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }
    }
    public class MatchConfigReq
    {
        public TimeSpan? TimeOfMatch { get; set; }

        public TimeSpan? TimeBreak { get; set; }


        public TimeSpan? TimeStartPlay { get; set; }

        public TimeSpan? TimeEndPlay { get; set; }

        public DateTime? startTime { get; set; }

        public ICollection<MatchDataTimeReq> matchs { get; set; } = new List<MatchDataTimeReq>();
    }

    public class MatchDataTimeReq
    {

        public int? id { get; set; }

        public DateTime? startDate { get; set; }

        public int? locationId { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }
    }
}
