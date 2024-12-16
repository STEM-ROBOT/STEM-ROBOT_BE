using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.Rsp;
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
               .Include(s => s.Stages).ThenInclude(m => m.Matches).ThenInclude(l => l.Location)
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
            return await _context.Schedules.Where(x => x.Id == scheduleID)
                .Include(x => x.RefereeCompetition)
                .ThenInclude(x => x.Referee)
                .Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID)
                .FirstOrDefaultAsync();
        }
        public async Task<RefereeCompetition> CheckSupRefereeCompetition(int refereeCompetitionId, int accountID)
        {
            return await _context.RefereeCompetitions.Where(x => x.Id == refereeCompetitionId && x.Referee.AccountId == accountID).FirstOrDefaultAsync();
        }
        public async Task<Schedule> SupRefereeCheck(int schedule)
        {
            return await _context.Schedules.Where(x => x.Id == schedule && x.IsJoin == true)
                .Include(m => m.Match)
                .Include(rc => rc.RefereeCompetition)

                .FirstOrDefaultAsync();

        }
        public async Task<Match> SupRefereeCompetitionMatchInfo(int matchId)
        {
            return await _context.Matches.Where(x => x.Id == matchId).Include(tm => tm.TeamMatches).ThenInclude(t => t.Team).Include(h => h.MatchHalves).FirstOrDefaultAsync();
        }
        public async Task<List<ScoreCategory>> CompetitionScore(int competitionId)
        {
            return await _context.ScoreCategories.Where(x => x.CompetitionId == competitionId).ToListAsync();
        }
        public async Task<List<Location>> CheckLocationCompetition(int competitionId)
        {
            return await _context.Locations.Where(x => x.CompetitionId == competitionId).ToListAsync();
        }
        public async Task<List<Schedule>> ScheduleSupRefereeCompetition(int refereeCompetitionId)
        {
            return await _context.Schedules
                .Where(x => x.RefereeCompetitionId == refereeCompetitionId)
                .Include(m => m.Match)
                .ThenInclude(tm => tm.TeamMatches)
                .ThenInclude(t => t.Team).ToListAsync();
        }
        public async Task<Schedule> CheckTimeoutCodeSchedule(int scheduleID, int accountId)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID)
                .Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee)
               .FirstOrDefaultAsync();
        }
        public async Task<Schedule> UpdateBusy(int scheduleID, int accountID)
        {
            return await _context.Schedules.Where(x => x.Id == scheduleID)
                .Include(x => x.RefereeCompetition).ThenInclude(x => x.Referee).ThenInclude(x => x.Account)
                .Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountID)
                .Include(x => x.Match)
                    .FirstOrDefaultAsync();
        }

        public async Task<Account> getEmail(int schdeDuleID)
        {
            return await _context.Accounts
         .Include(x => x.Tournaments)
             .ThenInclude(t => t.Competitions)
             .ThenInclude(c => c.Stages)
             .ThenInclude(s => s.Matches)
             .ThenInclude(m => m.Schedules)
         .Where(a => a.Tournaments
             .Any(t => t.Competitions
                 .Any(c => c.Stages
                     .Any(s => s.Matches
                         .Any(m => m.Schedules
                             .Any(sch => sch.Id == schdeDuleID))))))
         .FirstOrDefaultAsync();
        }
        public async Task<Schedule> checkTimeschedule(int scheduleID, int accountId)
        {
            var timecheck = await _context.Schedules.Where(x => x.Id == scheduleID && x.RefereeCompetition.Referee.AccountId == accountId).Include(m => m.Match).ThenInclude(mt => mt.TeamMatches).FirstOrDefaultAsync();
            return timecheck;
        }

        public async Task<Schedule> confirmSchedule(int scheduleId, int accoutId)
        {
            return await _context.Schedules
                .Where(x => x.Id == scheduleId && x.RefereeCompetition.Referee.AccountId == accoutId)
                 .Include(x => x.Match)
            .ThenInclude(m => m.TeamMatches)
                .ThenInclude(tm => tm.Team)
                .Include(x => x.Match)
                .ThenInclude(x => x.TeamMatches)
                .ThenInclude(ac => ac.Actions.Where(a=>a.Status == "accept"))
                .ThenInclude(cs => cs.ScoreCategory)
               .Include(x => x.Match)
               .ThenInclude(s => s.Stage)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Match>> checkMatchLast(int tableGroupId)
        {
            var matchLast = await _context.Matches.Where(x => x.TableGroupId == tableGroupId).ToListAsync();

            return matchLast;
        }
        public async Task<TableGroup> checkTableMatch(int tableGroupId)
        {
            return await _context.TableGroups.Where(x => x.Id == tableGroupId)
                .Include(tb => tb.TeamTables)
                .ThenInclude(t => t.Team)
                .ThenInclude(tm => tm.TeamMatches)
                .ThenInclude(ac => ac.Actions )
                .ThenInclude(sc=>sc.ScoreCategory)
                .FirstOrDefaultAsync();
        }
        public async Task<TeamMatch> matchWinSchedule(string matchCode)
        {
            return await _context.TeamMatches.Where(x => x.MatchWinCode == matchCode).FirstOrDefaultAsync();
        }
    }
}
