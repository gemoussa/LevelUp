﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Application.LevelUp.DTOs
{
    public class ProgressDTO
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
