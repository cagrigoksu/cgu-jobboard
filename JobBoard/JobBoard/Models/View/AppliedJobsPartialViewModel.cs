using JobBoard.Enums;
using JobBoard.Models.Classes;

namespace JobBoard.Models.View
{
    public class AppliedJobsPartialViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatusEnum? FilterStatus { get; set; }
        public ApplicationStatusEnum Status { get; set; }
        public IQueryable<AppliedJobsListModel> AppliedJobList { get; set; }
    }
}
