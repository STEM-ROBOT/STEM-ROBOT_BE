using Microsoft.EntityFrameworkCore;
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

    }
}
