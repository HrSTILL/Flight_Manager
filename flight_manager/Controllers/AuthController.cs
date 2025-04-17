using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using flight_manager.Models;
using flight_manager.Services;
using System.Security.Claims;

namespace flight_manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _authService.Login(loginModel, out string token);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            HttpContext.SignInAsync("ApplicationCookie", claimsPrincipal);

            Response.Cookies.Append("login_tok", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new
            {
                token = token,
                rank = user.Rank
            });
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("ApplicationCookie"); 
            Response.Cookies.Delete("login_tok"); 
            return Redirect("/");
        }

    }
}
