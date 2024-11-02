using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class TeamTableRsp
    {
        List<Teams> Teams { get; set; }
        public List<Tables> Tables { get; set; }
        public bool isTable { get; set; }
    }
    public class Tables
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public List<Teams> Teams { get; set; }
    }
    public class Teams
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
   

}
