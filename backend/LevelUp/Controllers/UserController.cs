using System.Threading.Tasks;
using LevelUp.Application.LevelUp.DTOs;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LevelUp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoalService _goalService; // Added this line
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IGoalService goalService, ILogger<UserController> logger)
        {
            _userService = userService;
            _goalService = goalService; // Initialize it here
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var (userId, token) = await _userService.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { UserId = userId, Token = token });
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserDTO userDTO)
        {
            try
            {
                var (userId, token) = await _userService.CreateUserAsync(userDTO);
                return Ok(new { userId, token });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        //{
        //    if (id != userDTO.Id)
        //    {
        //        return BadRequest("User ID mismatch");
        //    }

        //    var result = await _userService.UpdateUserAsync(userDTO);
        //    if (!result)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var result = await _userService.DeleteUserAsync(id);
        //    if (!result)
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}
        [HttpPost("{userId}/add-purpose")]
        public async Task<IActionResult> AddPurposeToHomePage(int userId, [FromBody] int samplePurposeId)
        {
            try
            {
                if (samplePurposeId <= 0)
                {
                    return BadRequest("Invalid sample purpose ID.");
                }

                await _userService.AddSamplePurposeToUserHomePageAsync(userId, samplePurposeId);
                return Ok("Purpose added successfully");
            }
            catch (Exception ex) when (ex.Message == "The user already has a purpose and cannot add another one.")
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding purpose to home page for user {UserId}", userId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPost("{userId}/add-goal-and-habits")]
        public async Task<IActionResult> AddGoalAndHabits(int userId, [FromBody] GoalDTO goalDto)
        {
            try
            {
                // Validate and process the request
                var goal = new Goal
                {
                    UserId = userId,
                    Title = goalDto.Title,
                    StartDate = goalDto.StartDate,
                    DueDate = goalDto.DueDate,
                  
                };

                await _goalService.AddGoalAsync(goal);

                return Ok("Goal added successfully");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a proper error response
                _logger.LogError(ex, "Error adding goal to home page");
                return StatusCode(500, "Internal server error");
            }
        }




    }
}
