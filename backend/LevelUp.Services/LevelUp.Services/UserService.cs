using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using LevelUp.Infrastructure;
using LevelUp.Application.LevelUp.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserAsync(int id);
        Task<(int userId, string token)> LoginAsync(LoginRequestDTO request);
        Task<(int userId, string token)> CreateUserAsync(UserDTO userDTO);
 

        Task AddGoalAndHabitsToUserHomePageAsync(int userId, int sampleGoalId);
        Task AddSamplePurposeToUserHomePageAsync(int userId, int samplePurposeId);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISampleRepository _sampleRepository;
        private readonly string _jwtSecret;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, ISampleRepository sampleRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _sampleRepository = sampleRepository ?? throw new ArgumentNullException(nameof(sampleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _jwtSecret = _configuration["Jwt:Secret"];
        }

      
        public async Task<(int userId, string token)> LoginAsync(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return (0, null); // Invalid request
            }

            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return (0, null); // Invalid credentials
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Username),
            // Add other claims as needed
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            // Return both userId and token
            return (user.Id, jwtToken);
        }

       

        public async Task<(int userId, string token)> CreateUserAsync(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }

            if (string.IsNullOrEmpty(userDTO.Password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(userDTO.Password));
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = hashedPassword, 
                IsAdmin = userDTO.IsAdmin,
            };
            var userId = await _userRepository.CreateUserAsync(user);
            var loginResult = await LoginAsync(new LoginRequestDTO
            {
                Username = userDTO.Username,
                Password = userDTO.Password
            });
            return loginResult;
        }
        public async Task<UserDTO> GetUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }
        //public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        //{
        //    if (userDTO == null)
        //    {
        //        throw new ArgumentNullException(nameof(userDTO));
        //    }

        //    var user = _mapper.Map<User>(userDTO);
        //    return await _userRepository.UpdateUserAsync(user);
        //}
        //public async Task<bool> DeleteUserAsync(int id)
        //{
        //    return await _userRepository.DeleteUserAsync(id);
        //}
        public async Task AddSamplePurposeToUserHomePageAsync(int userId, int samplePurposeId)
        {
            // Check if the user already has any purpose
            bool purposeExists = await _userRepository.UserHasPurposeAsync(userId);
            if (purposeExists)
            {
                throw new Exception("The user already has a purpose and cannot add another one.");
            }

            var samplePurpose = await _sampleRepository.GetSamplePurposeByIdAsync(samplePurposeId);
            if (samplePurpose == null) throw new Exception("Sample purpose not found");

            var newPurpose = new Purpose
            {
                UserId = userId,
                Title = samplePurpose.Title,
                // Add any other properties you want to copy from SamplePurpose to Purpose
            };

            await _userRepository.AddPurposeToUserHomePageAsync(newPurpose);
        }




        public async Task AddGoalAndHabitsToUserHomePageAsync(int userId, int sampleGoalId)
        {
            // Fetch the sample goal
            var sampleGoal = await _sampleRepository.GetSampleGoalByIdAsync(sampleGoalId);
            if (sampleGoal == null) throw new Exception("Sample goal not found");

            // Create a new goal for the user using the sample goal details
            var newGoal = new Goal
            {
                UserId = userId,
                Title = sampleGoal.Title,
                StartDate = DateTime.Now,
                DueDate = sampleGoal.DueDate,
                Status = "Not Started",
                IsAchieved = false
              
            };

            // Add the new goal to the user's goals
            var newGoalId = await _userRepository.AddGoalToUserAsync(newGoal);

            // Fetch the corresponding sample habits
            var sampleHabits = await _sampleRepository.GetHabitsBySampleGoalIdAsync(sampleGoalId);

            // Create and add new habits for the user
            foreach (var sampleHabit in sampleHabits)
            {
                var newHabit = new Habit
                {
                    UserId = userId,
                    GoalId = newGoalId,
                    Title = sampleHabit.Title,
                    Frequency = sampleHabit.Frequency
                    // Add other properties as needed
                };
                await _userRepository.AddHabitToUserAsync(newHabit);
            }
        }

    }
}
