using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class AreaRepo : GenericRep<Area>
    {
        public AreaRepo(StemdbContext context) : base(context)
        {
        }
    public async Task<List<Area>> GetListArea()
        {
            return await _context.Areas.ToListAsync();
        }


    }
}
