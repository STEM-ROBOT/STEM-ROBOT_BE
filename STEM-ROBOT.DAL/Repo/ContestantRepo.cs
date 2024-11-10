using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class ContestantRepo : GenericRep<Contestant>
    {
        public ContestantRepo(StemdbContext context) : base(context)
        {
        }
        public async Task BulkInsertAsyncSchool(List<Contestant> contestants)
        {
            await _context.BulkInsertAsync(contestants);
        }
        //sum contestant 
        public async Task<int> Sumcontestant(int TournamentID)
        {
            return await _context.Contestants.Where(x => x.TournamentId == TournamentID).CountAsync();
        }
    }
}
