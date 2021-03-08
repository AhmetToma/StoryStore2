using System;
using System.Collections.Generic;

#nullable disable

namespace StoryStore.Data
{
    public partial class StoryAgeRange
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public int AgeRangeId { get; set; }
    }
}
