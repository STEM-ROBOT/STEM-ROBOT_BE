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
    public class TableAssignmentReq {
     
        public ICollection<TableAssign> tableAssign { get; set; } =new List<TableAssign>();
    }

    public class TableAssign
    {
        public int TableGroupId { get; set; }
        public int TeamNextRound { get; set; }
        public string TableGroupName { get; set; }
        public List<int> Teams { get; set; }
    }

}
