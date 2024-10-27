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
    public class TournamentRepo : GenericRep<Tournament>
    {
        public TournamentRepo(StemdbContext context) : base(context)
        {
        }

        public async Task<List<TournamentRep>> GetListTournament()
        {
            var tournament = await _context.Tournaments.Select(t => new TournamentRep
            {
                Id = t.Id,
                Name = t.Name,
                Location = t.Location,
                Image = t.Image,
                contestant = t.Contestants.Count(x => x.TournamentId == x.Id),
                views = null,
                competitionNumber = t.Competitions.Where(x => x.IsActive == true).Count(x => x.TournamentId == x.Id),
                competitionActivateNumber = t.Competitions.Where(x => x.IsActive == true).Count(x => x.TournamentId == x.Id),
                imagesCompetition = t.Competitions.Select(g => new ImageCompetition {imageCompetition = g.Genre.Image }).ToList(),


            }).ToListAsync();
            return tournament;
        }
    }
}
