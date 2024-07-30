using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LevelUp.Core;
using LevelUp.Application.LevelUp.DTOs;

namespace LevelUp.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Goal, GoalDTO>();
            CreateMap<GoalDTO, Goal>();

            CreateMap<Habit, HabitDTO>();
            CreateMap<HabitDTO, Habit>();

            CreateMap<Task, TaskDTO>();
            CreateMap<TaskDTO, Task>();

            CreateMap<Progress, ProgressDTO>();
            CreateMap<ProgressDTO, Progress>();
        }
    }
}
