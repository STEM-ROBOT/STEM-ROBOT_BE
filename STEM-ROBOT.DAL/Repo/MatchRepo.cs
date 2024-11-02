using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Table = STEM_ROBOT.Common.Rsp.Table;

namespace STEM_ROBOT.DAL.Repo
{
    public class MatchRepo : GenericRep<Models.Match>
    {
        public MatchRepo(StemdbContext context) : base(context)
        {
        }


        public async Task<roundParent> GetRoundGameAsync(int competitionID)
        {
            // First fetch the relevant stages for the given competitionID
            var stages = await _context.Competitions
                .Where(x => x.Id == competitionID)
                .SelectMany(comp => comp.Stages)
                .ToListAsync();

            // Process each stage to build the RoundParent object
            var roundParent = stages.Select(async comp => new roundParent
            {
                IsAsign = comp.StageTables.FirstOrDefault()?.TableGroup.IsAsign ?? false, // Use null conditional operator
                groups = await GetListRoundAsync(comp.Id) // Await asynchronously outside of LINQ
            }).FirstOrDefault();

            // Await the task and return the result
            return roundParent != null ? await roundParent : null;
        }

        // Get groups
        private async Task<List<RoundGame>> GetListRoundAsync(int stageID)
        {
            var stage = await _context.Stages
                .Where(x => x.Id == stageID)
                .Include(x => x.StageTables)
                    .ThenInclude(x => x.TableGroup)
                .FirstOrDefaultAsync();

            var listRound = new List<RoundGame>();
            if (stage?.StageTables != null)
            {
                foreach (var stageTable in stage.StageTables)
                {
                    var roundGame = new RoundGame
                    {
                        Id = stage.Id,
                        Name = stage.Name,
                        Status = stage.Status,
                        matchrounds = await getTabel(stageTable.TableGroup.Id) // Adjusted to use async
                    };
                    listRound.Add(roundGame);
                }
            }

            return listRound;
        }

        // Get table
        private async Task<List<Table>> getTabel(int stageID)
        {
            var list = await _context.Stages.Where(x => x.Id == stageID).Include(x => x.Matches).ThenInclude(x => x.TeamMatches).FirstOrDefaultAsync();
            var listTabel = new List<Table>();
            foreach (var table in list.Matches)
            {
                var tables = new Table
                {
                    Id = table.Id,
                    tableName = list.Name,
                    matches = list.Matches
                    .Select(x => new {

                        teams = x.TeamMatches.ToList(),
                    }).Select(x => new TeamMatchRound
                    {
                        Id = table.Id,
                        IdMatch = x.teams.FirstOrDefault().MatchId,
                        TeamNameA = x.teams[0].NameDefault,
                        TeamNameB = x.teams[1].NameDefault,
                        date = table.StartDate,
                        time = table.TimeIn,

                    }).ToList()


                };
                listTabel.Add(tables);
            }
            return listTabel;
        }

        // public async Task<IEnumerable<roundParent>> getKnockOut(int competitionID)
        // {
        //     var listRoundParent = await _context.Competitions
        //.Where(x => x.Id == competitionID)
        //.Select(comp => new roundParent
        //{
        //    IsAsign = comp.Stages.FirstOrDefault().TableGroups.FirstOrDefault().IsAsign,

        //    knockout = new RoundGame
        //    {
        //        Name = "Knockout",
        //        matchrounds = comp.Stages
        //            .SelectMany(stage => stage.TableGroups)
        //            .Select(tableGroup => new Table
        //            {
        //                Id = 0,
        //                tableName = "",
        //                matches = tableGroup.Matches
        //                    .Select(match => new
        //                    {
        //                        match.Id,
        //                        match.StartDate,
        //                        match.TimeIn,
        //                        Teams = match.TeamMatches.Select(tm => tm.Team.Name).ToList()
        //                    })
        //                    .Where(m => m.Teams.Count == 2) // Only include matches with exactly 2 teams
        //                    .Select(m => new TeamMatchRound
        //                    {
        //                        IdMatch = m.Id,
        //                        TeamNameA = m.Teams[0],
        //                        TeamNameB = m.Teams[1],
        //                        date = m.StartDate ?? default(DateTime),
        //                        time = m.TimeIn ?? default(TimeSpan),
        //                        filed = null
        //                    })
        //                    .ToList()
        //            })
        //            .ToList()
        //    }
        //})
        //.ToListAsync();
        //     return listRoundParent;
        // }



