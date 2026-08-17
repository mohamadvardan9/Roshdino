using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IProductCategoryRepository
    {
        /// <summary>
        /// Retrieves all product categories.
        /// </summary>
        /// <returns>
        /// The task result contains a list of all product categories.
        /// </returns>
        Task<List<ProductCategory>> GetAllAsync();
        /// <summary>
        /// Retrieves a product category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product category.</param>
        /// <returns>
        /// The task result contains the product category if found; otherwise, null.
        /// </returns>
        Task<ProductCategory?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves a product category by its slug.
        /// </summary>
        /// <param name="slug">The slug value used to identify the product category.</param>
        /// <returns>
        /// The task result contains the product category if found; otherwise, null.
        /// </returns>
        Task<ProductCategory?> GetBySlugAsync(string slug);






        /// <summary>
        /// Adds a new product category to the repository.
        /// </summary>
        /// <param name="category">The product category entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous add operation.
        /// </returns>
        Task AddAsync(ProductCategory category);
        /// <summary>
        /// Updates an existing product category in the repository.
        /// </summary>
        /// <param name="category">The product category entity containing updated values.</param>
        void Update(ProductCategory category);
        /// <summary>
        /// Marks a product category as deleted.
        /// </summary>
        /// <param name="category">The product category entity to delete.</param>
        void Delete(ProductCategory category);







        /// <summary>
        /// Checks whether a product category with the specified slug already exists.
        /// </summary>
        /// <param name="slug">The slug value to check.</param>
        /// <param name="excludeId">
        /// The identifier of the category to exclude from the check, usually used during updates.
        /// </param>
        /// <returns>
        /// The task result is true if a matching slug exists; otherwise, false.
        /// </returns>
        Task<bool> SlugExistsAsync(string slug,int? excludeId = null);
        /// <summary>
        /// Determines whether the specified product category contains any products.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the product category.</param>
        /// <returns>
        /// The task result is true if the category has one or more products; otherwise, false.
        /// </returns>
        Task<bool> HasProductsAsync(int categoryId);
        Task SaveChangesAsync();






        // for search
        Task<IReadOnlyList<ProductCategory>> SearchAsync(string query, int limit);
    }
}
