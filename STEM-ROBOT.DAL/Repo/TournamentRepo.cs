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

        public async Task<TournamentListRep> GetListTournament(string? name = null, string? provinceCode = null, string? status = null, int? GenerId = null, int page = 1, int pageSize = 10)
        {

            var query = _context.Tournaments.AsQueryable();

            

            // Tính tổng số trang

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (GenerId.HasValue)
            {
                query = query.Where(t => t.Competitions.Any(c => c.GenreId == GenerId));
            }
            if (!string.IsNullOrEmpty(provinceCode))
            {
                query = query.Where(t => t.ProvinceCode == provinceCode || t.ProvinceCode == null);
            }
            int totalItems = await query.CountAsync();
            int skip = (page - 1) * pageSize;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var tournament = await query
              .OrderByDescending(t => t.CreateDate) // Sắp xếp theo CreateDate giảm dần (mới nhất trước)
              .Skip(skip)
              .Take(pageSize)
              .Select(t => new TournamentRep
              {
                  Id = t.Id,
                  Name = t.Name,
                  Location = t.Location,
                  Image = t.Image,
                  contestant = t.Contestants.Count(),
                  CreateDate = t.CreateDate,
                  Introduce = t.Introduce,
                  views = t.ViewTournament,
                  Status = t.Status,
                  competitionNumber = t.Competitions.Count(),
                  competitionActivateNumber = t.Competitions.Count(c => c.IsActive == true),
                  imagesCompetition = t.Competitions.Select(g => new ImageCompetition { imageCompetition = g.Genre.Image }).ToList(),
              }).ToListAsync();
            ;
            var resData = new TournamentListRep
            {
                tournamentRep = tournament,
                totalPages = totalPages,
            };
            return resData;
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
                    Image = x.Image,
                    Location = x.Location,
                    IsActive = x.Competitions.Count(x => x.IsActive == true)


                })
            .ToListAsync();
            return listTournamentModerator;
        }

        public async Task<int> CountTournament()
        {
            return await _context.Tournaments.CountAsync(x => x.Status == "Public");
        }
        public async Task<Tournament> TournamentById(int tournamentId)
        {
            return await _context.Tournaments.Where(t => t.Id == tournamentId).Include(c => c.Contestants).Include(cp=> cp.Competitions).FirstOrDefaultAsync();
        }
    }
}
