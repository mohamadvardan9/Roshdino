using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products from the system.
        /// </summary>
        /// <returns>
        /// A list of products.
        /// </returns>
        Task<List<ProductDto>> GetAllAsync();
        /// <summary>
        /// Retrieves all published products that are available for display.
        /// </summary>
        /// <returns>
        /// A list of published products.
        /// </returns>
        Task<List<ProductDto>> GetPublishedAsync();
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the product.
        /// </param>
        /// <returns>
        /// The product information if found; otherwise, null.
        /// </returns>
        Task<ProductDto?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves a product by its unique slug value.
        /// </summary>
        /// <param name="slug">
        /// The unique slug of the product.
        /// </param>
        /// <returns>
        /// The product information if found; otherwise, null.
        /// </returns>
        Task<ProductDto?> GetBySlugAsync(string slug);
        /// <summary>
        /// Retrieves all products belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">
        /// The identifier of the product category.
        /// </param>
        /// <returns>
        /// A list of products associated with the specified category.
        /// </returns>
        Task<List<ProductDto>> GetByCategoryAsync(int categoryId);





        Task<ServiceResult> CreateAsync(CreateProductDto dto);
        Task<ServiceResult> UpdateAsync(UpdateProductDto dto);
        Task<ServiceResult> DeleteAsync(int id);





        /// <summary>
        /// Removes a product image from the specified product and handles
        /// deletion of the associated physical file.
        /// </summary>
        /// <param name="imageId">
        /// The unique identifier of the image to remove.
        /// </param>
        /// <param name="productId">
        /// The unique identifier of the product that owns the image.
        /// </param>
        /// <param name="deleteFileCallback">
        /// A callback function responsible for deleting the physical image file.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult"/> indicating whether the image removal
        /// operation was successful or failed due to business rules.
        /// </returns>
        Task<ServiceResult> RemoveImageAsync(int imageId, int productId);
        /// <summary>
        /// Sets the specified product image as the main image.
        /// </summary>
        /// <param name="productId">
        /// The unique identifier of the product.
        /// </param>
        /// <param name="imageId">
        /// The unique identifier of the image that should be set as the main image.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult"/> indicating whether the main image update
        /// operation was successful.
        /// </returns>
        Task<ServiceResult> SetMainImageAsync(int productId, int imageId);
        /// <summary>
        /// Changes the publication status of a product.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the product.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult"/> indicating whether the publish status
        /// was successfully changed.
        /// </returns>
        Task<ServiceResult> TogglePublishAsync(int id);



    }
}
