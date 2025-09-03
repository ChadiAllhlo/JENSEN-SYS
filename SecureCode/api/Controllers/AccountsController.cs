using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(SignInManager<User> signInManager) : ControllerBase
    {
        private readonly HtmlSanitizer _htmlSanitizer = new();

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Email = _htmlSanitizer.Sanitize(model.Email);
            model.FirstName = _htmlSanitizer.Sanitize(model.FirstName);
            model.LastName = _htmlSanitizer.Sanitize(model.LastName);

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await signInManager.UserManager.AddToRoleAsync(user, model.Role);
                if (roleResult.Succeeded)
                {
                    Console.WriteLine($"Role '{model.Role}' assigned to {user.Email}: {roleResult.Succeeded}");
                    return Ok();
                }
                else
                {
                    await signInManager.UserManager.DeleteAsync(user);
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return ValidationProblem();
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            request.Email = _htmlSanitizer.Sanitize(request.Email);

            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: true, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var roles = await signInManager.UserManager.GetRolesAsync(user);
            
            Console.WriteLine($"Profile requested for {user.Email}, roles: [{string.Join(", ", roles)}]");
            
            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                Roles = roles
            });
        }
    }
}
