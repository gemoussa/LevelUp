using Microsoft.AspNetCore.Mvc;
using LevelUp.Infrastructure;
using LevelUp.Core;
using LevelUp.Application.LevelUp.Services;
using Microsoft.AspNetCore.Authorization;
namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController:ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService tasKService)
        {
            _taskService = tasKService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            if (task == null)
            {
                return BadRequest();
            }
            var createdTaskId = await _taskService.CreateTaskAsync(task);
            task.Id = createdTaskId;
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel task)
        {
            if (task == null || task.Id != id)
            {
                return BadRequest();
            }

            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            var updated = await _taskService.UpdateTaskAsync(task);
            if (!updated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

    }
}
