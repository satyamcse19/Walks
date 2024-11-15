﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using WalksAPI.CustomActionFilters;
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
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository RegionRepository ,IMapper mapper)
        {
            this._regionRepository = RegionRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {  
            var Regions = await _regionRepository.GetAllAsync();
            //create new exception for check Global middleware loger working
            //throw new Exception("This is new exception");
            return Ok(_mapper.Map<List<RegionDto>>(Regions));
        }

        //get single region (get region by id )
        //localhost:hostnumber/api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Region = await _regionRepository.GetByIdAsync(id);
            if (Region == null)
                return NotFound();
            return Ok(_mapper.Map<RegionDto>(Region));
        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create(AddRegionRequestDto addRegionRequestDto)
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
        public async Task<IActionResult> Update(Guid id, AddRegionRequestDto updateRegionRequestDto)
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

