using System.ComponentModel.DataAnnotations;

namespace JobBoard_React.Server.Entities
{
    public class UserDataModel
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool CompanyUser { get; set; }
        public DateTime LogOnDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
    }
}
