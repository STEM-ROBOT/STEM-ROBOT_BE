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
        {

        }
        public async Task<Competition> GetRoundGameAsync(int competitionId)
        {

            var competitionRefeee = await _context.Competitions
               .Where(s => s.Id == competitionId)
               .Include(sc => sc.RefereeCompetitions)
               .ThenInclude(rcr => rcr.Referee)
               .Include(s => s.Stages).ThenInclude(m => m.Matches).ThenInclude(l=> l.Location)
               .FirstOrDefaultAsync();


            //var roundParent = stages.Select(async comp => new roundParent
            //{
            //    IsAsign = comp.StageTables.FirstOrDefault()?.TableGroup.IsAsign ?? false, // Use null conditional operator
            //    groups = await GetListRoundAsync(comp.Id) // Await asynchronously outside of LINQ
            //}).FirstOrDefault();


            return competitionRefeee;
        }
        public async Task<List<Schedule>> GetRefereeGameAsync(int competitionId)
        {

            var competitionRefeee = await _context.Schedules
               .Where(sr => sr.RefereeCompetition.CompetitionId == competitionId)
               .Include(s => s.RefereeCompetition)
               .ThenInclude(r => r.Referee)
               .ToListAsync();
           

            return competitionRefeee;
        }
        public async Task<Schedule> CheckRefereeCompetition(int scheduleID, int accountID)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID).Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee).Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID).FirstOrDefaultAsync();
        }
        public async Task<Schedule> CheckTimeoutCodeSchedule(int scheduleID, int accountID, string code)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID && x.TimeOut > DateTime.Now && x.OptCode == code)
                .Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee)
                .Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID).FirstOrDefaultAsync();
        }
        public async Task<Schedule> UpdateBusy(int scheduleID, int accountID)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID)
                .Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee)
                .Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID)
                .Include(x => x.Match)
                .FirstOrDefaultAsync();
        }

    }
}
