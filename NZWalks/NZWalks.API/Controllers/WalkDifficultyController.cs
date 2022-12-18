using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository,IMapper  mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetWalkDifficultiesAsync()
        {
            var walkdifficulty = await _walkDifficultyRepository.GetAllWalkDifficultiesAsync();
            return Ok(walkdifficulty);
        }



        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkdifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(id);
            if(walkdifficulty == null)
            {
                return NotFound();
            }
            return Ok(walkdifficulty);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody]Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Validating the addWalkDifficultyRequest
            if(!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            //convert from DTO to Domain

            var walkdifficuly = new Models.Domain.WalkDifficulty
            {
                Code= addWalkDifficultyRequest.Code,

            };

            //add to repository
            var  walkdifficultydomain=await _walkDifficultyRepository.AddWalkDifficultyAsync(walkdifficuly);

            //convert from Domain to DTO
            var walkDifficultyDTO=_mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain);


            //return OK(response)
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validating updateWalkDifficultyRequest
            if(!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(updateWalkDifficultyRequest);
            }


            //convert DTO to Domain Model

            var walkdifficultydomain =new Models.Domain.WalkDifficulty()
            {
                Code=updateWalkDifficultyRequest.Code
            };

            //Add to Repository
             var walkdifficultydomain1=await _walkDifficultyRepository.UpdateWalkDifficultyAsync(id, walkdifficultydomain);

            //check if null
            if(walkdifficultydomain1==null)
            {
                return NotFound("Walk not found");
            }

            //Convert from Domain to DTO
             
            var walkdifficultyDTO= _mapper.Map<Models.DTO.WalkDifficulty>(walkdifficultydomain1);



            //return response
            return Ok(walkdifficultyDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkdifficulty = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);
            if(walkdifficulty==null)
            {
                return NotFound("Walk Not Found");

            }
            return Ok(walkdifficulty);

        }

        #region PrivateMethods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"{nameof(addWalkDifficultyRequest)} cannot be null");
            }
            if(string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be null or empty or whitespace");
            }
            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"{nameof(updateWalkDifficultyRequest)} cannot be null");
            }
            if (string.IsNullOrEmpty(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null or empty or whitespace");
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
