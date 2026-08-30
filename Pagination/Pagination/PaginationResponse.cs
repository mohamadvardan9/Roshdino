 

namespace DigitalMarketing.Web.Pagination
{
    public class PaginationResponse<T>
    {
        public IReadOnlyList<T> Items { get; init; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

    }
}
