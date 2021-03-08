using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StoryStore.Models
{
    public class AddStoryModel
    {


        [Required]
        public string storyName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public IFormFile StoryImage { get; set; }

        [Required]
        public int AgeRangeId { get; set; }

        public IFormFile PdfFile { get; set; }
        public IFormFile AudioFile { get; set; }

    }
}