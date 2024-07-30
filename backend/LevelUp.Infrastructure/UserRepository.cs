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
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
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
            var query = "INSERT INTO Users (Username, PasswordHash, Email, IsAdmin, Purpose) VALUES (@Username, @PasswordHash, @Email, @IsAdmin, @Purpose) RETURNING Id";
           
                return await _dbConnection.ExecuteScalarAsync<int>(query, user);
         
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var query = "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, Email = @Email, IsAdmin = @IsAdmin, Purpose = @Purpose WHERE Id = @Id";
           
                var rowsAffected = await _dbConnection.ExecuteAsync(query, user);
                return rowsAffected > 0;
            
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var query = "DELETE FROM Users WHERE Id = @Id";
           
                var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            
        }
    }

}
