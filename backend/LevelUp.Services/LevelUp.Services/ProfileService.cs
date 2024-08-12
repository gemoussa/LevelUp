using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(UserProfile profile);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<UserProfile> GetProfileAsync(int userId)
        {
            return await _profileRepository.GetProfileByUserIdAsync(userId);
        }

        public async Task<bool> UpdateProfileAsync(UserProfile profile)
        {
            return await _profileRepository.UpdateProfileAsync(profile);
        }
    }

}
