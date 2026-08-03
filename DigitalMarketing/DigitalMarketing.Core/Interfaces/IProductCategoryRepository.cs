using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetAllAsync();
        Task<ProductCategory?> GetByIdAsync(int id);
        Task<ProductCategory?> GetBySlugAsync(string slug);


        Task AddAsync(ProductCategory category);
        void UpdateAsync(ProductCategory category);
        void DeleteAsync(ProductCategory category);


        Task<bool> SlugExistsAsync(string slug,int? excludeId = null);
        Task<bool> HasProductsAsync(int categoryId);
        Task SaveChangesAsync();

    }
}
