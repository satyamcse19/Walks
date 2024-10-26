using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Text.Json;
using WalksAPI.CustomActionFilters;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using Microsoft.Extensions.Logging;

namespace WalksAPI.Controllers
{
    //localhost:hostname/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository RegionRepository ,IMapper mapper,ILogger<RegionsController> logger)
        {
            this._regionRepository = RegionRepository;
            this._mapper = mapper;
            this._logger = logger;
        }
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("get all coming");
                var Regions = await _regionRepository.GetAllAsync();
                _logger.LogInformation($"data coming {JsonSerializer.Serialize(_mapper.Map<List<RegionDto>>(Regions))}");
                return Ok(_mapper.Map<List<RegionDto>>(Regions));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,ex.Message);
                return StatusCode(500, "Internal server error");
            }
           
        }

        //get single region (get region by id )
        //localhost:hostnumber/api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Region = await _regionRepository.GetByIdAsync(id);
            if (Region == null)
                return NotFound();
            return Ok(_mapper.Map<RegionDto>(Region));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromForm] AddRegionRequestDto addRegionRequestDto)
        {
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);           
    
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, AddRegionRequestDto updateRegionRequestDto)
        {   
                var regionsDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
                regionsDomainModel = await _regionRepository.UpdateAsync(id, regionsDomainModel);
                return Ok(_mapper.Map<RegionDto>(regionsDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}

