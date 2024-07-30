using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LevelUp.Infrastructure;
using System.Data;
using Npgsql;
using LevelUp.Application;
using LevelUp.Application.LevelUp.Services;
namespace LevelUp.Application.LevelUp.MappingProfiles;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure JWT authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]))
                };
            });

        // Configure IDbConnection
        builder.Services.AddScoped<IDbConnection>(provider =>
            new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register application services and repositories
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
        builder.Services.AddScoped<IGoalRepository, GoalRepository>();
        builder.Services.AddScoped<IHabitRepository, HabitRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
         
        builder.Services.AddAutoMapper(typeof(MappingProfile));


        // Build the app
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}