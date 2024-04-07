using JobBoard.Models.Data;

namespace JobBoard.Models.View
{
    public class IndexViewModel
    {
        public int UserId { get; set; }
        public bool CompanyUser { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobCity { get; set; }
        public string JobCountry { get; set; }
        public int JobLevelId { get; set; }
        public DateTime JobPostDate { get; set; }
        public bool IsApplied { get; set; }

        public IQueryable<JobPostDataModel> JobPosts { get; set; }
    }
}
