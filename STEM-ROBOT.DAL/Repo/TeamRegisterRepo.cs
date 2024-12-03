using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
  
    public class TeamRegisterRepo : GenericRep<TeamRegister>
    {
        public TeamRegisterRepo(StemdbContext context) : base(context) {


        }
        public async Task<List<TeamRegister>> getTeamRegister(int id)
        {
            return await _context.TeamRegisters
                .Where(x => x.CompetitionId == id)
                .Include(s => s.ContestantTeams) 
                .ToListAsync();
        }
        public async Task<List<TeamRegister>> getTeamRegisterTournament(int id)
        {
            return await _context.TeamRegisters
                .Where(x => x.Competition.TournamentId == id)
                .Include (s => s.Competition.Genre)
                .Include(s => s.ContestantTeams)               
                .ToListAsync();
        }
    }
}
