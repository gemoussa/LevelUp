using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface ITemplateService
    {
        Task<IEnumerable<SampleGoal>> GetSampleGoalsAsync();
        Task<IEnumerable<SampleHabit>> GetSampleHabitsAsync();
        Task<IEnumerable<SamplePurpose>> GetSamplePurposesAsync();
        Task<IEnumerable<Habit>> GetHabitsByGoalIdAsync(int goalId);
        Task AddSampleGoalAsync(SampleGoal goal);
        Task AddSampleHabitAsync(SampleHabit habit);
        Task AddSamplePurposeAsync(SamplePurpose purpose);
    }
    public class TemplateService : ITemplateService
    {
        private readonly ISampleRepository _sampleRepository;

        public TemplateService(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public async Task<IEnumerable<SampleGoal>> GetSampleGoalsAsync()
        {
            return await _sampleRepository.GetSampleGoalsAsync();
        }

        public async Task<IEnumerable<SampleHabit>> GetSampleHabitsAsync()
        {
            return await _sampleRepository.GetSampleHabitsAsync();
        }

        public async Task<IEnumerable<SamplePurpose>> GetSamplePurposesAsync()
        {
            return await _sampleRepository.GetSamplePurposesAsync();
        }
        public async Task<IEnumerable<Habit>> GetHabitsByGoalIdAsync(int goalId)
        {
            if (goalId <= 0) throw new ArgumentException("Invalid goal ID", nameof(goalId));
            return await _sampleRepository.GetHabitsByGoalIdAsync(goalId);
        }
        public async Task AddSampleGoalAsync(SampleGoal goal)
        {
            // Validate and add the sample goal
            if (goal == null) throw new ArgumentNullException(nameof(goal));
            await _sampleRepository.AddSampleGoalAsync(goal);
        }

        public async Task AddSampleHabitAsync(SampleHabit habit)
        {
            // Validate and add the sample habit
            if (habit == null) throw new ArgumentNullException(nameof(habit));
            await _sampleRepository.AddSampleHabitAsync(habit);
        }

        public async Task AddSamplePurposeAsync(SamplePurpose purpose)
        {
            // Validate and add the sample purpose
            if (purpose == null) throw new ArgumentNullException(nameof(purpose));
            await _sampleRepository.AddSamplePurposeAsync(purpose);
        }
    }
}
