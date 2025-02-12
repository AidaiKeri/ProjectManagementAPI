using ProjectManagement.DAL.Enum;

namespace ProjectManagement.DAL.Models
{
    public class ProjectFilterOptions
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Priority? Priority { get; set; }
        public string ProjectName { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}
