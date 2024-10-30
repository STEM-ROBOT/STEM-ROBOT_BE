using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                            Id = tb.Id,
                            tableName = gr.Name,
                            matches = gr.Matches
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
                            Id = tableGroup.Id,
                            tableName = tableGroup.Name,
                            matches = tableGroup.Matches.Select(t => new
                            {
                                t.Id,
                                t.StartDate,
                                t.TimeIn,
                                Teams = t.TeamMatches.Select(tm => tm.Team.Name).ToList()

                            }).Where(x=> x.Teams.Count == 2)
                            .Select(t => new TeamMatchRound{
                                IdMatch = t.Id,
                                TeamNameA = t.Teams[0],
                                TeamNameB = t.Teams[1],
                                    date = t.StartDate.HasValue ? t.StartDate.Value : default(DateTime),
                                    time = t.TimeIn.HasValue ? t.TimeIn.Value : default(TimeSpan),
                                filed = null
                            }).ToList()
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


    }
}
