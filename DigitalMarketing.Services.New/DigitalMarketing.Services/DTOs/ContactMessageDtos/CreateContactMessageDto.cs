using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Services.DigitalMarketing.Services.DTOs.ContactMessageDtos
{
    public class CreateContactMessageDto
    {
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; } = null!;


        // Honeypot: فیلد مخفی که فقط ربات ها پرش می کنن. کاربر واقعی نمی بینش
        public string? Website { get; set; }
    }
}
