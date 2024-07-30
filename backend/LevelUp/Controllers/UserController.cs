using Microsoft.AspNetCore.Mvc;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Application.LevelUp.DTOs;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }

            var token = await _userService.LoginAsync(request);

            if (token != null)
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _userService.CreateUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUser), new { id }, userDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _userService.UpdateUserAsync(userDTO);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
