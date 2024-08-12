using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LevelUp.Core;

namespace LevelUp.Infrastructure
{
    public interface IPurposeRepository
    {
        Task<Purpose> GetPurposesByUserIdAsync(int userId);
        Task<Purpose> GetPurposeByIdAsync(int id);
        Task<int> CreatePurposeAsync(Purpose purpose);
        Task AddPurposeToUserAsync(int userId, int purposeId);
        Task<Purpose> GetUserPurposeAsync(int userId);
        Task UpsertUserPurposeAsync(int userId, string title);
        Task DeleteUserPurposeAsync(int userId);
    }

    public class PurposeRepository : IPurposeRepository
    {
        private readonly IDbConnection _dbConnection;

        public PurposeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Purpose> GetPurposesByUserIdAsync(int userId)
        {
            var sql = @"
            SELECT p.*
            FROM Purpose p
            INNER JOIN UserPurposes up ON p.Id = up.PurposeId
            WHERE up.UserId = @UserId";

            return await _dbConnection.QueryFirstOrDefaultAsync<Purpose>(sql, new { UserId = userId });
        }

        public async Task<Purpose> GetPurposeByIdAsync(int id)
        {
            var sql = "SELECT * FROM Purpose WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Purpose>(sql, new { Id = id });
        }

        public async Task<int> CreatePurposeAsync(Purpose purpose)
        {
            const string sql = "INSERT INTO purpose (title) VALUES (@Title) RETURNING id;";
            return await _dbConnection.ExecuteScalarAsync<int>(sql, purpose);
        }

        public async Task AddPurposeToUserAsync(int userId, int purposeId)
        {
            var sql = @"
            INSERT INTO UserPurposes (UserId, PurposeId)
            VALUES (@UserId, @PurposeId)
            ON CONFLICT (UserId, PurposeId) DO NOTHING";

            await _dbConnection.ExecuteAsync(sql, new { UserId = userId, PurposeId = purposeId });
        }

        public async Task<Purpose> GetUserPurposeAsync(int userId)
        {
            var query = "SELECT * FROM Purpose WHERE UserId = @UserId";
            return await _dbConnection.QuerySingleOrDefaultAsync<Purpose>(query, new { UserId = userId });
        }

        public async Task UpsertUserPurposeAsync(int userId, string title)
        {
            var query = @"
            INSERT INTO Purpose (UserId, Title) 
            VALUES (@UserId, @Title)
            ON CONFLICT (UserId) DO UPDATE 
            SET Title = @Title";

            await _dbConnection.ExecuteAsync(query, new { UserId = userId, Title = title });
        }

        public async Task DeleteUserPurposeAsync(int userId)
        {
            var query = "DELETE FROM Purpose WHERE UserId = @UserId";
            await _dbConnection.ExecuteAsync(query, new { UserId = userId });
        }
    }
}
