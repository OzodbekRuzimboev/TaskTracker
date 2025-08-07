using System.Collections.Generic;
using System.Text.Json;
using System.IO;

using TaskTracker.Models;

namespace TaskTracker
{
    public class TaskRepository
    {
        public void SaveToFile(List<TaskItem> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("tasks.json", json);
        }

        public List<TaskItem> LoadFromFile(string file)
        {
            if (!File.Exists(file))
                return new List<TaskItem>();

            string json = File.ReadAllText(file);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
}