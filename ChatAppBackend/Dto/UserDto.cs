using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatAppBackend.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PhotoLink { get; set; }
    }
}