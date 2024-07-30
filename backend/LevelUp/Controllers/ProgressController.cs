using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgressById(int id)
        {
            var progress = await _progressService.GetProgressByIdAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            return Ok(progress);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProgress()
        {
            var progressList = await _progressService.GetAllProgressAsync();
            return Ok(progressList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgress([FromBody] Progress progress)
        {
            if (progress == null)
            {
                return BadRequest();
            }

            var createdId = await _progressService.CreateProgressAsync(progress);
            return CreatedAtAction(nameof(GetProgressById), new { id = createdId }, progress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgress(int id, [FromBody] Progress progress)
        {
            if (progress == null || progress.Id != id)
            {
                return BadRequest();
            }

            var updated = await _progressService.UpdateProgressAsync(progress);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

      
    }
}

