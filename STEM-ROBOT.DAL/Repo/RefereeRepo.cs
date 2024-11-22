using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class RefereeRepo : GenericRep<Referee>
    {
        public RefereeRepo(StemdbContext context) : base(context) { }
        public async Task<Referee> GetListReferee(int userId)
        {
            return await _context.Referees.Where(x => x.AccountId == userId).Include(x => x.RefereeCompetitions)
                .ThenInclude(x => x.Competition).ThenInclude(x => x.Genre).Include(x => x.Tournament).FirstOrDefaultAsync();
        }
        public async Task<Referee> GetRefereeInfo(int userId)
        {
            return await _context.Referees.Where(x => x.AccountId == userId).Include(t => t.Tournament).FirstOrDefaultAsync();
        }
        public async Task<List<RefereeCompetition>> GetListSupReferee(int refeId)
        {
            return await _context.RefereeCompetitions.Where(rc => rc.RefereeId == refeId && rc.Role == "SRF")
                .Include(x => x.Competition).ThenInclude(x => x.Genre).ToListAsync();
        }
    }
}
