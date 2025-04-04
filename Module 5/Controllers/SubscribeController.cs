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

        [HttpPost("subscribe/{authorId}")]
        public async Task<IActionResult> Subscribe(int authorId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));
            int userId = int.Parse(userIdClaim.Value);

            if (userId == authorId)
            {
                return BadRequest(new ApiResponse(false, 400, "You cannot subscribe to yourself.", null));
            }
            var result = await _subscribeservice.SubscribeAsync(userId, authorId);
            if (result == "Subscribed successfully.")
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }
        [HttpDelete("unsubsribe/{authorId}")]
        public async Task<IActionResult> UnSubscribe(int authorId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            int userId = int.Parse(userIdClaim.Value);
            if (userId == authorId)
            {
                return BadRequest(new ApiResponse(false, 400, "You cannot unsubscribe to yourself.", null));
            }
            var result = await _subscribeservice.SubscribeAsync(userId, authorId);
            if (result == "UnSubscribed successfully.")
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }

    }
}
