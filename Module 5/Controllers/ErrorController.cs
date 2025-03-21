using System;
using Microsoft.AspNetCore.Mvc;

namespace Module_5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class errorController : ControllerBase
    {
        [HttpGet("success")]
        public IActionResult Success()
        {
            var response = new
            {
                success = true,
                statusCode = 200,
                message = "Request processed successfully!"
            };
            return Ok(response);
        }

        [HttpGet("bad-request")]
        public IActionResult BadRequestError()
        {
            throw new GlobalException("Invalid input format.");
        }

        [HttpGet("internal-error")]
        public IActionResult InternalServerError()
        {
           throw new Exception("This is an unexpected error.");
        }
    }
}
