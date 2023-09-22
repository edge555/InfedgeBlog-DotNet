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
            CreateMap<Story, StoryDto>()
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap();
            CreateMap<StoryPostDto, Story>();
            CreateMap<StoryUpdateDto, Story>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<SignupDto, User>();
            CreateMap<LoginDto, User>();
            CreateMap<UserPostDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserWithTokenDto>();
        }
    }
}
