namespace AssetTrackApi.Models
{
    public class SubCategory
    {
        public SubCategory(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
            Description = description;
            Id = GenerateUniqueId(name);
        }

        public string Id { get; private set; }
        public string CategoryId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }


        private static string GenerateUniqueId(string name)
        {
            var normalizedName = name.ToLower().Replace(" ", "-");
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var hash = Guid.NewGuid().ToString().Substring(0, 8);
            return $"{normalizedName}-{timestamp}-{hash}";
        }
    }
}
