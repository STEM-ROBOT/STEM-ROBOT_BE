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
    public class MatchRepo : GenericRep<Match>
    {
        public MatchRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<roundParent>> getRoundGame(int stageID)
        {
            var listRoundParent = await _context.Stages
                .Where(x => x.Id == stageID)
                .Select(st => new roundParent
                {
                    IsAsign = st.TableGroups.FirstOrDefault().IsAsign,
                    groups = st.TableGroups.Select(gr => new RoundGame
                    {
                        Id = gr.Id,
                        Status = st.Status,
                        Name = st.Name,
                        matchrounds = gr.TeamTables.Select(tb => new Table
                        {
                            Id = tb.Id,
                            tableName = gr.Name,
                            matches = gr.Matches
                                .Select(match => new 
                                {

                                   
                                    match.Id, // Match ID
                                    match.StartDate,
                                    match.TimeIn,
                                    Teams = match.TeamMatches.Select(tm => tm.Team.Name).ToList() // List of Team names in this Match
                                })
                                .Where(m => m.Teams.Count == 2) // Ensure there are exactly two teams
                                .Select(m => new TeamMatchRound
                                {
                                    IdMatch = m.Id,
                                    TeamNameA = m.Teams[0], // First team
                                    TeamNameB = m.Teams[1],  // Second team
                                   // date = (DateTime)m.StartDate,
                                  //  time = (DateTime)m.TimeIn,
                                    
                                    filed = null
                                    
                                })
                                .ToList()
                        }).ToList()
                    }).FirstOrDefault()
                }).ToListAsync();

            return listRoundParent;
        }

    }
}
