
using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = STEM_ROBOT.DAL.Models.Action;

namespace STEM_ROBOT.DAL.Repo
{
    public class ActionRepo : GenericRep<Action>
    {
        public ActionRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<Schedule> checkRefereeschedule(int scheduleId, int accoutId)
        {
            var timecheck = await _context.Schedules.Where(x => x.Id == scheduleId && x.RefereeCompetition.Referee.AccountId == accoutId).FirstOrDefaultAsync();
            return timecheck;
        }
    }
}
