JSON.CreateFile();
Task task = new Task { Description = "Tarea de prueba" };
Task task2 = new Task { Description = "Tarea de prueba" };

task2.UpdateStatus(TaskStatus.in_progress);

JSON.AddTask(task);
JSON.AddTask(task2);

foreach (var item in JSON.GetTasks())
{
    Console.WriteLine(item);
}