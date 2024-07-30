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
        Task<string> LoginAsync(LoginRequestDTO request);
        Task<int> CreateUserAsync(UserDTO userDTO);
        Task<bool> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _jwtSecret = _configuration["Jwt:Secret"];
        }

        public async Task<string> LoginAsync(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null; // Invalid credentials
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
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> CreateUserAsync(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }

            if (string.IsNullOrEmpty(userDTO.Password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(userDTO.Password));
            }

            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                IsAdmin = userDTO.IsAdmin,
                Purpose = userDTO.Purpose
            };

            // Create the user in the repository
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }
    }
}
