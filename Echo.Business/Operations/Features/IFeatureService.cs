using Echo.Business.Operations.Feature.Dtos;
using Echo.Business.Shared;

namespace Echo.Business.Operations.Feature
{
    public interface IFeatureService
    {
        // Adds a new feature to the system
        Task<ServiceMessage<string>> AddFeatureAsync(AddFeatureDto dto);

        // Returns a list of all active (not deleted) features
        Task<List<FeatureDto>> GetAllAsync();

        // Updates the details of an existing feature
        Task<ServiceMessage<string>> UpdateFeatureAsync(UpdateFeatureDto dto);

        // Soft-deletes a feature by its ID
        Task<ServiceMessage<string>> DeleteFeatureAsync(Guid id);

        // Returns a randomly selected feature from the existing active list
        Task<FeatureDto?> GetRandomFeatureAsync();

        // Retrieves a specific feature by its ID
        Task<FeatureDto?> GetByIdAsync(Guid id);
    }
}