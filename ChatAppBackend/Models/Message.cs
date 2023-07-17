using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatAppBackend.Models
{
    public class Message
    {
        public int Id { get; set; }

        public User Sender { get; set; }
        public int? SenderId { get; set; }

        public User Receiver { get; set; }
        public int? ReceiverId { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + ", ReceiverId: " + ReceiverId
                + ", SenderId: " + SenderId + ", DateTime: " + this.DateTime
                + ", Status: " + Status + ", Content: " + Content;
        }
    }
}