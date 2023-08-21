using AutoMapper;
using ChatAppBackend.Dto;
using ChatAppBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BC = BCrypt.Net.BCrypt;
using ChatAppBackend;
using ChatAppBackend.Filters;

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

        public IHttpActionResult GetUser(int id)
        {
            var user = _context.ChatUsers.SingleOrDefault(c => c.Id == id);
            if (user == null)
                return NotFound();
            return Ok(Mapper.Map<User, UserDto>(user));
        }


        [HttpPost]
        [Route("Api/ChatUser/Login")]
        public IHttpActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _context.ChatUsers.SingleOrDefault(c => c.Email == loginDto.Email);

            if (user == null)
                return NotFound();

            bool verified = BC.Verify(loginDto.Password, user.Password);

            var token = Utils.Utils.GenerateJwtToken(new {
                username = user.Email, userId = user.Id,
                photoLink = user.PhotoLink, name = user.Name
            });

            // Validating
            // var validated = Utils.Utils.ValidateJwtToken(token);

            return Ok(new { email = user.Email, name = user.Name, token = token });
        }

        [HttpPost]
        [Route("Api/ChatUser/Register")]
        public IHttpActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            string passwordHash = BC.HashPassword(registerDto.Password);

            var newUser = new User {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PhotoLink = registerDto.Photolink,
                Password = passwordHash
            };

            _context.ChatUsers.Add(newUser);
            _context.SaveChanges();

            var token = Utils.Utils.GenerateJwtToken(new
            {
                username = newUser.Email,
                userId = newUser.Id,
                photoLink = newUser.PhotoLink,
                name = newUser.Name
            });

            
            return Ok(new { email = newUser.Email, name = newUser.Name, token = token });
        }
    }
}
