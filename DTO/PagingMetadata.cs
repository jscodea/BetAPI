using BetAPI.Models;

namespace BetAPI.DTO
{
    public class PagingMetadata
    {
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
