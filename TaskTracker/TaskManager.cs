using System;
using System.Collections.Generic;

using TaskTracker.Models;

namespace TaskTracker
{
    public class TaskManager
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(TaskItem task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(int i)
        {
            if (i >= 0 && i < tasks.Count)
                tasks.RemoveAt(i);

            else
                Console.WriteLine("Не существует задачи с таким индексом.");
        }        

        public void MarkTaskCompleted(int i)
        {
            if (i >= 0 && i < tasks.Count)
                tasks[i].IsCompleted = true;
            
            else
                Console.WriteLine("Не существует задачи с таким индексом.");
        }

        public void GetTasks()
        {
            if (tasks.Count > 0)
            {
                Console.WriteLine("Все задачи:");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.Write(i + 1 + ". ");
                    Console.WriteLine(tasks[i]);
                    Console.WriteLine();
                }
            }

            else
                Console.WriteLine("Список задач пуст.");
            
        }
    }
}
