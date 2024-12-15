using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class TeamMatchRepo : GenericRep<TeamMatch>
    {
        public TeamMatchRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<Competition> getCompetition(int competitionId)
        {
            var competition = await _context.Competitions.Where(c => c.Id == competitionId).FirstOrDefaultAsync();

            return competition;

        }
        public async Task<List<Models.Action>> getAverageScore(int teamId,int matchId)
        {
            var competition = await _context.Actions.Where(c => c.TeamMatch.MatchId == matchId && c.TeamMatch.TeamId == teamId && c.Status == "accept").Include(s=>s.ScoreCategory).ToListAsync();

            return competition;

        }
    }
}
