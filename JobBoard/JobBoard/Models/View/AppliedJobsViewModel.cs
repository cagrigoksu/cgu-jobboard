using JobBoard.Models.Classes;

namespace JobBoard.Models.View
{
    public class AppliedJobsViewModel
    {
        public IQueryable<AppliedJobsListModel> AppliedJobList { get; set; }


    }
}
