using AutoMapper;
using LevelUp.Application.LevelUp.Services;
using LevelUp.Core;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UserProfile profile)
        {
            if (userId != profile.UserId)
            {
                return BadRequest("User ID mismatch");
            }

            var result = await _profileService.UpdateProfileAsync(profile);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
