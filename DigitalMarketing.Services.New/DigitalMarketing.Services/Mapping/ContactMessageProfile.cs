using AutoMapper;
using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.Mapping
{
    public class ContactMessageProfile : Profile
    {
        public ContactMessageProfile()
        {
            CreateMap<ContactMessage, ContactMessageDto>();

            CreateMap<CreateContactMessageDto, ContactMessage>();
                //.ForMember(dest => dest.web, opt => opt.Ignore()); // Honeypot فقط برای Validation، ذخیره نمی‌شه
        }
    }
}
