public static class Constants
{
    public const string JSONL_FILE_PATH = "data.jsonl";
    public const int ONE_ARGS = 1;
    public const int TWO_ARGS = 2;
    public const int THREE_ARGS = 3;
    public const string ADD_COMMAND = "add";
    public const string ADD_COMMAND_HELP = "task-cli add [description]";
    public const string UPDATE_COMMAND = "update";
    public const string UPDATE_COMMAND_HELP = "task-cli update [id] [description]";
    public const string DELETE_COMMAND = "delete";
    public const string DELETE_COMMAND_HELP = "task-cli delete [id]";
    public const string MIP_COMMAND = "mark-in-progress";
    public const string MIP_COMMAND_HELP = "task-cli mark-in-progress [id]";
    public const string MD_COMMAND = "mark-done";
    public const string MD_COMMAND_HELP = "task-cli mark-done [id]";
    public const string LIST_COMMAND = "list";
    public const string LIST_COMMAND_HELP = @"
task-cli  list
          list done
          list todo
          list in-progress";
    public const string LD_COMMAND = "done";
    public const string LT_COMMAND = "todo";
    public const string LIP_COMMAND = "in-progress";

    public const string HELP_COMMAND = @"

    ";
}