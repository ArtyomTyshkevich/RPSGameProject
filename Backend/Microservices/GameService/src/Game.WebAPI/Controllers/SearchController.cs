using AutoMapper;
using Game.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController( IMapper mapper, ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("{id:guid}/start")]
        public async Task<IActionResult> StartSearchGame(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartSearchGame initiated. Game ID: {GameId}", id);

            var roomId = await _searchService.StartSearchGame(id, cancellationToken);

            _logger.LogInformation("StartSearchGame succeeded. Game ID: {GameId}", id);

            return Ok(new { roomId });
        }

        [HttpPost("{id:guid}/stop")]
        public async Task<IActionResult> StopSearchGame( Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopSearchGame initiated. Game ID: {GameId}", id);

            await _searchService.StopSearchGame(id, cancellationToken);

            _logger.LogInformation("StopSearchGame succeeded. Game ID: {GameId}", id);

            return Ok();
        }
    }
}
