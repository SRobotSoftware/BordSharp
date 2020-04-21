using API.Domain.Models;
using API.Resources;
using AutoMapper;

namespace API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveBoardResource, Board>();
            CreateMap<SaveTaskResource, Task>();
        }
    }
}
