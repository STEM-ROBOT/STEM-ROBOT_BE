using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using STEM_ROBOT.Common.BLL;
using STEM_ROBOT.Common.DAL;
using STEM_ROBOT.Common.Req;
using STEM_ROBOT.Common.Rsp;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Svc
{

    public class AuthSvc : GenericSvc<Account>
    {
        private readonly AccountRepo _accountRep;

        private readonly IConfiguration _configuration;

        public AuthSvc(AccountRepo accountRep, IConfiguration configuration) : base(accountRep)
        {
            _accountRep = accountRep;

            _configuration = configuration;
        }
        public async Task<TokenRsp> Login(LoginReq loginReq)
        {
            var user = _accountRep.Find(x => x.Email == loginReq.Email && x.Password == loginReq.Password).FirstOrDefault();
            if (user == null) { throw new AggregateException("No user"); }
            var token = await GenarateToken(user);
            return token;
        }


        public async Task<TokenRsp> GenarateToken(Account user)
        {


            var jwttokenHandler = new JwtSecurityTokenHandler();
            var secretkey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new List<Claim>
    {
         new Claim(ClaimTypes.Name, user.Name),
         new Claim("Id", user.Id.ToString()),
         new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim("Email", user.Email),
         new Claim(ClaimTypes.Role, user.RoleId.ToString()),
    };
            var tokendescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretkey), SecurityAlgorithms.HmacSha512),
                Claims = new Dictionary<string, object>
                {


                }
            };


            var token = jwttokenHandler.CreateToken(tokendescription);
            var accessToken = jwttokenHandler.WriteToken(token);

            return new TokenRsp
            {
                Token = accessToken
            };

        }
    }
}
