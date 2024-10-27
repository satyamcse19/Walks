using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiVersioning.Interface;
using WebApiVersioning.Models.Dto;

namespace WebApiVersioning.V2.Controllers
{
    //localhost:hostname/api/Regions
    [Route("api/v2/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private readonly Icountry country;
        private readonly IMapper _mapper;

        public CountriesController(Icountry icountry, IMapper mapper)
        {
            this.country = icountry;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await country.GetCountriesAsync();
            //map domain to dto
            var countryDtos = _mapper.Map<List<CountrytDtov2>>(countries);  // v2 version chnage responce 
            return Ok(countryDtos);
        }
    }
}
