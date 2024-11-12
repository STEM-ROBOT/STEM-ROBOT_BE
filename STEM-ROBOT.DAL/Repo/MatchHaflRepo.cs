using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class MatchHaflRepo : GenericRep<MatchHalf>
    {
        public MatchHaflRepo(StemdbContext context) : base(context)
        {
        }

        public async Task<List<MatchPoint>> ListHaftMatch(int matchID)
        {
            var matchData = await _context.Matches
                .Where(m => m.Id == matchID)
                .Select(m => new
                {
                    MatchTimeIn = m.TimeIn,
                    Teams = m.TeamMatches.OrderBy(tm => tm.Id).Take(2).ToList(),
                    Halves = m.MatchHalves.Select(h => new
                    {
                        HalfId = h.Id,
                        Actions = h.Actions.Where(x=> x.Status.ToLower() == "accept").Select(a => new
                        {
                            TeamMatchId = a.TeamMatchId,
                            TeamName = a.TeamMatch.NameDefault,
                            ScoreCategoryType = a.ScoreCategory.Type,
                            ScoreCategoryDescription = a.ScoreCategory.Description,
                            Score = a.Score ?? 0,
                            EventTime = a.EventTime
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (matchData == null || matchData.Teams.Count != 2)
            {
                throw new InvalidOperationException("The match does not exist or does not have exactly two teams.");
            }

            var team1 = matchData.Teams[0].Id;
            var team2 = matchData.Teams[1].Id;

            var matchPoints = matchData.Halves.Select(h => new MatchPoint
            {
                haftMatch = h.HalfId,
                activity = new TeamAcctivity
                {
                    activityTeam1 = h.Actions
                        .Where(a => a.TeamMatchId == team1 )
                        .Select(a => new TeamActivity1
                        {
                            teamName = a.TeamName,
                            type = a.ScoreCategoryType,
                            description = a.ScoreCategoryDescription,
                            point = a.Score,
                            timeScore = CalculateElapsedMinutesAndSeconds(matchData.MatchTimeIn, a.EventTime)
                        }).ToList(),

                    activityTeam2 = h.Actions
                        .Where(a => a.TeamMatchId == team2)
                        .Select(a => new TeamActivity2
                        {
                            teamName = a.TeamName,
                            type = a.ScoreCategoryType,
                            description = a.ScoreCategoryDescription,
                            point = a.Score,
                            timeScore = CalculateElapsedMinutesAndSeconds(matchData.MatchTimeIn, a.EventTime)
                        }).ToList()
                }
            }).ToList();

            return matchPoints;
        }





        //hàm tính thời gian trả về action 
        public static string CalculateElapsedMinutesAndSeconds(TimeSpan? timeIn, TimeSpan? eventTime)
        {
            if (timeIn == null || eventTime == null)
            {
                throw new ArgumentException("TimeIn and EventTime must not be null.");
            }


            var absoluteEventTime = eventTime - timeIn;

           
            if (eventTime < timeIn)
            {
                throw new InvalidOperationException("EventTime is beyond the end of the match.");
            }

            // Extract minutes and seconds from eventTime relative to timeIn
            int minutes = eventTime.Value.Minutes;
            int seconds = eventTime.Value.Seconds;

            
            return $"{minutes:D2}:{seconds:D2}";
        }


    }
}
