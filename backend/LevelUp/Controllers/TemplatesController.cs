using System.Collections.Generic;
using System.Threading.Tasks;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplatesController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet("goals")]
        public async Task<IActionResult> GetSampleGoals()
        {
            var goals = await _templateService.GetSampleGoalsAsync();
            return Ok(goals);
        }

        [HttpGet("habits")]
        public async Task<IActionResult> GetSampleHabits()
        {
            var habits = await _templateService.GetSampleHabitsAsync();
            return Ok(habits);
        }

        [HttpGet("purposes")]
        public async Task<IActionResult> GetSamplePurposes()
        {
            var purposes = await _templateService.GetSamplePurposesAsync();
            return Ok(purposes);
        }

        [HttpGet("goals/{goalId}/habits")]
        public async Task<IActionResult> GetHabitsByGoalId(int goalId)
        {
            if (goalId <= 0) return BadRequest("Invalid goal ID");

            var habits = await _templateService.GetHabitsByGoalIdAsync(goalId);
            return Ok(habits);
        }

        [HttpPost("goals")]
        public async Task<IActionResult> AddSampleGoal([FromBody] SampleGoal goal)
        {
            if (goal == null)
                return BadRequest("Goal cannot be null");

            await _templateService.AddSampleGoalAsync(goal);
            return CreatedAtAction(nameof(GetSampleGoals), new { id = goal.Id }, goal);
        }

        [HttpPost("habits")]
        public async Task<IActionResult> AddSampleHabit([FromBody] SampleHabit habit)
        {
            if (habit == null)
                return BadRequest("Habit cannot be null");

            await _templateService.AddSampleHabitAsync(habit);
            return CreatedAtAction(nameof(GetSampleHabits), new { id = habit.Id }, habit);
        }

        [HttpPost("purposes")]
        public async Task<IActionResult> AddSamplePurpose([FromBody] SamplePurpose purpose)
        {
            if (purpose == null)
                return BadRequest("Purpose cannot be null");

            await _templateService.AddSamplePurposeAsync(purpose);
            return CreatedAtAction(nameof(GetSamplePurposes), new { id = purpose.Id }, purpose);
        }
    }
}
