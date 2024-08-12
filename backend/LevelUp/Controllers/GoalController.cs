using LevelUp.Application.LevelUp.DTOs;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalController:ControllerBase
    {
        private readonly IGoalService _goalService;
        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGoalById(int id)
        {
            var goal=await _goalService.GetGoalByIdAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            return Ok(goal);
        }

        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetGoalsByUserId(int userId)
        //{
        //    var goals = await _goalService.GetGoalsByUserIdAsync(userId);
        //    if (goals == null)
        //    {
        //        // Ensure you always return an empty list rather than null
        //        goals = new List<GoalDTO>();
        //    }
        //    return Ok(goals);
        //}

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoalsByUserId(int userId)
        {
            var goals = await _goalService.GetGoalsByUserIdAsync(userId);
            if (goals == null || !goals.Any())
            {
                goals = new List<GoalDTO>(); 
            }
            return Ok(goals);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] Goal goal )
        {
            if (goal == null || goal.Id != id)
            {
                return BadRequest();
            }

            var existingGoal = await _goalService.GetGoalByIdAsync(id);
            if (existingGoal == null)
            {
                return NotFound();
            }

            await _goalService.UpdateGoalAsync(goal);
            return NoContent();

        }
        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] Goal goal)
        {
            if (goal == null)
            {
                return BadRequest("Goal cannot be null.");
            }

            var goalId = await _goalService.CreateGoalAsync(goal);
            return CreatedAtAction(nameof(GetGoalById), new { id = goalId }, goal);
        }
    
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var existingGoal = await _goalService.GetGoalByIdAsync(id);
            if (existingGoal == null)
            {
                return NotFound();
            }

            await _goalService.DeleteGoalAsync(id);
            return NoContent();
        }
        [HttpPost("user/{userId}/goal/{goalId}")]
        public async Task<IActionResult> AddGoalToUser(int userId, int goalId)
        {
            try
            {
                await _goalService.AddGoalToUserAsync(userId, goalId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
