using System.ComponentModel.DataAnnotations;
using JobBoard.Models.Data;

namespace JobBoard.Models.View
{
    public class JobPostViewModel
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

        // View-based

        public bool Edit { get; set; }
        public bool Detail { get; set; }
    }
}
