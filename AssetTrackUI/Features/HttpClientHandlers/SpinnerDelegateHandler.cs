using AssetTrackUI.Services;

namespace AssetTrackUI.Features.HttpClientHandlers
{
    public class SpinnerDelegateHandler(SpinnerService spinnerService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                spinnerService.Show();
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                spinnerService.Hide();
            }
        }
    }
}
