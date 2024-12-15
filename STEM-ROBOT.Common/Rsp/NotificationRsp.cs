using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class NotificationRsp
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public bool? Status { get; set; }

        public int? AccountId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? RouterUi { get; set; }

    }

    public class CheckTimeSchedule
    {
        public int matchId { get; set; }
        public int teamMatchId { get; set; }
    }
}
