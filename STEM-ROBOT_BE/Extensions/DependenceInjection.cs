using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using STEM_ROBOT.BLL.Svc;
using STEM_ROBOT.DAL.Models;
using STEM_ROBOT.DAL.Repo;
using System.Text;

namespace STEM_ROBOT_BE.Extensions
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<AuthSvc>();  
            services.AddScoped<AccountSvc>();
            services.AddScoped<AccountRepo>();
            services.AddScoped<TournamentFormatRepo>();
            services.AddScoped<TournamentFormatSvc>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            // Cấu hình JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }

        public static IServiceCollection addDatabase(this IServiceCollection services)
        {
            services.AddDbContext<StemdbContext>(options => options.UseSqlServer(getConnectionString()));
            return services;
        }

        private static string getConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;

        }
    }
}
