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
