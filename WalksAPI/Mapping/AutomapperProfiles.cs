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
        }
    }
}
