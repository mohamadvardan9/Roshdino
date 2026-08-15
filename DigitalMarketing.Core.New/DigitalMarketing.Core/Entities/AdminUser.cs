using DigitalMarketing.DigitalMarketing.Core.Entities;


namespace DigitalMarketing.Core.DigitalMarketing.Core.Entities
{
    public class AdminUser : BaseEntity
    {
        public required string UserName { get; set; }
        public required string PassHash { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
