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

        public void RemoveTask(int i)
        {
            if (i >= 0 && i < tasks.Count)
                tasks.RemoveAt(i);

            else
                Console.WriteLine("Не существует задачи с таким индексом.");

            taskRepository.SaveToFile(tasks);
        }        

        public void RemoveAllTasks()
        {
            Console.Write("Вы уверены? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                tasks.Clear();
                Console.WriteLine("Все задачи успешно удалены.");

                taskRepository.SaveToFile(tasks);
            }
        }

        public void MarkTaskCompleted(int i)
        {
            if (i >= 0 && i < tasks.Count)
                tasks[i].IsCompleted = true;
            
            else
                Console.WriteLine("Не существует задачи с таким индексом.");

            taskRepository.SaveToFile(tasks);
        }

        public void GetTasks()
        {


            if (tasks.Count > 0)
            {
                tasks.Sort((a, b) =>
                {
                    int dateComparison = a.DueDate.CompareTo(b.DueDate);

                    if (dateComparison != 0)
                        return dateComparison;

                    return a.Priority.CompareTo(b.Priority);
                });

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