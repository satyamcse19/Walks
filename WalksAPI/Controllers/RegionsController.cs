﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;

namespace WalksAPI.Controllers
{
    //localhost:hostname/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly WalkDbContext _dbcontext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(WalkDbContext dbContext, IRegionRepository RegionRepository ,IMapper mapper)
        {
            this._dbcontext = dbContext;
            this._regionRepository = RegionRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {  
            var Regions = await _regionRepository.GetAllAsync();
            return Ok(_mapper.Map<List<RegionDto>>(Regions));
        }

        //get single region (get region by id )
        //localhost:hostnumber/api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Region = await _regionRepository.GetByIdAsync(id);
            if (Region == null)
                return NotFound();
            return Ok(_mapper.Map<RegionDto>(Region));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddRegionRequestDto addRegionRequestDto)
        {          
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);          
            var regionDto=_mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, AddRegionRequestDto updateRegionRequestDto)
        {          
            var regionsDomainModel= _mapper.Map<Region>(updateRegionRequestDto);
            regionsDomainModel = await _regionRepository.UpdateAsync(id, regionsDomainModel);
            return Ok(_mapper.Map<RegionDto>(regionsDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}

