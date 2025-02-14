using Chat.WebAPI.DI;
using Chat.WebAPI.Hubs;
using Chat.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.MapGrpcService<UserGRPCService>();
app.UseCors("_allowSignalRCors");
app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>("/ChatHub").RequireCors("_allowSignalRCors");

app.Run();
