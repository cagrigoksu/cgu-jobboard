using System.ComponentModel.DataAnnotations;
using JobBoard.Enums;
using JobBoard.Models.Data;

namespace JobBoard.Models.View
{
    public class JobApplyViewModel
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public int SectorId { get; set; }
        public int LevelId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime PostDate { get; set; }

        public IFormFile CV { get; set; }
    }
}
