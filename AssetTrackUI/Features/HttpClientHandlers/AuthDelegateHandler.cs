using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace AssetTrackUI.Features.HttpClientHandlers
{
    public class AuthDelegateHandler(AuthenticationStateProvider auth) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = await ((Authentication.AssetTrackAuthProvider)auth).GetAccessToken();

            if (token != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);

            ////write json responses to console
            //var response = await base.SendAsync(request, cancellationToken);
            //var res = await response.Content.ReadAsStringAsync(cancellationToken);
            //Debug.WriteLine(res);
            //return response;
        }
    }
}
