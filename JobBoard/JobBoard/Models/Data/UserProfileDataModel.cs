using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Data{
    public class UserProfileDataModel{
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname {get;set;} = null!;
        public DateTime LastEditDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

    }
}
