using STEM_ROBOT.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.DAL
{
    public class AccountReq : GenericRep<User>
    {
        private List<User> _users;

        public AccountReq()
        {
          
            _users = new List<User>
            {
                new User { Id = 1, Email = "Test123@gmail.com", Password = "123456", Role = "User" },
                new User { Id = 2, Email = "Admin123@gmail.com", Password = "123456", Role = "Admin" },
            };
        }

       
        public List<User> Get(Expression<Func<User, bool>> predicate = null)
        {
           
            if (predicate != null)
            {
                return _users.AsQueryable().Where(predicate).ToList();
            }
            return _users.ToList();
        }
    }

   
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
