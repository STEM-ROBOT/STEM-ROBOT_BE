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
    public class CompetitionRepo : GenericRep<Competition>
    {
        public CompetitionRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<List<Competition>> getListCompetition()
        {
            return await _context.Competitions.Include(x => x.Locations)
                .Include(x => x.Genre).ToListAsync();
        }
        public async Task<List<Competition>> getListCompetitionbyID(int id)
        {
            return await _context.Competitions.Where(x=> x.Id == id).Include(x => x.Locations)
                .Include(x => x.Genre).ToListAsync();
        }
        public async Task<List<Team>> GetTeamsByCompetitionId(int competitionId)
        {
            return await _context.Competitions
                          .Where(x => x.Id == competitionId)
                          .Include(x => x.Teams)  
                          .SelectMany(x => x.Teams)  
                          .ToListAsync();
        }
    
        public async Task<IEnumerable<Competition>> getListScoreCompetition(int competitionID)
        {
            return await  _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.ScoreCategories).ToListAsync();
        }
        public async Task<IEnumerable<ListPlayer>> getListPlayer()
        {
            var listplayer = await _context.Competitions
                .Select(t => new ListPlayer
                {
                    Id = t.Id,
                    Name = t.Genre.Name,
                    played = t.Teams.SelectMany(x => x.TeamMatches).Count(x => x.IsPlay == true),
                    win = t.Teams.SelectMany(x => x.TeamMatches).Count(x => x.ResultPlay == "Draw"),
                    draw = t.Teams.SelectMany(x => x.TeamMatches).Count(x => x.ResultPlay == "Win"),
                    lost = t.Teams.SelectMany(x => x.TeamMatches).Count(x => x.ResultPlay == "Lose"),
                    members = t.Teams.SelectMany(x => x.ContestantTeams).Select(v => new MemeberPlayer
                    {
                       Id = v.Contestant.Id,
                       Name = v.Contestant.Name,
                       avatar = v.Contestant.Avatar


                    }).ToList()

                }).ToListAsync();
            return listplayer;
        }
    }
}
