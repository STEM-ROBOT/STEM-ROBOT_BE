using EFCore.BulkExtensions;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class ProvinceRepo : GenericRep<Province>
    {
        public ProvinceRepo(StemdbContext context) : base(context)
        {

        }
        public async Task BulkInsertAsyncProvince(List<Province> provinceList)
        {
            await _context.BulkInsertAsync(provinceList);
        }
    }
}
