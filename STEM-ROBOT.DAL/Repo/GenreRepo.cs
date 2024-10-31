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
    public class GenreRepo : GenericRep<Genre>

    {
        public GenreRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<GenerCompetitonID> getGenerCompetitionID(int competitionID)
        {
            return await (from competition in _context.Competitions
                          where competition.Id == competitionID
                          join genre in _context.Genres on competition.GenreId equals genre.Id
                          select new GenerCompetitonID
                          {
                              id = competition.Id,
                              name = genre.Name,
                              status = competition.Status,
                              image = genre.Image,
                              registerTime = competition.StartTime ?? DateTime.UtcNow, 
                              numberContestantTeam = competition.NumberContestantTeam ?? 0 
                          }).FirstOrDefaultAsync();
        }
    }
}
