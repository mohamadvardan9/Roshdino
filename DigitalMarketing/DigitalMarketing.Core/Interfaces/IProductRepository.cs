using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetPublishedAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetBySlugAsync(string slug);
        Task<List<Product>> GetByCategoryAsync(int categoryId);

        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product); // Soft delete 

        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task SaveChangesAsync();


        // new added
        Task<ProductImage?> GetImageByIdAsync(int imageId);
        void RemoveImage(ProductImage image);
        Task SetMainImageAsync(int productId ,int imageId);
    }
}
