using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<List<ArticleCategoryDto>> GetAllAsync();
        Task<ArticleCategoryDto?> GetByIdAsync(int id);
        Task<ArticleCategoryDto?> GetBySlugAsync(string slug);



        /// <summary>
        /// Creates a new article category after validating the input data
        /// and ensuring the generated slug is unique.
        /// </summary>
        /// <param name="dto">The data required to create a new article category.</param>
        /// <returns>
        /// A service result indicating whether the operation was successful.
        /// Contains validation or business errors if the creation fails.
        /// </returns>
        Task<ServiceResult> CreateAsync(CreateArticleCategoryDto dto);
        /// <summary>
        /// Updates an existing article category after validating the provided data
        /// and checking slug uniqueness.
        /// </summary>
        /// <param name="id">The unique identifier of the article category to update.</param>
        /// <param name="dto">The updated article category data.</param>
        /// <returns>
        /// A service result indicating whether the update operation was successful.
        /// Contains validation or business errors if the update fails.
        /// </returns>
        Task<ServiceResult> UpdateAsync(int id , UpdateArticleCategoryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
