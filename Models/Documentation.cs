using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models
{
    public partial class Documentation
    {
        public int DocumentationId { get; set; }
        public string? AvailableRoute { get; set; }
        public string? Method { get; set; }
        public string? Description { get; set; }
    }
}
