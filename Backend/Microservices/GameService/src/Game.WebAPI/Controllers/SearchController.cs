using AutoMapper;
using Game.Application.Interfaces.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IPublishEndpoint publishEndpoint, IMapper mapper, ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartSearchGame([FromBody] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartSearchGame initiated. Game ID: {GameId}", id);

            try
            {
                await _searchService.StartSearchGame(id, cancellationToken);
                _logger.LogInformation("StartSearchGame succeeded. Game ID: {GameId}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartSearchGame failed. Game ID: {GameId}", id);
                throw;
            }
        }

        [HttpPost("{id}/stop")]
        public async Task<IActionResult> StopSearchGame([FromBody] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopSearchGame initiated. Game ID: {GameId}", id);

            try
            {
                await _searchService.StopSearchGame(id, cancellationToken);
                _logger.LogInformation("StopSearchGame succeeded. Game ID: {GameId}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StopSearchGame failed. Game ID: {GameId}", id);
                throw;
            }
        }
    }
}
