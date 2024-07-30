using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Application.LevelUp.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public int GoalId { get; set; }
    }
}
