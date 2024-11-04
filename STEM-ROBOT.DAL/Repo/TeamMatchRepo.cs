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
        public async Task<bool> getCompetition(int competitionId)
        {
            var competition = await _context.Competitions.Where(c => c.Id == competitionId).FirstOrDefaultAsync();
            if (competition == null) { 
                return false;
            }
            competition.IsTeamMacth = true;
            _context.SaveChanges();
            return true;

        }
    }
}
