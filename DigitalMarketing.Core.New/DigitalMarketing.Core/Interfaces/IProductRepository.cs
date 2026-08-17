using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves all products including their related category and images.
        /// </summary>
        /// <returns>
        /// The task result contains a list of all products ordered by creation date descending.
        /// </returns>
        Task<List<Product>> GetAllAsync();
        /// <summary>
        /// Retrieves all published products including their related category and images.
        /// </summary>
        /// <returns>
        /// The task result contains a list of published products ordered by creation date descending.
        /// </returns>
        Task<List<Product>> GetPublishedAsync();
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// The task result contains the product with its related data if found; otherwise, null.
        /// </returns>
        Task<Product?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves a published product by its slug value.
        /// </summary>
        /// <param name="slug">The unique slug identifier of the product.</param>
        /// <returns>
        /// The task result contains the published product with its related data if found; otherwise, null.
        /// </returns>
        Task<Product?> GetBySlugAsync(string slug);
        /// <summary>
        /// Retrieves all published products belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the product category.</param>
        /// <returns>
        /// The task result contains a list of published products in the specified category.
        /// </returns>
        Task<List<Product>> GetByCategoryAsync(int categoryId);









        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);





        /// <summary>
        /// Checks whether a product with the specified slug already exists.
        /// </summary>
        /// <param name="slug">The slug value to check for existence.</param>
        /// <param name="excludeId">
        /// The identifier of the product to exclude from the check.
        /// This is typically used when updating an existing product to prevent matching itself.
        /// </param>
        /// <returns>
        /// The task result is true if a product with the specified slug exists; otherwise, false.
        /// </returns>
        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task SaveChangesAsync();








        /// <summary>
        /// Retrieves a product image by its unique identifier.
        /// </summary>
        /// <param name="imageId">The unique identifier of the product image.</param>
        /// <returns>
        /// The task result contains the product image if found; otherwise, null.
        /// </returns>
        Task<ProductImage?> GetImageByIdAsync(int imageId);
        /// <summary>
        /// Permanently removes a product image from the database.
        /// This operation performs a hard delete and should be used when the physical image file is also removed.
        /// </summary>
        /// <param name="image">The product image entity to remove.</param>
        void RemoveImage(ProductImage image);
        /// <summary>
        /// Sets the specified image as the main image of a product.
        /// All other images belonging to the same product will be marked as non-main.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="imageId">The unique identifier of the image to set as the main image.</param>
        Task SetMainImageAsync(int productId ,int imageId);






        // for search in _Layout.cshmtl
        Task<IReadOnlyList<Product>> SearchAsync(string query, int limit);
    }
}
