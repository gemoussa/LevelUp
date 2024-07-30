using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Infrastructure;
using LevelUp.Core;

namespace LevelUp.Application.LevelUp.Services
{
    public interface ITaskService
    {
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskModel>> GetAllTasksAsync();
        Task<int> CreateTaskAsync(TaskModel task);

        Task<bool> UpdateTaskAsync(TaskModel task);
        Task<bool> DeleteTaskAsync(int id);
    }
    public class TaskService:ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }
        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
           return await _taskRepository.GetAllTasksAsync();
        }

        public async Task<int> CreateTaskAsync(TaskModel task)
        {
            return await _taskRepository.CreateTaskAsync(task);
        }

        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            return await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteTaskAsync(id);
        }
    }
}
