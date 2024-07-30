using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Core
{
    public class Progress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GoalId { get; set; }
        public DateTime ProgressDate { get; set; }
        public decimal ProgressValue { get; set; }
    }
}
