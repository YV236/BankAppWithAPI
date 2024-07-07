global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using BankAppWithAPI.Data;
global using BankAppWithAPI.Dtos.User;
global using BankAppWithAPI.Models;
global using AutoMapper;
global using BankAppWithAPI.Services.UserServices;
global using System.Security.Claims;
global using BankAppWithAPI.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using BankAppWithAPI.Extensions;

// TODO: delete global

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<Swashbuckle.AspNetCore.Filters.SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthorization();

//builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<DataContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
