using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class MatchHaflRepo : GenericRep<MatchHalf>
    {
        public MatchHaflRepo(StemdbContext context) : base(context)
        {
        }

        //public async Task<List<MatchHalf> > ListHaftMatch(int matchID)
        //{
        //   var time = await _context.MatchHalves.Where(x=> x.MatchId == matchID)
        //        .Select(x=> new MatchPoint
        //        {
        //            haftMatch = x.Id,
        //           activity = x. 

        //        }).ToListAsync();
        //}


    }
}
