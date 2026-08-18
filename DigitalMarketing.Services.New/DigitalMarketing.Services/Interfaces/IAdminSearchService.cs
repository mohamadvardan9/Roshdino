using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminSearch;


namespace DigitalMarketing.Services.DigitalMarketing.Services.Interfaces
{
    public interface IAdminSearchService
    {
        /// <summary>
        /// Performs a global search across the administrative modules, including articles,
        /// products, contact messages, article categories, and product categories.
        /// </summary>
        /// <param name="query">
        /// The search keyword used to find matching records. Leading and trailing
        /// whitespace is automatically removed.
        /// </param>
        /// <param name="limit">
        /// The maximum number of search results to return. The default value is <c>10</c>.
        /// </param>
        /// <returns>
        /// A read-only collection of <see cref="AdminSearchResultDto"/> objects containing
        /// the matched search results from all supported modules. If the search query is
        /// null, empty, or contains only whitespace, an empty collection is returned.
        /// </returns>
        /// <remarks>
        /// This method aggregates results from multiple repositories into a unified search
        /// result collection suitable for the administration panel's global search feature.
        /// Each result includes its title, type, icon, and navigation URL.
        /// </remarks>
        Task<IReadOnlyList<AdminSearchResultDto>> SearchAsync(string query, int limit = 10);
    }
}
