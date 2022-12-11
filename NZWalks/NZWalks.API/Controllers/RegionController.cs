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

        public RegionController(IRegionRepository regionRepository,IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this.mapper = mapper;
        }

        public IRegionRepository RegionRepository { get; }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionRepository.GetAllAsync();
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
            var regionsDTO=mapper.Map<List<Models.DTO.Region>>(regions);
           
            
            return  Ok(regionsDTO);
        }
    }
}
