using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitController:ControllerBase
    {
        private readonly IHabitService _habitService;
        public HabitController(IHabitService habitService)
        {
            _habitService = habitService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetHabitById(int id)
        {
            var habit = await _habitService.GetHabitByIdAsync(id);
            if (habit == null)
            {
                return NotFound();
            }
            return Ok(habit);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHabit([FromBody] Habit habit)
        {
            if (habit == null)
            {
                return BadRequest();
            }
            var createdHabit = await _habitService.CreateHabitAsync(habit);
            return CreatedAtAction(nameof(GetHabitById), new { id = createdHabit.Id }, createdHabit);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabit(int id, [FromBody] Habit habit)
        {
            if (habit == null || habit.Id != id)
            {
                return BadRequest();
            }

            var existingHabit = await _habitService.GetHabitByIdAsync(id);
            if (existingHabit == null)
            {
                return NotFound();
            }

            var updated = await _habitService.UpdateHabitAsync(habit);
            if (!updated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // DELETE: api/Habit/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var existingHabit = await _habitService.GetHabitByIdAsync(id);
            if (existingHabit == null)
            {
                return NotFound();
            }

            var deleted = await _habitService.DeleteHabitAsync(id);
            if (!deleted)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
