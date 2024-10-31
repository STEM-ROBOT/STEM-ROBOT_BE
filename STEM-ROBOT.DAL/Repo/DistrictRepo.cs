using EFCore.BulkExtensions;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class DistrictRepo : GenericRep<District>
    {
        public DistrictRepo(StemdbContext context) : base(context)
        {
        }
        public async Task BulkInsertAsyncDistrict(List<District> districtList)
        {
            await _context.BulkInsertAsync(districtList);
        }
    }

}
