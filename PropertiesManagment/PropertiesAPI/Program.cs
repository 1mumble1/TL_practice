using Application.Abstractions;
using Application.Services;
using Domain.Abstractions.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PropertiesDbContext>(
    options =>
    {
        options.UseSqlServer( builder.Configuration.GetConnectionString( "DefaultConnection" ),
            x => x.MigrationsAssembly( "Infrastructure.Migrations" ) );
    } );

builder.Services.AddScoped<IPropertiesRepository, PropertiesRepository>();
builder.Services.AddScoped<IRoomTypesRepository, RoomTypesRepository>();

builder.Services.AddScoped<IPropertiesService, PropertiesService>();
builder.Services.AddScoped<IRoomTypesService, RoomTypesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
