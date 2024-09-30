namespace AssetTrackApi.Models
{
    public class SubLocation
    {
        public long Id { get; set; }
        public long LocationId { get; set; }
        public string Name { get; set; }

        public Location Location { get; set; }
    }
}
