using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace STEM_ROBOT.DAL.Repo
{
    public class MatchRepo : GenericRep<Models.Match>
    {
        public MatchRepo(StemdbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<roundParent>> getRoundGame(int competitionID)
        {
            var listRoundParent = await _context.Competitions
                .Where(x => x.Id == competitionID)
                .Select(comp => new roundParent
                {
                    IsAsign = comp.Stages.FirstOrDefault().TableGroups.FirstOrDefault().IsAsign,

                    groups = comp.Stages.Select(stage => new RoundGame
                    {
                        Id = stage.Id,
                        Status = stage.Status,
                        Name = stage.Name,
                        matchrounds = stage.TableGroups.Select(tableGroup => new Table
                        {
                            Id = tableGroup.Id,
                            tableName = tableGroup.Name,
                            matches = tableGroup.Matches
                                .Select(match => new
                                {


                                    match.Id, 
                                    match.StartDate,
                                    match.TimeIn,
                                    Teams = match.TeamMatches.Select(tm => tm.Team.Name).ToList() 
                                })
                                .Where(m => m.Teams.Count == 2) 
                                .Select(m => new TeamMatchRound
                                {
                                    IdMatch = m.Id,
                                    TeamNameA = m.Teams[0], 
                                    TeamNameB = m.Teams[1],
                             //       date = m.StartDate.HasValue ? m.StartDate.Value : default(DateTime),
                               //     time = m.TimeIn.HasValue ? m.TimeIn.Value : default(TimeSpan),



                                    filed = null

                                })
                                .ToList()
                        }).ToList()
                    }).FirstOrDefault(),
                   
                }).ToListAsync();
                          
            var knockoutData = await getKnockOut(competitionID);

            
            foreach (var round in listRoundParent)
            {
                round.knockout = knockoutData.FirstOrDefault()?.knockout;
            }
            return listRoundParent;
        }


        public async Task<IEnumerable<roundParent>> getKnockOut(int competitionID)
        {
            var listRoundParent = await _context.Competitions
       .Where(x => x.Id == competitionID)
       .Select(comp => new roundParent
       {
           IsAsign = comp.Stages.FirstOrDefault().TableGroups.FirstOrDefault().IsAsign,

           knockout = new RoundGame
           {
               Name = "Knockout",
               matchrounds = comp.Stages
                   .SelectMany(stage => stage.TableGroups)
                   .Select(tableGroup => new Table
                   {
                       Id = 0,
                       tableName = "",
                       matches = tableGroup.Matches
                           .Select(match => new
                           {
                               match.Id,
                               match.StartDate,
                               match.TimeIn,
                               Teams = match.TeamMatches.Select(tm => tm.Team.Name).ToList()
                           })
                           .Where(m => m.Teams.Count == 2) // Only include matches with exactly 2 teams
                           .Select(m => new TeamMatchRound
                           {
                               IdMatch = m.Id,
                               TeamNameA = m.Teams[0],
                               TeamNameB = m.Teams[1],
                               date = m.StartDate ?? default(DateTime),
                               time = m.TimeIn ?? default(TimeSpan),
                               filed = null
                           })
                           .ToList()
                   })
                   .ToList()
           }
       })
       .ToListAsync();
            return listRoundParent;
        }



        public async Task<RoundGameKnockoutParent> getRoundGameKnockOut(int competitionID)
        {
            var listRoundGameKnockOut = await _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.Stages).ThenInclude(x => x.Matches).ThenInclude(x => x.TeamMatches).FirstOrDefaultAsync();
            if (listRoundGameKnockOut == null) return null;

            var rounds = new RoundGameKnockoutParent();
            foreach (var stage in listRoundGameKnockOut.Stages)
            {
                var roundsGame = new RoundGameKnockout
                {
                    roundId = stage.Id,
                    roundName = stage.Name,
                    teamsBye = await teamBye(competitionID),
                    matches = await getRoundGameMacth(competitionID),

                };
                rounds.rounds.Add(roundsGame);

            }
            return rounds; 

        }
        //hàm tính team thừa
        private async Task<List<RoundGameTeamBye>> teamBye(int CompetitionID)
        {
            var listTeam = await _context.Competitions.Where(x => x.Id == CompetitionID).Include(x => x.Teams).FirstOrDefaultAsync();
            var lisTeams = listTeam.Teams.ToList();
            int paw = lisTeams.Count;

            // tính số team lẻ hay đủ 
            bool isPowerOf2 = (paw & (paw - 1)) == 0;
            if (isPowerOf2) return null;
            int round = (int)Math.Ceiling(Math.Log2(paw));
            int closestPowerOf2 = (int)Math.Pow(2, round);
            int extraTeams = paw - (closestPowerOf2 / 2);
            if (extraTeams == 0) return null;
            var extrateamList = lisTeams.Take(extraTeams).ToList();

            var roundGamelist = extrateamList.Select(team => new RoundGameTeamBye
            {
                id = team.Id,
                name = team.Name,
            }).ToList();
            return roundGamelist;
        }
        // hàm get roundmatch
        private async Task<List<RoundGameMatch>> getRoundGameMacth(int competitionID)
        {
            var listMatch = await _context.Competitions.Where(x => x.Id == competitionID)
                .Select(stage => new RoundGameMatch
                {
                    matchId = stage.Stages.SelectMany(x => x.Matches).Select(x => x.Id).FirstOrDefault(),
                    teamsmatch = stage.Stages.SelectMany(x=> x.Matches).SelectMany(match => match.TeamMatches).Select(x=> new RoundGameTeamMatch
                    {
                        teamId =x.Id,
                        teamMatchId = (int)x.MatchId,
                        teamName = x.NameDefault
                    }).ToList(),
                }).ToListAsync();
            if(listMatch == null) return null;
            return listMatch;
        }
    }
}
