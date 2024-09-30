namespace AssetTrackUI.Core.API
{
    public partial class Client
    {
        public Client(System.Net.Http.HttpClient httpClient): this(httpClient.BaseAddress!.ToString(), httpClient)
        {
        }
    }
}
