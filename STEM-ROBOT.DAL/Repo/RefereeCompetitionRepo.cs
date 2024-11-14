﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class RefereeCompetitionRepo : GenericRep<RefereeCompetition>
    {
        public RefereeCompetitionRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<RefereeCompetition> ListRefereeCompetition(int comeptitionId, int acountId)
        {
            return await _context.RefereeCompetitions
    .Include(x => x.Referee)
    .Include(x => x.Competition)          
    .Include(x=> x.Schedules)
    .ThenInclude(x=> x.Match)
    .ThenInclude(x=> x.Location)
    .Include(x => x.Schedules)
    .ThenInclude(x => x.Match)
    .ThenInclude(x => x.TeamMatches)
    .ThenInclude(x=> x.Team)
    .Where(x => x.Referee.AccountId == acountId && x.CompetitionId == comeptitionId)
    .FirstOrDefaultAsync();
        }

        public async Task<RefereeCompetition> RefereeCompetition(int comeptitionId, int acountId)
        {
            return await _context.RefereeCompetitions
    .Include(x => x.Referee)
    .Include(x => x.Competition)
    .ThenInclude(g=>g.Genre)
    .Where(x => x.Referee.Account.Id == acountId && x.CompetitionId == comeptitionId)
    .FirstOrDefaultAsync();
        }
    }

}
