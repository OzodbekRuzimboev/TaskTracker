using System;

namespace TaskTracker.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem() { }

        public TaskItem(string description, DateTime dueDate, Priority priority, bool isCompleted = false)
        {
            Id = Guid.NewGuid();
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            IsCompleted = isCompleted;
        }

        public override string ToString()
        {
            return $"{Description}\n   Дата: {DueDate:dd.MM.yyyy}\n   Важность: {Priority}\n" +
                $"   Состояние: {(IsCompleted ? "выполнено" : "не выполнено")}";
        }
    }
}