using Microsoft.EntityFrameworkCore;
using STEM_ROBOT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL.Repo
{
    public class AccountRepo : GenericRep<Account>
    {

        public AccountRepo(StemdbContext context) : base(context)
        {
        }


        public async Task<List<Account>> GetAccount()
        {
            return await _context.Accounts.Include(x => x.Role).ToListAsync();
        }
    }

}