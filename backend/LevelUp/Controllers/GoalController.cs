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
    }
}
