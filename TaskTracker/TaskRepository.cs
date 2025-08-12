using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaskTracker.Models;

namespace TaskTracker
{
    public class TaskRepository
    {
        private readonly string _path;

        public TaskRepository(string path = "tasks.json")
        {
            _path = path;
        }
        public void SaveToFile(IReadOnlyList<TaskItem> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public List<TaskItem> LoadFromFile()
        {
            if (!File.Exists(_path))
                return new List<TaskItem>();

            try
            {
                var json = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
            catch (JsonException)
            {
                Console.WriteLine("Файл задач повреждён. Загружаю пустой список.");
                return new List<TaskItem>();
            }
        }
    }
}