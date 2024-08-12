using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LevelUp.Application.LevelUp.DTOs;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IGoalService
    {
        Task<Goal> GetGoalByIdAsync(int id);
        Task<IEnumerable<GoalDTO>> GetGoalsByUserIdAsync(int userId);
        Task<int> CreateGoalAsync(Goal goal);
        Task<bool>UpdateGoalAsync(Goal goal);
        Task<bool> DeleteGoalAsync(int id);
        Task AddGoalAsync(Goal goal);
        Task AddGoalToUserAsync(int userId, int goalId);
    }
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IMapper _mapper;

        public GoalService(IGoalRepository goalRepository, IMapper mapper)
        {
            _goalRepository = goalRepository;
            _mapper = mapper;
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _goalRepository.GetGoalByIdAsync(id);
        }
        public async Task<IEnumerable<GoalDTO>> GetGoalsByUserIdAsync(int userId)
        {
            var goals = await _goalRepository.GetGoalByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GoalDTO>>(goals);
        }
       

        public async Task<int> CreateGoalAsync(Goal goal)
        {
            return await _goalRepository.CreateGoalAsync(goal);
        }

        public async Task<bool> UpdateGoalAsync(Goal goal)
        {
            return await _goalRepository.UpdateGoalAsync(goal);
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            return await _goalRepository.DeleteGoalAsync(id);
        }
        public async Task AddGoalAsync(Goal goal)
        {
            await _goalRepository.AddGoalAsync(goal);
        }
        public async Task AddGoalToUserAsync(int userId, int goalId)
        {
            
            var goalExists = await _goalRepository.GetGoalByIdAsync(goalId);
            if (goalExists == null)
            {
                throw new KeyNotFoundException("Goal not found");
            }

            await _goalRepository.AddGoalToUserAsync(userId, goalId);
        }
    }

}

