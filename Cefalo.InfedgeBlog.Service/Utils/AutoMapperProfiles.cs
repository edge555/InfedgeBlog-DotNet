using AutoMapper;
using Cefalo.InfedgeBlog.Database.Model;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Story, StoryDto>().ReverseMap();
            CreateMap<StoryPostDto, Story>();
            CreateMap<StoryUpdateDto, Story>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserPostDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
