using Microsoft.AspNetCore.Components.Authorization;

namespace AssetTrackUI.Features.Authentication
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddCollectGoAuthProvider(this IServiceCollection services)
        {
            return services.AddScoped<AuthenticationStateProvider, AssetTrackAuthProvider>();
        }
    }
}
