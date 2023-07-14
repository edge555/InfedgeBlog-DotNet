using AutoMapper;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IDateTimeHandler _dateTimeHandler;
        private readonly DtoValidatorBase<StoryPostDto> _storyPostDtoValidator;
        private readonly DtoValidatorBase<StoryUpdateDto> _storyUpdateDtoValidator;
        public StoryService(IStoryRepository storyRepository, IUserService userService, IMapper mapper, IAuthService authService, IJwtTokenHandler jwtTokenHandler, IDateTimeHandler dateTimeHandler, DtoValidatorBase<StoryPostDto> storyPostDtoValidator, DtoValidatorBase<StoryUpdateDto> storyUpdateDtoValidator)
        {
            _storyRepository = storyRepository;
            _userService = userService;
            _mapper = mapper;
            _jwtTokenHandler = jwtTokenHandler;
            _dateTimeHandler = dateTimeHandler;
            _storyPostDtoValidator = storyPostDtoValidator;
            _storyUpdateDtoValidator = storyUpdateDtoValidator;
        }
        public async Task<IEnumerable<StoryDto>> GetStoriesAsync(int pageNumber, int pageSize)
        {
            List<Story> stories = await _storyRepository.GetStoriesAsync(pageNumber, pageSize);
            IEnumerable<StoryDto> storyDtos = _mapper.Map<IEnumerable<StoryDto>>(stories);
            return storyDtos;
        }
        public async Task<StoryDto> GetStoryByIdAsync(int Id)
        {
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if(story == null)
            {
                throw new NotFoundException("No story found with this id.");
            }
            var storyDto = _mapper.Map<StoryDto>(story);
            storyDto.AuthorName = _userService.GetUserByIdAsync(storyDto.AuthorId).Result.Name;
            return storyDto;
        }
        public async Task<StoryDto> PostStoryAsync(StoryPostDto storyPostDto)
        {
            _storyPostDtoValidator.ValidateDto(storyPostDto);
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            Story storyData = _mapper.Map<Story>(storyPostDto);
            storyData.AuthorId = _jwtTokenHandler.GetLoggedInUserId();
            storyData.CreatedAt = _dateTimeHandler.GetCurrentUtcTime();
            storyData.UpdatedAt = _dateTimeHandler.GetCurrentUtcTime();
            var newStory = await _storyRepository.PostStoryAsync(storyData);
            var storyDto = _mapper.Map<StoryDto>(newStory);
            return storyDto;
        }
        public async Task<StoryDto> UpdateStoryByIdAsync(int Id, StoryUpdateDto storyUpdateDto)
        {
            _storyUpdateDtoValidator.ValidateDto(storyUpdateDto);
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null)
            {
                throw new NotFoundException("No story found with this id.");
            }
            var loggedInUserId = _jwtTokenHandler.GetLoggedInUserId();
            if (story.AuthorId != loggedInUserId)
            {
                throw new ForbiddenException("You do not have permission to perform this action.");
            }
            Story storyData = _mapper.Map<Story>(storyUpdateDto);
            storyData.UpdatedAt = _dateTimeHandler.GetCurrentUtcTime();
            var updatedStory = await _storyRepository.UpdateStoryByIdAsync(Id, storyData);
            var storyDto = _mapper.Map<StoryDto>(updatedStory);
            return storyDto;
        }
        public async Task<Boolean> DeleteStoryByIdAsync(int Id)
        {
            if (_jwtTokenHandler.IsTokenExpired())
            {
                throw new UnauthorizedException("Token expired, Please log in again.");
            }
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null)
            {
                throw new NotFoundException("No story found with this id.");
            }
            var loggedInUserId = _jwtTokenHandler.GetLoggedInUserId();
            if(story.AuthorId != loggedInUserId)
            {
                throw new ForbiddenException("You do not have permission to perform this action.");
            }
            return await _storyRepository.DeleteStoryByIdAsync(Id);
        }
        public async Task<int> CountStoriesAsync()
        {
            return await _storyRepository.CountStoriesAsync();
        }
    }
}
