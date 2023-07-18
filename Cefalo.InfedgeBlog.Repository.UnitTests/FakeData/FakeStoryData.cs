using Cefalo.InfedgeBlog.Database.Model;
using FakeItEasy;

namespace Cefalo.InfedgeBlog.Repository.UnitTests.FakeData
{
    public class FakeStoryData
    {
        public Story fakeStory, fakeStory2, fakeUpdateStory;
        public List<Story> fakeStoryList;
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
            fakeStory2.Id = 2;
            fakeStory2.Title = "Title2";
            fakeStory2.Body = "Body2";
            fakeStory2.AuthorId = 1;
            fakeStory2.UpdatedAt = DateTime.UtcNow;
            fakeStory2.CreatedAt = DateTime.UtcNow;

            fakeStoryList = new List<Story> { fakeStory, fakeStory2 };

            fakeUpdateStory = A.Fake<Story>(x => x.WithArgumentsForConstructor(() => new Story()));
            fakeUpdateStory.Id = 1;
            fakeUpdateStory.Title = "Updated Title";
            fakeUpdateStory.Body = "Updated Body";
            fakeUpdateStory.AuthorId = 1;
            fakeUpdateStory.UpdatedAt = DateTime.UtcNow;
            fakeUpdateStory.CreatedAt = DateTime.UtcNow;

        }
    }
}
