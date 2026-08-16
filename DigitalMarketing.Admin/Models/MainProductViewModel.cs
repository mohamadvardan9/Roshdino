namespace DigitalMarketing.Admin.Models
{
    public class MainProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? ImageName { get; set; }
    }
}
