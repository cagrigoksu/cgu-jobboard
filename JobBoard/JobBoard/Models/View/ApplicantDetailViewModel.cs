using JobBoard.Enums;

namespace JobBoard.Models.View
{
    public class ApplicantDetailViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }

        public int ApplicantId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Name { get; set; }
        public String Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UrlResume { get; set; }
        public string UrlMotivationLetter { get; set; }
        public ApplicationStatusEnum ApplicationStatus { get; set; }
        public int ApplicationStatusInt { get; set; }
    }
}
