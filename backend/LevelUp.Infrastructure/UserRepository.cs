using System.Data;
using System.Data.Common;

    using Dapper;
using LevelUp.Core;
namespace LevelUp.Infrastructure
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<int> CreateUserAsync(User user);
        //Task<bool> UpdateUserAsync(User user);
        //Task<bool> DeleteUserAsync(int id);
        Task<bool> UserHasPurposeAsync(int userId);
        Task AddPurposeToUserHomePageAsync(Purpose purpose);
        Task<int> AddGoalToUserAsync(Goal goal);
        Task AddHabitToUserAsync(Habit habit);
    }
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";
           
                return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
            
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username";
            
                return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
           
        }

        public async Task<int> CreateUserAsync(User user)
        {
            var query = @"
                INSERT INTO Users (Username, PasswordHash, Email, IsAdmin) 
                VALUES (@Username, @PasswordHash, @Email, @IsAdmin ) 
                RETURNING Id";

            return await _dbConnection.ExecuteScalarAsync<int>(query, user);
         
        }

        public async Task<bool> UserHasPurposeAsync(int userId)
        {
            var query = "SELECT COUNT(1) FROM Purpose WHERE UserId = @UserId";
            var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { UserId = userId });
            return count > 0;
        }
        //adding the sample purpose into the user home page
        public async Task AddPurposeToUserHomePageAsync(Purpose purpose)
        {
            var query = "INSERT INTO Purpose (UserId, Title) VALUES (@UserId, @Title)";
            await _dbConnection.ExecuteAsync(query, new { UserId = purpose.UserId, Title = purpose.Title });
        }

        public async Task<int> AddGoalToUserAsync(Goal goal)
        {
            var query = @"
            INSERT INTO Goals (UserId, Title, StartDate, DueDate, Status, IsAchieved) 
            VALUES (@UserId, @Title, @StartDate, @DueDate, @Status, @IsAchieved)
            RETURNING Id";

            return await _dbConnection.ExecuteScalarAsync<int>(query, goal);
        }

        public async Task AddHabitToUserAsync(Habit habit)
        {
            var query = @"
            INSERT INTO Habits (UserId, GoalId, Title, Frequency) 
            VALUES (@UserId, @GoalId, @Title, @Frequency)";

            await _dbConnection.ExecuteAsync(query, habit);
        }

    }

}
