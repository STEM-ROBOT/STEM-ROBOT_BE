using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class TeamRepo : GenericRep<Team>
    {
        public TeamRepo(StemdbContext context) : base(context)
        {
        }
    }
}
