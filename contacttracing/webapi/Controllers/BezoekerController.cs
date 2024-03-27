using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using models.Bezoeker;
using webapi.Entities;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BezoekerController : ControllerBase
    {
        private readonly IBezoekerRepositorie _bezoekerRepository;

        public BezoekerController(IBezoekerRepositorie bezoekerRepository)
        {
            _bezoekerRepository = bezoekerRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<ICollection<GetBezoekerModel>>> GetAllBezoekersModels()
        {
            var bezoekerModels = await _bezoekerRepository.GetAllBezoekersModels();
            return Ok(bezoekerModels);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Medewerker,Bezoeker")]
        public async Task<ActionResult<GetBezoekerModel>> GetBezoekerModel(Guid id)
        {
            GetBezoekerModel? bezoekerModel = null;
            try
            {
                bezoekerModel = await _bezoekerRepository.GetBezoekerModel(id);
            }catch(Exception ex)
            {
                if (ex.Message == "Forbid") return Forbid();
                if (ex.Message == "NotFound") return NotFound();
            }
            
            return Ok(bezoekerModel);
        }

        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid>> PostBezoekerModel(PostAndPutBezoekerModel bezoekerModel)
        {
            var id = await _bezoekerRepository.PostBezoekerModel(bezoekerModel);
            if (id == Guid.Empty)
            {
                return StatusCode(500, "Failed to create bezoeker. Please try again later.");
            }
            return CreatedAtAction(nameof(GetBezoekerModel), new { id }, id);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid?>> PutBezoekerModel(Guid id, PostAndPutBezoekerModel bezoekerModel)
        {
            var updatedId = await _bezoekerRepository.PutBezoekerModel(id, bezoekerModel);
            if (updatedId == null)
            {
                return NotFound();
            }
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult> DeleteBezoekerModel(Guid id)
        {
            await _bezoekerRepository.DeleteBezoekerModel(id);
            return NoContent();
        }
    }
}
