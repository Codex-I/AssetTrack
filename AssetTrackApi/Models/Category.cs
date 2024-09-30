namespace AssetTrackApi.Models
{
    public class Category
    {
        public Category(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
            Description = description;
            Id = GenerateUniqueId(name); // Generate ID based on category name
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public SubCategory SubCategory { get; private set; }

        // One-to-many relationship with Assets
        public virtual ICollection<Asset> Assets { get; private set; } = new List<Asset>();

        // Method to update category name and description
        public void UpdateCategory(string name, string? description = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
            Description = description;
            Id = GenerateUniqueId(name); // Regenerate the ID based on the updated name
        }

        // Method to assign a subcategory
        public void AssignSubCategory(SubCategory subCategory)
        {
            ArgumentNullException.ThrowIfNull(subCategory);
            SubCategory = subCategory;
        }

        // Method to generate a unique ID based on the category name and a timestamp
        private static string GenerateUniqueId(string name)
        {
            // Use the name in lowercase to keep consistency, combined with a timestamp and a short hash for uniqueness
            var normalizedName = name.ToLower().Replace(" ", "-"); // Replace spaces with hyphens
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"); // High precision timestamp
            var hash = Guid.NewGuid().ToString()[..8]; // Short random string from a GUID

            // Combine all components to form the ID
            return $"{normalizedName}-{timestamp}-{hash}";
        }
    }
}
