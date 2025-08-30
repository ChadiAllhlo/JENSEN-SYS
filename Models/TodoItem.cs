namespace api.Models;

public class TodoItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public required string UserId { get; set; }
    public User? User { get; set; }
}

