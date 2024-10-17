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
}
