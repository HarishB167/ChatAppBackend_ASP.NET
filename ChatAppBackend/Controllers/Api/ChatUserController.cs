using AutoMapper;
using ChatAppBackend.Dto;
using ChatAppBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatAppBackend.Controllers.Api
{
    public class ChatUserController : ApiController
    {
        private ApplicationDbContext _context;

        public ChatUserController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult GetUsers()
        {
            var data = _context.ChatUsers.ToList()
                .Select(Mapper.Map<User, UserDto>);
            return Ok(data);
        }
    }
}
