using System.Collections.Generic;

using TaskTracker.Models;

namespace TaskTracker.Services
{
    public class FilterService
    {
        public List<TaskItem> ApplyFilters(List<TaskItem> tasks, FilterCriteria criteria)
        {
            var result = new List<TaskItem>();

            foreach (var task in tasks)
            {
                if (criteria.FilterByPriority && task.Priority != criteria.Priority)
                    continue;

                if (criteria.OnlyIncomplete && task.IsCompleted)
                    continue;

                result.Add(task);
            }

            if (criteria.SortByPriority && criteria.SortByDate)
            {
                result.Sort((a, b) =>
                {
                    int byPriority = a.Priority.CompareTo(b.Priority);
                    if (byPriority != 0) return byPriority;

                    return a.DueDate.CompareTo(b.DueDate);
                });
            }

            else if (criteria.SortByPriority)
            {
                result.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }

            else if (criteria.SortByDate)
            {
                result.Sort((a, b) => a.DueDate.CompareTo(b.DueDate));
            }
            
            return result;
        }
    }
}