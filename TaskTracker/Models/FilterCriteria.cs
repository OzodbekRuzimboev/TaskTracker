namespace TaskTracker.Models
{
    public class FilterCriteria
    {
        public bool FilterByPriority { get; set; }
        public Priority Priority { get; set; }

        public bool OnlyIncomplete { get; set; }

        public bool SortByPriority { get; set; }

        public bool SortByDate { get; set; }
    }
}