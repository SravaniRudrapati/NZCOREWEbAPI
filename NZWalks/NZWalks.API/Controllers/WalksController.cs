using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
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
            //Validating addWalkRequest
            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            
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
            //Validating the incoming updateWalkRequest
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }


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

            return Ok(walkDTO);

        }

        #region PrivateMethods

         private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
         {
            if(addWalkRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty");
            }
            if(string.IsNullOrEmpty(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty or whitespace");
            }

            if(addWalkRequest.Length < 0)

            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} cannot be less than zero");
            }
             var region=await _regionRepository.GetRegionAsync(addWalkRequest.RegionId);
            if(region==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");
            }

              var walkdifficulty=await _walkDifficultyRepository.GetWalkDifficultyAsync(addWalkRequest.WalkDifficultyId);
              if(walkdifficulty==null)
              {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");

              }
              if(ModelState.ErrorCount>0)
              {
                return false;

              }
            return true;

         }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if(updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),$"{nameof(updateWalkRequest)} cannot be empty");
            }
            if(string.IsNullOrEmpty(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),$"{nameof(updateWalkRequest.Name)} cannot be null or empty or whitespace");
            }

            if (updateWalkRequest.Length < 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} cannot be less than zero");
            }
            var region = await _regionRepository.GetRegionAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            var walkdifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(updateWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;

            }
            return true;

        }
        #endregion
    }
}
