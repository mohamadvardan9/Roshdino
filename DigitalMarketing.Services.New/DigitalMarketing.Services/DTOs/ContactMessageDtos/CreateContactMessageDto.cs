using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos
{
    public class CreateContactMessageDto
    {
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public required string Message { get; set; }


        // Honeypot: فیلد مخفی که فقط ربات ها پرش می کنن. کاربر واقعی نمی بینش
        public string? Website { get; set; }
    }
}
