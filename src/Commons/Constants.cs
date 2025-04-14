/// <summary>
/// This file contains constants used throughout the application.
/// It includes command names, help messages, and file paths.
/// </summary>
/// <remarks>
/// The constants are organized into static classes for better organization and readability.
/// Each class contains related constants, such as command names, help messages, and file paths.
/// This structure allows for easy access and modification of constants as needed.
/// </remarks>
public static class FileConstants
{
  public static readonly string JsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"task-traker", "data.jsonl");
}

public static class CommandNames
{
  public const string Add = "add";
  public const string Update = "update";
  public const string Delete = "delete";
  public const string MarkInProgress = "mark-in-progress";
  public const string MarkDone = "mark-done";
  public const string List = "list";
  public const string Help = "help";
}

public static class HelpMessages
{
  public static readonly Dictionary<string, string> _helpMessages = new Dictionary<string, string>{
    {CommandNames.Add, "\nUsage: task-cli add <description>\n\nAdds a new task with the given description."},
    {CommandNames.Update, "\nUsage: task-cli update <id> <new_description>\n\nUpdates the description of the task with the specified ID." },
    {CommandNames.Delete, "\nUsage: task-cli delete <id>\n\nDeletes the task with the specified ID."},
    {CommandNames.MarkInProgress, "\nUsage: task-cli mark-in-progress <id>\n\nMarks the task with the specified ID as 'in progress'."},
    {CommandNames.MarkDone, "\nUsage: task-cli mark-done <id>\n\nMarks the task with the specified ID as 'done'."},
    {CommandNames.List, @"
    Usage: task-cli list [status]

    Displays the list of tasks. If no status is specified, all tasks are shown.

    Options:
      list                  Shows all tasks.
      list done             Shows only completed tasks.
      list todo             Shows only pending tasks.
      list in-progress      Shows only tasks in progress."},
    {CommandNames.Help, @"
    Usage: task-cli <command> [arguments]

    Commands:
      add <description>       Add a new task with the given description.
      update <id> <desc>      Update the description of a task by ID.
      delete <id>             Delete a task by ID.
      mark-in-progress <id>   Mark a task as 'in progress' by ID.
      mark-done <id>          Mark a task as 'done' by ID.
      list                    List all tasks.
      list done               List only completed tasks.
      list todo               List tasks that are still to be done.
      list in-progress        List tasks that are in progress.

    Examples:
      task-cli add ""Buy groceries""
      task-cli update 3 ""Finish project""
      task-cli delete 2
      task-cli mark-in-progress 5
      task-cli mark-done 8
      task-cli list
      task-cli list done"}
  };
}

public static class ArgumentCounts{
  public const int Zero = 0;
  public const int One = 1;
  public const int Two = 2;
  public const int Three = 3;
}