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
    public interface IGoalRepository
    {
        Task<Goal> GetGoalByIdAsync(int id);
        Task<IEnumerable<Goal>>GetGoalByUserIdAsync(int userId);
        Task<int> CreateGoalAsync(Goal goal);
        Task<bool> UpdateGoalAsync(Goal goal);
        Task<bool> DeleteGoalAsync(int id);
        Task AddGoalAsync(Goal goal);
        Task AddGoalToUserAsync(int userId, int goalId);
    }
    public class GoalRepository:  IGoalRepository
    {
        private readonly IDbConnection _dbConnection;
        public GoalRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            var query = "SELECT * FROM Goals WHERE Id = @Id";
            
                return await _dbConnection.QuerySingleOrDefaultAsync<Goal>(query, new { Id = id });

        }
        public async Task AddGoalAsync(Goal goal)
        {
            var query = "INSERT INTO Goals (UserId, Title, StartDate, DueDate, IsAchieved, Status) " +
                        "VALUES (@UserId, @Title, @StartDate, @DueDate, @IsAchieved, @Status)";
            await _dbConnection.ExecuteAsync(query, goal);
        }
        //public async Task<IEnumerable<Goal>> GetGoalByUserIdAsync(int userId)
        //{
        //    var query = "SELECT * FROM Goals WHERE UserId = @UserId";
        //    return await _dbConnection.QueryAsync<Goal>(query, new { UserId = userId });
        //}
        public async Task<IEnumerable<Goal>> GetGoalByUserIdAsync(int userId)
        {
            // Step 1: Retrieve goals by user ID
            var query = "SELECT * FROM Goals WHERE UserId = @UserId";
            var goals = (await _dbConnection.QueryAsync<Goal>(query, new { UserId = userId })).ToList();

            // Step 2: Retrieve associated habits for each goal
            foreach (var goal in goals)
            {
                var habitsQuery = "SELECT * FROM Habits WHERE GoalId = @GoalId";
                goal.Habits = (await _dbConnection.QueryAsync<Habit>(habitsQuery, new { GoalId = goal.Id })).ToList();
            }

            return goals;
        }

        public async Task<int> CreateGoalAsync(Goal goal)
        {
            var query = "INSERT INTO Goals (UserId, Title, StartDate, DueDate, IsAchieved, Status) " +
                      "VALUES (@UserId, @Title, @StartDate, @DueDate, @IsAchieved, @Status) RETURNING Id";
            return await _dbConnection.ExecuteScalarAsync<int>(query, goal);
            
        }

        public async Task<bool> UpdateGoalAsync(Goal goal)
        {
            var query = "UPDATE Goals SET Title=@Title, Description=@Description, StartDate=@StartDate, DueDate=@DueDate, isAchieved=@isAchieved, Status=@Status";
           
                var rowsAffected = await _dbConnection.ExecuteAsync(query, new
                {
                    Id = goal.Id,
                    Title = goal.Title,
                    StartDate = goal.StartDate,
                    DueDate = goal.DueDate,
                    IsAchieved = goal.IsAchieved,
                    Status = goal.Status
                });
                return rowsAffected > 0;
            
        }
        public async Task<bool>DeleteGoalAsync(int id)
        {
            var query = "DELETE FROM Goals WHERE Id=@Id";
           
                var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            
        }
        public async Task AddGoalToUserAsync(int userId, int goalId)
        {
            var query = "INSERT INTO Goals (UserId, GoalId) VALUES (@UserId, @GoalId)";
            await _dbConnection.ExecuteAsync(query, new { UserId = userId, GoalId = goalId });
        }
    }
}
