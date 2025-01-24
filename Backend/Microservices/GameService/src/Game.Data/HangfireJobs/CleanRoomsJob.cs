using Game.Application.Interfaces.Repositories.UnitOfWork;

namespace Game.Data.HangfireJobs
{
    public class CleanRoomsJob
    {
        private readonly IUnitOfWork _unitOfWork;

        public CleanRoomsJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.CleanRoomsInPreparationAsync();
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
