using api.Data;
using api.Models;
using api.ViewModels;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly HtmlSanitizer _htmlSanitizer = new();

    public TodosController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todos = await _context.Todos
            .Where(t => t.UserId == user.Id)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodo(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo([FromBody] CreateTodoRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem();
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = new Todo
        {
            Title = _htmlSanitizer.Sanitize(request.Title.Trim()),
            Description = request.Description != null ? _htmlSanitizer.Sanitize(request.Description.Trim()) : null,
            UserId = user.Id
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem();
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (todo == null)
            return NotFound();

        if (request.Title != null)
            todo.Title = _htmlSanitizer.Sanitize(request.Title.Trim());
        if (request.Description != null)
            todo.Description = _htmlSanitizer.Sanitize(request.Description.Trim());
        todo.IsCompleted = request.IsCompleted ?? todo.IsCompleted;
        
        if (todo.IsCompleted && !todo.CompletedAt.HasValue)
            todo.CompletedAt = DateTime.UtcNow;
        else if (!todo.IsCompleted)
            todo.CompletedAt = null;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (todo == null)
            return NotFound();

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