        public async Task<RoundGameKnockoutParent> getRoundGameKnockOut(int competitionID)
        {
            var rounds = new RoundGameKnockoutParent();
            var listRoundGameKnockOut = await _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.Stages).ThenInclude(x => x.Matches).ThenInclude(x => x.TeamMatches).ThenInclude(tm =>tm.Team).Include(c=>c.Teams).FirstOrDefaultAsync();
            if (listRoundGameKnockOut == null) return null;
           rounds.isTeamMatch = (bool)listRoundGameKnockOut.IsTeamMacth;
           
            foreach (var stage in listRoundGameKnockOut.Stages)
            {
                var roundsGame = new RoundGameKnockout
                {
                    roundId = stage.Id,
                    roundName = stage.Name,

                    matches = stage.Matches.Select(m => new RoundGameMatch
                    {
                        matchId=m.Id,
                        teamsmatch= m.TeamMatches.Select(tm => new RoundGameTeamMatch
                        {
                            teamMatchId= tm.Id,
                            teamId= tm.TeamId != null ? tm.TeamId :null,
                            teamName= tm.NameDefault ,

                        }).ToList()

                    }).ToList()

                };
                rounds.rounds.Add(roundsGame);

            }
            rounds.teams = listRoundGameKnockOut.Teams.Select(t=> new RoundGameTeamBye
            {
                teamId = t.Id,
                name = t.Name,
            }).ToList();
            return rounds;

        }
        //hàm tính team thừa
       
        // hàm get roundmatch
        private async Task<List<RoundGameMatch>> getRoundGameMacth(int competitionID)
        {
            var listMatch = await _context.Competitions.Where(x => x.Id == competitionID)
                .Select(stage => new RoundGameMatch
                {
                    matchId = stage.Stages.SelectMany(x => x.Matches).Select(x => x.Id).FirstOrDefault(),
                    teamsmatch = stage.Stages.SelectMany(x => x.Matches).SelectMany(match => match.TeamMatches).Select(x => new RoundGameTeamMatch
                    {
                        teamId = (int)x.TeamId,
                        teamMatchId = (int)x.MatchId,
                        teamName = x.NameDefault
                    }).ToList(),
                }).ToListAsync();
            if (listMatch == null) return null;
            return listMatch;
        }


        //  sap xep team trong tran dau bang 
        public async Task<RoundParentTable> GetRoundParentTable(int competitionID)
        {
            var competition = await _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.TableGroups).ThenInclude(x => x.TeamTables).ThenInclude(x => x.Team).FirstOrDefaultAsync();

            var roundParentTable = new RoundParentTable
            {
                tableGroup = await GetListTeamTable(competition),
                rounds = await GetRoundGameTable(competitionID),
                isTeamMatch = competition.IsTeamMacth
            };

            return roundParentTable;
        }
     
        

        // List teams within stages for the competition
        private async Task<List<tableGroup>> GetListTeamTable(Competition competition)

        {
            
            var list = new List<tableGroup>();
           foreach(var lists in competition.TableGroups)
            {
                var rounds = new tableGroup
                {
                   team_tableId = lists.Id,
                   team_table = lists.TeamTables.Select(x=> x.Team).Select( xs => new RoundTableTeam
                   {
                       teamId = xs.Id,
                       teamName = xs.Name
                   }).ToList()
                    
                };
                list.Add(rounds);


            }
            return list;
          
        }


        private async Task<List<RoundGameTable>> GetRoundGameTable(int competitionID)
        {

            var competition = await _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.Stages).ThenInclude(x => x.StageTables).ThenInclude(x => x.TableGroup)
                 .ThenInclude(x => x.TeamTables).FirstOrDefaultAsync();
          
            // Initialize the result list
            var listRound = new List<RoundGameTable>();

           
            foreach (var stage in competition.Stages)
            {
                var roundGameTable = new RoundGameTable
                {
                    roundId = stage.Id,
                    roundName = stage.Name,

                    tables = stage.StageTables.Select(x=> x.TableGroup).Select(a => new RoundTable
                    {

                        tableId = a.Id,
                        tableName = a.Name,
                        matches = stage.Matches.Select(mat => new RoundGameMatch
                        {
                            matchId = mat.Id,
                            teamsmatch = mat.TeamMatches.Select(x=> new RoundGameTeamMatch
                            {          teamMatchId = x.MatchId,
                                       teamId = x.TeamId,
                                    teamName = x.NameDefault,
                                    isSetup = x.IsSetup

                            }).ToList()
                        }).ToList(),
                        
                    }).ToList()
                    
               
                };

                // Add the populated RoundGameTable object to the result list
                listRound.Add(roundGameTable);
            }

            return listRound;
        }


    }
}
