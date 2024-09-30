namespace AssetTrackApi.Models
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string Description { get; set; }  // Description of the maintenance task
        public DateTime ScheduledDate { get; set; }  // Date the maintenance is scheduled
        public bool Completed { get; set; }  // Whether the maintenance task was completed
        public DateTime? CompletionDate { get; set; }  // When it was completed (if applicable)

        public Asset Asset { get; set; }
    }
}
