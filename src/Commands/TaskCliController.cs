/// <summary>
/// TaskCliController class is responsible for handling command-line interface (CLI) commands related to task management.
/// It provides methods to add, update, delete, and list tasks, as well as display help messages for each command.
/// </summary>
/// remarks>
/// The TaskCliController class uses a dictionary to map command names to their corresponding actions.
/// It validates the number of arguments provided for each command and parses necessary data types (e.g., integers, enums).
/// It also handles the results of operations and prints appropriate messages to the console.
/// </remarks>
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

    /// <summary>
    /// Checks if the number of arguments provided matches the expected number.
    /// </summary>
    /// <param name="argsLenght">The number of arguments provided.</param>
    /// <param name="argsNeeded">The expected number of arguments.</param>
    /// <returns>True if the number of arguments matches; otherwise, false.</returns>
    public bool CheckArgsNumber(int argsLenght, int argsNeeded)
    {
        return argsLenght == argsNeeded;
    }

    /// <summary>
    /// Parses a string to an integer.
    /// </summary>
    /// <param name="stringID">The string to parse.</param>
    /// <param name="id">The parsed integer.</param>
    /// <returns>True (and parsed value) if parsing is successful; otherwise, false.</returns>
    public bool ParseId(string stringID, out int id)
    {
        return int.TryParse(stringID, out id);
    }

    /// <summary>
    /// Parses a string to a TaskStatus enum value.
    /// </summary>
    /// <param name="stringStatus">The string to parse.</param>
    /// <param name="status">The parsed TaskStatus enum value.</param>
    /// <returns>True (and parsed value) if parsing is successful; otherwise, false.</returns>
    public bool ParseEnum(string stringStatus, out TaskStatus status)
    {
        return Enum.TryParse(stringStatus, out status);
    }

    /// <summary>
    /// Handles the result of an operation and executes the success callback if the operation was successful.
    /// </summary>
    /// <param name="data">The data to be processed.</param>
    /// <param name="args">The command-line arguments.</param>
    /// <remarks>
    /// This method checks if the data is of type int and adds a new task with the given description.
    /// If the data is not of type int, it prints an error message.
    /// </remarks>
    public void GetIntData(object? data, string[] args)
    {
        if (data is int id)
        {
            Result r = TaskManager.AddTask(new TaskModel { Id = id, Description = args[1] });
            Console.WriteLine(r.Message);
        }
        else Console.WriteLine("Unexpected data type.");
    }

    /// <summary>
    /// Handles the result of an operation and executes the success callback if the operation was successful.
    /// </summary>
    /// <param name="data">The data to be processed.</param>
    /// <param name="args">The command-line arguments.</param>
    /// <remarks>
    /// This method checks if the data is of type List<TaskModel> and prints the tasks.
    /// If the data is not of type List<TaskModel>, it prints an error message.
    /// </remarks>
    public void GetListTaskModelData(object? data, string[] args)
    {
        if (data is List<TaskModel> tasks) PrintTasks(tasks);
        else Console.WriteLine("Unexpected data type.");
    }

    /// <summary>
    /// Handles the result of an operation and executes the success callback if the operation was successful.
    /// </summary>
    /// <param name="result">The result of the operation.</param>
    /// <param name="onSuccess">The action to execute on success.</param>
    /// <param name="args">The command-line arguments.</param>
    public void HandleResult(Result result, Action<object?, string[]> onSuccess, string[] args)
    {
        if (result.State)
        {
            onSuccess(result.Data, args);
        }
        else Console.WriteLine(result.Message);
    }

    /// <summary>
    /// Executes the command based on the provided arguments.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <remarks>
    /// This method checks if the command exists in the dictionary and executes the corresponding action.
    /// If the command is not recognized, it prints the help message.   
    /// </remarks>
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

    /// <summary>
    /// Prints the list of tasks to the console.
    /// </summary>
    /// <param name="tasks">The list of tasks to print.</param>
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

    /// <summary>
    /// Prints the usage information for a specific command or the help message if the command is not recognized.
    /// </summary>
    /// <param name="command">The command for which to print the usage information.</param>
    public void PrintUsage(string command)
    {
        if (HelpMessages._helpMessages.ContainsKey(command)) Console.WriteLine(HelpMessages._helpMessages[command]);
        else Console.WriteLine(HelpMessages._helpMessages[CommandNames.Help]);
    }
}