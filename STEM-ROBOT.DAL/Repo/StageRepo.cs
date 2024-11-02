using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class StageRepo : GenericRep<Stage>
    {
        public StageRepo(StemdbContext context) : base(context)
        {
        }
        public int GetLatestStageIdByCompetitionIdAndName(int competitionId, string stageName)
        {
            var stage = _context.Stages
                                .Where(s => s.CompetitionId == competitionId && s.Name == stageName)
                                .FirstOrDefault();

            if (stage == null)
            {
                throw new Exception("Stage not found for the given competition and name.");
            }

            return stage.Id;
        }
        public async Task<List<Stage>> GetAllStagesCompetition(int competitionId)
        {

            var stages = await _context.Stages
                .Where(s => s.CompetitionId == competitionId)
                .Include(s => s.Matches)
                .ThenInclude(m => m.TeamMatches)              
                .ThenInclude(m => m.Team)
                .Include(s => s.Matches)
                .ThenInclude(m => m.Location)
                .ToListAsync();
            return stages;
        }
    }
}
