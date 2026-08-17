using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IArticleCategoryRepository   
    {
        /// <summary>
        /// Retrieves all available article categories from the data source asynchronously.
        /// </summary>
        /// <returns>
        /// A task representing an asynchronous operation that returns a list of all article categories by name.
        /// </returns>
        Task<List<ArticleCategory>> GetAllAsync();
        /// <summary>
        /// Retrieves an article category by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the article category.</param>
        /// <returns>
        /// The task result contains the article category if found; otherwise, null.
        /// </returns>
        Task<ArticleCategory?> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves an article category by its unique slug asynchronously.
        /// </summary>
        /// <param name="slug">The unique slug of the article category.</param>
        /// <returns>
        /// The task result contains the article category if found; otherwise, null.
        /// </returns>
        Task<ArticleCategory?> GetBySlugAsync(string slug);





        Task AddAsync(ArticleCategory category);
        void Update(ArticleCategory category);
        /// <summary>
        /// Marks the specified article category as deleted by setting its IsDeleted property to true.
        /// </summary>
        /// <param name="category">The article category to mark as deleted.</param>
        void Delete(ArticleCategory category);







        /// <summary>
        /// Determines whether an article category with the specified slug exists.
        /// </summary>
        /// <param name="slug">The slug to check for existence.</param>
        /// <param name="excludeId">
        /// The unique identifier of an article category to exclude from the existence check.
        /// This parameter is useful when updating an existing category.
        /// </param>
        /// <returns>
        /// The task result is true if a matching article category exists; otherwise, false.
        /// </returns>
        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        /// <summary>
        /// Determines whether any articles are associated with the specified article category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the article category.</param>
        /// <returns>
        /// The task result is true if one or more articles are associated with the specified category; otherwise, false.
        /// </returns>
        Task<bool> HasArticlesAsync(int categoryId);
        Task SaveChangesAsync();




        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<IReadOnlyList<ArticleCategory>> SearchAsync(string query, int limit);
    }
}
