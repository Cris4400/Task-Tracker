using System.Text.Json;

/// <summary>
/// Class <c>TaskManager</c> provides methods to manage tasks in the Task-Traker application, including adding, retrieving, updating, and deleting tasks from a JSONL file.
/// </summary>
public static class TaskManager
{
    /// <summary>
    /// Adds a new task to the JSONL file.
    /// </summary>
    /// <param name="task">The task to be added.</param>
    /// <remarks>
    /// The method serializes the task object to JSON format and appends it to the JSONL file.
    /// If the file does not exist, it will be created.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Thrown when the file is not accessible.</exception>
    /// <exception cref="NotSupportedException">Thrown when the file format is not supported.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while writing to the file.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while serializing the task object.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    /// <returns>A Result object indicating the success or failure of the operation.</returns>
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

    /// <summary>
    /// Retrieves tasks from the JSONL file, optionally filtering by status.
    /// </summary> 
    /// <param name="taskStatus">The status to filter tasks by (optional).</param>
    /// <returns>A Result object containing the retrieved tasks or an error message.</returns>
    /// remarks>
    /// The method reads each line of the JSONL file, deserializes it into a TaskModel object, and adds it to a list if it matches the specified status.
    /// If no status is specified, all tasks are retrieved.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Thrown when the file is not accessible.</exception>
    /// <exception cref="NotSupportedException">Thrown when the file format is not supported.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while reading the file.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing a line.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    /// <returns>A Result object containing the retrieved tasks or an error message.</returns>
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

    /// <summary>
    /// Updates a task in the JSONL file based on the specified property and value.
    /// </summary>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <param name="property">The property of the task to be updated (e.g., "Description", "Status").</param>
    /// <param name="value">The new value for the specified property.</param>
    /// <remarks>
    /// The method reads each line of the JSONL file, deserializes it into a TaskModel object, and updates the specified property if it matches the task ID.
    /// /// If the property is "Description", it must be a string; if it's "Status", it must be of type TaskStatus.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Thrown when the file is not accessible.</exception>
    /// <exception cref="NotSupportedException">Thrown when the file format is not supported.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while reading or writing to the file.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing a line.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    /// <returns>A Result object indicating the success or failure of the operation.</returns>
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

    /// <summary>
    /// Retrieves the last ID from the JSONL file.
    /// </summary>
    /// <remarks>
    /// The method reads the last line of the JSONL file, deserializes it into a TaskModel object, and returns its ID incremented by 1.
    /// If the file does not exist or is empty, it returns 1 as the last ID.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Thrown when the file is not accessible.</exception>
    /// <exception cref="NotSupportedException">Thrown when the file format is not supported.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while reading the file.</exception>
    /// <exception cref="JsonException">Thrown when an error occurs while deserializing a line.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    /// <returns>A Result object containing the last ID or an error message.</returns>
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