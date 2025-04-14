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

    /// <summary>
    /// Constructor for the TaskCliController class.
    /// Initializes the dictionary of commands with their corresponding actions.
    /// </summary>
    /// <remarks>
    /// The constructor sets up the command dictionary with actions for each command.
    /// Each action is responsible for handling the command's logic, including argument validation and result handling.
    /// </remarks>
    public TaskCliController()
    {
        _commands = new Dictionary<string, Action<string[]>>
        {
            /// <summary>
            /// Handles the add command by checking the number of arguments and parsing the ID.
            /// If the ID is valid, it calls the TaskManager to add a new task and prints the result message.
            /// </summary>
            { CommandNames.Add, args => {
                if(CheckArgsNumber(args.Length, ArgumentCounts.Two)){
                    Result result = TaskManager.GetLastId();
                    HandleResult(result, GetIntData, args);
                }
                else PrintUsage(CommandNames.Add);
            }},
            /// <summary>
            /// Handles the update command by checking the number of arguments and parsing the ID.
            /// If the ID is valid, it calls the TaskManager to update the task and prints the result message.
            /// </summary>
            { CommandNames.Update, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Three) && ParseId(args[ArgumentCounts.One], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Description), args[ArgumentCounts.Two]);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.Update);
            }},
            /// <summary>
            /// Handles the delete command by checking the number of arguments and parsing the ID.
            /// If the ID is valid, it calls the TaskManager to delete the task and prints the result message.
            /// </summary>
            { CommandNames.Delete, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[ArgumentCounts.One], out int id)){
                    Result result = TaskManager.DeleteTask(id);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.Delete);
            }},
            /// <summary>
            /// Handles the mark-in-progress command by checking the number of arguments and parsing the ID.
            /// If the ID is valid, it calls the TaskManager to update the task status to "in progress" and prints the result message.
            /// </summary>
            { CommandNames.MarkInProgress, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[ArgumentCounts.One], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Status), TaskStatus.in_progress);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.MarkInProgress);
            }},
            /// <summary>
            /// Handles the mark-done command by checking the number of arguments and parsing the ID.
            /// If the ID is valid, it calls the TaskManager to update the task status to "done" and prints the result message.
            /// </summary>
            { CommandNames.MarkDone, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseId(args[ArgumentCounts.One], out int id)){
                    Result result = TaskManager.UpdateTask(id, nameof(TaskModel.Status), TaskStatus.done);
                    Console.WriteLine(result.Message);
                }
                else PrintUsage(CommandNames.MarkDone);
            }},
            /// <summary>
            /// Handles the list command by checking the number of arguments and parsing the status if provided.
            /// If the arguments are valid, it calls the TaskManager to get the tasks and prints them using the GetListTaskModelData method.    
            /// </summary>
            { CommandNames.List, args => {
                if (CheckArgsNumber(args.Length, ArgumentCounts.One)){
                    Result result = TaskManager.GetTasks(null);
                    HandleResult(result, GetListTaskModelData, args);
                }
                else if (CheckArgsNumber(args.Length, ArgumentCounts.Two) && ParseEnum(args[ArgumentCounts.One], out TaskStatus status)){
                    Result result = TaskManager.GetTasks(status);
                    HandleResult(result, GetListTaskModelData, args);
                }
                else PrintUsage(CommandNames.List);
            }},
            /// <summary>
            /// Handles the help command by checking the number of arguments.
            /// If the arguments are valid, it prints the help message for the specified command or a default help message if the command is not recognized.
            /// </summary>
            { CommandNames.Help, args => {
                if ( CheckArgsNumber(args.Length, ArgumentCounts.Two))
                {
                    if (HelpMessages._helpMessages.ContainsKey(args[ArgumentCounts.One])) Console.WriteLine(HelpMessages._helpMessages[args[ArgumentCounts.One]]);
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
            Result r = TaskManager.AddTask(new TaskModel { Id = id, Description = args[ArgumentCounts.One] });
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
        if (args.Length == 0)
        {
            PrintUsage(CommandNames.Help);
            return;
        }

        string command = args[ArgumentCounts.Zero].ToLower();
        if (_commands.ContainsKey(command)){
           Result result = TaskManager.VerifyDirectory();
           if (result.State) _commands[command](args);
           else Console.WriteLine(result.Message);
        }
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