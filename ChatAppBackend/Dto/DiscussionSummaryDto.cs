using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatAppBackend.Dto
{
    public class DiscussionSummaryDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageTimeMarker { get; set; }
        public string PhotoLink { get; set; }
    }
}