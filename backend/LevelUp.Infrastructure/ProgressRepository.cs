using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using Dapper;
using System.Data;

namespace LevelUp.Infrastructure
{
    public interface IProgressRepository
    {
        Task<Progress> GetProgressByIdAsync(int id);
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<int> CreateProgressAsync(Progress progress);
        Task<bool> UpdateProgressAsync(Progress progress);
     
    }

    public class ProgressRepository : IProgressRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProgressRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Progress> GetProgressByIdAsync(int id)
        {
            var query = "SELECT * FROM Progress WHERE Id = @Id";
            try
            {
                
                    return await _dbConnection.QuerySingleOrDefaultAsync<Progress>(query, new { Id = id });
                
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("An error occurred while fetching the progress.", ex);
            }
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            var query = "SELECT * FROM Progress";
            try
            {
               
                    return await _dbConnection.QueryAsync<Progress>(query);
                
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("An error occurred while fetching all progress records.", ex);
            }
        }

        public async Task<int> CreateProgressAsync(Progress progress)
        {
            var query = "INSERT INTO Progress (UserId, GoalId, ProgressDate, ProgressValue) " +
                        "VALUES (@UserId, @GoalId, @ProgressDate, @ProgressValue) RETURNING Id";
            try
            {
                
                    return await _dbConnection.ExecuteScalarAsync<int>(query, progress);
                
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("An error occurred while creating a new progress record.", ex);
            }
        }

        public async Task<bool> UpdateProgressAsync(Progress progress)
        {
            var query = @"
            UPDATE Progress 
            SET 
                UserId = @UserId, 
                GoalId = @GoalId, 
                ProgressDate = @ProgressDate, 
                ProgressValue = @ProgressValue 
            WHERE Id = @Id";

            try
            {
                
                    var rowsAffected = await _dbConnection.ExecuteAsync(query, new
                    {
                        Id = progress.Id,
                        UserId = progress.UserId,
                        GoalId = progress.GoalId,
                        ProgressDate = progress.ProgressDate,
                        ProgressValue = progress.ProgressValue
                    });
                    return rowsAffected > 0;
                
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("An error occurred while updating the progress record.", ex);
            }
        }

       
    }
}
