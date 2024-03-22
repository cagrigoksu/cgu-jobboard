using JobBoard.Models.Data;

namespace JobBoard.Models
{
    public class IndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public int SectorId { get; set; }
        public int LevelId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime PostDate { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

        public IQueryable<IndexViewModel> JobPosts { get; set; }
    }
}
