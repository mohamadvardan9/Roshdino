using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IArticleRepository
    {
        /// <summary>
        /// Retrieves all articles including their related category and images
        /// </summary>
        /// <returns>
        /// The task result contains a list of all articles ordered by publish date descending
        /// </returns>
        Task<List<Article>> GetAllAsync();
        /// <summary>
        /// Retrieves all published articles including their related category and images.
        /// </summary>
        /// <returns>
        /// The task result contains a list of published articles ordered by creation date descending.
        /// </returns>
        Task<List<Article>> GetPublishedAsync();
        /// <summary>
        /// Retrieves a article by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the article.</param>
        /// <returns>
        /// The task result contains the article with its related data if found; otherwise, null.
        /// </returns>
        Task<Article?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves a published article by its slug value.
        /// </summary>
        /// <param name="slug">The unique slug identifier of the article.</param>
        /// <returns>
        /// The task result contains the published article with its related data if found; otherwise, null.
        /// </returns>
        Task<Article?> GetBySlugAsync(string slug);
        /// <summary>
        /// Retrieves all published articles belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the article category.</param>
        /// <returns>
        /// The task result contains a list of published articles in the specified category.
        /// </returns>
        Task<List<Article>> GetByCategoryAsync(int categoryId);










        Task AddAsync(Article article);
        void Update(Article article);
        void Delete(Article article);






        /// <summary>
        /// Checks whether a article with the specified slug already exists.
        /// </summary>
        /// <param name="slug">The slug value to check for existence.</param>
        /// <param name="excludeId">
        /// The identifier of the article to exclude from the check.
        /// This is typically used when updating an existing article to prevent matching itself.
        /// </param>
        /// <returns>
        /// The task result is true if a article with the specified slug exists; otherwise, false.
        /// </returns>

        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task SaveChangesAsync();
        void RemoveImage(Article article);



        /// <summary>
        /// Searches articles by title or slug and returns
        /// a limited number of matching results.
        /// </summary>
        /// <param name="query">
        /// The search text used to match articles titles or slugs.
        /// </param>
        /// <param name="limit">
        /// The maximum number of results to return.
        /// </param>
        /// <returns>
        /// A read-only list of matching <see cref="Article"/> entities,
        /// ordered by creation date in descending order.
        /// </returns>
        Task<IReadOnlyList<Article>> SearchAsync(string query, int limit);
    }
}
