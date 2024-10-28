using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiVersioning.Interface;
using WebApiVersioning.Models.Dto;

namespace WebApiVersioning.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly Icountry _country;
        private readonly IMapper _mapper;

        public CountriesController(Icountry country, IMapper mapper)
        {
            _country = country;
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetV1()
        {
            var countries = await _country.GetCountriesAsync();
            var countryDtos = _mapper.Map<List<CountrytDto>>(countries);
            return Ok(countryDtos);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetV2()
        {
            var countries = await _country.GetCountriesAsync();
            var countryDtos = _mapper.Map<List<CountrytDtov2>>(countries);
            return Ok(countryDtos);
        }
    }
}
