using AutoMapper;
using ChatAppBackend.Dto;
using ChatAppBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatAppBackend.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            Mapper.CreateMap<Message, MessageDto>();
            Mapper.CreateMap<MessageDto, Message>()
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}