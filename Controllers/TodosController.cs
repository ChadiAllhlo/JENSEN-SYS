using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public TodosController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/todos
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

    // GET: api/todos/5
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

    // POST: api/todos
    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = new Todo
        {
            Title = request.Title,
            Description = request.Description,
            UserId = user.Id
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    // PUT: api/todos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (todo == null)
            return NotFound();

        todo.Title = request.Title ?? todo.Title;
        todo.Description = request.Description ?? todo.Description;
        todo.IsCompleted = request.IsCompleted ?? todo.IsCompleted;
        
        if (todo.IsCompleted && !todo.CompletedAt.HasValue)
            todo.CompletedAt = DateTime.UtcNow;
        else if (!todo.IsCompleted)
            todo.CompletedAt = null;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/todos/5
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

public class CreateTodoRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateTodoRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
}
