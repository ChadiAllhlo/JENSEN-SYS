using api.Data;
using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController(AppDbContext context, UserManager<User> userManager) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;
        private readonly HtmlSanitizer _htmlSanitizer = new();

        [HttpGet]
        public async Task<ActionResult> ListMyTodos()
        {
            var userId = _userManager.GetUserId(User);
            var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
            return Ok(new { success = true, data = todos });
        }

        [HttpPost]
        public async Task<ActionResult> CreateTodo(TodoCreateViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Title = _htmlSanitizer.Sanitize(model.Title);
            model.Description = _htmlSanitizer.Sanitize(model.Description ?? "");

            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid) return ValidationProblem();

            var userId = _userManager.GetUserId(User)!;
            var todo = new TodoItem
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = false,
                UserId = userId
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, new { success = true, data = todo });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTodo(string id)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo is null) return NotFound();
            return Ok(new { success = true, data = todo });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(string id, TodoUpdateViewModel model)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.Title = _htmlSanitizer.Sanitize(model.Title);
            model.Description = _htmlSanitizer.Sanitize(model.Description ?? "");

            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid) return ValidationProblem();

            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo is null) return NotFound();

            todo.Title = model.Title;
            todo.Description = model.Description;
            todo.IsCompleted = model.IsCompleted;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, data = todo });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(string id)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo is null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

