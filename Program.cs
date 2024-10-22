using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency injectionilla v�litetty tietokantayhteys controllereille
builder.Services.AddDbContext<NorthwindOriginalContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Azureen julkaistessa k�ytet��n t�t�:
//builder.Configuration.GetConnectionString("Azure")
//L�ytyy appsettings.jsonista





// ------------- Cors m��ritys ------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all");

app.UseAuthorization();

app.MapControllers();

app.Run();
