using Echo.Data.Entities;
using Echo.Data.Repositories;
using Echo.Data.UnitOfWork;

namespace Echo.Business.Operations.Settings
{
    public class SettingManager : ISettingService
    {
        private readonly IRepository<SettingEntity> _repo;
        private readonly IUnitOfWork _uow;

        // This fixed ID represents the single settings row used in the system
        private static readonly Guid SettingId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        public SettingManager(IRepository<SettingEntity> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow  = uow;
        }

        // Switches maintenance mode ON or OFF
        public async Task ToggleMaintenanceAsync()
        {
            var s = await _repo.GetByConditionAsync(x => x.Id == SettingId)
                     ?? throw new InvalidOperationException("Setting row not found");

            s.MaintenanceMode = !s.MaintenanceMode;
            await _uow.SaveChangesAsync();
        }

        // Checks if maintenance mode is currently enabled
        public async Task<bool> IsMaintenanceModeAsync()
        {
            var s = await _repo.GetByConditionAsync(x => x.Id == SettingId)
                     ?? throw new InvalidOperationException("Setting row not found");

            return s.MaintenanceMode;
        }
    }
}