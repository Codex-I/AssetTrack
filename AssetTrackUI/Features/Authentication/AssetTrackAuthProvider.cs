using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using AssetTrackUI.Core.API;

namespace AssetTrackUI.Features.Authentication
{
    public class AssetTrackAuthProvider(ILocalStorageService storage) : AuthenticationStateProvider
    {
        private readonly string storageKey = "UserSession";
        private readonly AuthenticationState anonymousState = new(new(new ClaimsIdentity()));

        public class AuthUser
        {
            public string? Id { get; init; }
            public string? Email { get; init; }
            public string? FullName { get; init; }
            public string? AvatarUrl { get; init; }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await storage.GetItemAsync<UserLoginResponse>(storageKey);

                if (response == null)
                    return anonymousState;

                return CreateAuthenticationState(response.AccessToken);
            }
            catch
            {
                return anonymousState;
            }
        }

        public async Task<AuthUser> GetAuthUser()
        {
            var response = await storage.GetItemAsync<UserLoginResponse>(storageKey)
                ?? throw new InvalidOperationException("Please login");

            var principal = GetClaimsPrincipal(response.AccessToken);

            return new AuthUser
            {
                Id = Find(ClaimTypes.Sid),
                Email = Find(ClaimTypes.Email),     //Find("email"),
                FullName = Find(ClaimTypes.Name),   //Find("unique_name")
                AvatarUrl = Find(ClaimTypes.Uri),
            };

            string? Find(string key)
            {
                return principal.Claims.FirstOrDefault(
                    x => x.Type == key)?.Value;
            }
        }

        public async Task<string?> GetAccessToken()
        {
            var response = await storage.GetItemAsync<UserLoginResponse>(storageKey);

            if (response != null)
                return response.AccessToken;

            return null;
        }

        public async Task SaveAuthenticationState(UserLoginResponse response)
        {
            await storage.SetItemAsync(storageKey, response);

            var authenticatedState = CreateAuthenticationState(response.AccessToken);
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
        }

        public async Task ClearAuthenticationState()
        {
            await storage.RemoveItemAsync(storageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
        }

        private static AuthenticationState CreateAuthenticationState(string accessToken)
        {
            var principal = GetClaimsPrincipal(accessToken);
            return new AuthenticationState(principal);
        }

        private static ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var identity = new ClaimsIdentity(jwtToken.Claims, "JwtAuth");

            return new ClaimsPrincipal(identity);
        }

    }
}