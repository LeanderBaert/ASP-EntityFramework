using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using models.AanwezigHeid;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AanwezigheidController : ControllerBase
    {
        private readonly IAanwezigheidRepositorie _aanwezigheidRepository;

        public AanwezigheidController(IAanwezigheidRepositorie aanwezigheidRepository)
        {
            _aanwezigheidRepository = aanwezigheidRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<ICollection<GetAanwezigHeidModel>>> GetAllAanwezigHeidModels()
        {
            ICollection<GetAanwezigHeidModel> aanwezigheidModels = await _aanwezigheidRepository.GetAllAanwezigHeidModels();
            return Ok(aanwezigheidModels);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Medewerker,Bezoeker")]
        public async Task<ActionResult<GetAanwezigHeidModel>> GetAanwezigHeidModel(Guid id)
        {
            GetAanwezigHeidModel aanwezigheidModel = null;

            try
            {
                aanwezigheidModel = await _aanwezigheidRepository.GetAanwezigHeidModel(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Forbid") return Forbid();
                if (ex.Message == "NotFound") return NotFound();
            }

            return Ok(aanwezigheidModel);
        }

        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid>> PostAanwezigHeidModel(PostAndPutAanwezigHeidModel model)
        {
            var id = await _aanwezigheidRepository.PostAanwezigHeidModel(model);
            return CreatedAtAction(nameof(GetAanwezigHeidModel), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid?>> PutAanwezigHeidModel(Guid id, PostAndPutAanwezigHeidModel model)
        {
            var updatedId = await _aanwezigheidRepository.PutAanwezigHeidModel(id, model);

            if (updatedId == null)
            {
                return NotFound();
            }

            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> DeleteAanwezigheidModel(Guid id)
        {
            await _aanwezigheidRepository.DeleteAanwezigheidModel(id);
            return NoContent();
        }
    }
}
