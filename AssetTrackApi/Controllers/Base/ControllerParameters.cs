using AssetTrackApi.Configuration;
using AssetTrackApi.Data;

namespace AssetTrackApi.Controllers.Base
{
    public class ControllerParameters(
                    IHttpContextAccessor accessor,
                    AssetTrackContext dbContext,
                    Settings settings)
    {
        public HttpContext? HttpContext { get; } = accessor.HttpContext;
        public AssetTrackContext DbContext { get; } = dbContext;
        public Settings Settings { get; } = settings;
    }
}
