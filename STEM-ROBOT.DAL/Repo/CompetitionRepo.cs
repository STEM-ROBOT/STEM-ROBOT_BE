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
    }
}
