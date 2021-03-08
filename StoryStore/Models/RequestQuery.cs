using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryStore.Models
{
    public class RequestQuery
    {

        public string StoryName { get; set; } = "";

        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

        public int? AgeRangeId { get; set; }
    }
}
