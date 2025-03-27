public static class Constants
{
    public const string JSONL_FILE_PATH = "data.jsonl";
    public const int ONE_ARGS = 1;
    public const int TWO_ARGS = 2;
    public const int THREE_ARGS = 3;
    public const string ADD_COMMAND = "add";
    public const string ADD_COMMAND_HELP = "\nUsage: task-cli add <description>\n\nAdds a new task with the given description.";
    public const string UPDATE_COMMAND = "update";
    public const string UPDATE_COMMAND_HELP = "\nUsage: task-cli update <id> <new_description>\n\nUpdates the description of the task with the specified ID.";
    public const string DELETE_COMMAND = "delete";
    public const string DELETE_COMMAND_HELP = "\nUsage: task-cli delete <id>\n\nDeletes the task with the specified ID.";
    public const string MIP_COMMAND = "mark-in-progress";
    public const string MIP_COMMAND_HELP = "\nUsage: task-cli mark-in-progress <id>\n\nMarks the task with the specified ID as 'in progress'.";
    public const string MD_COMMAND = "mark-done";
    public const string MD_COMMAND_HELP = "\nUsage: task-cli mark-done <id>\n\nMarks the task with the specified ID as 'done'.";
    public const string LIST_COMMAND = "list";
    public const string LIST_COMMAND_HELP = @"
Usage: task-cli list [status]

Displays the list of tasks. If no status is specified, all tasks are shown.

Options:
  list                  Shows all tasks.
  list done             Shows only completed tasks.
  list todo             Shows only pending tasks.
  list in-progress      Shows only tasks in progress.";

    public const string LD_COMMAND = "done";
    public const string LT_COMMAND = "todo";
    public const string LIP_COMMAND = "in-progress";

    public const string HELP_COMMAND = @"
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
  task-cli list done
";
}