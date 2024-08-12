using AutoMapper;
using LevelUp.Application.LevelUp.DTOs;
using LevelUp.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // Excluding password from DTO

        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignoring Id on update

        CreateMap<Goal, GoalDTO>();
        CreateMap<GoalDTO, Goal>();

        CreateMap<Habit, HabitDTO>();
        CreateMap<HabitDTO, Habit>();

        CreateMap<Task, TaskDTO>();
        CreateMap<TaskDTO, Task>();

        CreateMap<Progress, ProgressDTO>();
        CreateMap<ProgressDTO, Progress>();

        CreateMap<SampleGoal, SampleGoalDTO>();
        CreateMap<SampleHabit, SampleHabitDTO>();

        CreateMap<SamplePurpose, SamplePurposeDTO>();


        CreateMap<SamplePurposeDTO, SamplePurpose>();
     
    }
}
