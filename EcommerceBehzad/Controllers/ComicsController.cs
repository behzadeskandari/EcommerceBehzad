using System.Security.Claims;
using EcommerceBehzad.Application.Comics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBehzad.Controllers
{
    [ApiController]
    [Route("api/comics")]
    [Authorize]
    public class ComicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComicsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadComic(Guid id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("User context validation token invalid.");

            try
            {
                var query = new GetComicStreamQuery(id, userEmail);
                var result = await _mediator.Send(query);

                // This streams files with low memory usage by writing direct chunk responses to the client
                return new FileStreamResult(result.Stream, "application/pdf")
                {
                    FileDownloadName = result.FileName,
                    EnableRangeProcessing = true // Supports media scrubbing / resume features
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
