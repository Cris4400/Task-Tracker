JSON.CreateFile();
Task task = new Task { Description = "Tarea de prueba" };
Task task2 = new Task { Description = "Tarea de prueba" };
Task task3 = new Task { Description = "Tarea de prueba" };

JSON.AddTask(task);
JSON.AddTask(task2);
JSON.AddTask(task3);

foreach (var item in JSON.GetTasks(null))
{
    Console.WriteLine(item);
}

JSON.UpdateTask(1, "description", "Tarea de prueba actualizada");
JSON.UpdateTask(3, "status", TaskStatus.in_progress.ToString());
JSON.DeleteTask(2);

foreach (var item in JSON.GetTasks(null))
{
    Console.WriteLine(item);
}