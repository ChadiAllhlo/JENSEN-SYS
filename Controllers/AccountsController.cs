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

        // BAD LOGIN!!!
        [HttpPost("badlogin")]
        public ActionResult BadLogin()
        {
            HttpContext.Session.SetString("token", "fDJ8OJsgVAXE7JBs55vv5e2lOjeyicLMbQC70FSpzNMQod6xRfztn83r");
            var token = HttpContext.Session.GetString("token");
            Console.WriteLine($"TOKEN INSIDE BAD LOGIN {token}");

            // return RedirectToRoute(new { controller = "Products", action = "ListAllProducts" });
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
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
