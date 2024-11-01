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
        public async Task<Competition> getCompetition(int id)
        {
            return await _context.Competitions.Where(x=>x.Id == id).Include(t=>t.Tournament).Include(g=>g.Genre).FirstOrDefaultAsync();
        }
        public async Task<List<Competition>> getListCompetitionGener(int idTournament)
        {
            return await _context.Competitions.Where(x=> x.TournamentId == idTournament).Include(x => x.Genre).ToListAsync();
        }
        public async Task<List<Competition>> getListCompetitionbyID(int id)
        {
            return await _context.Competitions.Where(x => x.Id == id).Include(x => x.Locations)
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

        public async Task<CompetionCore> getListScoreCompetition(int competitionId)
        {
            var score_data = await _context.Competitions
                       .Where(x => x.Id == competitionId)
                       .Include(x => x.ScoreCategories).SelectMany(x => x.ScoreCategories).ToListAsync();
            var groupedScores = score_data
       .GroupBy(s => s.Type)
       .Select(g => new ScoreCompetition
       {
           Type = g.Key,
           score = g.Select(s => new ScoreList
           {
               Id = s.Id,
               Description = s.Description,
               Point = s.Point
           }).ToList()
       })
       .ToList();
            var competition_data = await _context.Competitions.Where(c => c.Id == competitionId).FirstOrDefaultAsync();
            var competioncore = new CompetionCore
            {
                Regulation = competition_data.Regulation,
                scoreCompetition = groupedScores,
            };

            return competioncore;
        }
        public async Task<IEnumerable<ListPlayer>> getListPlayer(int competitionId)
        {
            var listplayer = await _context.Teams.Where(c => c.CompetitionId == competitionId)
                .Select(t => new ListPlayer
                {
                    Id = t.Id,
                    Name = t.Name,
                    Logo = t.Image,

                    played = t.TeamMatches.Count(x => x.IsPlay == true),
                    win = t.TeamMatches.Count(x => x.ResultPlay == "Draw"),
                    draw = t.TeamMatches.Count(x => x.ResultPlay == "Win"),
                    lost = t.TeamMatches.Count(x => x.ResultPlay == "Lose"),
                    members = t.ContestantTeams.Select(v => new MemeberPlayer
                    {
                        Id = v.Contestant.Id,
                        Name = v.Contestant.Name,
                        avatar = v.Contestant.Image

                    }).ToList()

                }).ToListAsync();
            return listplayer;
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
                              registerTime = (DateTime)competition.RegisterTime ,
                              numberContestantTeam = competition.NumberContestantTeam ?? 0
                          }).FirstOrDefaultAsync();
        }
    }
}
