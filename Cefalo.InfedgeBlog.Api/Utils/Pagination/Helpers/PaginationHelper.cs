using Cefalo.InfedgeBlog.Api.Utils.Pagination.Filter;
using Cefalo.InfedgeBlog.Api.Utils.Pagination.Wrappers;

namespace Cefalo.InfedgeBlog.Api.Utils.Pagination.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPageCount = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPageCount));
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
