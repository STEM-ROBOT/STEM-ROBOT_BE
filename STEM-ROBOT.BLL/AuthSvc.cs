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

namespace STEM_ROBOT.BLL
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

        public SingleRsp Login(LoginReq loginReq)
        {
            var res = new SingleRsp();

            try
            {
                var user = _accountRep.Find(u => u.Email == loginReq.Email).FirstOrDefault();

                if (user == null)
                {
                    res.SetError("404", "User not found");
                }
                else if (user.Password != loginReq.Password)
                {
                    res.SetError("401", "Invalid password");
                }
                else
                {
                    var token = GenerateJwtToken(user);
                    res.setData("200", new { token });
                    res.SetMessage("Login successful");
                }
            }
            catch (Exception ex)
            {
                res.SetError("500", ex.Message);
            }

            return res;
        }

        private string GenerateJwtToken(Account acc)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, acc.Email),
                    //w Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
