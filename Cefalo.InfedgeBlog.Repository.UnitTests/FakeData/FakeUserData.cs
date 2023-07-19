using Cefalo.InfedgeBlog.Database.Models;
using FakeItEasy;

namespace Cefalo.InfedgeBlog.Repository.UnitTests.FakeData
{
    public class FakeUserData
    {
        public User fakeUser, fakeUser2, fakeUpdateUser;
        public List<User> fakeUserList;
        public FakeUserData()
        {
            fakeUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser.Id = 1;
            fakeUser.Username = "edge555";
            fakeUser.Name = "Shoaib Ahmed";
            fakeUser.Email = "shoaibahmed@gmail.com";
            fakeUser.Password = "abc123";
            fakeUser.UpdatedAt = DateTime.UtcNow;
            fakeUser.CreatedAt = DateTime.UtcNow;
            fakeUser.PasswordModifiedAt = DateTime.UtcNow;

            fakeUser2 = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUser2.Id = 2;
            fakeUser2.Username = "shoaib123";
            fakeUser2.Name = "Shoaib";
            fakeUser2.Email = "shoaib@gmail.com";
            fakeUser2.Password = "shoaib123";
            fakeUser2.UpdatedAt = DateTime.UtcNow;
            fakeUser2.CreatedAt = DateTime.UtcNow;
            fakeUser2.PasswordModifiedAt = DateTime.UtcNow;

            fakeUserList = new List<User> { fakeUser, fakeUser2 };

            fakeUpdateUser = A.Fake<User>(x => x.WithArgumentsForConstructor(() => new User()));
            fakeUpdateUser.Id = 1;
            fakeUpdateUser.Username = "edge555";
            fakeUpdateUser.Name = "Updated Name";
            fakeUpdateUser.Email = "shoaibahmed@gmail.com";
            fakeUpdateUser.Password = "updatedpassword123";
            fakeUpdateUser.UpdatedAt = DateTime.UtcNow;
            fakeUpdateUser.CreatedAt = DateTime.UtcNow;
            fakeUpdateUser.PasswordModifiedAt = DateTime.UtcNow;
        }
    }
}
