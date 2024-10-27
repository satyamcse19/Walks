using AutoMapper;
using WebApiVersioning.Models.Domain;
using WebApiVersioning.Models.Dto;
using WebApiVersioning.Repositories;

namespace WebApiVersioning.Mapping
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Country, CountrytDto>();
            CreateMap<Country, CountrytDtov2>();

        }
    }
}
