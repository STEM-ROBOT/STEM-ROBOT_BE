using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class ScheduleRepo : GenericRep<Schedule>
    {
        public ScheduleRepo(StemdbContext context) : base(context)
        { }
        public async Task<Schedule> CheckRefereeCompetition(int scheduleID, int accountID)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID).Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee).Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID).FirstOrDefaultAsync();
        }
        public async Task<Schedule> CheckTimeoutCodeSchedule(int scheduleID, int accountID,string code)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID && x.TimeOut > DateTime.Now && x.OptCode == code)
                .Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee)
                .Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID).FirstOrDefaultAsync();
        }
    }
}
