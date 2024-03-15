using EY.Energy.Application.DTO;
using EY.Energy.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EY.Energy.Application.Services;
using EY.Energy.Application.EmailConfiguration;

namespace EY.Energy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServices _userService;
        private readonly IEmailService _emailService;

        public AuthenticationController(AuthenticationServices userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return Unauthorized();

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name , user.Username ),
        new Claim (ClaimTypes.Role, user.role.ToString()!)
    };



            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(claims);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            try
            {
                var newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    Phone = model.Phone,
                    role = null
                };

                if (!newUser.Email.EndsWith("@tn.ey.com"))
                {
                    return BadRequest(new { message = "Emails for Consultant and Manager roles must end with @tn.ey.com" });
                }

                await _userService.CreateUser(newUser);

                var emailSubject = "Your new account has been created successfully";
                var emailMessage = $"Hi {newUser.FirstName},\n\nYour account has been successfully created.\n\nHere is your login information:\n\nUsername: {newUser.Username}\nPassword: {newUser.Password}\n\nBest regards,\nEY";
              
                await _emailService.SendEmailAsync(newUser.Email, emailSubject, emailMessage);

                return Ok(new { message = "The user account has been created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return StatusCode(500, new { message = $"An error occurred while creating the user account: {ex.Message}" });
            }
        }



        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var existingUser = await _userService.GetUserByUsername(model.Username);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (Enum.TryParse(model.Role, true, out Role role))
            {
                existingUser.role = role;
                await _userService.UpdateUser(existingUser);

                return Ok(new { message = "The role has been successfully assigned to the user." });
            }
            else
            {
                return BadRequest(new { message = "The specified role is invalid." });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
