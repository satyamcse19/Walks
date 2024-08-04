using AutoMapper;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
namespace WalksAPI.Mapping
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<WalkDto,Walk>().ReverseMap();
            CreateMap<DifficultyDto,Difficulty>().ReverseMap();
        }
    }
}
