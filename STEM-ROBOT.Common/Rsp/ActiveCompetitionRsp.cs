using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class ActiveCompetitionRsp
    {
        public bool? isFormat { get; set; }

        public bool? isTeam { get; set; }

        public bool? isLocation { get; set; }

        public bool? isTable { get; set; }

        public bool isTeamMatch { get; set; }

        public bool? isMatch { get; set; }

        public bool? isReferee { get; set; }

        public bool? isSchedule { get; set; }

        public int? formatId { get; set; }

    }
}
