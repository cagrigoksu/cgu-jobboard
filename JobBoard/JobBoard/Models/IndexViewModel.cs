using JobBoard.Models.Data;

namespace JobBoard.Models
{
    public class IndexViewModel
    {
        public int UserId { get; set; }
        public bool CompanyUser { get; set; } 
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsApplied { get; set; }

        public IQueryable<IndexViewModel> JobPosts { get; set; }
    }
}
