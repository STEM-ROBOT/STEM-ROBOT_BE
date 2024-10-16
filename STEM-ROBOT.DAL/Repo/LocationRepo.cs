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
        public LocationRes GetCompetitionNameByLocation(int id)
        {
            var query = from location in _context.Locations
                        join competition in _context.Competitions
                        on location.CompetitionId equals competition.Id
                        where location.Id == id
                        select new LocationRes
                        {
                            Id = location.Id,
                            Address = location.Address,
                            ContactPerson = location.ContactPerson,
                            Status = location.Status,
                            CompetitionId = location.CompetitionId,
                            CompetitionName = competition.Name  // Lấy tên của Competition
                        };

            return query.FirstOrDefault();
        }

        /*public override IEnumerable<LocationRes> All()
        {
            var query = from location in _context.Locations
                        join competition in _context.Competitions
                        on location.CompetitionId equals competition.Id
                        select new LocationRes
                        {
                            Id = location.Id,
                            Address = location.Address,
                            ContactPerson = location.ContactPerson,
                            Status = location.Status,
                            CompetitionId = location.CompetitionId,
                            CompetitionName = competition.Name  // Lấy tên của Competition
                        };

            return query.ToList();
        }*/
    }
}
