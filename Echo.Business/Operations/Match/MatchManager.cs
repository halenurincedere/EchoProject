using Echo.Business.Operations.Match.Dtos;
using Echo.Business.Shared;
using Echo.Data.Entities;
using Echo.Data.Repositories;
using Echo.Data.UnitOfWork;

namespace Echo.Business.Operations.Match
{
    public class MatchManager : IMatchService
    {
        private readonly IRepository<ConversationRoomEntity> _roomRepo;
        private readonly IRepository<UserSilentBadgeEntity> _badgeRepo;
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public MatchManager(
            IRepository<ConversationRoomEntity> roomRepo,
            IRepository<UserSilentBadgeEntity> badgeRepo,
            IRepository<UserEntity> userRepo,
            IUnitOfWork unitOfWork)
        {
            _roomRepo = roomRepo;
            _badgeRepo = badgeRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
        }

        // Creates a new match between two users with the given IDs (speaker & listener)
        // and also assigns badges to both of them.
        public async Task<ServiceMessage> CreateMatchAsync(CreateMatchDto dto)
        {
            try
            {
                var room = new ConversationRoomEntity
                {
                    Id = Guid.NewGuid(),
                    SpeakerId = dto.SpeakerId,
                    ListenerId = dto.ListenerId,
                    SpeakerMode = "I want to talk",
                    ListenerMode = "I want to support",
                    StartedAt = DateTime.UtcNow,
                    EndedAt = DateTime.UtcNow.AddMinutes(15),
                    CreatedAt = DateTime.UtcNow
                };

                var speakerBadge = new UserSilentBadgeEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.SpeakerId,
                    Reason = "Thank you for opening up",
                    CreatedAt = DateTime.UtcNow
                };

                var listenerBadge = new UserSilentBadgeEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.ListenerId,
                    Reason = "Thank you for silently being there",
                    CreatedAt = DateTime.UtcNow
                };

                await _roomRepo.AddAsync(room);
                await _badgeRepo.AddAsync(speakerBadge);
                await _badgeRepo.AddAsync(listenerBadge);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage
                {
                    IsSucceed = true,
                    Message = "Match created successfully. Badges awarded to both users."
                };
            }
            catch (Exception ex)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"An error occurred while creating the match: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
        }

        // Randomly picks two users from the system and matches them.
        public async Task<ServiceMessage> CreateRandomMatchAsync()
        {
            try
            {
                var users = await _userRepo.GetAll();

                var randomUsers = users
                    .Where(u => !u.IsDeleted)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(2)
                    .ToList();

                if (randomUsers.Count < 2)
                {
                    return new ServiceMessage
                    {
                        IsSucceed = false,
                        Message = "Not enough users available for matching."
                    };
                }

                var matchDto = new CreateMatchDto
                {
                    SpeakerId = randomUsers[0].Id,
                    ListenerId = randomUsers[1].Id
                };

                return await CreateMatchAsync(matchDto);
            }
            catch (Exception ex)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"Error during random match: {ex.InnerException?.Message ?? ex.Message}"
                };
            }
        }

        // Lists all existing match records, including speaker & listener names
        public async Task<List<GetMatchDto>> GetAllMatchesAsync()
        {
            var matches = await _roomRepo.GetAll();

            var result = matches.Select(room => new GetMatchDto
            {
                Id = room.Id,
                SpeakerName = $"{room.Speaker?.FirstName} {room.Speaker?.LastName}",
                ListenerName = $"{room.Listener?.FirstName} {room.Listener?.LastName}",
                CreatedAt = room.CreatedAt
            }).ToList();

            return result;
        }
    }
}