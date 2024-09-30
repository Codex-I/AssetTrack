using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackApi.Controllers.Base;
using AssetTrackApi.Contracts.QueryHelpers;
using AssetTrackApi.Contracts;
using AssetTrackApi.Models;

namespace AssetTrackApi.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController(ControllerParameters services) : SecureControllerBase(services)
    {
        // GET: api/Assets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAssets(
            [FromQuery] PaginationQuery paging,
            [FromQuery] string? searchText)
        {
            var query = Context.Users
                .Where(u => u.Id == CurrentUserId)
                .Include(u => u.ManagedAssets)
                .ThenInclude(a => a.Category)
                .Include(a => a.ManagedAssets)
                .ThenInclude(a => a.Location)
                .SelectMany(u => u.ManagedAssets);

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(a => a.Name.StartsWith(searchText));

            return Ok(await ToPagedListAsync(query, paging, x => new AssetResponse(x)));
        }

        // GET: api/Assets/5
        [HttpGet("{assetId}")]
        public async Task<ActionResult<AssetResponse>> GetAsset([FromRoute] Guid assetId)
        {
            var asset = await Context.Users
                .Where(U => U.Id == CurrentUserId)
                .Include(a => a.ManagedAssets)
                .ThenInclude(a => a.Category)
                .ThenInclude(a => a.SubCategory)
                .Include(a => a.ManagedAssets)
                .ThenInclude(a => a.Location)
                .ThenInclude(l => l.SubLocations)
                .SelectMany(a => a.ManagedAssets)
                .Where(a => a.Id == assetId)
                .SingleOrDefaultAsync();

            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            return Ok(new AssetResponse(asset));
        }

        // POST: api/Assets
        [HttpPost]
        public async Task<ActionResult<AssetResponse>> CreateAsset([FromBody] AssetCreateRequest assetRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = await Context.Users.FindAsync(CurrentUserId);
            if (owner == null)
                return BadRequest(new { message = "Invalid User ID" });

            var category = await Context.Categories.FindAsync(assetRequest.CategoryId);
            if (category == null)
                return BadRequest(new { message = "Invalid Category ID" });

            var asset = new Asset(
                category,
                assetRequest.Name,
                assetRequest.Condition,
                assetRequest.AcquisitionDate,
                assetRequest.WarrantyExpiry
            );

            owner.ManagedAssets.Add(asset);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsset), new { assetId = asset.Id }, new AssetResponse(asset));
        }

        // PUT: api/Assets/5
        [HttpPut("{assetId}")]
        public async Task<ActionResult<AssetResponse>> UpdateAsset([FromRoute] Guid assetId,
            [FromBody] UpdateAssetRequest request)
        {
            var asset = await Context.Users
                .Where(u => u.Id == CurrentUserId)
                .Include(u => u.ManagedAssets)
                .SelectMany(u => u.ManagedAssets)
                .SingleOrDefaultAsync(a => a.Id == assetId);

            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            // Update the asset fields
            asset.UpdateName(request.Name);
            asset.UpdateCondition(request.Condition);

            // Update the location and sublocation of the asset
            if (request.LocationId.HasValue)
            {
                var location = await Context.Locations
                    .Include(l => l.SubLocations)
                    .FirstOrDefaultAsync(l => l.Id == request.LocationId);

                if (location == null)
                    return BadRequest(new { message = "Invalid Location ID" });

                var subLocation = request.SubLocationId.HasValue ?
                     location.SubLocations.FirstOrDefault(s => s.Id == request.SubLocationId) : null;
                asset.AssignLocation(location, subLocation);
            }

            asset.UpdateWarrantyExpiry(request.WarrantyExpiry);
            asset.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await Context.SaveChangesAsync();

            return Ok(new AssetResponse(asset));
        }

        // PUT: api/Assets/AssignUser/{id}
        [HttpPut("AssignUser/{assetId}")]
        public async Task<ActionResult<AssetResponse>> AssignUser(Guid assetId, [FromBody] AssignUserRequest request)
        {
            var asset = await Context.Users
                .Where(u => u.Id == CurrentUserId)
                .Include(u => u.ManagedAssets)
                .SelectMany(u => u.ManagedAssets)
                .SingleOrDefaultAsync(a => a.Id == assetId);

            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            // Assign the asset to the new user (represented by a simple string)
            asset.AssignToUser(request.AssignedUser);

            await Context.SaveChangesAsync();

            return Ok(new AssetResponse(asset));
        }

        // DELETE: api/Assets/5
        [HttpDelete("{assetId}")]
        public async Task<ActionResult> DeleteAsset([FromRoute] Guid assetId)
        {
            var owner = await Context.Users
                .Include(u => u.ManagedAssets)
                .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (owner == null)
                return BadRequest(new { message = "Invalid User ID." });

            var asset = owner.ManagedAssets.FirstOrDefault(a => a.Id == assetId);

            if (asset == null)
                return NotFound(new { message = "Asset not found." });

            owner.ManagedAssets.Remove(asset);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
