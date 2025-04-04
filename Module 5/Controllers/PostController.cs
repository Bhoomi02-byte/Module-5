using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Models.Entities;
using Module_5.Services;
using Module_5.Utilities;

namespace Module_5.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController:ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(Roles = "Author")] 
        [HttpPost("{categoryId}")]
        public async Task<IActionResult> CreatePost(int categoryId, [FromBody] PostDto postDto)
        {
          
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return BadRequest(new ApiResponse(false, 401, JsonHelper.GetMessage(105), null));

            int userId = int.Parse(userIdClaim);

            var response = await _postService.CreateAsync(userId, categoryId, postDto);

            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return CreatedAtAction(nameof(CreatePost), response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ; 
            
            var posts = await _postService.GetAllAsync(userId, userRole);

            if (posts == null || !posts.Any())
                return NotFound(new ApiResponse(false, 404, JsonHelper.GetMessage(118), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(126), posts));
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> Get(int postId)

        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var post = await _postService.GetAsync(postId, userId, userRole);

            if (post == null || !post.Any())
                return NotFound(new ApiResponse(false, 404, "No posts found.", null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(126), post));

        }

        [Authorize(Roles = "Author")]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
             if (userIdClaim == null )
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            int userId = int.Parse(userIdClaim.Value);
           

            bool isDeleted = await _postService.DeleteAsync(postId, userId);

            if (!isDeleted)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(120), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(114), null));
        }

        [Authorize(Roles = "Author")]
        [HttpPut("{postId}")]
        public async Task<IActionResult> Update(int postId, [FromBody] PostDto postDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
           
                if (userIdClaim == null )
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            int userId = int.Parse(userIdClaim.Value);
            

            var isUpdated = await _postService.UpdateAsync(postId, userId, postDto);

            if (isUpdated==null)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(119), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(115), isUpdated));
        }

        [Authorize(Roles = "Author")]
        [HttpPatch("{postId}/publish")]
        public async Task<IActionResult>Publish(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null )
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            int userId = int.Parse(userIdClaim.Value);
            
            var isPublished = await _postService.PublishAsync(postId, userId);

            if (isPublished == null)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(121), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(116), null ));
        }

        [Authorize(Roles = "Author")]
        [HttpPatch("{postId}/unpublish")]
        public async Task<IActionResult> UnPublish(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            int userId = int.Parse(userIdClaim.Value);

            var isPublished = await _postService.UnPublishAsync(postId, userId);

            if (isPublished == null)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(121), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(117), null));
        }






    }
}
