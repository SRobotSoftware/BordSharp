using API.Domain.Models;
using API.Extensions;
using API.Resources;
using AutoMapper;

namespace API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Board, BoardResource>();
            CreateMap<Board, BoardMinimalResource>();
            CreateMap<Task, TaskResource>()
                .ForMember(src => src.Priority,
                           opt => opt.MapFrom(src => src.Priority.ToDescriptionString()));
            CreateMap<Task, TaskMinimalResource>()
                .ForMember(src => src.Priority,
                           opt => opt.MapFrom(src => src.Priority.ToDescriptionString()));
        }
    }
}
