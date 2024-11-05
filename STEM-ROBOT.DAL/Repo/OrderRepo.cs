using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class OrderRepo : GenericRep<Order>
    {
        public OrderRepo(StemdbContext context) : base(context)
        { }
        public async Task<List<Order>> GetListOrder(string? NameUser)
        {
            if(NameUser != null)
            {
                return await _context.Orders.Include(x=> x.Account).Where(x=> x.Account.Name.Contains(NameUser)).Include(x=> x.Payments).ToListAsync();
            }
            else { 
            return await _context.Orders.Include(x=> x.Account).Include(x=> x.Payments).ToListAsync();
            }
        }
    }
}
