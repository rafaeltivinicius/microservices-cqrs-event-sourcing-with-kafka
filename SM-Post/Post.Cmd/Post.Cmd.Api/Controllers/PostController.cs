using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.Requests;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public PostController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Author) || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Invalid post data.");
            }

            var command = new NewPostCommand
            {
                Id = Guid.NewGuid(),
                Author = request.Author,
                Message = request.Message
            };

            await _commandDispatcher.SendAsync(command);
            return Ok("Post created successfully.");
        }
    }
}
