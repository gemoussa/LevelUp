using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IGoalService
    {
        Task<Goal> GetGoalByIdAsync(int id);
        Task<int> CreateGoalAsync(Goal goal);
        Task<bool>UpdateGoalAsync(Goal goal);
        Task<bool> DeleteGoalAsync(int id);
    }
    public class GoalService:IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _goalRepository.GetGoalByIdAsync(id);
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
    }
}
