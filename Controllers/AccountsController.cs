using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(SignInManager<User> signInManager) : ControllerBase
    {
        private readonly HtmlSanitizer _htmlSanitizer = new();

        // BAD LOGIN!!! - disabled in JWT flow but kept for parity
        [HttpPost("badlogin")]
        public ActionResult BadLogin()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Email = _htmlSanitizer.Sanitize(model.Email);
            model.FirstName = _htmlSanitizer.Sanitize(model.FirstName);
            model.LastName = _htmlSanitizer.Sanitize(model.LastName);
            model.Password = _htmlSanitizer.Sanitize(model.Password);

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

            if (result.Succeeded) return Ok();

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            return NoContent();
        }
    }
}
