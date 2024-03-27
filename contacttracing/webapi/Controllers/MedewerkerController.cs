using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using models.MedeWerker;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class MedewerkerController : ControllerBase
    {
        private readonly IMedewerkerRepositorie _medewerkerRepository;

        public MedewerkerController(IMedewerkerRepositorie medewerkerRepository)
        {
            _medewerkerRepository = medewerkerRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<ICollection<GetMedeWerkerModel>>> GetAllMedewerkerModels()
        {
            var medewerkerModels = await _medewerkerRepository.GetAllMedewerkerModels();
            return Ok(medewerkerModels);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<GetMedeWerkerModel>> GetMedewerkerModel(Guid id)
        {
            GetMedeWerkerModel medewerkerModel = null;

            try
            {
                medewerkerModel = await _medewerkerRepository.GetMedewerkerModel(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "NotFound") return NotFound();
            }
            return Ok(medewerkerModel);
        }

        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid>> PostMedewerkerModel(PostAndPutMedewerkerModel medewerkerModel)
        {
            var id = await _medewerkerRepository.PostMedewerkerModel(medewerkerModel);
            return CreatedAtAction(nameof(GetMedewerkerModel), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid?>> PutMedewerkerModel(Guid id, PostAndPutMedewerkerModel medewerkerModel)
        {
            var updatedId = await _medewerkerRepository.PutMedewerkerModel(id, medewerkerModel);
            if (updatedId == null)
            {
                return NotFound();
            }
            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult> DeleteMedewerkerModel(Guid id)
        {
            await _medewerkerRepository.DeleteMedewerkerModel(id);
            return NoContent();
        }
    }
}
