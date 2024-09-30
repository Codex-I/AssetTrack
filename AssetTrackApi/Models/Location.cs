namespace AssetTrackApi.Models
{
    public class Location
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public virtual ICollection<SubLocation> SubLocations { get; private set; } = [];
        public virtual ICollection<Asset> Assets { get; private set; }  // Assets stored in this location
    }
}
