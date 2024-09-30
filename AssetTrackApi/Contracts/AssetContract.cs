using AssetTrackApi.Models;

namespace AssetTrackApi.Contracts
{
    public class AssetCreateRequest
    {
        public required string Name { get; init; }
        public required long CategoryId { get; init; }
        public required AssetCondition Condition { get; init; }
        public required DateTime AcquisitionDate { get; init; }
        public DateTime? WarrantyExpiry { get; init; }
    }

    public class UpdateAssetRequest
    {
        public required string Name { get; init; }
        public required AssetCondition Condition { get; set; }
        public long? LocationId { get; init; }
        public long? SubLocationId { get; init; }
        public DateTime? WarrantyExpiry { get; init; }
    }

    public class AssignUserRequest
    {
        public required string AssignedUser { get; init; }
    }

    public class AssetResponse(Asset asset)
    {
        public Guid Id { get; } = asset.Id;
        public string Name { get; } = asset.Name;
        public string AssetTag { get; } = asset.AssetTag;
        public string Category { get; } = asset.Category.Name;
        public string Location { get; } = asset.Location.Name;
        public string SubLocation { get; } = asset.SubLocation.Name;
        public string Owner { get; } = asset.Owner;
        public AssetCondition AssetCondition { get; } = asset.Condition;
        public DateTime AcquisitionDate { get; } = asset.AcquisitionDate;
        public DateTime? WarrantyExpiry { get; } = asset.WarrantyExpiry;
        public DateTime DateCreated { get; } = asset.CreatedAt;
        public DateTime? DateModified { get; } = asset.UpdatedAt;
    }

}
