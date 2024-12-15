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
    public class TeamRepo : GenericRep<Team>
    {
        public TeamRepo(StemdbContext context) : base(context)
        {
        }

        public List<Team> GetTeamsByCompetition(int id)
        {
            return _context.Teams
        .Where(x => x.CompetitionId == id)
        .Include(c => c.Competition)
        .Include(c => c.ContestantTeams)
        .ThenInclude(ct => ct.Contestant) 
        .ToList();
        }

        public async Task<TeamMatch> GetTeamWin(int competitionId)
        {
        //    var teamWinMatch = await  _context.Competitions
        //.Where(c => c.Id == competitionId)
        //.Include(c => c.Stages)
        //    .ThenInclude(s => s.Matches)
        //        .ThenInclude(m => m.TeamMatches)
        //            .ThenInclude(tm => tm.Team)
        //.SelectMany(c => c.Stages
        //    .Where(s => s.Name == "CK")
        //    .SelectMany(s => s.Matches
        //        .SelectMany(m => m.TeamMatches)
        //        .Where(tm => tm.ResultPlay == "Win")))
        //.OrderByDescending(tm => tm.Match.Id)
        //.FirstOrDefaultAsync();           
        //    if (teamWinMatch == null)
        //    {
        //        return null; 
        //    }

           
        //    return new TeamWinCompetition
        //    {
        //        img = teamWinMatch.Team.Image,
        //        name = teamWinMatch.Team.Name 
        //    };
        var team = _context.TeamMatches.Where(x=> x.Id == competitionId).FirstOrDefault();
            return team;
        }
    }
}
