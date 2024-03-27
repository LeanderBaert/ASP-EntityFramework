using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using models.Users;
using System;
using System.Threading.Tasks;
using webapi.Repositories;

namespace webapi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IPersoonRepositorie _persoonRepository;
        private readonly string CookiePath = "Contacttracing.RefreshToken";
        public UsersController(IPersoonRepositorie memberRepository)
        {
            _persoonRepository = memberRepository;
        }


        [AllowAnonymous]
        [HttpPost("authenticeer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticeerResponseModel>> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel)
        {

            try
            {
                PostAuthenticeerResponseModel postAuthenticeerResponseModel = await _persoonRepository.Authenticate(postAuthenticateRequestModel, IpAddress());

                SetTokenCookie(postAuthenticeerResponseModel.RefreshToken);

                return postAuthenticeerResponseModel;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("vernieuw-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticeerResponseModel>> VernieuwToken()
        {
            try
            {
                string refreshToken = Request.Cookies[CookiePath];

                PostAuthenticeerResponseModel postAuthenticeerResponseModel = await _persoonRepository.RenewToken(refreshToken, IpAddress());

                SetTokenCookie(postAuthenticeerResponseModel.RefreshToken);

                return postAuthenticeerResponseModel;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deactiveer-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> DeactivateToken(PostDeactivateTokenRequestModel postDeactivateTokenRequestModel)
        {
            try
            {
                string? token = postDeactivateTokenRequestModel.Token ?? Request.Cookies[CookiePath];

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Refresh token is verpicht");
                }

                await _persoonRepository.DeactivateToken(token, IpAddress());

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void SetTokenCookie(string token)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(2), //Refresh token
                IsEssential = true
            };

            Response.Cookies.Append(CookiePath, token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}