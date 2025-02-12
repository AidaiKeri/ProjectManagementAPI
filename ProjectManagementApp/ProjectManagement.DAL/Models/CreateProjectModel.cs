using ProjectManagement.DAL.Enum;

namespace ProjectManagement.DAL.Models
{
    public class CreateProjectModel
    {
        public string Name { get; set; }
        public string CustomerCompany { get; set; }
        public string ContractorCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Priority Priority { get; set; }
        public Guid? ProjectManagerId { get; set; }
    }
}
