using AssetTrackApi.Models;

namespace AssetTrackApi.Contracts
{
    public class CategoryCreateRequest
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
    }

    public class AssignSubCategoryRequest
    {
        public required string SubCategoryId { get; init; }
    }

    public class CategoryResponse(Category category)
    {
        public string Id { get; } = category.Id;
        public string Name { get; } = category.Name;
        public string SubCategory { get; } = category.SubCategory.Name;
        public List<AssetDto> Assets { get; } = category.Assets.Select(asset => new AssetDto(asset)).ToList();

        public class AssetDto(Asset asset)
        {
            public Guid Id { get; } = asset.Id;
            public string Name { get; } = asset.Name;
            public DateTime AcquisitionDate { get; } = asset.AcquisitionDate;
            public string Owner { get; } = asset.Owner;
        }
    }
}
