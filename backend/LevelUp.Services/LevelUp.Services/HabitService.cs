using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IHabitService
    {
        Task<Habit> CreateHabitAsync(Habit habit);
        Task<Habit> GetHabitByIdAsync(int id);
        Task<IEnumerable<Habit>> GetAllHabitsAsync();
        Task<bool> UpdateHabitAsync(Habit habit);
        Task<bool> DeleteHabitAsync(int id);
    }

    public class HabitService
    {
        private readonly IHabitRepository _habitRepository;

        public HabitService(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public async Task<Habit> GetHabitByIdAsync(int id)
        {
            return await _habitRepository.GetHabitByIdAsync(id);
        }
        public async Task<IEnumerable<Habit>> GetAllHabitsAsync()
        {
            return await _habitRepository.GetAllHabitsAsync();
        }

        public async Task<int> CreateHabitAsync(Habit habit)
        {
            return await _habitRepository.CreateHabitAsync(habit);
        }

        public async Task<bool> UpdateHabitAsync(Habit habit)
        {
            return await _habitRepository.UpdateHabitAsync(habit);
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            return await _habitRepository.DeleteHabitAsync(id);
        }
    }
}
