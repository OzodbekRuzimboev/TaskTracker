using System;
using System.Globalization;
using System.Collections.Generic;

using TaskTracker.Models;

namespace TaskTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskRepository taskRepository = new TaskRepository();
            List<TaskItem> tasks = taskRepository.LoadFromFile("tasks.json");

            TaskManager taskManager = new TaskManager(tasks, taskRepository);

            Console.WriteLine("1. Добавить задачу\n2. Показать список задач\n3. Пометить задачу выполненной\n4. Удалить задачу\n" +
                "5. Очистить список задач.\n6. Выйти");

            while (true)
            {
                Console.WriteLine();
                Console.Write("Выберите команду: ");
                string input = Console.ReadLine();

                if (input == "1")
                    AddTask(taskManager, taskRepository);

                else if (input == "2")
                    ShowTasks(taskManager);

                else if (input == "3")
                    CompleteTask(taskManager, taskRepository);

                else if (input == "4")
                    DeleteTask(taskManager, taskRepository);

                else if (input == "5")
                    DeleteAllTasks(taskManager, taskRepository);

                else
                    break;
            }
        }

        public static void AddTask(TaskManager taskManager, TaskRepository taskRepository)
        {
            Console.WriteLine();

            Console.Write("Введите задачу: ");
            string description = Console.ReadLine();

            DateTime dueTime;
            while (true)
            {
                Console.Write("Введите дату выполнения (dd.MM.yyyy): ");
                string inputDateTime = Console.ReadLine();

                if (DateTime.TryParseExact(inputDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueTime))
                    break;

                else
                    Console.WriteLine("Ошибка: введите дату в формате dd.MM.yyyy (например: 04.08.2025)");
            }

            Priority priority;
            while (true)
            {
                Console.Write("Введите важность задачи (low, medium, high): ");
                string inputPriority = Console.ReadLine();

                if (inputPriority == "low")
                {
                    priority = Priority.Low;
                    break;
                }

                else if (inputPriority == "medium")
                {
                    priority = Priority.Medium;
                    break;
                }

                else if (inputPriority == "high")
                {
                    priority = Priority.High;
                    break;
                }
            }

            TaskItem task = new TaskItem(description, dueTime, priority);

            taskManager.AddTask(task);
        }

        public static void ShowTasks(TaskManager taskManager)
        {
            Console.WriteLine();
            taskManager.GetTasks();
        }

        public static void CompleteTask(TaskManager taskManager, TaskRepository taskRepository)
        {
            Console.WriteLine();
            Console.Write("Введите индекс задачи, которая выполнена: ");

            if (int.TryParse(Console.ReadLine(), out int index))
                taskManager.MarkTaskCompleted(index - 1);

            else
                Console.WriteLine("Ошибка: введите корректное число.");
        }

        public static void DeleteTask(TaskManager taskManager, TaskRepository taskRepository)
        {
            Console.WriteLine();
            Console.Write("Введите индекс задачи, которую хотите удалить: ");

            if (int.TryParse(Console.ReadLine(), out int index))
                taskManager.RemoveTask(index - 1);

            else
                Console.WriteLine("Ошибка: введите корректное число.");
        }

        public static void DeleteAllTasks(TaskManager taskManager, TaskRepository taskRepository)
        {
            Console.WriteLine();
            taskManager.RemoveAllTasks();
        }
    }
}