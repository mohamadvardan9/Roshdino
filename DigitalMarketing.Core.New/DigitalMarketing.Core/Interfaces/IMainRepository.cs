using DigitalMarketing.DigitalMarketing.Core.Entities;


namespace DigitalMarketing.Core.DigitalMarketing.Core.Interfaces
{
    public interface IMainRepository
    {
        /// <summary>
        /// Gets the total number of articles
        /// </summary>
        Task<int> GetArticlesCountAsync();
        /// <summary>
        /// Gets the total number of products
        /// </summary>
        Task<int> GetProductsCountAsync();

        /// <summary>
        /// Gets the total number of draft articles.
        /// </summary>
        /// <returns>
        /// The number of articles that have not been published.
        /// </returns>
        Task<int> GetDraftArticlesCountAsync();
        /// <summary>
        /// Gets the total number of draft products.
        /// </summary>
        /// <returns>
        /// The number of products that have not been published.
        /// </returns>
        Task<int> GetDraftProductsCountAsync();

        /// <summary>
        /// Gets the total number of UnreadMessages.
        /// </summary>
        Task<int> GetUnreadMessagesCountAsync();






        /// <summary>
        /// Gets the most recently created articles.
        /// </summary>
        /// <param name="count">
        /// The maximum number of articles to retrieve.
        /// </param>
        /// <returns>
        /// A list containing the latest articles ordered by creation date in descending order.
        /// </returns>
        Task<List<Article>> GetLatestArticlesAsync(int count);
        /// <summary>
        /// Gets the most recently created products.
        /// </summary>
        /// <param name="count">
        /// The maximum number of products to retrieve.
        /// </param>
        /// <returns>
        /// A list containing the latest products ordered by creation date in descending order.
        /// </returns>
        Task<List<Product>> GetLatestProductsAsync(int count);





        // برای محاسبه‌ی رشد واقعی: تعداد مقاله/محصولی که تو یه بازه‌ی زمانی خاص ساخته شدن
        /// <summary>
        /// Gets the number of articles created on or after the specified date.
        /// </summary>
        /// <param name="since">
        /// The starting date used to filter articles.
        /// </param>
        /// <returns>
        /// The number of articles created since the specified date.
        /// </returns>
        Task<int> GetArticlesCountSinceAsync(DateTime since);
        /// <summary>
        /// Gets the number of products created on or after the specified date.
        /// </summary>
        /// <param name="since">
        /// The starting date used to filter products.
        /// </param>
        /// <returns>
        /// The number of products created since the specified date.
        /// </returns>
        Task<int> GetProductsCountSinceAsync(DateTime since);
    }
}
