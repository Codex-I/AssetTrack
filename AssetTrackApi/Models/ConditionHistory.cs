namespace AssetTrackApi.Models
{
    public class ConditionHistory
    {
        public long Id { get; private set; }
        public Guid AssetId { get; private set; }
        public AssetCondition PreviousCondition { get; private set; }
        public AssetCondition NewCondition { get; private set; }
        public DateTime ChangeDate { get; private set; }

        public Asset Asset { get; private set; }
    }

}
