using AutoMapper;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Database.Models;
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
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly DtoValidatorBase<StoryPostDto> _storyPostDtoValidator;
        private readonly DtoValidatorBase<StoryUpdateDto> _storyUpdateDtoValidator;
        public StoryService(IStoryRepository storyRepository, IMapper mapper, IAuthService authService,IUserService userService, DtoValidatorBase<StoryPostDto> storyPostDtoValidator, DtoValidatorBase<StoryUpdateDto> storyUpdateDtoValidator)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _authService = authService;
            _userService = userService;
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
                throw new NotFoundException("No story found with this id");
            }
            var storyDto = _mapper.Map<StoryDto>(story);
            storyDto.AuthorName = _userService.GetUserByIdAsync(storyDto.AuthorId).Result.Name;
            return storyDto;
        }
        public async Task<StoryDto> PostStoryAsync(StoryPostDto storyPostDto)
        {
            _storyPostDtoValidator.ValidateDto(storyPostDto);
            Story storyData = _mapper.Map<Story>(storyPostDto);
            storyData.AuthorId = _authService.GetLoggedInUserId();
            storyData.CreatedAt = DateTime.UtcNow;
            storyData.UpdatedAt = DateTime.UtcNow;
            var newStory = await _storyRepository.PostStoryAsync(storyData);
            var storyDto = _mapper.Map<StoryDto>(newStory);
            return storyDto;
        }
        public async Task<StoryDto> UpdateStoryAsync(int Id, StoryUpdateDto storyUpdateDto)
        {
            _storyUpdateDtoValidator.ValidateDto(storyUpdateDto);
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null)
            {
                throw new NotFoundException("No story found with this id");
            }
            var loggedInUserId = _authService.GetLoggedInUserId();
            if (story.AuthorId != loggedInUserId)
            {
                throw new UnauthorizedException("You are not authorized to perform this action.");
            }
            Story storyData = _mapper.Map<Story>(storyUpdateDto);
            storyData.UpdatedAt = DateTime.UtcNow;
            var updatedStory = await _storyRepository.UpdateStoryAsync(Id, storyData);
            var storyDto = _mapper.Map<StoryDto>(updatedStory);
            return storyDto;
        }
        public async Task<Boolean> DeleteStoryByIdAsync(int Id)
        {
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null)
            {
                throw new NotFoundException("No story found with this id");
            }
            var loggedInUserId = _authService.GetLoggedInUserId();
            if(story.AuthorId != loggedInUserId)
            {
                throw new UnauthorizedException("You are not authorized to perform this action.");
            }
            return await _storyRepository.DeleteStoryByIdAsync(Id);
        }
        public async Task<int> CountStoriesAsync()
        {
            return await _storyRepository.CountStoriesAsync();
        }
    }
}
