using System.ComponentModel.DataAnnotations;

namespace Echo.Data.Entities;


// Represents a system-wide setting, such as maintenance mode.
// This entity is designed to have a fixed GUID and usually a single row.

public class SettingEntity : BaseEntity // Inherits common fields like Id, CreatedAt, etc.
{
    // We override the base Id to use a fixed GUID manually (e.g., for seeding)
    [Key] 
    public new Guid Id { get; set; }

    // Indicates whether the system is currently under maintenance.
    // When true, public endpoints may return 503 or a custom maintenance message.
    public bool MaintenanceMode { get; set; }
}