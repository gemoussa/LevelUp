using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using LevelUp.Core;

namespace LevelUp.Infrastructure
{
    public interface IProfileRepository
    {
        Task<UserProfile> GetProfileByUserIdAsync(int userId);
        Task<int> CreateProfileAsync(UserProfile profile);
        Task<bool> UpdateProfileAsync(UserProfile profile);
        Task<bool> DeleteProfileAsync(int userId);
    }

    public class ProfileRepository : IProfileRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProfileRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

      
        public async Task<UserProfile> GetProfileByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Profiles WHERE UserId = @UserId";
            return await _dbConnection.QuerySingleOrDefaultAsync<UserProfile>(query, new { UserId = userId });
        }

        public async Task<int> CreateProfileAsync(UserProfile profile)
        {
            var query = @"
                INSERT INTO profiles (UserId, ProfilePictureURL, FirstName, LastName, DateOfBirth, Bio, Address, PhoneNumber)
                VALUES (@UserId, @ProfilePictureURL, @FirstName, @LastName, @DateOfBirth, @Bio, @Address, @PhoneNumber)
                RETURNING UserId";
            return await _dbConnection.ExecuteScalarAsync<int>(query, profile);
        }

        public async Task<bool> UpdateProfileAsync(UserProfile profile)
        {
            var query = @"
        UPDATE Profiles
        SET ProfilePictureURL = @ProfilePictureURL,
            FirstName = @FirstName,
            LastName = @LastName,
            DateOfBirth = @DateOfBirth,
            Bio = @Bio,
            Address = @Address,
            PhoneNumber = @PhoneNumber
        WHERE UserId = @UserId";

            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                profile.ProfilePictureUrl,
                profile.FirstName,
                profile.LastName,
                profile.DateOfBirth,
                profile.Bio,
                profile.Address,
                profile.PhoneNumber,
                profile.UserId
            });

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteProfileAsync(int userId)
        {
            var query = "DELETE FROM profiles WHERE UserId = @UserId";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new { UserId = userId });
            return rowsAffected > 0;
        }
    }
}
