using System.Runtime.CompilerServices;
using System.Text;

static class Constants
{
    public const string JSON_PATH = "data.json";
    public const string EMPTY_JSON = "[\n]";
}

public static class JSON
{
    public static int taskCounter = 0;
    public static void CreateFile()
    {
        try
        {
            File.WriteAllText(Constants.JSON_PATH, Constants.EMPTY_JSON);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void AddTask(Task task)
    {
        try
        {
            string separator = taskCounter == 0 ? "" : ",";
            DeleteBytes(2);

            File.AppendAllText(Constants.JSON_PATH, separator + task.ToJSON());
            File.AppendAllText(Constants.JSON_PATH, "\n]");
            taskCounter++;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void DeleteTask(Task task)
    {
        try
        {

            taskCounter--;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static List<string> GetTasks()
    {
        List<string> tasks = new List<string>();

        try
        {
            int count = 0;
            string padding = "";
            StringBuilder sb = new StringBuilder();

            foreach (string line in File.ReadLines(Constants.JSON_PATH))
            {
                if (line.Length > 4)
                {
                    count++;
                    string[] aux = line.Trim().Split(": ");
                    aux[1] = aux[1].Trim(',');
                    aux[1] = aux[1].Replace("\"", "");              

                    switch (count)
                    {
                        case 1:
                            string id = aux[1];
                            sb.Append(id + " ");
                            padding = new string(' ', id.Length);
                            break; 

                        case 2:
                            sb.AppendLine(aux[1]);
                            break;

                        case 3:
                            sb.Append($"{padding} Status: ");
                            sb.AppendLine(aux[1]);
                            break;

                        case 4:
                            sb.Append($"{padding} Created At: ");
                            sb.AppendLine(aux[1]);
                            break;

                        case 5:
                            sb.Append($"{padding} Updated At: ");
                            sb.AppendLine(aux[1]);
                            
                            tasks.Add(sb.ToString());
                            sb.Clear();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return tasks;
    }

    public static void DeleteBytes(int nBytes)
    {
        try
        {
            FileInfo fi = new FileInfo(Constants.JSON_PATH);
            FileStream fs = fi.Open(FileMode.Open);

            fs.SetLength(Math.Max(0, fi.Length - nBytes));
            fs.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}