using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Module_5.DTO;
using Module_5.Collections;
using Module_5.Services;
using Module_5.Utilities;

namespace Module_5.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Creates a new post under a specified category for the logged-in author.
        /// </summary>
        /// <param name="categoryId">The ID of the category to associate the post with.</param>
        /// <param name="postDto">Post data to be created.</param>
        /// <returns>Returns a response with the created post or error message.</returns>
        [Authorize(Roles = "Author")]
        [HttpPost("{categoryId}")]
        public async Task<IActionResult> CreatePost(string categoryId, [FromBody] PostDto postDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var response = await _postService.CreateAsync(userId, categoryId, postDto);

            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            return CreatedAtAction(nameof(CreatePost), response);
        }

        /// <summary>
        /// Retrieves all published posts.
        /// </summary>
        /// <returns>Returns a list of all posts or an error if it is unpublished.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetAllAsync();

            if (posts == null || !posts.Any())
                return NotFound(new ApiResponse(false, 404, JsonHelper.GetMessage(118), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(126), posts));
        }

        /// <summary>
        /// Retrieves a specific post by its ID for the current user.
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve.</param>
        /// <returns>Returns the post if found, otherwise an error message.</returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> Get(string postId)

        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var post = await _postService.GetAsync(postId, userId, userRole);

            if (post == null || !post.Any())
                return NotFound(new ApiResponse(false, 404, "No posts found.", null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(126), post));

        }

        /// <summary>
        /// Deletes a post by ID if the user is its author.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <returns>Returns success or error based on deletion result.</returns>
        [Authorize(Roles = "Author")]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> Delete(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }


            bool isDeleted = await _postService.DeleteAsync(postId, userId);

            if (!isDeleted)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(120), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(114), null));
        }

        /// <summary>
        /// Updates an existing post for the current user.
        /// </summary>
        /// <param name="postId">find a post by postId to update.</param>
        /// <param name="postDto">Updated post data.</param>
        /// <returns>Returns updated post data or error.</returns>
        [Authorize(Roles = "Author")]
        [HttpPut("{postId}")]
        public async Task<IActionResult> Update(string postId, [FromBody] PostDto postDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var isUpdated = await _postService.UpdateAsync(postId, userId, postDto);

            if (isUpdated == null)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(119), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(115), isUpdated));
        }

        /// <summary>
        /// Publishes a post by ID.
        /// </summary>
        /// <param name="postId">ID of the post to publish.</param>
        /// <returns>Returns success or error based on the result.</returns>
        [Authorize(Roles = "Author")]
        [HttpPatch("{postId}/publish")]
        public async Task<IActionResult> Publish(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }
            var isPublished = await _postService.PublishAsync(postId, userId);

            if (!isPublished)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(121), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(116), null));
        }

        /// <summary>
        /// Unpublishes a post by ID.
        /// </summary>
        /// <param name="postId">ID of the post to unpublish.</param>
        /// <returns>Returns success or error based on the result.</returns>
        [Authorize(Roles = "Author")]
        [HttpPatch("{postId}/unpublish")]
        public async Task<IActionResult> UnPublish(string postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            var isPublished = await _postService.UnPublishAsync(postId, userId);

            if (!isPublished)
                return NotFound(new ApiResponse(false, 403, JsonHelper.GetMessage(121), null));

            return Ok(new ApiResponse(true, 200, JsonHelper.GetMessage(117), null));
        }


        /// <summary>
        /// Uploads an image for a specific post.
        /// </summary>
        /// <param name="postId">ID of the post for which the image is to be uploaded.</param>
        /// <param name="image">Image file uploaded via form data.</param>
        /// <returns>Returns success or error based on the result of the upload.</returns>
        [HttpPost("{postId}/upload-image")]
        public async Task<IActionResult> UploadImage(string postId, [FromForm] IFormFile image)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            if (image == null || image.Length == 0)
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(153), null));

            var result = await _postService.UploadImageAsync(postId, userId, image, Request);

            if (result == JsonHelper.GetMessage(152))
                return Ok(new ApiResponse(true, 201, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));
        }

    }
}
