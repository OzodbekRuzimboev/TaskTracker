using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

using TaskTracker.Models;
using TaskTracker.Services;

namespace TaskTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskRepository taskRepository = new TaskRepository();
            List<TaskItem> tasks = taskRepository.LoadFromFile();

            TaskManager taskManager = new TaskManager(tasks, taskRepository);

            FilterCriteria filterCriteria = new FilterCriteria();
            FilterService filterService = new FilterService();

            Console.WriteLine("1. Добавить задачу\n2. Показать список задач\n3. Пометить задачу выполненной\n4. Удалить задачу\n" +
                "5. Очистить список задач\n6. Настройки отображения задач\n7. Выйти");

            while (true)
            {
                Console.WriteLine();
                Console.Write("Выберите команду: ");
                string input = Console.ReadLine();

                if (input == "1")
                    AddTask(taskManager);

                else if (input == "2")
                    ShowTasks(taskManager, filterCriteria, filterService);

                else if (input == "3")
                    CompleteTask(taskManager, filterCriteria, filterService);

                else if (input == "4")
                    DeleteTask(taskManager, filterCriteria, filterService);

                else if (input == "5")
                    DeleteAllTasks(taskManager);

                else if (input == "6")
                    OpenFilterSettings(filterCriteria);

                else if (input == "7")
                    break;

                else
                    Console.WriteLine("Введите корректное число");
            }
        }

        public static void AddTask(TaskManager taskManager)
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

        public static void ShowTasks(TaskManager taskManager, FilterCriteria filterCriteria, FilterService filterService)
        {
            Console.WriteLine();

            var allTasks = taskManager.GetAllTasks();
            var filtered = filterService.ApplyFilters(allTasks.ToList(), filterCriteria);

            PrintTasks(filtered, filterCriteria);
        }

        private static void PrintTasks(IEnumerable<TaskItem> tasks, FilterCriteria filterCriteria)
        {
            if (!tasks.Any())
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int index = 1;
            foreach (var task in tasks)
            {
                Console.WriteLine($"{index}. {task}");
                Console.WriteLine();
                index++;
            }

            Console.WriteLine();
            Console.WriteLine($"Действующие настройки отображения:\n" +
                $"Фильтр по приоритету: {(filterCriteria.FilterByPriority ? filterCriteria.Priority.ToString() : "off")}\n" +
                $"Только невыполненные: {(filterCriteria.OnlyIncomplete ? "on" : "off")}\n" +
                $"Сортировка по важности: {(filterCriteria.SortByPriority ? "on" : "off")}\n" +
                $"Сортировка по дате: {(filterCriteria.SortByDate ? "on" : "off")}");
        }

        public static void CompleteTask(TaskManager taskManager, FilterCriteria filterCriteria, FilterService filterService)
        {
            var allTasks = taskManager.GetAllTasks();
            var filtered = filterService.ApplyFilters(allTasks.ToList(), filterCriteria);

            if (filtered.Count == 0)
            {
                Console.WriteLine("Нет задач для выбора.");
                return;
            }

            Console.Write("Введите номер задачи, которую хотите пометить выполненной: ");
            if (!int.TryParse(Console.ReadLine(), out int number) || number < 1 || number > filtered.Count)
            {
                Console.WriteLine("Неверный номер.");
                return;
            }

            var id = filtered[number - 1].Id;
            if (!taskManager.MarkTaskCompleted(id))
                Console.WriteLine("Задача не найдена.");
        }

        public static void DeleteTask(TaskManager taskManager, FilterCriteria filterCriteria, FilterService filterService)
        {
            var allTasks = taskManager.GetAllTasks();
            var filtered = filterService.ApplyFilters(allTasks.ToList(), filterCriteria);

            if (filtered.Count == 0)
            {
                Console.WriteLine("Нет задач для удаления.");
                return;
            }

            Console.Write("Введите номер задачи, которую хотите удалить: ");
            if (!int.TryParse(Console.ReadLine(), out int number) || number < 1 || number > filtered.Count)
            {
                Console.WriteLine("Неверный номер.");
                return;
            }

            var id = filtered[number - 1].Id;
            if (!taskManager.RemoveTask(id))
                Console.WriteLine("Задача не найдена.");
        }

        public static void DeleteAllTasks(TaskManager taskManager)
        {
            Console.WriteLine();
            Console.Write("Вы уверены? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                taskManager.RemoveAllTasks();
                Console.WriteLine("Все задачи успешно удалены.");
            }
        }

        public static void OpenFilterSettings(FilterCriteria filterCriteria) 
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Установить фильтруемый приоритет\n2. Отключить фильтр по приоритету\n" +
                    "3. Включить/отключить фильтр \"только невыполненные\"\n4. Включить/отключить сортировку по приоритету\n" +
                    "5. Включить/отключить сортировку по дате\n6. Отключить все фильтры\n7. Назад");

                Console.WriteLine();
                Console.Write("Выберите настройку: ");
                string input = Console.ReadLine();

                if (input == "1")
                    ChoosePriority(filterCriteria);

                else if (input == "2")
                    filterCriteria.FilterByPriority = false;

                else if (input == "3")
                {
                    if (filterCriteria.OnlyIncomplete)
                        filterCriteria.OnlyIncomplete = false;

                    else
                        filterCriteria.OnlyIncomplete = true;
                }

                else if (input == "4")
                {
                    if (filterCriteria.SortByPriority)
                        filterCriteria.SortByPriority = false;

                    else
                        filterCriteria.SortByPriority = true;
                }

                else if (input == "5")
                {
                    if (filterCriteria.SortByDate)
                        filterCriteria.SortByDate = false;

                    else
                        filterCriteria.SortByDate = true;
                }

                else if (input == "6")
                {
                    filterCriteria.FilterByPriority = false;
                    filterCriteria.OnlyIncomplete = false;
                    filterCriteria.SortByPriority = false;
                    filterCriteria.SortByDate = false;
                }

                else if (input == "7")
                    break;

                else
                    Console.WriteLine("Введите корректное число.");
            }
        }

        public static void ChoosePriority(FilterCriteria filterCriteria)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Low\n2. Medium\n3. High");
                Console.WriteLine("Чтобы вернуться назад, нажмите '4'");
                Console.Write("Выберите приоритет, по которому хотите отфильтровать задачи: ");

                string inputPriority = Console.ReadLine();

                if (inputPriority == "1")
                {
                    filterCriteria.Priority = Priority.Low;
                    filterCriteria.FilterByPriority = true;
                    break;
                }

                else if (inputPriority == "2")
                {
                    filterCriteria.Priority = Priority.Medium;
                    filterCriteria.FilterByPriority = true;
                    break;
                }

                else if (inputPriority == "3")
                {
                    filterCriteria.Priority = Priority.High;
                    filterCriteria.FilterByPriority = true;
                    break;
                }

                else if (inputPriority == "4")
                    break;

                else
                    Console.WriteLine("Введите корректное число.");
            }
            
        }
    }
}