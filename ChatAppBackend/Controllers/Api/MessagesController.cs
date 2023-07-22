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
            var chatsForUser = _context.Messages.Where(c => c.SenderId == id || c.ReceiverId == id);

            var discussions = new Dictionary<int, Message>();

            foreach (var chat in chatsForUser)
            {
                var otherPersonId = chat.SenderId == id ? chat.ReceiverId.Value : chat.SenderId.Value;
                if (!discussions.ContainsKey(otherPersonId))
                    discussions.Add(otherPersonId, chat);
                else if (DateTime.Compare(discussions[otherPersonId].DateTime, chat.DateTime) < 0)
                    discussions[otherPersonId] = chat;
            }
            

            var data = discussions.Select(ch => {
                var secondPerson = GetSecondPerson(id, ch.Value);

                return new DiscussionSummaryDto
                {
                    UserId = secondPerson.Id,
                    Name = secondPerson.Name,
                    LastMessage = ch.Value.Content,
                    LastMessageTimeMarker = GetTimeMarkerForTime(ch.Value.DateTime),
                    PhotoLink = secondPerson.PhotoLink
                };
            });


            return Ok(data);
        }

        private User GetSecondPerson(int id, Message message)
        {
            var secondPersonId = message.SenderId == id ? message.ReceiverId : message.SenderId;
            return _context.ChatUsers.Single(u => u.Id == secondPersonId);
        }

        private string GetTimeMarkerForTime(DateTime dateTime)
        {
            var prevTime = dateTime;
            var duration = DateTime.Now - prevTime;

            if (duration.TotalDays <= 1)
                return $"{duration.Hours}h{duration.Minutes}";
            else return $"{prevTime.Day}-{prevTime.Month}-{prevTime.Year}";
        }
    }
}
