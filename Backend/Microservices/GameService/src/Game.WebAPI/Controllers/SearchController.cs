using AutoMapper;
using Game.Application.Interfaces.Repositories.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISearchService _searchService;

        public SearchController(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IMapper mapper, ISearchService searchService)
        {
            _searchService = searchService;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        [HttpPost("StartSearchGame")]
        public async Task<IActionResult> StartSearchGame(Guid id, CancellationToken cancellationToken)
        {
            await _searchService.StartSearchGame(id, cancellationToken);
            return Ok();
        }
        [HttpPost("StopSearchGame")]
        public async Task<IActionResult> StopSearchGame(Guid Id, CancellationToken cancellationToken)
        {
            await _searchService.StopSearchGame(Id, cancellationToken);
            return Ok();
        }
    }
}
