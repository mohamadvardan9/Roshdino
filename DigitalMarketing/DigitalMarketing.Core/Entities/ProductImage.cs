namespace DigitalMarketing.DigitalMarketing.Core.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; } = null!;
        public bool IsMain { get; set; } = false;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
