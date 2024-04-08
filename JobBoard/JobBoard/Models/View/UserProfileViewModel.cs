namespace JobBoard.Models.View
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }
        public String Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile CV { get; set; }
        public IFormFile MotivationLetter { get; set; }
    }
}
