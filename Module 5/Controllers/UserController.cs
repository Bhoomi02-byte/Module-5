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


        /// <summary>
        /// Likes a post on behalf of the logged-in user.
        /// </summary>
        /// <param name="postId">The ID of the post to like.</param>
        /// <returns>
        /// Returns a success response if the post is liked,
        /// or a bad request if the action fails.
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> Liked(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var isliked = await _userservice.LikePost(postId, userId);
            if (isliked == JsonHelper.GetMessage(142))
                return Ok(new ApiResponse(true, 200, isliked, null));

            return BadRequest(new ApiResponse(false, 400, isliked, null));
        }

        /// <summary>
        /// Unlikes a post on behalf of the logged-in user.
        /// </summary>
        /// <param name="postId">The ID of the post to unlike.</param>
        /// <returns>
        /// Returns a success response if the post is unliked,
        /// or a bad request if the action fails.
        /// </returns>
        [HttpDelete("unlike/{postId}")]
        public async Task<IActionResult> UnLiked(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var isUnliked = await _userservice.UnLikePost(postId, userId);

            if (isUnliked == JsonHelper.GetMessage(140))
                return Ok(new ApiResponse(true, 200, isUnliked, null));

            return BadRequest(new ApiResponse(false, 400, isUnliked, null));
        }

        /// <summary>
        /// Adds a comment to a post by the logged-in user.
        /// </summary>
        /// <param name="postId">The ID of the post to comment on.</param>
        /// <param name="commentDto">The comment data.</param>
        /// <returns>
        /// Returns a success response if the comment is added,
        /// or a bad request if the action fails.
        [HttpPost("comment/{postId}")]
        public async Task<IActionResult> Create(string postId, [FromBody] CommentDto commentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var response = await _userservice.CreateAsync(commentDto, postId, userId);
            if (response == JsonHelper.GetMessage(131))
            {
                return Ok(new ApiResponse(true, 200, response, null));
            }
            return BadRequest(new ApiResponse(false, 400, response, null));

        }


        /// <summary>
        /// Retrieves all comments on a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>
        /// Returns a list of comments on success,
        /// or a not found response if no comments exist.
        /// </returns>
        [HttpGet("comment/{postId}")]
        public async Task<IActionResult> Get(string postId)
        {

            var response = await _userservice.GetAsync(postId);

            if (response == null)
                return NotFound(new ApiResponse(false, 400, JsonHelper.GetMessage(132), response));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(133), response));

        }


        /// <summary>
        /// Deletes a specific comment from a post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>
        /// Returns a success response if the comment is deleted,
        /// or a bad request if deletion fails.
        /// </returns>
        [HttpDelete("{postId}/comment/{commentId}")]
        public async Task<IActionResult> Delete(string postId, string commentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            string result = await _userservice.DeleteAsync(postId, userId, commentId);

            if (result == JsonHelper.GetMessage(134))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));
        }

    }
}

