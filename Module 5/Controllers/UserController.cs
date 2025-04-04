using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Services;
using Module_5.Utilities;

namespace Module_5.Controllers
{

    [Route("api/post")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;

        public UserController(IUserService userService)
        {
            _userservice = userService;
        }
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> Liked(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            var isliked= await _userservice.LikePost(postId,userId);
            if(isliked=="Post liked successfully")
            {
                return Ok(new ApiResponse(true, 200, isliked, null));
            }
            return BadRequest(new ApiResponse(false,400,isliked, null)); 
        }

        [HttpDelete("unlike/{postId}")]

        public async Task<IActionResult> UnLiked(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            var isUnliked = await _userservice.UnLikePost(postId, userId);

            if (isUnliked == "Post UnLiked successfully")
            {
                return Ok(new ApiResponse(true, 200, isUnliked, null));
            }
            return BadRequest(new ApiResponse(false, 400, isUnliked, null));
        }

        [HttpPost("{postId}/comment")]
        public async Task<IActionResult> Create(int postId, [FromBody] CommentDto commentDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            var response= await _userservice.CreateAsync(commentDto,postId, userId);
            if(response == "Comment Added successfully")
            {
                return Ok(new ApiResponse(true, 200, response, null));
            }
            return  BadRequest(new ApiResponse(false, 400, response, null));

        }

        [HttpGet("{postId}/comment")]
        public async Task<IActionResult> Get(int postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            var response = await _userservice.GetAsync(postId);

            if (response == null)
            {
                return NotFound(new ApiResponse(false, 400, "No comments", response));
            }
            return Ok(new ApiResponse(true, 200, "Comments retrieved successfully", response));

        }

        [HttpDelete("{postId}/comment/{commentId}")]
        public async Task<IActionResult> Delete(int postId, int commentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            string result = await _userservice.DeleteAsync(postId, userId, commentId);

            if (result == "Comment deleted successfully.")
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));
        }
        
    }
}

