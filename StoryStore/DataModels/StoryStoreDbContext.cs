using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoryStore.Data;
using StoryStore.Models;

namespace StoryStore.DataModels
{
    public class StoryStoreDbContext : IdentityDbContext<AppUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public StoryStoreDbContext(DbContextOptions options) : base(options) { }


        public virtual DbSet<AgeRange> AgeRanges { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
     //   public virtual DbSet<StoryAgeRange> StoryAgeRanges { get; set; }


    }
}