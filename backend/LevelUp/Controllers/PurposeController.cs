using LevelUp.Application.LevelUp.Services;
using Microsoft.AspNetCore.Mvc;
using LevelUp.Application.LevelUp.DTOs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace LevelUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurposeController : ControllerBase
    {
        private readonly IPurposeService _purposeService;
        private readonly ILogger<PurposeController> _logger;

        public PurposeController(IPurposeService purposeService, ILogger<PurposeController> logger)
        {
            _purposeService = purposeService;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPurposesByUserId(int userId)
        {
            try
            {
                var purposes = await _purposeService.GetPurposeByUserIdAsync(userId);
                if (purposes == null)
                {
                    return NotFound($"No purposes found for user {userId}");
                }
                return Ok(purposes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting purposes for user {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("user/{userId}/purpose/{purposeId}")]
        public async Task<IActionResult> AddPurposeToUser(int userId, int purposeId)
        {
            try
            {
                await _purposeService.AddPurposeToUserAsync(userId, purposeId);
                return Ok("Purpose added successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding purpose {PurposeId} to user {UserId}", purposeId, userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurpose([FromBody] PurposeDTO purposeDto)
        {
            try
            {
                var purposeId = await _purposeService.CreatePurposeAsync(purposeDto);
                // Replace 'GetPurposeById' with a different method if needed
                return CreatedAtRoute("GetPurposeById", new { id = purposeId }, new { Id = purposeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating purpose");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("user/{userId}/purpose")]
        public async Task<IActionResult> GetUserPurpose(int userId)
        {
            try
            {
                var purpose = await _purposeService.GetUserPurposeAsync(userId);
                if (purpose == null)
                {
                    return NotFound("Purpose not found.");
                }
                return Ok(purpose);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purpose for user {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("user/{userId}/purpose")]
        public async Task<IActionResult> UpdateUserPurpose(int userId, [FromBody] UpdatePurposeDTO updatePurposeDto)
        {
            try
            {
                if (updatePurposeDto == null || string.IsNullOrEmpty(updatePurposeDto.Title))
                {
                    return BadRequest("Invalid input data.");
                }

                // Pass the UserId and DTO to the service layer
                await _purposeService.UpsertPurposeAsync(userId, updatePurposeDto);

                return Ok("Purpose updated successfully.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data for user {UserId}", userId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purpose for user {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("user/{userId}/purpose")]
        public async Task<IActionResult> DeleteUserPurpose(int userId)
        {
            try
            {
                await _purposeService.DeleteUserPurposeAsync(userId);
                return Ok("Purpose deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting purpose for user {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
