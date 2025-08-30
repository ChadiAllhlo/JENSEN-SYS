using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<User> userManager) : ControllerBase
    {
        private readonly HtmlSanitizer _sanitizer = new();

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Email = _sanitizer.Sanitize(model.Email);
            model.FirstName = _sanitizer.Sanitize(model.FirstName);
            model.LastName = _sanitizer.Sanitize(model.LastName);
            model.Password = _sanitizer.Sanitize(model.Password);

            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid) return ValidationProblem();

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            return Ok();
        }
    }
}

