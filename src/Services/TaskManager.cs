using System.Text.Json;
using Microsoft.VisualBasic;

public static class TaskManager
{
    public static Result AddTask(TaskModel task)
    {
        try
        {
            string taskJson = JsonSerializer.Serialize(task);
            File.AppendAllText(FileConstants.JSONL_FILE_PATH, taskJson + Environment.NewLine);
        }
        catch (UnauthorizedAccessException e)
        {
            return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} is not accessible.\nDetails: {e.Message}", null);
        }
        catch (NotSupportedException e)
        {
            return Result.Failure($"An error occurred while writing the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (IOException e)
        {
            return Result.Failure($"An error occurred while writing the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (JsonException e)
        {
            return Result.Failure($"An error ocurred trying to serialize a line.\nDetails: {e.Message}", null);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, null);
        }

        return Result.Success($"Task added successfully (ID: {task.Id}).", null);
    }

    public static Result GetTasks(TaskStatus? taskStatus)
    {
        List<TaskModel> tasks = new List<TaskModel>();
        try
        {
            if (File.Exists(FileConstants.JSONL_FILE_PATH))
            {
                foreach (string line in File.ReadLines(FileConstants.JSONL_FILE_PATH))
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);

                    if (task is null) continue;
                    if (taskStatus.HasValue && !task.Status.Equals(taskStatus.Value)) continue;

                    tasks.Add(task);
                }
            }
            else
            {
                return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} does not exist.", null);
            }
        }
        catch (UnauthorizedAccessException e)
        {
            return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} is not accessible.\nDetails: {e.Message}", null);
        }
        catch (NotSupportedException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (IOException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (JsonException e)
        {
            return Result.Failure($"An error ocurred trying to deserialize a line.\nDetails: {e.Message}", null);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, null);
        }

        return Result.Success("Tasks retrieved successfully.", tasks);
    }

    public static Result UpdateTask(int id, string property, object value)
    {
        bool exists = false;
        List<string> tasksLines = new List<string>();

        if (property == nameof(TaskModel.Description) && value is not string) return Result.Failure("Given description value is not a string.", null);
        if (property == nameof(TaskModel.Status) && value is not TaskStatus) return Result.Failure("Given status value is not TaskModel status.", null);

        try
        {
            if (File.Exists(FileConstants.JSONL_FILE_PATH))
            {
                foreach (string line in File.ReadLines(FileConstants.JSONL_FILE_PATH))
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);
                    string taskLine = line;

                    if (task is not null && task.Id == id)
                    {
                        if (property == nameof(TaskModel.Description) && task.Description == (string)value) return Result.Failure($"Task (ID: {id}) already has that description.", null);
                        if (property == nameof(TaskModel.Status) && task.Status.Equals((TaskStatus)value)) return Result.Failure($"Task (ID: {id}) already has that status.", null);

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

                if (exists)
                {
                    File.WriteAllLines(FileConstants.JSONL_FILE_PATH, tasksLines);
                    return Result.Success($"Task (ID: {id}) updated successfully.", null);
                }
            }
            else
            {
                return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} does not exist.", null);
            }
        }
        catch (UnauthorizedAccessException e)
        {
            return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} is not accessible.\nDetails: {e.Message}", null);
        }
        catch (NotSupportedException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (IOException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (JsonException e)
        {
            return Result.Failure($"An error ocurred trying to deserialize a line.\nDetails: {e.Message}", null);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, null);
        }

        return Result.Failure($"Task (ID: {id}) not found.", null);
    }

    public static Result DeleteTask(int id)
    {
        bool exists = false;
        List<string> tasksLines = new List<string>();

        try
        {
            if (File.Exists(FileConstants.JSONL_FILE_PATH))
            {
                foreach (string line in File.ReadLines(FileConstants.JSONL_FILE_PATH))
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(line);
                    if (task is not null)
                    {
                        if (task.Id != id) tasksLines.Add(line);
                        else exists = true;
                    }
                }

                if (exists)
                {
                    File.WriteAllLines(FileConstants.JSONL_FILE_PATH, tasksLines);
                    return Result.Success($"Task (ID: {id}) deleted successfully.", null);
                }
            }
            else
            {
                return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} does not exist.", null);
            }
        }
        catch (UnauthorizedAccessException e)
        {
            return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} is not accessible.\nDetails: {e.Message}", null);
        }
        catch (NotSupportedException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (IOException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (JsonException e)
        {
            return Result.Failure($"An error ocurred trying to deserialize a line.\nDetails: {e.Message}", null);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, null);
        }

        return Result.Failure($"Task (ID: {id}) not found.", null);
    }

    public static Result GetLastId()
    {
        int lastId = 1;

        try
        {
            if (File.Exists(FileConstants.JSONL_FILE_PATH))
            {
                string lastTask = File.ReadLines(FileConstants.JSONL_FILE_PATH).LastOrDefault(string.Empty);

                if (!string.IsNullOrEmpty(lastTask))
                {
                    TaskModel? task = JsonSerializer.Deserialize<TaskModel>(lastTask);
                    if (task is not null) lastId = task.Id + 1;
                }
            }
        }
        catch (UnauthorizedAccessException e)
        {
            return Result.Failure($"File {FileConstants.JSONL_FILE_PATH} is not accessible.\nDetails: {e.Message}", null);
        }
        catch (NotSupportedException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (IOException e)
        {
            return Result.Failure($"An error occurred while reading the file {FileConstants.JSONL_FILE_PATH}.\nDetails: {e.Message}", null);
        }
        catch (JsonException e)
        {
            return Result.Failure($"An error ocurred trying to deserialize a line.\nDetails: {e.Message}", null);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, null);
        }

        return Result.Success($"Last ID retrieved: {lastId}.", lastId);
    }
}