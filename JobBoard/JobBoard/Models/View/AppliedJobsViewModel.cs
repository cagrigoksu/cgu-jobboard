using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Models.View
{
    public class AppliedJobsViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public DateTime ApplicationDate { get; set; }
        
        public IQueryable<AppliedJobsListModel> AppliedJobList { get; set; }
    }
}
