using Chat.Data.Extensions;
using Game.Data.Configuration;
using Game.Data.Services;
using Game.WebAPI.DI;
using Game.WebAPI.Hubs;
using Game.WebAPI.NewFolder;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Host.ConfigureLogs(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<UserGRPCService>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("_allowSignalRCors");
app.UseHangfireDashboard("/hangfire-dashboard", new DashboardOptions()
{
    IgnoreAntiforgeryToken = true
});
app.MapControllers();
app.Services.SetupJobs();
app.MapHub<GameHub>("/GameHub").RequireCors("_allowSignalRCors");

app.Run();
