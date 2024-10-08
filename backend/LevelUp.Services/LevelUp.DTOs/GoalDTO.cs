﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Application.LevelUp.DTOs
{
    public class GoalDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public List<HabitDTO> Habits { get; set; } = new List<HabitDTO>();

    }
}
