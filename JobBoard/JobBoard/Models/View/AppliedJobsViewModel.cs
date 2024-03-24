using JobBoard.Models.Classes;
using JobBoard.Models.Data;

namespace JobBoard.Models.View
{
    public class AppliedJobsViewModel
    {
        public IQueryable<AppliedJobsListModel> AppliedJobList { get; set; }


    }
}
