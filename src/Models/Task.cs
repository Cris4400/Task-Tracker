public enum TaskStatus{
    todo,
    in_progress,
    done
}

public class Task{
    public static int IdCounter = 1;
    public int Id { get; } = IdCounter++;
    public required string Description { get; init; }
    public TaskStatus Status { get; private set; } = TaskStatus.todo;
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime UpdatedAt {get; private set;}
}