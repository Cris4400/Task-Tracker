using System.Text;

/// <summary>
/// Enum <c>TaskStatus</c> represents the status of a task.
/// </summary>
/// remarks>
/// The <c>TaskStatus</c> enum defines three possible statuses for a task: "todo", "in_progress", and "done".
/// These statuses can be used to track the progress of tasks in a task management application.
/// </remarks>
public enum TaskStatus
{
    todo,
    in_progress,
    done
}

/// <summary>
/// Class <c>TaskModel</c> represents a task stored in the Task-Tracker application's JSONL file with properties such as Id, Description, Status, CreatedAt, and UpdatedAt.
/// </summary>
public class TaskModel
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.todo; // Default status is "todo"
    public DateTime CreatedAt { get; set; } = DateTime.Now;  // Default created time is now
    public DateTime? UpdatedAt { get; set; } = null;

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the TaskModel object.
    /// </summary>
    public override string ToString()
    {
        string padding = new string(' ', Id.ToString().Length); // Padding for alignment, based on Id length

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{Id} {Description}");
        sb.AppendLine($"{padding} Status: {Status}");
        sb.AppendLine($"{padding} Created At: {CreatedAt}");
        sb.AppendLine($"{padding} Updated At: {(UpdatedAt.HasValue ? UpdatedAt.ToString() : "N/A")}");

        return sb.ToString();
    }
}