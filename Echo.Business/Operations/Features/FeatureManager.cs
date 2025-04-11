using Echo.Business.Shared;
using Echo.Data.Repositories;
using Echo.Data.UnitOfWork;
using Echo.Data.Entities;
using Echo.Business.Operations.Feature.Dtos;

namespace Echo.Business.Operations.Feature
{
    public class FeatureManager : IFeatureService
    {
        private readonly IRepository<FeatureEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public FeatureManager(IRepository<FeatureEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        // Adds a new feature if it doesn't already exist
        public async Task<ServiceMessage<string>> AddFeatureAsync(AddFeatureDto dto)
        {
            var exists = (await _repository.GetAll())
                .Any(f => f.Title.ToLower() == dto.Title.ToLower());

            if (exists)
            {
                return new ServiceMessage<string>
                {
                    IsSucceed = false,
                    Message = "This feature already exists.",
                    Data = dto.Title
                };
            }

            var entity = new FeatureEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                Note = dto.Note,
                Source = dto.Source,
                Tag = dto.Tag
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage<string>
            {
                IsSucceed = true,
                Message = "Feature successfully added.",
                Data = entity.Title
            };
        }

        // Retrieves all features that are not deleted (soft delete logic)
        public async Task<List<FeatureDto>> GetAllAsync()
        {
            var features = await _repository.GetAll();
            return features
                .Where(f => !f.IsDeleted)
                .Select(f => new FeatureDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Note = f.Note,
                    Source = f.Source,
                    Tag = f.Tag
                }).ToList();
        }

        // Retrieves a feature by ID if it exists and is not deleted
        public async Task<FeatureDto?> GetByIdAsync(Guid id)
        {
            var f = await _repository.GetByIdAsync(id);
            if (f == null || f.IsDeleted) return null;

            return new FeatureDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                Note = f.Note,
                Source = f.Source,
                Tag = f.Tag
            };
        }

        // Randomly selects one of the existing (non-deleted) features
        public async Task<FeatureDto?> GetRandomFeatureAsync()
        {
            var features = (await _repository.GetAll())
                .Where(f => !f.IsDeleted)
                .ToList();

            if (!features.Any()) return null;

            var random = new Random();
            var selected = features[random.Next(features.Count)];

            return new FeatureDto
            {
                Id = selected.Id,
                Title = selected.Title,
                Description = selected.Description,
                Note = selected.Note,
                Source = selected.Source,
                Tag = selected.Tag
            };
        }

        // Updates an existing feature's details
        public async Task<ServiceMessage<string>> UpdateFeatureAsync(UpdateFeatureDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null || entity.IsDeleted)
            {
                return new ServiceMessage<string>
                {
                    IsSucceed = false,
                    Message = "Feature not found.",
                    Data = dto.Title
                };
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Note = dto.Note;
            entity.Source = dto.Source;
            entity.Tag = dto.Tag;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage<string>
            {
                IsSucceed = true,
                Message = "Feature updated successfully.",
                Data = dto.Title
            };
        }

        // Performs a soft delete on the feature
        public async Task<ServiceMessage<string>> DeleteFeatureAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return new ServiceMessage<string>
                {
                    IsSucceed = false,
                    Message = "Feature not found to delete.",
                    Data = id.ToString()
                };
            }

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage<string>
            {
                IsSucceed = true,
                Message = "Feature has been soft-deleted.",
                Data = id.ToString()
            };
        }
    }
}