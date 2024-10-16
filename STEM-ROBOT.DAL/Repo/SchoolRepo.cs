using EFCore.BulkExtensions;
using STEM_ROBOT.DAL.Models;
namespace STEM_ROBOT.DAL.Repo
{
    public class SchoolRepo : GenericRep<School>
    {
        public SchoolRepo(StemdbContext context) : base(context)
        {
        }
        public async Task BulkInsertAsyncSchool(List<School> schoolList)
        {
            await _context.BulkInsertAsync(schoolList);
        }
    }
}
