using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IArticleService
    {
        /// <summary>
        /// Retrieves all articles from the system.
        /// </summary>
        /// <returns>
        /// A list of articles.
        /// </returns>
        Task<List<ArticleDto>> GetAllAsync();
        /// <summary>
        /// Retrieves all published articles that are available for display.
        /// </summary>
        /// <returns>
        /// A list of published articles.
        /// </returns>
        Task<List<ArticleDto>> GetPublishedAsync();
        /// <summary>
        /// Retrieves a article by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the article.
        /// </param>
        /// <returns>
        /// The article information if found; otherwise, null.
        /// </returns>
        Task<ArticleDto?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves a article by its unique slug value.
        /// </summary>
        /// <param name="slug">
        /// The unique slug of the article.
        /// </param>
        /// <returns>
        /// The article information if found; otherwise, null.
        /// </returns>
        Task<ArticleDto?> GetBySlugAsync(string slug);
        /// <summary>
        /// Retrieves all articles belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">
        /// The identifier of the articles category.
        /// </param>
        /// <returns>
        /// A list of articles associated with the specified category.
        /// </returns>
        Task<List<ArticleDto>> GetByCategoryAsync(int categoryId);








        Task<ServiceResult> CreateAsync(CreateArticleDto dto);
        Task<ServiceResult> UpdateAsync(UpdateArticleDto dto, Action<string>? deleteOldCoverCallback = null);
        Task<ServiceResult> DeleteAsync(int id);







        /// <summary>
        /// Changes the publication status of a article.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the article.
        /// </param>
        /// <returns>
        /// A <see cref="ServiceResult"/> indicating whether the publish status
        /// was successfully changed.
        /// </returns>
        Task<ServiceResult> TogglePublishAsync(int id);
    }
}
