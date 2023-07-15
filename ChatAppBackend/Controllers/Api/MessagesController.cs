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
    public class MessagesController : ApiController
    {
        private ApplicationDbContext _context;

        public MessagesController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult GetMessages()
        {
            var data = _context.Messages.ToList()
                .Select(Mapper.Map<Message, MessageDto>);
            return Ok(data);
        }

        public IHttpActionResult GetMessage(int id)
        {
            var message = _context.Messages.SingleOrDefault(c => c.Id == id);
            if (message == null)
                return NotFound();
            return Ok(Mapper.Map<Message, MessageDto>(message));
        }

        [HttpPost]
        public IHttpActionResult SendMessage(MessageDto messageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var message = Mapper.Map<MessageDto, Message>(messageDto);
            message.DateTime = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
            messageDto.Id = message.Id;

            return Created(new Uri(Request.RequestUri + "/" + message.Id), messageDto);
        }

        [Route("Api/Messages/ForUser/{id}")]
        public IHttpActionResult GetChatsForUserId(int id)
        {
            var data = _context.Messages.ToList()
                .Where(m => m.SenderId == id || m.ReceiverId == id)
                .Select(Mapper.Map<Message, MessageDto>);
            return Ok(data);
        }

        [Route("Api/Messages/ForReceiverId/{userId}/{receiverId}")]
        public IHttpActionResult GetChatsForUserReceiverId(int userId, int receiverId)
        {

            var data = _context.Messages.ToList()
                .Where(m => m.SenderId == userId && m.ReceiverId == receiverId)
                .Select(Mapper.Map<Message, MessageDto>);
            return Ok(data);
        }

        [Route("Api/Messages/SummaryForUser/{id}")]
        public IHttpActionResult GetDiscussionsSummaryForUserId(int id)
        {
            // TODO
            return Ok("User Id : " + id);
        }
    }
}
