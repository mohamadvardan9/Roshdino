namespace DigitalMarketing.Web.Pagination
{
    public interface IPaginationService
    {
        Task<PaginationResponse<T>> PaginateAsync<T>(IQueryable<T> query, PaginationRequest request,
            CancellationToken cancellationToken = default);
    }
}
