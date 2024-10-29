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

    }
}
