/// <summary>
/// The entry point for the Task-Traker application, which is a command-line task management tool.
/// It allows users to add, update, delete, and list tasks with various statuses.
/// </summary>
/// remarks>
/// The application is designed to be user-friendly and provides help messages for each command.
/// It uses a JSONL file to store task data, making it easy to manage and retrieve tasks.
/// </remarks>
/// author>Cris4400</author>
/// date>2025-04-13</date>
internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            TaskCliController controller = new TaskCliController();
            controller.ExecuteCommand(args);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
