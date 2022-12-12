using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this.mapper = mapper;
        }

        public IRegionRepository RegionRepository { get; }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllRegionsAsync();
            //var regionsDTO = new List<Models.DTO.Region>();
            // regions.ToList().ForEach(region=>
            //  {
            //      var regionDTO = new Models.DTO.Region()
            //      {
            //          Id = region.Id,
            //          Name = region.Name,
            //          Code = region.Code,
            //          Area = region.Area,
            //          Long = region.Long,
            //          Lat = region.Lat,
            //          Population = region.Population,
            //      };
            //      regionsDTO.Add(regionDTO);

            //  });
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);


            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]

        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetRegionAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);


            return Ok(regionDTO);



        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            //Convert DTO to Domain Model
            var region = new Models.Domain.Region() {

                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Name = addRegionRequest.Name,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Add Model to Repository

            var regionmodel = await _regionRepository.AddRegionAsync(region);

            //Convert Back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = regionmodel.Id,
                Code = regionmodel.Code,
                Area = regionmodel.Area,
                Name = regionmodel.Name,
                Lat = regionmodel.Lat,
                Long = regionmodel.Long,
                Population = regionmodel.Population


            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Deleting region from database
            var region = await _regionRepository.DeleteRegionAsync(id);

            //If null NotFound
            if (region == null)
            {
                return NotFound();
            }

            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Area = region.Area,
                Population = region.Population
            };
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionASync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            // Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };
            var updatedregion = await _regionRepository.UpdateRegionAsync(id, region);

            //if region is null

            if (updatedregion == null)
            {
                return NotFound();
            }

            //Convert from Model  to DTO
            var updatedRegionDTO = new Models.DTO.Region()
            {
                Id = updatedregion.Id,
                Name = updatedregion.Name,
                Code = updatedregion.Code,
                Lat = updatedregion.Lat,
                Long = updatedregion.Long,
                Area = updatedregion.Area,
                Population = region.Population
            };

            //Return Ok response
            return Ok(updatedRegionDTO);


        }
    }
}


    

