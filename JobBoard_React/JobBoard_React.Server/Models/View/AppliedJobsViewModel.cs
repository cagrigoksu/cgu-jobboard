using JobBoard_React.Server.Models.Classes;

namespace JobBoard_React.Server.Models.View
{
    public class AppliedJobsViewModel
    {
        public IQueryable<AppliedJobsListModel> AppliedJobList { get; set; }


    }
}
