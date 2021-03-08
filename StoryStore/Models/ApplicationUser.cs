using Microsoft.AspNetCore.Identity;
using StoryStore.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryStore.Models
{
    public class AppUser : IdentityUser
    {
        [ForeignKey(nameof(AgeRangeId))]
        public int AgeRangeId { get; set; }

        public AgeRange AgeRange { get; set; }
    }
}
