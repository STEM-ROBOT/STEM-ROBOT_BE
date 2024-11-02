using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Net.payOS;
using STEM_ROBOT.BLL;
using STEM_ROBOT.BLL.Mail;
using STEM_ROBOT.BLL.Mapper;

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
            services.AddScoped<FormatRepo>();
            services.AddScoped<FormatSvc>();
            services.AddScoped<GenreRepo>();
            services.AddScoped<GenreSvc>();
            services.AddScoped<TournamentRepo>();
            services.AddScoped<TournamentSvc>();
            services.AddScoped<LocationRepo>();
            services.AddScoped<LocationSvc>();
            services.AddScoped<SchoolRepo>();
            services.AddScoped<SchoolSvc>();
            services.AddScoped<RefereeRepo>();
            services.AddScoped<RefereeSvc>();
            services.AddScoped<ScoreCategoryRepo>();
            services.AddScoped<ScoreCategorySvc>();
            services.AddScoped<ContestantRepo>();
            services.AddScoped<ContestantSvc>();

            services.AddScoped<CompetitionRepo>();
            services.AddScoped<CompetitionSvc>();
            services.AddScoped<StageRepo>();
            services.AddScoped<StageSvc>();

            services.AddScoped<TeamSvc>();
            services.AddScoped<TeamRepo>();
            services.AddScoped<ScheduleRepo>();
            services.AddScoped<ScheduleSvc>();
            services.AddScoped<ActionRepo>();
            services.AddScoped<ActionSvc>();
            services.AddScoped<MatchRepo>();
            services.AddScoped<MatchSvc>();
            services.AddScoped<TableGroupRepo>();
            services.AddScoped<TableGroupSvc>();

            services.AddScoped<IMailService, MailService>();
            services.AddScoped<TeamMatchRepo>();


            services.AddScoped<PackageRepo>();
            services.AddScoped<PackageSvc>();
            services.AddScoped<OrderRepo>();
            services.AddScoped<OrderSvc>();
            services.AddScoped<PaymentRepo>();
            services.AddScoped<TeamTableRepo>();
            services.AddScoped<TeamTableSvc>();
            services.AddScoped<TeamMatchRepo>();
            services.AddScoped<TeamMatchSvc>();
            services.AddScoped<StorageSvc>();
            services.AddScoped<RefereeCompetitionRepo>();
            services.AddScoped<ProvinceRepo>();
            services.AddScoped<ProvinceSvc>();
            services.AddScoped<DistrictRepo>();
            services.AddScoped<DistrictSvc>();
            services.AddScoped<RealTimeSvc>();
            services.AddSingleton<Random>();
            services.AddScoped<StageTableRepo>();
            return services;
        }

        //public static IServiceCollection addHub(this IServiceCollection services)
        //{
        //    services.AddSignalR();
        //    services.AddSingleton<TournamentClient>();
        //    return services;
        //}
        //con fig mapper

        public static IServiceCollection AddPayOs(this IServiceCollection services)
        {
            services.AddSingleton<PayOS>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new PayOS(
                    configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
            });
            return services;
        }
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Map());
            });
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Map));
            return services;
        }
        //config add authen swagger
        public static IServiceCollection AddSwager(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "STEM_ROBOT API",
                    Version = "v1",
                    Description = "API for STEM Robot management system"
                });

                // Cấu hình cho JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter the JWT with Bearer prefix like: Bearer {token}",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
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
