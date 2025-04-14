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
        //Api to create a category 
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

        //Api to delete a category by categoryId

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
        //Api to get all category
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

        //Api to update a category

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

