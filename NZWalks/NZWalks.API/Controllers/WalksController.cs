using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database
            var walks = await _walkRepository.GetAllAsyncWalks();

            //convert from domain to DTO
            var walksDTO = _mapper.Map<List<Models.DTO.Walk>>(walks);


            //return ok            
            return Ok(walksDTO);

        }




        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {

            //Getting walk domain object from database
            var walk = await _walkRepository.GetAsyncWalk(id);

            //checking if null or not
            if (walk == null)
            {
                return NotFound();
            }

            //Converting from Domain to DTO
            var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);

            //Returning OK Response
            return Ok(walkDTO);

        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert from DTO to domain model

            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,


            };

            //add to Repository
            var walkDomain = await _walkRepository.AddAsyncWalk(walk);

            //Convert back from Domain to DTO

            var walkDTO = _mapper.Map<Models.DTO.Walk>(walkDomain);

            //return OK response to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>  UpdateWalkAsync([FromRoute]Guid id,[FromBody]Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //convert DTO to domain model
            var walkdomain = new Models.Domain.Walk
            {
                Name=updateWalkRequest.Name,
                Length=updateWalkRequest.Length,
                RegionId=updateWalkRequest.RegionId,
                WalkDifficultyId =updateWalkRequest.WalkDifficultyId,

            };

            //add to repository
            var updatedwalk=await _walkRepository.UpdateAsyncWalk(id, walkdomain);

            //check if null
              if(updatedwalk==null)
               {
                return NotFound("Id was not found");

               }

            //convert again back to DTO from domain

            var walkDTO = new Models.DTO.Walk
            {
                Id=updatedwalk.Id,
                Name=updatedwalk.Name,
                Length = updatedwalk.Length,
                RegionId = updatedwalk.RegionId,
                WalkDifficultyId=updatedwalk.WalkDifficultyId,


            };

            //return response
            return Ok(walkDTO);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //get walk from repository
            var walk=await _walkRepository.DeleteAsyncWalk(id);

            //check if it is null
            if(walk==null)
            {
                return NotFound("Walk not found");
            }
            var walkDTO=_mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walk);

        }
    }
}
