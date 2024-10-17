using EFCore.BulkExtensions;
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
        public async Task BulkInsertAsyncSchool(List<Contestant> schoolList)
        {
            await _context.BulkInsertAsync(schoolList);
        }
    }
}
