using AutoMapper;
using Broker.Events;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDTO>> GetAllGamesAsync(CancellationToken cancellationToken)
        {
            var games = await _unitOfWork.Games.GetAllGamesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO> GetGameByIdAsync(string id, CancellationToken cancellationToken)
        {
            return _mapper.Map<GameDTO>(await _unitOfWork.Games.GetGameByIdAsync(id, cancellationToken));
        }

        public async Task DeleteGameAsync(string id, CancellationToken cancellationToken)
        {
            await _unitOfWork.Games.DeleteGameAsync(id, cancellationToken);
        }
        public async Task AddGameAsync(GameResultDto gameResultDto, CancellationToken cancellationToken)
        {
            var game = _mapper.Map<Game>(gameResultDto);
            await _unitOfWork.Games.AddGameAsync(game, cancellationToken);
        }
    }
}