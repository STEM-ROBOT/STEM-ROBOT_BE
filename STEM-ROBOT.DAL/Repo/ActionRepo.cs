
using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.Rsp;
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
            var timecheck = await _context.Schedules.Where(x => x.Id == scheduleId).FirstOrDefaultAsync();
            return timecheck;
        }

        public async Task<List<ActionsRefereeSupRsp>> ActionByRefereeSup(int matchId, int refereeCompetitionId)
        {
            var data = await _context.Actions.Where(ac => ac.RefereeCompetitionId == refereeCompetitionId).Select(a => new ActionsRefereeSupRsp
            {
                Id = a.Id,
                EventTime = CalculateElapsedMinutesAndSeconds(a.EventTime),
                RefereeCompetitionId = a.RefereeCompetitionId,
                ScoreCategoryDescription = a.ScoreCategory.Description,
                ScoreCategoryId = a.ScoreCategoryId,
                ScoreCategoryPoint = a.ScoreCategory.Point,
                ScoreCategoryType = a.ScoreCategory.Type,
                Status = a.Status,
                HaftName = a.MatchHalf.HalfName,
                TeamMatchId = a.TeamMatchId,
                TeamName = a.TeamMatch.Team.Name,
                TeamLogo = a.TeamMatch.Team.Image,
            }).ToListAsync();

            return data;
        }
        public static string CalculateElapsedMinutesAndSeconds(TimeSpan? eventTime)
        {

            // Extract minutes and seconds from eventTime relative to timeIn
            int minutes = eventTime.Value.Minutes;
            int seconds = eventTime.Value.Seconds;


            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
