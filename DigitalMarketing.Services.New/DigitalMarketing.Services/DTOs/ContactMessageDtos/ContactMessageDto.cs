using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos
{
    public class ContactMessageDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public required string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
