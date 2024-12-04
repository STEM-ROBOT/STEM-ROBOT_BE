
using AutoMapper;
using Net.payOS;
using STEM_ROBOT.BLL;
using STEM_ROBOT.BLL.HubClient;
using STEM_ROBOT.BLL.Mapper;
using STEM_ROBOT.DAL;
using STEM_ROBOT_BE.Extensions;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.addDatabase();
builder.Services.AddPayOs();
//config mapper
builder.Services.AddMapper();
//config authen swagger 

builder.Services.AddSwager();
//config hubclient
builder.Services.AddSignalR();
//builder.Services.addHub();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("myAppCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://157.66.27.69:5173", "https://stem-robot-system.vercel.app")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                    .AllowCredentials();
                
    });
});
builder.Services.AddHttpContextAccessor();
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(IPAddress.Any, 5000); // HTTP
//    options.Listen(IPAddress.Any, 5001, listenOptions =>
//    {
//        listenOptions.UseHttps(); // HTTPS
//    });
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("myAppCors");

app.MapControllers();
// hubclient
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
   
});
app.MapHub<StemHub>("/stem-hub");
app.Run();
