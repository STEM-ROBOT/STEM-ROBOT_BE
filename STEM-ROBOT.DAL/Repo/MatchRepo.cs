using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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


        public async Task<Competition> GetRoundGameAsync(int competitionId)
        {

            var competition = await _context.Competitions
               .Where(s => s.Id == competitionId)
               .Include(l => l.Locations)
               .Include(tb => tb.TableGroups)
               .ThenInclude(s => s.StageTables)
               .ThenInclude(st => st.Stage)
               .ThenInclude(m => m.Matches)
               .ThenInclude(tm => tm.TeamMatches)
               .ThenInclude(t => t.Team)
               .ThenInclude(tt => tt.TeamTables)
               //.Include(st => st.Stages)
               .ThenInclude(m => m.TableGroup)
               //.ThenInclude(tm => tm.TeamMatches)
               .FirstOrDefaultAsync();

            //var roundParent = stages.Select(async comp => new roundParent
            //{
            //    IsAsign = comp.StageTables.FirstOrDefault()?.TableGroup.IsAsign ?? false, // Use null conditional operator
            //    groups = await GetListRoundAsync(comp.Id) // Await asynchronously outside of LINQ
            //}).FirstOrDefault();


            return competition != null ? competition : null;
        }
        public async Task<Competition> GetRoundKnocoutGameAsync(int competitionId)
        {

            var competition = await _context.Competitions
               .Where(s => s.Id == competitionId)
               .Include(l => l.Locations)
               .Include(st => st.Stages)
               .ThenInclude(m => m.Matches)
               .ThenInclude(tm => tm.TeamMatches)
               .ThenInclude(t => t.Team)
               .FirstOrDefaultAsync();
            return competition != null ? competition : null;
        }
        // Get groups


        // Get table
        //private async Task<List<Table>> getTabel(int stageID)
        //{
        //    var list = await _context.Stages.Where(x => x.Id == stageID).Include(x => x.Matches).ThenInclude(x => x.TeamMatches).FirstOrDefaultAsync();
        //    var listTabel = new List<Table>();
        //    foreach (var table in list.Matches)
        //    {
        //        var tables = new Table
        //        {
        //            Id = table.Id,
        //            tableName = list.Name,
        //            matches = list.Matches
        //            .Select(x => new
        //            {

        //                teams = x.TeamMatches.ToList(),
        //            }).Select(x => new TeamMatchRound
        //            {
        //                Id = table.Id,
        //                IdMatch = x.teams.FirstOrDefault().MatchId,
        //                TeamNameA = x.teams[0].NameDefault,
        //                TeamNameB = x.teams[1].NameDefault,
        //                date = table.StartDate,
        //                time = table.TimeIn,

        //            }).ToList()


        //        };
        //        listTabel.Add(tables);
        //    }
        //    return listTabel;
        //}

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
            var listRoundGameKnockOut = await _context.Competitions.Where(x => x.Id == competitionID).Include(x => x.Stages).ThenInclude(x => x.Matches).ThenInclude(x => x.TeamMatches).ThenInclude(tm => tm.Team).Include(c => c.Teams).FirstOrDefaultAsync();
            
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
                        matchId = m.Id,
                        teamMatches = m.TeamMatches.Select(tm => new RoundGameTeamMatch
                        {
                            teamMatchId = tm.Id,
                            teamId = tm.TeamId != null ? tm.TeamId : null,
                            teamName = tm.NameDefault,

                        }).ToList()

                    }).ToList()

                };
                rounds.rounds.Add(roundsGame);

            }
            rounds.teams = listRoundGameKnockOut.Teams.Select(t => new RoundGameTeamBye
            {
                teamId = t.Id,
                name = t.Name,
            }).ToList();
            return rounds;

        }
        //hàm tính team thừa

        // hàm get roundmatch
        //private async Task<List<RoundGameMatch>> getRoundGameMacth(int competitionID)
        //{
        //    var listMatch = await _context.Competitions.Where(x => x.Id == competitionID)
        //        .Select(stage => new RoundGameMatch
        //        {
        //            matchId = stage.Stages.SelectMany(x => x.Matches).Select(x => x.Id).FirstOrDefault(),
        //            teamMatches = stage.Stages.SelectMany(x => x.Matches).SelectMany(match => match.TeamMatches).Select(x => new RoundGameTeamMatch
        //            {
        //                teamId = (int)x.TeamId,
        //                teamMatchId = (int)x.MatchId,
        //                teamName = x.NameDefault
        //            }).ToList(),
        //        }).ToListAsync();
        //    if (listMatch == null) return null;
        //    return listMatch;
        //}


        //  sap xep team trong tran dau bang 
        public async Task<Competition> GetRoundParentTable(int competitionId)
        {
            var competition = await _context.Competitions
                .Where(s => s.Id == competitionId)
                .Include(tb => tb.TableGroups)
                .ThenInclude(tt => tt.TeamTables)
                .ThenInclude(t => t.Team)
                .Include(st => st.Stages.Where(st => st.StageMode == "Vòng bảng"))
                .ThenInclude(s => s.StageTables)
                .ThenInclude(tb => tb.TableGroup)
                .ThenInclude(m => m.Matches)
                .ThenInclude(tm => tm.TeamMatches)
                .FirstOrDefaultAsync();

            return competition;
        }



        // List teams within stages for the competition
        private async Task<List<tableGroup>> GetListTeamTable(Competition competition)

        {
            var list = new List<tableGroup>();
            foreach (var lists in competition.TableGroups)
            {
                var rounds = new tableGroup
                {
                    team_tableId = lists.Id,
                    team_table = lists.TeamTables.Select(x => x.Team).Select(xs => new RoundTableTeam
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
                 .ThenInclude(x => x.TeamTables).Include(x => x.Stages).ThenInclude(x => x.Matches).ThenInclude(x => x.TeamMatches).FirstOrDefaultAsync();

            // Initialize the result list


            var lisrounds = competition.Stages.Select(x => new RoundGameTable
            {
                roundId = x.Id,
                roundName = x.Name,
                tables = x.StageTables.Select(x => x.TableGroup).Select(xt => new RoundTable
                {
                    tableId = xt.Id,
                    tableName = xt.Name,
                    matches = x.Matches.Select(xs => new RoundGameMatch
                    {
                        matchId = xs.Id,
                        teamMatches = xs.TeamMatches.Select(xc => new RoundGameTeamMatch
                        {
                            teamMatchId = xc.MatchId,
                            teamId = xc.TeamId,
                            teamName = xc.NameDefault
                        }).ToList()

                    }).ToList()
                }).ToList()

            }).ToList();





            return lisrounds;
        }

        //realtime point schedule 

        public async Task<List<Teampoint>> TeamPoint(int matchID)
        {
            var list = await _context.Matches.Where(x => x.Id == matchID).SelectMany(team => team.TeamMatches.Select(tm => new Teampoint
            {
                id = (int)tm.Id,
                teamName = tm.Team.Name,
                teamImage = tm.Team.Image,
                teamMatchResulgPlay = tm.TotalScore

            })).ToListAsync();
            return list;
        }
        //
        public async Task<MatchlistPointParent> MatchListPoint(int teamMatchID)
        {

            var list = await _context.TeamMatches.Where(x => x.Id == teamMatchID)
                .Select(a => new MatchlistPointParent
                {
                    teamMatchId = a.MatchId,
                    teamMatchResult = (int)a.TotalScore,
                    teamName = a.Team.Name,
                    teamImage = a.Team.Image,
                    halfActionTeam = a.Actions.Select(c => new MatchListPoint
                    {
                        id = c.Id,
                        halfId = c.MatchHalf.Id,
                        halfName = c.MatchHalf.HalfName,
                        refereeCompetitionId = c.RefereeCompetition.Referee.Id,
                        refereeCompetitionName = c.RefereeCompetition.Referee.Name,
                        scoreTime = CalculateElapsedMinutesAndSeconds(a.Match.TimeIn,c.EventTime),
                        scoreDescription = c.ScoreCategory.Description,
                        scorePoint = c.ScoreCategory.Point,
                        scoreType = c.ScoreCategory.Type,
                        status = c.Status

                    }).ToList()
                }).FirstOrDefaultAsync();

            return list;
        }
        //hàm tính thời gian trả về action 
        public static string CalculateElapsedMinutesAndSeconds(TimeSpan? timeIn, TimeSpan? eventTime)
        {
            if (timeIn == null || eventTime == null)
            {
                throw new ArgumentException("TimeIn and EventTime must not be null.");
            }


            var absoluteEventTime = eventTime - timeIn;


            if (eventTime < timeIn)
            {
                throw new InvalidOperationException("EventTime is beyond the end of the match.");
            }

            // Extract minutes and seconds from eventTime relative to timeIn
            int minutes = eventTime.Value.Minutes;
            int seconds = eventTime.Value.Seconds;


            return $"{minutes:D2}:{seconds:D2}";
        }

    }
}
