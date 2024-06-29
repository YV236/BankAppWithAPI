global using Microsoft.EntityFrameworkCore;
global using BankAppWithAPI.Data;
global using BankAppWithAPI.Dtos.User;
global using BankAppWithAPI.Models;
global using AutoMapper;
global using BankAppWithAPI.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using BankAppWithAPI.Extensions;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<DataContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigration();
}

app.UseHttpsRedirection();
app.MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();