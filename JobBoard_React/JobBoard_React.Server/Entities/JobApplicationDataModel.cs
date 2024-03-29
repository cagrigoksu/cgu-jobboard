using System.ComponentModel.DataAnnotations;
using JobBoard_React.Server.Enums;

namespace JobBoard_React.Server.Entities
{
    public class JobApplicationDataModel
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public int JobId { get; set; }
        [Required]
        public int ApplicantId { get; set; }
        public string UrlResume { get; set; }
        public string UrlMotivationLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatusEnum Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
