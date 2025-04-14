using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Module_5.Services;
using Module_5.Utilities;

namespace Module_5.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly ISubscribeService _subscribeservice;

        public SubscribeController(ISubscribeService subscribeservice)
        {
            _subscribeservice = subscribeservice;
        }

        /// <summary>
        /// Subscribes the logged-in user to a specified author.
        /// </summary>
        /// <param name="authorId">The ID of the author to subscribe to.</param>
        /// <returns>
        /// Returns a success response if the subscription is successful,
        /// or a bad request if the user is already subscribed or tries to subscribe to themselves.
        /// </returns>
        [HttpPost("subscribe/{authorId}")]
        public async Task<IActionResult> Subscribe(string authorId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            if (userId == authorId)
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(144), null));

            var result = await _subscribeservice.SubscribeAsync(userId, authorId);
            if (result == JsonHelper.GetMessage(145))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }


        /// <summary>
        /// Unsubscribes the subscribed author.
        /// </summary>
        /// <param name="authorId">The ID of the author to unsubscribe.</param>
        /// <returns>
        /// Returns a success response if the unsubscription is successful,
        /// or a bad request if the user is not subscribed or tries to unsubscribe from themselves.
        /// </returns>
        [HttpDelete("unsubscribe/{authorId}")]
        public async Task<IActionResult> UnSubscribe(string authorId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(103), null));
            }

            if (userId == authorId)
                return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(148), null));

            var result = await _subscribeservice.UnsubscribeAsync(userId, authorId);
            if (result == JsonHelper.GetMessage(147))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }

    }
}
