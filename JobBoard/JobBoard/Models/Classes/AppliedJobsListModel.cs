using JobBoard.Enums;

namespace JobBoard.Models.Classes
{
    public class AppliedJobsListModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public int SectorId { get; set; }
        public int LevelId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatusEnum Status { get; set; }
    }
}
