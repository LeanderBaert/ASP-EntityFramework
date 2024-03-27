using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using models;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepositorie _restaurantRepository;

        public RestaurantController(IRestaurantRepositorie restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<ICollection<BaseRestaurantModel>>> GetAllRestaurantModels()
        {
            var restaurantModels = await _restaurantRepository.GetAllRestaurantModels();
            return Ok(restaurantModels);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Medewerker,Bezoeker")]
        public async Task<ActionResult<BaseRestaurantModel>> GetRestaurantModel(Guid id)
        {
            BaseRestaurantModel restaurantModel = null;

            try
            {
                restaurantModel = await _restaurantRepository.GetRestaurantModel(id);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Forbid") return Forbid();
                if (ex.Message == "NotFound") return NotFound();
            }

            return Ok(restaurantModel);
        }

        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid>> PostRestaurantModel(BaseRestaurantModel model)
        {
            var id = await _restaurantRepository.PostRestaurantModel(model);
            return CreatedAtAction(nameof(GetRestaurantModel), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<ActionResult<Guid?>> PutRestaurantModel(Guid id, BaseRestaurantModel model)
        {
            var updatedId = await _restaurantRepository.PutRestaurantModel(id, model);

            if (updatedId == null)
            {
                return NotFound();
            }

            return Ok(updatedId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> DeleteRestaurantModel(Guid id)
        {
            await _restaurantRepository.DeleteRestaurantModel(id);
            return NoContent();
        }
    }
}