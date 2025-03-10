using System.Runtime.CompilerServices;
using System.Text;

static class Constants
{
    public const string JSON_PATH = "data.json";
    public const string JSON_TEMP_PATH = "temp.json";
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
            DeleteBytes(Constants.JSON_PATH, 2);

            File.AppendAllText(Constants.JSON_PATH, separator + task.ToJSON());
            File.AppendAllText(Constants.JSON_PATH, "\n]");
            taskCounter++;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void UpdateTask(int id, string property, string value)
    {
        try
        {
            bool found = false;
            string newLine = "";

            foreach (string line in File.ReadLines(Constants.JSON_PATH))
            {
                if (found && line.Contains(property))
                {
                    newLine = $"\t\t\"{property}\": \"{value}\",";
                }
                else if (found && line.Contains("updatedAt"))
                {
                    newLine = $"\t\t\"updatedAt\": \"{DateTime.Now}\"";
                    found = false;
                }
                else
                {
                    newLine = line;
                }

                if (line.Contains($"\"id\": {id}"))
                {
                    found = true;
                }

                File.AppendAllText(Constants.JSON_TEMP_PATH, newLine + "\n");
            }

            File.Delete(Constants.JSON_PATH);
            File.Move(Constants.JSON_TEMP_PATH, Constants.JSON_PATH);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void DeleteTask(int id)
    {
        try
        {
            int nNextLines = 0;

            foreach (string line in File.ReadLines(Constants.JSON_PATH))
            {   
                if (nNextLines == 1){
                    nNextLines = 0;
                    DeleteBytes(Constants.JSON_TEMP_PATH, 3);
                }

                if (line.Contains($"\"id\": {id}"))
                {
                    nNextLines = 7;
                }

                if (nNextLines > 1)
                {
                    nNextLines--;
                }
                else{
                    File.AppendAllText(Constants.JSON_TEMP_PATH, line + "\n");
                }
            }

            File.Delete(Constants.JSON_PATH);
            File.Move(Constants.JSON_TEMP_PATH, Constants.JSON_PATH);
            taskCounter--;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static List<string> GetTasks(TaskStatus? status)
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
                            if (status != null && aux[1] != status.ToString())
                            {
                                count = 6;
                                sb.Clear();
                                break;
                            }

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

    public static void DeleteBytes(string path, int nBytes)
    {
        try
        {
            FileInfo fi = new FileInfo(path);
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