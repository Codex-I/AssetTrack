namespace AssetTrackApi.Contracts.QueryHelpers
{
    internal class StringSearchQuery
    {
        internal string[] SearchStrings { get; private set; } = [];

        public string SearchText
        {
            get
            {
                return string.Join(' ', SearchStrings);
            }
            set
            {
                char[] separators = [' '];
                SearchStrings = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        internal bool CanSearch => SearchStrings.Length > 0;
    }
}
