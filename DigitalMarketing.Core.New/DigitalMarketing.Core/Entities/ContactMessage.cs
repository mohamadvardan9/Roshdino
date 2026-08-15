using DigitalMarketing.DigitalMarketing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Core.DigitalMarketing.Core.Entities
{
    public class ContactMessage : BaseEntity
    {
        public required string FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public required string Message { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
