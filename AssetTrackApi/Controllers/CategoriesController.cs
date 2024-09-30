using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackApi.Models;
using AssetTrackApi.Controllers.Base;
using AssetTrackApi.Contracts;

namespace AssetTrackApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController(ControllerParameters services) : SecureControllerBase(services)
    {
        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            // Get categories that have assets belonging to the current user
            var categories = await Context.Categories
                .Include(c => c.SubCategory)
                .Include(c => c.Assets.Where(a => a.Owner == CurrentUserId)) // Include assets owned by the current user
                .ToListAsync();

            return Ok(categories.Select(c => new CategoryResponse(c)));
        }

        // GET: api/categories/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryResponse>> GetCategory([FromRoute] string categoryId)
        {
            // Get the category only if it has assets belonging to the current user
            var category = await Context.Categories
                .Include(c => c.SubCategory)
                .Include(c => c.Assets.Where(a => a.Owner == CurrentUserId)) // Include assets owned by the current user
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(new CategoryResponse(category));
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryCreateRequest categoryRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new category
            var category = new Category(categoryRequest.Name, categoryRequest.Description);

            Context.Categories.Add(category);
            await Context.SaveChangesAsync();

            // Return 201 Created and the new category
            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, new CategoryResponse(category));
        }

        // PUT: api/categories/{categoryId}
        [HttpPut("{categoryId}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory([FromRoute] string categoryId,
            [FromBody] CategoryCreateRequest categoryRequest)
        {
            var category = await Context.Categories.FindAsync(categoryId);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            // Update the category details
            category.UpdateCategory(categoryRequest.Name, categoryRequest.Description);
            await Context.SaveChangesAsync();

            return Ok(new CategoryResponse(category));
        }

        // DELETE: api/categories/{categoryId}
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] string categoryId)
        {
            var category = await Context.Categories.FindAsync(categoryId);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            Context.Categories.Remove(category);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/categories/assignsubcategory/{categoryId}
        [HttpPut("assignsubcategory/{categoryId}")]
        public async Task<ActionResult<CategoryResponse>> AssignSubCategory([FromRoute] string categoryId,
            [FromBody] AssignSubCategoryRequest request)
        {
            var category = await Context.Categories.FindAsync(categoryId);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            var subCategory = await Context.SubCategories.FindAsync(request.SubCategoryId);
            if (subCategory == null)
                return BadRequest(new { message = "SubCategory not found" });

            // Assign the subcategory to the category
            category.AssignSubCategory(subCategory);

            await Context.SaveChangesAsync();

            return new CategoryResponse(category);
        }
    }
}
