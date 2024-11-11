using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class NotificationRepo : GenericRep<Notification>
    {
        public NotificationRepo(StemdbContext context) : base(context)
        {
        }
        public async Task<List<Notification>> listNotifi(int userid)
        {
            return await _context.Notifications.Where(x=> x.AccountId == userid).ToListAsync();
        }
    }
}
