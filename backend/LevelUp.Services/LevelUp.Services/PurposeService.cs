using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LevelUp.Application.LevelUp.DTOs;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IPurposeService
    {
        Task<Purpose> GetPurposeByUserIdAsync(int userId);
        Task<Purpose> GetPurposeByIdAsync(int id);
        Task<bool> AddPurposeToUserAsync(int userId, int purposeId);
  
        Task<int> CreatePurposeAsync(PurposeDTO purposeDto);
        Task UpsertPurposeAsync(int userId, UpdatePurposeDTO updatePurposeDto);
        Task<Purpose> GetUserPurposeAsync(int userId);
        Task DeleteUserPurposeAsync(int userId);
    }

    public class PurposeService : IPurposeService
    {
        private readonly IPurposeRepository _purposeRepository;
        private readonly IMapper _mapper;

        public PurposeService(IPurposeRepository purposeRepository, IMapper mapper)
        {
            _purposeRepository = purposeRepository;
            _mapper = mapper;
        }

        public async Task<Purpose> GetPurposeByUserIdAsync(int userId)
        {
            return await _purposeRepository.GetPurposesByUserIdAsync(userId);
        }

        public async Task<bool> AddPurposeToUserAsync(int userId, int purposeId)
        {
            var purpose = await _purposeRepository.GetPurposeByIdAsync(purposeId);
            if (purpose == null)
            {
                throw new KeyNotFoundException("Purpose not found.");
            }

            await _purposeRepository.AddPurposeToUserAsync(userId, purposeId);
            return true;
        }

        public async Task<Purpose> GetPurposeByIdAsync(int id)
        {
            var purpose = await _purposeRepository.GetPurposeByIdAsync(id);
            if (purpose == null)
            {
                throw new KeyNotFoundException("Purpose not found.");
            }
            return purpose;
        }

        public async Task<int> CreatePurposeAsync(PurposeDTO purposeDto)
        {
            var purpose = new Purpose
            {
                Title = purposeDto.Title
            };

            var newPurposeId = await _purposeRepository.CreatePurposeAsync(purpose);
            return newPurposeId;
        }

        public async Task UpsertPurposeAsync(int userId, UpdatePurposeDTO updatePurposeDto)
        {
            if (updatePurposeDto == null || string.IsNullOrEmpty(updatePurposeDto.Title))
            {
                throw new ArgumentException("Invalid purpose data.");
            }

            await _purposeRepository.UpsertUserPurposeAsync(userId, updatePurposeDto.Title);
        }

        public async Task<Purpose> GetUserPurposeAsync(int userId)
        {
            return await _purposeRepository.GetUserPurposeAsync(userId);
        }

        public async Task DeleteUserPurposeAsync(int userId)
        {
            await _purposeRepository.DeleteUserPurposeAsync(userId);
        }
    }
}
