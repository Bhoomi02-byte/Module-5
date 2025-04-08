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
        //Api to subscribe the author
        [HttpPost("subscribe/{authorId}")]
        public async Task<IActionResult> Subscribe(int authorId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            if (userId == authorId)
             return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(144), null));
            
            var result = await _subscribeservice.SubscribeAsync(userId, authorId);
            if (result == JsonHelper.GetMessage(145))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }
        //Api to unsubscribe the author
        [HttpDelete("unsubscribe/{authorId}")]
        public async Task<IActionResult> UnSubscribe(int authorId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
                return Unauthorized(new ApiResponse(false, 401, JsonHelper.GetMessage(104), null));

            if (userId == authorId)
            return BadRequest(new ApiResponse(false, 400, JsonHelper.GetMessage(148), null));
            
            var result = await _subscribeservice.UnsubscribeAsync(userId, authorId);
            if (result == JsonHelper.GetMessage(147))
                return Ok(new ApiResponse(true, 200, result, null));

            return BadRequest(new ApiResponse(false, 400, result, null));


        }

    }
}
