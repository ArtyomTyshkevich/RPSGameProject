using Chat.WebAPI.DI;
using Chat.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы SignalR
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

// Убедитесь, что CORS разрешает подключение
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// Регистрация маршрута для хаба
app.MapHub<ChatHub>("/ChatHub");

app.Run();
