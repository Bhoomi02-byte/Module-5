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
        //Api to like a post
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> Liked(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
           
            var isliked= await _userservice.LikePost(postId,userId);
            if(isliked == JsonHelper.GetMessage(142))
             return Ok(new ApiResponse(true, 200, isliked, null));
            
            return BadRequest(new ApiResponse(false,400,isliked, null)); 
        }
        // Api to unlike a post
        [HttpDelete("unlike/{postId}")]

        public async Task<IActionResult> UnLiked(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            var isUnliked = await _userservice.UnLikePost(postId, userId);

            if (isUnliked == JsonHelper.GetMessage(140))
            return Ok(new ApiResponse(true, 200, isUnliked, null));
            
            return BadRequest(new ApiResponse(false, 400, isUnliked, null));
        }

        //Api to comment on a post

        [HttpPost("comment/{postId}")]
        public async Task<IActionResult> Create(int postId, [FromBody] CommentDto commentDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            var response = await _userservice.CreateAsync(commentDto,postId, userId);
            if(response == JsonHelper.GetMessage(131))
            {
                return Ok(new ApiResponse(true, 200, response, null));
            }
            return  BadRequest(new ApiResponse(false, 400, response, null));

        }
        //Api to get all comments by post

        [HttpGet("comment/{postId}")]
        public async Task<IActionResult> Get(int postId)
        {
        
            var response = await _userservice.GetAsync(postId);

            if (response == null)
              return NotFound(new ApiResponse(false, 400, JsonHelper.GetMessage(132), response));
            
            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(133), response));

        }
        //Api to delete a comment

        [HttpDelete("{postId}/comment/{commentId}")]
        public async Task<IActionResult> Delete(int postId, int commentId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            string result = await _userservice.DeleteAsync(postId, userId, commentId);

            if (result == JsonHelper.GetMessage(134))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));
        }
        
    }
}

