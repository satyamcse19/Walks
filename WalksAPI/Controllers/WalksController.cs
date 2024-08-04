﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    //localhost:hostname/api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walks = await _walkRepository.GetAllAsync();
            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Walk = await _walkRepository.GetByIdAsync(id);
            if (Walk == null)
                return NotFound();
            return Ok(_mapper.Map<WalkDto>(Walk));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);
                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var Walk = await _walkRepository.GetByIdAsync(id);
                if (Walk == null)
                    return NotFound();
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                return Ok(_mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var Walk = await _walkRepository.GetByIdAsync(id);
            if (Walk == null)
                return NotFound();
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
