using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Collections;
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

        /// <summary>
        /// Creates a new category. Only accessible to users with the Author role.
        /// </summary>
        /// <param name="categoryDto">The details of the category to be created (name and description).</param>
        /// <returns>
        /// 201 Created with category info if successful,  
        /// 400 Conflict if the category already exists,  
        /// 400 Bad Request if user ID is missing.
        /// </returns>
        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            bool isCreated = await _categoryService.CreateAsync(categoryDto,userId);
            if (!isCreated)
            {
                return Conflict(new ApiResponse(false, 400, JsonHelper.GetMessage(106), null));
            }
            return Ok(new ApiResponse(true, 201, JsonHelper.GetMessage(108), new
            {
                categoryDto.CategoryName,
                categoryDto.Description
            }));
        }


        /// <summary>
        /// Deletes a category by its ID. Only accessible to users with the Author role.
        /// </summary>
        /// <param name="categoryId">The ID of the category to be deleted.</param>
        /// <returns>
        /// 201 OK if successfully deleted,  
        /// 400 Conflict if deletion fails,  
        /// 400 Bad Request if user ID is missing.
        /// </returns>
        [Authorize(Roles = "Author")]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(string categoryId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var isDeleted = await _categoryService.DeleteAsync(categoryId, userId);
            if (isDeleted == JsonHelper.GetMessage(109))
            {
                return Ok(new ApiResponse(true, 201, JsonHelper.GetMessage(109), null));
               
            }
            return Conflict(new ApiResponse(false, 400, JsonHelper.GetMessage(111), null));



        }


        /// <summary>
        /// Retrieves all categories in the system.
        /// </summary>
        /// <returns>
        /// 200 OK with the list of categories,  
        /// 404 Not Found if no categories are available.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                return NotFound(new ApiResponse(false, 404, JsonHelper.GetMessage(107), null));
            }

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(124), categories));


        }

        /// <summary>
        /// Updates a category by ID. Only accessible to users with the Author role.
        /// </summary>
        /// <param name="categoryDto">The updated details of the category.</param>
        /// <param name="categoryId">The ID of the category to update.</param>
        /// <returns>
        /// 201 OK if the update is successful,  
        /// 403 Not Found if the category doesn't exist or the user is unauthorized,  
        /// 400 Bad Request if user ID is missing.
        /// </returns>
        [Authorize(Roles = "Author")]
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromBody] CategoryDto categoryDto, string categoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }
            var isUpdated = await _categoryService.UpdateAsync(categoryDto, userId, categoryId);

            if (isUpdated == null)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(110), null));

            return Ok(new ApiResponse(true, 201, JsonHelper.GetMessage(125), isUpdated));


        }
    }
}

