using JobBoard.Models.Classes;

namespace JobBoard.Models.View
{
    public class JobApplicantsViewModel
    {
        public int JobId { get; set; }
        public IQueryable<JobApplicantsListModel> Applicants { get; set; }
    }
}
