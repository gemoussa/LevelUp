using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LevelUp.Core;

namespace LevelUp.Infrastructure
{
    public interface IHabitRepository
    {
        Task<int> CreateHabitAsync(Habit habit);
        Task<Habit> GetHabitByIdAsync(int id);
        Task<IEnumerable<Habit>> GetAllHabitsAsync();
        Task<bool> UpdateHabitAsync(Habit habit);
        Task<bool> DeleteHabitAsync(int id);
    }

    public class HabitRepository : IHabitRepository
    {
        private readonly IDbConnection _dbConnection;
        public HabitRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<Habit> GetHabitByIdAsync(int id)
        {
            var query = "SELECT * FROM Habits WHERE Id = @Id";
           
                return await _dbConnection.QuerySingleOrDefaultAsync<Habit>(query, new { Id = id });
            
        }

        public async Task<IEnumerable<Habit>> GetAllHabitsAsync()
        {
            var query = "SELECT * FROM Habits";
           
                return await _dbConnection.QueryAsync<Habit>(query);
            
        }

        public async Task<int> CreateHabitAsync(Habit habit)
        {
            var query = "INSERT INTO Habits (GoalId, Title, Description, Frequency) VALUES (@GoalId, @Title, @Description, @Frequency) RETURNING Id";
           
                return await _dbConnection.ExecuteScalarAsync<int>(query, habit);
            
        }

        public async Task<bool> UpdateHabitAsync(Habit habit)
        {
            var query = "UPDATE Habits SET GoalId = @GoalId, Title = @Title, Description = @Description, Frequency = @Frequency WHERE Id = @Id";
           
                var rowsAffected = await _dbConnection.ExecuteAsync(query, habit);
                return rowsAffected > 0;
            
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            var query = "DELETE FROM Habits WHERE Id = @Id";
            
                var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            
        }
    }

}

