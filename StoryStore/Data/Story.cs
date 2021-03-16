using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace StoryStore.Data
{
    public partial class Story
    {
        public string StoryName { get; set; }
        public int StoryId { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string PdfUrl { get; set; }
        public string AudioUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? StoryDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(AgeRangeId))]
        public int AgeRangeId { get; set; }

        public AgeRange AgeRange { get; set; }
    }
}
