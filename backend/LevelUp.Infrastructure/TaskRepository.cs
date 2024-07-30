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
    public interface ITaskRepository
    {
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();

        Task<int> CreateTaskAsync(TaskModel task);
        Task<bool> UpdateTaskAsync(TaskModel task);
        Task<bool> DeleteTaskAsync(int id);
    }
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnection _dbConnection;
        public TaskRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            var query = "SELECT * FROM Tasks WHERE Id = @Id";

            return await _dbConnection.QuerySingleOrDefaultAsync<TaskModel>(query, new { Id = id });

        }

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            var query = "SELECT * FROM Tasks";
            return await _dbConnection.QueryAsync<TaskModel>(query);

        }

        public async Task<int> CreateTaskAsync(TaskModel task)
        {
            var query = "INSERT INTO Tasks (UserId, Title, Description, IsCompleted) " +
                       "VALUES (@UserId, @Title, @Description, @IsCompleted) RETURNING Id";
            return await _dbConnection.ExecuteScalarAsync<int>(query, task);

        }
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var query = "UPDATE Tasks SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            });
            return rowsAffected > 0;

        }


        public async Task<bool> DeleteTaskAsync(int id)
        {
            var query = "DELETE FROM Tasks WHERE Id=@Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;


        }
    }
}
