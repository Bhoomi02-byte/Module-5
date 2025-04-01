using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Models.Entities;
using Module_5.Services;
using Module_5.Utilities;




namespace Module_5.Controllers
{

    [Route("api/category")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new ApiResponse(false, 400, $"Invalid User ID in token. Extracted Value: {userIdClaim}",null));
            }

            bool isCreated = await _categoryService.CreateAsync(categoryDto, userId);
            if (!isCreated)
            {
                return Conflict(new ApiResponse(false, 400, "A Category with this name exists", null));
            }
            return Ok(new ApiResponse(true, 201, "Category created successfully", new
            {
                categoryDto.CategoryName,
                categoryDto.Description
            }));
        }

      [Authorize(Roles = "Author")]
      [HttpDelete("{categoryId}")]
      public async Task<IActionResult> Delete(int categoryId)
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new ApiResponse(false, 400, $"Invalid User ID in token. Extracted Value: {userIdClaim}",null));
            }

            bool isDeleted = await _categoryService.DeleteAsync(categoryId, userId);
            if (!isDeleted)
            {
                return Conflict(new ApiResponse(false, 400, "Failed to delete category.",null));
            }
            return Ok(new ApiResponse(true, 201, "Category deleted successfully",null));


        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                return NotFound(new ApiResponse(false, 404, "No categories found.", null));
            }

       return Ok(new ApiResponse(true, 200, "Categories retrieved successfully.", categories));


        }

        [Authorize(Roles = "Author")]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromBody] CategoryDto categoryDto, int categoryId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new ApiResponse(false, 400, $"Invalid User ID in token. Extracted Value: {userIdClaim}",null));
            }
            var isUpdated= await _categoryService.UpdateAsync(categoryDto,userId,categoryId);

            if (isUpdated == null)
                return NotFound(new ApiResponse(false, 403, "Only the author with 'Author' role can update this category.", null));

            return Ok(new ApiResponse(true, 201, "Category updated successfully.", isUpdated));



        }
      


        

    }

 }

