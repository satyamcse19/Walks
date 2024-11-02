using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.CustomActionFilters;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    //localhost:hostname/api/walks
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }
        //get walks
        //get:api/walks?filterOn=name&filterQuery=track
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll([FromQuery] String? filterOn , [FromQuery] string? filterQuery,
            [FromQuery]  string? sortBy , [FromQuery] bool ? isAscending , [FromQuery] int pageNumber = 1, int pageSize = 1000)
        {
            var walks = await _walkRepository.GetAllAsync(filterOn, filterQuery,sortBy, isAscending??true,pageNumber, pageSize);
            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var Walk = await _walkRepository.GetByIdAsync(id);
            if (Walk == null)
                return NotFound();
            return Ok(_mapper.Map<WalkDto>(Walk));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromForm] AddWalkRequestDto addWalkRequestDto)
        {
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);
                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] AddWalkRequestDto addWalkRequestDto)
        {
           
                var Walk = await _walkRepository.GetByIdAsync(id);
                if (Walk == null)
                    return NotFound();
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
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
