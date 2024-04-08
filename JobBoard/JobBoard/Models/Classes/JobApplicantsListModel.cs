using JobBoard.Enums;

namespace JobBoard.Models.Classes
{
    public class JobApplicantsListModel
    {
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ResumeURL { get; set; }
        public string MotivationLetterURL { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatusEnum Status { get; set; }
    }
}
