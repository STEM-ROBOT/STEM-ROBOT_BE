using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class TeamTableReq
    {
        public int TeamId { get; set; }
        public int TableGroupId { get; set; }
    }

    public class TableAssignmentReq
    {
        public int TableGroupId { get; set; }
        public List<int> Teams { get; set; }
    }

}
