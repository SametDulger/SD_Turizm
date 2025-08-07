namespace SD_Turizm.Core.DTOs
{
    public class PaginationRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";
        public string? SearchTerm { get; set; }
    }

    public class PaginationResponseDto<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public static class PaginationExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PaginationRequestDto request)
        {
            var skip = (request.Page - 1) * request.PageSize;
            return query.Skip(skip).Take(request.PageSize);
        }

        public static async Task<PaginationResponseDto<T>> ToPagedListAsync<T>(this IQueryable<T> query, PaginationRequestDto request)
        {
            var totalCount = await Task.FromResult(query.Count());
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            
            var data = await Task.FromResult(query.ApplyPagination(request).ToList());

            return new PaginationResponseDto<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasNextPage = request.Page < totalPages,
                HasPreviousPage = request.Page > 1
            };
        }
    }
} 