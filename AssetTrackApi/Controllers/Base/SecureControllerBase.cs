using AssetTrackApi.Configuration;
using AssetTrackApi.Contracts.QueryHelpers;
using AssetTrackApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetTrackApi.Controllers.Base
{
    //#if !DEBUG
    //    [Microsoft.AspNetCore.Authorization.Authorize]
    //#endif
    [ApiController]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public abstract class SecureControllerBase : ControllerBase
    {
        private readonly ClaimsPrincipal _principal;
        protected AssetTrackContext Context { get; }
        public SecureControllerBase(ControllerParameters services)
        {
            if (services.HttpContext == null)
                throw new Exception("services.HttpContext is NULL");

            _principal = services.HttpContext.User;
            Context = services.DbContext;
        }

        protected async Task<IEnumerable<TDestination>> ToPagedListAsync<TSource, TDestination>(
            IQueryable<TSource> source, PaginationQuery pg,
            Func<TSource, TDestination> converter)
        {
            var total = source.Count();
            var items = await source.Skip(pg.SkipCount).Take(pg.PageSize).ToListAsync();
            var list = new PagedList<TSource>(items, total, pg.PageNumber, pg.PageSize);

            Response.Headers[Settings.Headers.PAGE_SIZE] = list.PageSize.ToString();
            Response.Headers[Settings.Headers.PAGE_NUMBER] = list.PageNumber.ToString();
            Response.Headers[Settings.Headers.TOTAL_PAGES] = list.TotalPages.ToString();
            Response.Headers[Settings.Headers.TOTAL_ITEMS] = list.TotalItems.ToString();

            return list.Select(converter);
        }

        protected string CurrentUserId
        {
            get
            {
                if (_principal is null)
                    throw new UnauthorizedAccessException("Invalid LoginUser - principal");

                var claim = _principal.FindFirst(ClaimTypes.NameIdentifier) ??
                    throw new UnauthorizedAccessException();

                return claim.Value;
            }
        }
    }
}
