using Auth.BLL.DI;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.Load("Auth.BLL"));
builder.Services.AddControllers();
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.ConfigureLogs(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
Seed.InitializeRoles(app.Services).Wait();
app.MapControllers();

app.Run();