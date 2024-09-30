namespace AssetTrackApi.Contracts.QueryHelpers
{
    internal class DateRangeQuery(DateTime? dateFrom, DateTime? dateTo)
    {
        public DateTime? DateFrom { get; private set; } = dateFrom;

        public DateTime? DateTo { get; private set; } = dateTo;

        internal bool CanSearch => DateFrom.HasValue && DateTo.HasValue;
    }
}
