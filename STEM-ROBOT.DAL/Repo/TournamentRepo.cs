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

        public async Task<List<TournamentRep>> GetListTournament(string? name = null, string? status = null, int? competitionId = null, int page = 1, int pageSize = 10)
        {

            var query = _context.Tournaments.AsQueryable();


            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (competitionId.HasValue)
            {
                query = query.Where(t => t.Competitions.Any(c => c.Id == competitionId));
            }

            int skip = (page - 1) * pageSize;


            var tournament = await query
                .OrderBy(t => t.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(t => new TournamentRep
                {
                    Id = t.Id,
                    Name = t.Name,
                    Location = t.Location,
                    Image = t.Image,
                    contestant = t.Contestants.Count(),
                    views = null,
                    competitionNumber = t.Competitions.Count(c => c.TournamentId == t.Id),
                    competitionActivateNumber = t.Competitions.Count(c => c.IsActive == true && c.TournamentId == t.Id),
                    imagesCompetition = t.Competitions.Select(g => new ImageCompetition { imageCompetition = g.Genre.Image }).ToList(),
                }).ToListAsync();

            return tournament;
        }

        public async Task<IEnumerable<TournamentModerator>> getTournamentModerator(int userId)
        {
            var listTournamentModerator = await _context.Tournaments.Where(x => x.AccountId == userId)
                .Select(x => new TournamentModerator
                {
                    Id = x.Id,
                    Name = x.Name,
                    genre = x.Competitions.Select(x => x.Genre.Name).Distinct().Count(),
                    Status = x.Status,
                    Image =x.Image,
                    Location = x.Location,
                    IsActive = x.Competitions.Count(x=> x.IsActive == true)


                })
            .ToListAsync();
            return listTournamentModerator;
        }

        public async Task<int> CountTournament()
        {
            return await _context.Tournaments.CountAsync(x=> x.Status == "Public");
        }
    }
}
