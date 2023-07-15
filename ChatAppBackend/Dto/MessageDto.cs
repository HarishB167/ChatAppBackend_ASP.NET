﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatAppBackend.Dto
{
    public class MessageDto
    {
        public int Id { get; set; }

        [Required]
        public int? SenderId { get; set; }

        [Required]
        public int? ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime? DateTime { get; set; }
        public string Status { get; set; }
    }
}