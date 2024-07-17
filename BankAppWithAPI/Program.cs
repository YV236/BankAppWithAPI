global using Microsoft.EntityFrameworkCore;
global using BankAppWithAPI.Services.Authentication;
global using BankAppWithAPI.Services.UserServices;
global using System.Security.Claims;
using BankAppWithAPI.Data;
using BankAppWithAPI.Models;
using Microsoft.AspNetCore.Identity;
using BankAppWithAPI.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BankAppWithAPI.Services.BankAccountService;

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

builder.Services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager()
    .AddApiEndpoints();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




