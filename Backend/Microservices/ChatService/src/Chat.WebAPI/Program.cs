using Chat.Data.Services;
using Chat.WebAPI.DI;
using Chat.WebAPI.Hubs;

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

app.UseHttpsRedirection();
app.MapGrpcService<UserGRPCService>();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/ChatHub");

app.Run();
