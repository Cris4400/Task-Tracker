using System.Text.Json;

public static class TaskManager
{
    public static void AddTask(TaskModel task)
    {
        string taskJson = JsonSerializer.Serialize(task);

        try
        {
            File.AppendAllText(Constants.JSONL_FILE_PATH, taskJson + Environment.NewLine);
            Console.WriteLine($"Task added successfully (ID: {task.Id})");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error ocurred interacting with {Constants.JSONL_FILE_PATH} file.\nDetails:{e.Message}");
        }
    }

    public static List<TaskModel> GetTasks(TaskStatus? taskStatus)
    {
        List<TaskModel> tasks = new List<TaskModel>();
        try
        {
            foreach (string line in File.ReadLines(Constants.JSONL_FILE_PATH))
            {
                try
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);

                    if (task is null) continue;
                    if (taskStatus.HasValue && !task.Status.Equals(taskStatus.Value)) continue;

                    tasks.Add(task);
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"An error ocurred trying to deserialize line: {line}\nDetails:{e.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error ocurred interacting with {Constants.JSONL_FILE_PATH} file.\nDetails:{e.Message}");
        }

        return tasks;
    }

    public static bool UpdateTask(int id, string property, object value)
    {
        bool exists = false;
        List<string> tasksLines = new List<string>();

        if (property == nameof(TaskModel.Description) && value is not string) return false;
        if (property == nameof(TaskModel.Status) && value is not TaskStatus) return false;

        try
        {
            if (File.Exists(Constants.JSONL_FILE_PATH))
            {
                foreach (string line in File.ReadLines(Constants.JSONL_FILE_PATH))
                {
                    try
                    {
                        TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);
                        string taskLine = line;

                        if (task is not null && task.Id == id)
                        {
                            if (property == nameof(TaskModel.Description) && task.Description == (string) value) return false;
                            if (property == nameof(TaskModel.Status) && task.Status.Equals((TaskStatus) value)) return false;

                            exists = true;
                            task.UpdatedAt = DateTime.Now;

                            switch (property)
                            {
                                case nameof(TaskModel.Description):
                                    task.Description = (string)value;
                                    break;

                                case nameof(TaskModel.Status):
                                    task.Status = (TaskStatus)value;
                                    break;

                                default:
                                    break;
                            }

                            taskLine = JsonSerializer.Serialize(task);
                        }

                        tasksLines.Add(taskLine);
                    }
                    catch (JsonException e)
                    {
                        Console.WriteLine($"An error ocurred trying to deserialize line: {line}\nDetails:{e.Message}");
                    }
                }
            }

            if (exists)
            {
                File.WriteAllLines(Constants.JSONL_FILE_PATH, tasksLines);
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error ocurred interacting with {Constants.JSONL_FILE_PATH} file.\nDetails:{e.Message}");
        }

        Console.WriteLine($"Does not exists a task with ID {id}.");
        return false;
    }

    public static bool DeleteTask(int id)
    {
        bool exists = false;
        List<string> tasksLines = new List<string>();

        if (File.Exists(Constants.JSONL_FILE_PATH))
        {
            try
            {
                foreach (string line in File.ReadLines(Constants.JSONL_FILE_PATH))
                {
                    try
                    {
                        TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);
                        if (task is not null)
                        {
                            if (task.Id != id) tasksLines.Add(line);
                            else exists = true;
                        }
                    }
                    catch (JsonException e)
                    {
                        Console.WriteLine($"Error deserializing task while deleting ID {id}: {line}\nDetails:{e.Message}");
                    }
                }

                if (exists)
                {
                    File.WriteAllLines(Constants.JSONL_FILE_PATH, tasksLines);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error ocurred interacting with {Constants.JSONL_FILE_PATH} file.\nDetails:{e.Message}");
            }
        }

        Console.WriteLine($"Does not exists a task with ID {id}.");
        return false;
    }

    public static int GetLastId()
    {
        int lastId = 1;

        try
        {
            if (File.Exists(Constants.JSONL_FILE_PATH))
            {
                string lastTask = File.ReadLines(Constants.JSONL_FILE_PATH).LastOrDefault(string.Empty);

                if (!string.IsNullOrEmpty(lastTask))
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(lastTask);
                    if (task is not null) lastId = task.Id + 1;
                }
            }
        }
        catch (JsonException e)
        {
            Console.WriteLine($"An error ocurred trying to deserialize a line.\nDetails: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error ocurred interacting with {Constants.JSONL_FILE_PATH} file.\nDetails:{e.Message}");
        }

        return lastId;
    }
}
