using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WalksAPI.Data;
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
        public RegionsController(WalkDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult>  GetAll()
        {   // get  data from database - Domain models
            var Regions = await _dbcontext.Regions.ToListAsync();
            //map Domain models to Dto
            var RegionsDto = new List<RegionDto>();
            foreach (var Region in Regions)
            {
                RegionsDto.Add(new RegionDto()
                {
                    Id = Region.Id,
                    Name = Region.Name,
                    Code = Region.Code,
                    RegionImageUrl = Region.RegionImageUrl
                });
            }

            //back to return regionsDto to client

            return Ok(RegionsDto);
        }

        //get single region (get region by id )
        //localhost:7144/api/Regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult>  GetById([FromRoute] Guid id)
        {
            //var Regions = _dbcontext.Regions.Find(id);
            var Region = await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (Region == null)
                return NotFound();
            //map domain model to dto
            var RegionsDto = new RegionDto()
            {
                Id = Region.Id,
                Name = Region.Name,
                Code = Region.Code,
                RegionImageUrl = Region.RegionImageUrl
            };

            return Ok(RegionsDto);
        }

        [HttpPost]
        public async Task<IActionResult>Create([FromForm] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert  dto to domain 
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

           await _dbcontext.AddAsync(regionDomainModel);
           await _dbcontext.SaveChangesAsync();

            //map domain model back to dto  

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult>  Update([FromRoute] Guid id, AddRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel=await _dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null)
                return NotFound();
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name= updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
           await _dbcontext.SaveChangesAsync();

            //  map or convert  Domain Model to Dto

            var regionDto = new RegionDto
            {
                Id= regionDomainModel.Id,
                Code= regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel =await _dbcontext.Regions.FirstOrDefaultAsync(s => s.Id == id);
            if (regionDomainModel == null)
                return NotFound();
            _dbcontext.Regions.Remove(regionDomainModel);
            _dbcontext.SaveChanges();
            //return delete region back
            //map domain to dto 
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}

