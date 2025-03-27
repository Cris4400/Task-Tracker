internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                PrintUsage("");
                return;
            }

            switch (args[0].ToLower())
            {
                case Constants.ADD_COMMAND:
                    if (args.Length == Constants.TWO_ARGS)
                    {
                        TaskModel task = new TaskModel { Id = TaskManager.GetLastId(), Description = args[1] };
                        TaskManager.AddTask(task);
                    }
                    else PrintUsage(Constants.ADD_COMMAND_HELP);
                    break;

                case Constants.UPDATE_COMMAND:
                    UpdateFunction(args, Constants.THREE_ARGS, nameof(TaskModel.Description), args[2], TaskManager.UpdateTask, Constants.UPDATE_COMMAND_HELP);
                    break;

                case Constants.DELETE_COMMAND:
                    if (args.Length == Constants.TWO_ARGS)
                    {
                        int id;
                        bool parsed = int.TryParse(args[1], out id);
                        if (parsed) TaskManager.DeleteTask(id);
                        else PrintUsage(Constants.DELETE_COMMAND_HELP);
                    }
                    else
                    {
                        PrintUsage(Constants.DELETE_COMMAND_HELP);
                    }
                    break;

                case Constants.MIP_COMMAND:
                    UpdateFunction(args, Constants.TWO_ARGS, nameof(TaskModel.Status), TaskStatus.in_progress, TaskManager.UpdateTask, Constants.MIP_COMMAND_HELP);
                    break;

                case Constants.MD_COMMAND:
                    UpdateFunction(args, Constants.TWO_ARGS, nameof(TaskModel.Status), TaskStatus.done, TaskManager.UpdateTask, Constants.MD_COMMAND_HELP);
                    break;

                case Constants.LIST_COMMAND:
                    if (args.Length == Constants.ONE_ARGS) ListTasks(TaskManager.GetTasks(null));
                    else if (args.Length == Constants.TWO_ARGS)
                    {
                        if (Enum.TryParse(args[1], out TaskStatus status)) ListTasks(TaskManager.GetTasks(status));
                        else PrintUsage(Constants.LIST_COMMAND_HELP);
                    }
                    else PrintUsage(Constants.LIST_COMMAND_HELP);
                    break;

                default:
                    PrintUsage(Constants.HELP_COMMAND);
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void UpdateFunction(string[] args, int numberArgs, string property, object value, Func<int, string, object, bool> action, string help)
    {
        if (args.Length == numberArgs)
        {
            int id;
            bool parsed = int.TryParse(args[1], out id);
            if (parsed) action(id, property, value);
            return;
        }

        PrintUsage(help);
    }

    static void ListTasks(List<TaskModel> tasks)
    {
        foreach (TaskModel task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    static void PrintUsage(string command)
    {
        Console.WriteLine(command switch
        {
            Constants.ADD_COMMAND => Constants.ADD_COMMAND_HELP,
            Constants.UPDATE_COMMAND => Constants.UPDATE_COMMAND_HELP,
            Constants.DELETE_COMMAND => Constants.DELETE_COMMAND_HELP,
            Constants.MIP_COMMAND => Constants.MIP_COMMAND_HELP,
            Constants.MD_COMMAND => Constants.MD_COMMAND_HELP,
            Constants.LIST_COMMAND => Constants.LIST_COMMAND_HELP,
            _ => Constants.HELP_COMMAND
        });
    }

}
