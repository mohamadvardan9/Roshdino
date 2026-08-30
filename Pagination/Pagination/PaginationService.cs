using Microsoft.EntityFrameworkCore;

namespace DigitalMarketing.Web.Pagination
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginationResponse<T>> PaginateAsync<T>(IQueryable<T> query,
            PaginationRequest request,
            CancellationToken cancellationToken = default)
        {
            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);


            return new PaginationResponse<T>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            };
        }
    }
}
