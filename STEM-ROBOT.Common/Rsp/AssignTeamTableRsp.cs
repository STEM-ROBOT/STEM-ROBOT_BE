using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class AssignTeamTableRsp
    {
        public List<TableTeamRsp> Teams { get; set; }
        public List<TableRsp> Tables { get; set; }
    }

    public class TableTeamRsp
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
    public class TableRsp
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public List<TableTeamRsp> Teams { get; set; }
    }
}