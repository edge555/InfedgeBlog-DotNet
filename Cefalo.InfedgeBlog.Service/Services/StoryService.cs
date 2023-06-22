using AutoMapper;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Service.CustomExceptions;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        public StoryService(IStoryRepository storyRepository, IMapper mapper)
        {
            _storyRepository = storyRepository;
            _mapper = mapper;
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
            Story storyData = _mapper.Map<Story>(storyPostDto);
            var newStory = await _storyRepository.PostStoryAsync(storyData);
            var storyDto = _mapper.Map<StoryDto>(newStory);
            return storyDto;
        }
        public async Task<StoryDto> UpdateStoryAsync(int Id, StoryUpdateDto storyUpdateDto)
        {
            var story = await _storyRepository.GetStoryByIdAsync(Id);
            if (story == null)
            {
                throw new NotFoundException("No story found with this id");
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
            return await _storyRepository.DeleteStoryByIdAsync(Id);
        }
    }
}
