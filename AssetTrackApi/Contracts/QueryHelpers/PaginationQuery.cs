using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetTrackApi.Contracts.QueryHelpers
{
    public class PaginationQuery
    {
        public PaginationQuery() { }
        public PaginationQuery(int pageSize = 20)
        {
            PageSize = pageSize;
        }

        [Range(1, 1000)]
        public int PageNumber { get; set; } = 1;

        [Range(5, 500)]
        public int PageSize { get; set; } = 20;

        [BindNever]
        public int SkipCount => (PageNumber - 1) * PageSize;
    }
}
