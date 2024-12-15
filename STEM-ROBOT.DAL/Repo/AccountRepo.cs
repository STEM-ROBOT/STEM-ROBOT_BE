using Microsoft.EntityFrameworkCore;

using STEM_ROBOT.Common.Rsp;

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
        /*public async Task<List<Account>> GetAccounts()
        {
            return await _context.Accounts.Where(x => x.Role == "a").Include(x => x.Role).ToListAsync();
        }
        public async Task<Account> GetAccountById(int id)
        {
<<<<<<< HEAD
            return await _context.Accounts.Where(x => x.Role == "a" && x.Id == id).Include(x => x.Role).FirstOrDefaultAsync();
        }
=======
            return await _context.Accounts.Where(x => x.RoleId != 1 && x.Id == id).Include(x => x.Role).FirstOrDefaultAsync();
        }*/

        public async Task<List<Account>> GetAccount()
        {
            return await _context.Accounts.Where(x => x.Role == "MD").Include(x => x.Orders).ThenInclude(pa => pa.Package).Include(x=> x.Tournaments).Include(x=> x.Contestants).ToListAsync();        }
    }
    
}