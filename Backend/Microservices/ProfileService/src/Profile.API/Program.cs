using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Profile.API.Middlewares;
using Profile.BLL.DI;
using Profile.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbContext>(sp =>
 new MongoDbContext(
               builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString"),
               builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName")
           ));
builder.Services.AddBusinessLogic(builder.Configuration);
builder.Services.AddDbContext<ProfileDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
