using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryStore.Models
{
    public class AddUserModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string password { get; set; }
        [Required]

        public int AgeRangeId { get; set; }
    }
}
