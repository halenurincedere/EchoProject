namespace Echo.Business.Operations.Settings
{
    public interface ISettingService
    {
        // Turns maintenance mode on or off
        Task ToggleMaintenanceAsync();

        // Returns true if the app is currently in maintenance mode
        Task<bool> IsMaintenanceModeAsync();
    }
}