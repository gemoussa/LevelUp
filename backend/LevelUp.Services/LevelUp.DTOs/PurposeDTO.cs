using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.Application.LevelUp.DTOs
{
    public class PurposeDTO
    {
        public int SamplePurposeId { get; set; }

        [Required]
        public string Title { get; set; }
       
        public int? UserId { get; set; }
    }
}
