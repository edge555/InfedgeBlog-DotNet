using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Service.Dtos;
using FakeItEasy;

namespace Cefalo.InfedgeBlog.Api.UnitTests.FakeData
{
    public class FakeUserData
    {
        public User fakeUser;
        public User fakeUser2;
        public UserDto fakeUserDto;
        public UserDto fakeUserDto2;
        public List<UserDto> fakeUserDtoList;
        public UserPostDto fakeUserPostDto;
        public UserUpdateDto fakeUserUpdateDto;
        public SignupDto fakeSignupDto;
        public LoginDto fakeLoginDto;
        public UserWithTokenDto fakeUserWithTokenDto;
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

            fakeUserDto = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto.Id = 1;
            fakeUserDto.Username = "edge555";
            fakeUserDto.Name = "Shoaib Ahmed";
            fakeUserDto.Email = "shoaibahmed@gmail.com";
            fakeUserDto.CreatedAt = DateTime.UtcNow;

            fakeUserDto2 = A.Fake<UserDto>(x => x.WithArgumentsForConstructor(() => new UserDto()));
            fakeUserDto2.Id = 2;
            fakeUserDto2.Username = "shoaib123";
            fakeUserDto2.Name = "Shoaib";
            fakeUserDto2.Email = "shoaib@gmail.com";
            fakeUserDto2.CreatedAt = DateTime.UtcNow;

            fakeUserDtoList = new List<UserDto>
            {
                fakeUserDto,
                fakeUserDto2
            };

            fakeUserPostDto = A.Fake<UserPostDto>(x => x.WithArgumentsForConstructor(() => new UserPostDto()));
            fakeUserPostDto.Username = "edge555";
            fakeUserPostDto.Name = "Shoaib Ahmed";
            fakeUserPostDto.Email = "shoaibahmed@gmail.com";
            fakeUserPostDto.Password = "abc123";

            fakeUserUpdateDto = A.Fake<UserUpdateDto>(x => x.WithArgumentsForConstructor(() => new UserUpdateDto()));
            fakeUserUpdateDto.Name = "Ahmed Shoaib";
            fakeUserUpdateDto.Password = "abc1234";

            fakeSignupDto = A.Fake<SignupDto>(x => x.WithArgumentsForConstructor(() => new SignupDto()));
            fakeSignupDto.Username = "edge555";
            fakeSignupDto.Name = "Shoaib Ahmed";
            fakeSignupDto.Email = "shoaibahmed@gmail.com";
            fakeSignupDto.Password = "abc123";

            fakeLoginDto = A.Fake<LoginDto>(x => x.WithArgumentsForConstructor(() => new LoginDto()));
            fakeLoginDto.Username = "edge555";
            fakeLoginDto.Password = "abc123";

            fakeUserWithTokenDto = A.Fake<UserWithTokenDto>(x => x.WithArgumentsForConstructor(() => new UserWithTokenDto()));
            fakeUserWithTokenDto.Username = "edge555";
            fakeUserWithTokenDto.Name = "Shoaib Ahmed";
            fakeUserWithTokenDto.Email = "shoaibahmed@gmail.com";
            fakeUserWithTokenDto.CreatedAt = DateTime.UtcNow;
            fakeUserWithTokenDto.Token = "dummyToken";
        }
    }
}
