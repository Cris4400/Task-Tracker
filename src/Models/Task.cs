using System.Text;

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
    public DateTime? UpdatedAt {get; private set;} = null;

    public void UpdateStatus(TaskStatus status){
        Status = status;
        UpdatedAt = DateTime.Now;
    }

    public override string ToString(){
        string padding = new string(' ', Id.ToString().Length);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{Id} {Description}"); 
        sb.AppendLine($"{padding} Status: {Status}");
        sb.AppendLine($"{padding} Created At: {CreatedAt}");
        sb.AppendLine($"{padding} Created At: {CreatedAt}");

        return sb.ToString();
    }

    public string ToJSON(){
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\n\t{");

        sb.AppendLine($"\t\t\"id\": {Id},");
        sb.AppendLine($"\t\t\"description\": \"{Description}\",");
        sb.AppendLine($"\t\t\"status\": \"{Status}\",");
        sb.AppendLine($"\t\t\"createdAt\": \"{CreatedAt}\",");
        sb.AppendLine($"\t\t\"updatedAt\": \"{UpdatedAt}\"");

        sb.Append("\t}");

        return sb.ToString();
    }
}