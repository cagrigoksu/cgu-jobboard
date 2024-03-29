using System.ComponentModel.DataAnnotations;

namespace JobBoard_React.Server.Entities
{
    public class UserProfileDataModel{
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname {get;set;} = null!;
        public DateTime LastEditDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

    }
}
