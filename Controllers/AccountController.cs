using ApiSkeleton.Attributes;
using ApiSkeleton.Common;
using ApiSkeleton.Protocols;
using ApiSkeleton.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiSkeleton.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest req)
        {
            var res = await _userService.Authenticate(req);

            Response.Cookies.Append("X-Access-Token", res.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = false });
            Response.Cookies.Append("X-Username", res.Email, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Lax, Secure = false });

            if (res == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(res);
        }

        [Authorize(Roles = "Admin", Optional = false)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader(Name = Constants.HEADER_USER_ID)] long userId)
        {
            System.Console.WriteLine($"User ID : {userId}");
            return Ok(await _userService.GetAll());
        }
    }
}
