﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{
    public class DataAssignTeamTableRsp
    {
        public List<DataTeamRsp> Teams { get; set; }
        public List<DataTableRsp> Tables { get; set; }
    }

    public class DataTeamRsp
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
    public class DataTableRsp
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public List<DataTeamRsp> Teams { get; set; }
    }
}