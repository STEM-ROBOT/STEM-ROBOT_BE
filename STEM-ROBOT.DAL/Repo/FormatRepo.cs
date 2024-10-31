using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class FormatRepo : GenericRep<CompetitionFormat>
    {

        public FormatRepo(StemdbContext context) : base(context)
        {
        }
    }
}
