using Game.Data.Services;
using Game.WebAPI.DI;
using Game.WebAPI.Hubs;
using Game.WebAPI.NewFolder;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(builder.Configuration);

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

app.MapControllers();

app.MapHub<GameHub>("/GameHub");

app.Run();
