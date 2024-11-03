using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class LocationRepo : GenericRep<Location>
    {
        public LocationRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<List<Location>> GetLocations()
        {
            return await _context.Locations.Include(x=> x.Competition).ToListAsync();
        }
        public async Task<Location> GetLocationById(int id)
        {
            return await _context.Locations.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
