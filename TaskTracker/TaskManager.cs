using System;
using System.Collections.Generic;

using TaskTracker.Models;

namespace TaskTracker
{
    public class TaskManager
    {
        private List<TaskItem> tasks;
        private TaskRepository taskRepository;

        public TaskManager(List<TaskItem> list, TaskRepository taskRepository)
        {
            tasks = list ?? new List<TaskItem>();
            this.taskRepository = taskRepository;
        }

        public void AddTask(TaskItem task)
        {
            tasks.Add(task);
            taskRepository.SaveToFile(tasks);
        }

        public bool RemoveTask(Guid id)
        {
            var index = tasks.FindIndex(t => t.Id == id);
            if (index < 0) return false;

            tasks.RemoveAt(index);
            taskRepository.SaveToFile(tasks);
            return true;
        }

        public bool MarkTaskCompleted(Guid id)
        {
            var task = tasks.Find(t => t.Id == id);
            if (task == null) return false;

            task.IsCompleted = true;
            taskRepository.SaveToFile(tasks);
            return true;
        }    

        public void RemoveAllTasks()
        {
            tasks.Clear();
            taskRepository.SaveToFile(tasks);
        }

        public IReadOnlyList<TaskItem> GetAllTasks()
        {
            return tasks.AsReadOnly();
        }
    }
}