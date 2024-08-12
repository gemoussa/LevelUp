using LevelUp.Application;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                policy =>
                {
                    policy.WithOrigins("http://localhost:3001") // Update this to your front-end URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

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
        builder.Services.AddScoped<IGoalService, GoalService>();
        builder.Services.AddScoped<IHabitService, HabitService>();
        builder.Services.AddScoped<IHabitRepository, HabitRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ISampleRepository, SampleRepository>();
        builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IPurposeService, PurposeService>();
        builder.Services.AddScoped<ITemplateService, TemplateService>();
        builder.Services.AddScoped<IPurposeRepository, PurposeRepository>();
       
       

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

        // Use CORS policy
        app.UseCors("AllowSpecificOrigin");

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
