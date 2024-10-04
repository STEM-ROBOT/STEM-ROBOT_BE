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
        private List<Account> _account;
        public AccountRepo(StemdbContext context) : base(context)
        {
        }

        public override IEnumerable<Account> All(Expression<Func<Account, bool>> filter = null, Func<IQueryable<Account>, IOrderedQueryable<Account>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            return _context.Accounts.ToList();
        }

    }

}
