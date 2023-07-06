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
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly DtoValidatorBase<StoryPostDto> _storyPostDtoValidator;
        private readonly DtoValidatorBase<StoryUpdateDto> _storyUpdateDtoValidator;
        public StoryService(IStoryRepository storyRepository, IMapper mapper, IAuthService authService, DtoValidatorBase<StoryPostDto> storyPostDtoValidator, DtoValidatorBase<StoryUpdateDto> storyUpdateDtoValidator)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
            _authService = authService;
            _storyPostDtoValidator = storyPostDtoValidator;
            _storyUpdateDtoValidator = storyUpdateDtoValidator;
        }
        public async Task<IEnumerable<StoryDto>> GetStoriesAsync()
        {
            List<Story> stories = await _storyRepository.GetStoriesAsync();
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
            return storyDto;
        }
        public async Task<StoryDto> PostStoryAsync(StoryPostDto storyPostDto)
        {
            _storyPostDtoValidator.ValidateDto(storyPostDto);
            Story storyData = _mapper.Map<Story>(storyPostDto);
            storyData.AuthorId = _authService.GetLoggedInUserId();
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
    }
}
