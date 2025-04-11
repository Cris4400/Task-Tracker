public class TaskCliController
{
    public readonly Dictionary<string, Action<string[]>> _commands;

    public TaskCliController()
    {
        _commands = new Dictionary<string, Action<string[]>>
        {
            { CommandNames.Add, args => {
                if(CheckArgsNumber(args.Length, ArgumentCounts.Two)){
                    Result result = TaskManager.GetLastId();
                    HandleResult(result, GetIntData, args);
                }
                else PrintUsage(CommandNames.Add);
            }},
            { CommandNames.Update, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Three) && ParseId(args[1], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Description), args[2]);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.Update);
            }},
            { CommandNames.Delete, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[1], out int id)){
                    Result result = TaskManager.DeleteTask(id);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.Delete);
            }},
            { CommandNames.MarkInProgress, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[1], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Status), TaskStatus.in_progress);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.MarkInProgress);
            }},
            { CommandNames.MarkDone, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[1], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Status), TaskStatus.done);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.MarkDone);
            }},
            { CommandNames.List, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.One)){
                    Result result = TaskManager.GetTasks(null);
                    HandleResult(result, GetListTaskModelData, args);
                }
                else if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseEnum(args[1], out TaskStatus status)){
                    Result result = TaskManager.GetTasks(status);
                    HandleResult(result, GetListTaskModelData, args);
                }
                else PrintUsage(CommandNames.List);
            }},
            { CommandNames.Help, args => {
                if ( CheckArgsNumber(args.Length, ArgumentCounts.Two))
                {
                    if (HelpMessages._helpMessages.ContainsKey(args[1])) Console.WriteLine(HelpMessages._helpMessages[args[1]]);
                    else Console.WriteLine("Command not recognized");
                }
                else PrintUsage(CommandNames.Help);
            }}
        };
    }

    public bool CheckArgsNumber(int argsLenght, int argsNeeded)
    {
        return argsLenght == argsNeeded;
    }

    public bool ParseId(string stringID, out int id)
    {
        return int.TryParse(stringID, out id);
    }

    public bool ParseEnum(string stringStatus, out TaskStatus status)
    {
        return Enum.TryParse(stringStatus, out status);
    }

    public void GetIntData(object? data, string[] args)
    {
        if (data is int id)
        {
            Result r = TaskManager.AddTask(new TaskModel { Id = id, Description = args[1] });
            Console.WriteLine(r.Message);
        }
        else Console.WriteLine("Unexpected data type.");
    }

    public void GetListTaskModelData(object? data, string[] args)
    {
        if (data is List<TaskModel> tasks) PrintTasks(tasks);
        else Console.WriteLine("Unexpected data type.");
    }

    public void HandleResult(Result result, Action<object?, string[]> onSuccess, string[] args)
    {
        if (result.State)
        {
            onSuccess(result.Data, args);
        }
        else Console.WriteLine(result.Message);
    }

    public void ExecuteCommand(string[] args)
    {
        if (args.Length == ArgumentCounts.Zero)
        {
            PrintUsage(CommandNames.Help);
            return;
        }

        string command = args[0].ToLower();
        if (_commands.ContainsKey(command)) _commands[command](args);
        else PrintUsage(CommandNames.Help);
    }

    public void PrintTasks(List<TaskModel> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
        }

        foreach (TaskModel task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    public void PrintUsage(string command)
    {
        if (HelpMessages._helpMessages.ContainsKey(command)) Console.WriteLine(HelpMessages._helpMessages[command]);
        else Console.WriteLine(HelpMessages._helpMessages[CommandNames.Help]);
    }
}