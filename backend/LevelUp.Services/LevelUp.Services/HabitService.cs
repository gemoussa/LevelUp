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
    public interface IHabitService
    {
        Task<Habit> CreateHabitAsync(Habit habit);
        Task<Habit> GetHabitByIdAsync(int id);
        Task<IEnumerable<Habit>> GetAllHabitsAsync();
        Task<bool> UpdateHabitAsync(Habit habit);
        Task<bool> DeleteHabitAsync(int id);
        Task<IEnumerable<HabitDTO>> GetHabitsByGoalIdAsync(int goalId);
    }

    public class HabitService:IHabitService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IMapper _mapper;

        public HabitService(IHabitRepository habitRepository, IMapper mapper)
        {
            _habitRepository = habitRepository;
            _mapper = mapper;
        }

        public async Task<Habit> GetHabitByIdAsync(int id)
        {
            return await _habitRepository.GetHabitByIdAsync(id);
        }
        public async Task<IEnumerable<Habit>> GetAllHabitsAsync()
        {
            return await _habitRepository.GetAllHabitsAsync();
        }

        public async Task<Habit> CreateHabitAsync(Habit habit)
        {
            int id = await _habitRepository.CreateHabitAsync(habit);
            return await _habitRepository.GetHabitByIdAsync(id);
        }

        public async Task<bool> UpdateHabitAsync(Habit habit)
        {
            return await _habitRepository.UpdateHabitAsync(habit);
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            return await _habitRepository.DeleteHabitAsync(id);
        }
        public async Task<IEnumerable<HabitDTO>> GetHabitsByGoalIdAsync(int goalId)
        {
            var habits = await _habitRepository.GetHabitsByGoalIdAsync(goalId);
            return _mapper.Map<IEnumerable<HabitDTO>>(habits);
        }
    }
}
