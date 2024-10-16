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

        public AccountRes GetRoleNameAccount(int id)
        {
            var query = from account in _context.Accounts
                        join role in _context.Roles
                        on account.RoleId equals role.Id
                        where account.Id == id
                        select new AccountRes
                        {
                            Id = account.Id,
                            RoleId = account.RoleId,
                            RoleName = role.Name,
                            Name = account.Name,
                            PhoneNumber = account.PhoneNumber,
                            Email = account.Email,
                            Image = account.Image,
                            Status = account.Status
                        };

            return query.FirstOrDefault();

        }
        /*public async Task<List<AccountRes>> getAllAccountRole()
        {
            var acc = await _context.Accounts.Where(x=> x.RoleId == 1).Include(x => x.Role).ToListAsync();
            var mapper = _mapper.Map<List<AccountRes>>(acc);

            return mapper;
        } */

    }
}