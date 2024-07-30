using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Core
{
    public class Habit
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
    }
}
