using AssetTrackUI.Services;

namespace AssetTrackUI
{
    public class ApiDataSource<T>
    {
        public ApiDataSource<T> SetPage(PageState state)
        {
            PageState = state;
            return this;
        }

        public ApiDataSource<T> SetFilter(FilterState state)
        {
            FilterState = state;
            return this;
        }

        public async Task LoadData(AlertService alert)
        {
            try
            {
                if (Callback == null)
                    throw new Exception("Callback must be initialized first");

                IsLoading = true;
                ErrorMessage = string.Empty;

                var response = await Callback.Invoke(PageState, FilterState);

                Items = response.ToList() as PagedList<T>;
                if (Items != null)
                {
                    PageState = Items.PageState;
                }
                throw new Exception("Items is empty!");
            }
            catch (Exception ex)
            {
                Items = null;
                ErrorMessage = ex.Message;
                await alert.Confirm(ex.Message, "Api Error");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public Func<PageState, FilterState?, Task<ICollection<T>>>? Callback { get; set; }
        public PagedList<T>? Items { get; private set; }
        public PageState PageState { get; private set; } = new(10, 1);
        public FilterState? FilterState { get; private set; }
        public bool IsLoading { get; private set; } = false;
        public string? ErrorMessage { get; private set; }
    }

    public record class SortState();
    public record class FilterState();
}
