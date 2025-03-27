using System.Text;

public enum TaskStatus
{
    todo,
    in_progress,
    done
}

public class TaskModel
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.todo;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = null;

    public override string ToString()
    {
        string padding = new string(' ', Id.ToString().Length);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{Id} {Description}");
        sb.AppendLine($"{padding} Status: {Status}");
        sb.AppendLine($"{padding} Created At: {CreatedAt}");
        sb.AppendLine($"{padding} Updated At: {(UpdatedAt.HasValue ? UpdatedAt.ToString() : "N/A")}");

        return sb.ToString();
    }
}