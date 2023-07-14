using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Service.Dtos;
using FakeItEasy;

namespace Cefalo.InfedgeBlog.Api.UnitTests.FakeData
{
    public class FakeStoryData
    {
        public StoryDto fakeStoryDto;
        public StoryDto fakeStoryDto2;
        public List<StoryDto> fakeStoryDtoList;
        public StoryPostDto fakeStoryPostDto;
        public StoryUpdateDto fakeStoryUpdateDto;
        public FakeStoryData()
        {
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
            fakeStoryPostDto.Body = "Body";
            fakeStoryPostDto.UserId = 1;

            fakeStoryUpdateDto = A.Fake<StoryUpdateDto>(x => x.WithArgumentsForConstructor(() => new StoryUpdateDto()));
            fakeStoryUpdateDto.Title = "Updated Title";
            fakeStoryUpdateDto.Body = "Updated Body";

        }
    }
}
