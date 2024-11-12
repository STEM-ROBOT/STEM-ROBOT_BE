
using AutoMapper;
using Net.payOS;
using STEM_ROBOT.BLL;
using STEM_ROBOT.BLL.HubClient;
using STEM_ROBOT.BLL.Mapper;
using STEM_ROBOT.DAL;
using STEM_ROBOT_BE.Extensions;


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
        policy.WithOrigins("http://localhost:5173")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                    .AllowCredentials();
                
    });
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();
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
