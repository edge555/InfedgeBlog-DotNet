using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Dtos;
using FakeItEasy;

namespace Cefalo.InfedgeBlog.Service.UnitTests.FakeData
{
    public class FakeStoryData
    {
        public Story fakeStory;
        public Story fakeStory2;
        public List<Story> fakeStoryList;
        public StoryDto fakeStoryDto;
        public StoryDto fakeStoryDto2;
        public List<StoryDto> fakeStoryDtoList;
        public StoryPostDto fakeStoryPostDto;
        public StoryUpdateDto fakeStoryUpdateDto;
        public FakeStoryData()
        {
            fakeStory = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory.Id = 1;
            fakeStory.Title = "Title1";
            fakeStory.Body = "Body2";
            fakeStory.AuthorId = 1;
            fakeStory.UpdatedAt = DateTime.UtcNow;
            fakeStory.CreatedAt = DateTime.UtcNow;

            fakeStory2 = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeStory2.Id = 1;
            fakeStory2.Title = "Title2";
            fakeStory2.Body = "Body2";
            fakeStory2.AuthorId = 1;
            fakeStory2.UpdatedAt = DateTime.UtcNow;
            fakeStory2.CreatedAt = DateTime.UtcNow;

            fakeStoryList = new List<Story> { fakeStory, fakeStory2 };

            fakeStoryDto = A.Fake<StoryDto>(x => x.WithArgumentsForConstructor(() => new StoryDto()));
            fakeStoryDto.Id = 1;
            fakeStoryDto.Title = "Title1";
            fakeStoryDto.Body = "Body1";
            fakeStoryDto.AuthorId = 1;

            fakeStoryDto2 = A.Fake<StoryDto>(x => x.WithArgumentsForConstructor(() => new StoryDto()));
            fakeStoryDto2.Id = 2;
            fakeStoryDto2.Title = "Title2";
            fakeStoryDto2.Body = "Body2";
            fakeStoryDto2.AuthorId = 1;

            fakeStoryDtoList = new List<StoryDto> { fakeStoryDto, fakeStoryDto2 };

            fakeStoryPostDto = A.Fake<StoryPostDto>(x => x.WithArgumentsForConstructor(() => new StoryPostDto()));
            fakeStoryPostDto.Title = "Title1";
            fakeStoryPostDto.Body = "Body1";
            fakeStoryPostDto.UserId = 1;

            fakeStoryUpdateDto = A.Fake<StoryUpdateDto>(x => x.WithArgumentsForConstructor(() => new StoryUpdateDto()));
            fakeStoryUpdateDto.Title = "Updated Title";
            fakeStoryUpdateDto.Body = "Updated Body";

        }
    }
}
